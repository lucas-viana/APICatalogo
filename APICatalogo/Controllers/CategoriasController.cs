using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using X.PagedList;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriasController(IUnitOfWork unitOfWork, ILogger<CategoriasController> logger, IMapper mapper) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<CategoriasController> _logger = logger;
        private readonly IMapper _mapper = mapper;

        [HttpGet("pagination")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> Get([FromQuery] CategoriaParameters categoriaParameters)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriaAsync(categoriaParameters);

            if (categorias is null)
            {
                return NotFound();
            }

            return ObterCategoria(categorias);
        }

        [HttpGet("filter/nome/categoria")]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategoriasPorNome([FromQuery] CategoriasFiltroNome categoriaParameters)
        {
            var categorias = await _unitOfWork.CategoriaRepository.GetCategoriaPorNomeAsync(categoriaParameters);

            if(categorias is null)
            {
                return NotFound("Não existem categorias com este nome...");
            }
            return ObterCategoria(categorias);
        }
        [Authorize]
        private ActionResult<IEnumerable<CategoriaDTO>> ObterCategoria(IPagedList<Categoria> categorias)
        {
            var metadata = new
            {
                categorias.Count,
                categorias.PageSize,
                categorias.PageCount,
                categorias.TotalItemCount,
                categorias.HasNextPage,
                categorias.HasPreviousPage
            };

            var categoriaDto = _mapper.Map<IEnumerable<CategoriaDTO>>(categorias);

            Response.Headers.Append("X-Pagination", JsonConvert.SerializeObject(metadata));
            return Ok(categoriaDto);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<CategoriaDTO>>> GetCategorias()
        {
            var lista = await _unitOfWork.CategoriaRepository.GetAllAsync();
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
        public async Task<ActionResult<CategoriaDTO>> Post(CategoriaDTO categoriaDto)
        {
            if (categoriaDto is null)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);

            _unitOfWork.CategoriaRepository.Create(categoria);
            await _unitOfWork.CommitAsync();

            var novaCategoriaDto = _mapper.Map<CategoriaDTO>(categoria);
            return new CreatedAtRouteResult("ObterCategoria", new { id = novaCategoriaDto.CategoriaId }, novaCategoriaDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Put(int id, CategoriaDTO categoriaDto)
        {
            if (id != categoriaDto.CategoriaId)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var categoria = _mapper.Map<Categoria>(categoriaDto);
            _unitOfWork.CategoriaRepository.Update(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaDtoAtualizada = _mapper.Map<CategoriaDTO>(categoria);
            return Ok(categoriaDtoAtualizada);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Delete(int id)
        {
            var categoria = await _unitOfWork.CategoriaRepository.Get(c => c.CategoriaId == id);
            if (categoria is null)
            {
                _logger.LogWarning("Dados inválidos");
                return BadRequest("Dados inválidos");
            }
            var categoriaExcluida = _unitOfWork.CategoriaRepository.Delete(categoria);
            await _unitOfWork.CommitAsync();

            var categoriaExcluidaDto = _mapper.Map<CategoriaDTO>(categoriaExcluida);
            return Ok(categoriaExcluidaDto);
        }
    }
}