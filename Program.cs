using System;

namespace RecursiveDecentMathParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4 ^ 2 + log10(100) + 5 *4 + 5 = " 
                + MathParser.parse("4 ^ 2 + log10(100) + 5 *4 + 5"));
        }
    }
}
