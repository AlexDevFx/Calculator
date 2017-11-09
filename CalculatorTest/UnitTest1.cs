using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solver;

namespace CalculatorTest
{
    [TestClass]
    public class CalculatorTests
    {
        ExpressionCalculator calc;

        [TestInitialize]
        public void Init()
        {
            OperatorsList<double> operators = new OperatorsList<double>();
            operators.Add('+', new DoubleAdditionOperator(OperatorsPriority.Low));
            operators.Add('-', new DoubleSubstractOperator(OperatorsPriority.Low));
            operators.Add('*', new DoubleMultiplicationOperator(OperatorsPriority.Medium));
            operators.Add('/', new DoubleDivisionOperator(OperatorsPriority.Medium));

           calc = new ExpressionCalculator(operators);
        }

        [TestMethod]
        public void Solve_2Plus2_4Returned()
        {
            Assert.AreEqual(calc.Solve("2+2"), 4.0);
        }

        [TestMethod]
        public void Solve_3Mul2_6Returned()
        {
            Assert.AreEqual(calc.Solve("3*2"), 6.0);
        }

        [TestMethod]
        public void Solve_8Sub3_5Returned()
        {
            Assert.AreEqual(calc.Solve("8-3"), 5.0);
        }

        [TestMethod]
        public void Solve_5Div2_2dot5Returned()
        {
            Assert.AreEqual(calc.Solve("5/2"), 2.5);
        }

        [TestMethod]
        [ExpectedException(typeof(System.DivideByZeroException))]
        public void Solid_7Div0_Exception()
        {
           Assert.AreEqual(calc.Solve("7/0"), 0.0);
        }

        [TestMethod]
        public void Solve_5Mul2Sub1_9Returned()
        {
            Assert.AreEqual(calc.Solve("5*2-1"), 9.0);
        }

        [TestMethod]
        public void Solve_10Mul3Plus5Sub9Div3_32Returned()
        {
            Assert.AreEqual(calc.Solve("10*3+5-9/3"), 32.0);
        }

        [TestMethod]
        public void Solve_18Plus6Sub24Plus3Mul0Sub16Div5_0dot2Returned()
        {
            Assert.AreEqual(System.Math.Round(calc.Solve("18+6-24+3*0-16/5"),1), -3.2D);
        }
    }
}
