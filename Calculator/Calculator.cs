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
        private const string delimeter = " ";

        public ExpressionCalculator(IOperatorsList<double> new_operators) => _operators = new_operators;

        private string CleanExpression(string input) => input.Replace(" ", string.Empty);

        private bool IsDelimeter(char c) => delimeter.IndexOf(c) != -1;

        private string AddDelimeter(string s) => s += delimeter;

        private bool IsDigit(char c) => !_operators.IsOperator(c) && !IsDelimeter(c);

        private string ReadDigits(ref int i, string string_for_read)
        {
            string digits = string.Empty;

            while(i < string_for_read.Length && IsDigit(string_for_read[i]) )
            {
                digits += string_for_read[i];
                i++;
            }

            return digits;
        }

        private void PlaceOperatorsAccordingPriority(char current_operator, Stack<char> operators_stack, ref string out_string)
        {
            if (operators_stack.Count > 0)
            {
                IOperator<double> current = _operators.GetOperator(current_operator);
                IOperator<double> previous = _operators.GetOperator(operators_stack.Peek());

                if (current.Priority <= previous.Priority)
                {
                    out_string += AddDelimeter(operators_stack.Pop().ToString());
                }
            }
        }

        private double CalculateOperator(char operator_symbol, Stack<double> operands)
        {
            double second = operands.Pop(), first = operands.Pop();
            double result = 0.0 ;
            IOperator<double> op = _operators.GetOperator(operator_symbol);
            result = op.Calculate(first, second);
            return result;
        }

        private string ConvertToRpn(string input)
        {
            string rpnString = string.Empty;
            Stack<char> operatorsStack = new Stack<char>();

            for (int i = 0; i < input.Length; i++)
            {
                if (char.IsDigit(input[i]))
                {
                    rpnString += ReadDigits(ref i, input);
                    rpnString = AddDelimeter(rpnString);
                    i--;
                }

                if(_operators.IsOperator(input[i]) )
                {
                    PlaceOperatorsAccordingPriority(input[i], operatorsStack, ref rpnString);
                    operatorsStack.Push(input[i]);
                }
            }

            while (operatorsStack.Count > 0)
                rpnString += AddDelimeter(operatorsStack.Pop().ToString());

            return rpnString;
        }

        public double Solve(string input)
        {
            string expression = CleanExpression(input);
            expression = ConvertToRpn(expression);
            Stack<double> solve = new Stack<double>();

            for(int i = 0; i < expression.Length; i++)
            {
                if( char.IsDigit(expression[i]) )
                {
                    string number = ReadDigits(ref i, expression);
                    solve.Push(double.Parse(number));
                    i--;
                }

                if( _operators.IsOperator(expression[i]) )
                {
                    solve.Push(CalculateOperator(expression[i], solve));
                }
            }

            return solve.Peek();
        }
    }
#endregion Calculators
}
