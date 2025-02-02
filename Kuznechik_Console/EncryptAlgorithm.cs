using System;
using System.Collections.Generic;

namespace Kuznechik_Console
{
    public class EncryptAlgorithm
    {
        #region Поля и св-ва класса

        /// <summary>
        /// Коэффициенты линейного преобразования
        /// </summary>
        private readonly byte[] _lVector = new byte[] {
            0x94, 0x20, 0x85, 0x10, 0xC2, 0xC0, 0x01, 0xFB,
            0x01, 0xC0, 0xC2, 0x10, 0x85, 0x20, 0x94, 0x01
        };

        /// <summary>
        /// Значения подстановки нелинейного преобразования
        /// в шестнадцатиричной СС
        /// </summary>
        private readonly byte[] _pi = {
            0xFC, 0xEE, 0xDD, 0x11, 0xCF, 0x6E, 0x31, 0x16,
            0xFB, 0xC4, 0xFA, 0xDA, 0x23, 0xC5, 0x04, 0x4D,
            0xE9, 0x77, 0xF0, 0xDB, 0x93, 0x2E, 0x99, 0xBA,
            0x17, 0x36, 0xF1, 0xBB, 0x14, 0xCD, 0x5F, 0xC1,
            0xF9, 0x18, 0x65, 0x5A, 0xE2, 0x5C, 0xEF, 0x21,
            0x81, 0x1C, 0x3C, 0x42, 0x8B, 0x01, 0x8E, 0x4F,
            0x05, 0x84, 0x02, 0xAE, 0xE3, 0x6A, 0x8F, 0xA0,
            0x06, 0x0B, 0xED, 0x98, 0x7F, 0xD4, 0xD3, 0x1F,
            0xEB, 0x34, 0x2C, 0x51, 0xEA, 0xC8, 0x48, 0xAB,
            0xF2, 0x2A, 0x68, 0xA2, 0xFD, 0x3A, 0xCE, 0xCC,
            0xB5, 0x70, 0x0E, 0x56, 0x08, 0x0C, 0x76, 0x12,
            0xBF, 0x72, 0x13, 0x47, 0x9C, 0xB7, 0x5D, 0x87,
            0x15, 0xA1, 0x96, 0x29, 0x10, 0x7B, 0x9A, 0xC7,
            0xF3, 0x91, 0x78, 0x6F, 0x9D, 0x9E, 0xB2, 0xB1,
            0x32, 0x75, 0x19, 0x3D, 0xFF, 0x35, 0x8A, 0x7E,
            0x6D, 0x54, 0xC6, 0x80, 0xC3, 0xBD, 0x0D, 0x57,
            0xDF, 0xF5, 0x24, 0xA9, 0x3E, 0xA8, 0x43, 0xC9,
            0xD7, 0x79, 0xD6, 0xF6, 0x7C, 0x22, 0xB9, 0x03,
            0xE0, 0x0F, 0xEC, 0xDE, 0x7A, 0x94, 0xB0, 0xBC,
            0xDC, 0xE8, 0x28, 0x50, 0x4E, 0x33, 0x0A, 0x4A,
            0xA7, 0x97, 0x60, 0x73, 0x1E, 0x00, 0x62, 0x44,
            0x1A, 0xB8, 0x38, 0x82, 0x64, 0x9F, 0x26, 0x41,
            0xAD, 0x45, 0x46, 0x92, 0x27, 0x5E, 0x55, 0x2F,
            0x8C, 0xA3, 0xA5, 0x7D, 0x69, 0xD5, 0x95, 0x3B,
            0x07, 0x58, 0xB3, 0x40, 0x86, 0xAC, 0x1D, 0xF7,
            0x30, 0x37, 0x6B, 0xE4, 0x88, 0xD9, 0xE7, 0x89,
            0xE1, 0x1B, 0x83, 0x49, 0x4C, 0x3F, 0xF8, 0xFE,
            0x8D, 0x53, 0xAA, 0x90, 0xCA, 0xD8, 0x85, 0x61,
            0x20, 0x71, 0x67, 0xA4, 0x2D, 0x2B, 0x09, 0x5B,
            0xCB, 0x9B, 0x25, 0xD0, 0xBE, 0xE5, 0x6C, 0x52,
            0x59, 0xA6, 0x74, 0xD2, 0xE6, 0xF4, 0xB4, 0xC0,
            0xD1, 0x66, 0xAF, 0xC2, 0x39, 0x4B, 0x63, 0xB6,
        };

