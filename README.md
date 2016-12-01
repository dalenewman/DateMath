### DateMath

[![NuGet Pre Release](https://img.shields.io/nuget/vpre/DateMath.svg?style=plastic)](https://www.nuget.org/packages/DateMath)

This is a .NET library for parsing "date math" (as seen in [elasticsearch](https://www.elastic.co/guide/en/elasticsearch/reference/current/common-options.html#date-math)).

Let `now` be `2016-12-31 1 PM`:

```csharp
DateMath.Parse("now","yyyy-MM-dd h tt") // 2016-12-31 1 PM
DateMath.Parse("now+11h", "yyyy-MM-dd h tt") // 2017-01-01 12 AM
DateMath.Parse("now-13h+1y+1d-1s", "yyyy-MM-dd HH:mm:ss") // 2017-12-31 23:59:59
```

You can use this library for setting default parameters:


<table class="table table-condensed">
    <thead>
        <tr>
            <th>Range</th>
            <th>Start</th>
            <th>End</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>the last 10 days</td>
            <td>now-10d</td>
            <td>now</td>
        </tr>
        <tr>
            <td>the last month</td>
            <td>now-1M</td>
            <td>now</td>
        </tr>
        <tr>
            <td>a 60 day sliding window</td>
            <td>now-30d</td>
            <td>now+30d</td>
        </tr>
    </tbody>
</table>


See [tests](https://github.com/dalenewman/DateMath/blob/master/src/Testing/All.cs) for other usage.
