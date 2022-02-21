using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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

            dataReader.Close();
            conexao.Close();

            return lista;
        }

        public void Incluir(ProdutoInformation produto)
        {
            // conexao
            SqlConnection conexao = new SqlConnection();

            try
            {
                conexao.ConnectionString = Dados.StringDeConexao;

                // comando
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexao;
                comando.CommandText = "insert into Produtos(nome,preco,estoque) " +
                    "values (@nome, @preco, @estoque); select @@IDENTITY;";

                comando.Parameters.AddWithValue("@nome", produto.Nome);
                comando.Parameters.AddWithValue("@preco", produto.Preco);
                comando.Parameters.AddWithValue("@estoque", produto.Estoque);

                conexao.Open();
                produto.Codigo = Convert.ToInt32(comando.ExecuteScalar());
            }
            catch (SqlException ex)
            {
                throw new Exception("Servidor SQL Erro: " + ex.Number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        public void Alterar(ProdutoInformation produto)
        {
            // conexao
            SqlConnection conexao = new SqlConnection();

            try
            {
                conexao.ConnectionString = Dados.StringDeConexao;
                // comando
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexao;
                comando.CommandText = "AlterarProduto";
                comando.CommandType = CommandType.StoredProcedure;

                comando.Parameters.AddWithValue("@codigo", produto.Codigo);
                comando.Parameters.AddWithValue("@nome", produto.Nome);
                comando.Parameters.AddWithValue("@preco", produto.Preco);
                comando.Parameters.AddWithValue("@estoque", produto.Estoque);
                comando.Parameters.Add("@valorEstoque", SqlDbType.Int);
                comando.Parameters["@valorEstoque"].Direction = ParameterDirection.Output;

                conexao.Open();
                comando.ExecuteNonQuery();

                decimal valorEstoque = Convert.ToDecimal(comando.Parameters["@valorEstoque"]);

                if (valorEstoque < 500)
                {
                    throw new Exception("Atenção! Valor baixo no estoque.");
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Servidor SQL Erro: " + ex.Number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        public void Excluir(int codigo)
        {
            // conexao
            SqlConnection conexao = new SqlConnection();

            try
            {
                conexao.ConnectionString = Dados.StringDeConexao;

                // comando
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexao;
                comando.CommandText = "delete from Produtos where @codigo = " + codigo;

                conexao.Open();

                int resultado = comando.ExecuteNonQuery();

                if (resultado != 1)
                {
                    throw new Exception("não foi possível excluir o produto " + codigo);
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("Servidor SQL Erro: " + ex.Number);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}
