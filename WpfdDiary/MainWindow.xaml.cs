using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using DayTasks;

namespace WpfdDiary
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>


    

    public partial class MainWindow : Window
    {
        private static class CalendarInfo
        {
            static public  System.DateTime currentDate;
            static public TaskList taskList = new TaskList();
        }

        public MainWindow()
        {
            InitializeComponent();
            CalendarInfo.currentDate = calendar.SelectedDate ?? calendar.DisplayDate;
            CalendarInfo.taskList.LoadTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
            tasksGrid.ItemsSource = CalendarInfo.taskList.tasks;

            calendar.SelectedDatesChanged += DatesChanged;
        }

        private void DatesChanged(object sender, RoutedEventArgs e)
        {
            CalendarInfo.taskList.SaveTaskList(TaskList.DateToJsonFileName(CalendarInfo.currentDate));
            var date = (System.Windows.Controls.Calendar) sender;
            CalendarInfo.taskList.LoadTaskList(TaskList.DateToJsonFileName((System.DateTime) date.SelectedDate));
            tasksGrid.ItemsSource = CalendarInfo.taskList.tasks;
            CalendarInfo.currentDate = (System.DateTime) date.SelectedDate;
        }
    }

}
