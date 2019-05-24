using System;
using WindowsInput;
using System.Timers;

namespace AutoClicker
{
    class Program
    {
        private static Timer timer;
        private static InputSimulator simulator = new InputSimulator();
        private static string e;
        private static int counter = 0;
        private static double stopCount = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Jake's Auto Clicker!");
            Console.WriteLine("Enter an event follwed by an interval in ms");
            Console.WriteLine("Press Enter to stop or Escape to exit");
            Console.WriteLine("Press h for help :)");

            Action();

            Environment.Exit(0);
        }

        private static void Action()
        {
            var input = Console.ReadLine().Split(' ');
            stopCount = input[2] != null ? double.Parse(input[2]) : 1/ counter;
            while (true)
            {
                e = input[0];

                if (e.ToUpper() == "H")
                {
                    ShowHelp();
                }
                else
                {
                    if (ValidateEvent(e))
                    {
                        SetTimer(int.Parse(input[1]));
                        // End program
                        var end = Console.ReadKey();
                        if(end.Key.ToString() == "Enter")
                        {
                            timer.Stop();

                            Console.WriteLine("Action stopped");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input, try again derp");
                    }
                }
            }
        }

        private static void SetTimer(int interval)
        {
            timer = new Timer(interval);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs ev)
        {
            if (counter >= stopCount)
            {
                return;
            }
            switch (e)
            {
                case "lc":
                    simulator.Mouse.LeftButtonClick();
                    break;
                case "ldc":
                    simulator.Mouse.LeftButtonDoubleClick();
                    break;
            }

            counter++;
            Console.WriteLine("Clicked " + counter + " times");
        }

        private static bool ValidateEvent(string e)
        {
            return e == "lc" || e == "ldc";
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Action are:");
            Console.WriteLine("Left Click: lc");
            Console.WriteLine("Left Double Click: ldc");
        }
    }
}
