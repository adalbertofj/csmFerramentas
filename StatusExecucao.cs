using System;

public class statusexecucao (string NotaFiscal)
{
	Conexao conexao = new Conexao();
	SqlCommand cmd = new SqlCommand();

	public Processados ()
	{
	        //comando sql
            cmd.CommandText = "SELECT  COUNT(ID) QTD FROM DB_TEMP.DBO.TMP_KARDEX_DEP_700 WHERE KADE_TX_NR_DOCTO = @NOTA_FISCAL AND PROCESSOU = 'S' = '" + NotaFiscal + "'";

             //conectar banco de dados
            cmd.Connection = conexao.Conectar();

            DataSet ds = conexao.RetornarDataset(cmd.CommandText);

            DataRow dr in ds.Tables[0].Rows;

            return (ds.[QTD]);

    }


    public NaoProcessados()
    {
        //comando sql
        cmd.CommandText = "SELECT  COUNT(ID) QTD FROM DB_TEMP.DBO.TMP_KARDEX_DEP_700 WHERE KADE_TX_NR_DOCTO = @NOTA_FISCAL AND PROCESSOU = 'N' = '" + NotaFiscal + "'";

        //conectar banco de dados
        cmd.Connection = conexao.Conectar();

        DataSet ds = conexao.RetornarDataset(cmd.CommandText);

        DataRow dr in ds.Tables[0].Rows;

        return (ds.[QTD]);

    }

    public Total()
    {
        //comando sql
        cmd.CommandText = "SELECT  COUNT(ID) QTD FROM DB_TEMP.DBO.TMP_KARDEX_DEP_700 WHERE KADE_TX_NR_DOCTO = @NOTA_FISCAL = '" + NotaFiscal + "'";

        //conectar banco de dados
        cmd.Connection = conexao.Conectar();

        DataSet ds = conexao.RetornarDataset(cmd.CommandText);
        DataRow dr in ds.Tables[0].Rows;

        return (ds.[QTD]);
    }
}
