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
    /// Логика взаимодействия для Admin.xaml
    /// </summary>
    public partial class Admin : Window
    {
        List<Product> product = new List<Product>();
        public Admin()
        {
            InitializeComponent();
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
            BookTabel.ItemsSource = product;
        }



        private void ButtonDel(object sender, RoutedEventArgs e)
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
                    string sqlExpression = $"DELETE FROM Product WHERE idProduct = {SelectedItem.Id}";
                    SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void ButtonAdd(object sender, RoutedEventArgs e)
        {
            Add add = new Add();
            add.Show();
        }

        private void ButtonEdit(object sender, RoutedEventArgs e)
        {
            Product SelectedItem = (Product)BookTabel.SelectedItem;
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберете элемент");
            }
            else
            {
                Edit edit = new Edit(SelectedItem);
                edit.ShowDialog();
            }
        }
    }
}
