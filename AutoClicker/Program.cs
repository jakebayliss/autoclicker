using System;
using WindowsInput;
using System.Timers;
using WindowsInput.Native;
using System.Threading;

namespace AutoClicker
{
    class Program
    {
        private static System.Timers.Timer timer;
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
            if (input.Length == 3)
            {
                stopCount = input[2] != null ? double.Parse(input[2]) : 1 / counter;
            }
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
                        if(e == "d")
                        {
                            DropEverything();
                            Console.WriteLine("Done");
                            var end = Console.ReadLine();
                        }
                        else if(input.Length == 1 && e == "r")
                        {
                            simulator.Mouse.RightButtonDown();
                        }
                        else
                        {
                            SetTimer(int.Parse(input[1]));
                            // End program
                            var end = Console.ReadKey();
                            if (end.Key.ToString() == "Enter")
                            {
                                timer.Stop();

                                Console.WriteLine("Action stopped");
                            }
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
            timer = new System.Timers.Timer(interval);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs ev)
        {
            if(stopCount > 0)
            {
                if (counter >= stopCount)
                {
                    return;
                }
            }
            switch (e)
            {
                case "lc":
                    simulator.Mouse.LeftButtonClick();
                    break;
                case "ldc":
                    simulator.Mouse.LeftButtonDoubleClick();
                    break;
                case "rc":
                    simulator.Mouse.RightButtonClick();
                    break;
            }

            counter++;
            Console.WriteLine("Clicked " + counter + " times");
        }

        private static void DropEverything()
        {
            simulator.Mouse.LeftButtonClick(); //Focus the window
            simulator.Keyboard.KeyDown((VirtualKeyCode)16);

            for (int i = 0; i < 7; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    simulator.Mouse.LeftButtonClick();
                    Thread.Sleep(100);
                    simulator.Mouse.MoveMouseBy(60, 0);
                    Thread.Sleep(100); // can only drop 1 item per game tick
                }
                simulator.Mouse.LeftButtonClick();
                Thread.Sleep(100);
                if(i < 6) {
                    simulator.Mouse.MoveMouseBy(-180, 50);
                }
            }
            simulator.Keyboard.KeyUp((VirtualKeyCode)16);
        }

        private static bool ValidateEvent(string e)
        {
            return e == "lc" || e == "ldc" || e == "rc" || e == "r" || e == "d";
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Actions are:");
            Console.WriteLine("Left Click: lc");
            Console.WriteLine("Left Double Click: ldc");
            Console.WriteLine("Drop everything: d");
        }
    }
}
