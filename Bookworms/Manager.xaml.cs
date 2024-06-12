using Bookworms.Class;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Логика взаимодействия для Manager.xaml
    /// </summary>
    public partial class Manager : Window
    {
        List<History> history = new List<History>();
        public Manager()
        {
            InitializeComponent();
            using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
            {
                connection.Open();
                string sqlExpression = "SELECT Users.Fio, Product.Name, Status FROM HistoryOrders JOIN Users ON HistoryOrders.idUser = Users.idUser JOIN Product ON HistoryOrders.idProduct = Product.idProduct";
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {

                            History item = new();
                            item.NameUser = reader.GetString(0);
                            item.NameBook = reader.GetString(1);
                            item.Status = reader.GetString(2);
                            history.Add(item);

                        }
                    }
                }
            }
            BookTabel.ItemsSource = history;
        }

        private void UserClick(object sender, RoutedEventArgs e)
        {
            Users user = new Users();
            user.Show();
        }
    }
}
