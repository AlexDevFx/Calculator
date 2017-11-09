using System;
using Solver;

namespace Calculator
{
    class Program
    {
        static void Main(string[] args)
        {
            string mathExpression = Console.ReadLine();
            OperatorsList<double> operators = new OperatorsList<double>();

            operators.Add('+', new DoubleAdditionOperator(OperatorsPriority.Low));
            operators.Add('-', new DoubleSubstractOperator(OperatorsPriority.Low));
            operators.Add('*', new DoubleMultiplicationOperator(OperatorsPriority.Medium));
            operators.Add('/', new DoubleDivisionOperator(OperatorsPriority.Medium));

            ExpressionCalculator calculator = new ExpressionCalculator(operators);
            Console.WriteLine(calculator.Solve(mathExpression));
            Console.ReadKey();
        }

    }
}
