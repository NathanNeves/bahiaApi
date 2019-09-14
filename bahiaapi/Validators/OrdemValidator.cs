using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using bahiaapi.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
namespace bahiaapi.Validators
{
    public class OrdemValidator: AbstractValidator<Ordem>
    {
        public OrdemValidator() {

            RuleFor(x => x.quantidade).NotNull().WithMessage("O campo quantidade não pode ser nulo").NotEmpty().WithMessage("O campo quantidade não pode ser vazio").Must(quant=> quant!=0).WithMessage("quantidade não pode ser 0");
            RuleFor(x => x.data).NotNull().WithMessage("O campo data não pode ser nulo").NotEmpty().WithMessage("O campo data não pode ser vazio").Custom((data,context)=> {
                DateTime valorData;
                if (!DateTime.TryParse(data,out valorData))
                {
                    context.AddFailure("O campo data precisa seguir a seguinte formatação DD/MM/AAAA");
                }
                else if (valorData > DateTime.Today) {
                    context.AddFailure("O campo data precisa ser menor ou igual ao dia de hoje ");
                }
            });
            RuleFor(x => x.negociacao).NotNull().WithMessage("O campo negociacao não pode ser nulo").NotEmpty().WithMessage("O campo negociacao não pode ser vazio").Custom((negociacao, context) =>
            {
                bool validacao = ((negociacao == 'C') || (negociacao == 'V')) ? true : false;
                if (!validacao) {
                    context.AddFailure("O campo negociacao só aceita os valores C para compra ou V para venda");
                }
            });
            RuleFor(x => x.preco).NotNull().WithMessage("Preco não pode ser nulo").NotEmpty().WithMessage("Preco não pode ser vazio").GreaterThan(0).WithMessage("Preco deve ser maior do que zero");
        }
    }
}
