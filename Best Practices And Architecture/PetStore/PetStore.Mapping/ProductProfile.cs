using AutoMapper;
using PetStore.Models;
using PetStore.Models.Enumerations;
using PetStore.ServiceModels.Products.InputModels;
using PetStore.ServiceModels.Products.OutputModels;
using PetStore.ViewModels.Product.InputModels;
using System.Security.Cryptography.X509Certificates;

namespace PetStore.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            this.CreateMap<AddProductInputServiceModel, Product>()
             .ForMember(x => x.ProductType, y => y.MapFrom(x => Enum.Parse(typeof(ProductType), x.ProductType)));
            this.CreateMap<Product, ListAllProductByProductTypeServiceModel>();
            this.CreateMap<Product, ListAllProductsServiceModel>()
                .ForMember(x => x.ProductType, y => y.MapFrom(x => x.ProductType.ToString()));
            this.CreateMap<Product, ListAllProductsByNameServiceModel>()
                .ForMember(x => x.ProductType, y => y.MapFrom(x => x.ProductType.ToString()));
            this.CreateMap<EditProductInputServiceModel, Product>()
                .ForMember(x => x.ProductType, y => y.MapFrom(x => Enum.Parse(typeof(ProductType), x.ProductType)));
        }
    }
}
