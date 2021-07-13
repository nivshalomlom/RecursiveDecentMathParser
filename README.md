# RecursiveDecentMathParser
A recursive decent praser to read and compute mathematical expressions.

This parser reads expressions with the following rules:

1. start: expression
2. expression: term op expression
             | term
             | /* empty */
3. term: LEFT_BRACKETS expression RIGHT_BRACKETS
       | number
       | function
4. function: FUNCTION LEFT_BRACKETS number LEFT_BRACKETS
           | "log" log
5. number: NUMBER DOT NUMBER
         | NUMBER
6. log: number LEFT_BRACKETS number RIGHT_BRACKETS
      | LEFT_BRACKETS number RIGHT_BRACKETS
7. op: OPERATOR

tokens(non-terminals) are defined as such:

NUMBER - @"\d+" 
OPERATOR - @"\+|\-|\*|/|\^"
FUNCTION - @"sin|asin|cos|acos|tan|atan|sqrt|ln|log"
LEFT_BRACKETS - @"\("
RIGHT_BRACKETS - @"\)"
DOT - @"\."
SPACES - @"\s*"

note: the tokens are defined in the c#(.net) regex format
