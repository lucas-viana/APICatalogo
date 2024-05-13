namespace APICatalogo.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        public IProdutoRepository ProdutoRepository {  get; }
        public ICategoriaRepository CategoriaRepository {  get; }
        public void Commit();
    }
}
