using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Merchant.Extensions
{
     public static class StringExtensions
    {
        /// <summary>
        /// CONVERT AN ARRAY OF BYTES INTO THE UTF8 STRING
        /// </summary>
        /// <param name="b">ARRAY OF BYTES COMING IN</param>
        /// <returns>STRING REPRESENTATION OF THE BYTES PASSED IN</returns>
        public static string ConvertToString(this byte[] b)
        {
            return Encoding.UTF8.GetString(b);
        }

        /// <summary>
        /// REVERSES A STRING
        /// </summary>
        /// <param name="s">INCOMING STRING TO REVERSE</param>
        /// <returns>REVERSED STRING</returns>
        public static string Reverse(this string s)
        {
            StringBuilder tmpReturn = new StringBuilder();

            //LOOP THROUGH EACH CHARACTER FROM BACK TO FRONT AND INSERT IT INTO THE BUILDER
            for (int i = s.Length - 1; i >= 0; i--)
                tmpReturn.Append(s[i]);

            //RETURN THE REVERSED STRING TO THE CALLING FUNCTION
            return tmpReturn.ToString();
        }

        /// <summary>
        /// CONVERTS A STRING INTO UTF8 ENCODED BYTE ARRAY
        /// </summary>
        /// <param name="s">INCOMING STRING TO CONVERT</param>
        /// <returns>BYTE ARRAY OF THE INCOMING STRING</returns>
        public static byte[] ToByteArray(this string s)
        {
            //CONVERT THE STRING TO A BYTE ARRAY
            return Encoding.UTF8.GetBytes(s);
        }

        /// <summary>
        /// ENCRYPTS A STRING USING AES ENCRYPTION AND A PASSWORD
        /// </summary>
        /// <param name="s">INCOMING STRING TO ENCRYPT</param>
        /// <param name="password">PASSWORD TO USE FOR ENCRYPTION</param>
        /// <returns>BASE64 ENCODED ENCRYPTED STRING</returns>
        public static string ToAESHash(this string s, string password = "")
        {
            //CREATE AN AES CRYPTOGRAPHY CLASS

            //CREATE A PASSWORD DERIVED BYTES CLASS TO CONVERT A SIMPLE PASSWORD INTO SOMETHING MORE COMPLEX
            using (PasswordDeriveBytes tmpPasswordBytes = new PasswordDeriveBytes(password.ToByteArray(),
                                                                            password.Reverse().ToByteArray()))
            {
                //CREATE A ICRYPTO TRANSFORM PROVIDER USING 256 BITS FOR THE KEY WHICH IS DERIVED FROM THE PASSWORD PASSED IN

                using (Aes tmpAes = Aes.Create())
                {
                    using (ICryptoTransform tmpCrypto = tmpAes.CreateEncryptor(tmpPasswordBytes.GetBytes(256 / 8),

                                                                            tmpPasswordBytes.GetBytes(16)))
                    {
                        //CREATE A TEMPORARY MEMORY STREAM TO HOLD THE DATA WHICH IS BEING ENCRYPTED
                        using (MemoryStream tmpMemoryStream = new MemoryStream())
                        {
                            //CREATE THE CRYPTO STREAM WHICH WILL WRITE TO THE UNDERLYING MEMORY STREAM
                            using (CryptoStream tmpStream =
                                new CryptoStream(tmpMemoryStream, tmpCrypto, CryptoStreamMode.Write))
                            {
                                tmpStream.Write(s.ToByteArray(), 0, s.Length);
                                tmpStream.Flush();
                            }

                            //CONVERT THE DATA TO A BASE64 STRING SO WE HAVE ALL VALID CHARACTERS IN THE STRING
                            return Convert.ToBase64String(tmpMemoryStream.ToArray());
                        }
                    }
                }
            }
        }

        public static string EncryptAES(this string s, string password = "")
        {
            return ToAESHash(s, password);
        }

        public static string DecryptAES(this string s, string password = "")
        {
            return FromAESHash(s, password);
        }

        /// <summary>
        /// DECRYPTS A BASE64 ENCODED STRING USING AES AND PASSWORD
        /// </summary>
        /// <param name="s">INCOMING BASE64 ENCODED STRING TO DECRYPT</param>
        /// <param name="password">PASSWORD USED DURING THE ENCRYPTION PROCESS TO REVERSE THE SEQUENCE</param>
        /// <returns>ORIGINAL STRING PRIOR TO ENCRYPTION</returns>
        public static string FromAESHash(this string s, string password = "")
        {
            if (s == null)
            {
                return "";
            }
            //CREATE AN AES CRYPTOGRAPHY CLASS
            using (Aes tmpAes = Aes.Create())
            {
                //CREATE A PASSWORD DERIVED BYTES CLASS TO CONVERT A SIMPLE PASSWORD INTO SOMETHING MORE COMPLEX
                using (PasswordDeriveBytes tmpPasswordBytes = new PasswordDeriveBytes(password.ToByteArray(),
                                                                               password.Reverse().ToByteArray()))
                {

                    //CREATE A ICRYPTO TRANSFORM PROVIDER USING 256 BITS FOR THE KEY WHICH IS DERIVED FROM THE PASSWORD PASSED IN
                    using (ICryptoTransform tmpCrypto =
                        tmpAes.CreateDecryptor(tmpPasswordBytes.GetBytes(256 / 8),
                        tmpPasswordBytes.GetBytes(16)))
                    {
                        //CREATE A MEMORY STREAM FROM THE BASE64 STRING WHICH IS BEING PASSED IN
                        using (MemoryStream tmpMemoryStream = new MemoryStream(Convert.FromBase64String(s)))
                        {
                            //CREATE A NEW STREAM WHICH WILL DECRYPT THE DATA AS WE READ IT
                            using (CryptoStream tmpStream = new CryptoStream(tmpMemoryStream, tmpCrypto, CryptoStreamMode.Read))
                            {
                                int readCount;
                                byte[] tmpBuffer = new byte[1024];
                                StringBuilder tmpReturn = new StringBuilder();

                                //CATCH THE ERROR INCASE THE PASSWORD IS INVALID
                                try
                                {
                                    //AS LONG AS THEIR IS DATA ON THE STREAM KEEP READING
                                    while ((readCount = tmpStream.Read(tmpBuffer, 0, tmpBuffer.Length)) != 0)
                                    {
                                        tmpReturn.Append(tmpBuffer.Take(readCount).ToArray().ConvertToString());
                                    }

                                    //RETURN THE DECRYPTED STRING VALUE
                                    return tmpReturn.ToString();
                                }
                                catch (CryptographicException)
                                {
                                    return "";
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether [is null or empty] [the specified STR].
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        
    }
}