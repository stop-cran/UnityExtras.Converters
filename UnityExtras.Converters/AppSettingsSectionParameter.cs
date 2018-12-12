using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using Unity.Builder;
using Unity.Injection;
using Unity.Policy;

namespace Unity.Extras
{
    public class AppSettingsSectionParameter : InjectionParameterValue
    {
        private readonly IResolverPolicy policy;

        public AppSettingsSectionParameter()
        {
            policy = new AppSettingsSectionResolverPolicy();
        }

        public InjectionParameterValue this[string name] => this.Convert(settings => settings[name]);

        public override string ParameterTypeName => typeof(NameValueCollection).GetTypeInfo().Name;

        public override IResolverPolicy GetResolverPolicy(Type typeToBuild) => policy;

        public override bool MatchesType(Type t) => t == typeof(NameValueCollection);

        private sealed class AppSettingsSectionResolverPolicy : IResolverPolicy
        {
            public object Resolve(IBuilderContext context) =>
                ConfigurationManager.AppSettings;
        }
    }
}
