﻿using System.Globalization;
using Microsoft.Recognizers.Text.Number;
using Microsoft.Recognizers.Text.Number.Spanish;

namespace Microsoft.Recognizers.Text.NumberWithUnit.Spanish
{
    public class SpanishNumberWithUnitParserConfiguration : BaseNumberWithUnitParserConfiguration
    {
        public SpanishNumberWithUnitParserConfiguration(CultureInfo ci) : base(ci)
        {
            this.InternalNumberExtractor = new NumberExtractor(NumberMode.Default);
            this.InternalNumberParser = AgnosticNumberParserFactory.GetParser(AgnosticNumberParserType.Number, new SpanishNumberParserConfiguration());
            this.ConnectorToken = "de";
        }

        public override IParser InternalNumberParser { get; }

        public override IExtractor InternalNumberExtractor { get; }

        public override string ConnectorToken { get; }
    }
}
