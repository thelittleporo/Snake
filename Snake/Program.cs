using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Net.Cache;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Snake2;

internal class Program
{
    static async Task Main(string[] args)
    {
        //Random random = new Random();
        int cursorHorizontal = 0;
        int cursorVertikal = 0;
        string snakeHead = ">";
        bool isGameOver = false;

        //ShowWelcomeScreen();
        int[] returnvalue = DrawGamePanel(cursorHorizontal, cursorVertikal, snakeHead);

        int horizonfield = returnvalue[0];
        int verticalfield = returnvalue[1];

        cursorHorizontal = horizonfield / 2;
        cursorVertikal = verticalfield / 2 + 1;

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
                
                Console.SetCursorPosition(cursorHorizontal, cursorVertikal);
                await Task.Delay(200);
                Console.Write(" ");

                if (currentPressedKey == "W")
                {
                    cursorVertikal = cursorVertikal - 1;
                    ShowSnakeHead(cursorHorizontal, cursorVertikal, snakeHead);
                    currentMovement = "W";
                    isGameOver = CheckGameOver(cursorVertikal, cursorHorizontal, horizonfield, verticalfield);
                }
                if (currentPressedKey == "A" /*&& cursorLeft > 1*/)
                {
                    cursorHorizontal = cursorHorizontal - 1;
                    ShowSnakeHead(cursorHorizontal, cursorVertikal, snakeHead);
                    currentMovement = "A";
                    isGameOver = CheckGameOver(cursorVertikal, cursorHorizontal, horizonfield, verticalfield);
                }
                if (currentPressedKey == "S")
                {
                    cursorVertikal = cursorVertikal + 1;
                    ShowSnakeHead(cursorHorizontal, cursorVertikal, snakeHead);
                    currentMovement = "S";
                    isGameOver = CheckGameOver(cursorVertikal, cursorHorizontal, horizonfield, verticalfield);
                }
                if (currentPressedKey == "D")
                {
                    cursorHorizontal = cursorHorizontal + 1;
                    ShowSnakeHead(cursorHorizontal, cursorVertikal, snakeHead);
                    currentMovement = "D";
                    isGameOver = CheckGameOver(cursorVertikal, cursorHorizontal, horizonfield, verticalfield);
                }
            }
            else
            {
                if (currentMovement == "W")
                {
                    cursorVertikal = cursorVertikal - 1;
                    ShowSnakeHead(cursorHorizontal, cursorVertikal, snakeHead);
                    Console.SetCursorPosition(cursorHorizontal, cursorVertikal);
                    await Task.Delay(200);
                    Console.Write(" ");
                }
                if (currentMovement == "A")
                {
                    cursorHorizontal = cursorHorizontal - 1;
                    ShowSnakeHead(cursorHorizontal, cursorVertikal, snakeHead);
                    currentMovement = "A";
                    Console.SetCursorPosition(cursorHorizontal, cursorVertikal);
                    await Task.Delay(200);
                    Console.Write(" ");
                }
                if (currentMovement == "S")
                {
                    cursorVertikal = cursorVertikal + 1;
                    ShowSnakeHead(cursorHorizontal, cursorVertikal, snakeHead);
                    currentMovement = "S";
                    Console.SetCursorPosition(cursorHorizontal, cursorVertikal);
                    await Task.Delay(200);
                    Console.Write(" ");
                }
                if (currentMovement == "D")
                {
                    cursorHorizontal = cursorHorizontal + 1;
                    ShowSnakeHead(cursorHorizontal, cursorVertikal, snakeHead);
                    currentMovement = "D";
                    Console.SetCursorPosition(cursorHorizontal, cursorVertikal);
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

    private static bool CheckGameOver(int cursorVertikal, int cursorHorizontal, int horizonfield, int verticalfield)
    {
        if (cursorVertikal <= 0 || cursorHorizontal <= 0 || cursorVertikal == (verticalfield +1) || cursorHorizontal == horizonfield)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void ShowSnakeHead(int cursorHorizontal, int cursorVertikal, string snakeHead)
    {
        Console.SetCursorPosition(cursorHorizontal, cursorVertikal);
        Console.Write(snakeHead);
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
    private static int[] DrawGamePanel(int cursorVertikal, int cursorHorizontal, string snakeHead)
    {
        int horizonfield;
        int verticalfield;
        const int maxwidth = 100;
        const int maxheight = 25;

        Console.WriteLine("Tipp: Für schöneres Ergebnis ungerade Zahlen eingeben!\n");       
        Console.Write("Please enter field size horizontal (default 15): ");

        while (!int.TryParse(Console.ReadLine() , out horizonfield) && horizonfield <= maxwidth)
        {
            Console.Write("Bitte eine Zahl zwischen 25 und 100 eingeben: ");
        }
        Console.Write("Please enter field size vertical (default 7): ");
        
        while (!int.TryParse(Console.ReadLine(), out verticalfield) && verticalfield <= maxheight)
        {
            Console.Write("Bitte eine Zahl zwischen 25 und 100 eingeben: ");
        }      
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
        
        //Cursor in die Mitte des Feldes setzen
        cursorHorizontal = (horizonfield / 2);
        cursorVertikal = verticalfield / 2 + 1; //+1, da erste Zeile mitgezählt wird

        Console.SetCursorPosition(cursorHorizontal, cursorVertikal);
        Console.WriteLine(snakeHead);
        Console.CursorVisible = false;

        int[] returnvalue = { horizonfield, verticalfield };
        return returnvalue;
    }
}
