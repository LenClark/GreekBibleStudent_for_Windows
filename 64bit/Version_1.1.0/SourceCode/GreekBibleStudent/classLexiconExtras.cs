using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreekBibleStudent
{
    internal class classLexiconExtras
    {
        SortedSet<String> setOfKeys = new SortedSet<string>();

        public SortedSet<string> SetOfKeys { get => setOfKeys; set => setOfKeys = value; }

        public void addAKey(String keyValue)
        {
            if (!setOfKeys.Contains(keyValue))
            {
                setOfKeys.Add(keyValue);
            }
        }
    }
}
