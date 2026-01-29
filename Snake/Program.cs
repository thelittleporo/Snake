using System.Data;
using System.Net.Cache;
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
        string cage = "________________\n" +
                      "|              |\n" +
                      "|              |\n" +
                      "|              |\n" +
                      "|              |\n" +
                      "|              |\n" +
                      "|              |\n" +
                      "|              |\n" +
                      "¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯";
        bool isGameOver = false;

        ShowWelcomeScreen();
        InitializeGame(cage, cursorLeft, cursorTop, snakeHead);

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
        Console.WriteLine("====================");
        Console.WriteLine("     Game Over!");
        Console.ReadKey();
    }

    private static bool CheckGameOver(int cursorTop, int cursorLeft)
    {
        if (cursorTop <= 0 || cursorLeft <= 0)
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
        Console.ReadKey();
        Console.Clear();
    }
    private static void InitializeGame(string cage, int cursorLeft, int cursorTop, string snakeHead)
    {
        Console.WriteLine(cage);
        Console.SetCursorPosition(cursorLeft, cursorTop);
        Console.WriteLine(snakeHead);
        Console.WriteLine("\n\n\n\n\nW,A,S,D zum Steuern verwenden!");
        //var input = Console.ReadKey(true);
        Console.CursorVisible = false;
        //TextWriter standardOutput = Console.Out; //Standard Output des Konsolentexts speicher - Text wird angezeigt
    }
}
