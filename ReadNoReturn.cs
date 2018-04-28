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
                    Console.Write(keyPress.KeyChar);                    
                    word += keyPress.KeyChar;
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
