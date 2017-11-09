using System;
using System.Collections.Generic;
using System.Text;

namespace Solver
{
#region Operators
    public enum OperatorsPriority { High = 3, Medium = 2, Low = 1 };

    public interface IOperator<T>
    {
        T Calculate(T first, T second);
        OperatorsPriority Priority { get; }
    }

    public abstract class DoubleAbstractOperation : IOperator<double>
    {
        public OperatorsPriority Priority { get; }

        public DoubleAbstractOperation(OperatorsPriority priority) => Priority = priority;

        public abstract double Calculate(double first, double second);

    }

    public class DoubleAdditionOperator : DoubleAbstractOperation
    {
        public DoubleAdditionOperator(OperatorsPriority priority): base(priority) { }

        public override double Calculate(double first, double second) => first + second;
    }

    public class DoubleSubstractOperator : DoubleAbstractOperation
    {
        public DoubleSubstractOperator(OperatorsPriority priority) : base(priority) { }

        public override double Calculate(double first, double second) => first - second;
    }

    public class DoubleMultiplicationOperator : DoubleAbstractOperation
    {
        public DoubleMultiplicationOperator(OperatorsPriority priority) : base(priority) { }

        public override double Calculate(double first, double second) => first * second;
    }

    public class DoubleDivisionOperator : DoubleAbstractOperation
    {
        public DoubleDivisionOperator(OperatorsPriority priority) : base(priority) { }

        public override double Calculate(double first, double second)
        {
            if (double.Equals(second, 0.0D))
                throw new DivideByZeroException();
            return first / second;
        }
    }

    public interface IOperatorsList<T>
    {
        void Add(char symbol, IOperator<T> new_operator);
        bool IsOperator(char c);
        IOperator<T> GetOperator(char c);
    }

    public class OperatorsList<T>: IOperatorsList<T>
    {
        private Dictionary<char, IOperator<T>> _operators = new Dictionary<char, IOperator<T>>();

        public void Add(char symbol, IOperator<T> new_operator)
        {
            _operators.Add(symbol, new_operator);
        }

        public bool IsOperator(char c) => _operators.ContainsKey(c);

        public IOperator<T> GetOperator(char c)
        {
            return _operators[c];
        }
    }
    #endregion Operators

#region Calculators

    public interface IExpressionCalculator<T>
    {
        T Solve(string expression);
    }

    public class ExpressionCalculator: IExpressionCalculator<double>
    {
        private IOperatorsList<double> _operators;

        public ExpressionCalculator(IOperatorsList<double> new_operators)
        {
            _operators = new_operators;
        }

        private string CleanExpression(string input)
        {
            return input;
        }

        private bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }

        public string ConvertToRpn(string input)
        {
            string rpnString = string.Empty;
            Stack<char> operators = new Stack<char>();

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    while (i < input.Length && !_operators.IsOperator(input[i]) && !IsDelimeter(input[i]) )
                    {
                        rpnString += input[i];
                        i++;
                    }
                    rpnString += " ";
                    i--;
                }

                if(_operators.IsOperator(input[i]) )
                {
                    IOperator<double> op = _operators.GetOperator(input[i]);

                    if (operators.Count > 0)
                    {
                        if( op.Priority <= _operators.GetOperator(operators.Peek()).Priority )
                        {
                            rpnString += operators.Pop().ToString() + " ";
                        }
                    }

                    operators.Push(input[i]);
                }
            }

            while (operators.Count > 0)
                rpnString += operators.Pop() + " ";

            return rpnString;
        }

        public double Solve(string input)
        {
            double result = 0.0;
            string expression = CleanExpression(input);
            expression = ConvertToRpn(expression);
            Stack<double> solve = new Stack<double>();

            for(int i = 0; i < expression.Length; i++)
            {
                if( char.IsDigit(expression[i]) )
                {
                    string number = string.Empty;

                    while( i < expression.Length && !_operators.IsOperator(expression[i]) && !IsDelimeter(expression[i]) )
                    {
                        number += expression[i];
                        i++;
                    }

                    solve.Push(double.Parse(number));
                    i--;
                }

                if( _operators.IsOperator(expression[i]) )
                {
                    double second = solve.Pop(), first = solve.Pop();
                    IOperator<double> op = _operators.GetOperator(expression[i]);
                    result = op.Calculate(first, second);

                    solve.Push(result);
                }
            }

            return solve.Peek();
        }
    }
#endregion Calculators
}
