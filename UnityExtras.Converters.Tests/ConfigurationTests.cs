using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Unity;
using Unity.Extras;
using Unity.Injection;

namespace UnityExtras.Converters.Tests
{
    [TestFixture]
    public class ConfigurationTests
    {
        [Test]
        public void ShouldResolveCustomSection() =>
            CreateContainer()
                .RegisterType<ConstructorMock<TestConfigurationSection>>(
                    new InjectionConstructor(
                        new ConfigurationSectionParameter<TestConfigurationSection>("someTestSection")
                        ))
                .Resolve<ConstructorMock<TestConfigurationSection>>()
                .Value
                .ShouldNotBeNull();

        [Test]
        public void ShouldRetrieveIntValueFromCustomSection() =>
            CreateContainer()
                .RegisterType<ConstructorMock<int>>(
                    new InjectionConstructor(
                        new ConfigurationSectionParameter<TestConfigurationSection>("someTestSection")
                        .Convert(section => section.TestInt)
                        ))
                .Resolve<ConstructorMock<int>>()
                .Value
                .ShouldBe(4973);

        [Test]
        public void ShouldRetrieveStringValueFromCustomSection() =>
            CreateContainer()
                .RegisterType<ConstructorMock<string>>(
                    new InjectionConstructor(
                        new ConfigurationSectionParameter<TestConfigurationSection>("someTestSection")
                        .Convert(section => section.TestString)
                        ))
                .Resolve<ConstructorMock<string>>()
                .Value
                .ShouldBe("d2r_6Hdr");

        [Test]
        public void ShouldRetrieveStringValueFromCustomSectionRegistered() =>
            new UnityContainer()
            .RegisterInstance(new TestConfigurationSection
            {
                TestInt = 347356
            })
                .RegisterType<ConstructorMock<int>>(
                    new InjectionConstructor(
                        new ConfigurationSectionParameter<TestConfigurationSection>("someTestSection")
                        .Convert(section => section.TestInt)
                        ))
                .Resolve<ConstructorMock<int>>()
                .Value
                .ShouldBe(347356);

        [Test]
        public void ShouldResolveAppSettingsSection() =>
            CreateContainer()
                .RegisterType<ConstructorMock<IReadOnlyDictionary<string, string>>>(
                    new InjectionConstructor(
                        new AppSettingsSectionParameter()
                        ))
                .Resolve<ConstructorMock<IReadOnlyDictionary<string, string>>>()
                .Value
                .ShouldNotBeNull();

        [Test]
        public void ShouldRetrieveAppSetting() =>
            CreateContainer()
                .RegisterType<ConstructorMock<string>>(
                    new InjectionConstructor(
                        new AppSettingsSectionParameter()["testKey"]
                        ))
                .Resolve<ConstructorMock<string>>()
                .Value
                .ShouldBe("testValue");

        [Test]
        public void ShouldConvertAppSetting() =>
            CreateContainer()
                .RegisterType<ConstructorMock<int>>(
                    new InjectionConstructor(
                        new AppSettingsSectionParameter()
                            .Convert(settings => Convert.ToInt32(settings["testKey2"]))
                        ))
                .Resolve<ConstructorMock<int>>()
                .Value
                .ShouldBe(38460);

        [Test]
        public void ShouldConvertResolvedParameter() =>
            CreateContainer()
                .RegisterInstance(9)
                .RegisterType<ConstructorMock<int>>(
                    new InjectionConstructor(
                        new ResolvedParameter<int>().Convert(i => i + 4)))
                .Resolve<ConstructorMock<int>>()
                .Value
                .ShouldBe(13);

        [Test]
        public void ShouldConvertResolvedParameter2() =>
            CreateContainer()
                .RegisterInstance(9)
                .RegisterType<ConstructorMock<ConstructorMock<int>>>(
                    new InjectionConstructor(
                        new ResolvedParameter<int>().Convert(i => new ConstructorMock<int>(i + 4))
                        ))
                .Resolve<ConstructorMock<ConstructorMock<int>>>()
                .Value
                .Value
                .ShouldBe(13);

        [Test]
        public void ShouldResolveConnectionStringsSection() =>
            CreateContainer()
                .RegisterType<ConstructorMock<IReadOnlyDictionary<string, ConnectionStringSettings>>>(
                    new InjectionConstructor(
                        new ConnectionStringsSectionParameter()
                        ))
                .Resolve<ConstructorMock<IReadOnlyDictionary<string, ConnectionStringSettings>>>()
                .Value
                .ShouldNotBeNull();

        [Test]
        public void ShouldRetrieveConnectionString() =>
            CreateContainer()
                .RegisterType<ConstructorMock<string>>(
                    new InjectionConstructor(
                        new ConnectionStringsSectionParameter()["testConnection"]
                        ))
                .Resolve<ConstructorMock<string>>()
                .Value
                .ShouldBe("data source=testServer; initial catalog=testDb");

        [Test]
        public void ShouldConvertConnectionString() =>
           CreateContainer()
               .RegisterType<ConstructorMock<string>>(
                   new InjectionConstructor(
                       new ConnectionStringsSectionParameter()
                           .Convert(connectionStrings => connectionStrings["testConnection"].ProviderName)
                       ))
               .Resolve<ConstructorMock<string>>()
               .Value
               .ShouldBe("testProvider");

        public IUnityContainer CreateContainer() =>
            new UnityContainer()
                .RegisterInstance(ConfigurationManager.OpenExeConfiguration(
                    Assembly.GetExecutingAssembly().Location));
    }
}
