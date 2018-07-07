using System;

namespace WYSIWYG
{
    public class ReadNoReturn
    {
        public static string OnEnter()
        {
            bool exit = true;
            string word = "";

            do
            {
                var keyPress = Console.ReadKey(true);
                if (keyPress.Key != ConsoleKey.Enter)
                {
                    if (keyPress.Key == ConsoleKey.Backspace)
                    {
                        Console.Write("\b \b");
                        word = word.Substring(0, word.Length - 1);
                    }
                    else
                    {
                        Console.Write(keyPress.KeyChar);
                        word += keyPress.KeyChar;
                    }
                }
                else
                {
                    exit = false;
                }

            } while (exit);
            return word;
        }
    }
}
