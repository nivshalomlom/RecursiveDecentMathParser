using System;
using System.Collections.Generic;

namespace RecursiveDecentMathParser
{
    class Program
    {
        static void Main(string[] args)
        {
            // define variable dictionary
            Dictionary<string, double> dict = new Dictionary<string, double>()
            {
                {"x", 6 },
                {"y", 8 }
            };

            // parse x + 2 * y
            Console.WriteLine(MathParser.parse("x + 2 * y", dict));

        }
    }
}
