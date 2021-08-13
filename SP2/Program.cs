using System;
using System.Threading;

namespace SP2
{
    class elevator
    {
        public void up(bool running, int level1, int level2)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this); 
                    return;
                }
                Console.WriteLine("Лифт поднимается с " + level1 + " этажа на " + level2);
                Monitor.Pulse(this); // Разрешает выполнение следующего потока
                Monitor.Wait(this); // Ожидает завершения следующего потока

            }
        }
        public void down(bool running, int floor1, int floor2)
        {
            lock (this)
            {
                if (!running)
                {
                    Monitor.Pulse(this); 
                    return;
                }
                Console.WriteLine("Лифт опускается с " + floor1 + " этажа на " + floor2);
                Monitor.Pulse(this); // Разрешает выполнение следующего потока
                Monitor.Wait(this); // Ожидает завершения следующего потока
            }

        }
    }
    class MyThread
    {
        public Thread thread;
        elevator ttOb;
        static int floor_1, floor_2, floor_3;
        // Создаем новый поток.
        public MyThread(int floor1, int floor2, int floor3, bool first, elevator tt)
        {
            ThreadStart threadStart = new ThreadStart(this.run); // Создаем новый поток
            thread = new Thread(threadStart);
            ttOb = tt;
            floor_1 = floor1;
            floor_2 = floor2;
            floor_3 = floor3;
            if (first == true)
            {
                if (floor1 < floor2)
                    thread.Name = "up";
                else if (floor1 > floor2)
                    thread.Name = "down";
            }
            else if (floor2 < floor3)
                thread.Name = "up_";

            thread.Start();
        }
        // Начинаем выполнение нового потока.
        void run()
        {
            if (thread.Name == "up")
            {

                ttOb.up(true, floor_1, floor_2);
                ttOb.up(false, floor_1, floor_2);

            }
            else if ((thread.Name == "down"))
            {
                ttOb.down(true, floor_1, floor_2);
                ttOb.down(false, floor_1, floor_2);
            }
            else if (thread.Name == "up_")
            {
                ttOb.up(true, floor_2, floor_3);
                ttOb.up(false, floor_2, floor_3);
            }
            else
            {
                ttOb.down(true, floor_2, floor_3);
                ttOb.down(false, floor_2, floor_3);
            }
        }
    }
    class Program
    {
        public static int floor_elevator = 1;
        static void Main(string[] args)
        {
            for (int i = 1; i < 100; i++)
            {
                Console.WriteLine("Лифт на " + floor_elevator + " этаже");
                Console.WriteLine("Введите номер этажа на котором вы находитесь");
                int my_floor = int.Parse(Console.ReadLine());
                Console.WriteLine("Введите нужный номер этажа");
                int need_floor = int.Parse(Console.ReadLine());
                elevator tt = new elevator();
                MyThread mt1 = new MyThread(floor_elevator, my_floor, need_floor, true, tt);
                MyThread mt2 = new MyThread(floor_elevator, my_floor, need_floor, false, tt);
                mt1.thread.Join();
                mt2.thread.Join();
                floor_elevator = need_floor;
                Console.WriteLine("Лифт прибыл на " + need_floor + " этаж");
                Console.WriteLine();
            }
        }
    }
}

