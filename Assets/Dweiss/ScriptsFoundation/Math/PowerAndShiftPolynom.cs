using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dweiss.Math
{
    [System.Serializable]
    public class PowerAndShiftPolynom
    {
        [UnityEngine.SerializeField] private float pow;
        [UnityEngine.SerializeField] private float powCoefficient;
        [UnityEngine.SerializeField] private float shift;

        public PowerAndShiftPolynom(float pow, float shift, float powCoefficient)
        {
            this.pow = pow;
            this.powCoefficient = powCoefficient;
            this.shift = shift;
        }


        public string InspectorToString()
        {
            var sb = new System.Text.StringBuilder();
            for (int i = 1; i < 10; i++)
            {
                sb.Append(Evaluate(i).ToString("0") + ",");
            }

            return sb.ToString();
        }


        public double Evaluate(double v)
        {
            return powCoefficient*System.Math.Pow(v, pow) + shift;
        }

        public double DeEvaluate(double v)
        {
            if (powCoefficient == 0)
            {
                return v - shift;
            } else if (pow == 0)
            {
                return (v - shift) / powCoefficient;
            }
            else
            {
                return System.Math.Pow((v - shift) / powCoefficient, 1 / pow);
            }
        }

    }
}