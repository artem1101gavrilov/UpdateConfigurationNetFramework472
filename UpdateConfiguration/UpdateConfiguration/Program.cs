using System;
using System.Configuration;
using System.Threading;

namespace UpdateConfiguration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine($"{TestConfiguration.Instance.TestSection.Value} {(ConfigurationManager.GetSection(nameof(TestConfiguration)) as TestConfiguration).TestSection.Value}");
                Thread.Sleep(1000);
            }
        }
    }
}
