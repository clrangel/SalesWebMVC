using SalesWebMvc.Models;

namespace SalesWebMvc.Services
{
    public class SellerService
    {
        private readonly SalesWebMvcContext _context;

        public SellerService(SalesWebMvcContext context) => _context = context;

        //Método buscar todos
        public List<Seller> FindAll()
        {
            return _context.Seller.ToList();
        }
        
        //Método Inserir
        public void Insert(Seller obj)
        {
            _context.Add(obj);
            _context.SaveChanges();
        }
    }
}
