using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ProAcc.BL
{
    public class DBHelper : IDisposable
    {
        Base @base = new Base();
        private void openConnection()
        {
            if (this.conn != null && this.conn.State == ConnectionState.Closed)
            {
                this.conn.Open();
            }
        }

        private void closeConnection()
        {
            if (this.conn != null && this.conn.State == ConnectionState.Open)
            {
                this.conn.Close();
            }
        }

        public DBHelper(string commandText)
            : this(commandText, CommandType.Text)
        {
        }

        
        //public DBHelper(string commandText, CommandType type, ConnectionStrings connectionkey)
        public DBHelper(string commandText, CommandType type)
        {
            //ConnectionStringInfoAttribute conninfo = connectionkey.GetAttributeOf<ConnectionStringInfoAttribute>();
            // string constr = ConfigurationManager.ConnectionStrings[conninfo.ConnKey].ConnectionString;
            //constr = PartialDecryption(constr, _salt);
            string constr = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;

            


            // string en = encrypt(constr);
           
            //var strEncryptred = Cipher.Encrypt(constr, _salt);
            var DBconnection = Cipher.Decrypt(constr, @base._salt);

            //constr = Decrypt(constr);
            //string constr = ConfigurationManager.ConnectionStrings["DBconnection"].ConnectionString;
            this.conn = new SqlConnection(DBconnection);
            this.comm = new SqlCommand(commandText, this.conn);
            this.comm.CommandType = type;
        }



        //#region Encryption

        //private int _iterations = 2;
        //private int _keySize = 256;

        //private string _hash = "aaaa";
        //private string _salt = "d5cc07aa70fd47";
        //private string _vector = "da5f44a3a67b4f9a";

        //private string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        //private string PartialDecryption(string argPartialEncString, string argPassSalt)
        //{
        //    Regex partialEncStringSearch = new Regex(@"\{DEC\[(?<enctext>[^]]+)\]\}");
        //    MatchCollection matches = partialEncStringSearch.Matches(argPartialEncString);
        //    for (int i = matches.Count - 1; i >= 0; i--)
        //    {
        //        Match match = matches[i];
        //        argPartialEncString = argPartialEncString.Remove(match.Index, match.Length);
        //        argPartialEncString = argPartialEncString.Insert(match.Index, Decrypt<AesManaged>(match.Groups["enctext"].Value, argPassSalt));
        //    }
        //    return argPartialEncString;
        //}



        //private string Encrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        //{
        //    byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
        //    byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
        //    byte[] valueBytes = Encoding.UTF8.GetBytes(value);

        //    byte[] encrypted;
        //    using (T cipher = new T())
        //    {
        //        PasswordDeriveBytes _passwordBytes =
        //            new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
        //        byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

        //        cipher.Mode = CipherMode.CBC;

        //        using (ICryptoTransform encryptor = cipher.CreateEncryptor(keyBytes, vectorBytes))
        //        {
        //            using (MemoryStream to = new MemoryStream())
        //            {
        //                using (CryptoStream writer = new CryptoStream(to, encryptor, CryptoStreamMode.Write))
        //                {
        //                    writer.Write(valueBytes, 0, valueBytes.Length);
        //                    writer.FlushFinalBlock();
        //                    encrypted = to.ToArray();
        //                }
        //            }
        //        }
        //        cipher.Clear();
        //    }
        //    return Convert.ToBase64String(encrypted);
        //}

        //private string Decrypt<T>(string value, string password) where T : SymmetricAlgorithm, new()
        //{
        //    byte[] vectorBytes = Encoding.ASCII.GetBytes(_vector);
        //    byte[] saltBytes = Encoding.ASCII.GetBytes(_salt);
        //    byte[] valueBytes = Convert.FromBase64String(value);

        //    byte[] decrypted;
        //    int decryptedByteCount = 0;

        //    using (T cipher = new T())
        //    {
        //        PasswordDeriveBytes _passwordBytes = new PasswordDeriveBytes(password, saltBytes, _hash, _iterations);
        //        byte[] keyBytes = _passwordBytes.GetBytes(_keySize / 8);

        //        cipher.Mode = CipherMode.CBC;

        //        try
        //        {
        //            using (ICryptoTransform decryptor = cipher.CreateDecryptor(keyBytes, vectorBytes))
        //            {
        //                using (MemoryStream from = new MemoryStream(valueBytes))
        //                {
        //                    using (CryptoStream reader = new CryptoStream(from, decryptor, CryptoStreamMode.Read))
        //                    {
        //                        decrypted = new byte[valueBytes.Length];
        //                        decryptedByteCount = reader.Read(decrypted, 0, decrypted.Length);
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            return String.Empty;
        //        }

        //        cipher.Clear();
        //    }
        //    return Encoding.UTF8.GetString(decrypted, 0, decryptedByteCount);
        //}
        //#endregion


        //#region NewEncryption
        //public string encrypt(string encryptString)
        //{
        //    //string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
        //    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        //});
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(clearBytes, 0, clearBytes.Length);
        //                cs.Close();
        //            }
        //            encryptString = Convert.ToBase64String(ms.ToArray());
        //        }
        //    }
        //    return encryptString;
        //}

        //public string Decrypt(string cipherText)
        //{
        //    //string EncryptionKey = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        //    cipherText = cipherText.Replace(" ", "+");
        //    byte[] cipherBytes = Convert.FromBase64String(cipherText);
        //    using (Aes encryptor = Aes.Create())
        //    {
        //        Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
        //    0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        //});
        //        encryptor.Key = pdb.GetBytes(32);
        //        encryptor.IV = pdb.GetBytes(16);
        //        using (MemoryStream ms = new MemoryStream())
        //        {
        //            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        //            {
        //                cs.Write(cipherBytes, 0, cipherBytes.Length);
        //                cs.Close();
        //            }
        //            cipherText = Encoding.Unicode.GetString(ms.ToArray());
        //        }
        //    }
        //    return cipherText;
        //}
        //#endregion
        public static T ConvertTo<T>(object argValue)
        {
            if (argValue == DBNull.Value)
                return default(T);
            try
            {
                return (T)argValue;
            }
            catch
            {
                return default(T);
            }
        }

        public DBHelper beginTransact()
        {
            if (!this.transactOpened)
            {
                this.openConnection();
                this.trans = this.conn.BeginTransaction();
                this.comm.Transaction = this.trans;
                this.transactOpened = true;
            }
            return this;
        }

        public void saveTransPoint(string pointName)
        {
            if (this.transactOpened && this.trans != null)
            {
                this.trans.Save(pointName);
            }
        }

        public void commit()
        {
            if (this.transactOpened)
            {
                if (this.trans != null)
                {
                    this.trans.Commit();
                }
                this.closeConnection();
                this.transactOpened = false;
            }
        }

        public void rollback()
        {
            if (this.transactOpened)
            {
                if (this.trans != null)
                {
                    this.trans.Rollback();
                }
                this.closeConnection();
                this.transactOpened = false;
            }
        }

        public void rollback(string pointName)
        {
            if (this.transactOpened)
            {
                if (this.trans != null)
                {
                    this.trans.Rollback(pointName);
                }
                this.transactOpened = false;
            }
        }

        public DBHelper addIn(string parameterName, object parameterValue)
        {
            SqlParameter value = new SqlParameter(parameterName, parameterValue);
            this.comm.Parameters.Add(value);
            return this;
        }

        public DBHelper addIn(string parameterName, object parameterValue, SqlDbType type)
        {
            SqlParameter sqlParameter = new SqlParameter(parameterName, parameterValue);
            sqlParameter.SqlDbType = type;
            this.comm.Parameters.Add(sqlParameter);
            return this;
        }

        public DBHelper addIn(string parameterName, object parameterValue, SqlDbType type, int size)
        {
            SqlParameter sqlParameter = new SqlParameter(parameterName, parameterValue);
            sqlParameter.SqlDbType = type;
            sqlParameter.Size = size;
            this.comm.Parameters.Add(sqlParameter);
            return this;
        }

        public DBHelper addIn(string parameterName, object parameterValue, SqlDbType type, byte precision, byte scale)
        {
            SqlParameter sqlParameter = new SqlParameter(parameterName, parameterValue);
            sqlParameter.SqlDbType = type;
            sqlParameter.Precision = precision;
            sqlParameter.Scale = scale;
            this.comm.Parameters.Add(sqlParameter);
            return this;
        }

        public DBHelper addOut(string parameterName, SqlDbType type, int size)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameterName;
            sqlParameter.Direction = ParameterDirection.Output;
            sqlParameter.SqlDbType = type;
            sqlParameter.Size = size;
            this.comm.Parameters.Add(sqlParameter);
            return this;
        }

        public DBHelper addOut(string parameterName, SqlDbType type, byte precision, byte scale)
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = parameterName;
            sqlParameter.Direction = ParameterDirection.Output;
            sqlParameter.SqlDbType = type;
            sqlParameter.Precision = precision;
            sqlParameter.Scale = scale;
            this.comm.Parameters.Add(sqlParameter);
            return this;
        }

        public DBHelper addReturn()
        {
            SqlParameter sqlParameter = new SqlParameter();
            sqlParameter.ParameterName = "@DBHELPER_RETURN_VALUE";
            sqlParameter.Direction = ParameterDirection.ReturnValue;
            this.comm.Parameters.Add(sqlParameter);
            return this;
        }

        public int getReturned()
        {
            return int.Parse(this.getValue("@DBHELPER_RETURN_VALUE").ToString());
        }

        public object getValue(string parameterName)
        {
            return this.comm.Parameters[parameterName].Value;
        }

        public DBHelper CreateNewCommand(string commandText)
        {
            return this.CreateNewCommand(commandText, CommandType.Text);
        }

        public DBHelper CreateNewCommand(string commandText, CommandType commandType)
        {
            this.comm.CommandText = commandText;
            this.comm.Parameters.Clear();
            this.comm.CommandType = commandType;
            return this;
        }

        public int Execute()
        {
            this.openConnection();
            int result = this.comm.ExecuteNonQuery();
            if (!this.transactOpened)
            {
                this.closeConnection();
            }
            return result;
        }

        public DataTable ExecuteDataTable()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(this.comm);
            DataTable dataTable = new DataTable();
            sqlDataAdapter.Fill(dataTable);
            return dataTable;
        }

        public DataSet ExecuteDataSet()
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(this.comm);
            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);
            return dataSet;
        }

        public DataRow ExecuteDataRow()
        {
            DataRow result = null;
            DataTable dataTable = this.ExecuteDataTable();
            if (dataTable.Rows.Count > 0)
            {
                result = dataTable.Rows[0];
            }
            return result;
        }

        public object ExecuteScalar()
        {
            this.openConnection();
            object result = this.comm.ExecuteScalar();
            if (!this.transactOpened)
            {
                this.closeConnection();
            }
            return result;
        }

        public bool HasRows()
        {
            DataTable dataTable = this.ExecuteDataTable();
            int count = dataTable.Rows.Count;
            dataTable.Dispose();
            return count > 0;
        }

        public void Dispose()
        {
            if (this.trans != null)
            {
                this.trans.Dispose();
            }
            if (this.comm != null)
            {
                this.comm.Dispose();
            }
            this.closeConnection();
        }

        private SqlConnection conn;

        private SqlCommand comm;

        private SqlTransaction trans;

        private bool transactOpened;
    }
}