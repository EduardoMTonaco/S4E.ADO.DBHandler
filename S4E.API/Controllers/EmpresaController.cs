using FluentResults;
using Microsoft.AspNetCore.Mvc;
using S4E.ADO.Models;
using S4E.ADO.Models.Dto.EmpresaDto;
using S4E.ADO.Services;

namespace S4E.ADO.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmpresaController : Controller
    {
        private EmpresaService _empresaService;
        public EmpresaController(EmpresaService empresaService)
        {
            _empresaService = empresaService;
        }
        [HttpGet("id/{id}")]
        public IActionResult RecuperaEmpresaPorId(int id)
        {
            Empresa empresa = _empresaService.RecuperaEmpresaPorId(id);
            if (empresa == null)
            {
                return NotFound();
            }
            return Ok(empresa);
        }
        [HttpPost()]
        public IActionResult AdicionaEmpresa([FromBody] CreateEmpresaDto empresaDto)
        {
            Empresa empresa = _empresaService.AdicionaEmpresa(empresaDto);
            return CreatedAtAction(nameof(RecuperaEmpresaPorId), new {id = empresa.Id }, empresa);
        }
        [HttpGet]
        public IEnumerable<Empresa> RecuperaEmpresas()
        {
            return _empresaService.RecuperaEmpresas();
        }
        [HttpGet("nome/{nome}")]
        public IEnumerable<Empresa> RecuperempresasPorNome(string nome)
        {
            return _empresaService.RecuperaEmpresasPorNome(nome);
        }
        [HttpGet("cnpj/{cnpj}")]
        public IActionResult RecuperaEmpresaporCpf(string cnpj)
        {
            Empresa empresa = _empresaService.RecuperaEmpresaPorCnpj(cnpj);
            if (empresa == null)
            {
                return new NotFoundObjectResult(empresa);
            }
            return new OkObjectResult(empresa);
        }
        [HttpPost("{id}")]
        public IActionResult AtualizaEmpresa(int id, [FromBody] CreateEmpresaDto empresaDto)
        {
            Result resultado = _empresaService.AtualizaEmpresa(id, empresaDto);
            if (resultado.IsFailed)
            {
                return new NotFoundResult();
            }
            return new NoContentResult();
        }
        [HttpDelete("id")]
        public IActionResult DeletaEmpresa(int id)
        {
            Result resultado = _empresaService.DeletaEmpresa(id);
            if (resultado.IsFailed)
            {
                return new NotFoundResult();
            }
            return new NoContentResult();
        }
    }
}
