using System.Text;

namespace CHERRY.UI.Models
{
    public class MorseCodeConverter
    {
        private static readonly Dictionary<char, string> MorseCodeDictionary = new Dictionary<char, string>
        {
            { 'A', ".-" }, { 'B', "-..." }, { 'C', "-.-." }, { 'D', "-.." },
            { 'E', "." }, { 'F', "..-." }, { 'G', "--." }, { 'H', "...." },
            { 'I', ".." }, { 'J', ".---" }, { 'K', "-.-" }, { 'L', ".-.." },
            { 'M', "--" }, { 'N', "-." }, { 'O', "---" }, { 'P', ".--." },
            { 'Q', "--.-" }, { 'R', ".-." }, { 'S', "..." }, { 'T', "-" },
            { 'U', "..-" }, { 'V', "...-" }, { 'W', ".--" }, { 'X', "-..-" },
            { 'Y', "-.--" }, { 'Z', "--.." },
            { '0', "-----" }, { '1', ".----" }, { '2', "..---" },
            { '3', "...--" }, { '4', "....-" }, { '5', "....." },
            { '6', "-...." }, { '7', "--..." }, { '8', "---.." },
            { '9', "----." },
            { ' ', "/" } 
        };

        public static string ToMorseCode(string input)
        {
            StringBuilder morseCode = new StringBuilder();

            foreach (char character in input.ToUpper())
            {
                if (MorseCodeDictionary.TryGetValue(character, out string morseChar))
                {
                    morseCode.Append(morseChar + " ");
                }
            }

            return morseCode.ToString().Trim();
        }
    }
}
