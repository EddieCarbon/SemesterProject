﻿using SemesterProject.Core;
using SemesterProject.Database;
using System.Windows;

namespace SemesterProject
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var database = new HotelDbContext();
            database.Database.EnsureCreated();

            DatabaseLocator.Database = database;
        }
    }
}
