using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace DayTasks
{
    [DataContract]
    sealed internal class DayTask
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool Сompleted { get; set; }
        [DataMember]
        public string Info { get; set; }
    }

    sealed internal class TaskList
    {
        public List<DayTask> tasks { get; set;}

        public void SaveTaskList(string fileName)
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(List<DayTask>));

            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                jsonFormatter.WriteObject(fs, tasks);
            }
        }

        public void LoadTaskList(string fileName)
        {
            var jsonFormatter = new DataContractJsonSerializer(typeof(List<DayTask>));

            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
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
