using Post.Domain.Repository.IRepository;
using Post.Domain.Service.IService;
using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Service
{
    class ProductService : IProductService
    {
        IProductRepository productRepository;
        public ProductService(IProductRepository productRepository) { 
            this.productRepository = productRepository;
        }
        public List<Product> getAllProducts()
        {
            return productRepository.GetProducts().ToList();
        }
    }
}
