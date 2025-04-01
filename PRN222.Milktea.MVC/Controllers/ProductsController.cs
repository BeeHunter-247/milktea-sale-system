using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PRN222.Milktea.Repository.Models;
using PRN222.Milktea.Service.BusinessModels;
using PRN222.Milktea.Service.Services.Interfaces;

namespace PRN222.Milktea.MVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public ProductsController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var products =await _productService.GetProductsAsync();
            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductsByIdAsync(id);

            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryId,Name,Price,Description,Image,ProductType,IsActive")] ProductModel product)
        {
            if (ModelState.IsValid)
            {
                await _productService.AddProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");
            return View(product);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductsByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,CategoryId,Name,Price,Description,Image,ProductType,IsActive")] ProductModel product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _productService.UpdateProductAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var productExist = await _productService.GetProductsByIdAsync(id);

                    if (productExist == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(await _categoryService.GetCategories(), "CategoryId", "Name");
            return View(product);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productService.GetProductsByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetProductsByIdAsync(id);
            if (product != null)
            {
                await _productService.DeleteProductAsync(id);
            }

            return RedirectToAction("Index");
        }
    }
}
