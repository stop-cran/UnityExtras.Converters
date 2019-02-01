using System;
using System.Configuration;
using System.Reflection;
using Unity.Injection;
using Unity.Resolution;

namespace Unity.Extras
{
    public class ConfigurationSectionParameter<TSection> : ParameterBase, IResolverFactory<Type>, IResolverFactory<ParameterInfo>
        where TSection : ConfigurationSection
    {
        private readonly string section;

        public ConfigurationSectionParameter(string section) : base(typeof(TSection))
        {
            this.section = section;
        }

        public ResolveDelegate<TContext> GetResolver<TContext>(Type info) where TContext : IResolveContext =>
            Resolve;

        public ResolveDelegate<TContext> GetResolver<TContext>(ParameterInfo info) where TContext : IResolveContext =>
            Resolve;

        private TSection Resolve<TContext>(ref TContext context) where TContext : IResolveContext =>
            context.Container.TryResolve<TSection>()
                ?? (TSection)context.Container.TryResolve<Configuration>()?.GetSection(section)
                ?? (TSection)ConfigurationManager.GetSection(section);
    }
}
