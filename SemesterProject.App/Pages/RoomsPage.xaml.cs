using SemesterProject.Database;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        /// <summary>
        /// Creates a new room based on the input from the user interface.
        /// </summary>
        public void Create()
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                var number = NumberTextBox.Text.Trim();
                var capacity = CapacityTextBox.Text.Trim();
                var beds = BedsTextBox.Text.Trim();
                var cost = CostTextBox.Text.Trim();
                var hotelid = HotelIDTextBox.Text.Trim();
                var description = DescriptionTextBox.Text.Trim();

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

                // Check if HotelId exists
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
                Read();

                MessageBox.Show("Room created successfully.");
            }
        }

        /// <summary>
        /// Retrieves the list of rooms from the database and updates the user interface.
        /// </summary>
        public void Read()
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                RoomsList = context.Rooms.ToList();
                ItemList.ItemsSource = RoomsList;
            }
        }

        /// <summary>
        /// Updates the selected room with the modified information from the user interface.
        /// </summary>
        public void Update()
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                Room selectedRoom = ItemList.SelectedItem as Room;

                var number = NumberTextBox.Text.Trim();
                var capacity = CapacityTextBox.Text.Trim();
                var beds = BedsTextBox.Text.Trim();
                var cost = CostTextBox.Text.Trim();
                var hotelid = HotelIDTextBox.Text.Trim();
                var description = DescriptionTextBox.Text.Trim();

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
                    MessageBox.Show("Please select a room.");
                    return;
                }

                // Check if Hotel exists
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
                    Read();

                    MessageBox.Show("Room updated successfully.");
                }
            }
        }

        /// <summary>
        /// Deletes the selected room from the database and updates the user interface.
        /// </summary>
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
                    Read();

                    MessageBox.Show("Room deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Please select a room.");
                    return;
                }
            }
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
        }
    }
}

