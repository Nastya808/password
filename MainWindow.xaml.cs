using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace PasswordGeneratorApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenerateButton_Click(object sender, RoutedEventArgs e)
        {
            PasswordsListBox.Items.Clear(); // Очистим список перед новой генерацией

            if (int.TryParse(LengthTextBox.Text, out int length) && length > 0)
            {
                List<string> generatedPasswords = GeneratePasswords(length);

                if (generatedPasswords.Any())
                {
                    foreach (var password in generatedPasswords)
                    {
                        PasswordsListBox.Items.Add(password);
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось сгенерировать пароли. Пожалуйста, проверьте настройки.");
                }
            }
            else
            {
                MessageBox.Show("Введите корректную длину пароля.");
            }
        }

        private List<string> GeneratePasswords(int length)
        {
            List<string> passwords = new List<string>();
            int numberOfPasswords = 20;

            for (int i = 0; i < numberOfPasswords; i++)
            {
                string password = GeneratePassword(length);
                passwords.Add(password);
            }

            return passwords;
        }

        private string GeneratePassword(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=[]{}|;:'<>,.?/~";
            char[] password = new char[length];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];
                crypto.GetBytes(data);

                for (int i = 0; i < length; i++)
                {
                    password[i] = chars[data[i] % chars.Length];
                }
            }
            return new string(password);
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            if (PasswordsListBox.SelectedItem != null)
            {
                Clipboard.SetText(PasswordsListBox.SelectedItem.ToString());
                MessageBox.Show("Пароль скопирован в буфер обмена.", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Выберите пароль для копирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
