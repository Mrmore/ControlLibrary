using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ControlLibrary.Tools
{
    public class JudgmentCharacter
    {
        private static Regex regex = new Regex(@"([\u0e01-\u0e5f]+)", RegexOptions.Multiline);
        public static string JudgmentString(string content)
        {
            if (regex.IsMatch(content))
            {
                var matches = regex.Matches(content);
                for (int i = 0; i < matches.Count; i++)
                {
                    var match = regex.Match(content);
                    content = content.Remove(match.Index, match.Length - 1);
                }
            }
            return content;
        }
    }
}
