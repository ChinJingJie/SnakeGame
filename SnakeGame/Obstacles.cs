using System;
using System.Collections.Generic;
using System.Text;

namespace SnakeGame
{
    public class Obstacles
    {
        private string _obstacle;
        private int _obstaclex;
        private int _obstacley;

        public Obstacles() { }

        public Obstacles(string obstacle, int obstaclex, int obstacley)
        {
            _obstacle = obstacle;
            _obstaclex = obstaclex;
            _obstacley = obstacley;
        }

        public string SnakeObstacles
        {
            get { return _obstacle; }
            set { _obstacle = value; }
        }

        public int ObstacleX
        {
            get { return _obstaclex; }
            set { _obstaclex = value; }
        }

        public int ObstacleY
        {
            get { return _obstacley; }
            set { _obstacley = value; }
        }

        public void GenerateObstacles()
        {
            Console.SetCursorPosition(_obstaclex, _obstacley);
            Console.Write(_obstacle);
        }
    }
}
