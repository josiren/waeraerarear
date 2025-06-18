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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        public AdminPage()
        {
            InitializeComponent();
            LoadRoles();
            LoadUsers();
        }
        private void LoadUsers()
        {
            UsersDataGrid.ItemsSource = AppData.context.Users.ToList();  
        }

        private void LoadRoles()
        {
            var roles = AppData.context.Roles.ToList();

            foreach (var role in roles)
            {
                var comboBoxItem = new ComboBoxItem
                {
                    Content = role.Name,
                    Tag = role.Id,
                };
                NewUserRoleComboBox.Items.Add(comboBoxItem);
            }
        }

        private void AddUserBtn_Click(object sendpublicer, RoutedEventArgs e)
        {
            string login = NewUserLoginTextBox.Text;
            string password = NewUserPasswordBox.Password;
            string surname = NewUserSurnameTextBox.Text;
            string name = NewUserNameTextBox.Text;
            string patronymic = NewUserPatronymicTextBox.Text;
            int roleId = NewUserRoleComboBox.SelectedIndex + 1;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var existingUser = AppData.context.Users.FirstOrDefault(u => u.Username == login);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User newUser = new User
            {
                Username = login,
                Password = password,
                Surname = surname,
                Name = name,
                Patronymic = patronymic,
                RoleId = roleId,
                IsBanned = false,
                IsFirstLogin = true
            };

            AppData.context.Users.Add(newUser);
            AppData.context.SaveChanges();

            MessageBox.Show("Пользователь успешно добавлен.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            LoadUsers();
        }

        private void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                var user = button.DataContext as User;
                if (user != null)
                {
                    EditUserWindow editUserWindow = new EditUserWindow(user.Id);
                    editUserWindow.ShowDialog();
                    LoadUsers();
                }
            }
        }

        private void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is User userToDelete)
            {
                var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя '{userToDelete.Username}'?",
                                             "Подтверждение удаления", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        AppData.context.Users.Remove(userToDelete);
                        AppData.context.SaveChanges();
                        MessageBox.Show("Пользователь успешно удалён.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadUsers();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при удалении пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {

            AppData.MainFrame.Navigate(new AuthPage());
        }
    }
}
