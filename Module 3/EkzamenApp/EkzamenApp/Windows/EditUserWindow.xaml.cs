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
using System.Windows.Shapes;
using EkzamenApp.Models;

namespace EkzamenApp.Windows
{
    /// <summary>
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private int _userId;
        private User _currentUser;

        public EditUserWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadRoles();
            LoadUserData();
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
                UserRoleComboBox.Items.Add(comboBoxItem);
            }
        }

        private void LoadUserData()
        {
            _currentUser = AppData.context.Users.FirstOrDefault(u => u.Id == _userId);
            if (_currentUser != null)
            {
                UsernameTextBox.Text = _currentUser.Username;
                SurnameTextBox.Text = _currentUser.Surname;
                NameTextBox.Text = _currentUser.Name;
                PatronymicTextBox.Text = _currentUser.Patronymic ?? string.Empty;
                PasswordBox.Password = _currentUser.Password;

                foreach (ComboBoxItem item in UserRoleComboBox.Items)
                {
                    if (item.Tag != null && int.TryParse(item.Tag.ToString(), out int roleId) && roleId == _currentUser.RoleId)
                    {
                        UserRoleComboBox.SelectedItem = item;
                        break;
                    }
                }

                IsBannedCheckBox.IsChecked = _currentUser.IsBanned;
                IsFirstLoginCheckBox.IsChecked = _currentUser.IsFirstLogin;
            }
            else
            {
                MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (_currentUser == null) return;

            if (string.IsNullOrWhiteSpace(SurnameTextBox.Text) || string.IsNullOrWhiteSpace(NameTextBox.Text) || UserRoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Фамилия, имя и роль обязательны для заполнения.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _currentUser.Surname = SurnameTextBox.Text;
            _currentUser.Name = NameTextBox.Text;
            _currentUser.Patronymic = string.IsNullOrWhiteSpace(PatronymicTextBox.Text) ? null : PatronymicTextBox.Text;
            _currentUser.Password = PasswordBox.Password;

            if (UserRoleComboBox.SelectedItem is ComboBoxItem selectedRole && int.TryParse(selectedRole.Tag?.ToString(), out int roleId))
            {
                _currentUser.RoleId = roleId;
            }

            _currentUser.IsBanned = IsBannedCheckBox.IsChecked == true;
            _currentUser.IsFirstLogin = IsFirstLoginCheckBox.IsChecked == true;

            try
            {
                AppData.context.Users.Update(_currentUser);
                AppData.context.SaveChanges();
                MessageBox.Show("Данные пользователя успешно сохранены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
