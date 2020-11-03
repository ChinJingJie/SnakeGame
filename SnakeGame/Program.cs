using System;
using System.Collections.Generic;
using System.Media;

namespace SnakeGame
{
    class Program
    {
        //declare variables

        char ch;
        int x, y, nTail, dx, dy, pts, foodx, foody, obstaclex, obstacley, shieldx, shieldy, delayInMillisecs, times, shieldtimes, Level;
        int[] TailX = new int[100];
        int[] TailY = new int[100];

        int consoleWidthLimit = 79;
        int consoleHeightLimit = 24;

        Random randx = new Random();
        Random randy = new Random();

        List<int> Obstaclesx = new List<int>();
        List<int> Obstaclesy = new List<int>();

        bool gameLive, pause, die, win, isprinted, shield;

        ConsoleKeyInfo consoleKey; // holds whatever key is pressed

        void MainMenu() {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();

            // fix window size
            Console.SetWindowSize(consoleWidthLimit + 2, consoleHeightLimit + 2);
            // clear to color
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = false;
        }

        void Initialization() {
            //snake
            ch = '*';
            x = 0;
            y = 2;
            dx = 1;
            dy = 0;
            nTail = 2;
            Level = 1;
            die = false;
            win = false;
            
            //food
            foodx = randx.Next(1, 78);
            foody = randy.Next(1, 23);
            //obstacles 
            //generate 10 obstacles positions
            for (int i = 0; i < 10; i++)
            {
                obstaclex = randx.Next(1, 78);
                obstacley = randy.Next(1, 23);

                Obstaclesx.Add(obstaclex);
                Obstaclesy.Add(obstacley);
            }
            //shield
            shieldx = randx.Next(1, 78);
            shieldy = randx.Next(1, 23);
            //score
            pts = 0;
            // delay to slow down the character movement so you can see it
            delayInMillisecs = 200;
            // Timer
            times = 0;
            shieldtimes = 0;
            //game status
            gameLive = true;
            pause = false;
            isprinted = false;
            shield = false;
        }

