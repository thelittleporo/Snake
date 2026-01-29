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
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(snakeHead);
                    currentMovement = "W";
                }
                if (currentPressedKey == "A" /*&& cursorLeft > 1*/)
                {
                    cursorLeft = cursorLeft - 1;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(snakeHead);
                    currentMovement = "A";
                }
                if (currentPressedKey == "S")
                {
                    cursorTop = cursorTop + 1;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(snakeHead);
                    currentMovement = "S";
                }
                if (currentPressedKey == "D")
                {
                    cursorLeft = cursorLeft + 1;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(snakeHead);
                    currentMovement = "D";
                }
            }
            else
            {
                //aktuelle Position des Schlangenkopfes löschen und warten
                Console.SetCursorPosition(cursorLeft, cursorTop);
                await Task.Delay(200);
                Console.Write(" ");

                if (currentMovement == "W")
                {
                    cursorTop = cursorTop - 1;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(snakeHead);
                }
                if (currentMovement == "A")
                {
                    cursorLeft = cursorLeft - 1;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(snakeHead);
                    currentMovement = "A";
                }
                if (currentMovement == "S")
                {
                    cursorTop = cursorTop + 1;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(snakeHead);
                    currentMovement = "S";
                }
                if (currentMovement == "D")
                {
                    cursorLeft = cursorLeft + 1;
                    Console.SetCursorPosition(cursorLeft, cursorTop);
                    Console.Write(snakeHead);
                    currentMovement = "D";
                }
            }
        }
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
