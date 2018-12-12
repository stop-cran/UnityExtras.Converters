using System.Configuration;

namespace UnityExtras.Converters.Tests
{
    public class TestConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty(nameof(TestString), IsRequired = true)]
        public string TestString
        {
            get { return (string)this[nameof(TestString)]; }
            set { this[nameof(TestString)] = value; }
        }


        [ConfigurationProperty(nameof(TestInt), DefaultValue = 1234)]
        public int TestInt
        {
            get { return (int)this[nameof(TestInt)]; }
            set { this[nameof(TestInt)] = value; }
        }
    }
}
