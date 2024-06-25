using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using X.PagedList;

namespace APICatalogo.Repository
{
    public class CategoriaRepository(AppDbContext context) : Repository<Categoria>(context), ICategoriaRepository
    {
        public async Task<IPagedList<Categoria>> GetCategoriaAsync(CategoriaParameters categoriasParameters)
        {
            var categorias = await _context.Categorias.OrderBy(c => c.CategoriaId).ToListAsync();
            var categoriasOrdenados = await categorias.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);
            return categoriasOrdenados;
        }

        public async Task<IPagedList<Categoria>> GetCategoriaPorNomeAsync(CategoriasFiltroNome categoriasFiltroNome)
        {
            var categorias = await GetAllAsync();
            if (!string.IsNullOrEmpty(categoriasFiltroNome.Nome))
            {
                categorias = categorias.Where(c => c.Nome.ToUpper().Contains(categoriasFiltroNome.Nome.ToUpper()));
            }
            return await categorias.ToPagedListAsync(categoriasFiltroNome.PageNumber, categoriasFiltroNome.PageSize);
        }
    }
}
