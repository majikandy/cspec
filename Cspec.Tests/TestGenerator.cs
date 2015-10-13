namespace Cspec.Tests
{
    using System;
    using System.Diagnostics;
    using System.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestGenerator
    {
        [TestInitialize]
        public void Initialise()
        {
            Delete(@"Features.html");
            Delete(@"Features.txt");
        }

        [TestMethod]
        public void test_html_generator()
        {  
            var command = @"runner\Cspec.Generator.exe";
            var args = @"HTML ..\..\..\Cspec.Tests\bin\Release\Cspec.Tests.dll ..\..\Features Features.html";
            ExecuteProcess(command, args);

            Assert.IsTrue(File.Exists(@"Features.html"));
        }

        [TestMethod]
        public void test_gherkin_generator()
        {
            var command = @"runner\Cspec.Generator.exe";
            var args = @"GHERKIN ..\..\..\Cspec.Tests\bin\Release\Cspec.Tests.dll ..\..\Features Features.txt";
            ExecuteProcess(command, args);

            Assert.IsTrue(File.Exists(@"Features.txt"));
        }

        private static void ExecuteProcess(string command, string args)
        {
            var process = new Process { StartInfo = new ProcessStartInfo(command, args) };
            process.Start();
            process.WaitForExit();
        }

        private static void Delete(string filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
