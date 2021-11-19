using OARS;
using System;
using System.IO;
using System.Text;

namespace Examples
{
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("Download file...");

            string key = File.ReadAllText("cngo.pem");

            // Set up OARS config object
            OarsConfiguration myOarsConfig = new OarsConfiguration("cngo", key);
            myOarsConfig.SetEnvironment(OarsApiEnv.Development, OarsDbEnv.Development);

            // Download a file from OARS
            OarsResult downloadResult = Oars.Download(myOarsConfig, "test.txt");

            // If the returned content type is application/json, then the transaction failed
            if (downloadResult.contentType != "text/html")
            {
                string message = Encoding.ASCII.GetString(downloadResult.data);

                Console.WriteLine("Failed to retrieve file!");
                Console.WriteLine(message);

                return;
            }

            // Success, write the contents to a file
            File.WriteAllBytes("test.txt", downloadResult.data);

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

            // Upload a file
            Console.WriteLine("\nUpload file...");
            string filename = "oars_demo_records.json";
            string toUpload = File.ReadAllText(filename);
            OarsResult uploadResult = Oars.UploadJson(myOarsConfig, filename, Encoding.ASCII.GetBytes(toUpload));
            Console.WriteLine(Encoding.ASCII.GetString(uploadResult.data));
            Console.WriteLine(uploadResult.contentType);
        }
    }
}
