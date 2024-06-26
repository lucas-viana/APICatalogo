﻿using APICatalogo.Context;
using APICatalogo.Repository.Interfaces;

namespace APICatalogo.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private IProdutoRepository _produtoRepository;

        private ICategoriaRepository _categoriaRepository;

        public AppDbContext _context;


        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _produtoRepository = _produtoRepository ?? new ProdutoRepository(_context);
            }
        }
        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _categoriaRepository = _categoriaRepository ?? new CategoriaRepository(_context);
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