        // таблица обратного нелинейного преобразования
        private readonly byte[] _reversePi = {
            0xA5, 0x2D, 0x32, 0x8F, 0x0E, 0x30, 0x38, 0xC0,
            0x54, 0xE6, 0x9E, 0x39, 0x55, 0x7E, 0x52, 0x91,
            0x64, 0x03, 0x57, 0x5A, 0x1C, 0x60, 0x07, 0x18,
            0x21, 0x72, 0xA8, 0xD1, 0x29, 0xC6, 0xA4, 0x3F,
            0xE0, 0x27, 0x8D, 0x0C, 0x82, 0xEA, 0xAE, 0xB4,
            0x9A, 0x63, 0x49, 0xE5, 0x42, 0xE4, 0x15, 0xB7,
            0xC8, 0x06, 0x70, 0x9D, 0x41, 0x75, 0x19, 0xC9,
            0xAA, 0xFC, 0x4D, 0xBF, 0x2A, 0x73, 0x84, 0xD5,
            0xC3, 0xAF, 0x2B, 0x86, 0xA7, 0xB1, 0xB2, 0x5B,
            0x46, 0xD3, 0x9F, 0xFD, 0xD4, 0x0F, 0x9C, 0x2F,
            0x9B, 0x43, 0xEF, 0xD9, 0x79, 0xB6, 0x53, 0x7F,
            0xC1, 0xF0, 0x23, 0xE7, 0x25, 0x5E, 0xB5, 0x1E,
            0xA2, 0xDF, 0xA6, 0xFE, 0xAC, 0x22, 0xF9, 0xE2,
            0x4A, 0xBC, 0x35, 0xCA, 0xEE, 0x78, 0x05, 0x6B,
            0x51, 0xE1, 0x59, 0xA3, 0xF2, 0x71, 0x56, 0x11,
            0x6A, 0x89, 0x94, 0x65, 0x8C, 0xBB, 0x77, 0x3C,
            0x7B, 0x28, 0xAB, 0xD2, 0x31, 0xDE, 0xC4, 0x5F,
            0xCC, 0xCF, 0x76, 0x2C, 0xB8, 0xD8, 0x2E, 0x36,
            0xDB, 0x69, 0xB3, 0x14, 0x95, 0xBE, 0x62, 0xA1,
            0x3B, 0x16, 0x66, 0xE9, 0x5C, 0x6C, 0x6D, 0xAD,
            0x37, 0x61, 0x4B, 0xB9, 0xE3, 0xBA, 0xF1, 0xA0,
            0x85, 0x83, 0xDA, 0x47, 0xC5, 0xB0, 0x33, 0xFA,
            0x96, 0x6F, 0x6E, 0xC2, 0xF6, 0x50, 0xFF, 0x5D,
            0xA9, 0x8E, 0x17, 0x1B, 0x97, 0x7D, 0xEC, 0x58,
            0xF7, 0x1F, 0xFB, 0x7C, 0x09, 0x0D, 0x7A, 0x67,
            0x45, 0x87, 0xDC, 0xE8, 0x4F, 0x1D, 0x4E, 0x04,
            0xEB, 0xF8, 0xF3, 0x3E, 0x3D, 0xBD, 0x8A, 0x88,
            0xDD, 0xCD, 0x0B, 0x13, 0x98, 0x02, 0x93, 0x80,
            0x90, 0xD0, 0x24, 0x34, 0xCB, 0xED, 0xF4, 0xCE,
            0x99, 0x10, 0x44, 0x40, 0x92, 0x3A, 0x01, 0x26,
            0x12, 0x1A, 0x48, 0x68, 0xF5, 0x81, 0x8B, 0xC7,
            0xD6, 0x20, 0x0A, 0x08, 0x00, 0x4C, 0xD7, 0x74
        };

