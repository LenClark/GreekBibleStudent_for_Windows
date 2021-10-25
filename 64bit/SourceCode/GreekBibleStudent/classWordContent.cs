using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    public class classWordContent
    {
        String catString, parseString, uniqueValue, textWord, accentlessTextWord, bareTextWord, punctuation, preWordChars, postWordChars, rootWord;

        public string CatString { get => catString; set => catString = value; }
        public string ParseString { get => parseString; set => parseString = value; }
        public string UniqueValue { get => uniqueValue; set => uniqueValue = value; }
        public string TextWord { get => textWord; set => textWord = value; }
        public string AccentlessTextWord { get => accentlessTextWord; set => accentlessTextWord = value; }
        public string BareTextWord { get => bareTextWord; set => bareTextWord = value; }
        public string Punctuation { get => punctuation; set => punctuation = value; }
        public string PreWordChars { get => preWordChars; set => preWordChars = value; }
        public string PostWordChars { get => postWordChars; set => postWordChars = value; }
        public string RootWord { get => rootWord; set => rootWord = value; }
    }
}
