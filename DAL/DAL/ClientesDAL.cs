using System;
using Loja.Modelos;
using System.Data.SqlClient;
using System.Data;

namespace Loja.DAL
{
    public class ClientesDAL
    {
        // Métodos

        public void Incluir (ClienteInformation cliente)
        {
            // conexão
            SqlConnection conexao = new SqlConnection();

            conexao.ConnectionString = Dados.StringDeConexao;

            try
            {
                // comando
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexao;
                comando.CommandText = "insert into Clientes(nome,email,telefone)" +
                    " values (@nome,@email,@telefone); select @@IDENTITY;";
                comando.Parameters.AddWithValue("@nome", cliente.Nome);
                comando.Parameters.AddWithValue("@email", cliente.Email);
                comando.Parameters.AddWithValue("@telefone", cliente.Telefone);

                conexao.Open();
                cliente.Codigo = Convert.ToInt32(comando.ExecuteScalar());
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

        public void Alterar (ClienteInformation cliente)
        {
            // conexão
            SqlConnection conexao = new SqlConnection();

            try
            {
                conexao.ConnectionString = Dados.StringDeConexao;

                // comando
                SqlCommand comando = new SqlCommand();
                comando.Connection = conexao;
                comando.CommandType = CommandType.Text;
                comando.CommandText = "update Clientes set nome = @nome, email = @email," +
                    "telefone = @telefone where codigo = @codigo;";
                comando.Parameters.AddWithValue("@codigo", cliente.Codigo);
                comando.Parameters.AddWithValue("@nome", cliente.Nome);
                comando.Parameters.AddWithValue("@email", cliente.Email);
                comando.Parameters.AddWithValue("@telefone", cliente.Telefone);

                conexao.Open();
                comando.ExecuteNonQuery();
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
                comando.CommandText = "delete from Clientes where codigo = " + codigo;

                conexao.Open();

                int resultado = comando.ExecuteNonQuery();

                if (resultado != 1)
                {
                    throw new Exception("Não foi possível excluir o cliente " + codigo);
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

        public DataTable Listagem()
        {
            DataTable tabela = new DataTable();
            SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from Clientes", Dados.StringDeConexao);
            dataAdapter.Fill(tabela);
            return tabela;
        }
    }
}
