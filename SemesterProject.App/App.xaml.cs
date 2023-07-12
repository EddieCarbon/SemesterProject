using SemesterProject.Database;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SemesterProject.App
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

        public class DatabaseLocator
        {
            public static HotelDbContext Database { get; set; }
        }

    }
}
