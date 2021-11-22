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

            if(File.Exists("test.txt"))
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
            string filename1 = "uploadMe.txt";
            string toUpload = File.ReadAllText(filename1);
            OarsResult uploadResult1 = Oars.Upload(myOarsConfig, filename1, Encoding.ASCII.GetBytes(toUpload));
            Console.WriteLine(Encoding.ASCII.GetString(uploadResult1.data));
            Console.WriteLine(uploadResult1.contentType);

            // Insert records
            Console.WriteLine("\nInsert records...");
            string filename2 = "oars_demo_records.json";
            string toInsert = File.ReadAllText(filename2);
            OarsResult uploadResult2 = Oars.UploadJson(myOarsConfig, filename2, Encoding.ASCII.GetBytes(toInsert));
            Console.WriteLine(Encoding.ASCII.GetString(uploadResult2.data));
            Console.WriteLine(uploadResult2.contentType);
        }
    }
}
