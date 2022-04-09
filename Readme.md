# CronNET

CronNET is a simple C# library for running tasks based on a cron schedule.

This repo is fork of original CronNET library, ported for work with .net 6

## Tests

![Tests](https://github.com/awitwicki/CronNET/actions/workflows/dotnet.yml/badge.svg)

## Cron Schedules

CronNET supports most cron scheduling.  See tests for supported formats.

```
*    *    *    *    *  
┬    ┬    ┬    ┬    ┬
│    │    │    │    │
│    │    │    │    │
│    │    │    │    └───── day of week (0 - 6) (Sunday=0 )
│    │    │    └────────── month (1 - 12)
│    │    └─────────────── day of month (1 - 31)
│    └──────────────────── hour (0 - 23)
└───────────────────────── min (0 - 59)
```

```
  `* * * * *`        Every minute.
  `0 * * * *`        Top of every hour.
  `0,1,2 * * * *`    Every hour at minutes 0, 1, and 2.
  `*/2 * * * *`      Every two minutes.
  `1-55 * * * *`     Every minute through the 55th minute.
  `* 1,10,20 * * *`  Every 1st, 10th, and 20th hours.
```
## Examples

```C#
CronDaemon d = new CronDaemon();

d.AddJob("*/1 * * * *", () => 
{
    Console.WriteLine(DateTime.Now.ToString());
});

d.Start();

// Wait and sleep forever. Let the cron daemon run.
await Task.Delay(Timeout.Infinite);
```

```C#
private void task()
{
    Console.WriteLine("Hello, world.")
}

cron_daemon.add_job(new CronJob("* * * * *", task));
cron_daemon.start();

// Wait and sleep forever. Let the cron daemon run.
await Task.Delay(Timeout.Infinite);

// Stop daemon if you want to
cron_daemon.stop();
```
