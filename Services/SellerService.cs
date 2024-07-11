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

        //Deltar Vendedor
        //Busca Vendedor
        //public Seller FindById(int id) => _context.Seller.FirstOrDefault(obj => obj.Id == id);
        public Seller FindById(int id)
        {
            return _context.Seller.FirstOrDefault(obj => obj.Id == id);
        }
        //Deleta Vendedor
        public void Remove(int id) 
        {
            //_context.Remove(id);
            var obj = _context.Seller.Find(id);
            _context.Seller.Remove(obj);
            _context.SaveChanges();
        }
    }
}
