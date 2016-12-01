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
using dalenewman;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Testing {

    [TestClass]
    public class All {

        /* Caution: These might not all pass, all the time, because time passes. */

        public static string Format = "yyyy-MM-dd";
        public static string FormatWithTime = "yyyy-MM-dd HH:mm:ss";

        [TestMethod]
        public void TestNow() {
            var expected = DateTime.UtcNow.ToString(Format);
            var actual = DateMath.Parse("now", Format);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void TestAddToNow() {
            var expected = DateTime.UtcNow.AddDays(3.0).AddHours(7.0).ToString(Format);
            var actual = DateMath.Parse("now+3d+7h", Format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddToNowWithARealDateTime() {
            var expected = DateTime.UtcNow.AddDays(3.0).AddHours(7.0);
            var actual = DateMath.Parse("now+3d+7h");
            Assert.AreEqual(expected.ToString(FormatWithTime), actual.ToString(FormatWithTime));
        }

        [TestMethod]
        public void TestAGoodDate() {
            const string expected = "2016-11-01";
            var actual = DateMath.Parse("2016-11-01||", Format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestABadDate() {
            const string expected = "201x-11-01||";
            var actual = DateMath.Parse("201x-11-01||", Format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddYear() {
            const string expected = "2017-12-01";
            var actual = DateMath.Parse("2016-12-01||+1y", Format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddMonth() {
            const string expected = "2016-12-01";
            var actual = DateMath.Parse("2016-11-01||+1M", Format);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void TestAddDay() {
            const string expected = "2016-11-21";
            var actual = DateMath.Parse("2016-11-20||+1d", Format);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddHour() {
            const string expected = "2016-12-01 01:00:00";
            var actual = DateMath.Parse("2016-12-01||+1h", FormatWithTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddMinute() {
            const string expected = "2016-12-01 00:01:00";
            var actual = DateMath.Parse("2016-12-01||+1m", FormatWithTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddSecond() {
            const string expected = "2016-12-01 00:00:01";
            var actual = DateMath.Parse("2016-12-01||+1s", FormatWithTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddTwoSeconds() {
            const string expected = "2016-12-01 00:00:02";
            var actual = DateMath.Parse("2016-12-01||+2s", FormatWithTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestAddCombo() {
            const string expected = "2016-12-01 01:02:03";
            var actual = DateMath.Parse("2016-12-01||+1h+2m+3s", FormatWithTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestSubtract() {
            const string expected = "2016-12-31 23:59:59";
            var actual = DateMath.Parse("2017-01-01||-1s", FormatWithTime);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TestOutOfOrder() {
            var expected = DateTime.UtcNow.AddDays(1.0).AddMinutes(1.0).AddSeconds(1.0);
            var actual = DateMath.Parse("now+1d+1s+1m");
            Assert.AreEqual(expected.ToString(FormatWithTime), actual.ToString(FormatWithTime));
        }

    }
}
