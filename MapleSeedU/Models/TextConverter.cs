// Project: MapleSeedU
// File: TextConverter.cs
// Updated By: Jared
// 

#region usings

using System;

#endregion

namespace MapleSeedU.Models
{
    public class TextConverter
    {
        private readonly Func<string, string> _convertion;

        public TextConverter(Func<string, string> convertion)
        {
            _convertion = convertion;
        }

        public string ConvertText(string inputText)
        {
            return _convertion(inputText);
        }
    }
}