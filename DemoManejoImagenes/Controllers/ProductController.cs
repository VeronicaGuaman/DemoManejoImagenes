using DemoManejoImagenes.Models;
using DemoManejoImagenes.Models.Data;
using DemoManejoImagenes.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DemoManejoImagenes.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductBlobConfiguration _productBlobConfiguration;
        private readonly DemoContext _context;

        public ProductController(IProductBlobConfiguration productBlobConfiguration, DemoContext context)
        {
            _productBlobConfiguration = productBlobConfiguration;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<Product> productModel = await GetProducts();
            return View(productModel);
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            //Validar el método
            if (ModelState.IsValid)
            {
                try
                {
                    //Verificar si se ha agregado una imagen
                    if (model.ImageFile != null)
                    {
                        //Guardamos la imagen en el contenedor de Blob Storage
                        await _productBlobConfiguration.UploadBlobAsync(model.ImageFile, "products");
                        model.UrlImage = model.ImageFile.FileName;
                    }

                    //Guardar la información del producto en base de datos
                    _context.Products.Add(model);
                    await _context.SaveChangesAsync();

                    //Una vez guardada la información se redirige a la lista de productos
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(model);
        }

        public IActionResult Delete(int id)
        {
            //Recuperamos la información del producto 
            var product = _context.Products.FirstOrDefault(p => p.Id == id);

            //Verificamos si existe el producto en base de datos
            if (product is null)
                return NotFound();
            try
            {
                //Eliminamos la imágen de Azure Blob Storage
                _productBlobConfiguration.DeleteBlob(product.UrlImage, "products");
                //Eliminar el producto de la base de datos
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
