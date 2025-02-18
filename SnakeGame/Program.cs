using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Program
    {
        static int screenWidth = 32;
        static int screenHeight = 16;
        static Random random = new Random();
        static int score = 5;
        static bool isGameOver = false;
        static string movement = "RIGHT";
        static List<int> snakeBodyX = new List<int>();
        static List<int> snakeBodyY = new List<int>();
        static SnakeSegment snakeHead = new SnakeSegment();
        static int berryX, berryY;

        static void Main(string[] args)
        {
            InitializeGame();
            GameLoop();
        }

        static void InitializeGame()
        {
            Console.WindowHeight = screenHeight;
            Console.WindowWidth = screenWidth;
            snakeHead.xpos = screenWidth / 2;
            snakeHead.ypos = screenHeight / 2;
            snakeHead.color = ConsoleColor.Red;
            berryX = random.Next(0, screenWidth);
            berryY = random.Next(0, screenHeight);
        }

        static void GameLoop()
        {
            while (!isGameOver)
            {
                Console.Clear();
                DrawBorders();
                CheckCollision();
                DrawSnakeAndBerry();
                HandleInput();
                UpdateSnakePosition();
                Thread.Sleep(100);
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
        }

        static void DrawBorders()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }
            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        static void DrawSnakeAndBerry()
        {
            Console.ForegroundColor = snakeHead.color;
            Console.SetCursorPosition(snakeHead.xpos, snakeHead.ypos);
            Console.Write("■");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(berryX, berryY);
            Console.Write("■");
        }

        static void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow && movement != "DOWN") movement = "UP";
                if (key.Key == ConsoleKey.DownArrow && movement != "UP") movement = "DOWN";
                if (key.Key == ConsoleKey.LeftArrow && movement != "RIGHT") movement = "LEFT";
                if (key.Key == ConsoleKey.RightArrow && movement != "LEFT") movement = "RIGHT";
            }
        }

        static void UpdateSnakePosition()
        {
            snakeBodyX.Add(snakeHead.xpos);
            snakeBodyY.Add(snakeHead.ypos);
            switch (movement)
            {
                case "UP": snakeHead.ypos--; break;
                case "DOWN": snakeHead.ypos++; break;
                case "LEFT": snakeHead.xpos--; break;
                case "RIGHT": snakeHead.xpos++; break;
            }
            if (snakeBodyX.Count > score)
            {
                snakeBodyX.RemoveAt(0);
                snakeBodyY.RemoveAt(0);
            }
        }

        static void CheckCollision()
        {
            if (snakeHead.xpos == screenWidth - 1 || snakeHead.xpos == 0 ||
                snakeHead.ypos == screenHeight - 1 || snakeHead.ypos == 0)
            {
                isGameOver = true;
            }
            for (int i = 0; i < snakeBodyX.Count; i++)
            {
                if (snakeBodyX[i] == snakeHead.xpos && snakeBodyY[i] == snakeHead.ypos)
                {
                    isGameOver = true;
                }
            }
            if (berryX == snakeHead.xpos && berryY == snakeHead.ypos)
            {
                score++;
                berryX = random.Next(1, screenWidth - 2);
                berryY = random.Next(1, screenHeight - 2);
            }
        }

        class SnakeSegment
        {
            public int xpos { get; set; }
            public int ypos { get; set; }
            public ConsoleColor color { get; set; }
        }
    }
}
