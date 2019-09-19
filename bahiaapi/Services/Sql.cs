using System.Threading.Tasks;
using System.Data.SqlClient;
using bahiaapi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;

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
                    cmd.Parameters.AddWithValue("@data", DateTime.ParseExact(ordem.data,"dd/MM/yyyy",CultureInfo.InvariantCulture));
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
                    SqlCommand cmd = new SqlCommand("SELECT O.quantidade AS [quantidade],O.preco AS [preco],O.data AS [data], O.classe_negociacao AS [negociacao],A.descricao AS [Ativo],O.id_ordem FROM dbo.Ordem as O  INNER JOIN dbo.Ativo AS A ON O.fk_id_ativo = A.id_ativo WHERE O.data = @novadata  ORDER BY O.data", conn);
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
        /// Método usado para buscar posições relacionadas a um determinado ativo no banco de dados
        /// </summary>]
        /// <param name="data" description="Data da posição"></param>
        /// <param name="nomeAtivo" description="Nome do ativo"></param>
        /// <returns>Retorna uma lista de posições</returns>
        public int getPosicao(string nomeAtivo,DateTime data) {
            using (SqlConnection conn = new SqlConnection(conString))
            {
                try
                {   
                    conn.Open();
                    //váriavel que receberá a posição
                    int posicao = 0;
                    //Comando Sql para buscar as posições e os valores gerados
                    SqlCommand cmd = new SqlCommand("SELECT SUM(CASE WHEN A.descricao = @descricao AND O.data <= convert(date,@data,103) THEN (CASE WHEN O.quantidade < 0 and O.classe_negociacao = 'V' THEN O.quantidade WHEN O.classe_negociacao = 'C' THEN O.quantidade ELSE O.quantidade*-1 END) ELSE 0 END) As Posicao FROM dbo.Ordem AS O,dbo.Ativo AS A; ", conn);
                    cmd.Parameters.AddWithValue("@descricao",nomeAtivo);
                    cmd.Parameters.AddWithValue("@data",data);
                    SqlDataReader dataReader = cmd.ExecuteReader();
                    
                    //Execução Linha a Linha do Resultado

                    while (dataReader.Read())
                    {
                        int index = 0;
                        posicao = Convert.ToInt32(dataReader.GetDouble(index++));

                    }
                    dataReader.Close();

                    return posicao;
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
