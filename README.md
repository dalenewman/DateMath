# Date Math

[![.NET](https://github.com/dalenewman/DateMath/actions/workflows/dotnet.yml/badge.svg)](https://github.com/dalenewman/DateMath/actions/workflows/dotnet.yml)
[![NuGet](https://img.shields.io/nuget/v/DateMath.svg)](https://www.nuget.org/packages/DateMath)

DateMath parses and applies Elasticsearch-style date math expressions in .NET. It supports expressions anchored to the current UTC time or a specific date, arithmetic with common date units, and rounding.

The package targets .NET Standard 2.0, making it compatible with modern .NET and .NET Framework 4.6.1 or later. Microsoft recommends .NET Framework 4.7.2 or later when consuming .NET Standard 2.0 libraries.

## Installation

Install the package with the .NET CLI:

```shell
dotnet add package DateMath
```

Or with the NuGet Package Manager Console:

```powershell
Install-Package DateMath
```

## Usage

Expressions can use `now` as their anchor:

```csharp
using DaleNewman;

DateMath.Parse("now", "yyyy-MM-dd h tt");
DateMath.Parse("now+11h", "yyyy-MM-dd h tt");
DateMath.Parse("now-13h+1y+1d-1s", "yyyy-MM-dd HH:mm:ss");
DateMath.Parse("now/d", "yyyy-MM-dd HH:mm:ss.fff");
```

A fixed anchor date is separated from its math expression by `||`:

```csharp
var result = DateMath.Parse("2016-12-31 1 PM||+11h", "yyyy-MM-dd h tt");
// 2017-01-01 12 AM
```

Use `TryParse` when you need to check whether parsing succeeded:

```csharp
if (DateMath.TryParse("now+11h", out DateTime date))
{
    // Use date.
}
```

Use `Apply` when you already have a date:

```csharp
var date = new DateTime(2016, 12, 31, 9, 30, 2);
var newYears = DateMath.Apply(date, "+1d/y");
// 2017-01-01 00:00:00
```

## Operators

| Unit | Meaning |
| --- | --- |
| `y` | Year |
| `M` | Month |
| `w` | Week |
| `d` | Day |
| `h` | Hour |
| `m` | Minute |
| `s` | Second |

Prefix a unit with `+` or `-` to add or subtract it, such as `now-10d`. Prefix it with `/` to round down, such as `now/M` for the start of the current month.

Common reporting ranges include:

| Range | Start | End |
| --- | --- | --- |
| Last 10 days | `now-10d` | `now` |
| Month to date | `now/M` | `now` |
| Year to date | `now/y` | `now` |
| Sliding 60-day window | `now-30d` | `now+30d` |

See the [tests](https://github.com/dalenewman/DateMath/blob/master/test/DateMath.Tests/DateMathTests.cs) for more examples, the [changelog](https://github.com/dalenewman/DateMath/blob/master/CHANGELOG.md) for release history, and the [NuGet packaging guide](https://github.com/dalenewman/DateMath/blob/master/docs/nuget-packaging.md) for local release instructions.

## Contributing

Issues and pull requests are welcome in the [GitHub repository](https://github.com/dalenewman/DateMath). Build and test locally with:

```shell
dotnet test --configuration Release
```

DateMath is licensed under the [Apache License 2.0](https://github.com/dalenewman/DateMath/blob/master/LICENSE.md).
