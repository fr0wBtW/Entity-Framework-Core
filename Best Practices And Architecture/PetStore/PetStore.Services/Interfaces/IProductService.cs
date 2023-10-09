using PetStore.ServiceModels.Products.InputModels;
using PetStore.ServiceModels.Products.OutputModels;

namespace PetStore.Services.Interfaces
{
    public interface IProductService
    {
        void AddProduct(AddProductInputServiceModel model);

        ICollection<ListAllProductsServiceModel> GetAll();

        ICollection<ListAllProductByProductTypeServiceModel> ListAllByProductType(string type);

        ICollection<ListAllProductsByNameServiceModel> SearchByName(string searchStr, bool caseSensitive);

        bool RemoveById(string id);
        
        bool RemoveByName(string name);

        void EditProduct(string id, EditProductInputServiceModel model);
    }
}
