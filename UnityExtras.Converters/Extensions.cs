using System;
using System.Collections.Generic;
using System.Configuration;
using Unity.Injection;

namespace Unity.Extras
{
    public static class Extensions
    {
        public static ConvertedParameterValue<ResolvedParameter<TFrom>, TFrom, TTo> Convert<TFrom, TTo>(
            this ResolvedParameter<TFrom> parameter,
            Func<TFrom, TTo> converter) =>
            new ConvertedParameterValue<ResolvedParameter<TFrom>, TFrom, TTo>(parameter, converter);

        public static ConvertedParameterValue<ResolvedArrayParameter<TFrom>, TFrom[], TTo> Convert<TFrom, TTo>(
         this ResolvedArrayParameter<TFrom> parameter,
         Func<TFrom[], TTo> converter) =>
         new ConvertedParameterValue<ResolvedArrayParameter<TFrom>, TFrom[], TTo>(parameter, converter);

        public static ConvertedParameterValue<ConfigurationSectionParameter<TFrom>, TFrom, TTo> Convert<TFrom, TTo>(
            this ConfigurationSectionParameter<TFrom> parameter,
            Func<TFrom, TTo> converter)
            where TFrom : ConfigurationSection =>
            new ConvertedParameterValue<ConfigurationSectionParameter<TFrom>, TFrom, TTo>(parameter, converter);

        public static ConvertedParameterValue<AppSettingsSectionParameter, IReadOnlyDictionary<string, string>, TTo> Convert<TTo>(
            this AppSettingsSectionParameter parameter,
            Func<IReadOnlyDictionary<string, string>, TTo> converter) =>
            new ConvertedParameterValue<AppSettingsSectionParameter, IReadOnlyDictionary<string, string>, TTo>(parameter, converter);

        public static ConvertedParameterValue<ConnectionStringsSectionParameter, IReadOnlyDictionary<string, ConnectionStringSettings>, TTo> Convert<TTo>(
            this ConnectionStringsSectionParameter parameter,
            Func<IReadOnlyDictionary<string, ConnectionStringSettings>, TTo> converter) =>
            new ConvertedParameterValue<ConnectionStringsSectionParameter, IReadOnlyDictionary<string, ConnectionStringSettings>, TTo>(parameter, converter);

        internal static T TryResolve<T>(this IUnityContainer container) where T : class =>
            container.IsRegistered<T>() ? container.Resolve<T>() : null;
    }
}
