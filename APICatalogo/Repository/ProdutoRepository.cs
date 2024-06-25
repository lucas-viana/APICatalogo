using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using X.PagedList;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {

        public ProdutoRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await GetAllAsync();
            var listaProdutosOrdenada = produtos.OrderBy(p => p.ProdutoId);
            var produtosOrdenados = await produtos.ToPagedListAsync(produtosParameters.PageNumber, produtosParameters.PageSize);
            return produtosOrdenados;
        }

        public async Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroPreco)
        {
            var produtos = await GetAllAsync();

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
            var listaProdutosFiltrada = await produtos.ToPagedListAsync(produtosFiltroPreco.PageNumber, produtosFiltroPreco.PageSize);
            return listaProdutosFiltrada;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var produtos = await GetAllAsync();
            var lista = produtos.Where(c => c.CategoriaId == id);
            return lista;
        }
    }
}
