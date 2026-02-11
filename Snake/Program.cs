using Snake;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.Cache;
using System.Numerics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Snake2;

internal class Program
{
    public const string SNAKE_HEAD_SYMBOL = ">"; //🐲
    public const string SNAKE_BODY_SYMBOL = "+";
    public const string FRUIT_SYMBOL = "*";
    public const string SPACE = " ";

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8; //für Unicode Zeichen       
        //ShowWelcomeScreen();

        await StartSingleRoundOfSnake();
    }

    private async static Task StartSingleRoundOfSnake()
    {
        bool isGameOver = false;

        //DrawGamePanel gibt die Größe des Spielfeldes zurück
        int[] fieldValues = DrawGamePanel();
        int playFieldWidth = fieldValues[0];
        int playFieldHeight = fieldValues[1];

        Position snakeHeadPos = new Position();

        snakeHeadPos.x = playFieldWidth / 2;
        snakeHeadPos.y = playFieldHeight / 2 + 1;

        DrawAtPosition(snakeHeadPos, SNAKE_HEAD_SYMBOL);

        List<Position> snakePositions = new List<Position>();
        //snakePositions.Add(snakeHeadPos);

        string currentPressedKey = "";
        string lastPressedKey = "";
        var currentMovement = "";

        Position fruitPos = new Position();
        bool isFruitExisting = false;
        int snakeLenght = 0;
        int i = 0;


        while (isGameOver == false)
        {
            if (isFruitExisting == false)
            {
                int[] fruitPosition = GetRandomFruitPositions(playFieldWidth, playFieldHeight, snakeHeadPos, fruitPos);
                fruitPos.x = fruitPosition[0];
                fruitPos.y = fruitPosition[1];
                Console.SetCursorPosition(fruitPos.x, fruitPos.y);
                Console.WriteLine(FRUIT_SYMBOL);
                isFruitExisting = true;
            }

            //Wenn Taste gedrückt wird
            if (Console.KeyAvailable)
            {
                //Speicher den Wert der Taste in eine Variable
                currentPressedKey = Console.ReadKey(true).Key.ToString();

                if (currentPressedKey == lastPressedKey)
                {
                    continue;
                }
                lastPressedKey = currentPressedKey;
            }
            if (IsPressedKeyMovementKey(currentPressedKey))
            {
                //aktuelle Position des Schlangenkopfes löschen
                //DrawAtPosition(snakeHeadPos, SPACE);
                //aktuelle Schlange zeichnen

                if (currentPressedKey == "W")
                {
                    snakeHeadPos.y--;
                }
                if (currentPressedKey == "A")
                {
                    snakeHeadPos.x--;
                }
                if (currentPressedKey == "S")
                {
                    snakeHeadPos.y++;
                }
                if (currentPressedKey == "D")
                {
                    snakeHeadPos.x++;
                }

                DrawAtPosition(snakeHeadPos, SNAKE_HEAD_SYMBOL);
                currentMovement = currentPressedKey;
                isGameOver = CheckGameOver(snakeHeadPos, playFieldWidth, playFieldHeight);
                currentPressedKey = "";
            }
            else
            {
                if (currentMovement == "W" || currentMovement == "A" || currentMovement == "S" || currentMovement == "D")
                {
                    //aktuelle Position des Schlangenkopfes löschen
                    //DrawAtPosition(snakeHeadPos, SPACE);
                    //aktuelle Schlange zeichnen

                    if (currentMovement == "W")
                    {
                        snakeHeadPos.y--;
                    }
                    else if (currentMovement == "A")
                    {
                        snakeHeadPos.x--;
                    }
                    else if (currentMovement == "S")
                    {
                        snakeHeadPos.y++;
                    }
                    else if (currentMovement == "D")
                    {
                        snakeHeadPos.x++;
                    }
                    DrawAtPosition(snakeHeadPos, SNAKE_HEAD_SYMBOL);
                    isGameOver = CheckGameOver(snakeHeadPos, playFieldWidth, playFieldHeight);
                }
            }
            bool isFruitField = IsSnakeOnFruitField(snakeHeadPos, fruitPos);

            if (isFruitField)
            {
                Position previouSnakeHeadPos = new Position();
                previouSnakeHeadPos.x = snakeHeadPos.x;
                previouSnakeHeadPos.y = snakeHeadPos.y;
                snakePositions.Add(previouSnakeHeadPos);
                snakeLenght++;
                isFruitExisting = false;
            }        

            await Task.Delay(200);
        }
        ShowGameOverScreen();
    }

    private static void DrawAtPosition(Position Pos, string textOutput)
    {
        Console.SetCursorPosition(Pos.x, Pos.y);
        Console.Write(textOutput);
    }

    private static bool IsSnakeOnFruitField(Position snakePos, Position fruitPos)
    {
        if (snakePos.y == fruitPos.y && snakePos.x == fruitPos.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static int[] GetRandomFruitPositions(int playFieldWidth, int playFieldHeight, Position snakePos, Position fruitPos)
    {
        Random random = new Random();
        fruitPos.x = random.Next(1, playFieldWidth - 2);
        fruitPos.y = random.Next(1, playFieldHeight - 1);
        if (fruitPos.x == snakePos.x && fruitPos.y == snakePos.y)
        {
            fruitPos.x += 2;
        }

        int[] FruitPositions = { fruitPos.x, fruitPos.y };
        return FruitPositions;
    }

    private static void ShowGameOverScreen()
    {
        Console.Clear();
        Console.WriteLine("=====================");
        Console.WriteLine("     Game Over!   ");
        Console.ReadKey();
    }

    private static bool CheckGameOver(Position snakePos, int playFieldWidth, int playFieldHeight)
    {
        if (snakePos.y <= 0 || snakePos.x <= 0 || snakePos.y == (playFieldHeight + 1) || snakePos.x == (playFieldWidth - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static bool IsPressedKeyMovementKey(string? pressedKey)
    {
        if (pressedKey == "W" || pressedKey == "A" || pressedKey == "S" || pressedKey == "D")
            return true;
        else
            return false;
    }

    private static void ShowWelcomeScreen()
    {
        Console.WriteLine("=========================");
        Console.WriteLine("        SNAKE           ");
        Console.WriteLine("      ++++++++      \r\n    ++        ++    \r\n   ++          ++   \r\n   + ++      ++ +   \r\n   ++          ++   \r\n    ++         +    \r\n     + +    + ++    \r\n      ++++++++      \r\n         +++        \r\n        ++++        \r\n       ++  ++       ");
        Console.WriteLine("=========================");
        Console.WriteLine("\nPress any Key to Play!");
        Console.ReadKey();
        Console.Clear();
    }
    private static int[] DrawGamePanel()
    {
        int playFieldWidth;
        int playFieldHeight;
        const int maxWidth = 100;
        const int minWidth = 5;
        const int maxHeight = 25;
        const int minHeight = 5;

        //Console.WriteLine("Tipp: Für schöneres Ergebnis ungerade Zahlen eingeben!\n");
        //Console.Write("Please enter field size horizontal (default 15): ");

        //while (!int.TryParse(Console.ReadLine(), out playFieldWidth) || playFieldWidth <= maxWidth && playFieldWidth <= minWidth)
        //{
        //    Console.Write($"Bitte eine Zahl zwischen {minWidth} und {maxWidth} eingeben: ");
        //}
        //Console.Write("Please enter field size vertical (default 7): ");

        //while (!int.TryParse(Console.ReadLine(), out playFieldHeight) || playFieldHeight <= maxHeight && playFieldHeight <= minHeight)
        //{
        //    Console.Write($"Bitte eine Zahl zwischen {minHeight} und {maxHeight} eingeben: ");
        //}
        playFieldWidth = 15;
        playFieldHeight = 7;

        Console.Clear();

        //Cage erstellen
        Console.Write("╔");
        for (int i = playFieldWidth - 2; i > 0; i--) //-2, da die Ecken abgezogen werden
        {
            Console.Write("═");
        }
        Console.Write("╗");
        for (int i = playFieldHeight; i > 0; i--)
        {
            Console.Write("\n║");
            for (int n = playFieldWidth - 2; n > 0; n--)
            {
                Console.Write(" ");
            }
            Console.Write("║");
        }
        Console.WriteLine();
        Console.Write("╚");
        for (int i = playFieldWidth - 2; i > 0; i--) //-2, da die Ecken abgezogen werden
        {
            Console.Write("═");
        }
        Console.Write("╝");

        Console.CursorVisible = false;

        int[] returnFieldvalues = { playFieldWidth, playFieldHeight };
        return returnFieldvalues;
    }
}
