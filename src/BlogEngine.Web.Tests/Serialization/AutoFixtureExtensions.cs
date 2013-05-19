using System;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace BlogEngine.Web.Tests
{
    public static class AutoFixtureExtensions
    {
        public static object CreateFromType(this ISpecimenBuilder specimenBuilder, Type typeToCreate)
        {
            var method = typeof(SpecimenFactory)
                .GetMethod("Create", new[] { typeof(ISpecimenBuilder) });
            var generic = method.MakeGenericMethod(typeToCreate);
            return generic.Invoke(null, new[] { specimenBuilder });
        }
    }
}