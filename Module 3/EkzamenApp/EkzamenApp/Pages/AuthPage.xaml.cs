using System;
using System.Collections.Generic;
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
using EkzamenApp.Models;
using EkzamenApp.Windows;

namespace EkzamenApp.Pages
{
    /// <summary>
    /// Interaction logic for AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        private int attemptCounter = 1;
        public AuthPage()
        {
            InitializeComponent();
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string username = UsernameTextBox.Text;
                string password = PasswordBox.Password;

                var currentUser = AppData.context.Users.FirstOrDefault(u => u.Username == username);

                if (currentUser != null)
                {
                    var lastEnterLog = AppData.context.EnterLogs
                        .Where(el => el.UserId == currentUser.Id)
                        .OrderByDescending(el => el.Date)
                        .FirstOrDefault();

                    if (lastEnterLog != null && (DateTime.Now - lastEnterLog.Date).TotalDays > 30)
                    {
                        currentUser.IsBanned = true;
                        AppData.context.Users.Update(currentUser);
                        AppData.context.SaveChanges();

                        MessageBox.Show("Вы заблокированы из-за длительного отсутствия активности. Обратитесь к администратору.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (currentUser.IsBanned)
                    {
                        MessageBox.Show("Вы заблокированы. Обратитесь к администратору", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                var validUser = AppData.context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (validUser == null)
                {
                    int attemptsLeft = 3 - attemptCounter;
                    if (attemptCounter < 3)
                    {
                        MessageBox.Show($"Вы ввели неверный логин или пароль. Пожалуйста, проверьте ещё раз введённые данные.\nОсталось попыток: {attemptsLeft}",
                            "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        attemptCounter++;
                    }
                    else
                    {
                        MessageBox.Show("Вы заблокированы. Обратитесь к администратору", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Error);

                        if (currentUser != null)
                        {
                            currentUser.IsBanned = true;
                            AppData.context.Users.Update(currentUser);
                            AppData.context.SaveChanges();
                        }

                        UsernameTextBox.IsEnabled = false;
                        PasswordBox.IsEnabled = false;
                        LoginBtn.IsEnabled = false;
                    }
                    return;
                }

                var enterLog = new EnterLog
                {
                    UserId = validUser.Id,
                    Date = DateTime.Now
                };
                AppData.context.EnterLogs.Add(enterLog);
                AppData.context.SaveChanges();

                attemptCounter = 1;

                MessageBox.Show("Вы успешно авторизовались", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);

                if (validUser.IsFirstLogin == true)
                {
                    var passWindow = new SwitchPasswordWindow(validUser.Id);
                    bool? dialogResult = passWindow.ShowDialog();

                    if (dialogResult != true || !passWindow.IsPasswordChanged)
                    {
                        return;
                    }
                }

                NavigateByUserRole(validUser.RoleId);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка авторизации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        private void NavigateByUserRole(int roleId)
        {
            switch (roleId)
            {
                case 1:
                    AppData.MainFrame.Navigate(new UserPage());
                    break;
                case 2:
                    AppData.MainFrame.Navigate(new AdminPage());
                    break;
                case 3:
                    MessageBox.Show("Ошибка доступа. \nОбратитесь к администратору!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    break;
            }
        }
    }
}
