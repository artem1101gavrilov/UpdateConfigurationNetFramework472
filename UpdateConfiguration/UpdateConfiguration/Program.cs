using System;
using System.Configuration;
using System.IO;
using System.Threading;

namespace UpdateConfiguration
{
    internal class Program
    {
        public static string rootFolderPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string configFilePath = Path.Combine(rootFolderPath, "UpdateConfiguration.exe.config");

        static void Main(string[] args)
        {
            Console.WriteLine(configFilePath);
            // Второе решение
            Thread monitorThread = new Thread(ConfigFileMonitor.BeginConfigFilesMonitor);
            monitorThread.Start();
            while (true)
            {
                Console.WriteLine($"{TestConfiguration.Instance.TestSection.Value} {(ConfigurationManager.GetSection(nameof(TestConfiguration)) as TestConfiguration).TestSection.Value}");
                Thread.Sleep(1000);
            }
        }
    }

    /// <summary>
    /// Monitors for any change in the app.config file
    /// <see cref="https://stackoverflow.com/questions/13876372/using-filesystemmonitoring-for-reading-changes-in-app-config-and-writing-to-app"/>
    /// </summary>
    public class ConfigFileMonitor
    {
        private static FileSystemWatcher _watcher;
        private static string _configFilePath = String.Empty;
        private static string _configFileName = String.Empty;

        /// <summary>
        /// Texts the files surveillance.
        /// </summary>
        public static void BeginConfigFilesMonitor()
        {
            try
            {
                string fileToMonitor = Program.configFilePath;
                if (fileToMonitor.Length == 0)
                    Console.WriteLine("Incorrect config file specified to watch");
                else
                {
                    _configFileName = Path.GetFileName(fileToMonitor);
                    _configFilePath = fileToMonitor.Substring(0, fileToMonitor.IndexOf(_configFileName));
                }
                WatchConfigFiles(_configFilePath, _configFileName);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }

        /// <summary>
        /// Watches the files.
        /// </summary>
        /// <param name="targetDir">The target dir.</param>
        /// <param name="filteredBy">The filtered by.</param>
        public static void WatchConfigFiles(string targetDir, string filteredBy)
        {
            try
            {
                _watcher = new FileSystemWatcher();
                _watcher.Path = targetDir;
                _watcher.Filter = filteredBy;
                _watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite;
                _watcher.EnableRaisingEvents = true;

                _watcher.Changed += new FileSystemEventHandler(FileChanged);
                _watcher.WaitForChanged(WatcherChangeTypes.Changed);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }


        /// <summary>
        /// Handles the Changed event of the File control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.IO.FileSystemEventArgs"/> instance containing the event data.</param>
        protected static void FileChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                _watcher.EnableRaisingEvents = false;

                string filechange = e.FullPath;
                Console.WriteLine("Configuration File: " + filechange + "changed");
                
                _watcher.EnableRaisingEvents = true;

                ConfigurationManager.RefreshSection(nameof(TestConfiguration));
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
        }
    }
}