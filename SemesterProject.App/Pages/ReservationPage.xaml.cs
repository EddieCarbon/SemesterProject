﻿using SemesterProject.Database;
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
                    var isUserExist = IsUserExist(userid);
                    var isDateValid = IsDateValid(startDate, endDate);

                    var reservation = new Reservation()
                    {
                        UserID = int.Parse(userid),
                        RoomID = int.Parse(roomid),
                        StartDate = DateTime.ParseExact(startDate, "MM.dd.yyyy", CultureInfo.InvariantCulture),
                        EndDate = DateTime.ParseExact(endDate, "MM.dd.yyyy", CultureInfo.InvariantCulture),
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

                        MessageBox.Show("User created successfully.");
                    }
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

                    Reservation reservation = context.Reservations.Where(r
                        => r.ID == selectedRow.ID).FirstOrDefault();
                    

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
                        
                    }
                }   

            }
        }

        public void Delete()
        {
            using (HotelDbContext context = new ())
            {
                Reservation selectedId = ItemList.SelectedItem as Reservation;
                
                if (selectedId == null)
                {
                    MessageBox.Show("Please select a reservation.");
                    return;
                }

                context.Reservations.Remove(selectedId);
                context.SaveChanges();

                MessageBox.Show("Reservation deleted successfully.");
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
                if (room == null)
                {
                    return 0;
                }
                var price = room.Cost;
                var totalCost = price * totalDays;
                return totalCost;
            }
        }

        public bool IsUserExist(string userid)
        {
            using (HotelDbContext context = new ())
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
