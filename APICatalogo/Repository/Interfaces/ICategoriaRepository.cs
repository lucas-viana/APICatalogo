using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repository.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        Task<IPagedList<Categoria>> GetCategoriaAsync(CategoriaParameters produtosParameters);
        Task<IPagedList<Categoria>> GetCategoriaPorNomeAsync(CategoriasFiltroNome categoriasFiltroNome);
    }
}
