using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bahiaapi.Models
{
    public class Posicao:Ordem
    {    //Quantidade de um determinado ativo após a operação de compra ou venda realizada
        public double posicao { get; set; }
        //Simulação da quantidade de dinheiro em caixa após a operação de compra ou venda realizada
        public double caixa { get; set; }
    }
}
