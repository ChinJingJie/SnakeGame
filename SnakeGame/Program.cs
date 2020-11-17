using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace SnakeGame
{
    public class Program
    {
        //declare variables

        char ch;
        string user;
        int x, y, nTail, dx, dy, pts, ptsTotal, foodx, foody, obstaclex1, obstacley1, obstaclex2, obstacley2, obstaclex3, obstacley3, shieldx, shieldy, heartx, hearty, delayInMillisecs, times, shieldtimes, hearttimes, Level, heartcount;
        int[] TailX = new int[100];
        int[] TailY = new int[100];

        int consoleWidthLimit = 79;
        int consoleHeightLimit = 24;

        Random randx = new Random();
        Random randy = new Random();

        bool gameLive, pause, die, win, isprinted, shield, heart;

        ConsoleKeyInfo consoleKey; // holds whatever key is pressed

        public void Option()
        {
            while (true)
            {
                Console.WriteLine("====================");
                Console.WriteLine("|   Main Menu      |");
                Console.WriteLine("|   1. Play        |");
                Console.WriteLine("|   2. Scoreboard  |");
                Console.WriteLine("|   3. Quit        |");
                Console.WriteLine("====================");
                Console.WriteLine("Instruction:");
                Console.WriteLine("Move up/down/right/left.");
                Console.WriteLine("");
                Console.Write("Choice: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    Console.Write("Username: ");
                    user = Console.ReadLine();
                    // start game
                    while (true)
                    {
                        Initialization();
                        Updates();
                        Console.Clear();
                    }
                }
                else if (choice == "2")
                {
                    Console.Clear();
                    Console.WriteLine("====================");
                    Console.WriteLine("Scoreboard:");
                    Console.WriteLine("====================");
                    var path = "score.txt";
                    string[] score = File.ReadAllLines(path, Encoding.UTF8);
                    foreach (string line in score)
                    {
                        Console.WriteLine(line);
                    }
                    Console.WriteLine("====================");
                }
                else if (choice == "3")
                {
                    gameLive = false;
                    break;
                }
            }
        }

        public void MainMenu() {
            // fix window size
            Console.SetWindowSize(consoleWidthLimit + 2, consoleHeightLimit + 2);
            // clear to color
            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.CursorVisible = false;
        }

        public void Initialization() {
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
            heartcount = 0;
            //food
            foodx = randx.Next(1, 78);
            foody = randy.Next(1, 23);
            //obstacles 
            obstaclex1 = randx.Next(1, 78);
            obstacley1 = randy.Next(1, 23);
            obstaclex2 = randx.Next(1, 78);
            obstacley2 = randy.Next(1, 23);
            obstaclex3 = randx.Next(1, 78);
            obstacley3 = randy.Next(1, 23);
            //shield
            shieldx = randx.Next(1, 78);
            shieldy = randx.Next(1, 23);
            //heart
            heartx = randx.Next(1, 78);
            hearty = randx.Next(1, 23);
            //score
            pts = 0;
            ptsTotal = 0;
            // delay to slow down the character movement so you can see it
            delayInMillisecs = 200;
            // Timer
            times = 0;
            shieldtimes = 0;
            hearttimes = 0;
            //game status
            gameLive = true;
            pause = false;
            isprinted = false;
            shield = false;
            heart = false;
        }

        public void Input() {
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

        public void Logic() {

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
            if (x > consoleWidthLimit - 2)
                x = 0;
            if (x < 0)
                x = consoleWidthLimit - 2;

            y += dy;
            if (y > consoleHeightLimit - 2)
                y = 1;
            if (y < 1)
                y = consoleHeightLimit - 2;

            //time interval for random new food
            if (times == 120 && pause == false)
            {
                foodx = randx.Next(1, 78);
                foody = randy.Next(1, 23);
                times = 0;
            }

            if (shieldtimes == 120 && pause == false && shield == false)
            {
                shieldx = randx.Next(1, 78);
                shieldy = randy.Next(1, 23);
                shieldtimes = 0;
            }

            if (hearttimes == 120 && pause == false && heart == false)
            {
                heartx = randx.Next(1, 78);
                hearty = randy.Next(1, 23);
                hearttimes = 0;
            }

            //when food eaten
            if (x == foodx && y == foody)
            {
                //SoundPlayer playSound = new SoundPlayer(Properties.Resources.coin1); //add sound media
                //playSound.Play(); //play sound media
                Console.Beep();
                nTail++;
                pts += 1;
                ptsTotal += 1;
                foodx = randx.Next(1, 78);
                foody = randy.Next(1, 23);
            }

            if (x == shieldx && y == shieldy)
            {
                shield = true;
            }

            if (x == heartx && y == hearty)
            {
                heartx = 0;
                hearty = 0;
                heart = true;
                heartcount += 1;
            }

            //when crash wall
            if ((x == obstaclex1 && y == obstacley1) || (x == obstaclex2 && y == obstacley2) || (x == obstaclex3 && y == obstacley3))
            {
                if (shield == false && heart == false)
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
                    if ((shield == true && heart == false) || (shield == true && heart == true))
                    {
                        shield = false;
                    }else
                    {
                        heart = false;
                        heartcount = 0;
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
        public void Die() {
            Console.SetCursorPosition((consoleWidthLimit / 2) - 3, consoleHeightLimit / 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Game Over", Console.ForegroundColor);
            Console.SetCursorPosition((consoleWidthLimit / 2) - 3, (consoleHeightLimit / 2) + 1);
            Console.WriteLine("Score: " + ptsTotal);
            string[] scores = { "Username " + user + ": Level "+ Level+ " - Score: " + ptsTotal };
            File.AppendAllLines("score.txt", scores);
            Console.SetCursorPosition((consoleWidthLimit / 2) -15, (consoleHeightLimit / 2) + 2);
            Console.WriteLine("Press 'r' to return to main page");

            while (true)
            {
                consoleKey = Console.ReadKey(true);

                if (consoleKey.Key == ConsoleKey.Enter)
                {
                    Environment.Exit(0);
                }
                if (consoleKey.Key == ConsoleKey.R)
                {
                    Console.Clear();
                    Option();
                }
            }
        }

        public void Win() {
            Console.SetCursorPosition((consoleWidthLimit / 2) - 3, consoleHeightLimit / 2);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Game Finished", Console.ForegroundColor);
            Console.SetCursorPosition((consoleWidthLimit / 2) - 3, (consoleHeightLimit / 2) + 1);
            Console.WriteLine("Score: " + ptsTotal);
            string[] scores = { "Username " + user + ": Level " + Level + " - Score: " + ptsTotal };
            File.AppendAllLines("score.txt", scores);
            Console.SetCursorPosition((consoleWidthLimit / 2) -15, (consoleHeightLimit / 2) + 2);
            Console.WriteLine("Press 'r' to return to main page");

            while (true)
            {
                consoleKey = Console.ReadKey(true);

                if (consoleKey.Key == ConsoleKey.Enter)
                {
                    Environment.Exit(0);
                }
                if (consoleKey.Key == ConsoleKey.R)
                {
                    Console.Clear();
                    Option();
                }
            }
        }

        public void Render() {
            Console.SetCursorPosition(0, 0);
            ConsoleColor cc = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(5, 0);
            Console.WriteLine("Level: " + Level);
            Console.SetCursorPosition(30, 0);
            Console.WriteLine("Heart: " + heartcount);
            Console.SetCursorPosition(70, 0);
            Console.WriteLine("Score: ");
            Console.SetCursorPosition(77, 0);
            Console.WriteLine(ptsTotal);
            Console.ForegroundColor = cc;
            times++;
            shieldtimes++;
            for (int i = 0; i < consoleHeightLimit; i++)
            {
                for (int j = 0; j < consoleWidthLimit; j++)
                {
                    if (j == foodx && i == foody)
                    {
                        Console.Write("#");
                    }
                    else if (j == obstaclex1 && i == obstacley1)
                    {
                        Console.Write("|");
                    }
                    else if (j == obstaclex2 && i == obstacley2)
                    {
                        Console.Write("|");
                    }
                    else if (j == obstaclex3 && i == obstacley3)
                    {
                        Console.Write("|");
                    }
                    else if (j == shieldx && i == shieldy)
                    {
                        if(shield == false)
                        {
                            Console.Write("[");
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

                    else if (j == heartx && i == hearty)
                    {
                        if (heart == false)
                        {
                            Console.Write("H", Console.ForegroundColor = ConsoleColor.Red);
                            Console.ForegroundColor = ConsoleColor.Black;
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

        public void Updates() {
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

        public bool getGameLive()
        {
            return gameLive;
        }

        public int getLevel()
        {
            return Level;
        }

        public void SetScore(int score)
        {
            pts = score;
        }
        
        public int getTail()
        {
            return nTail;
        }

        public void SetX(int X)
        {
            x = X;
        }

        public void SetY(int Y)
        {
            y = Y;
        }
        public void SetFoodX(int foodX)
        {
            foodx = foodX;
        }

        public void SetFoodY(int foodY)
        {
            foody = foodY;
        }
        public void SetDX(int dX)
        {
            dx = dX;
        }

        public void SetDY(int dY)
        {
            dy = dY;
        }

        public void SetHeartCount(int count)
        {
            heartcount = count;
        }

        public int getHeart()
        {
            return heartcount;
        }
        
        static void Main(string[] args)
        {
            Program snake = new Program();
            snake.MainMenu();
            snake.Option();
        }
    }

}
