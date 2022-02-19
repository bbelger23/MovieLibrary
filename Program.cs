using System;
using System.IO;
using NLog.Web;

namespace MovieLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "//nlog.config";
            
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();
            
            var file = "movies.csv";

            Console.WriteLine("Enter 1 to see movies");
            Console.WriteLine("Enter 2 to add movie");
            Console.WriteLine("Press any other key to exit");

            string option = Console.ReadLine();

            if (option == "1")
            {
                if (File.Exists(file))
                {
                    StreamReader sr = new StreamReader(file);
                    while (!sr.EndOfStream)
                    {
                        string row = sr.ReadLine();

                        string[] movie = row.Split(",");
                        
                        string movieID = movie[0];


                        string genre = movie[2].Replace("|", ",");

                        Console.WriteLine($"MovieID: {movieID}, Genre: {genre}");
                    }
                }
                else
                {
                    logger.Warn("File does not exist. {file}", file);
                }
            }
            else if (option == "2")
            {
                
            }
        }
    }
}
