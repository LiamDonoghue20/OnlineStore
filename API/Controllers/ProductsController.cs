using API.Dtos;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using API.Errors;

namespace API.Controllers
{

    public class ProductsController : BaseApiController
    {
        //using the repo to manipulate data rather than directly to the controller for more abstraction 
   
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandsRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        //need 3 instances of the generic repository needed for each different entity (product, type and brand)
        public ProductsController(IGenericRepository<Product> productsRepo,
             IGenericRepository<ProductBrand> productBrandsRepo, 
             IGenericRepository<ProductType> productTypeRepo,
             IMapper mapper)
        {
            _productTypeRepo = productTypeRepo;
            _mapper = mapper;
            _productBrandsRepo = productBrandsRepo;
            _productsRepo = productsRepo;
         
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            //map each product returned to the product to return dto and then map it to a list
            return Ok(_mapper
            .Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]


        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            //create new instance of product with brands/types spec and pass the ID
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            //use the specification that is returned to call the get entity method with the specification
           var product = await _productsRepo.GetEntityWithSpec(spec);

           if (product == null) return NotFound(new ApiResponse(404));

           return _mapper.Map<Product, ProductToReturnDto>(product);
          
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _productBrandsRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await _productTypeRepo.ListAllAsync());
        }
    }
}