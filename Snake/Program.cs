using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.Cache;
using System.Numerics;
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

        int snakePositionX = 0;
        int snakePositionY = 0;

        //DrawGamePanel gibt die Größe des Spielfeldes zurück
        int[] fieldValues = DrawGamePanel(); 
        int playFieldWidth = fieldValues[0];
        int playFieldHeight = fieldValues[1];

        snakePositionX = playFieldWidth / 2;
        snakePositionY = playFieldHeight / 2 + 1;
        DrawAtPosition(snakePositionX, snakePositionY, SNAKE_HEAD_SYMBOL);
        List<int> snakePositions = new List<int>();
        snakePositions.Add(snakePositionX);
        snakePositions.Add(snakePositionY);

        string currentPressedKey = "";
        var currentMovement = "";

        int fruitPosX = 0;
        int fruitPosY = 0;
        bool isFruitExisting = false;



        while (isGameOver == false)
        {
            if (isFruitExisting == false)
            {
                int[] fruitPosition = GetRandomFruitPositions(playFieldWidth, playFieldHeight, snakePositionX, snakePositionY);
                fruitPosX = fruitPosition[0];
                fruitPosY = fruitPosition[1];
                Console.SetCursorPosition(fruitPosX, fruitPosY);
                Console.WriteLine(FRUIT_SYMBOL);
                isFruitExisting = true;
            }

            int snakeEndpositionX = snakePositions[snakePositions.Count - 2];
            int snakeEndpositionY = snakePositions[snakePositions.Count - 1];

            //Wenn Taste gedrückt wird
            if (Console.KeyAvailable) 
            {
                currentPressedKey = Console.ReadKey(true).Key.ToString(); //Speicher den Wert der Taste in eine Variable
            }

            if (IsPressedKeyMovementKey(currentPressedKey))
            {
                //aktuelle Position des Schlangenkopfes löschen
                DrawAtPosition(snakePositionX, snakePositionY, SPACE);

                if (currentPressedKey == "W")
                {
                    snakePositionY = snakePositionY - 1;
                }
                if (currentPressedKey == "A")
                {
                    snakePositionX = snakePositionX - 1;
                }
                if (currentPressedKey == "S")
                {
                    snakePositionY = snakePositionY + 1;
                }
                if (currentPressedKey == "D")
                {
                    snakePositionX = snakePositionX + 1;
                }

                DrawAtPosition(snakePositionX, snakePositionY, SNAKE_HEAD_SYMBOL);
                currentMovement = currentPressedKey;
                isGameOver = CheckGameOver(snakePositionY, snakePositionX, playFieldWidth, playFieldHeight);
                currentPressedKey = "";
            }
            
            else
            {
                if (currentMovement == "W" || currentMovement == "A" || currentMovement == "S" || currentMovement == "D")
                {
                    //aktuelle Position des Schlangenkopfes löschen
                    DrawAtPosition(snakePositionX, snakePositionY, SPACE);

                    if (currentMovement == "W")
                    {
                        snakePositionY = snakePositionY - 1;
                    }
                    if (currentMovement == "A")
                    {
                        snakePositionX = snakePositionX - 1;
                    }
                    if (currentMovement == "S")
                    {
                        snakePositionY = snakePositionY + 1;
                    }
                    if (currentMovement == "D")
                    {
                        snakePositionX = snakePositionX + 1;
                    }

                    DrawAtPosition(snakePositionX, snakePositionY, SNAKE_HEAD_SYMBOL);
                    isGameOver = CheckGameOver(snakePositionY, snakePositionX, playFieldWidth, playFieldHeight);
                }
            }

            bool isFruitField = IsSnakeOnFruitField(snakePositionY, snakePositionX, fruitPosX, fruitPosY);

            if (isFruitField)
            {
                snakePositions.Add(snakeEndpositionX);
                snakePositions.Add(snakeEndpositionY);
                isFruitExisting = false;
            }

            await Task.Delay(200);
        }
        ShowGameOverScreen();
    }

    private static void DrawAtPosition(int xValue, int yValue, string textOutput)
    {
        Console.SetCursorPosition(xValue, yValue);
        Console.Write(textOutput);
    }

    private static bool IsSnakeOnFruitField(int snakePositionY, int snakePositionX, int fruitPosX, int fruitPosY)
    {
        if (snakePositionY == fruitPosY && snakePositionX == fruitPosX)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static int[] GetRandomFruitPositions(int playFieldWidth, int playFieldHeight, int snakePositionX, int snakePositionY)
    {
        Random random = new Random();
        int fruitPosX = random.Next(1, playFieldWidth - 2);
        int fruitPosY = random.Next(1, playFieldHeight - 1);
        if (fruitPosX == snakePositionX && fruitPosY == snakePositionY)
        {
            fruitPosX = fruitPosX + 2;
        }

        int[] FruitPositions = { fruitPosX, fruitPosY };
        return FruitPositions;
    }

    private static void ShowGameOverScreen()
    {
        Console.Clear();
        Console.WriteLine("=====================");
        Console.WriteLine("     Game Over!   ");
        Console.ReadKey();
    }

    private static bool CheckGameOver(int snakePositionY, int snakePositionX, int playFieldWidth, int playFieldHeight)
    {
        if (snakePositionY <= 0 || snakePositionX <= 0 || snakePositionY == (playFieldHeight + 1) || snakePositionX == (playFieldWidth - 1))
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
