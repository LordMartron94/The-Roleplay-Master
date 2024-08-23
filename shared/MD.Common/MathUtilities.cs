namespace MD.Common
{
    public static class MathUtilities
    {
        /// <summary>
        /// Uses Sigmoid to squish a list (array) of items to sum up to a value of 1.
        /// </summary>
        public static double[] CalculateSigmoid(double[] values)
        {
            double[] sigmoided = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                double activatedV = 1 / (1 + Math.Exp(-values[i]));
                sigmoided[i] = activatedV;
            }

            return sigmoided;
        }

        /// <summary>
        /// Uses a calculation to return the derivative (slope) of the Sigmoid function based on a list of values.
        /// </summary>
        public static double[] SigmoidDerivative(double[] values)
        {
            double[] derivative = new double[values.Length];

            for (int i = 0; i < values.Length; i++)
            {
                double derivativeV = Math.Exp(values[i]) / Math.Pow(Math.Exp(values[i]) + 1, 2);
                derivative[i] = derivativeV;
            }

            return derivative;
        }
    }
}