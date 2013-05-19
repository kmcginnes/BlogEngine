using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using KellermanSoftware.CompareNetObjects;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoNSubstitute;
using Xunit;

namespace BlogEngine.Web.Tests
{
    public class AppServiceSpecs<TAppService> 
        where TAppService : IAppService
    {
        protected IFixture Fixture;
        private Event[] _given;
        private Command _when;

        public AppServiceSpecs()
        {
            Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
        }

        protected void Given(params Event[] events)
        {
            _given = events;
        }

        protected void When(Command command)
        {
            _when = command;
        }

        protected void Expect(params Event[] events)
        {
            Expect(null, events);
        }

        protected void Expect(Command command, params Event[] events)
        {
            var eventStore = new SingleCommitMemoryStore();
            foreach (var @event in _given)
            {
                eventStore.Preload(((dynamic)@event).Id.GetId(), @event);
            }

            Fixture.Register<IEventStore>(() => eventStore);

            Event[] actual;
            try
            {
                var sut = Fixture.Create<TAppService>();
                sut.Execute(_when);
                actual = eventStore.Appended ?? new Event[0];
            }
            catch (DomainError e)
            {
                actual = new Event[] { new ExceptionThrown(e.Name) };
            }
            catch (TargetInvocationException e)
            {
                var domainError = e.InnerException as DomainError;
                if (domainError != null)
                {
                    actual = new Event[] { new ExceptionThrown(domainError.Name) };
                }
                else
                {
                    throw e.InnerException;
                }
            }

            // Check that events exist in stream?
            var results = CompareAssert(events, actual).ToArray();

            PrintResults(results);

            if (results.Any(r => r.Failure != null))
                Assert.True(false, "Specification failed");
        }

        public void ExpectError(string error)
        {
            Expect(new ExceptionThrown(error));
        }

        protected static IEnumerable<ExpectResult> CompareAssert(
            Event[] expected,
            Event[] actual)
        {
            var max = Math.Max(expected.Length, actual.Length);

            for (int i = 0; i < max; i++)
            {
                var ex = expected.Skip(i).FirstOrDefault();
                var ac = actual.Skip(i).FirstOrDefault();

                var expectedString = ex == null ? "No event expected" : ex.ToString();
                var actualString = ac == null ? "No event actually" : ac.ToString();

                var result = new ExpectResult { Expectation = expectedString };

                var compareObjects = new CompareObjects();
                if (!compareObjects.Compare(ex, ac))
                {
                    var stringRepresentationsDiffer = expectedString != actualString;

                    result.Failure = stringRepresentationsDiffer ?
                        GetAdjusted("Was:  ", actualString) :
                        GetAdjusted("Diff: ", compareObjects.DifferencesString);
                }

                yield return result;
            }
        }

        public static string GetAdjusted(string adj, string text)
        {
            var first = true;
            var builder = new StringBuilder();
            foreach (var s in text.Split(new[] { Environment.NewLine }, StringSplitOptions.None))
            {
                builder.Append(first ? adj : new string(' ', adj.Length));
                builder.AppendLine(s);
                first = false;
            }
            return builder.ToString();
        }

        protected void PrintResults(ICollection<ExpectResult> exs)
        {
            var results = exs.ToArray();
            var failures = results.Where(f => f.Failure != null).ToArray();
            if (!failures.Any())
            {
                Console.WriteLine();
                Console.WriteLine("Results: [Passed]");
                return;
            }
            Console.WriteLine();
            Console.WriteLine("Results: [Failed]");

            for (int i = 0; i < results.Length; i++)
            {
                PrintAdjusted("  " + (i + 1) + ". ", results[i].Expectation);
                PrintAdjusted("     ", results[i].Failure ?? "PASS");
            }
        }

        static void PrintAdjusted(string adj, string text)
        {
            Console.Write(GetAdjusted(adj, text));
        }

        protected class ExceptionThrown : Event, IAmFakeEventForTesting
        {

            public string Name { get; set; }

            public string FakeType { get { return Name; } }

            public ExceptionThrown(string name)
            {
                Name = name;
            }

            public override string ToString()
            {
                return string.Format("Domain error '{0}'", Name);
            }
        }
    }

    public interface IAmFakeEventForTesting
    {
        string FakeType { get; }
    }

    public class ExpectResult
    {
        public string Failure;
        public string Expectation;
    }
}
