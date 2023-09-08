using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAtividadeEntrevista.Utils
{
    public class ValidadorCPF
    {
        /// <summary>
        /// Verifica se o cpf é válido
        /// </summary>
        /// <param name="cpf"></param>
        /// <returns></returns>
        public  static bool EValidoCPF(string cpf)
        {
            cpf = cpf.Replace(".", "").Replace("-", "");

            // Verifique se o CPF tem 11 dígitos numéricos
            if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            {
                return false;
            }

            // Calculo dos dígitos verificadores
            int soma = 0;
            for (int i = 0; i < 9; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (10 - i);
            }

            int primeiroDigito = 11 - (soma % 11);
            if (primeiroDigito >= 10)
            {
                primeiroDigito = 0;
            }

            if (int.Parse(cpf[9].ToString()) != primeiroDigito)
            {
                return false;
            }

            soma = 0;
            for (int i = 0; i < 10; i++)
            {
                soma += int.Parse(cpf[i].ToString()) * (11 - i);
            }

            int segundoDigito = 11 - (soma % 11);
            if (segundoDigito >= 10)
            {
                segundoDigito = 0;
            }

            if (int.Parse(cpf[10].ToString()) != segundoDigito)
            {
                return false;
            }

            return true;
        }
    }
}