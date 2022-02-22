using Loja.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Loja.DAL
{
    public class VendasDAL
    {
        // Propriedade que retorna uma lista de produtos
        public DataTable ListaDeProdutos
        {
            get
            {
                // conexão
                SqlConnection conexao = new SqlConnection();
                conexao.ConnectionString = Dados.StringDeConexao;
                conexao.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(
                    "select * from produtos", conexao);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                conexao.Close();

                return dataTable;
            }
        }

        public DataTable dataTable
        {
            get
            {
                // conexão
                SqlConnection conexao = new SqlConnection();
                conexao.ConnectionString = Dados.StringDeConexao;
                conexao.Open();

                SqlDataAdapter dataAdapter = new SqlDataAdapter(
                    "select * from Clientes", conexao);

                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                conexao.Close();

                return dataTable;
            }
        }

        public void Incluir(VendaInformation venda)
        {
            //conexão
            SqlConnection conexao = new SqlConnection();
            SqlTransaction transacao = null;

            try
            {
                conexao.ConnectionString = Dados.StringDeConexao;

                // comando
                SqlCommand comando1 = new SqlCommand();
                comando1.Connection = conexao;
                comando1.CommandText = @"insert into Vendas (
                    CodigoCliente,
                    CodigoProduto
                    Data,
                    Quantidade,
                    Faturado)
                    VALUES (
                    @CodigoCliente,
                    @CodigoProduto,
                    @Data,
                    @Quantidade,
                    @Faturado); select @@IDENTITY;";

                SqlCommand comando2 = new SqlCommand();
                comando2.Connection = conexao;
                comando2.CommandText = @"update Produtos
                    Set Estoque = Estoque - @Quantidade
                    where Codigo=@CodigoProduto";

                conexao.Open();

                transacao = conexao.BeginTransaction(IsolationLevel.Serializable); //default

                comando1.Transaction = transacao;
                comando2.Transaction = transacao;

                comando1.Parameters.AddWithValue("@CodigoCliente", venda.CodigoCliente);
                comando1.Parameters.AddWithValue("@CodigoProduto", venda.CodigoProduto);
                comando1.Parameters.AddWithValue("@Data", venda.Data);
                comando1.Parameters.AddWithValue("@Quantidade", venda.Quantidade);
                comando1.Parameters.AddWithValue("@Faturado", venda.Faturado);

                comando2.Parameters.AddWithValue("@Quantidade", venda.Quantidade);
                comando2.Parameters.AddWithValue("@CodigoProduto", venda.CodigoProduto);

                venda.Codigo = Convert.ToInt32(comando1.ExecuteScalar());
                comando2.ExecuteNonQuery();

                transacao.Commit();
            }
            catch (Exception ex)
            {
                transacao.Rollback();
                throw new Exception("Erro no servidor: " + ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}
