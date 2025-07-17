using System.Windows;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace TradeClicker
{
    public partial class ColorPickerWindow : Window
    {
        // Private field to store the selected color
        private Color _selectedColor;

        // Property to get the selected color
        public Color SelectedColor
        {
            get { return _selectedColor; }
            private set { _selectedColor = value; }
        }

        // Constructor
        public ColorPickerWindow()
        {
            InitializeComponent();
        }

        // Event handler for OK button click
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (colorPicker.SelectedColor.HasValue)
            {
                SelectedColor = colorPicker.SelectedColor.Value;
                this.DialogResult = true; // Indicate that a color was selected
            }
            else
            {
                System.Windows.MessageBox.Show("Please select a color."); // Explicitly use System.Windows.MessageBox
            }
        }
    }
}
