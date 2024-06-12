using Bookworms.Class;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows;

namespace Bookworms
{
    /// <summary>
    /// Логика взаимодействия для Client.xaml
    /// </summary>
    public partial class Client : Window
    {
        int UserId;
        public Client(int idUser)
        {
            InitializeComponent();
            UserId = idUser;
            Update();
        }

        private void ButtonBasket(object sender, RoutedEventArgs e)
        {
            Basket basket = new Basket(UserId);
            basket.Show();
        }

        private void ButtonOrder(object sender, RoutedEventArgs e)
        {
            Orders order = new Orders(UserId);
            order.Show();
        }

        private void ButtonBasketAdd(object sender, RoutedEventArgs e)
        {
            Product SelectedItem = (Product)BookTabel.SelectedItem;
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберете элемент");
            }
            else
            {
                using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
                {
                    connection.Open();
                    string sqlExpression = $"INSERT INTO Basket(idUser, idProduct) VALUES({UserId}, {SelectedItem.Id}) ";
                    SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();

                }
            }
        }

        private void ButtonMyOrder(object sender, RoutedEventArgs e)
        {
            Orders orders = new Orders(UserId);
            orders.Show();
        }

        private void Search_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Update(); 

            List<Product> products = new List<Product>();
            if (Search.Text == string.Empty || Search.Text == "")
            {
                Update();
            }
            else
            {
                foreach (Product item in BookTabel.Items)
                {
                    if (item.Name.Contains(Search.Text) || item.Price.ToString().Contains(Search.Text) || item.Description.Contains(Search.Text) || item.Category.Contains(Search.Text))
                    {
                        products.Add(item);
                    }
                }
                BookTabel.ItemsSource = null;
                BookTabel.ItemsSource = products;
            }
        }
        public void Update()
        {
            List<Product> product = new List<Product>();
            using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
            {
                connection.Open();
                string sqlExpression = "SELECT * FROM Product";
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
                            item.Description = reader.GetString(3);
                            item.idCategory = reader.GetInt32(4);
                            item.Avalilability = reader.GetString(5);
                            product.Add(item);

                        }
                    }
                }

            }
            using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
            {
                connection.Open();
                foreach (Product prod in product)
                {

                    string SQLExpression = $"SELECT Category.Category FROM Category WHERE Category.idCategory = {prod.idCategory}";
                    SQLiteCommand command = new SQLiteCommand(SQLExpression, connection);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows) // если есть данные
                        {
                            while (reader.Read())   // построчно считываем данные
                            {
                                prod.Category = reader.GetString(0);
                            }
                        }
                    }
                }
            }
            BookTabel.ItemsSource = null;
            BookTabel.ItemsSource = product;
    }
    }
}
