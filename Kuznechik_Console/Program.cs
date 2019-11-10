using System;
using System.IO;
using System.Text;

namespace Kuznechik_Console
{
    public class Program
    {
        private const string EXIT_MESSAGE = "Для завершения работы приложения нажмите любую клавишу\r\n";

        public static void Main(string[] args)
        {
            Console.WriteLine("Для работы приложения введите полные пути  для чтения файлов с \n\rмастер-ключами и исходным текстом, а также путь для сохранения результата работы алгоритма\n\r");
            Console.WriteLine("Данные в файлах должны быть указаны через запятую без пробелов, \n\rкол-во ключей должно совпадать с кол-вом блоков исходного текста\n\r");

            Console.WriteLine("Введите путь для чтения файла с мастер-ключами:");
            var masterKeyPath = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Введите путь для чтения файла с исходным кодом:");
            var sourceTextPath = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Введите путь для чтения сохранения файла с результатом работы алгоритма:");
            var resultPath = Console.ReadLine();
            Console.WriteLine();

            if (string.IsNullOrEmpty(sourceTextPath) ||
                string.IsNullOrEmpty(resultPath) ||
                string.IsNullOrEmpty(masterKeyPath))
            {
                Console.WriteLine("Какой-то из необходимых путей не был указан, попробуйте перезапустить приложение\r\n");
                Console.WriteLine(EXIT_MESSAGE);
                Console.ReadKey();
                return;
            }

            if (!string.IsNullOrEmpty(sourceTextPath) &&
                !string.IsNullOrEmpty(resultPath) &&
                !string.IsNullOrEmpty(masterKeyPath))
            {
                var masterKey = File.ReadAllText(masterKeyPath).Split(',');
                var text = File.ReadAllText(sourceTextPath).Split(',');

                if (masterKey.Length != text.Length)
                {
                    Console.WriteLine("Кол-во ключей должно совпадать c кол-вом блоков шифруемого текста.");
                    Console.WriteLine(EXIT_MESSAGE);
                    Console.ReadKey();
                    return;
                }

                var result = new StringBuilder();
                for (int i = 0; i < masterKey.Length; i++)
                {
                    EncryptAlgorithm encryptAlgorithm = new EncryptAlgorithm();

                    // запускаем генерацию раундовых ключей
                    encryptAlgorithm.GenerateRoundKey(masterKey[i].GetBytes());

                    var encryptText = encryptAlgorithm.Encrypt(text[i].GetBytes()).GetString();

                    var decrypText = encryptAlgorithm.Decrypt(encryptText.GetBytes()).GetString();

                    // проверяем ожидаемый результат дешифрования
                    if (!text[i].Equals(decrypText))
                    {
                        Console.WriteLine($"Исходный текст и результат дешифрования не совпали: Text: {text[i]}, Decrypt text: {decrypText}");
                    }

                    var resultString = $"MasterKey:{masterKey[i]}, Text:{text[i]}, EncryptText:{encryptText}, DecryptText:{decrypText}\n";

                    result.Append(resultString);
                }

                File.WriteAllText(resultPath, result.ToString());
            }

            Console.WriteLine("Работа алгоритма Кузнечик завершена");

            Console.ReadKey();
        }
    }
}
