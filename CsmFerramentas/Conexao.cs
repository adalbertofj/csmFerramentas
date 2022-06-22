using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;

namespace CsmFerramentas
{
    class Conexao
                
    {

        SqlConnection con = new SqlConnection();


        //construtor
        public Conexao()
        {
            con.ConnectionString = @"Data Source=BDDEVSQL01\SQL01;Initial Catalog=COSMOS_PREPROD;User ID=USRPROCSM;Password=COSMOS";
        }
        
        //metodo conectar
        public SqlConnection Conectar()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
           
            }
            return con;
        }
        
        //metodo desconectar
        public void Desconectar()

        {

            if (con.State == System.Data.ConnectionState.Open )
            {
                con.Close ();
            }

        }

        //metodo para controlar transacao
        public void IniciaTransacao()
        {
            con.BeginTransaction();
           
        }

        public void FinalizaTransacao()
        {
            con.BeginTransaction().Commit();
        }

      

        public DataSet RetornarDataset(string sql)
        {
            SqlDataAdapter adapter = new SqlDataAdapter(sql, con);

            DataSet retorno = new DataSet();
            adapter.Fill(retorno, "retorno");

            return retorno;
        }
        




    }
}
