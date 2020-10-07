using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public class Food
    {
        private string _food;
        private int _foodx;
        private int _foody;

        public Food() { }

        public Food(string food, int foodx, int foody) {
            _food = food;
            _foodx = foodx;
            _foody = foody;
        }

        public string SnakeFood
        {
            get { return _food; }
            set { _food = value; }
        }

        public int FoodX
        {
            get { return _foodx; }
            set { _foodx = value; }
        }

        public int FoodY
        {
            get { return _foody; }
            set { _foody = value; }
        }

        public void GenerateFood(int foodx, int foody)
        {
            Console.SetCursorPosition(foodx, foody);
            Console.Write(_food);
        }
    }
}
