using SemesterProject.Database;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SemesterProject.App
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

        /// <summary>
        /// Creates a new reservation based on the input from the user interface.
        /// </summary>
        public void Create()
        {
            using (HotelDbContext context = new())
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
                    var isUserExist = IsUserExist(userid);
                    var isDateValid = IsDateValid(startDate, endDate);

                    var reservation = new Reservation()
                    {
                        UserID = int.Parse(userid),
                        RoomID = int.Parse(roomid),
                        StartDate = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                        Days = totalDays,
                        TotalCost = price
                    };

                    if (price == 0)
                    {
                        MessageBox.Show("Room not found.");
                        return;
                    }
                    else if (!isUserExist)
                    {
                        MessageBox.Show("User not found.");
                        return;
                    }
                    else if (!isDateValid)
                    {
                        MessageBox.Show("Invalid date.");
                        return;
                    }

                    if (price != 0 && isUserExist && isDateValid)
                    {
                        context.Reservations.Add(reservation);
                        context.SaveChanges();
                        Read();

                        MessageBox.Show("Reservation created successfully.");
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the list of reservations from the database and updates the user interface.
        /// </summary>
        public void Read()
        {
            using (HotelDbContext context = new())
            {
                Reservations = context.Reservations.ToList();
                ItemList.ItemsSource = Reservations;
            }
        }

        /// <summary>
        /// Updates the selected reservation with the modified information from the user interface.
        /// </summary>
        public void Update()
        {
            using (HotelDbContext context = new())
            {
                Reservation selectedRow = ItemList.SelectedItem as Reservation;

                if (selectedRow == null)
                {
                    MessageBox.Show("Please select a reservation.");
                    return;
                }

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
                    var isUserExist = IsUserExist(userid);
                    var isDateValid = IsDateValid(startDate, endDate);

                    Reservation reservation = context.Reservations.Find(selectedRow.ID);
                    reservation.UserID = int.Parse(userid);
                    reservation.RoomID = int.Parse(roomid);
                    reservation.StartDate = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    reservation.EndDate = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    reservation.Days = totalDays;
                    reservation.TotalCost = price;

                    if (price == 0)
                    {
                        MessageBox.Show("Room not found.");
                        return;
                    }
                    else if (!isUserExist)
                    {
                        MessageBox.Show("User not found.");
                        return;
                    }
                    else if (!isDateValid)
                    {
                        MessageBox.Show("Invalid date.");
                        return;
                    }
                    if (price != 0 && isUserExist && isDateValid)
                    {
                        context.SaveChanges();
                        Read();

                        MessageBox.Show("Reservation updated successfully.");
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the selected reservation from the database and updates the user interface.
        /// </summary>
        public void Delete()
        {
            using (HotelDbContext context = new())
            {
                Reservation selectedId = ItemList.SelectedItem as Reservation;

                if (selectedId == null)
                {
                    MessageBox.Show("Please select a reservation.");
                    return;
                }

                context.Reservations.Remove(selectedId);
                context.SaveChanges();
                Read();

                MessageBox.Show("Reservation deleted successfully.");
            }
        }

        /// <summary>
        /// Calculates the number of days between the start and end dates.
        /// </summary>
        public int GetDays(string startDate, string endDate)
        {
            DateTime start = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            TimeSpan days = end - start;
            var totalDays = days.Days;
            return totalDays;
        }

        /// <summary>
        /// Calculates the total cost of a room based on its ID and the number of days.
        /// </summary>
        public float GetTotalCost(string roomid, int totalDays)
        {
            using (HotelDbContext context = new())
            {
                var id = int.Parse(roomid);
                var room = context.Rooms.Where(r => r.ID == id).FirstOrDefault();
                if (room == null)
                {
                    return 0;
                }
                var price = room.Cost;
                var totalCost = price * totalDays;
                return totalCost;
            }
        }

        /// <summary>
        /// Validates that a given user ID exists in the database.
        /// </summary>
        public bool IsUserExist(string userid)
        {
            using (HotelDbContext context = new())
            {
                var id = int.Parse(userid);
                var user = context.Users.Where(u => u.ID == id).FirstOrDefault();
                if (user == null)
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Validates that the start date is earlier than or equal to the end date.
        /// </summary>
        public bool IsDateValid(string startDate, string endDate)
        {
            DateTime start = DateTime.ParseExact(startDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "dd.MM.yyyy", CultureInfo.InvariantCulture);
            if (start > end)
            {
                return false;
            }
            return true;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
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
            Update();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Delete();
            Read();
        }
    }
}

