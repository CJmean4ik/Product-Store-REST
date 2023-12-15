using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using UsersRestApi.Database.EF.UpdateComponents.Arguments;
using UsersRestApi.Database.Entities;
using UsersRestApi.Models;

namespace UsersRestApi.Database.EF.UpdateComponents
{
    public class ProductModifierArgumentChanger : IProductModifierArgumentChanger
    {
        public required Dictionary<Func<ProductEntity, ProductEntity, bool>, ModifierArgumentsBase<ProductEntity, DatabaseContext>> Tracker { get; set; }
        private ImageConfig _imageConfig;
        public ProductModifierArgumentChanger(IOptions<ImageConfig> imageConfig)
        {
            _imageConfig = imageConfig.Value;
            InitializeTracker();
        }

        public void InitializeTracker()
        {
            Tracker = new()
            {
                [(oldProduct, newProduct) => newProduct.Name != "" && oldProduct.Name != newProduct.Name] =
                new ProductModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldProduct, newProduct) => 
                    {
                        oldProduct.Name = newProduct.Name;
                        string oldPath = _imageConfig.ProductPath.Replace("FOR_RAPLACE", oldProduct.Name);
                        string newPath = _imageConfig.ProductPath.Replace("FOR_RAPLACE", newProduct.Name);
                        FileSystem.Rename(oldPath,newPath);
                    },
                    Attacher = (oldProduct, db) => db.Entry(oldProduct).Property(p => p.Name).IsModified = true
                },

                [(oldProduct, newProduct) => newProduct.Description != "" && oldProduct.Description != newProduct.Description] =
                new ProductModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldProduct, newProduct) => oldProduct.Description = newProduct.Description,
                    Attacher = (oldProduct, db) => db.Entry(oldProduct).Property(p => p.Description).IsModified = true
                },

                [(oldProduct, newProduct) => newProduct.Price != 0 && oldProduct.Price != newProduct.Price] =
                new ProductModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldProduct, newProduct) => oldProduct.Price = newProduct.Price,
                    Attacher = (oldProduct, db) => db.Entry(oldProduct).Property(p => p.Price).IsModified = true
                },

                [(oldProduct, newProduct) => newProduct.CountOnStorage != 0 && oldProduct.CountOnStorage != newProduct.CountOnStorage] =
                new ProductModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldProduct, newProduct) => oldProduct.CountOnStorage = newProduct.CountOnStorage,
                    Attacher = (oldProduct, db) => db.Entry(oldProduct).Property(p => p.CountOnStorage).IsModified = true
                },
                [(oldProduct, newProduct) => newProduct.PreviewImageName != "" && oldProduct.PreviewImageName != newProduct.PreviewImageName] =
                new ProductModifierArguments
                {
                    IsModified = false,
                    ValueChanger = (oldProduct, newProduct) => oldProduct.PreviewImageName = newProduct.PreviewImageName,
                    Attacher = (oldProduct, db) => db.Entry(oldProduct).Property(p => p.PreviewImageName).IsModified = true
                },       
            };
        }

        public void SearchModifieArguments(ProductEntity oldEntity, ProductEntity newEntity)
        {

            foreach (var track in Tracker)
            {
                if (track.Key.Invoke(oldEntity, newEntity))
                {
                    track.Value.ValueChanger.Invoke(oldEntity, newEntity);
                    track.Value.IsModified = true;
                }
            }
        }
        public void ChangeFoundModifieArguments(ProductEntity oldEntity, DatabaseContext db)
        {
            foreach (var track in Tracker)
            {
                if (track.Value.IsModified)
                {
                    track.Value.Attacher.Invoke(oldEntity, db);
                }
            }
        }

        public async Task SaveChangesAsync(DatabaseContext context)
        {
            await context.SaveChangesAsync();
        }             
    }
}
