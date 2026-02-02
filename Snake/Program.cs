using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.Cache;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Snake2;

internal class Program
{
    static async Task Main(string[] args)
    {
        Random random = new Random();
        int randomNumber = random.Next(0, 105); //Für Früchte später
        int cursorLeft = 7;
        int cursorTop = 4;
        string snakeHead = ">";
        bool isGameOver = false;

        //ShowWelcomeScreen();
        DrawGamePanel(cursorLeft, cursorTop, snakeHead);

        //List<char> movementkeys = new List<char>()
        //{
        //   'W'
        //};

        string currentPressedKey = "";
        var currentMovement = "";

        while (isGameOver == false)
        {
            if (Console.KeyAvailable) //Wenn Taste gedrückt wird
            {
                currentPressedKey = Console.ReadKey(true).Key.ToString(); //Speicher den Wert der Taste in eine Variable
            }

            if (IsPressedKeyMovementKey(currentPressedKey) /*movementkeys*/) //Bewegungstaste?
            {
                //aktuelle Position des Schlangenkopfes löschen und warten
                
                Console.SetCursorPosition(cursorLeft, cursorTop);
                await Task.Delay(200);
                Console.Write(" ");

                if (currentPressedKey == "W")
                {
                    cursorTop = cursorTop - 1;
                    ShowSnakeHead(cursorLeft, cursorTop, snakeHead);
                    currentMovement = "W";
                    isGameOver = CheckGameOver(cursorTop, cursorLeft);
                }
                if (currentPressedKey == "A" /*&& cursorLeft > 1*/)
                {
                    cursorLeft = cursorLeft - 1;
                    ShowSnakeHead(cursorLeft, cursorTop, snakeHead);
                    currentMovement = "A";
                    isGameOver = CheckGameOver(cursorTop, cursorLeft);
                }
                if (currentPressedKey == "S")
                {
                    cursorTop = cursorTop + 1;
                    ShowSnakeHead(cursorLeft, cursorTop, snakeHead);
                    currentMovement = "S";
                    isGameOver = CheckGameOver(cursorTop, cursorLeft);
                }
                if (currentPressedKey == "D")
                {
                    cursorLeft = cursorLeft + 1;
                    ShowSnakeHead(cursorLeft, cursorTop, snakeHead);
                    currentMovement = "D";
                    isGameOver = CheckGameOver(cursorTop, cursorLeft);
                }
            }
            else
            {
                if (currentMovement == "W")
                {
                    cursorTop = cursorTop - 1;
                    ShowSnakeHead(cursorLeft, cursorTop, snakeHead);
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    await Task.Delay(200);
                    Console.Write(" ");
                }
                if (currentMovement == "A")
                {
                    cursorLeft = cursorLeft - 1;
                    ShowSnakeHead(cursorLeft, cursorTop, snakeHead);
                    currentMovement = "A";
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    await Task.Delay(200);
                    Console.Write(" ");
                }
                if (currentMovement == "S")
                {
                    cursorTop = cursorTop + 1;
                    ShowSnakeHead(cursorLeft, cursorTop, snakeHead);
                    currentMovement = "S";
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    await Task.Delay(200);
                    Console.Write(" ");
                }
                if (currentMovement == "D")
                {
                    cursorLeft = cursorLeft + 1;
                    ShowSnakeHead(cursorLeft, cursorTop, snakeHead);
                    currentMovement = "D";
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    await Task.Delay(200);
                    Console.Write(" ");
                }
            }
        }
        ShowGameOverScreen();
    }
    private static void ShowGameOverScreen()
    {
        Console.Clear();
        Console.WriteLine("=====================");
        Console.WriteLine("     Game Over!   ");
        Console.ReadKey();
    }

    private static bool CheckGameOver(int cursorTop, int cursorLeft)
    {
        if (cursorTop <= 0 || cursorLeft <= 0 || cursorTop >= 8 || cursorLeft >= 15)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void ShowSnakeHead(int cursorLeft, int cursorTop, string snakeHead)
    {
        Console.SetCursorPosition(cursorLeft, cursorTop);
        Console.Write(snakeHead);
    }

    private static bool IsPressedKeyMovementKey(string? pressedKey/*, List<char> movementkeys*/)
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
        Console.WriteLine("\n\nUse W,A,S,D");
        Console.ReadKey();
        Console.Clear();
    }
    private static void DrawGamePanel(int cursorVertikal, int cursorHorizontal, string snakeHead)
    {
        Console.Write("Please enter field size horizontal (default 15): ");
        int horizonfield = Convert.ToInt32(Console.ReadLine());
        Console.Write("Please enter field size vertical(default 8): ");
        int verticalfield = Convert.ToInt32(Console.ReadLine());
        Console.Clear();

        //Cage erstellen
        for (int i = horizonfield; i > 0; i--)
        {
            Console.Write("_");
        }
        for (int i = verticalfield; i > 0; i--)
        {
            Console.Write("\n|");
            for (int n = horizonfield-2; n > 0; n--)
            {
                Console.Write(" ");
            }
            Console.Write("|");
        }
        Console.WriteLine();
        for (int i = horizonfield; i > 0; i--)
        {
            Console.Write("¯");
        }
        
        cursorHorizontal = (horizonfield / 2 + 1);
        cursorVertikal = verticalfield / 2;

        Console.SetCursorPosition(cursorVertikal, cursorHorizontal);
        Console.WriteLine(snakeHead);
        //var input = Console.ReadKey(true);
        Console.CursorVisible = false;
        //TextWriter standardOutput = Console.Out; //Standard Output des Konsolentexts speicher - Text wird angezeigt
    }
}
