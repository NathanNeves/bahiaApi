using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bahiaapi.Services;
using bahiaapi.Models;
using bahiaapi.Validators;
using FluentValidation;
using FluentValidation.Results;
using System.Globalization;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace bahiaapi.Controllers
{
    [Route("/Ordem")]
    public class OrdemController : Controller
    {    //Instanciação das queries sql
        private Sql sql = new Sql();
        /// <summary>
        /// Função do tipo POST que possui como objetivo cadastrar uma ordem de compra ou venda para o usuário
        /// </summary>
        /// <param name="ordem">Nome da Ordem</param>
        /// <returns>Retorna uma um statusCode com diferentes com um json</returns>
        [HttpPost]
        public IActionResult registrarOrdem([FromBody] Models.Ordem ordem)
        {   
            //Validador do request feito pelo usuário
            OrdemValidator validator = new OrdemValidator();
            //Resultado da validação dos dados obtidos através do requesto feito pelo usuário
            ValidationResult resultado = validator.Validate(ordem);
            
            //Caso exista algum dado considerado inválido retorna uma lista de erros para o usuário
            if (!resultado.IsValid)
            { List<string> listaErro = new List<string>();
                foreach (var erro in resultado.Errors) {
                    listaErro.Add(erro.ErrorMessage);
                }
                return BadRequest(new { erro = listaErro });
            }
            //Caso não exista nenhum erro, realiza a consulta no banco de dados em busca de um ativo com o mesmo nome dado na ordem
            Ativo ativoResponse = sql.getAtivo(ordem.nomeAtivo);
            //Se não encontrar nenhum ativo com esse nome retorna um erro ao usuário
            if (ativoResponse.nomeAtivo == null)
            {

                return BadRequest(new { erro = "Ativo não encontrado" });
            }
            //Caso encontre, faz uma validação para se a quantidade solicitada bate com o lote mínimo
            if (!ordem.validarQuantidadeValor(ordem.quantidade, ativoResponse.quantidade)) {

                return base.BadRequest(new { erro = "A quantidade mínima permitada para esse ativo é de " + ativoResponse.quantidade });
            }
            //Caso seja maior que o lote mínimo, verifica se é um multiplo do lote mínimo
            if (!Models.Ordem.validarQuantidadeMultiplo(ordem.quantidade, ativoResponse.quantidade)) {

                return base.BadRequest(new { erro = "A quantidade precisa ser múltipla de " + ativoResponse.quantidade });
            }
            //atribui o id da ordem o valor do id do ativo resgatado no banco de dados
            ordem.tratarOrdem(ativoResponse);
            //Tenta realizar um INSERT no banco de dados
            int insertResponse = sql.setOrdem(ordem);
            //Caso tenha conseguido retorna uma mensagem para o usuário
            if (insertResponse > 0) {
                return Ok(new { mensagem = "Ordem inserida com sucesso" });
            }
            //Caso não tenha conseguido retorna um erro para o usuário
            return BadRequest(new { erro = "Houve algum problema na criação da ordem, tente novamente mais tarde ou contate o administrador do sistema" });

        }

        /// <summary>
        /// Requisição do tipo GET que retorna todas as ordens realizadas em um determinado dia
        /// </summary>
        /// <param name="dia">Dia em que as ordens foram efetuadas</param>
        /// <param name="mes">Mês em que as ordens foram efetuadas</param>
        /// <param name="ano">Ano em que as ordens foram efetuadas</param>
        /// <returns></returns>
        [HttpGet("{dia}/{mes}/{ano}")]
        public IActionResult Get(string dia,string mes,string ano) {
            //Variável  que constroe a string de data
            string stringData = dia + "/"+mes+"/" + ano;
            DateTime data;
            //Tenta transformar a string em data, caso não obtenha sucesso retorna um erro para o usuário
            if (!DateTime.TryParseExact(stringData,"dd/MM/yyyy",CultureInfo.InvariantCulture,DateTimeStyles.None, out data)) {
                return BadRequest(new { erro = "Data Invalida, tente o formato DD/MM/YYYY" });

            }
            //Realiza a consulta no banco de dados
            List<Models.Ordem> list = sql.getOrdem(data);
            //Caso a lista esteja vazia, retorna um erro para o usuário
            if (list.Count == 0) {
                return NotFound(new {erro = "Nenhum registro foi encontrado"});
            }
            //Caso a lista não esteja vazia, retorna a lista
            return Ok(list);

        }
    
    }
        
        
    }

    