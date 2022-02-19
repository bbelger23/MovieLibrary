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
                // read first line due to headers
                sr.ReadLine();
                while (!sr.EndOfStream)
                {
                    string row = sr.ReadLine();
                    // looking for quote in string
                    int idx = row.IndexOf('"');
                    if (idx == -1)
                    {
                        // movie data is separated by comma
                        string[] movie = row.Split(",");
                        
                        MovieID.Add(UInt64.Parse(movie[0]));

                        MovieTitle.Add(movie[1]);
                        // replacing "|" with ", "
                        MovieGenre.Add(movie[2].Replace("|", ", "));
                    }
                    else
                    {
                        MovieID.Add(UInt64.Parse(row.Substring(0, idx - 1)));
                        // removed ID and first quote from string   
                        row = row.Substring(idx + 1);
                        // finds the next quote    
                        idx = row.IndexOf('"');
                            
                        MovieTitle.Add(row.Substring(0, idx));
                        // removes title and last comma from string    
                        row = row.Substring(idx + 2);
                        // replacing "|" with ", "    
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
                    // loops through file
                    for (int i = 0; i < MovieID.Count; i++)
                    {
                        // displays all of the entries in file
                        Console.WriteLine($"MovieID: {MovieID[i]}");
                        Console.WriteLine($"Title: {MovieTitle[i]}");
                        Console.WriteLine($"Genre: {MovieGenre[i]}");
                        Console.WriteLine();
                    }
                }
                else if (option == "2")
                {
                    // ask user for movie title
                    Console.WriteLine("Enter the movie title");

                    string title = Console.ReadLine();

                    List<string> LowerCaseMovieTitle = MovieTitle.ConvertAll(t => t.ToLower());
                    if (LowerCaseMovieTitle.Contains(title.ToLower()))
                    {
                        logger.Info("Duplicate movie title {title} found", title);
                    }
                    else
                    {
                        UInt64 Id = MovieID.Max() + 1;
                        List<string> moviegenre = new List<string>();
                        string genre;
                        do{
                            Console.WriteLine("Enter a genre");

                            genre = Console.ReadLine();

                            if (genre != "" && genre.Length > 0)
                            {
                                moviegenre.Add(genre);
                            }
                        } while (genre != "");

                        if (moviegenre.Count == 0)
                        {
                            moviegenre.Add("(no genre)");
                        }

                        string genreString = string.Join("|", moviegenre);

                        title = title.IndexOf(',') != -1 ? $"\"{title}\"" : title; 

                        StreamWriter sw = new StreamWriter(file);
                        sw.WriteLine($"{Id},{title},{genreString}");
                        sw.Close();

                        MovieID.Add(Id);
                        MovieTitle.Add(title);
                        MovieGenre.Add(genreString);
                    }
                }
            }
        }
    }
}
