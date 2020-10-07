using System;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // start game
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // display this char on the console during the game
            string ch = "***";
            bool gameLive = true;
            ConsoleKeyInfo consoleKey; // holds whatever key is pressed

            // location info & display
            int x = 0, y = 2; // y is 2 to allow the top row for directions & space
            int dx = 1, dy = 0;
            int consoleWidthLimit = 79;
            int consoleHeightLimit = 24;
            //Random value
            Random randx = new Random();
            Random randy = new Random();

            // fix window size
            Console.SetWindowSize(consoleWidthLimit + 2, consoleHeightLimit + 2);
            // clear to color
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.Clear();

            // delay to slow down the character movement so you can see it
            int delayInMillisecs = 50;

            // whether to keep trails
            bool trail = false;
            
             // Timer
            int times = 0;
            bool timesUp = true;
            

            //Food
            int foodx = randx.Next(1, 78);
            int foody = randy.Next(1, 23);
            Food food = new Food("#", randx.Next(1, 78), randy.Next(1, 23));
            food.GenerateFood();
            //Obstacles
            for (int i = 0; i < 10; i++)
            {
                Obstacles obstacles = new Obstacles("||", randx.Next(1, 78), randy.Next(1, 23));
                obstacles.GenerateObstacles();
            }
            
            string snakelength = "   ";
            do // until escape
            {
                // print directions at top, then restore position
                // save then restore current color
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Arrows move up/down/right/left. Press 'esc' quit.");
                Console.SetCursorPosition(x, y);
                Console.ForegroundColor = cc;
                times++;

                if (times == 50)
                {
                    Console.SetCursorPosition(foodx, foody);
                    if (timesUp == true) {
                        Console.Write(' ');
                    }
                    foodx = randx.Next(1, 78);
                    foody = randy.Next(1, 23);
                    Food food = new Food("#", foodx, foody);
                    food.GenerateFood();
                    times = 0;                                       
                }

                // see if a key has been pressed
                if (Console.KeyAvailable)
                {
                    // get key and use it to set options
                    consoleKey = Console.ReadKey(true);
                    switch (consoleKey.Key)
                    {

                        case ConsoleKey.UpArrow: //UP
                            dx = 0;
                            dy = -1;
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case ConsoleKey.DownArrow: // DOWN
                            dx = 0;
                            dy = 1;
                            Console.ForegroundColor = ConsoleColor.Cyan;
                            break;
                        case ConsoleKey.LeftArrow: //LEFT
                            dx = -1;
                            dy = 0;
                            Console.ForegroundColor = ConsoleColor.Green;
                            break;
                        case ConsoleKey.RightArrow: //RIGHT
                            dx = 1;
                            dy = 0;
                            Console.ForegroundColor = ConsoleColor.Black;
                            break;
                        case ConsoleKey.Escape: //END
                            gameLive = false;
                            break;
                    }
                }

                // find the current position in the console grid & erase the character there if don't want to see the trail
                Console.SetCursorPosition(x, y);
                if (trail == false)
                    Console.Write(snakelength);

                // calculate the new position
                // note x set to 0 because we use the whole width, but y set to 1 because we use top row for instructions
                x += dx;
                if (x > consoleWidthLimit)
                    x = 0;
                if (x < 0)
                    x = consoleWidthLimit;

                y += dy;
                if (y > consoleHeightLimit)
                    y = 2; // 2 due to top spaces used for directions
                if (y < 2)
                    y = consoleHeightLimit;

                // write the character in the new position
                Console.SetCursorPosition(x, y);
                Console.Write(ch);
                
                if (x == foodx && y == foody)
                {
                    ch += "*";
                    snakelength += " ";
                    foodx = randx.Next(1, 78);
                    foody = randy.Next(1, 23);
                    Food food1 = new Food("#", foodx, foody);
                    food.GenerateFood(foodx, foody);
                }

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

            } while (gameLive);
        }
    }

}
