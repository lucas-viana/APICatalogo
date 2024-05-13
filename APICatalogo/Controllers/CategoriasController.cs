using APICatalogo.Models;
using APICatalogo.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoriasController> _logger;

        public CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categoria>> GetCategorias()
        {
            var lista = _unitOfWork.CategoriaRepository.GetAll();
            if (lista is null)
            {
                return NotFound();
            }
            return Ok(lista);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<Produto> Get(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrado...");
                return NotFound($"Categoria com id = {id} não encontrado...");
            }
            return Ok(categoria);
        }

        [HttpPost]
        public ActionResult Post(Categoria categoria)
        {
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            _unitOfWork.CategoriaRepository.Create(categoria);
            _unitOfWork.Commit();
            return new CreatedAtRouteResult("ObterCategoria", new { id = categoria.CategoriaId }, categoria);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();

            return Ok(categoria);
        }

        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var categoriaExcluida = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            _unitOfWork.Commit();
            return Ok(categoriaExcluida);
        }
    }
}