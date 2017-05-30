﻿using System.Collections.Generic;

namespace Microsoft.Recognizers.Text
{
    public interface IModel
    {
        string ModelTypeName { get; }

        List<ModelResult> Parse(string query);
    }

    public class ModelResult
    {
        public string Text { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public string TypeName { get; set; }
        //Resolution field
        public SortedDictionary<string, object> Resolution { get; set; }
    }
}