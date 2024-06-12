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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bookworms
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<string> GetUserRole(string userLogin, string userPassword)
        {
            SQLiteConnection connection = new SQLiteConnection("Data Source=Bookworms.db");
            List<string> result = new List<string>();
            //Изначально роль не определена, что будет означать, что вход не удался.
           

            connection.Open();

            //Запрос, который нам отдает название роли
            string sqlExpression = $"SELECT Users.idUser, Role.Role FROM Users JOIN Role ON Role.idRole = Users.idRole WHERE Users.Login = '{userLogin}' AND Users.Password = '{userPassword}'";
            SQLiteCommand command = new SQLiteCommand(sqlExpression, connection);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())   // построчно считываем данные
                    {

                        //Запись названия роли в переменную
                       
                        result.Add(reader.GetInt32(0).ToString());
                        result.Add(reader.GetString(1));
                    }
                }
                else
                {
                    result.Add("Роль не определена");
                }
            }

            connection.Close();

            //Возвращаем роль в то место, где вызываем функцию.
            return result;
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EnterClick(object sender, RoutedEventArgs e)
        {
           
              List <string> role = GetUserRole(LoginTextBox.Text, PasswordTextBox.Text);
               if (role[0] == "Роль не определена")
                {
                  MessageBox.Show("Ошибка входа. Введены неверные данные.");
                }
                else
                {
                    if (role[1] == "Покупатель")
                   {
                        MessageBox.Show($"Успешный вход. Ваша роль - {role[1]}");
                        Client client = new Client(int.Parse(role[0]));
                        client.Show();
                        this.Close();
                    }
                    else if (role[1] == "Администратор")
                    {
                      
                        MessageBox.Show($"Успешный вход. Ваша роль - {role[1]}");
                        Admin admin = new Admin();
                        admin.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Успешный вход. Ваша роль - {role[1]}");
                        Manager manager = new Manager();
                       manager.Show();
                        this.Close();
                    }
                }
            }
        }
    }

