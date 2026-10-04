using la3OOP;
using System;
namespace la3OOP
{
    internal class SeriesCalculator
    {
        private double x;
        private int n;
        private double eps;

        public SeriesCalculator(double x, int n, double eps)
        {
            this.x=x;
            this.n=n;
            this.eps=eps;
        }

        public double ComputeExact()
        {
            return Math.Log(1.0/ (2+2*x+x*x));
        } 
        public double ComputeSN()
        {
            double t = (1+x)* (1+x);
            double a = -t; //a1
            double SN = a; //первый уже в сумме

            for (int j=2; j<=n; j++)
            {
                a= -a*t*(j-1)/j;
                SN=SN+a;
            }
            return SN;
        }

        public double ComputeSE()
        {
            double t = (1 + x) * (1 + x);
            double a = -t;      // a₁
            double SE = 0;
            int j = 1;

            while (Math.Abs(a) >= eps)
            {
                SE = SE + a;
                j = j + 1;
                a = -a * t * (j - 1) / j;
            }

            return SE;
        }
    }

        
      
  

    class Program
    {
        static void Main()
        {
            double a = -2.0;
            double b = -0.1;
            int k = 10;
            int n = 40;
            double eps = 0.0001;
            double h = (b - a) / k;

            Console.WriteLine("\tВычисление функции");

            for (int i = 0; i <= k; i++)
            {
                double x = a + i * h;
                SeriesCalculator calc = new SeriesCalculator(x, n, eps);

                double SN = calc.ComputeSN();
                double SE = calc.ComputeSE();
                double Y = calc.ComputeExact();

                Console.WriteLine($"X={x,6:F2}  SN={SN,10:F6}  SE={SE,10:F6}  Y={Y,10:F6}");
            }
        }

    }
}
