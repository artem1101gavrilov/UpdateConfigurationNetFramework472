using System;
using System.Configuration;

namespace UpdateConfiguration
{
    public class TestConfiguration : ConfigurationSection
    {
        public static TestConfiguration Instance
        { 
            get 
            {
                // Первое решение
                // ConfigurationManager.RefreshSection(nameof(TestConfiguration));
                return (TestConfiguration)ConfigurationManager.GetSection(nameof(TestConfiguration));
            }  
        }

        private TestConfiguration() { }

        [ConfigurationProperty(nameof(TestSection), IsRequired = false)]
        public TestSection TestSection
        {
            get => (TestSection)this[nameof(TestSection)];
            set => this[nameof(TestSection)] = value;
        }
    }

    public class TestSection : ConfigurationElement
    {
        private const string SectionValue = "value";
        [ConfigurationProperty(SectionValue, IsRequired = false)]
        public int Value
        {
            get => (int)this[SectionValue];
            set => this[SectionValue] = value;
        }
    }
}