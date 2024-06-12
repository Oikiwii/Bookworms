using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
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
    /// Логика взаимодействия для Add.xaml
    /// </summary>
    public partial class Add : Window
    {
        public Add()
        {
            InitializeComponent();
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonSave(object sender, RoutedEventArgs e)
        {
            if (Name.Text == string.Empty || Price.Text == string.Empty || Description.Text == string.Empty || CatComboBox.Text == String.Empty || AvailabilityComboBox.Text == string.Empty)
            {
                MessageBox.Show("Введите данные");
            }
            else
            {
                using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
                {
                    connection.Open();
                    string sqlExpression = $"INSERT INTO Product(Name, Price, Description, idCategory, Availability) VALUES('{Name.Text}', {Price.Text}, '{Description.Text}', (SELECT idCategory FROM Category WHERE Category.Category = '{CatComboBox.Text}'), '{AvailabilityComboBox.Text}') ";
                    SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
