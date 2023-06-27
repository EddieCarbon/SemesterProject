using SemesterProject.Database;
using System;
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
            Read();
        }

        public void Create()
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                var number = NumberTextBox.Text;
                var capacity = CapacityTextBox.Text;
                var beds = BedsTextBox.Text;
                var cost = CostTextBox.Text;
                var hotelid = HotelIDTextBox.Text;
                var description = DescriptionTextBox.Text;

                if (string.IsNullOrEmpty(number) && string.IsNullOrEmpty(capacity) && 
                    string.IsNullOrEmpty(beds) && string.IsNullOrEmpty(cost) &&
                    string.IsNullOrEmpty(hotelid) && string.IsNullOrEmpty(description))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }

                foreach (var row in context.Rooms)
                {
                    if (row.RoomNumber == int.Parse(number))
                    {
                        MessageBox.Show("Room with this number already exists.");
                        return;
                    }
                }

                // Check Hotel exists
                var hotel = context.Hotels.FirstOrDefault(h => h.ID == int.Parse(hotelid));
                
                if (hotel == null) 
                {
                    MessageBox.Show("Hotel with this ID does not exist.");
                    return;
                }

                var room = new Room()
                {
                    RoomNumber = int.Parse(number),
                    Capacity = int.Parse(capacity),
                    NumberOfBeds = int.Parse(beds),
                    Cost = float.Parse(cost),
                    Description = description,
                    HotelID = int.Parse(hotelid)
                };
                
                context.Rooms.Add(room);
                context.SaveChanges();
                
                MessageBox.Show("Room created successfully.");
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
                var hotelid = HotelIDTextBox.Text;
                var description = DescriptionTextBox.Text;

                if (selectedRoom != null)
                {
                    if (selectedRoom.RoomNumber != int.Parse(number))
                    {
                        foreach (var room in context.Rooms)
                        {
                            if (room.RoomNumber == int.Parse(number))
                            {
                                MessageBox.Show("Room with this number already exists.");
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select room.");
                    return;
                }

                // Check Hotel exists
                var hotel = context.Hotels.FirstOrDefault(h => h.ID == int.Parse(hotelid));

                if (hotel == null)
                {
                    MessageBox.Show("Hotel with this ID does not exist.");
                    return;
                }

                if (selectedRoom != null)
                {
                    Room room = context.Rooms.Find(selectedRoom.ID);
                    room.RoomNumber = int.Parse(number);
                    room.Capacity = int.Parse(capacity);
                    room.NumberOfBeds = int.Parse(beds);
                    room.Cost = float.Parse(cost);
                    room.HotelID = int.Parse(hotelid);
                    room.Description = description;

                    context.SaveChanges();
                    MessageBox.Show("Room update successfully.");
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
                else
                {
                    MessageBox.Show("Please select room.");
                    return;
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
