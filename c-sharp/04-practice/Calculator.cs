namespace CalculatorLibrary
{
    public class Calculator
    {
        public static double Add(double num1, double num2)
        {
            return num1 + num2;
        }

        public static double Subtract(double num1, double num2)
        {
            return num1 - num2;
        }

        public static double Multiply(double num1, double num2)
        {
            return num1 * num2;
        }

        public static double Divide(double num1, double num2)
        {
            return num1 / num2;
        }

        public static double Maximum(params double[] numbers)
        {
            double max = numbers[0];
            foreach (var num in numbers)
            {
                if (num > max)
                {
                    max = num;
                }
            }
            return max;
        }
    }
}