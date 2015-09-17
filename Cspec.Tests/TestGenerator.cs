namespace Cspec.Tests
{
    using System;
    using System.Diagnostics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestGenerator
    {
        [TestMethod]
        public void test_runner()
        {
           
            var p = new ProcessStartInfo(@"runner\Cspec.Generator.exe", @"HTML ..\..\..\Cspec.Tests\bin\Release\Cspec.Tests.dll Features.html");
            
            var process = new Process();
            process.StartInfo = p;
            process.Start();
        }
    }
}
