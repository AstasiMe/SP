using System;
using System.Threading;

namespace SP3
{
    class integral
    {
        static double Polinom(double A, double B, double C, double D, double x)
        {
            return A * Math.Pow(x, 3) + B * Math.Pow(x, 2) + C * x + D;
        }
        public double s1, s2, s3 = 0;
        public void square1(bool running, double A, double B, double C, double D, double x)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this);
                    return;
                }
                s1 = Polinom(A, B, C, D, x);
                Console.WriteLine("Площадь первого прямоугольника равна=" + s1);
                Monitor.Pulse(this); // Разрешает выполнение следующего потока
                Monitor.Wait(this); // Ожидает завершения следующего потока

            }
        }
        public void square2(bool running, double A, double B, double C, double D, double x)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this);
                    return;
                }
                s2 = Polinom(A, B, C, D, x);
                Console.WriteLine("Площадь второго прямоугольника равна=" + s2);
                Monitor.Pulse(this); // Разрешает выполнение следующего потока
                Monitor.Wait(this); // Ожидает завершения следующего потока
            }
        }
        public void square3(bool running, double A, double B, double C, double D, double x)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this);
                    return;
                }
                s3 = Polinom(A, B, C, D, x);
                Console.WriteLine("Площадь третьего прямоугольника равна=" + s3);
                Monitor.Pulse(this); // Разрешает выполнение следующего потока
                Monitor.Wait(this); // Ожидает завершения следующего потока
            }
        }
    }
    class MyThread
    {
        public Thread thrd;
        integral ttOb;

        // Создаем новый поток.
        public static double a, b, c, d, x, h;
        public MyThread(string name, integral tt, double A, double B, double C, double D, double X, double H)
        {
            thrd = new Thread(new ThreadStart(this.run));
            ttOb = tt;
            thrd.Name = name;
            thrd.Start();
            a = A;
            b = B;
            c = C;
            d = D;
            x = X;
            h = H;
        }
        // Начинаем выполнение нового потока.
        void run()
        {
            if (thrd.Name == "1")
            {
                ttOb.square1(true, a, b, c, d, x + h);
                ttOb.square1(false, a, b, c, d, x + h);
            }
            else if (thrd.Name == "2")
            {
                ttOb.square2(true, a, b, c, d, x + 2 * h);
                ttOb.square2(false, a, b, c, d, x + 2 * h);
            }
            else if (thrd.Name == "3")
            {
                ttOb.square3(true, a, b, c, d, x + 3 * h);
                ttOb.square3(false, a, b, c, d, x + 3 * h);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите a=");
            double a = double.Parse(Console.ReadLine());
            Console.Write("Введите b=");
            double b = double.Parse(Console.ReadLine());
            Console.WriteLine("Выберите тип функции");
            Console.WriteLine("Функция имеет вид: A*x^3+B*x^2+C*x+D");
            Console.WriteLine("Введите коэффициенты A,B,C,D");
            Console.Write("Введите A=");
            double A = double.Parse(Console.ReadLine());
            Console.Write("Введите B=");
            double B = double.Parse(Console.ReadLine());
            Console.Write("Введите C=");
            double C = double.Parse(Console.ReadLine());
            Console.Write("Введите D=");
            double D = double.Parse(Console.ReadLine());
            double h = (b - a) / 3;
            integral tt = new integral();
            MyThread mt1 = new MyThread("1", tt, A, B, C, D, a, h);
            MyThread mt2 = new MyThread("2", tt, A, B, C, D, a, h);
            MyThread mt3 = new MyThread("3", tt, A, B, C, D, a, h);
            mt1.thrd.Join();
            mt2.thrd.Join();
            mt3.thrd.Join();
        }
    }
}
