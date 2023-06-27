using SemesterProject.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SemesterProject.App.Pages
{
    /// <summary>
    /// Interaction logic for ReservationPage.xaml
    /// </summary>
    public partial class ReservationPage : Page
    {
        public List<Reservation> Reservations { get; set; }

        public ReservationPage()
        {
            InitializeComponent();
            Read();
        }


        public void Create()
        {
            using (HotelDbContext context = new ())
            {
                var userid = UserIDTextBox.Text;
                var roomid = RoomIDTextBox.Text;
                var startDate = StartDateTextBox.Text;
                var endDate = EndDateTextBox.Text;

                if (string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(roomid) || string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }
                else
                {
                    var totalDays = GetDays(startDate, endDate);
                    var price = GetTotalCost(roomid, totalDays);

                    var reservation = new Reservation()
                    {
                        UserID = int.Parse(userid),
                        RoomID = int.Parse(roomid),
                        StartDate = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Days = totalDays,
                        TotalCost = price
                    };

                    context.Reservations.Add(reservation);
                    context.SaveChanges();

                    MessageBox.Show("User created successfully.");
                }
            }
        }

        public void Read()
        {
            using (HotelDbContext context = new ())
            {
                Reservations = context.Reservations.ToList();
                ItemList.ItemsSource = Reservations;
            }
        }

        public void Update()
        {
            using (HotelDbContext context = new ())
            {
                // TODO: Update
            }
        }

        public void Delete()
        {
            using (HotelDbContext context = new ())
            {
                // TODO: Delete
            }
        }

        public int GetDays(string startDate, string endDate)
        {
            DateTime start = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            TimeSpan days = end - start;
            var totalDays = days.Days;
            return totalDays;
        }

        public float GetTotalCost(string roomid, int totalDays)
        {
            using (HotelDbContext context = new ())
            {
                var id = int.Parse(roomid);
                var room = context.Rooms.Where(r => r.ID == id).FirstOrDefault();
                var price = room.Cost;
                var totalCost = price * totalDays;
                return totalCost;
            }
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            Create();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            Read();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            //Update();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            //Delete();
        }
    }
}
