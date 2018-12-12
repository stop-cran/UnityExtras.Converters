using System;
using System.Configuration;
using System.Reflection;
using Unity.Builder;
using Unity.Injection;
using Unity.Policy;

namespace Unity.Extras
{
    public class ConfigurationSectionParameter<TSection> : InjectionParameterValue
        where TSection : ConfigurationSection
    {
        private readonly IResolverPolicy policy;

        public ConfigurationSectionParameter(string section)
        {
            policy = new ConfigurationSectionResolverPolicy(section);
        }

        public override string ParameterTypeName => typeof(TSection).GetTypeInfo().Name;

        public override IResolverPolicy GetResolverPolicy(Type typeToBuild) => policy;

        public override bool MatchesType(Type t) => t == typeof(TSection);

        private sealed class ConfigurationSectionResolverPolicy : IResolverPolicy
        {
            private readonly string section;

            public ConfigurationSectionResolverPolicy(string section)
            {
                this.section = section;
            }

            public object Resolve(IBuilderContext context) =>
                (TSection)ConfigurationManager.GetSection(section);
        }
    }
}
