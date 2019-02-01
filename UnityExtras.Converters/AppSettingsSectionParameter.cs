using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Unity.Injection;
using Unity.Resolution;

namespace Unity.Extras
{
    public class AppSettingsSectionParameter : ParameterBase, IResolverFactory<Type>, IResolverFactory<ParameterInfo>
    {
        public AppSettingsSectionParameter() : base(typeof(IReadOnlyDictionary<string, string>))
        { }

        public ConvertedParameterValue<AppSettingsSectionParameter, IReadOnlyDictionary<string, string>, string> this[string name] =>
            this.Convert(settings => settings[name]);

        public ResolveDelegate<TContext> GetResolver<TContext>(Type info) where TContext : IResolveContext =>
            Resolve;

        public ResolveDelegate<TContext> GetResolver<TContext>(ParameterInfo info) where TContext : IResolveContext =>
            Resolve;

        private IReadOnlyDictionary<string, string> Resolve<TContext>(ref TContext context) where TContext : IResolveContext
        {
            var loadedSettings = context.Container.TryResolve<Configuration>()?.AppSettings.Settings;

            if (loadedSettings != null)
                return new KeyValueConfigurationCollectionReadOnlyDictionaryAdapter(loadedSettings);
            return new NameValueCollectionReadOnlyDictionaryAdapter(ConfigurationManager.AppSettings);
        }


        private class KeyValueConfigurationCollectionReadOnlyDictionaryAdapter : IReadOnlyDictionary<string, string>
        {
            private readonly KeyValueConfigurationCollection collection;

            public KeyValueConfigurationCollectionReadOnlyDictionaryAdapter(KeyValueConfigurationCollection collection)
            {
                this.collection = collection;
            }

            public string this[string key] => collection[key].Value;

            public IEnumerable<string> Keys => collection.AllKeys;

            public IEnumerable<string> Values => collection.AllKeys.Select(key => this[key]);

            public int Count => collection.Count;

            public bool ContainsKey(string key) => collection.AllKeys.Contains(key);

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator() =>
                collection.AllKeys.Select(key => new KeyValuePair<string, string>(key, this[key])).GetEnumerator();

            public bool TryGetValue(string key, out string value)
            {
                if (ContainsKey(key))
                {
                    value = this[key];
                    return true;
                }

                value = null;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }


        private class NameValueCollectionReadOnlyDictionaryAdapter : IReadOnlyDictionary<string, string>
        {
            private readonly NameValueCollection collection;

            public NameValueCollectionReadOnlyDictionaryAdapter(NameValueCollection collection)
            {
                this.collection = collection;
            }

            public string this[string key] => collection[key];

            public IEnumerable<string> Keys => collection.AllKeys;

            public IEnumerable<string> Values => collection.AllKeys.Select(key => this[key]);

            public int Count => collection.Count;

            public bool ContainsKey(string key) => collection.AllKeys.Contains(key);

            public IEnumerator<KeyValuePair<string, string>> GetEnumerator() =>
                collection.AllKeys.Select(key => new KeyValuePair<string, string>(key, this[key])).GetEnumerator();

            public bool TryGetValue(string key, out string value)
            {
                if (ContainsKey(key))
                {
                    value = this[key];
                    return true;
                }

                value = null;
                return false;
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}
