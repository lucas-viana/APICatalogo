using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Linq;

namespace APICatalogo.Repository.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        IEnumerable<Produto> GetProdutosPorCategoria(int id);
        PagedList<Produto> GetProdutos (ProdutosParameters produtosParameters);
    }
}
