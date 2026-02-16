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
    public const string SNAKE_HEAD_SYMBOL = "█";
    public const string SNAKE_BODY_SYMBOL = "█";
    public const string FRUIT_SYMBOL = "●";
    public const string SPACE = " ";
    public const string BARRIER = "⚠";

    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8; //für Unicode Zeichen       
        ShowWelcomeScreen();

        while (true)
        {
            await StartSingleRoundOfSnake();
        }
    }

    private async static Task StartSingleRoundOfSnake()
    {
        bool isGameOver = false;
        int difficulty = SetDifficulty();

        //DrawGamePanel gibt die Größe des Spielfeldes zurück
        int[] fieldValues = DrawGamePanel();
        int playFieldWidth = fieldValues[0];
        int playFieldHeight = fieldValues[1];

        Position snakeHeadPos = new Position();

        snakeHeadPos.x = playFieldWidth / 2;
        snakeHeadPos.y = playFieldHeight / 2 + 1;

        DrawAtPosition(snakeHeadPos, SNAKE_HEAD_SYMBOL);

        //allSnakePositions wird nie gelöscht und speichert somit x Positionen des Schlangenkörpers
        List<Position> allSnakePositions = new List<Position>();
        //curentSnakePosition speichert immer nur für 1 Runde, alle Positionen des aktuellen Schlangenkörpers
        List<Position> currentSnakePosition = new List<Position>();
        //speichert alle Positionen der Barrieren
        List<Position> barrierPositions = new List<Position>();

        string currentPressedKey = "";
        string lastPressedKey = "";
        var currentMovement = "";

        Position fruitPos = new Position();
        bool isFruitExisting = false;
        int snakeBodyLenght = 0;
        int roundsSinceFirstFruit = 0;
        bool DoesSnakeBodyExist = false;
        int delay = 200;

        while (isGameOver == false)
        {
            Console.SetCursorPosition(0, playFieldHeight + 2);
            Console.WriteLine($"Score: {snakeBodyLenght}");

            if (isFruitExisting == false)
            {
                if (difficulty > 1)
                {
                    //Barriere erstellen
                    Position barrierPos = new Position();
                    int[] barrierPosition = GetRandomPosition(playFieldWidth, playFieldHeight, snakeHeadPos, fruitPos, currentSnakePosition);
                    barrierPos.x = barrierPosition[0];
                    barrierPos.y = barrierPosition[1];
                    Console.SetCursorPosition(barrierPos.x, barrierPos.y);
                    Console.WriteLine(BARRIER);
                    barrierPositions.Add(barrierPos);
                }

                //Frucht erstellen
                int[] fruitPosition = GetRandomPosition(playFieldWidth, playFieldHeight, snakeHeadPos, fruitPos, currentSnakePosition);
                fruitPos.x = fruitPosition[0];
                fruitPos.y = fruitPosition[1];
                while (barrierPositions.Contains(fruitPos))
                {
                    fruitPosition = GetRandomPosition(playFieldWidth, playFieldHeight, snakeHeadPos, fruitPos, currentSnakePosition);
                    fruitPos.x = fruitPosition[0];
                    fruitPos.y = fruitPosition[1];
                }
                Console.SetCursorPosition(fruitPos.x, fruitPos.y);
                Console.WriteLine(FRUIT_SYMBOL);
                isFruitExisting = true;              
            }

            currentSnakePosition.Clear();

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
            //Schlangenkopf vor Bewegung speichern - für den Körper, falls eine Frucht eingesammelt wird
            Position previouSnakeHeadPos = GetPreviousSnakePos(snakeHeadPos);

            if (IsPressedKeyMovementKey(currentPressedKey))
            {
                //Schlange (Kopf und Körper) löschen
                DrawAtPosition(snakeHeadPos, SPACE);
                if (DoesSnakeBodyExist)
                {
                    for (int i = 0; snakeBodyLenght > i ; i++)
                    {
                        DrawAtPosition(allSnakePositions[roundsSinceFirstFruit - 1 - i], SPACE);
                    }
                }

                DoMovement(currentPressedKey, snakeHeadPos);

                currentMovement = currentPressedKey;
                currentPressedKey = "";
            }
            else if (IsPressedKeyMovementKey(currentMovement))
            {
                //Schlange (Kopf und Körper) löschen
                DrawAtPosition(snakeHeadPos, SPACE);
                if (DoesSnakeBodyExist)
                {
                    for (int i = 0; snakeBodyLenght > i; i++)
                    {
                        DrawAtPosition(allSnakePositions[roundsSinceFirstFruit - 1 - i], SPACE);
                    }
                }
                DoMovement(currentMovement, snakeHeadPos);
            }

            if (IsSnakeOnFruitField(snakeHeadPos, fruitPos))
            {
                snakeBodyLenght++;
                isFruitExisting = false;
                DoesSnakeBodyExist = true;
            }

            //Schlangenkopf zeichnen
            DrawAtPosition(snakeHeadPos, SNAKE_HEAD_SYMBOL);

            if (DoesSnakeBodyExist)
            {
                allSnakePositions.Add(previouSnakeHeadPos);

                //Schlangenkörper zeichnen
                for (int i = 0; snakeBodyLenght > i; i++)
                {
                    DrawAtPosition(allSnakePositions[roundsSinceFirstFruit - i], SNAKE_BODY_SYMBOL);
                    currentSnakePosition.Add(allSnakePositions[roundsSinceFirstFruit - i]);
                }
                roundsSinceFirstFruit++;
            }
            isGameOver = CheckGameOver(snakeHeadPos, playFieldWidth, playFieldHeight, currentSnakePosition, barrierPositions);

            if (difficulty < 3)
            {
                await Task.Delay(delay);
            }
            else
            {
                await Task.Delay(delay);
                if (roundsSinceFirstFruit % 2 == 0)
                {
                    delay = delay - 1;
                }
            }

        }
        await ShowGameOverScreenAsync(snakeBodyLenght);
    }

    private static Position GetPreviousSnakePos(Position snakeHeadPos)
    {
        Position previouSnakeHeadPos = new Position();
        previouSnakeHeadPos.x = snakeHeadPos.x;
        previouSnakeHeadPos.y = snakeHeadPos.y;

        return previouSnakeHeadPos;
    }

    private static void DoMovement(string pressedKey, Position snakeHeadPos)
    {
        if (pressedKey == "W" || pressedKey == "UpArrow")
        {
            snakeHeadPos.y--;
        }
        if (pressedKey == "A" || pressedKey == "LeftArrow")
        {
            snakeHeadPos.x--;
        }
        if (pressedKey == "S" || pressedKey == "DownArrow")
        {
            snakeHeadPos.y++;
        }
        if (pressedKey == "D" || pressedKey == "RightArrow")
        {
            snakeHeadPos.x++;
        }
    }

    private static void DrawAtPosition(Position Pos, string textOutput)
    {
        Console.SetCursorPosition(Pos.x, Pos.y);
        Console.Write(textOutput);
    }

    private static bool IsSnakeOnFruitField(Position snakeHeadPos, Position fruitPos)
    {
        if (snakeHeadPos.y == fruitPos.y && snakeHeadPos.x == fruitPos.x)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static int[] GetRandomPosition(int playFieldWidth, int playFieldHeight, Position snakePos, Position fruitPos, List <Position> currentSnakePosition)
    {
        Random random = new Random();
        fruitPos.x = random.Next(1, playFieldWidth - 2);
        fruitPos.y = random.Next(1, playFieldHeight - 1);

        while (fruitPos == snakePos || currentSnakePosition.Contains(fruitPos))
        {
            fruitPos.x = random.Next(1, playFieldWidth - 2);
            fruitPos.y = random.Next(1, playFieldHeight - 1);
        }

        int[] FruitPositions = { fruitPos.x, fruitPos.y };
        return FruitPositions;
    }

    private static async Task ShowGameOverScreenAsync(int snakeBodyLenght)
    {
        Console.Clear();
        Console.WriteLine("   _____          __  __ ______    ______      ________ _____  \r\n" +
            "  / ____|   /\\   |  \\/  |  ____|  / __ \\ \\    / /  ____|  __ \\ \r\n" +
            " | |  __   /  \\  | \\  / | |__    | |  | \\ \\  / /| |__  | |__) |\r\n" +
            " | | |_ | / /\\ \\ | |\\/| |  __|   | |  | |\\ \\/ / |  __| |  _  / \r\n" +
            " | |__| |/ ____ \\| |  | | |____  | |__| | \\  /  | |____| | \\ \\ \r\n" +
            "  \\_____/_/    \\_\\_|  |_|______|  \\____/   \\/   |______|_|  \\_\\\r\n" +
            "                                                               \r\n" +
            "                                                               ");
        Console.WriteLine($"\nYour Score is: {snakeBodyLenght}");
        await Task.Delay(800);

        Console.WriteLine("\n\nPress any key to play again!");
        Console.ReadKey();
        Console.Clear();
    }

    private static bool CheckGameOver(Position snakeHeadPos, int playFieldWidth, int playFieldHeight, List <Position> currentSnakeBodyPosition, List<Position> barrierPositions)
    {

        if (snakeHeadPos.y <= 0 || snakeHeadPos.x <= 0 || snakeHeadPos.y == (playFieldHeight + 1) || snakeHeadPos.x == (playFieldWidth - 1))
        {
            return true;
        }
        else if (currentSnakeBodyPosition.Contains(snakeHeadPos) || (barrierPositions.Contains(snakeHeadPos)))
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
        if (pressedKey == "W" || pressedKey == "A" || pressedKey == "S" || pressedKey == "D" || 
            pressedKey == "UpArrow" || pressedKey == "RightArrow" || pressedKey == "LeftArrow" || pressedKey == "DownArrow")
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
        const int maxHeight = 24;
        const int minHeight = 5;

        Console.WriteLine("Tipp: Für schöneres Ergebnis ungerade Zahlen eingeben!\n");
        Console.Write($"Please enter field size horizontal between {minWidth} and {maxWidth}: ");

        while (!int.TryParse(Console.ReadLine(), out playFieldWidth) || (playFieldWidth > maxWidth || playFieldWidth < minWidth))
        {
            Console.Write($"Please enter a number between {minWidth} and {maxWidth} eingeben: ");
        }
        Console.Write($"Please enter field size vertical between {minHeight} and {maxHeight}: ");

        while (!int.TryParse(Console.ReadLine(), out playFieldHeight) || (playFieldHeight > maxHeight || playFieldHeight < minHeight))
        {
            Console.Write($"Please enter a number between {minHeight} and {maxHeight}: ");
        }
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

    private static int SetDifficulty()
    {
        int difficulty = 0;

        Console.Write("Select difficulty level normal = 1, medium = 2, difficult = 3: ");

        while (!int.TryParse(Console.ReadLine(), out difficulty) || (difficulty > 3 || difficulty < 1))
        {
            Console.Write($"Please enter the number 1, 2 or 3: ");
        }
        Console.Clear();
        return difficulty;
        
    }
}
