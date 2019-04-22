using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using DayTasks;
using System.ComponentModel;
using System;
using System.Linq;
using System.Windows.Controls.Primitives;

namespace WpfdDiary
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
            public static IEnumerable<DayTask> SelectActiveTopics()
            {
                var tasks = from dayTasks in taskList.tasks
                            where typesState[dayTasks.Type]==true
                            select dayTasks;
                return tasks;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            CalendarInfo.currentDate = calendar.SelectedDate ?? calendar.DisplayDate;
            CalendarInfo.taskList.LoadTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
            tasksGrid.ItemsSource = CalendarInfo.taskList.tasks;
            calendar.SelectedDatesChanged += DatesChanged;

            //foreach (ToggleButton button in ButtonGrid.Children)
            //{
            //    button.Click+= PressedTypeButton;
            //}

            //foreach (TaskType type in Enum.GetValues(typeof(TaskType)))
            //{
            //    CalendarInfo.typesState[type] = true;
            //}
        }

        private void DatesChanged(object sender, RoutedEventArgs e)
        {
            CalendarInfo.taskList.SaveTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
            var date = (System.Windows.Controls.Calendar)sender;
            CalendarInfo.taskList.LoadTaskList(TaskList.DateToJsonFileName((System.DateTime)date.SelectedDate));
            tasksGrid.ItemsSource = CalendarInfo.taskList.tasks;//CalendarInfo.SelectActiveTopics();
            CalendarInfo.currentDate = (System.DateTime)date.SelectedDate;
        }

        public void AppExit(object sender, CancelEventArgs e)
        {
            CalendarInfo.taskList.SaveTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
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

               //tasksGrid.ItemsSource = CalendarInfo.SelectActiveTopics();
            }
        }
    }
}
