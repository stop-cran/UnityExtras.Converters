using System;
using System.Configuration;
using System.Reflection;
using Unity.Builder;
using Unity.Injection;
using Unity.Policy;

namespace Unity.Extras
{
    public class ConnectionStringsSectionParameter : InjectionParameterValue
    {
        private readonly IResolverPolicy policy;

        public ConnectionStringsSectionParameter()
        {
            policy = new ConnectionStringsSectionResolverPolicy();
        }

        public InjectionParameterValue this[string name] =>
            this.Convert(settings => settings[name].ConnectionString);

        public override string ParameterTypeName =>
            typeof(ConnectionStringSettingsCollection).GetTypeInfo().Name;

        public override IResolverPolicy GetResolverPolicy(Type typeToBuild) => policy;

        public override bool MatchesType(Type t) =>
            t == typeof(ConnectionStringSettingsCollection);

        private sealed class ConnectionStringsSectionResolverPolicy : IResolverPolicy
        {
            public object Resolve(IBuilderContext context) =>
                ConfigurationManager.ConnectionStrings;
        }
    }
}
