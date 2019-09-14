using System.Threading.Tasks;
using System.Data.SqlClient;
using bahiaapi.Models;
using System;
using System.Collections.Generic;

namespace bahiaapi.Services
{
    public class Sql
    {
        //String de conexão
        private readonly string conString = "Data Source=bahiadb.cw3f3qkeipyy.us-east-1.rds.amazonaws.com,1433;Initial Catalog=bahia;User ID =nathan;Password=keykeychave;";

        /// <summary>
        /// Método que serve para buscar no banco de dados um Ativo, usando o nome como parametro para busca
        /// </summary>
        /// <param name="nome"></param>
        /// <returns description="Um objeto da Classe Ativo"></returns>
        public Ativo getAtivo(string nome)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                     //Ativo a ser retornado
                    Ativo ativo = new Ativo();
                    //Comando Sql usado para selectionar um determinado ativo de acordo com a descricao
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Ativo WHERE descricao = @descricao", conn);
                    cmd.Parameters.AddWithValue("@descricao", nome);
                    SqlDataReader dataReader = cmd.ExecuteReader();
                   //Leitor linha a linha do resultado da query
                    while (dataReader.Read())
                    {
                        int index = 0;
                        ativo.id = dataReader.GetInt32(index++);
                        ativo.nomeAtivo = dataReader.GetString(index++);
                        ativo.quantidade = dataReader.GetInt32(index++);
                    }
                    dataReader.Close();
                    return ativo;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        /// <summary>
        /// Método usado para cadastrar uma ordem de compra ou de Venda
        /// </summary>
        /// <param name="ordem"></param>
        /// <returns description="Retorna um inteiro"></returns>
        public int setOrdem(Ordem ordem)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Ordem(fk_id_ativo,quantidade,preco,data,classe_negociacao) VALUES (@idativo,@quantidade,@preco,@data,@classe_negociacao)");
                    cmd.Parameters.AddWithValue("@idativo", ordem.id);
                    cmd.Parameters.AddWithValue("@quantidade", ordem.quantidade);
                    cmd.Parameters.AddWithValue("@preco", ordem.preco);
                    cmd.Parameters.AddWithValue("@data", DateTime.Parse(ordem.data));
                    cmd.Parameters.AddWithValue("@classe_negociacao", ordem.negociacao);
                    cmd.Connection = conn;
                    int linhasAfetadas = cmd.ExecuteNonQuery();
                    return linhasAfetadas;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                }

            }
        }

        /// <summary>
        /// Método que busca no banco de dados todas ordens efetuadas em uma determinada data
        /// </summary>
        /// <param name="data"></param>
        /// <returns description="Retorna uma lista com as ordens buscadas no banco de dados"></returns>
        public List<Ordem> getOrdem(DateTime data)
        {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {   //Lista de ativos a ser retornada
                    List<Ordem> listaOrdens = new List<Ordem>();
                    //Conexao aberta
                    conn.Open();
                    //Query para selecionar todos os ativos em uma determinada data
                    SqlCommand cmd = new SqlCommand("SELECT O.quantidade AS [quantidade],O.preco AS [preco],O.data AS [data], O.classe_negociacao AS [negociacao],A.descricao AS [Ativo] FROM dbo.Ordem as O  INNER JOIN dbo.Ativo AS A ON O.fk_id_ativo = A.id_ativo WHERE O.data = @novadata  ORDER BY O.data", conn);
                    cmd.Parameters.AddWithValue("@novadata", data);
                    SqlDataReader dataReader = cmd.ExecuteReader();

                    //Loop linha a linha do resultado da query no banco de dados
                    while (dataReader.Read())
                    {
                        int index = 0;
                        Ordem ordem = new Ordem();
                        ordem.quantidade = Convert.ToInt32(dataReader.GetDouble(index++));
                        ordem.preco = dataReader.GetDouble(index++);
                        ordem.data = Convert.ToString(dataReader.GetDateTime(index++)).Split(" ")[0];
                        ordem.negociacao = Convert.ToChar(dataReader.GetString(index++));
                        ordem.nomeAtivo = dataReader.GetString(index++);
                        listaOrdens.Add(ordem);
                    }
                    dataReader.Close();
                    return listaOrdens;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                }


            }
        }
        /// <summary>
        /// Método usado para buscar todas as ordens presentes no banco de dados ordenadas pela data de criação
        /// </summary>
        /// <returns description="Retorna uma lista de objetos da classe Ordem"></returns>        
        public List<Ordem> getOrdem() {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {   //Lista de Ordens a ser retornada
                    List<Ordem> listaOrdens = new List<Ordem>();
                    conn.Open();
                    //Query usada para buscar todas as ordens de criacao
                    SqlCommand cmd = new SqlCommand("SELECT O.quantidade AS [quantidade],O.preco AS [preco],O.data AS [data], O.classe_negociacao AS [negociacao],A.descricao AS [Ativo] FROM dbo.Ordem as O  INNER JOIN dbo.Ativo AS A ON O.fk_id_ativo = A.id_ativo ORDER BY O.data", conn);
                    SqlDataReader dataReader = cmd.ExecuteReader();
                   //Loop linha a linha do resultado da query no banco de dados
                    while (dataReader.Read())
                    {
                        int index = 0;
                        Ordem ordem = new Ordem();
                        ordem.quantidade = Convert.ToInt32(dataReader.GetDouble(index++));
                        ordem.preco = dataReader.GetDouble(index++);
                        ordem.data = Convert.ToString(dataReader.GetDateTime(index++)).Split(" ")[0];
                        ordem.negociacao = Convert.ToChar(dataReader.GetString(index++));
                        ordem.nomeAtivo = dataReader.GetString(index++);
                        listaOrdens.Add(ordem);
                    }
                    //Fechamento do reader
                    dataReader.Close();
                    return listaOrdens;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                }


            }
        }

        /// <summary>
        /// Método usado para buscar posições relacionadas a um determinado ativo no banco de dados
        /// </summary>
        /// <param name="nomeAtivo" description="Nome do ativo"></param>
        /// <returns description="Retorna uma lista de posições"></returns>
        public List<Posicao> getPosicao(string nomeAtivo) {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {   //Lista de Posições a serem retornadas
                    List<Posicao> listaPosicao = new List<Posicao>();
                    conn.Open();
                    //Comando Sql para buscar as posições e os valores gerados
                    SqlCommand cmd = new SqlCommand("SELECT A.descricao AS [Ativo],O.data,O.classe_negociacao AS [Negociacao],O.preco AS preco,O.quantidade,SUM(CASE WHEN O.quantidade < 0 and O.classe_negociacao = 'V' THEN O.quantidade WHEN O.classe_negociacao = 'C' THEN O.quantidade ELSE O.quantidade*-1 END) OVER (ORDER BY O.data rows unbounded preceding) AS posicao,SUM(CASE WHEN O.classe_negociacao = 'C' THEN O.quantidade*-1*O.preco ELSE (CASE WHEN O.quantidade < 0 THEN O.quantidade*-1*O.preco ELSE O.quantidade*O.preco END) END)  OVER (ORDER BY O.data rows unbounded preceding) AS caixa FROM dbo.Ordem AS O  INNER JOIN dbo.Ativo AS A ON O.fk_id_ativo = A.id_ativo WHERE A.descricao = @descricao ORDER BY O.data", conn);
                    cmd.Parameters.AddWithValue("@descricao",nomeAtivo);
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    //Execução Linha a Linha do Resultado
                    while (dataReader.Read())
                    {
                        int index = 0;
                        Posicao posicao = new Posicao();
                        posicao.nomeAtivo = dataReader.GetString(index++);
                        posicao.data = Convert.ToString(dataReader.GetDateTime(index++)).Split(" ")[0];
                        posicao.negociacao = Convert.ToChar(dataReader.GetString(index++));
                        posicao.preco = dataReader.GetDouble(index++);
                        posicao.quantidade = Convert.ToInt32(dataReader.GetDouble(index++));
                        posicao.posicao = dataReader.GetDouble(index++);
                        posicao.caixa = dataReader.GetDouble(index++);
                        listaPosicao.Add(posicao);
                    }
                    dataReader.Close();

                    return listaPosicao;
                }
                catch (Exception e)
                {
                    throw e;
                }
                finally
                {
                    conn.Close();
                }


            }

        }
    }
}