        // размер блока
        private readonly int _size = 16;
        private readonly byte[][] _resultMultiplication = Multiplication();

        /// <summary>
        /// Сохраняем раундовые ключи
        /// </summary>
        private byte[][] _keys;

        public List<string> EncryptSteps { get; set; } = new List<string>();
        public List<string> DecryptSteps { get; set; } = new List<string>();
        public List<string> KeysList { get; set; } = new List<string>();

        #endregion

        /// <summary>
        /// Шифрование исходного текста
        /// </summary>
        /// <param name="data">Массив байт исходного текста</param>
        /// <returns></returns>
        public byte[] Encrypt(byte[] data)
        {
            byte[] block = new byte[_size];
            byte[] temp = new byte[_size];

            Array.Copy(data, block, _size);

            for (int i = 0; i < 9; i++)
            {
                LSX(ref temp, ref _keys[i], ref block);
                Array.Copy(temp, block, _size);
                // сохраняем шаги шифрования
                EncryptSteps.Add(GetString(block));
            }

            X(ref block, ref _keys[9]);

            return block;
        }

        /// <summary>
        /// Шифрование исходного текста
        /// </summary>
        /// <param name="data">Массив байт исходного текста</param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            byte[] block = new byte[_size];
            byte[] temp = new byte[_size];

            Array.Copy(data, block, _size);

            for (int i = 9; i > 0; i--)
            {
                Array.Copy(block, temp, _size);

                // последовательность операций XOR, обратное линейное преобразование,
                // обратное нелинейное преобразование
                X(ref temp, ref _keys[i]);
                ReverseL(ref temp);
                ReverseS(ref temp);
                Array.Copy(temp, block, _size);

                // сохраняем шаги дешифрования
                DecryptSteps.Add(GetString(block));
            }

            X(ref block, ref _keys[0]);

            return block;

            // Нелинейное преобразование S представляет собой применение к каждому 8-битному подвектору
            // 128-битного входного вектора фиксированной подстановки
            void ReverseS(ref byte[] a)
            {
                for (int i = 0; i < _size; i++)
                {
                    a[i] = _reversePi[a[i]];
                }
            }
        }

        /// <summary>
        /// Генерим раундовые ключи
        /// </summary>
        /// <param name="key">Массив байт мастер-ключа</param>
        public void GenerateRoundKey(byte[] key)
        {
            _keys = new byte[10][];
            for (int i = 0; i < 10; i++)
            {
                _keys[i] = new byte[_size];
            }

            byte[] x = new byte[_size];
            byte[] y = new byte[_size];
            byte[] c = new byte[_size];

            // разделяем ключ на 2 части и получаем 1-ю пару ключей
            for (int i = 0; i < _size; i++)
            {
                _keys[0][i] = x[i] = key[i];
                _keys[1][i] = y[i] = key[i + 16];
            }

            // генерим следующие раундовые ключи
            for (int k = 1; k < 5; k++)
            {
                for (int j = 1; j <= 8; j++)
                {
                    //  генерим константы
                    C(ref c, 8 * (k - 1) + j);
                    // последовательность операций XOR, нелинейное преобразование, линейное преобразование
                    F(ref c, ref x, ref y);
                }

                // записываем сгенеренные раундовые ключи
                Array.Copy(x, _keys[2 * k], _size);
                Array.Copy(y, _keys[2 * k + 1], _size);
            }

            for (int i = 0; i < 10; i++)
            {
                // сохраняем информацию по ключам в виде текста
                KeysList.Add(GetString(_keys[i]));
            }

            void C(ref byte[] param, int i)
            {
                Array.Clear(param, 0, _size);
                param[15] = (byte)i;
                L(ref param);
            }
        }

