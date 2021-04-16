using import_manager.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace import_manager
{
    public static class DocumentExtension
    {
        /// <summary>
        /// По документу 1-600Реестр учета многодетных семей.doc
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="line"></param>
        public static void ConvertWord(this Document doc, string line)
        {
            doc.id = Guid.NewGuid();
            string[] param = line.Split(";");
            doc.setNumber(param[0]);

            if(string.IsNullOrEmpty(NormalString(param[1])) 
                || NormalString(param[1]).StartsWith("(")
                || char.IsDigit(NormalString(param[1])[0]))
            {
                // значит это лишняя строка
                doc.n_number = null;
            }
            doc.setFio(param[1]);
            doc.setBirthDay(param[1]);

            doc.c_address = NormalString(param[2]);
            doc.setDate(param[3]);

            doc.c_intent = NormalString(param[4]);
            doc.c_account = NormalString(param[5]);
            doc.c_accept = NormalString(param[6]);
            doc.c_earth = NormalString(param[7]);

            doc.setTakeOffSolution(param[8]);
            doc.setTakeOffMessage(param[9]);

            doc.c_notice = NormalString(param[10]);
        }

        /// <summary>
        /// Реестр учета многодетных семей .xlsx
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="line"></param>
        public static void ConvertExcel(this Document doc, string line)
        {
            doc.id = Guid.NewGuid();
            string[] param = line.Split(";");
            doc.setNumber(param[0]);

            if (string.IsNullOrEmpty(NormalString(param[1]))
                || NormalString(param[1]).StartsWith("(")
                || char.IsDigit(NormalString(param[1])[0]))
            {
                // значит это лишняя строка
                doc.n_number = null;
            }
            doc.setFio(param[1]);
            doc.setBirthDay(param[2]);

            doc.setYear(param[3]);
            doc.c_document = NormalString(param[4]);
            doc.c_address = NormalString(param[5]);
            doc.setDate(param[6]);
            doc.c_time = NormalString(param[7]);

            doc.c_intent = NormalString(param[8]);
            doc.c_account = NormalString(param[9]);
            doc.c_accept = NormalString(param[10]);
            doc.c_earth = NormalString(param[11]);

            doc.setTakeOffSolution(param[12]);
            doc.setTakeOffMessage(param[13]);

            doc.c_notice = NormalString(param[14]);
        }

        public static string NormalString(string str)
        {
            return str.Trim();
        }

        /// <summary>
        /// Поиск даты в строке
        /// </summary>
        /// <param name="str">строка для поиска</param>
        /// <returns></returns>
        public static string GetDateString(string str)
        {
            str = NormalString(str);
            Regex regex = new Regex(@"\d{2}\.\d{2}\.\d{2,4}", RegexOptions.IgnoreCase);
            if(regex.IsMatch(str))
            {
                return regex.Match(str).Value;
            }
            return null;
        }

        /// <summary>
        /// чтение фио
        /// </summary>
        /// <param name="fio"></param>
        /// <returns></returns>
        public static string GetFioString(string fio)
        {
            char[] delimiterChars = { ',' };
            return fio.Split(delimiterChars)[0];
        }

        /// <summary>
        /// чтение даты рождения
        /// </summary>
        /// <param name="fio"></param>
        /// <returns></returns>
        public static string GetBirthDayString(string fio)
        {
            char[] delimiterChars = { ',' };
            string[] items = fio.Split(delimiterChars);
            return NormalString(items[items.Length - 1]);
        }

        /// <summary>
        /// Преобразование документа в человека
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static People Convert(this Document doc)
        {
            People people = new People();
            people.c_address = doc.c_address;
            people.c_document = doc.c_document;
            people.c_fio = doc.c_fio;
            people.d_birthday = doc.d_birthday;
            people.n_year = doc.n_year;

            return people;
        }

        public static void ToImport(this Document doc)
        {
            if (doc.n_year == 0 && doc.d_date.HasValue && doc.d_date.Value != DateTime.MinValue && doc.d_birthday != DateTime.MinValue)
            {
                doc.n_year = doc.d_date.Value.Year - doc.d_birthday.Year;
            }

            foreach (Document document in doc.documents)
            {
                doc.c_accept += "\n" + document.c_accept;
                doc.c_account += "\n" + document.c_account;
            }

            foreach(People people in doc.childrens)
            {
                if (doc.d_date.HasValue && doc.d_date.Value != DateTime.MinValue && people.d_birthday != DateTime.MinValue)
                {
                    people.n_year = doc.d_date.Value.Year - people.d_birthday.Year;
                }
            }

            if (doc.childrens.Count > 0)
            {
                doc.jb_child = Newtonsoft.Json.JsonConvert.SerializeObject(doc.childrens);
            } else
            {
                doc.jb_child = null;
            }
        }
    }
}
