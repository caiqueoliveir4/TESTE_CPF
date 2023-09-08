using FI.AtividadeEntrevista.BLL;
using WebAtividadeEntrevista.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FI.AtividadeEntrevista.DML;
using WebAtividadeEntrevista.DTO;
using FromBody = System.Web.Http.FromBodyAttribute;
using WebAtividadeEntrevista.Utils;
using WebGrease.Css.Extensions;

namespace WebAtividadeEntrevista.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Incluir()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Incluir([FromBody] ClienteDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var erros = ModelState.Values
                        .SelectMany(item => item.Errors)
                        .Select(error => error.ErrorMessage)
                        .ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }

                var clienteBo = new BoCliente();
                var beneficiarioBo = new BoBeneficiario();

                if (clienteBo.VerificarExistencia(model.Cliente.Cpf))
                {
                   
                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado");
                }

                if (!ValidadorCPF.EValidoCPF(model.Cliente.Cpf))
                {
                  
                    Response.StatusCode = 400;
                    return Json("CPF inválido");
                }

                if (model.Beneficiarios.Any(beneficiario => !ValidadorCPF.EValidoCPF(beneficiario.Cpf)))
                {
                  
                    Response.StatusCode = 400;
                    return Json("CPF inválido");
                }

                var beneficiariosDuplicados = model.Beneficiarios.Where(beneficiario => model.Beneficiarios.Count(beneficiario2 => beneficiario.Cpf == beneficiario2.Cpf) != 1);
                if (beneficiariosDuplicados.Any())
                {
                   
                    Response.StatusCode = 400;
                    return Json($"O CPF {beneficiariosDuplicados.First().Cpf} está em mais de um beneficiário");
                }

                model.Cliente.Id = clienteBo.Incluir(new Cliente()
                {
                    CEP = model.Cliente.CEP,
                    Cidade = model.Cliente.Cidade,
                    Email = model.Cliente.Email,
                    Estado = model.Cliente.Estado,
                    Logradouro = model.Cliente.Logradouro,
                    Nacionalidade = model.Cliente.Nacionalidade,
                    Cpf = model.Cliente.Cpf,
                    Nome = model.Cliente.Nome,
                    Sobrenome = model.Cliente.Sobrenome,
                    Telefone = model.Cliente.Telefone
                });

                foreach (BeneficiarioModel beneficiario in model.Beneficiarios)
                {
                    beneficiarioBo.Incluir(new Beneficiario()
                    {
                        Cpf = beneficiario.Cpf,
                        Nome = beneficiario.Nome,
                        IdCliente = model.Cliente.Id
                    });
                }

                return Json("Cadastro efetuado com sucesso");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json("Ocorreu um erro durante o cadastramento: " + ex.Message);
            }
        }



        [HttpPost]
        public JsonResult Alterar([FromBody] ClienteDTO model)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    var erros = ModelState.Values
                        .SelectMany(item => item.Errors)
                        .Select(error => error.ErrorMessage)
                        .ToList();

                    Response.StatusCode = 400;
                    return Json(string.Join(Environment.NewLine, erros));
                }

                var clienteBo = new BoCliente();
                var beneficiarioBo = new BoBeneficiario();               

                if (clienteBo.VerificarExistencia(model.Cliente.Cpf, model.Cliente.Id))
                {

                    Response.StatusCode = 400;
                    return Json("CPF já cadastrado");
                }

                if (!ValidadorCPF.EValidoCPF(model.Cliente.Cpf))
                {

                    Response.StatusCode = 400;
                    return Json("CPF inválido");
                }

                if (model.Beneficiarios.Any(beneficiario => !ValidadorCPF.EValidoCPF(beneficiario.Cpf)))
                {

                    Response.StatusCode = 400;
                    return Json("CPF inválido");
                }

                var beneficiariosDuplicados = model.Beneficiarios.Where(beneficiario => model.Beneficiarios.Count(beneficiario2 => beneficiario.Cpf == beneficiario2.Cpf) != 1);
                if (beneficiariosDuplicados.Any())
                {

                    Response.StatusCode = 400;
                    return Json($"O CPF {beneficiariosDuplicados.First().Cpf} está em mais de um beneficiário");
                }

                clienteBo.Alterar(new Cliente()
                {
                    Id = model.Cliente.Id,
                    CEP = model.Cliente.CEP,
                    Cidade = model.Cliente.Cidade,
                    Email = model.Cliente.Email,
                    Estado = model.Cliente.Estado,
                    Logradouro = model.Cliente.Logradouro,
                    Nacionalidade = model.Cliente.Nacionalidade,
                    Cpf = model.Cliente.Cpf,
                    Nome = model.Cliente.Nome,
                    Sobrenome = model.Cliente.Sobrenome,
                    Telefone = model.Cliente.Telefone
                });

                var idsBeneficiariosInseridosOuAlterados = new List<long>();
                var idsBeneficiariosAtuais = beneficiarioBo.Listar(model.Cliente.Id).Select(x => x.Id).ToList();
                foreach (BeneficiarioModel beneficiario in model.Beneficiarios)
                {
                    if (beneficiario.Id.HasValue)
                    {
                        beneficiarioBo.Alterar(new Beneficiario()
                        {
                            Id = (long) beneficiario.Id,
                            Cpf = beneficiario.Cpf,
                            Nome = beneficiario.Nome,
                            IdCliente = model.Cliente.Id
                        });
                        idsBeneficiariosInseridosOuAlterados.Add((long) beneficiario.Id);
                    } else {
                        var resultado = beneficiarioBo.Incluir(new Beneficiario()
                        {
                            Cpf = beneficiario.Cpf,
                            Nome = beneficiario.Nome,
                            IdCliente = model.Cliente.Id
                        });
                        idsBeneficiariosInseridosOuAlterados.Add(resultado);
                    }
                }

                // Remove os beneficiarios que foram excluidos
                idsBeneficiariosAtuais.Where(x => !idsBeneficiariosInseridosOuAlterados.Any(y => y == x)).ForEach(id => beneficiarioBo.Excluir(id));

                return Json("Alteração feita com sucesso");

            }
            catch (Exception ex)
            {
                Response.StatusCode = 500; // Código de erro do servidor
                return Json("Ocorreu um erro durante a alteração: " + ex.Message);
            }
        }

        [HttpGet]
        public ActionResult Alterar(long id)
        {
            BoCliente boCliente = new BoCliente();
            BoBeneficiario boBeneficiario = new BoBeneficiario();
            Cliente cliente = boCliente.Consultar(id);
            List<Beneficiario> beneficiarios = boBeneficiario.Listar(id);
            ClienteDTO model = new ClienteDTO();

            if (cliente != null)
            {
                model.Cliente = new ClienteModel()
                {
                    Id = cliente.Id,
                    CEP = cliente.CEP,
                    Cidade = cliente.Cidade,
                    Email = cliente.Email,
                    Estado = cliente.Estado,
                    Logradouro = cliente.Logradouro,
                    Nacionalidade = cliente.Nacionalidade,
                    Cpf = cliente.Cpf,
                    Nome = cliente.Nome,
                    Sobrenome = cliente.Sobrenome,
                    Telefone = cliente.Telefone
                };
            }

            if (beneficiarios != null)
            {
                model.Beneficiarios = new List<BeneficiarioModel>(); 

                foreach (Beneficiario beneficiario in beneficiarios)
                {
                    BeneficiarioModel beneficiarioModel = new BeneficiarioModel()
                    {
                        Id = beneficiario.Id,
                        Cpf = beneficiario.Cpf,
                        Nome = beneficiario.Nome,
                        IdCliente = beneficiario.IdCliente
                    };

                    model.Beneficiarios.Add(beneficiarioModel); 
                }
            }

            return View(model);
        }



        [HttpPost]
        public JsonResult ClienteList(int jtStartIndex = 0, int jtPageSize = 0, string jtSorting = null)
        {
            try
            {
                int qtd = 0;
                string campo = string.Empty;
                string crescente = string.Empty;
                string[] array = jtSorting.Split(' ');

                if (array.Length > 0)
                    campo = array[0];

                if (array.Length > 1)
                    crescente = array[1];

                List<Cliente> clientes = new BoCliente().Pesquisa(jtStartIndex, jtPageSize, campo, crescente.Equals("ASC", StringComparison.InvariantCultureIgnoreCase), out qtd);

                //Return result to jTable
                return Json(new { Result = "OK", Records = clientes, TotalRecordCount = qtd });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "ERROR", Message = ex.Message });
            }
        }

        
       
    }
}