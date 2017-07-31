# mns-netcore
[![Build status](https://ci.appveyor.com/api/projects/status/ut67i00jpk0vn3f3?svg=true)](https://ci.appveyor.com/project/aries-zhang/mns-netcore)
[![NuGet](https://img.shields.io/nuget/v/Nuget.Core.svg)](https://www.nuget.org/packages/mns-netcore/)


## How to use  


Send messages:
```
var Queue = MNS.Configure(Host, AccessKey, AccessSecret).Queue("test");

for (int i = 0; i < 200; i++)
{
    var result = Queue.SendMessage(args.Length >= 1 ? args[0] + i : "test" + i);
}
```

Receive messages:
```
var Queue = MNS.Configure(Host, AccessKey, AccessSecret).Queue("test");

while (true)
{
    try
    {
        var message = await Queue.ReceiveMessage();

        Console.WriteLine("message processing..");

        SomeBusiness.Process(message);

        Console.WriteLine("message processed");

        Queue.BatchDeleteMessage(message.ReceiptHandle);

        Console.WriteLine("message deleted.");
    }
    catch (Exception ex)
    {
        if (ex is MessageNotExistException)
            continue;
    }
}
```