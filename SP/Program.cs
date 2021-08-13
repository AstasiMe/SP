using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SP
{
    class trafficLights
    {
        public void green(bool running)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this); // Уведомление любых ожидающих потоков
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(new string('о', 5) + "\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(new string('о', 3) + "\n");
                Console.ResetColor(); // сбрасываем в стандартный
                Monitor.Pulse(this); 
                Monitor.Wait(this); 
            }
        }
        public void red(bool running)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this);
                    return;
                }
                Console.ForegroundColor = ConsoleColor.Red; // устанавливаем цвет
                Console.Write(new string('о', 5) + "\n");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(new string('о', 3) + "\n");
                Console.ResetColor(); // сбрасываем в стандартный
                Monitor.Pulse(this); 
                Monitor.Wait(this); 
            }
        }
    }
    class MyThread
    {
        public Thread thread;
        trafficLights ttOb;
       
        public MyThread(string name, trafficLights tt)
        {
            ThreadStart threadStart = new ThreadStart(this.run); 
            thread = new Thread(threadStart);
            ttOb = tt;
            thread.Name = name;
            thread.Start();
        }
        
        void run()
        {
            if (thread.Name == "green")
            {
                for (int i = 0; i < 3; i++)
                {
                    ttOb.green(true);
                    ttOb.green(false);
                }
            }
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    ttOb.red(true);
                    ttOb.red(false);
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            trafficLights tt = new trafficLights();
            Console.WriteLine("Светофор: on");
            MyThread mt1 = new MyThread("red", tt);
            MyThread mt2 = new MyThread("green", tt);
            mt1.thread.Join();
            mt2.thread.Join();
            Console.WriteLine("Светофор: off");
        }
    }
}

