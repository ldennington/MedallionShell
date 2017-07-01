﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleCommand;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Medallion.Shell.Tests
{
    [TestClass]
    public class PlatformCompatibilityTest
    {
        [TestMethod]
        public void TestReadAfterExit() => RunTest(() => PlatformCompatibilityTests.TestReadAfterExit());

        [TestMethod]
        public void TestWriteAfterExit() => RunTest(() => PlatformCompatibilityTests.TestWriteAfterExit());

        [TestMethod]
        public void TestExitWithMinusOne() => RunTest(() => PlatformCompatibilityTests.TestExitWithMinusOne());

        private static void RunTest(Expression<Action> testMethod)
        {
            var compiled = testMethod.Compile();
            UnitTestHelpers.AssertDoesNotThrow(compiled, "should run on .NET");

            var methodName = ((MethodCallExpression)testMethod.Body).Method.Name;
            const string MonoPath = @"C:\Program Files\Mono\bin\mono.exe";
            var command = Command.Run(MonoPath, "SampleCommand.exe", nameof(SampleCommand.PlatformCompatibilityTests), methodName);
            command.Result.Success.ShouldEqual(true, "should run on Mono. Got: " + command.Result.StandardError);
        }
    }
}
