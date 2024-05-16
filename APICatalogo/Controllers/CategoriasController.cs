using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoriasController> _logger;
        private readonly IMapper _mapper;

        public CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CategoriaDTO>> GetCategorias()
        {
            var lista = _unitOfWork.CategoriaRepository.GetAll();
            if (lista is null)
            {
                return NotFound();
            }
            var categorias = _mapper.Map<IEnumerable<CategoriaDTO>>(lista);
            return Ok(categorias);
        }

        [HttpGet("{id:int}", Name = "ObterCategoria")]
        public ActionResult<CategoriaDTO> Get(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning($"Categoria com id = {id} não encontrado...");
                return NotFound($"Categoria com id = {id} não encontrado...");
            }

            var categoriaDto = _mapper.Map<CategoriaDTO>(categoria);

            return Ok(categoriaDto);
        }

        [HttpPost]
        public ActionResult<CategoriaDTO> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _unitOfWork.CategoriaRepository.Create(categoria);
            _unitOfWork.Commit();

            var novaCategoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult<CategoriaDTO> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _unitOfWork.CategoriaRepository.Update(categoria);
            _unitOfWork.Commit();

            var categoriaDtoAtualizada = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDtoAtualizada);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<CategoriaDTO> Delete(int id)
        {
            var categoria = _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
            _unitOfWork.Commit();

            var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);
            return Ok(categoriaExcluidaDto);
        }
    }
}