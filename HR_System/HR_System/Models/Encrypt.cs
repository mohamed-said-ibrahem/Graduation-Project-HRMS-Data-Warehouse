using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace HR.Models
{
    public class Encrypt
    {
        public static string Encode(string password)
        {
            try
            {
                byte[] b = new byte[password.Length];
                b = System.Text.Encoding.UTF8.GetBytes(password);
                string encryptedData = Convert.ToBase64String(b);
                return encryptedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in Encode: " + ex.Message);
            }
        }
    }
}