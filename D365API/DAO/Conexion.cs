using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365API
{
   public class Conexion
    {
        #region "VARIABLES"
        private String cnx;
        private SqlCommand oCommandSql = new SqlCommand();
        private SqlConnection oConnection = new SqlConnection();
        private SqlDataAdapter oAdapterSql = new SqlDataAdapter();
        private SqlTransaction oTransacc;
        #endregion "VARIABLES"

        #region "PROPIEDADES"               
        public SqlConnection oSQLConnection { get { return this.oConnection; } }
        #endregion "PROPIEDADES"

        #region "CONSTRUCTOR"
        public Conexion(string cnx)
        {
            this.cnx = cnx;
        }
        #endregion "CONSTRUCTOR"

        #region Métodos

        public void OpenConnection()
        {

            this.oConnection.ConnectionString = this.cnx;
            this.oConnection.Open();
        }

        public void CloseConnection()
        {
            if (this.oConnection.State == ConnectionState.Open)
                this.oConnection.Close();
        }

        public DataSet FillDataSet(String sQuerySql, Hashtable objHash = null, int iTimeOut = 0)
        {
            Boolean bConexionAbierta = false;
            DataSet _objDataset = new DataSet();

            this.oCommandSql.Connection = this.oConnection;
            if (this.oConnection.State != ConnectionState.Open)
            {
                this.OpenConnection();
                bConexionAbierta = true;
            }

            this.oCommandSql.CommandType = CommandType.Text;
            this.oCommandSql.CommandText = sQuerySql;
            this.oCommandSql.CommandTimeout = 1200;
            if (iTimeOut > 0)
                this.oCommandSql.CommandTimeout = iTimeOut;
            this.oAdapterSql = new SqlDataAdapter(this.oCommandSql);
            _objDataset = new DataSet();
            if (objHash != null && objHash.Count > 0)
            {
                this.oCommandSql.Parameters.Clear();
                foreach (DictionaryEntry oDictionaryEntry in objHash)
                    this.oCommandSql.Parameters.AddWithValue(oDictionaryEntry.Key.ToString(), oDictionaryEntry.Value);
            }
            this.oAdapterSql.Fill(_objDataset);
            if (bConexionAbierta)
            {
                if (this.oConnection.State == ConnectionState.Open)
                    this.CloseConnection();
            }
            return _objDataset;

        }
        public int ExecuteCommand(String sQuerySql, Hashtable objHash = null)
        {
            int iRows = -1;
            bool bConexionAbierta = false;
            this.oCommandSql.Connection = this.oConnection;
            if (this.oConnection.State != ConnectionState.Open)
            {
                OpenConnection();
                bConexionAbierta = true;
            }
            this.oCommandSql.CommandType = CommandType.Text;
            this.oCommandSql.CommandText = sQuerySql;
            this.oCommandSql.CommandTimeout = 1200;
            if (objHash != null && objHash.Count > 0)
            {
                this.oCommandSql.Parameters.Clear();
                foreach (DictionaryEntry oDictionaryEntry in objHash)
                    this.oCommandSql.Parameters.AddWithValue(oDictionaryEntry.Key.ToString(), (object)oDictionaryEntry.Value ?? DBNull.Value);
            }
            iRows = this.oCommandSql.ExecuteNonQuery();
            if (bConexionAbierta)
                if (this.oConnection.State == ConnectionState.Open)
                    CloseConnection();
            return iRows;
        }

        public DataSet ExecteProcedure(String SentenciaProcedure, Hashtable oParams)
        {
            Boolean bConexionAbierta = false;
            DataSet _objDataset = new DataSet();

            this.oCommandSql.Connection = this.oConnection;
            if (this.oConnection.State != ConnectionState.Open)
            {
                this.OpenConnection();
                bConexionAbierta = true;
            }

            this.oCommandSql.CommandType = CommandType.StoredProcedure;
            this.oCommandSql.CommandText = SentenciaProcedure;
            this.oCommandSql.CommandTimeout = 1200;
            _objDataset = new DataSet();
            this.oCommandSql.Parameters.Clear();
            foreach (DictionaryEntry oDictionaryEntry in oParams)
                this.oCommandSql.Parameters.AddWithValue(oDictionaryEntry.Key.ToString(), oDictionaryEntry.Value);
            this.oAdapterSql = new SqlDataAdapter(this.oCommandSql);
            this.oAdapterSql.Fill(_objDataset);
            if (bConexionAbierta)
            {
                if (this.oConnection.State == ConnectionState.Open)
                    this.CloseConnection();
            }
            return _objDataset;
        } 
        public void StarTransacction()
        {
            this.oTransacc = this.oConnection.BeginTransaction();
            this.oCommandSql.Connection = this.oConnection;
            this.oCommandSql.Transaction = this.oTransacc;
        }
        public void CommitTransacction()
        {
            this.oTransacc.Commit();
        }
        public void RollBackTransacction()
        {
            this.oTransacc.Rollback();
        }
        #endregion
    }
}
