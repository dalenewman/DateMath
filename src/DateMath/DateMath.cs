#region license
// DateMath
// Date Math for .NET
// Copyright 2016 Dale Newman
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
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace dalenewman {

    public static class DateMath {

        private static readonly Regex AnchorDate = new Regex(@"^now|[\d\-]{6,}\|\|");
        private static readonly Regex Operator = new Regex(@"[/+/-]{1}\d+[yMwdhHms]{1}");

        public static string Parse(string expression, string format) {
            string result;
            TryParse(expression, out result, format);
            return result;
        }

        public static DateTime Parse(string expression) {
            DateTime result;
            TryParse(expression, out result);
            return result;
        }

        public static bool TryParse(string expression, out string result, string format) {
            DateTime date;
            if (TryParse(expression, out date)) {
                result = date.ToString(format);
                return true;
            }
            result = expression;
            return false;
        }

        public static bool TryParse(string expression, out DateTime result) {

            // try get anchor date
            var matchAnchorDate = AnchorDate.Match(expression);
            if (matchAnchorDate.Success) {
                string operators;
                DateTime date;

                var value = matchAnchorDate.Value.ToLower();

                if (value == "now") {
                    date = DateTime.UtcNow;
                    operators = expression.Substring(3);
                } else {
                    value = value.TrimEnd(new[] { '|' });
                    if (!DateTime.TryParse(value, out date)) {
                        result = DateTime.MinValue;
                        return false;
                    }
                    operators = expression.Substring(matchAnchorDate.Value.Length);
                }

                date = Operator.Matches(operators).Cast<Match>().Aggregate(date, (current, match) => ApplyOperator(current, match.Value));

                result = date;
                return true;
            }

            result = DateTime.MinValue;
            return false;
        }

        private static DateTime ApplyOperator(DateTime input, string @operator) {

            var numberPart = @operator.Substring(1, @operator.Length - 2);
            var number = int.Parse(numberPart);
            var daysInYear = DateTime.IsLeapYear(input.Year) && input < new DateTime(input.Year, 2, 29) ? 366 : 365;
            var add = @operator[0] == '+';
            var unit = @operator[@operator.Length - 1];

            TimeSpan timeSpan;
            //yMwdhHms

            switch (unit) {
                case 'y': // year
                    timeSpan = new TimeSpan(daysInYear, 0, 0, 0);
                    break;
                case 'M': // month
                    timeSpan = new TimeSpan(daysInYear / 12, 0, 0, 0);
                    break;
                case 'w': // week
                    timeSpan = new TimeSpan(7, 0, 0, 0);
                    break;
                case 'd': // day
                    timeSpan = new TimeSpan(1, 0, 0, 0);
                    break;
                case 'h':// hour
                case 'H':
                    timeSpan = new TimeSpan(0, 1, 0, 0);
                    break;
                case 'm': // minute
                    timeSpan = new TimeSpan(0, 0, 1, 0);
                    break;
                default: // second
                    timeSpan = new TimeSpan(0, 0, 0, 1);
                    break;
            }

            if (number > 1) {
                timeSpan = new TimeSpan(number * timeSpan.Ticks);
            }

            return add ? input.Add(timeSpan) : input.Subtract(timeSpan);

        }
    }
}

