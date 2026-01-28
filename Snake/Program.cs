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


        List<char> movementkeys = new List<char>()
        {
           'W'
        };

        string currentPressedKey = "";

        while (isGameOver == false)
        {
            if (Console.KeyAvailable) //Wenn Taste gedrückt
            {
                currentPressedKey = Console.ReadKey(true).Key.ToString();
            }

            if (IsPressedKeyMovementKey(currentPressedKey, movementkeys))
            {
                cursorLeft = cursorLeft + 1;
                Console.SetCursorPosition(cursorLeft, cursorTop);
                Console.Write(snakeHead);
            }
            await Task.Delay(200);      
           
        }
        Console.ReadKey();
    }

    private static bool IsPressedKeyMovementKey(string? pressedKey, List<char> movementkeys)
    {
        //TODO: implementieren
        if (pressedKey != "W")
            return false;
        else
            return true;
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
        var input = Console.ReadKey(true);
        Console.CursorVisible = false;
        TextWriter standardOutput = Console.Out; //Standard Output des Konsolentexts speicher - Text wird angezeigt
    }
}
