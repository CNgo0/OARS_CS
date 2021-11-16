﻿using Oars;
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

            OarsResult oarsResult = Oars.Oars.Download("cngo", key, "tesdt.txt", "development");

            if(oarsResult.contentType != "text/html")
            {
                string message = Encoding.ASCII.GetString(oarsResult.data);

                Console.WriteLine("Failed to retrieve file!");
                Console.WriteLine(message);

                return;
            }

            File.WriteAllBytes("test.txt", oarsResult.data);

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