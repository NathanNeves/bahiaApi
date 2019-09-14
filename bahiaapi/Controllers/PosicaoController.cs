using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using bahiaapi.Services;
using bahiaapi.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bahiaapi.Controllers
{
    [Route("/Posicao")]
    public class PosicaoController : Controller
    {   private Sql sql = new Sql();
        //Método usado para retornar ao usuário todas as posições relacionadas a um ativo, ordenadas por data
        [HttpGet("{nomeAtivo}")]
        public IActionResult Get(string nomeAtivo)
        {   //Recebimento das posições do banco de dados
            List<Posicao> listaAtivos = sql.getPosicao(nomeAtivo);
            //Caso não exista nenhum registro no banco com aquele nome retornar erro, para o usuário
            if (listaAtivos.Count == 0) {
                return BadRequest(new {erro = "Não existe nenhuma posição com esse ativo, cheque o nome do ativo ou crie uma ordem"});
            }
            //Retornar lista com os ativos
            return Ok(listaAtivos);
        }

    }
}
