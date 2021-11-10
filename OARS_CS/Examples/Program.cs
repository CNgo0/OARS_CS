using OARS_CS;
using System;
using System.IO;

namespace Examples
{
    public static class Program
    {
        public static void Main()
        {
            string key = File.ReadAllText("cngo.pem");

            byte[] rawFile = OARS.Download("cngo", key, "test.txt", "development");

            File.WriteAllBytes("test.txt", rawFile);

            if (File.Exists("test.txt"))
            {
                string[] fileContents = File.ReadAllLines("test.txt");
                foreach (string line in fileContents)
                {
                    Console.WriteLine(line);
                }
                File.Delete("test.txt");
            }
            else
            {
                Console.WriteLine("File was not downloaded!");
            }
        }
    }
}
