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
    /// Interaction logic for SwitchPasswordWindow.xaml
    /// </summary>
    public partial class SwitchPasswordWindow : Window
    {
        private readonly int _userId;
        public bool IsPasswordChanged { get; private set; } = false;

        public SwitchPasswordWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
        }

        private void ChangePasswordBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(CurrentPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(NewPasswordBox.Password) ||
                string.IsNullOrWhiteSpace(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Все поля должны быть заполнены!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var user = AppData.context.Users.Find(_userId);

            if (user == null || user.Password != CurrentPasswordBox.Password)
            {
                MessageBox.Show("Текущий пароль указан неверно!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewPasswordBox.Password == CurrentPasswordBox.Password)
            {
                MessageBox.Show("Новый пароль должен отличаться от текущего!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (NewPasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Новый пароль и подтверждение не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            user.Password = NewPasswordBox.Password;
            user.IsFirstLogin = false;
            AppData.context.SaveChanges();

            MessageBox.Show("Пароль успешно изменён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

            IsPasswordChanged = true;
            this.DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            IsPasswordChanged = false;
            this.DialogResult = false;
            this.Close();
        }
    }
}
