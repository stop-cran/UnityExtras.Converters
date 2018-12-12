using System;
using System.Reflection;
using Unity.Builder;
using Unity.Injection;
using Unity.Policy;

namespace Unity.Extras
{
    internal sealed class ConvertedParameterValue<TFrom, TTo> : InjectionParameterValue
    {
        private readonly IResolverPolicy policy;

        public ConvertedParameterValue(InjectionParameterValue inner, Func<TFrom, TTo> converter)
        {
            policy = new ConvertResolverPolicy(inner.GetResolverPolicy(typeof(TFrom)), converter);
        }

        public override string ParameterTypeName => typeof(TTo).GetTypeInfo().Name;

        public override IResolverPolicy GetResolverPolicy(Type typeToBuild) => policy;

        public override bool MatchesType(Type t) => t == typeof(TTo);

        private sealed class ConvertResolverPolicy : IResolverPolicy
        {
            private readonly IResolverPolicy inner;
            private readonly Func<TFrom, TTo> converter;

            public ConvertResolverPolicy(IResolverPolicy inner, Func<TFrom, TTo> converter)
            {
                this.inner = inner;
                this.converter = converter;
            }

            public object Resolve(IBuilderContext context) =>
                converter((TFrom)inner.Resolve(context));
        }
    }
}
