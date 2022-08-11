using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    internal class classSearchVerify
    {
        bool isWordGiven;
        String rootWord;

        public bool IsWordGiven { get => isWordGiven; set => isWordGiven = value; }
        public string RootWord { get => rootWord; set => rootWord = value; }
    }
}
