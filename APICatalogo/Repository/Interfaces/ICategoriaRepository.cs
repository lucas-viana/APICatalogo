using APICatalogo.Models;
using APICatalogo.Pagination;

namespace APICatalogo.Repository.Interfaces
{
    public interface ICategoriaRepository : IRepository<Categoria>
    {
        PagedList<Categoria> GetCategoria(CategoriaParameters produtosParameters);
    }
}
