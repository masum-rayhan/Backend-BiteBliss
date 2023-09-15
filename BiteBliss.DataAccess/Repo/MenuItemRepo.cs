using BiteBliss.DataAcces.Data;
using BiteBliss.DataAcces.Repo.IRepo;
using BiteBliss.DataAccess.Repo;
using BiteBliss.Models.DataTables;
using BiteBliss.Models.DataTables.Dto;
using BiteBliss.DataAcces.Repo.Services;
using BiteBliss.DataAcces.Utils;

namespace BiteBliss.DataAcces.Repo;

public class MenuItemRepo : Repository<MenuItem>, IMenuItemRepo
{
    private readonly AppDbContext _db;
    private readonly BlobService _blobService;
    private readonly ICacheService cacheService;
    public MenuItemRepo(AppDbContext db, BlobService blobService) : base(db)
    {
        _db = db;
        _blobService = blobService;
    }

    public async Task<MenuItem> CreateMenuItemAsync(MenuItemCreateDTO menuItemCreateDTO)
    {
        try
        {
            if (menuItemCreateDTO.File == null || menuItemCreateDTO.File.Length == 0)
            {
                throw new ArgumentException("Image is required");
            }

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

            return menuItemToCreate;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<MenuItem> UpdateMenuItemAsync(int id, MenuItemUpdateDTO menuItemUpdateDTO)
    {
        try
        {
            if (menuItemUpdateDTO == null || id != menuItemUpdateDTO.Id)
            {
                throw new ArgumentException("Invalid data");
            }

            MenuItem menuItemToUpdate = await _db.MenuItems.FindAsync(id);

            //menuItemToUpdate.Id = menuItemUpdateDTO.Id;
            menuItemToUpdate.Name = menuItemUpdateDTO.Name;
            menuItemToUpdate.Price = menuItemUpdateDTO.Price;
            menuItemToUpdate.Category = menuItemUpdateDTO.Category;
            menuItemToUpdate.SpecialTag = menuItemUpdateDTO.SepcialTag;
            menuItemToUpdate.Description = menuItemUpdateDTO.Description;

            if(menuItemUpdateDTO.File != null && menuItemUpdateDTO.File.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(menuItemUpdateDTO.File.FileName)}";

                await _blobService.DeleteBlob(menuItemToUpdate.Image.Split('/').Last(), SD.SD_Storage_Container);

                menuItemToUpdate.Image = await _blobService.UploadBlob(fileName, SD.SD_Storage_Container, menuItemUpdateDTO.File);
            }

            _db.MenuItems.Update(menuItemToUpdate);
            _db.SaveChanges();

            return menuItemToUpdate;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public async Task<bool> DeleteMenuItemAsync(int id)
    {
        try
        {
            if(id == 0)
                throw new ArgumentException("Invalid data");

            MenuItem menuItem = await _db.MenuItems.FindAsync(id);

            await _blobService.DeleteBlob(menuItem.Image.Split('/').Last(), SD.SD_Storage_Container);

            _db.MenuItems.Remove(menuItem);
            await _db.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
