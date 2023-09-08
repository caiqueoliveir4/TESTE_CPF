using System;
using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        /// <summary>
        /// Id Beneficiario
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// Nome Beneficiario
        /// </summary>
        [Required]
        public string Nome { get; set; }

        /// <summary>
        /// CPF do beneficiario
        /// </summary>
        [Required]
        [MaxLength(14)]
        [RegularExpression(@"^\d{3}\.\d{3}\.\d{3}-\d{2}$", ErrorMessage = "Digite um CPF Valido")]
        public string Cpf { get; set; }

        /// <summary>
        /// Id do cliente
        /// </summary>
        public long IdCliente { get; set; }
    }
}