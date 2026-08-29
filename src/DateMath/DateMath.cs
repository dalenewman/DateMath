#region license
// DateMath
// Date Math for .NET
// Copyright 2016-2026 Dale Newman
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using System.Globalization;
using System.Text.RegularExpressions;

namespace DaleNewman;

/// <summary>
/// Parses and applies Elasticsearch-style date math expressions.
/// </summary>
public static class DateMath
{
    private const string AnchorDatePattern = @"^now|.{6,}\|\|";
    private const string OperatorPattern = @"[/+/-]{1}\d+[dhMmswy]{1}";
    private const string RoundingPattern = @"/[dhMmswy]{1}";

    private static readonly Regex AnchorDate = new(AnchorDatePattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
    private static readonly Regex Operator = new(OperatorPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
    private static readonly Regex Rounding = new(RoundingPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);

    /// <summary>
    /// Parses a date math expression and formats the result.
    /// </summary>
    /// <param name="expression">The date math expression.</param>
    /// <param name="format">The date and time format string.</param>
    /// <returns>The formatted date, or the original expression when parsing fails.</returns>
    public static string Parse(string expression, string format)
    {
        _ = TryParse(expression, out var result, format);
        return result;
    }

    /// <summary>
    /// Parses a date math expression.
    /// </summary>
    /// <param name="expression">The date math expression.</param>
    /// <returns>The parsed date, or <see cref="DateTime.MinValue"/> when parsing fails.</returns>
    public static DateTime Parse(string expression)
    {
        _ = TryParse(expression, out var result);
        return result;
    }

    /// <summary>
    /// Attempts to parse a date math expression and format the result.
    /// </summary>
    /// <param name="expression">The date math expression.</param>
    /// <param name="result">The formatted date, or the original expression when parsing fails.</param>
    /// <param name="format">The date and time format string.</param>
    /// <returns><see langword="true"/> when parsing succeeds; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string expression, out string result, string format)
    {
        if (TryParse(expression, out var date))
        {
            result = date.ToString(format, CultureInfo.CurrentCulture);
            return true;
        }

        result = expression;
        return false;
    }

    /// <summary>
    /// Attempts to parse a date math expression.
    /// </summary>
    /// <param name="expression">The date math expression.</param>
    /// <param name="result">The parsed date, or <see cref="DateTime.MinValue"/> when parsing fails.</param>
    /// <returns><see langword="true"/> when parsing succeeds; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(string expression, out DateTime result)
    {
        var matchAnchorDate = AnchorDate.Match(expression);
        if (matchAnchorDate.Success)
        {
            string operators;
            DateTime date;

            var value = matchAnchorDate.Value;

            if (string.Equals(value, "now", StringComparison.OrdinalIgnoreCase))
            {
                date = DateTime.UtcNow;
                operators = expression.Substring(3);
            }
            else
            {
                value = value.TrimEnd('|');
                if (!DateTime.TryParse(value, out date))
                {
                    result = DateTime.MinValue;
                    return false;
                }

                operators = expression.Substring(matchAnchorDate.Value.Length);
            }

            result = Apply(date, operators);
            return true;
        }

        result = DateTime.MinValue;
        return false;
    }

    /// <summary>
    /// Applies date math operators and rounding to an existing date.
    /// </summary>
    /// <param name="input">The date to modify.</param>
    /// <param name="math">The date math operators and optional rounding expression.</param>
    /// <returns>The modified date.</returns>
    public static DateTime Apply(DateTime input, string math)
    {
        foreach (Match match in Operator.Matches(math))
        {
            input = ApplyOperator(input, match.Value);
        }

        var matchRounder = Rounding.Match(math);
        if (matchRounder.Success)
        {
            input = ApplyRounding(input, matchRounder.Value[1]);
        }

        return input;
    }

    private static TimeSpan UnitToInterval(char unit)
    {
        return unit switch
        {
            'w' => new TimeSpan(7, 0, 0, 0),
            'd' => new TimeSpan(1, 0, 0, 0),
            'h' => new TimeSpan(0, 1, 0, 0),
            'm' => new TimeSpan(0, 0, 1, 0),
            _ => new TimeSpan(0, 0, 0, 1),
        };
    }

    private static DateTime ApplyOperator(DateTime input, string @operator)
    {
        var numberPart = @operator.Substring(1, @operator.Length - 2);
        var number = int.Parse(numberPart, CultureInfo.CurrentCulture);
        var add = @operator[0] == '+';
        var unit = @operator[@operator.Length - 1];

        if (unit == 'y')
        {
            return input.AddYears(add ? number : -number);
        }

        if (unit == 'M')
        {
            return input.AddMonths(add ? number : -number);
        }

        var interval = UnitToInterval(unit);
        if (number > 1)
        {
            interval = new TimeSpan(number * interval.Ticks);
        }

        return add ? input.Add(interval) : input.Subtract(interval);
    }

    private static DateTime Floor(DateTime input, TimeSpan interval)
    {
        return input.AddTicks(-(input.Ticks % interval.Ticks));
    }

    private static DateTime ApplyRounding(DateTime input, char unit)
    {
        return unit switch
        {
            'y' => new DateTime(input.Year, 1, 1),
            'M' => new DateTime(input.Year, input.Month, 1),
            _ => Floor(input, UnitToInterval(unit)),
        };
    }
}
