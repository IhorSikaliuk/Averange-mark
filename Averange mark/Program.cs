using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Averange_mark
{
    class Program
    {
        static string[] ReadFilesList ()
        {
            List<string> files = new List<string>();
            Console.WriteLine("Введіть назви файлів (без розширення):");
            while (true)
            {
                string input = Console.ReadLine();
                if (input == "")
                    break;
                files.Add(input);
            }
            return files.ToArray();
        }
        static (string, Dictionary<string, float>) ReadFile(StreamReader stream)
        {
            string subject = "";
            Dictionary<string, float> examList = new Dictionary<string, float>();
            try
            {
                subject = stream.ReadLine();
                string fileLine;
                while( (fileLine = stream.ReadLine()) != null)
                {
                    string[] str = fileLine.Split('-');
                    examList.Add(str[0], float.Parse(str[1]));
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                System.Environment.Exit(1);
            }
            return (subject, examList);
        }
        static void Main (string[] args)
        {
            Console.InputEncoding = System.Text.Encoding.Unicode;
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            string[] filesList = ReadFilesList();   //Введення користувачам переліку вхідних файлів
            const string fileExtention = ".txt";
            StreamReader[] filesReadStreams = new StreamReader[filesList.Count()];
            while (true)    //Перевірка наявності файлів, введених користувачем
            {
                try
                {
                    for (int i = 0; i < filesList.Count(); i++)
                        filesReadStreams[i] = new StreamReader($"{filesList[i]}{fileExtention}");
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    filesList = ReadFilesList();
                    filesReadStreams = new StreamReader[filesList.Count()];
                }
            }

            Dictionary<string, float>[] studentsLists = new Dictionary<string, float>[filesList.Count()];
            for (int i = 0; i < filesList.Count(); i++)    //Зчитування даних із файлів
            {
                string subject;
                (subject, studentsLists[i]) = ReadFile(filesReadStreams[i]);
                filesReadStreams[i].Close();
                Console.WriteLine($"\n{subject}");
                foreach (string s in studentsLists[i].Keys)
                    Console.WriteLine($"{s} - {studentsLists[i][s]}");
            }

            SortedDictionary<string, float> results = new SortedDictionary<string, float>();
            int numberOfLists = studentsLists.Count();
            for (int i = 0; i < numberOfLists; i++)     //Запис результатів (середніх арифметичних оцінок) у словник
            {
                foreach (string s in studentsLists[i].Keys)
                {
                    if (!results.ContainsKey(s))
                        results.Add(s, (studentsLists[i][s] / numberOfLists));
                    else
                        results[s] += studentsLists[i][s] / numberOfLists;
                }
            }

            Console.WriteLine("\nРезультати:");
            StreamWriter fileWriteStream = new StreamWriter($"Результати - {DateTime.Now.ToString("dd.MM.yyyy HH.mm")}.txt");
            foreach (string s in results.Keys)
            {
                fileWriteStream.WriteLine($"{s} - {results[s]}");
                Console.WriteLine($"{s} - {results[s]}");
            }
            fileWriteStream.Close();
            Console.ReadKey();
        }
    }
}
