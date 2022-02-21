using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Loja.Modelos;

namespace Loja.DAL
{
    public class ProdutosDAL
    {
        public ArrayList ProdutosEmFalta()
        {
            // conexao
            SqlConnection conexao = new SqlConnection(Dados.StringDeConexao);

            // comando
            SqlCommand comando = new SqlCommand("select * from Produtos where Estoque < 10", conexao);

            conexao.Open();
            SqlDataReader dataReader = comando.ExecuteReader();
            ArrayList lista = new ArrayList();

            while (dataReader.Read())
            {
                ProdutoInformation produto = new ProdutoInformation();
                produto.Codigo = Convert.ToInt32(dataReader["codigo"]);
                produto.Nome = dataReader["nome"].ToString();
                produto.Preco = Convert.ToDecimal(dataReader["preco"]);
                produto.Estoque = Convert.ToInt32(dataReader["preco"]);

                lista.Add(produto);
            }

            return lista;
        }
    }
}
