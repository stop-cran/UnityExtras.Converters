using System;
using System.Collections.Specialized;
using System.Configuration;
using Unity.Injection;

namespace Unity.Extras
{
    public static class Extensions
    {
        public static InjectionParameterValue Convert<TFrom, TTo>(
            this ResolvedParameter<TFrom> parameter,
            Func<TFrom, TTo> converter) =>
            new ConvertedParameterValue<TFrom, TTo>(parameter, converter);

        public static InjectionParameterValue Convert<TFrom, TTo>(
         this ResolvedArrayParameter<TFrom> parameter,
         Func<TFrom[], TTo> converter) =>
         new ConvertedParameterValue<TFrom[], TTo>(parameter, converter);

        public static InjectionParameterValue Convert<TFrom, TTo>(
            this ConfigurationSectionParameter<TFrom> parameter,
            Func<TFrom, TTo> converter)
            where TFrom : ConfigurationSection =>
            new ConvertedParameterValue<TFrom, TTo>(parameter, converter);

        public static InjectionParameterValue Convert<TTo>(
            this AppSettingsSectionParameter parameter,
            Func<NameValueCollection, TTo> converter) =>
            new ConvertedParameterValue<NameValueCollection, TTo>(parameter, converter);

        public static InjectionParameterValue Convert<TTo>(
            this ConnectionStringsSectionParameter parameter,
            Func<ConnectionStringSettingsCollection, TTo> converter) =>
            new ConvertedParameterValue<ConnectionStringSettingsCollection, TTo>(parameter, converter);
    }
}
