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
        //ресурсы для текущей отображаемой информации
        private static class CalendarInfo
        {
            static public System.DateTime currentDate;
            static public TaskList taskList = new TaskList();
            static public Dictionary<TaskType, bool?> typesState = new Dictionary<TaskType, bool?>();
            static public bool isChanged = false;
            public static List<DayTask> SelectActiveTopics()
            {
                IEnumerable<DayTask> tasks = from dayTasks in taskList.tasks
                                             where typesState[dayTasks.Тип] == true
                                             select dayTasks;
                return tasks.ToList<DayTask>();
            }
        }

        //инициальзация объектов и назначение обработчиков событий
        public MainWindow()
        {
            InitializeComponent();
            CalendarInfo.currentDate = calendar.SelectedDate ?? calendar.DisplayDate;
            CalendarInfo.taskList.LoadTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
            tasksGrid.ItemsSource = CalendarInfo.taskList.tasks;

            //загрузка типов заданий
            taskTypesList.ItemsSource = Enum.GetValues(typeof(TaskType));
            taskTypesList.SelectedIndex = 0;

            calendar.SelectedDatesChanged += DatesChanged;

            foreach (ToggleButton button in ButtonGrid.Children)
            {
                button.Click += PressedTypeButton;
            }

            //Здесь вставтлять свои цвета
            TypeColors.colors[0] = (Color)ColorConverter.ConvertFromString("#CDABFF");
            TypeColors.colors[1] = (Color)ColorConverter.ConvertFromString("#DEF7FE");
            TypeColors.colors[2] = (Color)ColorConverter.ConvertFromString("#E7ECFF");
            TypeColors.colors[3] = (Color)ColorConverter.ConvertFromString("#C3FBD8");
            TypeColors.colors[4] = (Color)ColorConverter.ConvertFromString("#FDEED9");
            TypeColors.colors[5] = (Color)ColorConverter.ConvertFromString("#FFFADD");
            TypeColors.colors[6] = (Color)ColorConverter.ConvertFromString("#FFA8A3");

            var index = 1;
            foreach (ToggleButton button in ButtonGrid.Children)
            {
                SolidColorBrush myBrush = FindResource($"Br{index}") as SolidColorBrush;
                myBrush.Color = TypeColors.colors[index - 1];
                index++;
            }

            foreach (TaskType type in Enum.GetValues(typeof(TaskType)))
            {
                CalendarInfo.typesState[type] = true;
            }
        }

        //событие смены даты
        private void DatesChanged(object sender, RoutedEventArgs e)
        {
            CalendarInfo.taskList.SaveTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
            var date = (System.Windows.Controls.Calendar)sender;
            CalendarInfo.taskList.LoadTaskList(TaskList.DateToJsonFileName((System.DateTime)date.SelectedDate));
            tasksGrid.ItemsSource = CalendarInfo.SelectActiveTopics();
            CalendarInfo.currentDate = (System.DateTime)date.SelectedDate;
        }

        //сохранение при выходе
        public void AppExit(object sender, CancelEventArgs e)
        {
            CalendarInfo.taskList.SaveTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
        }

        //изменение отображыемых видов задач
        public void PressedTypeButton(Object sender, EventArgs e)
        {
            if (sender is ToggleButton button)
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

        //добавить задачу
        private void AddTask(object sender, RoutedEventArgs e)
        {

            var newTask = new DayTask
            {
                Тип = (TaskType)taskTypesList.SelectedItem,
                Имя = nameTextBox.Text,
                Информация = infoTextBox.Text,
                Выполнено = false,
            };

            CalendarInfo.taskList.tasks.Add(newTask);
            tasksGrid.ItemsSource = CalendarInfo.SelectActiveTopics() ?? new List<DayTask>();
        }

        //удалить выделенные задачи
        private void DeleteTask(object sender, RoutedEventArgs e)
        {
            var selectedTasks = tasksGrid.SelectedItems;
            foreach (var elem in selectedTasks)
            {
                CalendarInfo.taskList.tasks.RemoveAll(x => x == elem);
            }
            tasksGrid.ItemsSource = CalendarInfo.SelectActiveTopics() ?? new List<DayTask>();
        }

        //изменились ли значения
        private void TasksGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            CalendarInfo.isChanged = true;
        }
    }

    //раскрашивание строки в зависимости от типов
    public class TypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(TypeColors.colors[(int)value]);
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }

    static public class TypeColors
    {
        static public Color[] colors = new Color[7];
    }
}
