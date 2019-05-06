using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DayTasks
{
    enum TaskType
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
    sealed internal class DayTask
    {
        [DataMember]
        public TaskType Тип { get; set; }

        [DataMember]
        public string Заголовок { get; set; }

        [DataMember]
        public string Информация { get; set; }

        [DataMember]
        public bool Выполнено { get; set; }

        public override string ToString()
        {
            return $"{Тип} - {Заголовок} : {Информация} ({Выполнено})";
        }       
    }

    sealed internal class TaskList
    {
        public List<DayTask> tasks { get; set; }

        //сохранить задачи текущего дня в json формат
        public void SaveTaskList(string fileName)
        {
            var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\WpfdDiaryTasks";
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }

            var jsonFormatter = new DataContractJsonSerializer(typeof(List<DayTask>));

            using (var fs = new FileStream(dir + @"\" + fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, tasks);
            }
        }

        //загрузить задачи для текущего дня из json файла
        public void LoadTaskList(string fileName)
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
                    tasks = (List<DayTask>)jsonFormatter.ReadObject(fs);
                }
                catch
                {
                    tasks = new List<DayTask>();
                }

            }
        }

        public static string DateToJsonFileName(System.DateTime dataTime) => $"{ dataTime.Year}_{ dataTime.Month}_{ dataTime.Day}.json";
    }
}
