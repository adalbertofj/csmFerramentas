using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace CsmFerramentas
{
    class EstornarNotaFiscal
    {
        Conexao conexao = new Conexao();
        SqlCommand cmd = new SqlCommand();
        public string mensagem;
        

        public EstornarNotaFiscal(string NotaFiscal)
          {
            
            try
            {

                //comando sql
                cmd.CommandText = "SELECT ID," +
                "KADE_CD_PRODUTO, " +
                "KADE_TX_NR_DOCTO, " +
                "KADE_QT_MOV, " +
                "KADE_TX_NUMLOTE " +
                "FROM " +
                "DB_TEMP.DBO.TMP_KARDEX_DEP_700 " +
                "WHERE PROCESSOU = 'N' " +
                "AND KADE_TX_NR_DOCTO  = '" + NotaFiscal + "'";

                SqlTransaction transaction;

                //conectar banco de dados
                cmd.Connection = conexao.Conectar();



                cmd.Parameters.Clear();
                DataSet ds = conexao.RetornarDataset(cmd.CommandText);
                

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    
                    transaction = cmd.Connection.BeginTransaction();
                    cmd.Transaction = transaction;

                    string sSql01 = "DECLARE @NUMRETORNO INT " +
                            "SET DATEFORMAT DMY " +
                            "SET NOCOUNT ON " +
                            "EXEC   @numRetorno = [dbo].[sp_ESDeposito] " +
                            "@numCD_Deposito       = 3 ," +
                            "@numCD_Produto        = @PRODUTO," +
                            "@strTP_Ocorr = 'E'," +
                            "@strTP_OrigemDestino = 3," +
                            "@numCD_OrigemDestino = 2," +
                            "@strTP_Movimento = 'EBA'," +
                            "@numQT_Quantidade = @QTMOV," +
                            "@numNR_NumDocumento = @NOTA_FISCAL," +
                            "@numCD_UsuAlt = 1," +
                            "@numRetorno = @numRetorno OUTPUT," +
                            "@strMotivo = 'Xxx'," +
                            "@strLote = @LOTE," +
                            "@flgAtualizaDevolucao = 'N' ";


                    cmd.CommandText = sSql01;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@ID", dr["ID"]);
                    cmd.Parameters.AddWithValue("@PRODUTO", dr["KADE_CD_PRODUTO"]);
                    cmd.Parameters.AddWithValue("@QTMOV", dr["KADE_QT_MOV"]);
                    cmd.Parameters.AddWithValue("@NOTA_FISCAL", dr["KADE_TX_NR_DOCTO"]);
                    cmd.Parameters.AddWithValue("@LOTE", dr["KADE_TX_NUMLOTE"]);

                                        

                    //SqlTransaction Transacao = conexao.IniciaTransacao();

                    //executa a procedure para realizar o estorno do produto da nota fiscal
                    cmd.CommandTimeout = 900;

                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch(SqlException)
                    {
                        continue;
                    }

                    
                    //StatusExecucao sta = new StatusExecucao(dr["KADE_TX_NR_DOCTO"]);

                    //cmd.ExecuteNonQuery();



                    // realiza o estorno do estoque reservado 
                    string sSql02 = "UPDATE PRODUTO_DEPOSITO " +
                                        "SET PRDP_QT_ESTRES = CASE WHEN(PRDP_QT_ESTRES - @QTMOV) < 0 THEN 0 ELSE(PRDP_QT_ESTRES - @QTMOV) END " +
                                        "WHERE DEPO_CD_DEPOSITO = 3 " +
                                        "AND PRME_CD_PRODUTO = @PRODUTO";

                        cmd.CommandText = sSql02;
                        cmd.ExecuteNonQuery();
                        

                    //marca o registro como processado
                    string sSql03 = "UPDATE DB_TEMP.DBO.TMP_KARDEX_DEP_700 SET PROCESSOU = 'S' WHERE ID = @ID ";

                        cmd.CommandText = sSql03;
                        cmd.ExecuteNonQuery();
                    
                    
                    //comito a transacao 
                    transaction.Commit();

                }


                this.mensagem = "Cadastrado com sucesso!";

            }
            catch (SqlException ex)
            {
                cmd.Connection.BeginTransaction().Rollback(); 
                MessageBox.Show(ex.Number + ex.Message);
                

            }

            conexao.Desconectar();

        }


}
}
