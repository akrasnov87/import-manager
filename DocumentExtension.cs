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
        public static void Convert(this Document doc, string line)
        {
            doc.id = (int)(DateTime.Now.Ticks - DateTime.Parse("2021-04-15").Ticks);
            string[] param = line.Split(";");
            if(!string.IsNullOrEmpty(param[0]))
            {
                doc.n_number = int.Parse(NormalString(param[0].Replace(".", "")));
            } else
            {
                doc.n_number = null;
            }
            if(string.IsNullOrEmpty(NormalString(param[1])) 
                || NormalString(param[1]).StartsWith("(")
                || char.IsDigit(NormalString(param[1])[0]))
            {
                // значит это лишняя строка
                doc.n_number = -1;
                return;
            }
            string fioNormal = NormalString(param[1]);
            doc.c_fio = GetFioString(fioNormal);
            if (!string.IsNullOrEmpty(GetBirthDayString(fioNormal)) 
                && GetDateString(GetBirthDayString(fioNormal)) != null)
            {
                doc.d_birthday = DateTime.Parse(GetDateString(GetBirthDayString(fioNormal)));
            }

            doc.c_address = NormalString(param[2]);
            if (!string.IsNullOrEmpty(NormalString(param[3])))
            {
                doc.d_date = DateTime.Parse(NormalString(param[3]));
            }
            doc.c_intent = NormalString(param[4]);
            doc.c_account = NormalString(param[5]);
            doc.c_accept = NormalString(param[6]);
            doc.c_earth = NormalString(param[7]);

            if(!string.IsNullOrEmpty(NormalString(param[8])) && GetDateString(param[8]) != null)
            {
                doc.d_take_off_solution = DateTime.Parse(GetDateString(param[8]));
            }

            if (!string.IsNullOrEmpty(NormalString(param[9])) && GetDateString(param[9]) != null)
            {
                doc.d_take_off_message = DateTime.Parse(GetDateString(param[9]));
            }

            doc.c_notice = NormalString(param[10]);
        }

        static string NormalString(string str)
        {
            return str.Trim();
        }

        /// <summary>
        /// Поиск даты в строке
        /// </summary>
        /// <param name="str">строка для поиска</param>
        /// <returns></returns>
        static string GetDateString(string str)
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
        static string GetFioString(string fio)
        {
            char[] delimiterChars = { ',' };
            return fio.Split(delimiterChars)[0];
        }

        /// <summary>
        /// чтение даты рождения
        /// </summary>
        /// <param name="fio"></param>
        /// <returns></returns>
        static string GetBirthDayString(string fio)
        {
            char[] delimiterChars = { ',' };
            string[] items = fio.Split(delimiterChars);
            return NormalString(items[items.Length - 1]);
        }
    }
}
