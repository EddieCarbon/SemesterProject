using System.Text.RegularExpressions;
using System.Windows.Input;

namespace SemesterProject.App
{
    public class NumberTextBox
    { 
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
