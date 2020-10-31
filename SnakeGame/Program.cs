using System;
using System.Collections.Generic;
using System.Media;

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
            // point initialization
            int pts = 0;
            //randomize food position 
            int foodx = randx.Next(1, 78);  
            int foody = randy.Next(1, 23);
            //randomize obstacle position 
            int obstaclex = randx.Next(1, 78);
            int obstacley = randy.Next(1, 23);
            List<int> Obstaclesx = new List<int>();
            List<int> Obstaclesy = new List<int>();

            bool stop = true;


            // fix window size
            Console.SetWindowSize(consoleWidthLimit + 2, consoleHeightLimit + 2);
            // clear to color
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();

            // delay to slow down the character movement so you can see it
            int delayInMillisecs = 50;

            // whether to keep trails
            bool trail = false;
            
             // Timer
            int times = 0;
            bool timesUp = true;
            

            //Food
            Food food = new Food("#", randx.Next(1, 78), randy.Next(1, 23));
            food.GenerateFood(foodx, foody);
            //Obstacles

            for (int i = 0; i < 10; i++)
            {
                obstaclex = randx.Next(1, 78);
                obstacley = randy.Next(1, 23);

                Obstaclesx.Add(obstaclex);
                Obstaclesy.Add(obstacley);

                Obstacles obstacles = new Obstacles("||", randx.Next(1, 78), randy.Next(1, 23));
                obstacles.GenerateObstacles(obstaclex, obstacley);
            }
           
            string snakelength = "   ";
            do // until escape
            {
                // print directions at top, then restore position
                // save then restore current color
                ConsoleColor cc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(0, 0);
                Console.WriteLine("Move up/down/right/left. Press 'p' to pause/resume and 'esc' quit.");
                Console.SetCursorPosition(70, 0);
                Console.WriteLine("Score: "); 
                Console.SetCursorPosition(77, 0);
                Console.WriteLine(pts); 
                Console.ForegroundColor = cc;
                times++;

                if (times == 120 && stop == true)
                {
                    Console.SetCursorPosition(foodx, foody);
                    if (timesUp == true) {
                        Console.Write(' ');
                    }
                    foodx = randx.Next(1, 78);
                    foody = randy.Next(1, 23);
                    Food food1 = new Food("#", foodx, foody);
                    food1.GenerateFood(foodx, foody);
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
                        case ConsoleKey.P: //P key
                            if (stop == true)
                            {
                                dx = 0;
                                dy = 0;
                                stop = false;
                            }
                            else {
                                dx = 1;
                                dy = 0;
                                stop = true;
                            }   
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
                
                if (x == foodx && y == foody) //when food eaten
                {
                    SoundPlayer playSound = new SoundPlayer(Properties.Resources.coin1); //add sound media
                    playSound.Play(); //play sound media
                    ch += "*";
                    snakelength += " ";
                    pts += 1;
                    foodx = randx.Next(1, 78);
                    foody = randy.Next(1, 23);
                    Food food1 = new Food("#", foodx, foody);
                    food.GenerateFood(foodx, foody);
                }

                for (int i = 0; i < 10; i++)
                {
                    if (x == Obstaclesx[i] && y == Obstaclesy[i])  //when crash wall
                    {
                        dx = 0;
                        dy = 0;
                        if (stop ==true) { //before stop the game movement
                            SoundPlayer playSound1 = new SoundPlayer(Properties.Resources.Downer01); //add sound media
                            playSound1.Play(); //play sound media
                        }

                        stop = false;
                      
                        Console.SetCursorPosition((consoleWidthLimit / 2)-3, consoleHeightLimit / 2);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Game Over", Console.ForegroundColor);
                        Console.SetCursorPosition((consoleWidthLimit / 2)-3, (consoleHeightLimit / 2) + 1);
                        Console.WriteLine( "Score: " + pts);

                        if (Console.KeyAvailable)
                        {
                            consoleKey = Console.ReadKey(true);

                            if (consoleKey.Key == ConsoleKey.Enter)
                            {
                                gameLive = false;
                                break;
                            }
                        }
                    }
                }
                
                if(pts == 20)
                {

                    dx = 0;
                    dy = 0;

                    stop = false;

                    Console.SetCursorPosition((consoleWidthLimit / 2)-3, consoleHeightLimit / 2);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Game Finished", Console.ForegroundColor);
                    Console.SetCursorPosition((consoleWidthLimit / 2) - 3, (consoleHeightLimit / 2) + 1);
                    Console.WriteLine("Score: " + pts);

                    if (Console.KeyAvailable)
                    {
                        consoleKey = Console.ReadKey(true);

                        if (consoleKey.Key == ConsoleKey.Enter)
                        {
                            gameLive = false;
                            break;
                        }
                    }
                }
                

                // pause to allow eyeballs to keep up
                System.Threading.Thread.Sleep(delayInMillisecs);

            } while (gameLive);
        }
    }

}
