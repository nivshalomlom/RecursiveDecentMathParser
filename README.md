# RecursiveDecentMathParser
A recursive decent praser to read and compute mathematical expressions, according to the defined rules and tokens.

## Parser rules and tokens
Rules(non-terminals) are defined as such

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

tokens(terminals) are defined as such:

* NUMBER - @"\d+" 
* OPERATOR - @"\\+|\\-|\\*|/|\\^"
* FUNCTION - @"sin|asin|cos|acos|tan|atan|sqrt|ln|log"
* LEFT_BRACKETS - @"\\("
* RIGHT_BRACKETS - @"\\)"
* DOT - @"\\."
* SPACES - @"\s*"

note: the tokens are defined in the c#(.net) regex format

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
| expression   |               | NUMBER        | 5          | 25             | *                 | 4               | 5 + 20           |
