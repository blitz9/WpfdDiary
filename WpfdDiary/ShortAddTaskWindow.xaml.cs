using System;
using System.Windows;

using DayTasks;

namespace ShortTaskWindow
{
    /// <summary>
    /// Логика взаимодействия для AddTaskWindow.xaml
    /// </summary>
    public partial class ShortAddTaskWindow : Window
    {
        public ShortAddTaskWindow ()
        {
            InitializeComponent();
            taskTypesList.ItemsSource = Enum.GetValues(typeof(TaskType));
            taskTypesList.SelectedIndex = 0;
            selectedData.SelectedDate = DateTime.Today;
        }

        private void AddTaskButton_Click (object sender, RoutedEventArgs e)
        {
            var newTask = new DayTask
            {
                Тип = (TaskType)taskTypesList.SelectedItem,
                Заголовок = nameTextBox.Text,
                Информация = infoTextBox.Text,
                Выполнено = false,
            };

            var data = (DateTime)selectedData.SelectedDate;
            var tasks = new TaskList();
            tasks.LoadTaskList(TaskList.DateToJsonFileName(data));
            tasks.Tasks.Add(newTask);
            tasks.SaveTaskList(TaskList.DateToJsonFileName(data));
        }
    }
}
