using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Luna.Biz.Extensions
{
    static class TimeSpanExtensions
    {
        public static TimeSpan ParseTwitchTime(string input)
        {
            var m = Regex.Match(input, @"^(?:(\d+)h)? *(?:(\d+)m)? *(?:(\d+)s)?$", RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            int hs = m.Groups[1].Success ? int.Parse(m.Groups[1].Value) : 0;
            int ms = m.Groups[2].Success ? int.Parse(m.Groups[2].Value) : 0;
            int ss = m.Groups[3].Success ? int.Parse(m.Groups[3].Value) : 0;

            return new TimeSpan(hs, ms, ss);
        }
    }
}
