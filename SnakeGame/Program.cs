namespace Snake
{
    class Program
    {
        static int screenWidth = 64;
        static int screenHeight = 32;
        static Random random = new Random();
        static int score = 5;
        static bool isGameOver = false;
        static string movement = "RIGHT";
        static List<int> snakeBodyX = new List<int>();
        static List<int> snakeBodyY = new List<int>();
        static Pixel snakeHead = new Pixel();
        static int berryX, berryY;

        static void Main()
        {
            InitializeGame();
            GameLoop();
        }

        static void InitializeGame()
        {
            Console.WindowHeight = screenHeight;
            Console.WindowWidth = screenWidth;
            snakeHead.X = screenWidth / 2;
            snakeHead.Y = screenHeight / 2;
            snakeHead.Color = ConsoleColor.Red;
            GenerateBerry();
        }

        static void GameLoop()
        {
            while (!isGameOver)
            {
                Console.Clear();
                DrawBorders();
                DrawSnake();
                DrawBerry();
                CheckCollisions();
                HandleInput();
                UpdateSnakePosition();
                Thread.Sleep(500);
            }
            EndGame();
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

        static void DrawSnake()
        {
            Console.ForegroundColor = snakeHead.Color;
            Console.SetCursorPosition(snakeHead.X, snakeHead.Y);
            Console.Write("■");
            for (int i = 0; i < snakeBodyX.Count; i++)
            {
                Console.SetCursorPosition(snakeBodyX[i], snakeBodyY[i]);
                Console.Write("■");
            }
        }

        static void DrawBerry()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(berryX, berryY);
            Console.Write("■");
        }

        static void HandleInput()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.UpArrow && movement != "DOWN") movement = "UP";
                if (key == ConsoleKey.DownArrow && movement != "UP") movement = "DOWN";
                if (key == ConsoleKey.LeftArrow && movement != "RIGHT") movement = "LEFT";
                if (key == ConsoleKey.RightArrow && movement != "LEFT") movement = "RIGHT";
            }
        }

        static void UpdateSnakePosition()
        {
            snakeBodyX.Add(snakeHead.X);
            snakeBodyY.Add(snakeHead.Y);
            switch (movement)
            {
                case "UP": snakeHead.Y--; break;
                case "DOWN": snakeHead.Y++; break;
                case "LEFT": snakeHead.X--; break;
                case "RIGHT": snakeHead.X++; break;
            }
            if (snakeBodyX.Count > score)
            {
                snakeBodyX.RemoveAt(0);
                snakeBodyY.RemoveAt(0);
            }
        }

        static void CheckCollisions()
        {
            if (snakeHead.X == 0 || snakeHead.X == screenWidth - 1 ||
                snakeHead.Y == 0 || snakeHead.Y == screenHeight - 1)
            {
                isGameOver = true;
            }
            for (int i = 0; i < snakeBodyX.Count; i++)
            {
                if (snakeBodyX[i] == snakeHead.X && snakeBodyY[i] == snakeHead.Y)
                {
                    isGameOver = true;
                }
            }
            if (snakeHead.X == berryX && snakeHead.Y == berryY)
            {
                score++;
                GenerateBerry();
            }
        }

        static void GenerateBerry()
        {
            berryX = random.Next(1, screenWidth - 2);
            berryY = random.Next(1, screenHeight - 2);
        }

        static void EndGame()
        {
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine($"Game over, Score: {score}");
        }

        class Pixel
        {
            public int X { get; set; }
            public int Y { get; set; }
            public ConsoleColor Color { get; set; }
        }
    }
}
