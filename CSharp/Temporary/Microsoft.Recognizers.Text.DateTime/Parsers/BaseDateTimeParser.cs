﻿using Microsoft.Recognizers.Text.DateTime.Utilities;
using System.Collections.Generic;
using DateObject = System.DateTime;

namespace Microsoft.Recognizers.Text.DateTime.Parsers
{
    public class BaseDateTimeParser : IDateTimeParser
    {
        public static readonly string ParserName = Constants.SYS_DATETIME_DATETIME; // "DateTime";
        
        private readonly IDateTimeParserConfiguration config;

        public BaseDateTimeParser(IDateTimeParserConfiguration configuration)
        {
            config = configuration;
        }

        public ParseResult Parse(ExtractResult result)
        {
            return this.Parse(result, DateObject.Now);
        }

        public DateTimeParseResult Parse(ExtractResult er, DateObject refTime)
        {
            var referenceTime = refTime;

            object value = null;
            if (er.Type.Equals(ParserName))
            {
                var innerResult = MergeDateAndTime(er.Text, referenceTime);
                if (!innerResult.Success)
                {
                    innerResult = ParseBasicRegex(er.Text, referenceTime);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseTimeOfToday(er.Text, referenceTime);
                }
                if (!innerResult.Success)
                {
                    innerResult = ParseSpecailTimeOfDate(er.Text, referenceTime);
                }

                if (innerResult.Success)
                {
                    innerResult.FutureResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATETIME, Util.FormatDateTime((DateObject) innerResult.FutureValue)}
                    };
                    innerResult.PastResolution = new Dictionary<string, string>
                    {
                        {TimeTypeConstants.DATETIME, Util.FormatDateTime((DateObject) innerResult.PastValue)}
                    };
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

        private DTParseResult ParseBasicRegex(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var trimedText = text.Trim().ToLower();

            // handle "now"
            var match = this.config.NowRegex.Match(trimedText);
            if (match.Success && match.Index == 0 && match.Length == trimedText.Length)
            {
                var timex = "";
                this.config.GetMatchedNowTimex(trimedText, out timex);
                ret.Timex = timex;
                ret.FutureValue = ret.PastValue = referenceTime;
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        // merge a Date entity and a Time entity
        private DTParseResult MergeDateAndTime(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();

            var er1 = this.config.DateExtractor.Extract(text);
            if (er1.Count == 0)
            {
                er1 = this.config.DateExtractor.Extract(this.config.TokenBeforeDate + text);
                if (er1.Count == 1)
                {
                    er1[0].Start -= this.config.TokenBeforeDate.Length;
                }
                else
                {
                    return ret;
                }
            }
            else
            {
                // this is to understand if there is an ambiguous token in the text. For some languages (e.g. spanish)
                // the same word could mean different things (e.g a time in the day or an specific day).
                if (this.config.HaveAmbiguousToken(text, er1[0].Text))
                {
                    return ret;
                }
            }

            var er2 = this.config.TimeExtractor.Extract(text);
            if (er2.Count == 0)
            {
                // here we filter out "morning, afternoon, night..." time entities
                er2 = this.config.TimeExtractor.Extract(this.config.TokenBeforeTime + text);
                if (er2.Count == 1)
                {
                    er2[0].Start -= this.config.TokenBeforeTime.Length;
                }
                else
                {
                    return ret;
                }
            }

            // handle case "Oct. 5 in the afternoon at 7:00"
            // in this case "5 in the afternoon" will be extract as a Time entity
            var correctTimeIdx = 0;
            while (correctTimeIdx < er2.Count && er2[correctTimeIdx].IsOverlap(er1[0]))
            {
                correctTimeIdx++;
            }
            if (correctTimeIdx >= er2.Count)
            {
                return ret;
            }

            var pr1 = this.config.DateParser.Parse(er1[0], referenceTime.Date);
            var pr2 = this.config.TimeParser.Parse(er2[correctTimeIdx], referenceTime);
            if (pr1.Value == null || pr2.Value == null)
            {
                return ret;
            }

            var futureDate = (DateObject) ((DTParseResult) pr1.Value).FutureValue;
            var pastDate = (DateObject) ((DTParseResult) pr1.Value).PastValue;
            var time = (DateObject) ((DTParseResult) pr2.Value).FutureValue;

            var hour = time.Hour;
            var min = time.Minute;
            var sec = time.Second;

            // handle morning, afternoon
            if (this.config.PMTimeRegex.IsMatch(text) && hour < 12)
            {
                hour += 12;
            }
            else if (this.config.AMTimeRegex.IsMatch(text) && hour >= 12)
            {
                hour -= 12;
            }

            var timeStr = pr2.TimexStr;
            if (timeStr.EndsWith("ampm"))
            {
                timeStr = timeStr.Substring(0, timeStr.Length - 4);
            }
            timeStr = "T" + hour.ToString("D2") + timeStr.Substring(3);
            ret.Timex = pr1.TimexStr + timeStr;

            var val = (DTParseResult) pr2.Value;
            if (hour <= 12 && !this.config.PMTimeRegex.IsMatch(text) && !this.config.AMTimeRegex.IsMatch(text) &&
                !string.IsNullOrEmpty(val.comment))
            {
                //ret.Timex += "ampm";
                ret.comment = "ampm";
            }
            ret.FutureValue = new DateObject(futureDate.Year, futureDate.Month, futureDate.Day, hour, min, sec);
            ret.PastValue = new DateObject(pastDate.Year, pastDate.Month, pastDate.Day, hour, min, sec);
            ret.Success = true;

            return ret;
        }

        private DTParseResult ParseTimeOfToday(string text, DateObject referenceTime)
        {
            var ret = new DTParseResult();
            var trimedText = text.ToLowerInvariant().Trim();

            int hour = 0, min = 0, sec = 0;
            string timeStr = string.Empty;

            var wholeMatch = this.config.SimpleTimeOfTodayAfterRegex.Match(trimedText);
            if (!(wholeMatch.Success && wholeMatch.Length == trimedText.Length))
                wholeMatch = this.config.SimpleTimeOfTodayBeforeRegex.Match(trimedText);
            if (wholeMatch.Success && wholeMatch.Length == trimedText.Length)
            {
                var hourStr = wholeMatch.Groups["hour"].Value;
                if (string.IsNullOrEmpty(hourStr))
                {
                    hourStr = wholeMatch.Groups["hournum"].Value.ToLower();
                    hour = this.config.Numbers[hourStr];
                }
                else
                {
                    hour = int.Parse(hourStr);
                }
                timeStr = "T" + hour.ToString("D2");
            }
            else
            {
                var ers = this.config.TimeExtractor.Extract(trimedText);
                if (ers.Count != 1)
                {
                    ers = this.config.TimeExtractor.Extract(this.config.TokenBeforeTime + trimedText);
                    if (ers.Count == 1)
                    {
                        ers[0].Start -= this.config.TokenBeforeTime.Length;
                    }
                    else
                    {
                        return ret;
                    }
                }

                var pr = this.config.TimeParser.Parse(ers[0], referenceTime);
                if (pr.Value == null)
                {
                    return ret;
                }

                var time = (DateObject) ((DTParseResult) pr.Value).FutureValue;

                hour = time.Hour;
                min = time.Minute;
                sec = time.Second;
                timeStr = pr.TimexStr;
            }


            var match = this.config.SpecificNightRegex.Match(trimedText);

            if (match.Success)
            {
                var matchStr = match.Value.ToLowerInvariant();

                // handle "last", "next"
                var swift = this.config.GetSwiftDay(matchStr);

                var date = referenceTime.AddDays(swift).Date;

                // handle "morning", "afternoon"
                hour = this.config.GetHour(matchStr, hour);

                // in this situation, luisStr cannot end up with "ampm", because we always have a "morning" or "night"
                if (timeStr.EndsWith("ampm"))
                {
                    timeStr = timeStr.Substring(0, timeStr.Length - 4);
                }
                timeStr = "T" + hour.ToString("D2") + timeStr.Substring(3);

                ret.Timex = Util.FormatDate(date) + timeStr;
                ret.FutureValue = ret.PastValue = new DateObject(date.Year, date.Month, date.Day, hour, min, sec);
                ret.Success = true;
                return ret;
            }

            return ret;
        }

        private DTParseResult ParseSpecailTimeOfDate(string text, DateObject refeDateTime)
        {
            var ret = new DTParseResult();
            var ers = this.config.DateExtractor.Extract(text);
            if (ers.Count != 1)
            {
                return ret;
            }
            var beforeStr = text.Substring(0, ers[0].Start ?? 0);
            if (this.config.TheEndOfRegex.IsMatch(beforeStr))
            {
                var pr = this.config.DateParser.Parse(ers[0], refeDateTime);
                var futureDate = (DateObject) ((DTParseResult) pr.Value).FutureValue;
                var pastDate = (DateObject) ((DTParseResult) pr.Value).PastValue;
                ret.Timex = pr.TimexStr + "T23:59";
                ret.FutureValue = futureDate.AddDays(1).AddMinutes(-1);
                ret.PastValue = pastDate.AddDays(1).AddMinutes(-1);
                ret.Success = true;
                return ret;
            }
            return ret;
        }
    }
}