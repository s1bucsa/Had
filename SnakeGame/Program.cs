using System;
using System.Collections.Generic;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;

            Random random = new Random();
            int score = 5;
            bool isGameOver = false;

            SnakeSegment snakeHead = new SnakeSegment();
            snakeHead.xpos = screenWidth / 2;
            snakeHead.ypos = screenHeight / 2;
            snakeHead.color = ConsoleColor.Red;

            string movement = "RIGHT";

            List<int> snakeBodyX = new List<int>();
            List<int> snakeBodyY = new List<int>();

            int berryX = random.Next(0, screenWidth);
            int berryY = random.Next(0, screenHeight);

            DateTime startTime;
            DateTime currentTime;

            string buttonPressed = "no";

            while (true)
            {
                Console.Clear();
                if (snakeHead.xpos == screenWidth - 1 || snakeHead.xpos == 0 ||
                    snakeHead.ypos == screenHeight - 1 || snakeHead.ypos == 0)
                {
                    isGameOver = true;
                }

                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, screenHeight - 1);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(0, i);
                    Console.Write("■");
                }
                for (int i = 0; i < screenHeight; i++)
                {
                    Console.SetCursorPosition(screenWidth - 1, i);
                    Console.Write("■");
                }

                Console.ForegroundColor = ConsoleColor.Green;
                if (berryX == snakeHead.xpos && berryY == snakeHead.ypos)
                {
                    score++;
                    berryX = random.Next(1, screenWidth - 2);
                    berryY = random.Next(1, screenHeight - 2);
                }

                for (int i = 0; i < snakeBodyX.Count; i++)
                {
                    Console.SetCursorPosition(snakeBodyX[i], snakeBodyY[i]);
                    Console.Write("■");
                    if (snakeBodyX[i] == snakeHead.xpos && snakeBodyY[i] == snakeHead.ypos)
                    {
                        isGameOver = true;
                    }
                }

                if (isGameOver)
                {
                    break;
                }

                Console.SetCursorPosition(snakeHead.xpos, snakeHead.ypos);
                Console.ForegroundColor = snakeHead.color;
                Console.Write("■");

                Console.SetCursorPosition(berryX, berryY);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");

                startTime = DateTime.Now;
                buttonPressed = "no";

                while (true)
                {
                    currentTime = DateTime.Now;
                    if (currentTime.Subtract(startTime).TotalMilliseconds > 500) { break; }

                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true);

                        if (key.Key.Equals(ConsoleKey.UpArrow) && movement != "DOWN" && buttonPressed == "no")
                        {
                            movement = "UP";
                            buttonPressed = "yes";
                        }
                        if (key.Key.Equals(ConsoleKey.DownArrow) && movement != "UP" && buttonPressed == "no")
                        {
                            movement = "DOWN";
                            buttonPressed = "yes";
                        }
                        if (key.Key.Equals(ConsoleKey.LeftArrow) && movement != "RIGHT" && buttonPressed == "no")
                        {
                            movement = "LEFT";
                            buttonPressed = "yes";
                        }
                        if (key.Key.Equals(ConsoleKey.RightArrow) && movement != "LEFT" && buttonPressed == "no")
                        {
                            movement = "RIGHT";
                            buttonPressed = "yes";
                        }
                    }
                }

                snakeBodyX.Add(snakeHead.xpos);
                snakeBodyY.Add(snakeHead.ypos);

                switch (movement)
                {
                    case "UP":
                        snakeHead.ypos--;
                        break;
                    case "DOWN":
                        snakeHead.ypos++;
                        break;
                    case "LEFT":
                        snakeHead.xpos--;
                        break;
                    case "RIGHT":
                        snakeHead.xpos++;
                        break;
                }

                if (snakeBodyX.Count > score)
                {
                    snakeBodyX.RemoveAt(0);
                    snakeBodyY.RemoveAt(0);
                }
            }

            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        }

        class SnakeSegment
        {
            public int xpos { get; set; }
            public int ypos { get; set; }
            public ConsoleColor color { get; set; }
        }
    }
}
