﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeSpanParserUtil;

namespace TimeSpanParserUtil.Tests {

    [TestClass]
    public class ReversedDocExamplesTests {

        // examples from https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings 

        [TestMethod]
        [DataRow("1.12:24:02", 1, 12, 23, 62, 0)]
        //[DataRow("1.03:14:56.1667", 1, 03, 14, 56, 1667)] 
        //[DataRow("1.03:14:56.1667000", 1, 03, 14, 56, 1667)]
        [DataRow("00:00:00", 0, 0, 0, 0, 0)] // c
        [DataRow("00:30:00", 0, 0, 30, 0, 0)] // c
        [DataRow("3.17:25:30.5000000", 3, 17, 25, 30, 500)] // c
        [DataRow("1:3:16:50.5", 1, 3, 16, 50, 500)] // g
        [DataRow("1:3:16:50.599", 1, 3, 16, 50, 599)] // g // fails (actual: 1.03:16:50.5989999)
        [DataRow("0:18:30:00.0000000", 0, 18, 30, 0, 0)] // G
        public void ReversedFormatStringUS(string parseThis, int days, int hours, int minutes, int seconds, int milliseconds) {
            var expected = new TimeSpan(days, hours, minutes, seconds, milliseconds);

            TimeSpan actual;
            bool success = TimeSpanParser.TryParse(parseThis, out actual);

            Assert.IsTrue(success);
            Assert.AreEqual(expected, actual);
        }

        // fr-FR examples from https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-timespan-format-strings 

        [TestMethod]
        [DataRow("1.12:24:02", 1, 12, 23, 62, 0)]
        [DataRow("00:00:00", 0, 0, 0, 0, 0)] // c
        [DataRow("00:30:00", 0, 0, 30, 0, 0)] // c
        [DataRow("1:3:16:50,5", 1, 3, 16, 50, 500)] // g fr-FR (TODO)
        [DataRow("1:3:16:50,599", 1, 3, 16, 50, 599)] // g fr-FR (TODO)
        [DataRow("0:18:30:00,0000000", 0, 18, 30, 0, 0)] // G fr-FR (TODO)
        public void ReversedFormatStringFR(string parseThis, int days, int hours, int minutes, int seconds, int milliseconds) {
            var expected = new TimeSpan(days, hours, minutes, seconds, milliseconds);

            var options = new TimeSpanParserOptions();
            options.FormatProvider = new CultureInfo("fr-FR");
            TimeSpan actual;
            bool success = TimeSpanParser.TryParse(parseThis, out actual, options);

            Assert.IsTrue(success);
            Assert.AreEqual(expected, actual);
        }

        // examples from https://msdn.microsoft.com/en-us/library/system.timespan.ticks(v=vs.110).aspx

        [TestMethod]
        [DataRow("00:00:00.0000001", 1)]
        [DataRow("128.17:30:33.3444555", 111222333444555)]
        [DataRow("20.84745602 days",    18_012_202_000_000)]
        [DataRow("20.84745602 days",    18_012_202_000_000)]
        [DataRow("20.20:20:20.2000000", 18_012_202_000_000)]
        public void TimeSpanTicks(string parseThis, long ticks) {
            // 10,000 ticks is one millisecond

            var expected = new TimeSpan(ticks);

            var options = new TimeSpanParserOptions();
            options.FormatProvider = new CultureInfo("en-US");

            TimeSpan actual;
            bool success = TimeSpanParser.TryParse(parseThis, out actual, options);
            Assert.IsTrue(success);
            Assert.AreEqual(expected, actual);
        }

    }
}
