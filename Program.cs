using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NLog.Web;

namespace MovieLibrary
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = Directory.GetCurrentDirectory() + "//nlog.config";
            
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();
            
            string file = "movies.csv";

            if (!File.Exists(file))
            {
                logger.Warn("File does not exist. {file}", file);
            }
            else
            {

                List<UInt64> MovieID = new List<UInt64>();
                List<string> MovieTitle = new List<string>();
                List<string> MovieGenre = new List<string>();

                StreamReader sr = new StreamReader(file);
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string row = sr.ReadLine();

                    int idx = row.IndexOf('"');
                    if (idx == -1)
                    {
                        string[] movie = row.Split(",");
                        
                        MovieID.Add(UInt64.Parse(movie[0]));

                        MovieTitle.Add(movie[1]);

                        MovieGenre.Add(movie[2].Replace("|", ", "));
                    }
                    else
                    {
                        MovieID.Add(UInt64.Parse(row.Substring(0, idx - 1)));
                           
                        row = row.Substring(idx + 1);
                            
                        idx = row.IndexOf('"');
                            
                        MovieTitle.Add(row.Substring(0, idx));
                            
                        row = row.Substring(idx + 2);
                            
                        MovieGenre.Add(row.Replace("|", ", "));
                    }
                }
                sr.Close();
            

                Console.WriteLine("Enter 1 to see movies");
                Console.WriteLine("Enter 2 to add movie");
                Console.WriteLine("Press any other key to exit");

                string option = Console.ReadLine();

                if (option == "1")
                {
                    for (int i = 0; i < MovieID.Count; i++)
                    {
                        Console.WriteLine($"MovieID: {MovieID[i]}");
                        Console.WriteLine($"Title: {MovieTitle[i]}");
                        Console.WriteLine($"Genre: {MovieGenre[i]}");
                        Console.WriteLine();
                    }
                }
                else if (option == "2")
                {
                
                }
            }
        }
    }
}
