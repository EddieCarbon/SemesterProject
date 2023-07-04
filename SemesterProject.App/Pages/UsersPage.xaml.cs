using SemesterProject.Database;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SemesterProject.App
{
    /// <summary>
    /// Interaction logic for UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public List<User> UsersList { get; set; }

        public UsersPage()
        {
            InitializeComponent();
            Read();
        }

        public void Create()
        {
            using (HotelDbContext context = new HotelDbContext())
            {
                var name = NameTextBox.Text;
                var lastname = LastNameTextBox.Text;
                var email = EmailTextBox.Text;
                var phone = PhoneTextBox.Text;
                var street = StreetTextBox.Text;
                var apartmentNumber = ApartmentNumberTextBox.Text;
                var city = CityTextBox.Text;
                var postalCode = PostalCodeTextBox.Text;
                var country = CountryTextBox.Text;

                if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(lastname) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone))
                {
                    MessageBox.Show("Please fill all fields.");
                    return;
                }
                else
                {
                    var user = new User()
                    {
                        Name = name,
                        LastName = lastname,
                        Email = email,
                        PhoneNumber = phone,
                        Street = street,
                        ApartmentNumber = int.Parse(apartmentNumber),
                        City = city,
                        PostalCode = postalCode,
                        Country = country,
                    };

                    context.Users.Add(user);
                    context.SaveChanges();
                    Read();

                    MessageBox.Show("User created successfully.");
                }
            }
        }

        public void Read()
        {
            using (HotelDbContext context = new ())
            {
                UsersList = context.Users.ToList();
                ItemList.ItemsSource = UsersList;
            }
        }

        public void Update()
        {
            using (HotelDbContext context = new ())
            {
                User selectedUser = ItemList.SelectedItem as User;

                var name = NameTextBox.Text;
                var lastname = LastNameTextBox.Text;
                var email = EmailTextBox.Text;
                var phone = PhoneTextBox.Text;
                var street = StreetTextBox.Text;
                var apartmentNumber = ApartmentNumberTextBox.Text;
                var city = CityTextBox.Text;
                var postalCode = PostalCodeTextBox.Text;
                var country = CountryTextBox.Text;

                if (selectedUser != null)
                {
                    User user = context.Users.Find(selectedUser.ID);
                    
                    user.Name = name;
                    user.LastName = lastname;
                    user.Email = email;
                    user.PhoneNumber = phone;
                    user.Street = street;
                    user.ApartmentNumber = int.Parse(apartmentNumber);
                    user.City = city;
                    user.PostalCode = postalCode;
                    user.Country = country;

                    context.SaveChanges();
                    Read();
                    MessageBox.Show("User updated successfully.");
                }
                else
                {
                    MessageBox.Show("Please select row.");
                    return;
                }
            }
        }

        public void Delete()
        {
            using (HotelDbContext context = new ())
            {
                User selectedUser = ItemList.SelectedItem as User;

                if (selectedUser != null)
                {
                    User user = context.Users.Single(x => x.ID == selectedUser.ID);
                    
                    context.Remove(user);
                    context.SaveChanges();
                    Read();
                    MessageBox.Show("User deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Please select row.");
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
            Read();
        }
    }
}
