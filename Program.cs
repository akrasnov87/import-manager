using import_manager.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace import_manager
{
    /// <summary>
    /// Для запуска передать следущие аргуметы:
    ///  - имя файл в формате csv
    ///  - режим чтения данных: excel, word
    /// </summary>
    class Program
    {
        static string docName;
        /// <summary>
        /// Режим чтения файла:
        ///  - doc: информация из word
        ///  - excel: информация из основного excel документа
        /// </summary>
        static Mode mode;

        static void Main(string[] args)
        {
            docName = args[0];
            switch(args[1])
            {
                case "word":
                    mode = Mode.word;
                    break;

                case "excel":
                    mode = Mode.excel;
                    break;
            }
            string txt = File.ReadAllText(args[0]);
            var list = CsvReader(txt, mode);
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

        static List<Document> CsvReader(string txt, Mode mode)
        {
            Document parent = null;
            List<Document> items = new List<Document>();
            string[] lines = txt.Split("\r\n");
            for (int i = 0; i < lines.Length; i++)
            {
                if (i > (mode == Mode.word ? 1 : 0))
                {
                    string line = lines[i];
                    if (string.IsNullOrEmpty(line))
                        continue;

                    Document item = new Document();
                    if (mode == Mode.excel)
                    {
                        item.ConvertExcel(line);
                    }
                    else
                    {
                        item.ConvertWord(line);
                    }
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
