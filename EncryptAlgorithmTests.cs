﻿using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace Kuznecik_GOST
{
    public class EncryptAlgorithmTests
    {
        /// <summary>
        /// исходный текст
        /// </summary>
        private string SourceText => "1122334455667700FFEEDDCCBBAA9988";

        /// <summary>
        /// ожидаемый шифртекст
        /// </summary>
        private string EncryptResult => "7F679D90BEBC24305A468D42B9D4EDCD";

        /// <summary>
        /// Ключ шифрования
        /// </summary>
        private byte[] Key => GetBytes("8899AABBCCDDEEFF0011223344556677FEDCBA98765432100123456789ABCDEF");

        [Test]
        public void Should_correct_generate_round_key()
        {
            EncryptAlgorithm encryptAlgorithm = new EncryptAlgorithm();
            // запускаем генерацию раундовых ключей
            var keys = encryptAlgorithm.GenerateRoundKey(Key);

            // проверяем ожидаемый результат раундовых ключей
            CollectionAssert.AreEqual("8899AABBCCDDEEFF0011223344556677", GetString(keys[0]));
            CollectionAssert.AreEqual("FEDCBA98765432100123456789ABCDEF", GetString(keys[1]));
            CollectionAssert.AreEqual("DB31485315694343228D6AEF8CC78C44", GetString(keys[2]));
            CollectionAssert.AreEqual("3D4553D8E9CFEC6815EBADC40A9FFD04", GetString(keys[3]));
            CollectionAssert.AreEqual("57646468C44A5E28D3E59246F429F1AC", GetString(keys[4]));
            CollectionAssert.AreEqual("BD079435165C6432B532E82834DA581B", GetString(keys[5]));
            CollectionAssert.AreEqual("51E640757E8745DE705727265A0098B1", GetString(keys[6]));
            CollectionAssert.AreEqual("5A7925017B9FDD3ED72A91A22286F984", GetString(keys[7]));
            CollectionAssert.AreEqual("BB44E25378C73123A5F32F73CDB6E517", GetString(keys[8]));
            CollectionAssert.AreEqual("72E9DD7416BCF45B755DBAA88E4A4043", GetString(keys[9]));
        }

        [Test]
        public void Should_get_correct_encrypt_steps()
        {
            EncryptAlgorithm encryptAlgorithm = new EncryptAlgorithm();
            // запускаем генерацию раундовых ключей
            encryptAlgorithm.GenerateRoundKey(Key);
            encryptAlgorithm.Encrypt(GetBytes(SourceText));

            // проверяем шаги шифрования LSX
            CollectionAssert.AreEqual("E297B686E355B0A1CF4A2F9249140830", encryptAlgorithm.EncryptSteps[0]);
            CollectionAssert.AreEqual("285E497A0862D596B36F4258A1C69072", encryptAlgorithm.EncryptSteps[1]);
            CollectionAssert.AreEqual("0187A3A429B567841AD50D29207CC34E", encryptAlgorithm.EncryptSteps[2]);
            CollectionAssert.AreEqual("EC9BDBA057D4F4D77C5D70619DCAD206", encryptAlgorithm.EncryptSteps[3]);
            CollectionAssert.AreEqual("1357FD11DE9257290C2A1473EB6BCDE1", encryptAlgorithm.EncryptSteps[4]);
            CollectionAssert.AreEqual("28AE31E7D4C2354261027EF0B32897DF", encryptAlgorithm.EncryptSteps[5]);
            CollectionAssert.AreEqual("07E223D56002C013D3F5E6F714B86D2D", encryptAlgorithm.EncryptSteps[6]);
            CollectionAssert.AreEqual("CD8EF6CD97E0E092A8E4CCA61B38BF65", encryptAlgorithm.EncryptSteps[7]);
            CollectionAssert.AreEqual("0D8E40E4A800D06B2F1B37EA379EAD8E", encryptAlgorithm.EncryptSteps[8]);
        }

        [Test]
        public void Should_generate_correct_encrypt_text()
        {
            EncryptAlgorithm encryptAlgorithm = new EncryptAlgorithm();
            // запускаем генерацию раундовых ключей
            encryptAlgorithm.GenerateRoundKey(Key);

            // проверяем ожидаемый результат шифрования
            CollectionAssert.AreEqual(EncryptResult, encryptAlgorithm.Encrypt(GetBytes(SourceText)));
        }

        [Test]
        public void Should_get_correct_decrypt_steps()
        {
            EncryptAlgorithm encryptAlgorithm = new EncryptAlgorithm();
            // запускаем генерацию раундовых ключей
            encryptAlgorithm.GenerateRoundKey(Key);
            encryptAlgorithm.Decrypt(GetBytes(EncryptResult));

            // проверяем шаги шифрования
            CollectionAssert.AreEqual("76CA149EEF27D1B10D17E3D5D68E5A72", encryptAlgorithm.DecryptSteps[0]);
            CollectionAssert.AreEqual("5D9B06D41B9D1D2D04DF7755363E94A9", encryptAlgorithm.DecryptSteps[1]);
            CollectionAssert.AreEqual("79487192AA45709C115559D6E9280F6E", encryptAlgorithm.DecryptSteps[2]);
            CollectionAssert.AreEqual("AE506924C8CE331BB918FC5BDFB195FA", encryptAlgorithm.DecryptSteps[3]);
            CollectionAssert.AreEqual("BBFFBFC8939EAAFFAFB8E22769E323AA", encryptAlgorithm.DecryptSteps[4]);
            CollectionAssert.AreEqual("3CC2F07CC07A8BEC0F3EA0ED2AE33E4A", encryptAlgorithm.DecryptSteps[5]);
            CollectionAssert.AreEqual("F36F01291D0B96D591E228B72D011C36", encryptAlgorithm.DecryptSteps[6]);
            CollectionAssert.AreEqual("1C4B0C1E950182B1CE696AF5C0BFC5DF", encryptAlgorithm.DecryptSteps[7]);
            CollectionAssert.AreEqual("99BB99FF99BB99FFFFFFFFFFFFFFFFFF", encryptAlgorithm.DecryptSteps[8]);
        }

        [Test]
        public void Should_generate_correct_decrypt_text()
        {
            EncryptAlgorithm encryptAlgorithm = new EncryptAlgorithm();
            // запускаем генерацию раундовых ключей
            encryptAlgorithm.GenerateRoundKey(Key);

            // проверяем ожидаемый результат дешифрования
            CollectionAssert.AreEqual(SourceText, encryptAlgorithm.Decrypt(GetBytes(EncryptResult)));
        }

        private byte[] GetBytes(string text)
        {
            return Enumerable.Range(0, text.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(text.Substring(x, 2), 16))
                         .ToArray();
        }

        private string GetString(byte[] text)
        {
            return BitConverter.ToString(text).Replace("-", "");
        }
    }
}
