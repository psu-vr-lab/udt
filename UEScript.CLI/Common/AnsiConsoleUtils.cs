using System.Net;
using Spectre.Console;

namespace UEScript.CLI.Common;

public static class AnsiConsoleUtils
{
    public static Task WrapTaskAroundProgressBar(string title, Func<Task> task)
    {
        return AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var gettingReadyTask = ctx.AddTask($"[cyan]{title}[/]");

            await task();
            
            while (!ctx.IsFinished)
            {
                await Task.Delay(300);
                // @Cleanup: Handle real progress values?
                gettingReadyTask.Increment(10.5);
            }
        });
    }
    
    public static Task WrapTaskAroundProgressBar(string title, Action task)
    {
        return AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var gettingReadyTask = ctx.AddTask($"[cyan]{title}[/]");

            task();
            
            while (!ctx.IsFinished)
            {
                await Task.Delay(300);
                // @Cleanup: Handle real progress values?
                gettingReadyTask.Increment(10.5);
            }
        });
    }
    
    public static Task WrapTaskAroundProgressBar(string title, Func<ProgressTask, Task> action)
    {
        return AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var gettingReadyTask = ctx.AddTask($"[cyan]{title}[/]");

            await action(gettingReadyTask);
            
            //while (!ctx.IsFinished)
            //{
              //  await Task.Delay(300);
                // @Cleanup: Handle real progress values?
                //gettingReadyTask.Increment(10.5);
            // }
        });
    }
}