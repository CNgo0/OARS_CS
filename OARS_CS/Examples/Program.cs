using Oars;
using System;
using System.IO;
using System.Text;

namespace Examples
{
    public static class Program
    {
        public static void Main()
        {
            string key = File.ReadAllText("cngo.pem");

            // Set up OARS config object
            OarsConfiguration myOarsConfig = new OarsConfiguration("cngo", key);
            myOarsConfig.SetEnvironment(OarsApiEnv.Development, OarsDbEnv.Development);

            // Download a file from OARS
            OarsResult oarsResult = Oars.Oars.Download(myOarsConfig, "test.txt");

            // If the returned content type is application/json, then the transaction failed
            if (oarsResult.contentType != "text/html")
            {
                string message = Encoding.ASCII.GetString(oarsResult.data);

                Console.WriteLine("Failed to retrieve file!");
                Console.WriteLine(message);

                return;
            }

            // Success, write the contents to a file
            File.WriteAllBytes("test.txt", oarsResult.data);

            if (File.Exists("test.txt"))
            {
                // Read file and print contents to the screen
                string[] fileContents = File.ReadAllLines("test.txt");
                foreach (string line in fileContents)
                {
                    Console.WriteLine(line);
                }

                // Clean up
                File.Delete("test.txt");
            }
            else
            {
                Console.WriteLine("File was not downloaded!");
            }

            Console.WriteLine("Upload file...");
            byte[] toUpload = File.ReadAllBytes("uploadMe.txt");
            Console.WriteLine(toUpload.Length);
        }
    }
}
