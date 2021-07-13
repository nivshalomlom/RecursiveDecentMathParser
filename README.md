# RecursiveDecentMathParser
A recursive decent praser to read and compute mathematical expressions, according to the defined rules and tokens.

## Parser rules and tokens
Rules(non-terminals) are defined as such:

1. start: expression
2. expression: term op expression <br />
             | term <br />
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

tokens(terminals) are defined as such:

* NUMBER - @"\d+" 
* OPERATOR - @"\\+|\\-|\\*|/|\\^"
* FUNCTION - @"sin|asin|cos|acos|tan|atan|sqrt|ln|log"
* LEFT_BRACKETS - @"\\("
* RIGHT_BRACKETS - @"\\)"
* DOT - @"\\."
* SPACES - @"\s*"

**note**: the tokens are defined in the c#(.net) regex format

## Exmaple
code:
```c#
 Console.WriteLine(MathParser.parse("5 + 4 * 5");
 ```
 output:
```bash
25
```

## How does it work?
lets break down the previous example for the input "5 + 4 * 5"

| current rule | current input | token matched | toke value | current output | previous operator | previous number | computation done |
| ------------ | ------------- | ------------- | ---------- | -------------- | ----------------- | --------------- | ---------------- |
| start        | 5 + 4 * 5     |               |            | 0              |                   |                 |                  |
| expression   | 5 + 4 * 5     |               |            | 0              |                   |                 |                  |
| term         | 5 + 4 * 5     |               |            | 0              |                   |                 |                  |
| number       | 5 + 4 * 5     |               |            | 0              |                   |                 |                  |
| number       | + 4 * 5       | NUMBER        | 5          | 0              |                   |                 | 0 + 5            |
| expression   | + 4 * 5       |               |            | 5              |                   |                 |                  |
| op           | + 4 * 5       |               |            | 5              |                   |                 |                  |
| op           | 4 * 5         | OPERATOR      | +          | 5              | +                 | 5               |                  |
| expression   | 4 * 5         |               |            | 5              | +                 | 5               |                  |
| term         | 4 * 5         |               |            | 5              | +                 | 5               |                  |
| number       | 4 * 5         |               |            | 5              | +                 | 5               |                  |
| number       | * 5           | NUMBER        | 4          | 5              | +                 | 4               |                  |
| expression   | * 5           |               |            | 5              | +                 | 4               |                  |
| op           | 5             |               |            | 5              | *                 | 4               |                  |
| expression   | 5             |               |            | 5              | *                 | 4               |                  |
| number       | 5             |               |            | 5              | *                 | 4               |                  |
| number       |               | NUMBER        | 5          | 5              | *                 | 4               | 4 * 5            |
| expression   |               |               |            | 25             | *                 | 4               | 5 + 20           |

table values explained:
* current rule - the rule the parser is looking at.
* current input - whats left of the input given to the parser.
* token matched - if a token was matched in this line this is its name.
* token value - if a token was matched in this line this is the matched text value.
* current ouput - the current output the parser has computed.
* previous operator - the last matched operator.
* previous number - the last matched number.
* computation done - shows any computations and parser has done in a given line.

**note**: you can see that some values and operators are kept and used later, its to keep the right order of operations(multiplicatio/divison and then addition/subtraction), for more information check the source code.
