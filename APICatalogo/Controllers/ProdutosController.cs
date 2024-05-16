using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProdutosController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ProdutoDTO>> Get()
        {
            var produtos = _unitOfWork.ProdutoRepository.GetAll().ToList();
            if (produtos is null)
            {
                return NotFound("Produtos não encontrados...");
            }
            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);
            return Ok(produtosDto);
        }

        [HttpGet("categoria/{id}")]
        public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosPorCategoria(int id)
        {
            var produtos = _unitOfWork.ProdutoRepository.GetProdutosPorCategoria(id);

            if (produtos is null || !produtos.Any())
            {
                return NotFound("Não existem produtos nesta categoria ou a categoria informada é inexistente.");
            }

            var produtosDto = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

            return Ok(produtosDto);
        }

        [HttpGet("{id:int}", Name = "ObterProduto")]

        public ActionResult<ProdutoDTO> Get(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);
            if (produto is null)
            {
                NotFound("Produto não encontrado");
            }
            var produtoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoDto);
        }

        [HttpPost]
        public ActionResult Post(ProdutoDTO produtoDto)
        {
            if (produtoDto == null)
            {
                return BadRequest("Produto inválido");
            }
            var produto = _mapper.Map<Produto>(produtoDto);

            var novoProduto = _unitOfWork.ProdutoRepository.Create(produto);
            _unitOfWork.Commit();

            var novoprodutoDto = _mapper.Map<ProdutoDTO>(novoProduto);
            return new CreatedAtRouteResult("ObterProduto", new { id = novoprodutoDto.ProdutoId }, novoprodutoDto);
        }

        [HttpPut("{id:int}")]
        public ActionResult Put(int id, ProdutoDTO produtoDto)
        {
            if (id != produtoDto.ProdutoId)
            {
                return BadRequest();
            }
            var produto = _mapper.Map<Produto>(produtoDto);
            var produtoAtualizado = _unitOfWork.ProdutoRepository.Update(produto);
            _unitOfWork.Commit();

            var produtoAtualizadoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoAtualizadoDto);
        }

        [HttpDelete("{id:int}")]
        public ActionResult<ProdutoDTO> Delete(int id)
        {
            var produto = _unitOfWork.ProdutoRepository.Get(p => p.ProdutoId == id);

            if (produto is null)
            {
                return NotFound("Produto não encontrado...");
            }

            _unitOfWork.ProdutoRepository.Delete(produto);
            _unitOfWork.Commit();

            var produtoExcluidoDto = _mapper.Map<ProdutoDTO>(produto);
            return Ok(produtoExcluidoDto);
        }
    }
}