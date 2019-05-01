using System;
using System.Windows;
using DayTasks;

namespace ShortTaskWindow
{
    /// <summary>
    /// Логика взаимодействия для AddTaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public TaskWindow()
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

            DateTime data = (DateTime)selectedData.SelectedDate;
            TaskList tasks = new TaskList();
            tasks.LoadTaskList(TaskList.DateToJsonFileName(data));
            tasks.tasks.Add(newTask);
            tasks.SaveTaskList(TaskList.DateToJsonFileName(data));
        }
    }
}
