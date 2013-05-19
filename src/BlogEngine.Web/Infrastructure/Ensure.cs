using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogEngine
{
    public class EnsureSubject<T>
    {
        public T Subject { get; private set; }

        public EnsureSubject(T subject)
        {
            Subject = subject;
        }
    }

    public class EnsurePredicate<T>
    {
        public EnsureSubject<T> Subject { get; private set; }
        public Func<bool> Predicate { get; private set; }

        public EnsurePredicate(EnsureSubject<T> subject, Func<bool> predicate)
        {
            Subject = subject;
            Predicate = predicate;
        }

        public void WithDomainError(string name, string format, params object[] args)
        {
            if (!Predicate())
            {
                throw DomainError.Named(name, format, args);
            }
        }
    }

    public static class EnsureExtensions
    {
        public static EnsurePredicate<T> IsNew<T>(this EnsureSubject<T> that) where T : IAggregateState
        {
            return new EnsurePredicate<T>(that, () => that.Subject.IsNew);
        }
        public static EnsurePredicate<T> HasBeenCreated<T>(this EnsureSubject<T> that) where T : IAggregateState
        {
            return new EnsurePredicate<T>(that, () => !that.Subject.IsNew);
        }
        public static EnsurePredicate<T> IsNotNull<T>(this EnsureSubject<T> that) where T : class
        {
            return new EnsurePredicate<T>(that, () => that.Subject != null);
        }
        public static EnsurePredicate<T> IsNull<T>(this EnsureSubject<T> that) where T : class
        {
            return new EnsurePredicate<T>(that, () => that.Subject == null);
        }
        public static EnsurePredicate<int> IsNot(this EnsureSubject<int> that, int compareToValue)
        {
            return new EnsurePredicate<int>(that, () => that.Subject != compareToValue);
        }
        public static EnsurePredicate<string> IsNotNullOrWhitespace(this EnsureSubject<string> that)
        {
            return new EnsurePredicate<string>(that, () => !string.IsNullOrWhiteSpace(that.Subject));
        }
        public static EnsurePredicate<IEnumerable<T>> DoesNotContain<T>(this EnsureSubject<IEnumerable<T>> that, T element)
        {
            return new EnsurePredicate<IEnumerable<T>>(that, () => !that.Subject.Contains(element));
        }
    }
}