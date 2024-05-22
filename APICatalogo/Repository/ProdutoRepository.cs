using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext context) : base(context) 
        {
        }

        public IEnumerable<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            var produtos = _context.Produtos.OrderBy(on => on.Nome).Skip((produtosParameters.PageNumber - 1) *produtosParameters.PageSize ).Take(produtosParameters.PageSize).ToList();
            return produtos;
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
