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
    }

    public abstract class AbstractOperation: IOperator<double>
    {
        public OperatorsPriority Priority { get; }

        public AbstractOperation(OperatorsPriority priority) => Priority = priority;

        public abstract double Calculate(double first, double second);

    }

    public class AdditionOperator : IOperator<double>
    {
        public OperatorsPriority Priority { get; }

        public AdditionOperator() => Priority = OperatorsPriority.Low;

        public double Calculate(double first, double second) => first + second;
    }

    public class SubstractOperator : IOperator<double>
    {
        public OperatorsPriority Priority { get; }

        public SubstractOperator() => Priority = OperatorsPriority.Low;

        public double Calculate(double first, double second) => first - second;
    }

    public class MultiplicationOperator : IOperator<double>
    {
        public OperatorsPriority Priority { get; }

        public MultiplicationOperator() => Priority = OperatorsPriority.Medium;

        public double Calculate(double first, double second) => first * second;
    }

    public class DivisionOperator : IOperator<double>
    {
        public OperatorsPriority Priority { get; }

        public DivisionOperator() => Priority = OperatorsPriority.Medium;

        public double Calculate(double first, double second)
        {
            if (double.Equals(second, 0.0D))
                throw new DivideByZeroException();
            return first / second;
        }
    }

    public class ArithmeticalOperators
    {
        private Dictionary<char, IOperator<double>> _operators = new Dictionary<char, IOperator<double>>();

        public void Add(char symbol, IOperator<double> new_operator)
        {
            _operators.Add(symbol, new_operator);
        }

        public bool IsOperator(char c) => _operators.ContainsKey(c);

        public IOperator<double> this[char c] { get => _operators[c]; }

    }
#endregion Operators

    public class ExpressionCalculator
    {
        private ArithmeticalOperators _operators = new ArithmeticalOperators();

        public ExpressionCalculator()
        {
            _operators.Add('+', new AdditionOperator());
            _operators.Add('-', new SubstractOperator());
            _operators.Add('*', new MultiplicationOperator());
            _operators.Add('/', new DivisionOperator());
        }

        private int GetOperatorPriority(char c)
        {
            int operatorPriority = 0;

            switch(c)
            {
                case '*':
                    operatorPriority = 4;
                    break;
                case '/':
                    operatorPriority = 4;
                    break;
                case '+':
                    operatorPriority = 2;
                    break;
                case '-':
                    operatorPriority = 3;
                    break;
                default:
                    break;
            }

            return operatorPriority;
        }

        private string CleanExpression(string input)
        {
            return input;
        }

        private bool IsOperator(char c)
        {
            return "+-*/".Contains(c.ToString());
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
                    if(operators.Count > 0)
                    {
                        if( GetOperatorPriority(input[i]) <= GetOperatorPriority(operators.Peek()))
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

                    result = _operators[expression[i]].Calculate(first, second);

                    solve.Push(result);
                }
            }

            return solve.Peek();
        }
    }
}
