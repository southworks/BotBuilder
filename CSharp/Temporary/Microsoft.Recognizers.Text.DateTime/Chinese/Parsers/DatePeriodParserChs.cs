﻿using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Recognizers.Text.DateTime.Chinese.Extractors;
using Microsoft.Recognizers.Text.Number.Chinese.Extractors;
using Microsoft.Recognizers.Text.Number.Chinese.Parsers;
using DateObject = System.DateTime;
using Microsoft.Recognizers.Text.DateTime.Parsers;
using Microsoft.Recognizers.Text.DateTime.Utilities;

namespace Microsoft.Recognizers.Text.DateTime.Chinese.Parsers
{
    public class DatePeriodParserChs : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATEPERIOD; //"DatePeriod";

        private static readonly IExtractor _singleDateExtractor = new DateExtractorChs();

        private static readonly IExtractor _integerExtractor = new IntegerExtractor();

        private static readonly IParser _integerParser = new ChineseNumberParser(new ChineseNumberParserConfiguration());

        private static readonly IExtractor durationextractor = new DurationExtractorChs();

        private static readonly Calendar _cal = DateTimeFormatInfo.InvariantInfo.Calendar;

        private readonly IFullDateTimeParserConfiguration config;

        public DatePeriodParserChs(IFullDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult extResult)
        {
            return this.Parse(extResult, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refDate)
        {
            var referenceDate = refDate;

            object value = null;
            var luisStr = string.Empty;
            var valueStr = string.Empty;
            if (er.Type.Equals(ParserName))
            {
                var innerResult = ParseSimpleCases(er.Text, referenceDate);
                if (!innerResult.Success)
                {
                    innerResult = ParseOneWordPeriod(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = MergeTwoTimePoints(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseNumberWithUnit(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseYearAndMonth(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseYearToYear(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseYear(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseWeekOfMonth(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseSeason(er.Text, referenceDate);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseQuarter(er.Text, referenceDate);
                }


                if (innerResult.Success)
                {
                    if (innerResult.FutureValue != null && innerResult.PastValue != null)
                    {
                        innerResult.FutureResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                Util.FormatDate(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATE,
                                Util.FormatDate(((Tuple<DateObject, DateObject>) innerResult.FutureValue).Item2)
                            }
                        };
                        innerResult.PastResolution = new Dictionary<string, string>
                        {
                            {
                                TimeTypeConstants.START_DATE,
                                Util.FormatDate(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item1)
                            },
                            {
                                TimeTypeConstants.END_DATE,
                                Util.FormatDate(((Tuple<DateObject, DateObject>) innerResult.PastValue).Item2)
                            }
                        };
                    }
                    else
                    {
                        innerResult.PastResolution = innerResult.FutureResolution = new Dictionary<string, string>();
                    }

                    value = innerResult;
                }
            }

            var ret = new DateTimeParseResult
            {
                Text = er.Text,
                Start = er.Start,
                Length = er.Length,
                Type = er.Type,
                Data = er.Data,
                Value = value,
                TimexStr = value == null ? "" : ((DTParseResult) value).Timex,
                ResolutionStr = ""
            };
            return ret;
        }

        private DTParseResult ParseSimpleCases(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int beginDay = referenceDate.Day, endDay = referenceDate.Day;
            var noYear = false;
            var inputYear = false;

            var trimedText = text.Trim();
            var match = DatePeriodExtractorChs.SimpleCasesRegex.Match(trimedText);
            string beginLuisStr = string.Empty, endLuisStr = string.Empty;
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var days = match.Groups["day"];
                beginDay = this.config.DayOfMonth[days.Captures[0].Value.ToLower()];
                endDay = this.config.DayOfMonth[days.Captures[1].Value.ToLower()];

                var monthStr = match.Groups["month"].Value;
                var yearStr = match.Groups["year"].Value;
                if (!string.IsNullOrEmpty(yearStr))
                {
                    year = int.Parse(yearStr);
                    inputYear = true;
                }
                else
                {
                    noYear = true;
                }
                if (!string.IsNullOrEmpty(monthStr))
                {
                    month = this.config.MonthOfYear[monthStr.ToLower()];
                }
                else
                {
                    monthStr = match.Groups["relmonth"].Value.Trim().ToLower();
                    var thismatch = DatePeriodExtractorChs.ThisRegex.Match(monthStr);
                    var nextmatch = DatePeriodExtractorChs.NextRegex.Match(monthStr);
                    var lastmatch = DatePeriodExtractorChs.LastRegex.Match(monthStr);

                    if (thismatch.Success)
                    {
                        // do nothing
                    }
                    else if (nextmatch.Success)
                    {
                        if (month != 12)
                        {
                            month += 1;
                        }
                        else
                        {
                            month = 1;
                            year += 1;
                        }
                    }
                    else
                    {
                        if (month != 1)
                        {
                            month -= 1;
                        }
                        else
                        {
                            month = 12;
                            year -= 1;
                        }
                    }
                }

                if (inputYear || DatePeriodExtractorChs.ThisRegex.Match(monthStr).Success ||
                    DatePeriodExtractorChs.NextRegex.Match(monthStr).Success)
                {
                    beginLuisStr = Util.LuisDate(year, month, beginDay);
                    endLuisStr = Util.LuisDate(year, month, endDay);
                }
                else
                {
                    beginLuisStr = Util.LuisDate(-1, month, beginDay);
                    endLuisStr = Util.LuisDate(-1, month, endDay);
                }
            }
            else
            {
                return ret;
            }

            int futureYear = year, pastYear = year;
            var startDate = new DateObject(year, month, beginDay);
            if (noYear && startDate < referenceDate)
            {
                futureYear++;
            }
            if (noYear && startDate >= referenceDate)
            {
                pastYear--;
            }

            ret.Timex = $"({beginLuisStr},{endLuisStr},P{endDay - beginDay}D)";
            ret.FutureValue = new Tuple<DateObject, DateObject>(
                new DateObject(futureYear, month, beginDay),
                new DateObject(futureYear, month, endDay));
            ret.PastValue = new Tuple<DateObject, DateObject>(
                new DateObject(pastYear, month, beginDay),
                new DateObject(pastYear, month, endDay));
            ret.Success = true;

            return ret;
        }

        // handle like "2016年到2017年"
        private static DTParseResult ParseYearToYear(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var match = DatePeriodExtractorChs.YearToYear.Match(text);
            if (match.Success)
            {
                var yearMatch = DatePeriodExtractorChs.YearRegex.Matches(text);
                var yearInChineseMatch = DatePeriodExtractorChs.YearInChineseRegex.Matches(text);
                var BeginYear = 0;
                var EndYear = 0;
                if (yearMatch.Count == 2)
                {
                    var yearFrom = yearMatch[0].Groups["year"].Value;
                    var yearTo = yearMatch[1].Groups["year"].Value;
                    BeginYear = int.Parse(yearFrom);
                    EndYear = int.Parse(yearTo);
                }
                else if (yearInChineseMatch.Count == 2)
                {
                    var yearFrom = yearInChineseMatch[0].Groups["yearchs"].Value;
                    var yearTo = yearInChineseMatch[1].Groups["yearchs"].Value;
                    BeginYear = ConvertChineseToInteger(yearFrom);
                    EndYear = ConvertChineseToInteger(yearTo);
                }
                else if (yearInChineseMatch.Count == 1 && yearMatch.Count == 1)
                {
                    if (yearMatch[0].Index < yearInChineseMatch[0].Index)
                    {
                        var yearFrom = yearMatch[0].Groups["year"].Value;
                        var yearTo = yearInChineseMatch[0].Groups["yearch"].Value;
                        BeginYear = int.Parse(yearFrom);
                        EndYear = ConvertChineseToInteger(yearTo);
                    }
                    else
                    {
                        var yearFrom = yearInChineseMatch[0].Groups["yearch"].Value;
                        var yearTo = yearMatch[0].Groups["year"].Value;
                        BeginYear = ConvertChineseToInteger(yearFrom);
                        EndYear = int.Parse(yearTo);
                    }
                }
                if (BeginYear < 100 && BeginYear >= 90)
                {
                    BeginYear += 1900;
                }
                else if (BeginYear < 100 && BeginYear < 20)
                {
                    BeginYear += 2000;
                }
                if (EndYear < 100 && EndYear >= 90)
                {
                    EndYear += 1900;
                }
                else if (EndYear < 100 && EndYear < 20)
                {
                    EndYear += 2000;
                }

                var beginDay = new DateObject(BeginYear, 1, 1);
                var endDay = new DateObject(EndYear, 12, 31);
                var BeginTimex = BeginYear.ToString("D4");
                var EndTimex = EndYear.ToString("D4");
                ret.Timex = $"({BeginTimex},{EndTimex},P{EndYear - BeginYear}Y)";
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
                ret.Success = true;
                return ret;
            }
            return ret;
        }

        // for case "2016年5月"
        private DTParseResult ParseYearAndMonth(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var match = DatePeriodExtractorChs.YearAndMonth.Match(text);
            if (!(match.Success && match.Length == text.Length))
            {
                match = DatePeriodExtractorChs.PureNumYearAndMonth.Match(text);
            }
            if (!(match.Success && match.Length == text.Length))
            {
                return ret;
            }

            // parse year
            var year = referenceDate.Year;
            var yearNum = match.Groups["year"].Value;
            var yearChs = match.Groups["yearchs"].Value;
            var yearRel = match.Groups["yearrel"].Value;
            if (!string.IsNullOrEmpty(yearNum))
            {
                if (yearNum.EndsWith("年"))
                {
                    yearNum = yearNum.Substring(0, yearNum.Length - 1);
                }
                year = int.Parse(yearNum);
            }
            else if (!string.IsNullOrEmpty(yearChs))
            {
                if (yearChs.EndsWith("年"))
                {
                    yearChs = yearChs.Substring(0, yearChs.Length - 1);
                }
                year = ConvertChineseToInteger(yearChs);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                if (yearRel.EndsWith("去年"))
                {
                    year--;
                }
                else if (yearRel.EndsWith("明年"))
                {
                    year++;
                }
            }

            if (year < 100 && year >= 90)
            {
                year += 1900;
            }
            else if (year < 20)
            {
                year += 2000;
            }

            var monthStr = match.Groups["month"].Value;
            var month = this.config.MonthOfYear[monthStr] > 12 ? this.config.MonthOfYear[monthStr]%12 : this.config.MonthOfYear[monthStr];
            var beginDay = new DateObject(year, month, 1);
            var endDay = new DateObject();
            if (month == 12)
            {
                endDay = new DateObject(year + 1, 1, 1);
            }
            else
            {
                endDay = new DateObject(year, month + 1, 1);
            }
            ret.Timex = year.ToString("D4") + "-" + month.ToString("D2");
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
            ret.Success = true;
            return ret;
        }

        // case like "今年三月" "这个周末" "五月"
        private DTParseResult ParseOneWordPeriod(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            int year = referenceDate.Year, month = referenceDate.Month;
            int futureYear = year, pastYear = year;

            var trimedText = text.Trim().ToLower();
            var match = DatePeriodExtractorChs.OneWordPeriodRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var monthStr = match.Groups["month"].Value;
                if (trimedText.Equals("今年"))
                {
                    ret.Timex = referenceDate.Year.ToString("D4");
                    ret.FutureValue =
                        ret.PastValue =
                            new Tuple<DateObject, DateObject>(new DateObject(referenceDate.Year, 1, 1), referenceDate);
                    ret.Success = true;
                    return ret;
                }
                var thismatch = DatePeriodExtractorChs.ThisRegex.Match(trimedText);
                var nextmatch = DatePeriodExtractorChs.NextRegex.Match(trimedText);
                var lastmatch = DatePeriodExtractorChs.LastRegex.Match(trimedText);
                if (!string.IsNullOrEmpty(monthStr))
                {
                    var swift = -10;

                    if (trimedText.StartsWith("明年"))
                    {
                        swift = 1;
                    }
                    else if (trimedText.StartsWith("去年"))
                    {
                        swift = -1;
                    }
                    else if (trimedText.StartsWith("今年"))
                    {
                        swift = 0;
                    }
                    month = this.config.MonthOfYear[monthStr.ToLower()];

                    if (swift >= -1)
                    {
                        ret.Timex = (referenceDate.Year + swift).ToString("D4") + "-" + month.ToString("D2");
                        year = year + swift;
                        futureYear = pastYear = year;
                    }
                    else
                    {
                        ret.Timex = "XXXX-" + month.ToString("D2");
                        if (month < referenceDate.Month)
                        {
                            futureYear++;
                        }
                        if (month >= referenceDate.Month)
                        {
                            pastYear--;
                        }
                    }
                }
                else
                {
                    var swift = 0;
                    if (nextmatch.Success)
                    {
                        swift = 1;
                    }
                    else if (lastmatch.Success)
                    {
                        swift = -1;
                    }

                    if (trimedText.EndsWith("周") | trimedText.EndsWith("星期"))
                    {
                        var monday = referenceDate.This(DayOfWeek.Monday).AddDays(7*swift);
                        ret.Timex = monday.Year.ToString("D4") + "-W" +
                                    _cal.GetWeekOfYear(monday, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                        .ToString("D2");
                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(
                                    referenceDate.This(DayOfWeek.Monday).AddDays(7*swift),
                                    referenceDate.This(DayOfWeek.Sunday).AddDays(7*swift).AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                    if (trimedText.EndsWith("周末"))
                    {
                        DateObject beginDate, endDate;
                        beginDate = referenceDate.This(DayOfWeek.Saturday).AddDays(7*swift);
                        endDate = referenceDate.This(DayOfWeek.Sunday).AddDays(7*swift);

                        ret.Timex = beginDate.Year.ToString("D4") + "-W" +
                                    _cal.GetWeekOfYear(beginDate, CalendarWeekRule.FirstDay, DayOfWeek.Monday)
                                        .ToString("D2") + "-WE";
                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate.AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                    if (trimedText.EndsWith("月"))
                    {
                        month = referenceDate.AddMonths(swift).Month;
                        year = referenceDate.AddMonths(swift).Year;
                        ret.Timex = year.ToString("D4") + "-" + month.ToString("D2");
                        futureYear = pastYear = year;
                    }
                    else if (trimedText.EndsWith("年"))
                    {
                        year = referenceDate.AddYears(swift).Year;
                        if (trimedText.EndsWith("去年"))
                        {
                            year--;
                        }
                        else if (trimedText.EndsWith("明年"))
                        {
                            year++;
                        }
                        else if (trimedText.EndsWith("前年"))
                        {
                            year -= 2;
                        }
                        else if (trimedText.EndsWith("后年"))
                        {
                            year += 2;
                        }
                        ret.Timex = year.ToString("D4");
                        ret.FutureValue =
                            ret.PastValue =
                                new Tuple<DateObject, DateObject>(new DateObject(year, 1, 1),
                                    new DateObject(year, 12, 31).AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                }
            }
            else
            {
                return ret;
            }

            // only "month" will come to here
            ret.FutureValue = new Tuple<DateObject, DateObject>(
                new DateObject(futureYear, month, 1),
                new DateObject(futureYear, month, 1).AddMonths(1));
            ret.PastValue = new Tuple<DateObject, DateObject>(
                new DateObject(pastYear, month, 1),
                new DateObject(pastYear, month, 1).AddMonths(1));
            ret.Success = true;
            return ret;
        }

        // concert Chinese Number to Integer
        private static int ConvertChineseToNum(string numStr)
        {
            var num = -1;
            var er = _integerExtractor.Extract(numStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                {
                    num = Convert.ToInt32((double) (_integerParser.Parse(er[0]).Value ?? 0));
                }
            }
            return num;
        }

        // convert Chinese Year to Integer
        private static int ConvertChineseToInteger(string yearChsStr)
        {
            var year = 0;
            var num = 0;

            var er = _integerExtractor.Extract(yearChsStr);
            if (er.Count != 0)
            {
                if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                {
                    num = Convert.ToInt32((double) (_integerParser.Parse(er[0]).Value ?? 0));
                }
            }
            if (num < 10)
            {
                num = 0;
                foreach (var ch in yearChsStr)
                {
                    num *= 10;
                    er = _integerExtractor.Extract(ch.ToString());
                    if (er.Count != 0)
                    {
                        if (er[0].Type.Equals(Number.Constants.SYS_NUM_INTEGER))
                        {
                            num += Convert.ToInt32((double) (_integerParser.Parse(er[0]).Value ?? 0));
                        }
                    }
                }
                year = num;
            }
            else
            {
                year = num;
            }
            return year == 0 ? -1 : year;
        }

        // only contains year like "2016年"
        private static DTParseResult ParseYear(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var match = DatePeriodExtractorChs.YearRegex.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                var tmp = match.Value;
                if (tmp.EndsWith("年"))
                {
                    tmp = tmp.Substring(0, tmp.Length - 1);
                }
                var num = 0;
                var year = 0;
                if (tmp.Length == 2)
                {
                    num = int.Parse(tmp);
                    if (num < 100 && num >= 20)
                    {
                        num += 1900;
                    }
                    else if (num < 20)
                    {
                        num += 2000;
                    }
                    year = num;
                }
                else
                {
                    year = int.Parse(tmp);
                }
                var beginDay = new DateObject(year, 1, 1);
                var endDay = new DateObject(year + 1, 1, 1);
                ret.Timex = year.ToString("D4");
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
                ret.Success = true;
                return ret;
            }
            match = DatePeriodExtractorChs.YearInChineseRegex.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                var tmp = match.Value;
                if (tmp.EndsWith("年"))
                {
                    tmp = tmp.Substring(0, tmp.Length - 1);
                }
                if (tmp.Length == 1)
                {
                    return ret;
                }
                var re = ConvertChineseToInteger(tmp);
                var year = re;

                if (year < 100 && year >= 90)
                {
                    year += 1900;
                }
                else if (year < 100 && year < 20)
                {
                    year += 2000;
                }
                var beginDay = new DateObject(year, 1, 1);
                var endDay = new DateObject(year + 1, 1, 1);
                ret.Timex = year.ToString("D4");
                ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDay, endDay);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        // parse entities that made up by two time points
        private DTParseResult MergeTwoTimePoints(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var er = _singleDateExtractor.Extract(text);
            if (er.Count < 2)
            {
                er = _singleDateExtractor.Extract("on " + text);
                if (er.Count < 2)
                {
                    return ret;
                }
                er[0].Start -= 3;
                er[1].Start -= 3;
            }

            var pr1 = this.config.DateParser.Parse(er[0], referenceDate);
            var pr2 = this.config.DateParser.Parse(er[1], referenceDate);
            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            DateObject futureBegin = (DateObject) ((DTParseResult) pr1.Value).FutureValue,
                futureEnd = (DateObject) ((DTParseResult) pr2.Value).FutureValue;
            DateObject pastBegin = (DateObject) ((DTParseResult) pr1.Value).PastValue,
                pastEnd = (DateObject) ((DTParseResult) pr2.Value).PastValue;
            if (futureBegin > futureEnd)
            {
                futureBegin = pastBegin;
            }
            if (pastEnd < pastBegin)
            {
                pastEnd = futureEnd;
            }


            ret.Timex = $"({pr1.TimexStr},{pr2.TimexStr},P{(futureEnd - futureBegin).TotalDays}D)";
            ret.FutureValue = new Tuple<DateObject, DateObject>(futureBegin, futureEnd);
            ret.PastValue = new Tuple<DateObject, DateObject>(pastBegin, pastEnd);
            ret.Success = true;

            return ret;
        }

        // handle like "前两年" "前三个月"
        private DTParseResult ParseNumberWithUnit(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();

            var numStr = string.Empty;
            var unitStr = string.Empty;

            // if there are NO spaces between number and unit
            var match = DatePeriodExtractorChs.NumberCombinedWithUnit.Match(text);
            if (match.Success)
            {
                var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                var beforeStr = text.Substring(0, match.Index).Trim().ToLowerInvariant();
                if (this.config.UnitMap.ContainsKey(srcUnit))
                {
                    unitStr = this.config.UnitMap[srcUnit];
                    numStr = match.Groups["num"].Value;
                    var prefixMatch = DatePeriodExtractorChs.PastRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "D":
                                beginDate = referenceDate.AddDays(-double.Parse(numStr));
                                endDate = referenceDate;
                                break;
                            case "W":
                                beginDate = referenceDate.AddDays(-7*double.Parse(numStr));
                                endDate = referenceDate;
                                break;
                            case "MON":
                                beginDate = referenceDate.AddMonths(-Convert.ToInt32(double.Parse(numStr)));
                                endDate = referenceDate;
                                break;
                            case "Y":
                                beginDate = referenceDate.AddYears(-Convert.ToInt32(double.Parse(numStr)));
                                endDate = referenceDate;
                                break;
                            default:
                                return ret;
                        }
                        ret.Timex = $"({Util.LuisDate(beginDate)},{Util.LuisDate(endDate)},P{numStr}{unitStr[0]})";
                        ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                        ret.Success = true;
                        return ret;
                    }
                    prefixMatch = DatePeriodExtractorChs.FutureRegex.Match(beforeStr);
                    if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                    {
                        DateObject beginDate, endDate;
                        switch (unitStr)
                        {
                            case "D":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddDays(double.Parse(numStr));
                                break;
                            case "W":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddDays(7*double.Parse(numStr));
                                break;
                            case "MON":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddMonths(Convert.ToInt32(double.Parse(numStr)));
                                break;
                            case "Y":
                                beginDate = referenceDate;
                                endDate = referenceDate.AddYears(Convert.ToInt32(double.Parse(numStr)));
                                break;
                            default:
                                return ret;
                        }
                        ret.Timex =
                            $"({Util.LuisDate(beginDate.AddDays(1))},{Util.LuisDate(endDate.AddDays(1))},P{numStr}{unitStr[0]})";
                        ret.FutureValue =
                            ret.PastValue = new Tuple<DateObject, DateObject>(beginDate.AddDays(1), endDate.AddDays(1));
                        ret.Success = true;
                        return ret;
                    }
                }
            }

            // for case "前两年" "后三年"
            var duration_res = durationextractor.Extract(text);
            if (duration_res.Count > 0)
            {
                var beforeStr = text.Substring(0, (int) duration_res[0].Start).Trim().ToLowerInvariant();
                match = DatePeriodExtractorChs.UnitRegex.Match(duration_res[0].Text);
                if (match.Success)
                {
                    var srcUnit = match.Groups["unit"].Value.ToLowerInvariant();
                    var numberStr = duration_res[0].Text.Substring(0, match.Index).Trim().ToLowerInvariant();
                    var number = ConvertChineseToNum(numberStr);
                    if (this.config.UnitMap.ContainsKey(srcUnit))
                    {
                        unitStr = this.config.UnitMap[srcUnit];
                        numStr = number.ToString();
                        var prefixMatch = DatePeriodExtractorChs.PastRegex.Match(beforeStr);
                        if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                        {
                            DateObject beginDate, endDate;
                            switch (unitStr)
                            {
                                case "D":
                                    beginDate = referenceDate.AddDays(-double.Parse(numStr));
                                    endDate = referenceDate;
                                    break;
                                case "W":
                                    beginDate = referenceDate.AddDays(-7*double.Parse(numStr));
                                    endDate = referenceDate;
                                    break;
                                case "MON":
                                    beginDate = referenceDate.AddMonths(-Convert.ToInt32(double.Parse(numStr)));
                                    endDate = referenceDate;
                                    break;
                                case "Y":
                                    beginDate = referenceDate.AddYears(-Convert.ToInt32(double.Parse(numStr)));
                                    endDate = referenceDate;
                                    break;
                                default:
                                    return ret;
                            }
                            ret.Timex = $"({Util.LuisDate(beginDate)},{Util.LuisDate(endDate)},P{numStr}{unitStr[0]})";
                            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
                            ret.Success = true;
                            return ret;
                        }
                        prefixMatch = DatePeriodExtractorChs.FutureRegex.Match(beforeStr);
                        if (prefixMatch.Success && prefixMatch.Length == beforeStr.Length)
                        {
                            DateObject beginDate, endDate;
                            switch (unitStr)
                            {
                                case "D":
                                    beginDate = referenceDate;
                                    endDate = referenceDate.AddDays(double.Parse(numStr));
                                    break;
                                case "W":
                                    beginDate = referenceDate;
                                    endDate = referenceDate.AddDays(7*double.Parse(numStr));
                                    break;
                                case "MON":
                                    beginDate = referenceDate;
                                    endDate = referenceDate.AddMonths(Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                case "Y":
                                    beginDate = referenceDate;
                                    endDate = referenceDate.AddYears(Convert.ToInt32(double.Parse(numStr)));
                                    break;
                                default:
                                    return ret;
                            }
                            ret.Timex =
                                $"({Util.LuisDate(beginDate.AddDays(1))},{Util.LuisDate(endDate.AddDays(1))},P{numStr}{unitStr[0]})";
                            ret.FutureValue =
                                ret.PastValue =
                                    new Tuple<DateObject, DateObject>(beginDate.AddDays(1), endDate.AddDays(1));
                            ret.Success = true;
                            return ret;
                        }
                    }
                }
            }
            return ret;
        }

        // case like "三月的第一周"
        private DTParseResult ParseWeekOfMonth(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var trimedText = text.Trim().ToLowerInvariant();
            var match = DatePeriodExtractorChs.WeekOfMonthRegex.Match(text);
            if (!match.Success)
            {
                return ret;
            }

            var cardinalStr = match.Groups["cardinal"].Value;
            var monthStr = match.Groups["month"].Value;
            var noYear = false;
            int year;

            int cardinal;
            if (cardinalStr.Equals("最后一"))
            {
                cardinal = 5;
            }
            else
            {
                cardinal = this.config.CardinalMap[cardinalStr];
            }
            int month;
            if (string.IsNullOrEmpty(monthStr))
            {
                var swift = 0;
                if (trimedText.StartsWith("下个"))
                {
                    swift = 1;
                }
                else if (trimedText.StartsWith("上个"))
                {
                    swift = -1;
                }
                month = referenceDate.AddMonths(swift).Month;
                year = referenceDate.AddMonths(swift).Year;
                ret.Timex = referenceDate.Year.ToString("D4") + "-" + month.ToString("D2");
            }
            else
            {
                month = this.config.MonthOfYear[monthStr];
                ret.Timex = "XXXX" + "-" + month.ToString("D2");
                year = referenceDate.Year;
                noYear = true;
            }

            var value = ComputeDate(cardinal, 1, month, year);

            var futureDate = value;
            var pastDate = value;
            if (noYear && futureDate < referenceDate)
            {
                futureDate = ComputeDate(cardinal, 1, month, year + 1);
                if (futureDate.Month != month)
                {
                    futureDate = futureDate.AddDays(-7);
                }
            }
            if (noYear && pastDate >= referenceDate)
            {
                pastDate = ComputeDate(cardinal, 1, month, year - 1);
                if (pastDate.Month != month)
                {
                    pastDate = pastDate.AddDays(-7);
                }
            }

            ret.Timex += "-W" + cardinal.ToString("D2");
            ret.FutureValue = new Tuple<DateObject, DateObject>(futureDate, futureDate.AddDays(7));
            ret.PastValue = new Tuple<DateObject, DateObject>(pastDate, pastDate.AddDays(7));
            ret.Success = true;

            return ret;
        }

        // parse "今年夏天"
        private DTParseResult ParseSeason(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var match = DatePeriodExtractorChs.SeasonWithYear.Match(text);
            if (match.Success && match.Length == text.Length)
            {
                // pare year 
                var year = referenceDate.Year;
                var hasYear = false;
                var yearNum = match.Groups["year"].Value;
                var yearChs = match.Groups["yearchs"].Value;
                var yearRel = match.Groups["yearrel"].Value;
                if (!string.IsNullOrEmpty(yearNum))
                {
                    hasYear = true;
                    if (yearNum.EndsWith("年"))
                    {
                        yearNum = yearNum.Substring(0, yearNum.Length - 1);
                    }
                    year = int.Parse(yearNum);
                }
                else if (!string.IsNullOrEmpty(yearChs))
                {
                    hasYear = true;
                    if (yearChs.EndsWith("年"))
                    {
                        yearChs = yearChs.Substring(0, yearChs.Length - 1);
                    }
                    year = ConvertChineseToInteger(yearChs);
                }
                else if (!string.IsNullOrEmpty(yearRel))
                {
                    hasYear = true;
                    if (yearRel.EndsWith("去年"))
                    {
                        year--;
                    }
                    else if (yearRel.EndsWith("明年"))
                    {
                        year++;
                    }
                }

                if (year < 100 && year >= 90)
                {
                    year += 1900;
                }
                else if (year < 100 && year < 20)
                {
                    year += 2000;
                }

                // parse season
                var seasonStr = match.Groups["season"].Value;
                ret.Timex = this.config.SeasonMap[seasonStr];
                if (hasYear)
                {
                    ret.Timex = year.ToString("D4") + "-" + ret.Timex;
                }
                ret.Success = true;
                return ret;
            }
            return ret;
        }

        private DTParseResult ParseQuarter(string text, DateObject referenceDate)
        {
            var ret = new DTParseResult();
            var match = DatePeriodExtractorChs.QuarterRegex.Match(text);
            if (!(match.Success && match.Length == text.Length))
            {
                return ret;
            }

            // pare year 
            var year = referenceDate.Year;
            var yearNum = match.Groups["year"].Value;
            var yearChs = match.Groups["yearchs"].Value;
            var yearRel = match.Groups["yearrel"].Value;
            if (!string.IsNullOrEmpty(yearNum))
            {
                if (yearNum.EndsWith("年"))
                {
                    yearNum = yearNum.Substring(0, yearNum.Length - 1);
                }
                year = int.Parse(yearNum);
            }
            else if (!string.IsNullOrEmpty(yearChs))
            {
                if (yearChs.EndsWith("年"))
                {
                    yearChs = yearChs.Substring(0, yearChs.Length - 1);
                }
                year = ConvertChineseToInteger(yearChs);
            }
            else if (!string.IsNullOrEmpty(yearRel))
            {
                if (yearRel.EndsWith("去年"))
                {
                    year--;
                }
                else if (yearRel.EndsWith("明年"))
                {
                    year++;
                }
            }

            if (year < 100 && year >= 90)
            {
                year += 1900;
            }
            else if (year < 100 && year < 20)
            {
                year += 2000;
            }

            // parse quarterNum
            var cardinalStr = match.Groups["cardinal"].Value;
            var quarterNum = this.config.CardinalMap[cardinalStr];

            var beginDate = new DateObject(year, quarterNum*3 - 2, 1);
            var endDate = new DateObject(year, quarterNum*3 + 1, 1);
            ret.FutureValue = ret.PastValue = new Tuple<DateObject, DateObject>(beginDate, endDate);
            ret.Timex = $"({Util.LuisDate(beginDate)},{Util.LuisDate(endDate)},P3M)";
            ret.Success = true;

            return ret;
        }

        private static DateObject ComputeDate(int cadinal, int weekday, int month, int year)
        {
            var firstDay = new DateObject(year, month, 1);
            var firstWeekday = firstDay.This((DayOfWeek) weekday);
            if (weekday == 0)
            {
                weekday = 7;
            }
            if (weekday < (int) firstDay.DayOfWeek)
            {
                firstWeekday = firstDay.Next((DayOfWeek) weekday);
            }
            return firstWeekday.AddDays(7*(cadinal - 1));
        }
    }
}