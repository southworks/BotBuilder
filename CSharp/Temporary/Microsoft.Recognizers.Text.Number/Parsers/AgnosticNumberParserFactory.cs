﻿using Microsoft.Recognizers.Text.Number.Chinese.Parsers;
using Microsoft.Recognizers.Text.Number.Utilities;
using System.Collections.Generic;

namespace Microsoft.Recognizers.Text.Number.Parsers
{
    public enum AgnosticNumberParserType
    {
        Cardinal,
        Double,
        Fraction,
        Integer,
        Number,
        Ordinal,
        Percentage
    }

    public static class AgnosticNumberParserFactory
    {
        public static BaseNumberParser GetParser(AgnosticNumberParserType type, INumberParserConfiguration languageConfiguration)
        {
            var isChinese = languageConfiguration.CultureInfo.Name.ToLowerInvariant() == Culture.Chinese;

            BaseNumberParser parser;

            if (isChinese)
            {
                parser = new ChineseNumberParser(languageConfiguration as ChineseNumberParserConfiguration);
            }
            else
            {
                parser = new BaseNumberParser(languageConfiguration);
            }

            switch (type)
            {
                case AgnosticNumberParserType.Cardinal:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_CARDINAL, Constants.SYS_NUM_INTEGER, Constants.SYS_NUM_DOUBLE };
                    break;
                case AgnosticNumberParserType.Double:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_DOUBLE };
                    break;
                case AgnosticNumberParserType.Fraction:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_FRACTION };
                    break;
                case AgnosticNumberParserType.Integer:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_INTEGER };
                    break;
                case AgnosticNumberParserType.Ordinal:
                    parser.SupportedTypes = new List<string> { Constants.SYS_NUM_ORDINAL };
                    break;
                case AgnosticNumberParserType.Percentage:
                    if (!isChinese)
                    {
                        parser = new BasePercentageParser(languageConfiguration);
                    }
                    break;
            }

            return parser;
        }
    }
}