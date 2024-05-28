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

        public PagedList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            var produtos = _context.Produtos.OrderBy(p => p.ProdutoId).AsQueryable();
            var produtosOrdenados = PagedList<Produto>.ToPagedList(produtos, produtosParameters.PageNumber, produtosParameters.PageSize);
            return produtosOrdenados;
        }

        public PagedList<Produto> GetProdutosFiltroPreco(ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = GetAll().AsQueryable();

            if (produtosFiltroPreco.Preco.HasValue)
            {
                if (produtosFiltroPreco.Criterio == Criterio.maior)
                {
                    produtos = produtos.Where(p => p.Preco > produtosFiltroPreco.Preco).OrderBy(p => p.Preco);
                }
                else if (produtosFiltroPreco.Criterio == Criterio.menor)
                {
                    produtos = produtos.Where(p => p.Preco < produtosFiltroPreco.Preco).OrderBy(p => p.Preco);
                }
                if (produtosFiltroPreco.Criterio == Criterio.igual)
                {
                    produtos = produtos.Where(p => p.Preco == produtosFiltroPreco.Preco).OrderBy(p => p.Preco);

                }
            }
            var listaProdutosFiltrada = PagedList<Produto>.ToPagedList(produtos, produtosFiltroPreco.PageNumber, produtosFiltroPreco.PageSize);
            return listaProdutosFiltrada;
        }

        public IEnumerable<Produto> GetProdutosPorCategoria(int id)
        {
            return GetAll().Where(c => c.CategoriaId == id);
        }
    }
}
