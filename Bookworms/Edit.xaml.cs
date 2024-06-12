using Bookworms.Class;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bookworms
{
    /// <summary>
    /// Логика взаимодействия для Edit.xaml
    /// </summary>
    public partial class Edit : Window
    {
        Product item;
        public Edit(Product a)
        {
            InitializeComponent();
            Name.Text = a.Name;
            Price.Text = a.Price.ToString(); 
            Description.Text = a.Description;
            CatComboBox.Text = a.Category.ToString();
            AuailabilityComboBox.Text = a.Avalilability;
            item = a;
        }



        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSave(object sender, RoutedEventArgs e)
        {
            if (Name.Text == string.Empty || Price.Text == string.Empty || Description.Text == string.Empty || CatComboBox.Text == string.Empty || AuailabilityComboBox.Text == string.Empty)
            {
                MessageBox.Show("Не все поля заполнены");

            }
            else
            {
                using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
                {
                    connection.Open();
                    string sqlExpression = $"UPDATE Product SET Name = '{Name.Text}', Price = {int.Parse(Price.Text)}, Description = '{Description.Text}', idCategory = (SELECT idCategory FROM Category WHERE Category.Category = '{item.Category}'), Availability = '{item.Avalilability}' WHERE idProduct = {item.Id}";
                    SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Данные изменены");

                }
            }
        }
    }
}
