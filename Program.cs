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
            docName = args[0];
            string txt = File.ReadAllText(args[0]);
            var list = CsvReader(txt);
            using (ApplicationContext db = new ApplicationContext())
            {
                foreach (Document document in list)
                {
                    document.ToImport();
                    db.Documents.Add(document);
                }

                db.SaveChanges();
            }
            Console.WriteLine("Обработано: " + list.Count);
        }

        static List<Document> CsvReader(string txt)
        {
            Document parent = null;
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
                        if (string.IsNullOrEmpty(item.c_fio) || item.d_birthday == DateTime.MinValue)
                        {
                            parent.documents.Add(item);
                        }
                        else
                        {
                            parent.childrens.Add(item.Convert());
                        }
                    }
                    else
                    {
                        parent = item;
                        item.c_import_doc = docName;
                        items.Add(item);
                    }
                }
            }
            return items;
        }
    }
}
