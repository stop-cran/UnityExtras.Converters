using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Unity.Injection;
using Unity.Resolution;

namespace Unity.Extras
{
    public class ConnectionStringsSectionParameter : ParameterBase, IResolverFactory<Type>, IResolverFactory<ParameterInfo>
    {
        public ConnectionStringsSectionParameter() : base(typeof(IReadOnlyDictionary<string, ConnectionStringSettings>))
        { }

        public ConvertedParameterValue<ConnectionStringsSectionParameter, IReadOnlyDictionary<string, ConnectionStringSettings>, string> this[string name] =>
            this.Convert(settings => settings[name].ConnectionString);

        public ResolveDelegate<TContext> GetResolver<TContext>(Type info) where TContext : IResolveContext =>
            Resolve;

        public ResolveDelegate<TContext> GetResolver<TContext>(ParameterInfo info) where TContext : IResolveContext =>
            Resolve;

        private IReadOnlyDictionary<string, ConnectionStringSettings> Resolve<TContext>(ref TContext context) where TContext : IResolveContext =>
            new ReadOnlyDictionary<string, ConnectionStringSettings>(
                (context.Container.TryResolve<ConnectionStringsSection>()?.ConnectionStrings
                ?? context.Container.TryResolve<Configuration>()?.ConnectionStrings.ConnectionStrings
                ?? ConfigurationManager.ConnectionStrings)
                .Cast<ConnectionStringSettings>()
                .ToDictionary(e => e.Name));
    }
}
