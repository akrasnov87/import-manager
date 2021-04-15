using import_manager.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace import_manager
{
    class Program
    {
        static string docName;
        static void Main(string[] args)
        {
            string txt = File.ReadAllText(args[0]);
            var list = CsvReader(txt);
            Console.WriteLine("Hello World!");
        }

        static List<Document> CsvReader(string txt)
        {
            int? parent = -1;
            List<Document> items = new List<Document>();
            string[] lines = txt.Split("\r\n");
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > 1)
                {
                    string line = lines[i];
                    if (string.IsNullOrEmpty(line))
                        continue;

                    Document item = new Document();
                    item.Convert(line);
                    if (item.n_number == null)
                    {
                        item.f_parent = parent;
                    }
                    else
                    {
                        parent = item.id;
                    }
                    items.Add(item);
                }
            }
            return items;
        }
    }
}
