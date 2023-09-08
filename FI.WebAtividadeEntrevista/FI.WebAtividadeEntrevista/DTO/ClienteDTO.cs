using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAtividadeEntrevista.Models;

namespace WebAtividadeEntrevista.DTO
{
    public class ClienteDTO 
    {
        public ClienteModel Cliente { get; set; }
        public List<BeneficiarioModel> Beneficiarios { get; set; }
    }
}