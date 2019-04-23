using DayTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfDiary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static class CalendarInfo
        {
            static public System.DateTime currentDate;
            static public TaskList taskList = new TaskList();
            static public Dictionary<TaskType, bool?> typesState = new Dictionary<TaskType, bool?>();
            static public bool isChanged = false;
            public static List<DayTask> SelectActiveTopics()
            {
                IEnumerable<DayTask> tasks = from dayTasks in taskList.tasks
                                             where typesState[dayTasks.Type] == true
                                             select dayTasks;
                return tasks.ToList<DayTask>();
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            CalendarInfo.currentDate = calendar.SelectedDate ?? calendar.DisplayDate;
            CalendarInfo.taskList.LoadTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
            tasksGrid.ItemsSource = CalendarInfo.taskList.tasks;

            taskTypesList.ItemsSource = Enum.GetValues(typeof(TaskType));
            taskTypesList.SelectedIndex = 0;

            calendar.SelectedDatesChanged += DatesChanged;

            foreach (ToggleButton button in ButtonGrid.Children)
            {
                button.Click += PressedTypeButton;
            }

            foreach (TaskType type in Enum.GetValues(typeof(TaskType)))
            {
                CalendarInfo.typesState[type] = true;
            }
        }

        private void DatesChanged(object sender, RoutedEventArgs e)
        {
            CalendarInfo.taskList.SaveTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
            var date = (System.Windows.Controls.Calendar)sender;
            CalendarInfo.taskList.LoadTaskList(TaskList.DateToJsonFileName((System.DateTime)date.SelectedDate));
            tasksGrid.ItemsSource = CalendarInfo.SelectActiveTopics();
            CalendarInfo.currentDate = (System.DateTime)date.SelectedDate;
        }

        public void AppExit(object sender, CancelEventArgs e)
        {
            CalendarInfo.taskList.SaveTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
        }

        private void ColorRow(DataGrid dg)
        {
            for (int i = 0; i < dg.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(i);

                if (row != null)
                {
                    SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(100, 0, 0));
                    row.Background = brush;
                }
            }
        }

        public void PressedTypeButton(Object sender, EventArgs e)
        {
            if (sender is System.Windows.Controls.Primitives.ToggleButton button)
            {
                switch (button.Name)
                {
                    case "IdeasButton":
                        CalendarInfo.typesState[TaskType.Идеи] = !button.IsChecked;
                        break;
                    case "WorkButton":
                        CalendarInfo.typesState[TaskType.Работа] = !button.IsChecked;
                        break;
                    case "StudyButton":
                        CalendarInfo.typesState[TaskType.Учёба] = !button.IsChecked;
                        break;
                    case "PurchasesButton":
                        CalendarInfo.typesState[TaskType.Покупки] = !button.IsChecked;
                        break;
                    case "BirthdayButton":
                        CalendarInfo.typesState[TaskType.Дни_Рождения] = !button.IsChecked;
                        break;
                    case "HouseholdChoresButton":
                        CalendarInfo.typesState[TaskType.Домашние_Дела] = !button.IsChecked;
                        break;
                    case "ImportantMatterButton":
                        CalendarInfo.typesState[TaskType.Важные_Дела] = !button.IsChecked;
                        break;
                }

                tasksGrid.ItemsSource = CalendarInfo.SelectActiveTopics() ?? new List<DayTask>();
            }
        }

        private void AddTask(object sender, RoutedEventArgs e)
        {

            var newTask = new DayTask
            {
                Type = (TaskType)taskTypesList.SelectedItem,
                Name = nameTextBox.Text,
                Info = infoTextBox.Text,
                Сompleted = false,
            };

            CalendarInfo.taskList.tasks.Add(newTask);
            tasksGrid.ItemsSource = CalendarInfo.SelectActiveTopics() ?? new List<DayTask>();
        }

        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            var selectedTasks = tasksGrid.SelectedItems;
            foreach (var elem in selectedTasks)
            {
                CalendarInfo.taskList.tasks.RemoveAll(x => x == elem);
            }
            tasksGrid.ItemsSource = CalendarInfo.SelectActiveTopics() ?? new List<DayTask>();
        }


        private void TasksGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            CalendarInfo.isChanged = true;
        }
    }

    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((TaskType)value)
            {
                case TaskType.Идеи:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#8b00ff"));
                case TaskType.Работа:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#0000ff"));
                case TaskType.Учёба:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#42aaff"));
                case TaskType.Покупки:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#008000"));
                case TaskType.Дни_Рождения:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffff00"));
                case TaskType.Домашние_Дела:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ffa500"));
                case TaskType.Важные_Дела:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ff2400"));
                default:
                    return new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
