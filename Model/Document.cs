using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace import_manager.Model
{
    [Table("dd_documents", Schema = "core")]
    public class Document
    {
        public Document()
        {
            childrens = new List<People>();
            documents = new List<Document>();
            c_document = "";
            c_time = "";
            f_user = 1;
        }

        /// <summary>
        /// Идентификатор
        /// </summary>
        [Key]
        public Guid id { get; set; }
        /// <summary>
        /// Номер
        /// </summary>
        public int? n_number { get; set; }
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
        /// <summary>
        /// Дата подачи заявления
        /// </summary>
        public DateTime? d_date { get; set; }
        /// <summary>
        /// Время подачи заявления
        /// </summary>
        public string c_time { get; set; }
        /// <summary>
        /// Цель использования земельного участка
        /// </summary>
        public string c_intent { get; set; }
        /// <summary>
        /// Постановление о постановке на учет
        /// </summary>
        public string c_account { get; set; }
        /// <summary>
        /// Дата и номер принятия решения
        /// </summary>
        public string c_accept { get; set; }
        /// <summary>
        /// Кадастровый номер земельного участка
        /// </summary>
        public string c_earth { get; set; }
        /// <summary>
        /// Решение о снятии с учета
        /// </summary>
        public DateTime? d_take_off_solution { get; set; }
        /// <summary>
        /// Сообщение заявителю о снятии с учета
        /// </summary>
        public DateTime? d_take_off_message { get; set; }
        /// <summary>
        /// Примечание
        /// </summary>
        public string c_notice { get; set; }
        /// <summary>
        /// Пользователь
        /// </summary>
        public int f_user { get; set; }
        /// <summary>
        /// Вложения
        /// </summary>
        [Column(TypeName = "jsonb")]
        public string jb_child { get; set; }
        /// <summary>
        /// признак удаления
        /// </summary>
        public bool sn_delete { get; set; }
        /// <summary>
        /// Документ из которого импортировались данные
        /// </summary>
        public string c_import_doc { get; set; }
        /// <summary>
        /// Замечания после импорта
        /// </summary>
        public string c_import_warning { get; set; }

        /// <summary>
        /// Для колонки jb_child
        /// </summary>
        [NotMapped]
        public List<People> childrens { get; set; }

        /// <summary>
        /// Используется для импорта из Word
        /// </summary>
        [NotMapped]
        public List<Document> documents { get; set; }

        public void setNumber(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                n_number = int.Parse(DocumentExtension.NormalString(value.Replace(".", "")));
            }
            else
            {
                n_number = null;
            }
        }

        public void setFio(string value)
        {
            string fioNormal = DocumentExtension.NormalString(value);
            c_fio = DocumentExtension.GetFioString(fioNormal);
        }

        public void setBirthDay(string value)
        {
            string fioNormal = DocumentExtension.NormalString(value);
            
            if (!string.IsNullOrEmpty(DocumentExtension.GetBirthDayString(fioNormal))
                && DocumentExtension.GetDateString(DocumentExtension.GetBirthDayString(fioNormal)) != null)
            {
                try
                {
                    d_birthday = DateTime.Parse(DocumentExtension.GetDateString(DocumentExtension.GetBirthDayString(fioNormal)));
                }
                catch (Exception e)
                {
                    c_import_warning = e.Message;
                }
            }
        }

        public void setDate(string value)
        {
            if (!string.IsNullOrEmpty(DocumentExtension.NormalString(value)))
            {
                d_date = DateTime.Parse(DocumentExtension.NormalString(value));
            }
        }

        public void setTakeOffSolution(string value)
        {
            if (!string.IsNullOrEmpty(DocumentExtension.NormalString(value)) && DocumentExtension.GetDateString(value) != null)
            {
                d_take_off_solution = DateTime.Parse(DocumentExtension.GetDateString(value));
            }
        }

        public void setTakeOffMessage(string value)
        {
            if (!string.IsNullOrEmpty(DocumentExtension.NormalString(value)) && DocumentExtension.GetDateString(value) != null)
            {
                d_take_off_message = DateTime.Parse(DocumentExtension.GetDateString(value));
            }
        }

        public void setYear(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                n_year = int.Parse(DocumentExtension.NormalString(value));
            }
        }
    }
}
