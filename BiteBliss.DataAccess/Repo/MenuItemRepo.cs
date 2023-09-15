using BiteBliss.DataAcces.Data;
using BiteBliss.DataAcces.Repo.IRepo;
using BiteBliss.DataAccess.Repo;
using BiteBliss.Models.DataTables;
using BiteBliss.Models.DataTables.Dto;
using BiteBliss.DataAcces.Repo.Services;
using BiteBliss.DataAcces.Utils;
using BiteBliss.DataAccess.Repo.IRepo.Services;
using BiteBliss.DataAccess.Repo.Services;
using BiteBliss.DataAcces.Repo.IRepo.Services;

namespace BiteBliss.DataAcces.Repo;

public class MenuItemRepo : Repository<MenuItem>, IMenuItemRepo
{
    private readonly AppDbContext _db;
    private readonly IBlobService _blobService;
    private readonly ICacheService _cacheService;
    public MenuItemRepo(AppDbContext db, IBlobService blobService, ICacheService cacheService) : base(db, cacheService)
    {
        _db = db;
        _blobService = blobService;
        _cacheService = cacheService;
    }

    public async Task<MenuItem> CreateMenuItemAsync(MenuItemCreateDTO menuItemCreateDTO)
    {
        try
        {
            if (menuItemCreateDTO.File == null || menuItemCreateDTO.File.Length == 0)
                throw new ArgumentException("Image is required");

            string cacheKey = $"MenuItem_{menuItemCreateDTO.Name}";

            var cacheData = await _cacheService.GetDataAsync<MenuItem>(cacheKey);

            if(cacheData != null)
                return cacheData;

            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemCreateDTO.File.FileName)}";

            MenuItem menuItemToCreate = new()
            {
                Name = menuItemCreateDTO.Name,
                Price = menuItemCreateDTO.Price,
                Category = menuItemCreateDTO.Category,
                SpecialTag = menuItemCreateDTO.SepcialTag,
                Description = menuItemCreateDTO.Description,
                Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, menuItemCreateDTO.File)
            };

            _db.MenuItems.Add(menuItemToCreate);
            await _db.SaveChangesAsync();

            await _cacheService.SetDataAsync<MenuItem>(cacheKey, menuItemToCreate, DateTimeOffset.Now.AddSeconds(30));

            return menuItemToCreate;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to create MenuItem.", ex);
        }
    }

    public async Task<MenuItem> UpdateMenuItemAsync(int id, MenuItemUpdateDTO menuItemUpdateDTO)
    {
        try
        {
            if (menuItemUpdateDTO == null || id != menuItemUpdateDTO.Id)
                throw new ArgumentException("Invalid data");

            string cacheKey = $"MenuItem_{menuItemUpdateDTO.Name}";

            var cacheData = await _cacheService.GetDataAsync<MenuItem>(cacheKey);

            if(cacheData != null)
                return cacheData;

            MenuItem menuItemToUpdate = await _db.MenuItems.FindAsync(id);

            //menuItemToUpdate.Id = menuItemUpdateDTO.Id;
            menuItemToUpdate.Name = menuItemUpdateDTO.Name;
            menuItemToUpdate.Price = menuItemUpdateDTO.Price;
            menuItemToUpdate.Category = menuItemUpdateDTO.Category;
            menuItemToUpdate.SpecialTag = menuItemUpdateDTO.SepcialTag;
            menuItemToUpdate.Description = menuItemUpdateDTO.Description;

            if (menuItemUpdateDTO.File != null && menuItemUpdateDTO.File.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemUpdateDTO.File.FileName)}";

                await _blobService.DeleteBlob(menuItemToUpdate.Image.Split('/').Last(), SD.SD_Storage_Container);

                menuItemToUpdate.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, menuItemUpdateDTO.File);
            }

            _db.MenuItems.Update(menuItemToUpdate);
            _db.SaveChanges();

            await _cacheService.SetDataAsync<MenuItem>(cacheKey, menuItemToUpdate, DateTimeOffset.Now.AddSeconds(30));

            return menuItemToUpdate;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to update MenuItem.", ex);
        }
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        try
        {
            if (id == 0)
                throw new ArgumentException("Invalid data");

            string cacheKey = $"MenuItem_{id}";

            var cacheData = await _cacheService.GetDataAsync<MenuItem>(cacheKey);

            if (cacheData != null)
                await _cacheService.RemoveDataAsync(cacheKey);

            MenuItem menuItemToDelete = await _db.MenuItems.FindAsync(id);

            await _blobService.DeleteBlob(menuItemToDelete.Image.Split('/').Last(), SD.SD_Storage_Container);

            _db.MenuItems.Remove(menuItemToDelete);
            await _db.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Failed to delete MenuItem.", ex);
        }
    }
}
