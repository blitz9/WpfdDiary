using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DayTasks;

namespace WpfdDiary
{
    /// <summary>
    /// Логика взаимодействия для AddTaskWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow()
        {
            InitializeComponent();
            taskTypesList.ItemsSource = Enum.GetValues(typeof(TaskType));
            taskTypesList.SelectedIndex = 0;
            selectedData.SelectedDate = System.DateTime.Today;
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var newTask = new DayTask
            {
                Тип = (TaskType)taskTypesList.SelectedItem,
                Имя = nameTextBox.Text,
                Информация = infoTextBox.Text,
                Выполнено = false,
            };

            DateTime data = (DateTime) selectedData.SelectedDate;
            TaskList tasks = new TaskList();
            tasks.LoadTaskList(TaskList.DateToJsonFileName(data));
            tasks.tasks.Add(newTask);
            tasks.SaveTaskList(TaskList.DateToJsonFileName(data));
        }
    }
}
