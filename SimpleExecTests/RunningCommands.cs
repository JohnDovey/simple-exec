using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using SimpleExec;
using SimpleExecTests.Infra;
using Xbehave;
using Xunit;

namespace SimpleExecTests
{
    public class RunningCommands
    {
        private static readonly string command = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "hello-world.cmd" : "ls";

        [Scenario]
        public void RunningASucceedingCommand(Exception exception)
        {
            "When I run a succeeding command"
                .x(() => exception = Record.Exception(() => Command.Run(command)));

            "Then no exception is thrown"
                .x(() => Assert.Null(exception));
        }

        [Scenario]
        public void RunningASucceedingCommandWithArgs(Exception exception)
        {
            "When I run a succeeding command"
                .x(() => exception = Record.Exception(
                    () => Command.Run("dotnet", $"exec {Tester.Path} hello world")));

            "Then no exception is thrown"
                .x(() => Assert.Null(exception));
        }

        [Scenario]
        public void RunningASucceedingCommandAsync(Exception exception)
        {
            "When I run a succeeding command async"
                .x(async () => exception = await Record.ExceptionAsync(() => Command.RunAsync(command)));

            "Then no exception is thrown"
                .x(() => Assert.Null(exception));
        }

        [Scenario]
        public void RunningAFailingCommand(Exception exception)
        {
            "When I run a failing command"
                .x(() => exception = Record.Exception(
                    () => Command.Run("dotnet", $"exec {Tester.Path} error hello world")));

            "Then a non-zero exit code exception is thrown"
                .x(() => Assert.IsType<NonZeroExitCodeException>(exception));

            "And the exception contains the exit code"
                .x(() => Assert.Equal(1, ((NonZeroExitCodeException)exception).ExitCode));
        }

        [Scenario]
        public void RunningAFailingCommandAsync(Exception exception)
        {
            "When I run a failing command async"
                .x(async () => exception = await Record.ExceptionAsync(
                    () => Command.RunAsync("dotnet", $"exec {Tester.Path} error hello world")));

            "Then a non-zero exit code exception is thrown"
                .x(() => Assert.IsType<NonZeroExitCodeException>(exception));

            "And the exception contains the exit code"
                .x(() => Assert.Equal(1, ((NonZeroExitCodeException)exception).ExitCode));
        }

        [Scenario]
        public void RunningANonExistentCommand(Exception exception)
        {
            "When I run a non-existent command"
                .x(() => exception = Record.Exception(
                    () => Command.Run("simple-exec-tests-non-existent-command")));

            "Then a Win32Exception exception, of all things, is thrown"
                .x(() => Assert.IsType<Win32Exception>(exception));
        }

        [Scenario]
        public void RunningANonExistentCommandAsync(Exception exception)
        {
            "When I run a non-existent command async"
                .x(async () => exception = await Record.ExceptionAsync(
                    () => Command.RunAsync("simple-exec-tests-non-existent-command")));

            "Then a Win32Exception exception, of all things, is thrown"
                .x(() => Assert.IsType<Win32Exception>(exception));
        }

        [Scenario]
        [Example(null)]
        [Example("")]
        [Example(" ")]
        public void RunningNoCommand(string name, Exception exception)
        {
            "When I run no command"
                .x(() => exception = Record.Exception(
                    () => Command.Run(name)));

            "Then an ArgumentException exception is thrown"
                .x(() => Assert.IsType<ArgumentException>(exception));
        }

        [Scenario]
        [Example(null)]
        [Example("")]
        [Example(" ")]
        public void RunningNoCommandAsync(string name, Exception exception)
        {
            "When I run no command async"
                .x(async () => exception = await Record.ExceptionAsync(
                    () => Command.RunAsync(name)));

            "Then an ArgumentException exception is thrown"
                .x(() => Assert.IsType<ArgumentException>(exception));
        }
    }
}
