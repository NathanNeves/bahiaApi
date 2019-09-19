using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bahiaapi.Services;
using bahiaapi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bahiaapi.Controllers
{
    [Route("/Posicao")]
    public class PosicaoController : Controller
    {   private Sql sql = new Sql();
        //Método usado para retornar ao usuário todas as posições relacionadas a um ativo, ordenadas por data
        [HttpGet("{nomeAtivo}/{dia}/{mes}/{ano}")]
        public IActionResult Get(string nomeAtivo,string dia,string mes,string ano)
        {
            string stringData = dia + "/" + mes + "/" + ano;
            DateTime data;
            //Tenta transformar a string em data, caso não obtenha sucesso retorna um erro para o usuário
            if (!DateTime.TryParseExact(stringData, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out data))
            {
                return BadRequest(new { erro = "Data Invalida" });

            }
            //Recebimento da posição do banco de dados
            int posicao = sql.getPosicao(nomeAtivo,data);
            //Retornar lista com os ativos
            return Ok(new {Ativo = nomeAtivo,dataPosição = stringData ,posicaoDoAtivo = posicao});
        }

    }
}
