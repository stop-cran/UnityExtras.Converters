# Overview [![NuGet](https://img.shields.io/nuget/v/UnityExtras.Converters.svg)](https://www.nuget.org/packages/UnityExtras.Converters) [![Build Status](https://travis-ci.com/stop-cran/UnityExtras.Converters.svg?branch=master)](https://travis-ci.com/stop-cran/UnityExtras.Converters)

This package provides an extension for [Unity Container](https://github.com/unitycontainer/unity).
It provides a bunch of `InjectionParameterValue`-inherited types and extension methods to convert resolved parameters of another type, as well as retrieve settings from ConfigurationManager.
In particular, the configuration resolve parameters retrieve the settings at the stage of resolution, rather than immediately, in their constructors - this allows more flexibility.

# Installation

NuGet package is available [here](https://www.nuget.org/packages/UnityExtras.Converters/).

```PowerShell
PM> Install-Package UnityExtras.Converters
```

# Example

If one needs to convert a resolved parameter while injecting it into another, here's an option of using `Convert` extension method:

```C#
class ClassA
{
    public ClassA(int value)
    {
        Value = value;
    }

    public int Value { get; }
}

class ClassB
{
    public ClassB(string value)
    {
        Value = value;
    }

    public string Value { get; }
}

```

Resolution example:

```C#
new UnityContainer()
    .RegisterInstance(9)
    .RegisterType<ClassA>(
        new InjectionConstructor(
            new ResolvedParameter<int>().Convert(i => i + 4)))
    .Resolve<ClassA>()
    .Value
    .ShouldBe(13);
```

A simple example of injecting app settings without reading them at the container registration stage (which can cause undesirable side-effects):

```C#
new UnityContainer()
    .RegisterType<ClassB>(
        new InjectionConstructor(
            new AppSettingsSectionParameter()["testKey"]
            ))
    .Resolve<ClassB>()
    .Value
    .ShouldBe("testValue");
```

The same for a connection string:

```C#
new UnityContainer()
    .RegisterType<ClassB>(
        new InjectionConstructor(
            new ConnectionStringsSectionParameter()["testConnection"]
            ))
    .Resolve<ClassB>()
    .Value
    .ShouldBe("data source=testServer; initial catalog=testDb");
```

Any other `ConfigurationSection`:

```C#
new UnityContainer()
    .RegisterType<ClassA>(
        new InjectionConstructor(
            new ConfigurationSectionParameter<TestConfigurationSection>("someTestSection")
                .Convert(section => section.TestInt)
            ))
    .Resolve<ClassA>()
    .Value
    .ShouldBe(4973);
```

Resolved app settings from configuration loaded from another source:

```C#
new UnityContainer()
    .RegisterInstance(ConfigurationManager.OpenExeConfiguration(
        Assembly.GetExecutingAssembly().Location))
    .RegisterType<ClassB>(
        new InjectionConstructor(
            new AppSettingsSectionParameter()["testKey"]
            ))
    .Resolve<ClassB>()
    .Value
    .ShouldBe("testValue");
```

Resolved settings from a custom section, registered in the container:
```
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
```

`app.config` file used:

```XML
<configuration>
  <configSections>
    <section name="someTestSection"
             type="UnityExtras.Converters.Tests.TestConfigurationSection, UnityExtras.Converters.Tests" />
  </configSections>

  <someTestSection TestString="d2r_6Hdr" TestInt="4973" />
  
  <appSettings>
    <add key="testKey" value="testValue" />
  </appSettings>

  <connectionStrings>
    <add name="testConnection" providerName="testProvider"
         connectionString="data source=testServer; initial catalog=testDb" />
  </connectionStrings>
</configuration>
```

See more examples at the [test project](https://github.com/stop-cran/UnityExtras.Converters/blob/master/UnityExtras.Converters.Tests/ConfigurationTests.cs).