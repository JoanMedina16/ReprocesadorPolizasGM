using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PD.Herramientas
{
    public class LOGI_EncryptaOTM_PD
    {
        public static string Encripta(string pwd)
        {
            pwd = pwd.ToUpper().Trim();
            string encrip = string.Empty;
            int ascii;
            int i;
            for (i = pwd.Length - 1; i >= 0; i--)
            {
                ascii = Asc(pwd.Substring(i, 1));
                if (i % 2 == 1)
                    ascii = ascii + 10;
                else
                    ascii = ascii - 3;
                encrip = encrip + Chr(ascii);
            }
            int letraadi = encrip.Length + 73;
            encrip = encrip.Substring(0, encrip.Length - 1) + Chr(letraadi) + encrip.Substring(encrip.Length - 1, 1);
            encrip = encrip.Replace(";", "!");
            encrip = encrip.Replace("a", "{");
            encrip = encrip.Replace("b", "|");
            encrip = encrip.Replace("c", "}");
            encrip = encrip.Replace("d", "~");
            return encrip.ToLower();
        }
        public static string DesEncripta(string strPassword)
        {
            string pwd = strPassword.Substring(0, strPassword.Length - 2);
            pwd = pwd + strPassword.Substring(strPassword.Length - 1, 1);
            pwd = pwd.Replace(";", "!");
            pwd = pwd.ToUpper().Trim();
            string encrip = string.Empty;
            int ascii;
            int i;

            if (pwd.Length % 2 == 0)
            {
                for (i = pwd.Length; i > 0; i--)
                {
                    ascii = Asc(pwd.Substring(i - 1, 1));
                    if (i % 2 == 1)
                        ascii = ascii - 10;
                    else
                        ascii = ascii + 3;

                    encrip = encrip + Chr(ascii);
                }
                //this.TextBox1.Text = encrip;
            }
            else
            {
                for (i = pwd.Length - 1; i >= 0; i--)
                {
                    ascii = Asc(pwd.Substring(i, 1));
                    if (i % 2 == 1)
                        ascii = ascii - 10;
                    else
                        ascii = ascii + 3;

                    encrip = encrip + Chr(ascii);
                }
                //this.TextBox1.Text = encrip;
            }

            return encrip;
        }
        private static int Asc(string s)
        {
            return Encoding.ASCII.GetBytes(s)[0];
        }
        private static char Chr(int c)
        {
            return Convert.ToChar(c);
        }
    }
}