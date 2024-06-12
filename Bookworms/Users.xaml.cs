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
    /// Логика взаимодействия для Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        List<Human> human = new List<Human>();
        int UserId;
        public Users()
        {
            InitializeComponent();
            using (var connection = new SQLiteConnection("Data Source=Bookworms.db"))
            {
                connection.Open();
                string sqlExpression = "SELECT * FROM Users";
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
                            if (reader.GetInt32(3) == 1)
                            {
                                Human item = new();
                                item.IdUser = reader.GetInt32(0);
                                item.Login = reader.GetString(1);
                                item.Fio = reader.GetString(4);
                                item.Contact = reader.GetString(5);
                                human.Add(item);
                            }
                        }
                    }
                }

            }
            UserTabel.ItemsSource = human;
        }

        private void ButtonBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
