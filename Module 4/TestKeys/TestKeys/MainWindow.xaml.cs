using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Window = System.Windows.Window;
using Word = Microsoft.Office.Interop.Word;

namespace TestKeys
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Word.Application wordApp = null;
        private Document wordDoc = null; 

        public MainWindow()
        {
            InitializeComponent();
        }
        private async void GetDataBtn_Click(object sender, RoutedEventArgs e)
        {
            var httpclient = new HttpClient()
            {
                BaseAddress = new Uri("http://127.0.0.1:4444/TransferSimulator/")
            };

            var response = await httpclient.GetAsync("snils");
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<ResponseModel>(jsonResponse);
            EmailBox.Content = result?.value ?? "Значение отсутствует";
        }

        private void PostResultBtn_Click(object sender, RoutedEventArgs e)
        {
            if (EmailBox.Content == null)
            {
                MessageBox.Show("Сначала получите данные");
                return;
            }

            string snils = EmailBox.Content.ToString();

            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Word Documents (*.docx)|*.docx|All files (*.*)|*.*",
                    Title = "Выберите файл Word для записи результатов"
                };

                if (openFileDialog.ShowDialog() != true)
                {
                    MessageBox.Show("Файл не выбран. Операция отменена.");
                    return;
                }

                string pathDocument = openFileDialog.FileName;

                wordApp = new Word.Application();
                wordApp.Visible = true;
                wordDoc = wordApp.Documents.Open(pathDocument, ReadOnly: false, Visible: true);

                bool isFormatValid = IsSnilsFormatValid(snils);
                bool isChecksumValid = IsSnilsChecksumValid(snils);

                // Тест-кейс 1: формат
                string action1 = "Проверка формата СНИЛС (XXX-XXX-XXX YY)";
                string expectedResult1 = "СНИЛС соответствует формату";
                WriteResult(action1, expectedResult1, isFormatValid ? "Успешно" : "Не успешно");

                // Тест-кейс 2: контрольное число
                string action2 = "Проверка контрольного числа СНИЛС";
                string expectedResult2 = "Контрольное число корректно";
                WriteResult(action2, expectedResult2, isChecksumValid ? "Успешно" : "Не успешно");

                if (isFormatValid && isChecksumValid)
                {
                    ResponseBox.Content = "СНИЛС корректен";
                }
                else
                {
                    ResponseBox.Content = "Некорректный СНИЛС";
                }

                wordDoc.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при работе с документом: {ex.Message}");

                if (wordDoc != null)
                {
                    wordDoc.Close(false);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordDoc);
                    wordDoc = null;
                }
                if (wordApp != null)
                {
                    wordApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                    wordApp = null;
                }
            }
        }


        private bool IsSnilsFormatValid(string snils)
        {
            if (string.IsNullOrWhiteSpace(snils))
                return false;

            var regex = new Regex(@"^\d{3}-\d{3}-\d{3} \d{2}$");
            return regex.IsMatch(snils);
        }

        private bool IsSnilsChecksumValid(string snils)
        {
            if (!IsSnilsFormatValid(snils))
                return false;

            string digits = snils.Replace("-", "").Replace(" ", "");
            if (digits.Length != 11)
                return false;

            string numberPart = digits.Substring(0, 9);
            string checksumStr = digits.Substring(9, 2);

            if (!int.TryParse(numberPart, out _) || !int.TryParse(checksumStr, out int actualChecksum))
                return false;

            int sum = 0;
            for (int i = 0; i < 9; i++)
            {
                sum += (9 - i) * (numberPart[i] - '0');
            }

            int expectedChecksum;
            if (sum < 100)
                expectedChecksum = sum;
            else if (sum == 100 || sum == 101)
                expectedChecksum = 0;
            else
            {
                expectedChecksum = sum % 101;
                if (expectedChecksum == 100 || expectedChecksum == 101)
                    expectedChecksum = 0;
            }

            return actualChecksum == expectedChecksum;
        }


        private void WriteResult(string action, string expectedResult, string result)
        {
            if (wordDoc == null)
                throw new InvalidOperationException("Документ Word не открыт.");

            Word.Table table = wordDoc.Tables[1];

            int emptyRow = -1;
            for (int row = 1; row <= table.Rows.Count; row++)
            {
                bool isRowEmpty = true;
                for (int col = 1; col <= table.Columns.Count; col++)
                {
                    string cellText = table.Cell(row, col).Range.Text.Trim('\r', '\a', ' ');
                    if (!string.IsNullOrEmpty(cellText))
                    {
                        isRowEmpty = false;
                        break;
                    }
                }
                if (isRowEmpty)
                {
                    emptyRow = row;
                    break;
                }
            }

            if (emptyRow == -1)
            {
                table.Rows.Add();
                emptyRow = table.Rows.Count;
            }

            table.Cell(emptyRow, 1).Range.Text = action;
            table.Cell(emptyRow, 2).Range.Text = expectedResult;
            table.Cell(emptyRow, 3).Range.Text = result;
        }
    }
}