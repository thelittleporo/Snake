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
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8; //für Unicode Zeichen
        const string snakeHead = ">"; //🐲
        bool isGameOver = false;


        //ShowWelcomeScreen();

        int snakePositionX = 0;
        int snakePositionY = 0;

        int[] Fieldvalues = DrawGamePanel(snakePositionX, snakePositionY, snakeHead); //DrawGamePanel gibt die Größe des Spielfeldes zurück

        int playFieldWidth = Fieldvalues[0];
        int playFieldHeight = Fieldvalues[1];
        snakePositionX = playFieldWidth / 2;
        snakePositionY = playFieldHeight / 2 + 1;
        string currentPressedKey = "";
        var currentMovement = "";
        int[] Fruitposition = GetRandomFruitPositions(playFieldWidth, playFieldHeight, snakePositionX, snakePositionY);
        int fruitPosX = Fruitposition[0];
        int fruitPosY = Fruitposition[1];
        bool isFruitField = false;
        List<int> ArchiveSnakePosition = new List<int>();
        int x = 0;
        int y = 1;
        bool SaveAllSnakeHeadPositions = false;

        //Spiel läuft
        while (isGameOver == false)
        {
            Console.SetCursorPosition(fruitPosX, fruitPosY);
            Console.WriteLine("*");

            if (Console.KeyAvailable) //Wenn Taste gedrückt wird
            {
                currentPressedKey = Console.ReadKey(true).Key.ToString(); //Speicher den Wert der Taste in eine Variable
            }

            if (IsPressedKeyMovementKey(currentPressedKey)) //Bewegungstaste?
            {
                //aktuelle Position des Schlangenkopfes löschen und warten               
                Console.SetCursorPosition(snakePositionX, snakePositionY);
                await Task.Delay(200);
                Console.Write(" ");

                if (currentPressedKey == "W" || currentPressedKey == "A" || currentPressedKey == "S" || currentPressedKey == "D")
                {
                    int previeousSnakePositionY = snakePositionY;
                    int previeousSnakePositionX = snakePositionX;

                    if (SaveAllSnakeHeadPositions)
                    {
                        ArchiveSnakePosition.Add(previeousSnakePositionX); ArchiveSnakePosition.Add(previeousSnakePositionY);

                    }
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

                    isFruitField = CheckFruitField(snakePositionY, snakePositionX, fruitPosX, fruitPosY);

                    if (isFruitField)
                    {
                        if (SaveAllSnakeHeadPositions == false)
                        {
                            ArchiveSnakePosition.Add(previeousSnakePositionX); ArchiveSnakePosition.Add(previeousSnakePositionY);
                        }
                        SaveAllSnakeHeadPositions = true; //Ab jetzt werden ALLE previousPositions gespeichert, um Schlangenkörper zu bewegen                                       

                        Console.SetCursorPosition(ArchiveSnakePosition[x], ArchiveSnakePosition[y]);
                        Console.WriteLine("+");
                        await Task.Delay(200);
                        Console.SetCursorPosition(ArchiveSnakePosition[x], ArchiveSnakePosition[y]);
                        Console.WriteLine(" ");
                        //Um beim nächsten Durchlauf auf nächstes Element in der Liste zuzugreifen 
                        x = x + 2;
                        y = y + 2;
                        Console.SetCursorPosition(fruitPosX, fruitPosY);
                        Console.WriteLine(" ");
                        //neue random Fruchtposition erstellen
                        Fruitposition = GetRandomFruitPositions(playFieldWidth, playFieldHeight, snakePositionX, snakePositionY);
                        fruitPosX = Fruitposition[0];
                        fruitPosY = Fruitposition[1];
                    }
                    ShowSnakeHead(snakePositionX, snakePositionY, snakeHead);
                    currentMovement = currentPressedKey;
                    isGameOver = CheckGameOver(snakePositionY, snakePositionX, playFieldWidth, playFieldHeight);
                }
            }
            else
            {
                if (currentMovement == "W" || currentMovement == "A" || currentMovement == "S" || currentMovement == "D")
                {
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
                    isFruitField = CheckFruitField(snakePositionY, snakePositionX, fruitPosX, fruitPosY);
                    if (isFruitField)
                    {
                        Console.SetCursorPosition(fruitPosX, fruitPosY);
                        Console.WriteLine(" ");
                        Fruitposition = GetRandomFruitPositions(playFieldWidth, playFieldHeight, snakePositionX, snakePositionY);
                        fruitPosX = Fruitposition[0];
                        fruitPosY = Fruitposition[1];
                    }
                    ShowSnakeHead(snakePositionX, snakePositionY, snakeHead);
                    //ShowSnakeBody();
                    Console.SetCursorPosition(snakePositionX, snakePositionY);
                    await Task.Delay(200);
                    Console.Write(" ");
                }
            }
        }
        ShowGameOverScreen();
    }

    //private static void ShowSnakeBody()
    //{
    //    throw new NotImplementedException();
    //}

    private static bool CheckFruitField(int snakePositionY, int snakePositionX, int fruitPosX, int fruitPosY)
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
        if (snakePositionY <= 0 || snakePositionX <= 0 || snakePositionY == (playFieldHeight + 1) || snakePositionX == playFieldWidth)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static void ShowSnakeHead(int snakePositionX, int snakePositionY, string snakeHead)
    {
        Console.SetCursorPosition(snakePositionX, snakePositionY);
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
    private static int[] DrawGamePanel(int snakePositionY, int snakePositionX, string snakeHead)
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

        //Cursor in die Mitte des Feldes setzen
        snakePositionX = (playFieldWidth / 2);
        snakePositionY = playFieldHeight / 2 + 1; //+1, da erste Zeile mitgezählt wird

        Console.SetCursorPosition(snakePositionX, snakePositionY);
        Console.WriteLine(snakeHead);
        Console.CursorVisible = false;

        int[] returnFieldvalues = { playFieldWidth, playFieldHeight };
        return returnFieldvalues;
    }
}
