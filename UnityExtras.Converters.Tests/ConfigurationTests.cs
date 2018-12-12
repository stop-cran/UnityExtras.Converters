using NUnit.Framework;
using Shouldly;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
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
            new UnityContainer()
                .RegisterType<ConstructorMock<TestConfigurationSection>>(
                    new InjectionConstructor(
                        new ConfigurationSectionParameter<TestConfigurationSection>("someTestSection")
                        ))
                .Resolve<ConstructorMock<TestConfigurationSection>>()
                .Value
                .ShouldNotBeNull();

        [Test]
        public void ShouldRetrieveIntValueFromCustomSection() =>
            new UnityContainer()
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
            new UnityContainer()
                .RegisterType<ConstructorMock<string>>(
                    new InjectionConstructor(
                        new ConfigurationSectionParameter<TestConfigurationSection>("someTestSection")
                        .Convert(section => section.TestString)
                        ))
                .Resolve<ConstructorMock<string>>()
                .Value
                .ShouldBe("d2r_6Hdr");

        [Test]
        public void ShouldResolveAppSettingsSection() =>
            new UnityContainer()
                .RegisterType<ConstructorMock<NameValueCollection>>(
                    new InjectionConstructor(
                        new AppSettingsSectionParameter()
                        ))
                .Resolve<ConstructorMock<NameValueCollection>>()
                .Value
                .ShouldNotBeNull();

        [Test]
        public void ShouldRetrieveAppSetting() =>
            new UnityContainer()
                .RegisterType<ConstructorMock<string>>(
                    new InjectionConstructor(
                        new AppSettingsSectionParameter()["testKey"]
                        ))
                .Resolve<ConstructorMock<string>>()
                .Value
                .ShouldBe("testValue");

        [Test]
        public void ShouldConvertAppSetting() =>
            new UnityContainer()
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
            new UnityContainer()
                .RegisterInstance(9)
                .RegisterType<ConstructorMock<int>>(
                    new InjectionConstructor(
                        new ResolvedParameter<int>().Convert(i => i + 4)))
                .Resolve<ConstructorMock<int>>()
                .Value
                .ShouldBe(13);

        [Test]
        public void ShouldConvertResolvedParameter2() =>
            new UnityContainer()
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
        public void ShouldConvertResolvedArrayParameter() =>
            new UnityContainer()
                .RegisterInstance("a", 9)
                .RegisterInstance("b", 15)
                .RegisterInstance("c", 33)
                .RegisterType<ConstructorMock<int>>(
                    new InjectionConstructor(
                        new ResolvedArrayParameter<int>(
                            new ResolvedParameter<int>("a"),
                            new ResolvedParameter<int>("b")
                            ).Convert(ii => ii.Sum() / 3)
                        ))
                .Resolve<ConstructorMock<int>>()
                .Value
                .ShouldBe(8);

        [Test]
        public void ShouldResolveConnectionStringsSection() =>
            new UnityContainer()
                .RegisterType<ConstructorMock<ConnectionStringSettingsCollection>>(
                    new InjectionConstructor(
                        new ConnectionStringsSectionParameter()
                        ))
                .Resolve<ConstructorMock<ConnectionStringSettingsCollection>>()
                .Value
                .ShouldNotBeNull();

        [Test]
        public void ShouldRetrieveConnectionString() =>
            new UnityContainer()
                .RegisterType<ConstructorMock<string>>(
                    new InjectionConstructor(
                        new ConnectionStringsSectionParameter()["testConnection"]
                        ))
                .Resolve<ConstructorMock<string>>()
                .Value
                .ShouldBe("data source=testServer; initial catalog=testDb");

        [Test]
        public void ShouldConvertConnectionString() =>
           new UnityContainer()
               .RegisterType<ConstructorMock<string>>(
                   new InjectionConstructor(
                       new ConnectionStringsSectionParameter()
                           .Convert(connectionStrings => connectionStrings["testConnection"].ProviderName)
                       ))
               .Resolve<ConstructorMock<string>>()
               .Value
               .ShouldBe("testProvider");
    }
}
