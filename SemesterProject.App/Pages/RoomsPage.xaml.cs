﻿using SemesterProject.Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SemesterProject.App.Pages
{
    /// <summary>
    /// Interaction logic for RoomsPage.xaml
    /// </summary>
    public partial class RoomsPage : Page
    {
        public List<Room> RoomsList { get; set; }

        public RoomsPage()
        {
            InitializeComponent();
        }

        public void Create()
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                var number = NumberTextBox.Text;
                var capacity = CapacityTextBox.Text;
                var beds = BedsTextBox.Text;
                var cost = CostTextBox.Text;
                var description = DescriptionTextBox.Text;

                if (string.IsNullOrWhiteSpace(number) || string.IsNullOrWhiteSpace(capacity) || string.IsNullOrWhiteSpace(beds) || string.IsNullOrWhiteSpace(cost) || string.IsNullOrWhiteSpace(description))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }
                else
                {
                    var room = new Room()
                    {
                        RoomNumber = int.Parse(number),
                        Capacity = int.Parse(capacity),
                        NumberOfBeds = int.Parse(beds),
                        Cost = int.Parse(cost),
                        Description = description
                    };
                    context.Rooms.Add(room);
                    context.SaveChanges();


                    MessageBox.Show("Room created successfully.");
                }
            }
        }

        public void Read()
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                RoomsList = context.Rooms.ToList();
                ItemList.ItemsSource = RoomsList;
            }
        }

        public void Update()
        {
            using (HotelDbContext context = new())
            {
                Room selectedRoom = ItemList.SelectedItem as Room;

                var number = NumberTextBox.Text;
                var capacity = CapacityTextBox.Text;
                var beds = BedsTextBox.Text;
                var cost = CostTextBox.Text;
                var description = DescriptionTextBox.Text;

                if (selectedRoom != null)
                {
                    Room room = context.Rooms.Find(selectedRoom.ID);
                    room.RoomNumber = int.Parse(number);
                    room.Capacity = int.Parse(capacity);
                    room.NumberOfBeds = int.Parse(beds);
                    room.Cost = int.Parse(cost);
                    room.Description = description;

                    context.SaveChanges();
                }
            }
        }

        public void Delete()
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                Room selectedRoom = ItemList.SelectedItem as Room;

                if (selectedRoom != null)
                {
                    Room room = context.Rooms.Single(x => x.ID == selectedRoom.ID);

                    context.Remove(room);
                    context.SaveChanges();
                }
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
            Update();
        }
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }
    }
}
