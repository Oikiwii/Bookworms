using Bookworms.Class;
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
    /// Логика взаимодействия для Basket.xaml
    /// </summary>
    public partial class Basket : Window
    {
        List<Product> list = new List<Product>();
        int UserId;
        public Basket(int idUser)
        {
            InitializeComponent();
            UserId = idUser;
            using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
            {
                connection.Open();

                string sqlExpression = $"SELECT Basket.idBasket, Product.Name, Product.Price FROM Basket JOIN Product ON Product.idProduct = Basket.idProduct WHERE Basket.idUser = {UserId}";
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);

                //Используем класс SqliteDataReader для считывания данных по запросу Select
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    //Если у нас есть хоть какие-то строки, то у нас будет все считываться
                    if (reader.HasRows)
                    {

                        //И пока у нас программа считывает строки, мы с ними взаимодействуем
                        while (reader.Read())
                        {

                            Product item = new();
                            item.Id = reader.GetInt32(0);
                            item.Name = reader.GetString(1);
                            item.Price = reader.GetInt32(2);
                            list.Add(item);
                        }
                    }
                }
            }
            TableBasket.ItemsSource = list;

        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonBuy(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Покупка прошла");
            using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
            {
                connection.Open();
             foreach (Product product in list) { 
                string sqlExpression = $"INSERT INTO HistoryOrders(idUser, idProduct, Status) VALUES ({UserId}, {product.Id}, 'Ожидает')";
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
              }
            }
            using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
            {
                connection.Open();
                string sqlExpression = $"DELETE FROM Basket WHERE idUser = {UserId}";
                SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                command.ExecuteNonQuery();
            }
        }
        private void ButtonDel(object sender, RoutedEventArgs e)
        {
            Product SelectedItem = (Product)TableBasket.SelectedItem;
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберете элемент");
            }
            else
            {
                using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
                {
                    connection.Open();
                    string sqlExpression = $"DELETE FROM Basket WHERE idBasket = {SelectedItem.Id}";
                    SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                    MessageBox.Show("Позиция удалена");
                }
            }
        }

      
    }
}
