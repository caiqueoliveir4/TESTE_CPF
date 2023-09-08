using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoBeneficiario
    {
        /// <summary>
        /// Inclui um novo beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public long Incluir(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario bn = new DAL.DaoBeneficiario();
            return bn.Incluir(beneficiario);
        }

        /// <summary>
        /// Altera um beneficiario
        /// </summary>
        /// <param name="beneficiario">Objeto de beneficiario</param>
        public void Alterar(DML.Beneficiario beneficiario)
        {
            DAL.DaoBeneficiario bn = new DAL.DaoBeneficiario();
            bn.Alterar(beneficiario);
        }

        /// <summary>
        /// Consulta o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public DML.Beneficiario Consultar(long id)
        {
            DAL.DaoBeneficiario bn = new DAL.DaoBeneficiario();
            return bn.Consultar(id);
        }

        /// <summary>
        /// Excluir o beneficiario pelo id
        /// </summary>
        /// <param name="id">id do beneficiario</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoBeneficiario bn = new DAL.DaoBeneficiario();
            bn.Excluir(id);
        }

        /// <summary>
        /// Lista os beneficiarios por idCliente
        /// </summary>
        public List<DML.Beneficiario> Listar(long idCliente)
        {
            DAL.DaoBeneficiario bn = new DAL.DaoBeneficiario();
            return bn.Listar(idCliente);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoBeneficiario bn = new DAL.DaoBeneficiario();
            return bn.VerificarExistencia(CPF);
        }

        /// <summary>
        /// Verifica há beneficiario com CPF duplicado para o mesmo id de cliente
        /// </summary>
        /// <param name="cpf"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool VerificaBeneficiarioDuplicado(string cpf, long id)
        {
            DAL.DaoBeneficiario bn = new DAL.DaoBeneficiario();
            return bn.VerificaBeneficiarioDuplicado(cpf, id);
        }

    }
}
