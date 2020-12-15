namespace Dweiss.Math
{
    [System.Serializable]
    public class Polynom
    {
        [UnityEngine.SerializeField]private float[] coefficients;
        public Polynom(float[] coefficients)
        {
            this.coefficients = coefficients;
        }

        public double Evaluate(float num)
        {
            double exp = 1;
            double sum = exp * coefficients[0];
            
            for (int i = 1; i < coefficients.Length; ++i)
            {
                exp = exp* num;
                sum += exp * coefficients[i];
            }
            return sum;
        }

     
    }
}
