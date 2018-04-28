using System;
using System.IO;


namespace WYSIWYG
{
    class FileOutput
    {
        public void Write()
        {
            // Create a string array with the lines of text
            string[] lines = { "First line", "Second line", "Third line" };

            // Set a variable to the My Documents path.
            string mydocpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(mydocpath + @"\WriteLines.txt"))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }
        }
        
    }
}
