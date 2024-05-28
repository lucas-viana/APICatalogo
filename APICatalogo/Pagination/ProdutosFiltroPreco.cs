namespace APICatalogo.Pagination
{
    public class ProdutosFiltroPreco : QueryStringParameters
    {
        public decimal? Preco { get; set; }
        public Criterio Criterio {  get; set; }
    }
}
