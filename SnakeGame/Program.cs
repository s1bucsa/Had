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
        static ConsoleColor borderColor = ConsoleColor.Green;
        static ConsoleColor snakeColor = ConsoleColor.Yellow;
        static ConsoleColor berryColor = ConsoleColor.Cyan;

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
                Thread.Sleep(300);
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
            // Uložení poslední části hada pro pozdější vymazání
            Pixel lastPart = snakeBody.Count > 0 ? snakeBody[0] : null;

            // Přidání nové hlavy hada
            snakeBody.Add(new Pixel { X = snakeHead.X, Y = snakeHead.Y, Color = snakeColor });

            // Pohyb hlavy podle směru
            switch (movement)
            {
                case "UP": snakeHead.Y--; break;
                case "DOWN": snakeHead.Y++; break;
                case "LEFT": snakeHead.X--; break;
                case "RIGHT": snakeHead.X++; break;
            }

            // Udržení délky hada odpovídající skóre
            if (snakeBody.Count > score)
            {
                // Vymazání posledního segmentu
                Console.SetCursorPosition(lastPart.X, lastPart.Y);
                Console.Write(" ");

                // Odstranění segmentu z listu
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
