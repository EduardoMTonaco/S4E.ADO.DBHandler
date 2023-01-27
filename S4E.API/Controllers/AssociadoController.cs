using AutoMapper;
using FluentResults;
using Microsoft.AspNetCore.Mvc;
using S4E.ADO.Models;
using S4E.ADO.Models.Dto.AssociadoDto;
using S4E.ADO.Services;
using System.Text.Json;

namespace S4E.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssociadoController : Controller
    {
        private AssociadoService _associadoService;
        public AssociadoController(AssociadoService associadoService ) 
        {
            _associadoService = associadoService;
        }

        [HttpGet("id/{id}")]
        public IActionResult RecuperaAssociadoPorId(int id)
        {
            Associado associado = _associadoService.RecuperaAssociadoPorId(id);
            if(associado == null) 
            {
                return NotFound();
            }
            return Ok(associado);

        }
        [HttpPost()]
        public IActionResult AdicionaAssociado([FromBody] CreateAssociadoDto associadoDto)
        {
            Associado associado = _associadoService.AdicionaAssociado(associadoDto);
                return CreatedAtAction(nameof(RecuperaAssociadoPorId), new {id = associado.id }, associado);
        }
        [HttpGet]
        public IEnumerable<Associado> RecuperaAssociados()
        {
            return _associadoService.RecuperaAssociados();
        }
        [HttpGet("nome/{nome}")]
        public IEnumerable<Associado> RecuperassociadosPorNome(string nome)
        {
            return _associadoService.RecuperaAssociadosPorNome(nome);
        }
        [HttpGet("cpf/{cpf}")]
        public IActionResult RecuperaAssociadoporCpf(string cpf)
        {
            Associado associado = _associadoService.RecuperaAssociadoPorCPF(cpf);
            if(associado== null) 
            {
                return new NotFoundObjectResult(associado);
            }
            return new OkObjectResult(associado);
        }
        [HttpPost("{id}")]
        public IActionResult AtualizaAssociado(int id, [FromBody]CreateAssociadoDto associadoDto)
        {
            Result resultado = _associadoService.AtualizaAssociado(id, associadoDto);
            if (resultado.IsFailed) 
            {
                return new NotFoundResult();
            }
            return new NoContentResult();
        }
        [HttpDelete("id")]
        public IActionResult DeletaAssociado(int id)
        {
            Result resultado = _associadoService.DeletaAssociado(id);
            if (resultado.IsFailed)
            {
                return new NotFoundResult();
            }
            return new NoContentResult();
        }
    }
}
