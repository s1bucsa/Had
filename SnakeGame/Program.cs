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
        static List<Pixel> snakeBody = new List<Pixel>();
        static Pixel snakeHead = new Pixel();
        static int berryX, berryY;
        static int poisonBerryX, poisonBerryY;
        static int speedBerryX, speedBerryY;
        static ConsoleColor borderColor = ConsoleColor.Green;
        static ConsoleColor snakeColor = ConsoleColor.Yellow;
        static ConsoleColor berryColor = ConsoleColor.Green;
        static ConsoleColor poisonBerryColor = ConsoleColor.Red;
        static ConsoleColor speedBerryColor = ConsoleColor.Blue;
        static int gameSpeed = 300;

        static void Main()
        {
            InitializeGame();
            GameLoop();
        }

        static void InitializeGame()
        {
            Console.SetWindowSize(screenWidth, screenHeight);
            Console.CursorVisible = false;
            snakeHead = new Pixel { X = screenWidth / 2, Y = screenHeight / 2, Color = snakeColor };
            GenerateBerry();
        }

        static void GameLoop()
        {
            while (!isGameOver)
            {
                DrawGame();
                HandleInput();
                UpdateSnakePosition();
                CheckCollisions();
                Thread.Sleep(gameSpeed);
            }
            EndGame();
        }

        static void DrawGame()
        {
            Console.SetCursorPosition(0, 0);
            DrawBorders();
            DrawSnake();
            DrawBerry();
        }

        static void DrawBorders()
        {
            Console.ForegroundColor = borderColor;
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
            foreach (var part in snakeBody)
            {
                Console.SetCursorPosition(part.X, part.Y);
                Console.Write("■");
            }
        }

        static void DrawBerry()
        {
            Console.ForegroundColor = berryColor;
            Console.SetCursorPosition(berryX, berryY);
            Console.Write("■");

            Console.ForegroundColor = poisonBerryColor;
            Console.SetCursorPosition(poisonBerryX, poisonBerryY);
            Console.Write("■");

            Console.ForegroundColor = speedBerryColor;
            Console.SetCursorPosition(speedBerryX, speedBerryY);
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
            Pixel lastPart = snakeBody.Count > 0 ? snakeBody[0] : null;
            snakeBody.Add(new Pixel { X = snakeHead.X, Y = snakeHead.Y, Color = snakeColor });

            switch (movement)
            {
                case "UP": snakeHead.Y--; break;
                case "DOWN": snakeHead.Y++; break;
                case "LEFT": snakeHead.X--; break;
                case "RIGHT": snakeHead.X++; break;
            }

            if (snakeBody.Count > score)
            {
                Console.SetCursorPosition(lastPart.X, lastPart.Y);
                Console.Write(" ");
                snakeBody.RemoveAt(0);
            }
        }

        static void CheckCollisions()
        {
            if (snakeHead.X == 0 || snakeHead.X == screenWidth - 1 ||
                snakeHead.Y == 0 || snakeHead.Y == screenHeight - 1)
            {
                isGameOver = true;
            }

            foreach (var part in snakeBody)
            {
                if (part.X == snakeHead.X && part.Y == snakeHead.Y)
                {
                    isGameOver = true;
                }
            }

            // Had snědl obyčejnou malinu (zelená)
            if (snakeHead.X == berryX && snakeHead.Y == berryY)
            {
                score++;
                GenerateBerry();
            }

            // Had snědl jedovatou malinu (červená) - okamžitá smrt
            if (snakeHead.X == poisonBerryX && snakeHead.Y == poisonBerryY)
            {
                isGameOver = true;  // Hra končí
            }

            // Had snědl zrychlovací malinu (modrá)
            if (snakeHead.X == speedBerryX && snakeHead.Y == speedBerryY)
            {
                gameSpeed = Math.Max(50, gameSpeed - 100); // Zrychlení hry, ale ne pod 50 ms
                GenerateBerry();
            }
        }

        static void GenerateBerry()
        {
            // Nejprve vymažeme staré maliny
            Console.SetCursorPosition(berryX, berryY);
            Console.Write(" ");
            Console.SetCursorPosition(poisonBerryX, poisonBerryY);
            Console.Write(" ");
            Console.SetCursorPosition(speedBerryX, speedBerryY);
            Console.Write(" ");

            // Vygenerujeme nové pozice
            berryX = random.Next(1, screenWidth - 2);
            berryY = random.Next(1, screenHeight - 2);

            poisonBerryX = random.Next(1, screenWidth - 2);
            poisonBerryY = random.Next(1, screenHeight - 2);

            speedBerryX = random.Next(1, screenWidth - 2);
            speedBerryY = random.Next(1, screenHeight - 2);
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
