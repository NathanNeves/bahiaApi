using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections;
using System.Globalization;
using System.ComponentModel.DataAnnotations;

namespace bahiaapi.Models
{
    public class Ordem:Ativo
    {   //Atributo para receber a data da ordem
        public string data { get; set; }
        //Atributo para receber o tipo de negociação efetuada
        public char negociacao { get; set; }
        //Atributo para determinar o preço da ação no dia em que a operação foi efetuada
        public double preco { get; set; }


        /// <summary>
        /// Validador que verifica se a quantidade inserida é maior que o lote mínimo, retorna True se sim e False se não
        /// </summary>
        /// <param name="quantidadeInserida">quantidade inserida pelo usuário</param>
        /// <param name="loteMinimo">lote mínimo do ativo solicitado</param>
        /// <returns></returns>
        public bool validarQuantidadeValor(int quantidadeInserida,int loteMinimo) {
           //Se o tipo de negociacao for uma compra verifica se a quantidade inserida é maior que o lote mínimo
            if (this.negociacao == 'C')
            {
                bool validacao = ((quantidadeInserida > loteMinimo)) ? true : false;
                return validacao;
            }
            //Caso o contrário retorna false
            return true;
            

        }

        /// <summary>
        /// Validador que verifica se a quantidade inserida é multipla do lote mínimo 
        /// </summary>
        /// <param name="quantidadeInserida">Quantidade fornecida pelo usuário ao enviar os dados</param>
        /// <param name="loteMinimo">Quantidade necessária para efetuar uma ordem</param>
        /// <returns>Retorna um true se a quantidade inserida for multipla do lote mínimo</returns>
        public static bool validarQuantidadeMultiplo(int quantidadeInserida,int loteMinimo) {
            bool validacao = (quantidadeInserida % loteMinimo == 0) ? true : false;
            return validacao;
        }

        /// <summary>
        /// Trata algumas informações relacionadas a uma determinada ordem
        /// </summary>
        /// <param name="ativo">Ativo a ser tratado</param>
        public void tratarOrdem(Ativo ativo) {
            //Atribui o id do ativo ao id da ordem
            this.id = ativo.id;

        }


    }

    }

