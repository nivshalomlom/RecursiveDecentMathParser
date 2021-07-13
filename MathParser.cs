using System;
using System.Collections.Generic;

namespace RecursiveDecentMathParser
{
    /// <summary>
    /// A reacusive decent parser to read and compute mathamtical expressions
    /// </summary>
    class MathParser
    {

        // Terminals recognized by the parser
        enum TOKEN
        {
            NUMBER,
            OPERATOR,
            FUNCTION,
            LEFT_BRACKETS,
            RIGHT_BRACKETS,
            DOT,
            SPACES
        }

        private static Dictionary<TOKEN, string> tokenRegexes = new Dictionary<TOKEN, string>()
        {
            // Matchamtical terms
            { TOKEN.NUMBER, @"\d+" },
            { TOKEN.OPERATOR, @"\+|\-|\*|/|\^" },
            { TOKEN.FUNCTION, @"sin|asin|cos|acos|tan|atan|sqrt|ln|log" },
            // Special charcters
            { TOKEN.LEFT_BRACKETS, @"\(" },
            { TOKEN.RIGHT_BRACKETS, @"\)" },
            { TOKEN.SPACES, @"\s*" },
            { TOKEN.DOT, @"\." }
        };

        // Operators with high precedence
        private static char[] urgentOperators = { '*', '/', '^' };

        // The parser that handles the text
        private static Parser input;

        // A method to match a token with the parser
        private static string match(TOKEN token)
        {
            string regex;
            if (tokenRegexes.TryGetValue(token, out regex))
                return input.match(regex);
            else throw new Exception("MathParser: error unknowen token");
        }

        // A method to check whether a operator is urgent(has high precedence)
        private static bool isOperatorUrgent(char oper)
        {
            foreach (char urgentOp in urgentOperators)
                if (urgentOp == oper)
                    return true;
            return false;
        }

        // A simple method for computing the binary operation of 2 numbers
        private static double computeOperation(double num1, char oper, double num2)
        {
            switch (oper)
            {
                case '+':
                    return num1 + num2;
                case '-':
                    return num1 - num2;
                case '*':
                    return num1 * num2;
                case '/':
                    return num1 / num2;
                case '^':
                    return Math.Pow(num1, num2);
                default:
                    throw new Exception("MathParser: error unknown operator " + oper);
            }
        }

        /* 
         * op: OPERATOR
         */
        private static char op()
        {
            match(TOKEN.SPACES);
            string output = match(TOKEN.OPERATOR);
            return output[0];
        }

        /*
         * number: NUMBER DOT NUMBER
         *       | NUMBER   
         */
        private static double number()
        {
            match(TOKEN.SPACES);
            string num = match(TOKEN.NUMBER);
            // if its a fraction
            if (input.remainingCharcters() > 1 && input.getLookAHead(1)[0] == '.')
            {
                match(TOKEN.DOT);
                num += '.' + match(TOKEN.NUMBER);
            }
            return float.Parse(num);
        }

        /* 
         * log: number LEFT_BRACKETS number RIGHT_BRACKETS
         *    | LEFT_BRACKETS number RIGHT_BRACKETS
         */
        private static double log()
        {
            // get base number if defined
            char lookahead = input.getFirstCharcter();
            double baseNum = lookahead == '(' ? 10 : number();
            match(TOKEN.LEFT_BRACKETS);
            double num = number();
            match(TOKEN.RIGHT_BRACKETS);
            // compute and return
            if (baseNum == 10)
                return Math.Log10(num);
            return Math.Log(num) / Math.Log(baseNum);
        }

        /* 
        * function: FUNCTION LEFT_BRACKETS number LEFT_BRACKETS
        *         | "log" log
        */
        private static double function()
        {
            string funcName = match(TOKEN.FUNCTION);
            if (funcName.Equals("log"))
                return log();
            else
            {
                match(TOKEN.LEFT_BRACKETS);
                double num = number();
                match(TOKEN.LEFT_BRACKETS);
                switch (funcName)
                {
                    case "sin":
                        return Math.Sin(num);
                    case "asin":
                        return Math.Asin(num);
                    case "cos":
                        return Math.Cos(num);
                    case "acos":
                        return Math.Acos(num);
                    case "tan":
                        return Math.Tan(num);
                    case "atan":
                        return Math.Atan(num);
                    case "sqrt":
                        return Math.Sqrt(num);
                    case "ln":
                        return Math.Log(num);
                    default:
                        throw new Exception("MathParser: error unknown function " + funcName);
                }
            }
        }

        /*
        * term: LEFT_BRACKETS expression RIGHT_BRACKETS
        *     | number
        *     | function
         */
        private static double term()
        {
            char lookahead = input.getFirstCharcter();
            if (lookahead == '(')
            {
                match(TOKEN.LEFT_BRACKETS);
                double num = expression(0, ' ');
                match(TOKEN.RIGHT_BRACKETS);
                return num;
            }
            else if (char.IsDigit(lookahead))
                return number();
            else return function();
        }

        /* 
        * expression: term op expression
        *           | term
        *           | // empty
        */
        private static double expression(double prevValue, char prevOper)
        {
            match(TOKEN.SPACES);
            if (input.remainingCharcters() == 0)
                return prevValue;
            double num1 = term();
            // Handle urgent operator from previous step if needed
            if (isOperatorUrgent(prevOper))
                num1 = computeOperation(prevValue, prevOper, num1);
            // if the number read was the end of a expression
            if (input.remainingCharcters() == 0 || input.getFirstCharcter() == ')')
                return num1;
            char oper = op();
            if (isOperatorUrgent(oper))
                // if operator is urgent pass it to next step where it will be handled
                return expression(num1, oper);
            else
            {
                double num2 = expression(num1, oper);
                return computeOperation(num1, oper, num2);
            }
        }

        /// <summary>
        /// A method to compute mathamatical expressions
        /// </summary>
        /// <param name="exp"> a string representation of a mathamtical expression </param>
        /// <returns> the expression's double value </returns>
        public static double parse(string exp)
        {
            input = new Parser(exp);
            return expression(0, ' ');
        }

    }
}
