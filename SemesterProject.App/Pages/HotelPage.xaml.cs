using SemesterProject.Database;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SemesterProject.App.Pages
{
    /// <summary>
    /// Interaction logic for HotelPage.xaml
    /// </summary>
    public partial class HotelPage : Page
    {
        public List<Hotel> HotelList { get; set; }

        public HotelPage()
        {
            InitializeComponent();
            Read();
        }

        /// <summary>
        /// Creates a new hotel based on the input from the user interface.
        /// </summary>
        public void Create()
        {
            using (HotelDbContext context = new())
            {
                var name = NameTextBox.Text;
                bool pool = IsPool.IsChecked ?? false;
                bool restaurant = IsRestaurant.IsChecked ?? false;
                bool bar = IsBar.IsChecked ?? false;
                bool gym = IsGym.IsChecked ?? false;
                bool spa = IsSpa.IsChecked ?? false;

                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }
                else
                {
                    var hotel = new Hotel()
                    {
                        Name = name,
                        IsPool = pool,
                        IsRestaurant = restaurant,
                        IsBar = bar,
                        IsGym = gym,
                        IsSpa = spa
                    };
                    context.Hotels.Add(hotel);
                    context.SaveChanges();
                    Read();

                    MessageBox.Show("Hotel created successfully.");
                }
            }
        }

        /// <summary>
        /// Retrieves the list of hotels from the database and updates the user interface.
        /// </summary>
        public void Read()
        {
            using (HotelDbContext context = new())
            {
                HotelList = context.Hotels.ToList();
                ItemList.ItemsSource = HotelList;
            }
        }

        /// <summary>
        /// Updates the selected hotel with the modified information from the user interface.
        /// </summary>
        public void Update()
        {
            using (HotelDbContext context = new())
            {
                var hotel = ItemList.SelectedItem as Hotel;
                var name = NameTextBox.Text;
                bool pool = IsPool.IsChecked ?? false;
                bool restaurant = IsRestaurant.IsChecked ?? false;
                bool bar = IsBar.IsChecked ?? false;
                bool gym = IsGym.IsChecked ?? false;
                bool spa = IsSpa.IsChecked ?? false;
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }
                else
                {
                    hotel.Name = name;
                    hotel.IsPool = pool;
                    hotel.IsRestaurant = restaurant;
                    hotel.IsBar = bar;
                    hotel.IsGym = gym;
                    hotel.IsSpa = spa;
                    context.Hotels.Update(hotel);
                    context.SaveChanges();
                    Read();

                    MessageBox.Show("Hotel updated successfully.");
                }
            }
        }

        /// <summary>
        /// Deletes the selected hotel from the database and updates the user interface.
        /// </summary>
        public void Delete()
        {
            using (HotelDbContext context = new())
            {
                var hotel = ItemList.SelectedItem as Hotel;

                if (hotel == null)
                {
                    MessageBox.Show("Please select a hotel.");
                    return;
                }
                else
                {
                    context.Hotels.Remove(hotel);
                    context.SaveChanges();
                    Read();

                    MessageBox.Show("Hotel deleted successfully.");
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
