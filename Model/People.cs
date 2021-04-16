using System;
using System.Collections.Generic;
using System.Text;

namespace import_manager.Model
{
    public class People
    {
        /// <summary>
        /// Фамилия, Имя, Отчество заявителя
        /// </summary>
        public string c_fio { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime d_birthday { get; set; }
        /// <summary>
        /// Возраст на момент постановки
        /// </summary>
        public int n_year { get; set; }
        /// <summary>
        /// Реквизиты документа, удостоверяющего личность
        /// </summary>
        public string c_document { get; set; }
        /// <summary>
        /// Адрес, телефон
        /// </summary>
        public string c_address { get; set; }
    }
}