        void Input() {
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
                        break;
                    case ConsoleKey.DownArrow: // DOWN
                        dx = 0;
                        dy = 1;
                        break;
                    case ConsoleKey.LeftArrow: //LEFT
                        dx = -1;
                        dy = 0;
                        break;
                    case ConsoleKey.RightArrow: //RIGHT
                        dx = 1;
                        dy = 0;
                        break;
                    case ConsoleKey.Escape: //END
                        gameLive = false;
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.P: //P key
                        if (pause == false)
                        {
                            dx = 0;
                            dy = 0;
                            pause = true;
                        }
                        else
                        {
                            dx = 1;
                            dy = 0;
                            pause = false;
                        }
                        break;
                }
            }
        }

        void Logic() {

            //track tail position of the snake when movement occurs
            int preX = TailX[0];
            int preY = TailY[0];
            int tempX, tempY;

            if (!pause)
            {
                TailX[0] = x;
                TailY[0] = y;
                for (int i = 1; i < nTail; i++)
                {
                    tempX = TailX[i];
                    tempY = TailY[i];
                    TailX[i] = preX;
                    TailY[i] = preY;
                    preX = tempX;
                    preY = tempY;
                }
            }

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

            //time interval for random new food
            if (times == 120 && pause == false)
            {
                foodx = randx.Next(1, 78);
                foody = randy.Next(1, 23);
                times = 0;
            }

            if (shieldtimes == 120 && pause == false)
            {
                shieldx = randx.Next(1, 78);
                shieldy = randy.Next(1, 23);
                times = 0;
            }

            //when food eaten
            if (x == foodx && y == foody) 
            {
                //SoundPlayer playSound = new SoundPlayer(Properties.Resources.coin1); //add sound media
                //playSound.Play(); //play sound media
                Console.Beep();
                nTail++;
                pts += 1;
                foodx = randx.Next(1, 78);
                foody = randy.Next(1, 23);
            }

            if (x == shieldx && y == shieldy)
            {
                //SoundPlayer playSound = new SoundPlayer(Properties.Resources.coin1); //add sound media
                //playSound.Play(); //play sound media
                shield = true;
                
            }

            //when crash wall
            for (int i = 0; i < 10; i++)
            {
                if (x == Obstaclesx[i] && y == Obstaclesy[i])  
                {
                    if(shield == false)
                    {
                        if (pause == false)
                        { //before stop the game movement
                          // SoundPlayer playSound1 = new SoundPlayer(Properties.Resources.Downer01); //add sound media
                          //playSound1.Play(); //play sound media
                            dx = 0;
                            dy = 0;
                            pause = true;
                        }
                        gameLive = false;
                        die = true;
                    }

                    else
                    {
                        shield = false;
                    }

                    
                }
            }

            //when win
            if (pts == 2)
            {
                Level += 1;
                delayInMillisecs -= 50;
                pts = 0;
            }

            if(Level == 4)
            {
                if (pause == false)
                {
                    dx = 0;
                    dy = 0;
                    pause = true;
                }
                gameLive = false;
                win = true;
            }
        }
        void Die() {
            Console.SetCursorPosition((consoleWidthLimit / 2) - 3, consoleHeightLimit / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over", Console.ForegroundColor);
            Console.SetCursorPosition((consoleWidthLimit / 2) - 3, (consoleHeightLimit / 2) + 1);
            Console.WriteLine("Score: " + pts);

            while (true)
            {
                consoleKey = Console.ReadKey(true);

                if (consoleKey.Key == ConsoleKey.Enter)
                {
                    Environment.Exit(0);
                }
            }
        }

        void Win() {
            Console.SetCursorPosition((consoleWidthLimit / 2) - 3, consoleHeightLimit / 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Game Finished", Console.ForegroundColor);
            Console.SetCursorPosition((consoleWidthLimit / 2) - 3, (consoleHeightLimit / 2) + 1);
            Console.WriteLine("Score: " + pts);
            while (true)
            {
                consoleKey = Console.ReadKey(true);

                if (consoleKey.Key == ConsoleKey.Enter)
                {
                    Environment.Exit(0);
                }
            }
        }

        void Render() {
            Console.SetCursorPosition(0, 0);
            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Level: " + Level );
            Console.SetCursorPosition(70, 0);
            Console.WriteLine("Score: ");
            Console.SetCursorPosition(77, 0);
            Console.WriteLine(pts);
            Console.ForegroundColor = cc;
            times++;
            shieldtimes++;
            for (int i = 0; i < consoleHeightLimit; i++)
            {
                for (int j = 0; j < consoleWidthLimit; j++)
                {
                    for (int o = 0; o < 10; o++)
                    {
                        if (Obstaclesx[o] == j && Obstaclesy[o] == i)
                        {
                            Console.Write("||");
                        }
                    }

                    if (j == foodx && i == foody)
                    {
                        Console.Write("#");
                    }

                    else if (j == shieldx && i == shieldy)
                    {
                        if(shield == false)
                        {
                            Console.Write("[]");
                        }
                        
                    }

                    else if (j == x && i == y)
                    {
                        
                        if (shield == true)
                        {
                            Console.Write("]");
                        }

                        else
                        {
                            Console.Write(ch);
                        }

                    }

                    else
                    {                       
                        isprinted = false;
                        for (int k = 0; k < nTail; k++)
                        {
                            if (TailX[k] == j && TailY[k] == i)
                            {
                                Console.Write(ch);
                                isprinted = true;
                            }
                        }
                        if (!isprinted)
                            Console.Write(" ");                        
                    }
                    
                }
                Console.WriteLine();
            }
        }

        void Updates() {
            while (gameLive)
            {
                Input();
                Logic();
                Render();
                System.Threading.Thread.Sleep(delayInMillisecs);
            }
            if (!gameLive && win)
                Win();
            if (!gameLive && die)
                Die();
        }
        static void Main(string[] args)
        {
            Program snake = new Program();
            snake.MainMenu();
            while (true)
            {
                snake.Initialization();
                snake.Updates();
                Console.Clear();
            }
        }
    }

}
