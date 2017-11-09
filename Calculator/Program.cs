using System;
using Solver;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string mathExpression = Console.ReadLine();
            ExpressionCalculator calculator = new ExpressionCalculator();
            Console.WriteLine(calculator.ConvertToRpn(mathExpression));
            Console.WriteLine(calculator.Solve(mathExpression));
            Console.ReadKey();
        }

    }
}
