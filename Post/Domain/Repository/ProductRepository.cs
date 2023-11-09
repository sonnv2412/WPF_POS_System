using Microsoft.EntityFrameworkCore;
using Post.Domain.Repository.IRepository;
using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Repository
{
    class ProductRepository : IProductRepository
    {
        private PostContext _context;
        public ProductRepository(PostContext context) {
            _context= context;
        }
        public Product GetProductById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.Include(p => p.Category).Include(p => p.Unit);
        }
    }
}
