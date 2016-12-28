### Date Math

[![Build status](https://ci.appveyor.com/api/projects/status/0f7vppj64aogn4ao?svg=true)](https://ci.appveyor.com/project/dalenewman/datemath)
[![NuGet Pre Release](https://img.shields.io/nuget/vpre/DateMath.svg)](https://www.nuget.org/packages/DateMath)

This is a .NET library for parsing "date math" as seen in Elasticsearch.
To find out more about date math, read [this](https://www.elastic.co/guide/en/elasticsearch/reference/current/common-options.html#date-math) and [this](https://www.elastic.co/guide/en/elasticsearch/client/net-api/current/date-math-expressions.html).

You can use this library for setting default 
report parameters, e.g:

<table class="table table-condensed">
    <thead>
        <tr>
            <th>Range Description</th>
            <th>Start</th>
            <th>End</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>last 10 days</td>
            <td>now-10d</td>
            <td>now</td>
        </tr>
        <tr>
            <td>month to date</td>
            <td>now/M</td>
            <td>now</td>
        </tr>
        <tr>
            <td>year to date</td>
            <td>now/y</td>
            <td>now</td>
        </tr>
        <tr>
            <td>a sliding 60 day window</td>
            <td>now-30d</td>
            <td>now+30d</td>
        </tr>
    </tbody>
</table>

#### Basic Usage

Let `now` be `2016-12-31 1 PM`:

```csharp
DateMath.Parse("now","yyyy-MM-dd h tt"); // 2016-12-31 1 PM
DateMath.Parse("now+11h", "yyyy-MM-dd h tt"); // 2017-01-01 12 AM
DateMath.Parse("now-13h+1y+1d-1s", "yyyy-MM-dd HH:mm:ss"); // 2017-12-31 23:59:59
DateMath.Parse("now/d", "yyyy-MM-dd HH:mm:ss.fff"); // 2016-12-31 00:00:00.000
```

If you want to know if your string was parsed successfully:

```csharp
DateTime date;
if(DateMath.TryParse("now+11h", out date)){
    // do something with date
}
```

If you already have a date:

```csharp
DateTime date = new DateTime(2016,12,31,9,30,2);
var newYears = DateMath.Apply(date, "+1d/y");
```


See [tests](https://github.com/dalenewman/DateMath/blob/master/src/Testing/All.cs) for other usage.




