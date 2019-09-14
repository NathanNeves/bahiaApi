using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bahiaapi.Models;
namespace bahiaapi.Models
{   /// <summary>
///     Modelo usado para representar um determinado ativo no banco de dados 
/// </summary>
    public class Ativo
    {   
        //Id do Ativo
        public int id { get; set; }
        //Descricao do Ativo
        public string nomeAtivo {  get;  set; }
        //Quantidade do Ativo
        public int quantidade {  get;  set; }
    }
}
