namespace Snake2;

internal class Program
{
    static Task Main(string[] args)
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

        Welcome_to_Snake();
        Console.WriteLine(cage);
        Console.SetCursorPosition(cursorLeft, cursorTop);
        Console.WriteLine(snakeHead);
        Console.WriteLine("\n\n\n\n\nW,A,S,D zum Steuern verwenden!");
        var input = Console.ReadKey(true);
        Console.CursorVisible = false;
        TextWriter standardOutput = Console.Out; //Standard Output des Konsolentexts speicher - Text wird angezeigt

        while (true)
        {

            //if (input.Key == ConsoleKey.D)
            //{
            //    while (cursorLeft < 14 && !(Console.KeyAvailable))
            //    {
            //        Console.SetOut(standardOutput);
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        await Task.Delay(100);
            //        Console.WriteLine(" ");
            //        cursorLeft = cursorLeft + 1;
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(snakeHead);
            //        await Task.Delay(100);
            //    }
            //    Console.SetOut(TextWriter.Null); //Text wird in Konsole nicht angezeigt
            //    Console.ReadKey(false); //Um den Bool Console.KeyAvailable zurückzusetzen
            //}
            //if (input.Key == ConsoleKey.A)
            //{
            //    while (cursorLeft > 1 && !(Console.KeyAvailable))
            //    {
            //        Console.SetOut(standardOutput);
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        await Task.Delay(100);
            //        Console.WriteLine(" ");
            //        cursorLeft = cursorLeft - 1;
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(snakeHead);
            //        await Task.Delay(100);
            //    }
            //    Console.SetOut(TextWriter.Null); //Text wird in Konsole nicht angezeigt
            //    Console.ReadKey(false); //Um den Bool Console.KeyAvailable zurückzusetzen

            //}

            ////!(Console.KeyAvailable) - solange true, BIS Taste gedrückt wird
            //while (!(Console.KeyAvailable && input.Key == ConsoleKey.D) && cursorLeft < 14)    //https://stackoverflow.com/a/24424239
            //{
            //    Console.SetOut(standardOutput);
            //    Console.SetCursorPosition(cursorLeft, cursorTop);
            //    await Task.Delay(100);
            //    Console.WriteLine(" ");
            //    cursorLeft = cursorLeft + 1;
            //    Console.SetCursorPosition(cursorLeft, cursorTop);
            //    Console.WriteLine(snakeHead);
            //    await Task.Delay(100);
            //}
            //Console.SetOut(TextWriter.Null); //Text wird in Konsole nicht angezeigt
            //Console.ReadKey(false); //Um den Bool Console.KeyAvailable zurückzusetzen


            //while (!(Console.KeyAvailable && input.Key == ConsoleKey.A) && cursorLeft > 1)
            //{
            //    Console.SetOut(standardOutput);
            //    Console.SetCursorPosition(cursorLeft, cursorTop);
            //    await Task.Delay(100);
            //    Console.WriteLine(" ");
            //    cursorLeft = cursorLeft - 1;
            //    Console.SetCursorPosition(cursorLeft, cursorTop);
            //    Console.WriteLine(snakeHead);
            //    await Task.Delay(100);
            //}
            //Console.SetOut(TextWriter.Null); //Text wird in Konsole nicht angezeigt
            //Console.ReadKey(false); //Um den Bool Console.KeyAvailable zurückzusetzen


            //if (input.Key == ConsoleKey.D)
            //{
            //    for (int i = cursorLeft; i < 14; i++)
            //    {
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(" ");
            //        await Task.Delay(100);
            //        cursorLeft = cursorLeft + 1;
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(snakeHead);
            //        await Task.Delay(100);
            //    }
            //}
            //else if (input.Key == ConsoleKey.A)
            //{
            //    for (int i = cursorLeft; i > 1; i--)
            //    {
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(" ");
            //        await Task.Delay(100);
            //        cursorLeft = cursorLeft - 1;
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(snakeHead);
            //        await Task.Delay(100);
            //    }
            //}
            //else if (input.Key == ConsoleKey.S)
            //{
            //    for (int i = cursorTop; i < 7; i++)
            //    {
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(" ");
            //        await Task.Delay(100);
            //        cursorTop = cursorTop + 1;
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(snakeHead);
            //        await Task.Delay(100);
            //    }
            //}
            //else if (input.Key == ConsoleKey.W)
            //{
            //    //Schlange läuft nach oben
            //    for (int i = cursorTop; i > 1; i--)
            //    {
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(" ");
            //        await Task.Delay(100);
            //        cursorTop = cursorTop - 1;
            //        Console.SetCursorPosition(cursorLeft, cursorTop);
            //        Console.WriteLine(snakeHead);
            //        await Task.Delay(100);
            //    }
            //}
            Console.ReadKey();
        }
    }


    private static void Welcome_to_Snake()
    {
        Console.WriteLine("=========================");
        Console.WriteLine("        SNAKE           ");
        Console.WriteLine("      ++++++++      \r\n    ++        ++    \r\n   ++          ++   \r\n   + ++      ++ +   \r\n   ++          ++   \r\n    ++         +    \r\n     + +    + ++    \r\n      ++++++++      \r\n         +++        \r\n        ++++        \r\n       ++  ++       ");
        Console.WriteLine("=========================");
        Console.WriteLine("\nPress any Key to Play!");
        Console.ReadKey();
        Console.Clear();
    }
}
