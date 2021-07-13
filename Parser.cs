using System;
using System.Text.RegularExpressions;

namespace RecursiveDecentMathParser
{
    /// <summary>
    /// A genric parser for string processing 
    /// </summary>
    class Parser
    {

        // the text being parsed
        private string input;

        /// <summary>
        /// A constructor to build a new parser
        /// </summary>
        /// <param name="input"> the text to be parsed </param>
        public Parser(string input)
        {
            this.input = input;
        }

        /// <summary>
        /// A method to try and match the start of a string to given regex, <br></br>
        /// if successfull the parser moves to the location of the first character after the matched string 
        /// </summary>
        /// <param name="regex"> the pattern that will be used for matching </param>
        /// <returns> the matched string, if nothing matched returns a empty string </returns>
        public string match(string regex)
        {
            Regex rx = new Regex(regex);
            MatchCollection mc = rx.Matches(this.input);
            if (mc.Count > 0 && mc[0].Index == 0)
            {
                this.input = this.input.Substring(mc[0].Value.Length);
                return mc[0].Value;
            }
            return "";
        }

        /// <summary>
        /// A method to peek at text at the current postion of the parser
        /// </summary>
        /// <param name="size"> how much characters to return </param>
        /// <returns> the specified amount of characters from the current poistion of the praser in the text </returns>
        public string getLookAHead(int size)
        {
            if (this.input.Length < size)
                return this.input;
            return this.input.Substring(0, size);
        }

        /// <summary>
        /// Returns the first charcter in the remaining input
        /// </summary>
        /// <returns> the first charcter in the remaining input </returns>
        public char getFirstCharcter()
        {
            return this.input[0];
        }

        /// <summary>
        /// A method to check how many characters are left for the praser to read
        /// </summary>
        /// <returns> the amount of characters remaining </returns>
        public int remainingCharcters()
        {
            return this.input.Length;
        }

        /// <summary>
        /// A method to read a input text from a file
        /// </summary>
        /// <param name="filePath"> the path of the file </param>
        /// <returns> a new parser looking at the start of the text </returns>
        public static Parser readFromFile(string filePath)
        {
            string text = System.IO.File.ReadAllText(filePath);
            return new Parser(text);
        }

    }
}