        /// <summary>
        /// Умножаем на неприводимый многочлен  x^8 + x^7 + x^6 + x + 1 в поле Галуа
        /// </summary>
        /// <returns></returns>
        private static byte[][] Multiplication()
        {
            byte[][] tmpTable = new byte[256][];
            for (int x = 0; x < 256; x++)
            {
                tmpTable[x] = new byte[256];
                for (int y = 0; y < 256; y++)
                {
                    tmpTable[x][y] = LinearTransformL((byte)x, (byte)y);
                }
            }
            return tmpTable;

            byte LinearTransformL(byte a, byte b)
            {
                byte p = 0;
                byte counter;
                byte hi_bit_set;
                for (counter = 0; counter < 8 && a != 0 && b != 0; counter++)
                {
                    if ((b & 1) != 0)
                        p ^= a;
                    hi_bit_set = (byte)(a & 0x80);
                    a <<= 1;
                    if (hi_bit_set != 0)
                        a ^= 0xc3;
                    b >>= 1;
                }
                return p;
            }
        }

        /// <summary>
        /// Преобразования ячейки Фейстеля
        /// </summary>
        /// <param name="k"></param>
        /// <param name="a1"></param>
        /// <param name="a0"></param>
        private void F(ref byte[] k, ref byte[] a1, ref byte[] a0)
        {
            byte[] temp = new byte[_size];

            LSX(ref temp, ref k, ref a1);
            X(ref temp, ref a0);

            Array.Copy(a1, a0, _size);
            Array.Copy(temp, a1, _size);
        }

        /// <summary>
        /// Сложение 2 двоичных векторов по модулю 2
        /// </summary>
        /// <param name="result">Результат XOR двух векторов (result, data)</param>
        /// <param name="data">Вектор, с которым ксорим первый вектор</param>
        private void X(ref byte[] result, ref byte[] data)
        {
            for (int i = 0; i < _size; i++)
            {
                result[i] ^= data[i];
            }
        }

        private void L(ref byte[] data)
        {
            for (int i = 0; i < _size; i++)
            {
                // Выполняем R-преобразование - сдвигаем данные для линейного преобразования L.
                // Сдвигаем на длину блока, равную 16
                byte x = data[15];
                for (int j = 14; j >= 0; j--)
                {
                    data[j + 1] = data[j];
                    x ^= _resultMultiplication[data[j]][_lVector[j]];
                }
                data[0] = x;
            }
        }

        private void LSX(ref byte[] result, ref byte[] k, ref byte[] a)
        {
            Array.Copy(k, result, _size);
            X(ref result, ref a);
            S(ref result);
            L(ref result);

            // Нелинейное преобразование S представляет собой применение к каждому 8-битному подвектору 128-битного входного вектора фиксированной подстановки
            void S(ref byte[] data)
            {
                for (int i = 0; i < _size; i++)
                {
                    data[i] = _pi[data[i]];
                }
            }
        }

        private void ReverseL(ref byte[] data)
        {
            for (int i = 0; i < _size ; i++)
            {
                // Выполняем обратное R-преобразование - сдвигаем данные для линейного преобразования L.
                // Сдвигаем на длину блока, равную 16
                byte x = data[0];
                var length = _size - 1;
                for (int j = 0; j < length; j++)
                {
                    data[j] = data[j + 1];
                    x ^= _resultMultiplication[data[j]][_lVector[j]];
                }
                data[15] = x;
            }
        }

        private string GetString(byte[] text)
        {
            return BitConverter.ToString(text).Replace("-", "");
        }
    }
}