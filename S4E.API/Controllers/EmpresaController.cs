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
            try
            {
                Empresa empresa = _empresaService.RecuperaEmpresaPorId(id);
                if (empresa == null)
                {
                    return NotFound();
                }
                return Ok(empresa);
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
        public IActionResult AdicionaEmpresa([FromBody] CreateEmpresaDto empresaDto)
        {
            try
            {
                Empresa empresa = _empresaService.AdicionaEmpresa(empresaDto);
                return CreatedAtAction(nameof(RecuperaEmpresaPorId), new { id = empresa.Id }, empresa);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        public IActionResult RecuperaEmpresas()
        {
            try
            {
                return Ok(_empresaService.RecuperaEmpresas());
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
        [HttpGet("nome/{nome}")]
        public IActionResult RecuperempresasPorNome(string nome)
        {
            try
            {
                return Ok(_empresaService.RecuperaEmpresasPorNome(nome));
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
        [HttpGet("cnpj/{cnpj}")]
        public IActionResult RecuperaEmpresaporCpf(string cnpj)
        {
            try
            {
                Empresa empresa = _empresaService.RecuperaEmpresaPorCnpj(cnpj);
                if (empresa == null)
                {
                    return new NotFoundObjectResult(empresa);
                }
                return new OkObjectResult(empresa);
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
        public IActionResult AtualizaEmpresa(int id, [FromBody] CreateEmpresaDto empresaDto)
        {
            try
            {
                Result resultado = _empresaService.AtualizaEmpresa(id, empresaDto);
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
        public IActionResult DeletaEmpresa(int id)
        {
            try
            {
                Result resultado = _empresaService.DeletaEmpresa(id);
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
