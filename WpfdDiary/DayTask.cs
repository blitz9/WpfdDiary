using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DayTasks
{
    internal enum TaskType
    {
        Идеи = 0,
        Работа = 1,
        Учёба = 2,
        Покупки = 3,
        Дни_Рождения = 4,
        Домашние_Дела = 5,
        Важные_Дела = 6,
    }

    [DataContract]
    internal sealed class DayTask
    {
        [DataMember]
        public TaskType Тип { get; set; }

        [DataMember]
        public string Заголовок { get; set; }

        [DataMember]
        public string Информация { get; set; }

        [DataMember]
        public bool Выполнено { get; set; }

        public override string ToString ()
        {
            var state = Выполнено ? "Выполнено" : "Не выполено";
            return $"{Тип} - {Заголовок} : {Информация} ({state})";
        }
    }

    internal sealed class TaskList
    {
        public List<DayTask> Tasks { get; set; }

        //сохранить задачи текущего дня в json формат
        public void SaveTaskList (string fileName)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WpfdDiaryTasks";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var jsonFormatter = new DataContractJsonSerializer(typeof(List<DayTask>));

            using (var fs = new FileStream(dir + @"\" + fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, Tasks);
            }
        }

        //загрузить задачи для текущего дня из json файла
        public void LoadTaskList (string fileName)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WpfdDiaryTasks";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var jsonFormatter = new DataContractJsonSerializer(typeof(List<DayTask>));

            using (var fs = new FileStream(dir + @"\" + fileName, FileMode.OpenOrCreate))
            {
                try
                {
                    Tasks = (List<DayTask>)jsonFormatter.ReadObject(fs);
                }
                catch
                {
                    Tasks = new List<DayTask>();
                }
            }
        }

        public static string DateToJsonFileName (System.DateTime dataTime) => $"{ dataTime.Year}_{ dataTime.Month}_{ dataTime.Day}.json";
    }
}
