using FluentResults;
using Microsoft.AspNetCore.Mvc;
using S4E.ADO.Models;
using S4E.ADO.Models.Dto.AssociadoDto;
using S4E.ADO.Services;

namespace S4E.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssociadoController : Controller
    {
        private AssociadoService _associadoService;
        public AssociadoController(AssociadoService associadoService)
        {
            _associadoService = associadoService;
        }

        [HttpGet("id/{id}")]
        public IActionResult RecuperaAssociadoPorId(int id)
        {
            try
            {
                Associado associado = _associadoService.RecuperaAssociadoPorId(id);
                if (associado == null)
                {
                    return NotFound();
                }
                return Ok(associado);
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpPost()]
        public IActionResult AdicionaAssociado([FromBody] CreateAssociadoDto associadoDto)
        {
            try
            {
                Associado associado = _associadoService.AdicionaAssociado(associadoDto);
                return CreatedAtAction(nameof(RecuperaAssociadoPorId), new { id = associado.Id }, associado);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet]
        public IEnumerable<Associado> RecuperaAssociados()
        {
            return _associadoService.RecuperaAssociados();
        }
        [HttpGet("nome/{nome}")]
        public IActionResult RecuperassociadosPorNome(string nome)
        {
            try
            {
                return Ok(_associadoService.RecuperaAssociadosPorNome(nome));
            }
            catch (ArgumentNullException ex)
            {
                string mensagemDeErro = ex.Message.Remove(ex.Message.IndexOf("("));
                return BadRequest(mensagemDeErro);
            }

        }
        [HttpGet("cpf/{cpf}")]
        public IActionResult RecuperaAssociadoporCpf(string cpf)
        {
            try
            {
                Associado associado = _associadoService.RecuperaAssociadoPorCPF(cpf);
                if (associado == null)
                {
                    return new NotFoundObjectResult(associado);
                }
                return new OkObjectResult(associado);
            }
            catch (ArgumentNullException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }
        [HttpPost("{id}")]
        public IActionResult AtualizaAssociado(int id, [FromBody] CreateAssociadoDto associadoDto)
        {
            try
            {
                Result resultado = _associadoService.AtualizaAssociado(id, associadoDto);
                if (resultado.IsFailed)
                {
                    return new NotFoundResult();
                }
                return new NoContentResult();
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }
        [HttpDelete("id")]
        public IActionResult DeletaAssociado(int id)
        {
            try
            {
                Result resultado = _associadoService.DeletaAssociado(id);
                if (resultado.IsFailed)
                {
                    return new NotFoundResult();
                }
                return new NoContentResult();
            }
            catch (ArgumentNullException ex)
            {

                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }

        }
    }
}
