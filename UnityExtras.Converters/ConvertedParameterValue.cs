using System;
using System.Reflection;
using Unity.Injection;
using Unity.Resolution;

namespace Unity.Extras
{
    public sealed class ConvertedParameterValue<TParameter, TFrom, TTo> : ParameterBase,
        IResolverFactory<Type>, IResolverFactory<ParameterInfo>
        where TParameter : IResolverFactory<Type>, IResolverFactory<ParameterInfo>
    {
        private readonly TParameter inner;
        private readonly Func<TFrom, TTo> converter;

        public ConvertedParameterValue(TParameter inner, Func<TFrom, TTo> converter)
        {
            this.inner = inner;
            this.converter = converter;
        }

        public ResolveDelegate<TContext> GetResolver<TContext>(Type info) where TContext : IResolveContext =>
            (ref TContext context) => converter((TFrom)inner.GetResolver<TContext>(info)(ref context));

        public ResolveDelegate<TContext> GetResolver<TContext>(ParameterInfo info) where TContext : IResolveContext =>
            (ref TContext context) => converter((TFrom)inner.GetResolver<TContext>(info)(ref context));
    }
}
