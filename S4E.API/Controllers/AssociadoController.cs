using Microsoft.AspNetCore.Mvc;
using S4E.ADO.Models.Dto.AssociadoDto;
using S4E.ADO.Models;
using S4E.ADO.Services;

namespace S4E.ADO.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssociadoController : Controller
    {
        private AssociadoService _associadoService;

        public AssociadoController(AssociadoService associadoService)
        {
            _associadoService = new AssociadoService();
        }
        #region "METODOS"

        [HttpPost]
        public ActionResult AdicionaAssociado([FromBody] CreateAssociadoDto associuadoDto)
        {
            Associado associado = _associadoService.AdicionaAssociado(associuadoDto);
            return CreatedAtAction(nameof(RecuperaAssociadoPorId), associado.id, associado);
        }

        [HttpGet("id/{id}")]
        public ActionResult RecuperaAssociadoPorId(int id)
        {
            Associado associado = _associadoService.RecuperaAssociadoPorId(id);
            if (associado != null)
            {
                return Ok(associado);
            }
            return NotFound();
        }
        [HttpGet("cpf/{cpf}")]
        public IActionResult RecuperaAssociadoPorCpf(string cpf)
        {
            Associado associado = _associadoService.RecuperaAssociadoPorCPF(cpf);
            if (associado != null)
            {
                return Ok(associado);
            }
            return NotFound();
        }
        [HttpGet("nome/{nome}")]
        public IEnumerable<Associado> RecuperaAssociadosPorNome(string nome)
        {
            return _associadoService.RecuperaAssociadosPorNome(nome);
        }


        #endregion
    }
}
