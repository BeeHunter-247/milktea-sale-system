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
    public class ComboesController : Controller
    {
        private readonly IComboService _comboService;
		private readonly IProductService _productService;

		public ComboesController(IComboService comboService, IProductService productService)
        {
            _comboService = comboService;
            _productService = productService;
        }



        // GET: Comboes
        public async Task<IActionResult> Index(int pageIndex = 1, int pageSize = 6)
        {
            var pagination = new PaginationModel
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
            };

            var milkteaSaleDBContext = await _comboService.GetComboAsync(pagination);
			pagination.PageTotals = (int)Math.Ceiling(milkteaSaleDBContext.TotalCount / (double)pagination.PageSize);

			ViewBag.Pagination = pagination;

            return View(milkteaSaleDBContext);
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _comboService.GetComboByIdAsync(id);
            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }

        public async Task<IActionResult> Create()
        {
            ViewData["ProductId1"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            ViewData["ProductId2"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            ViewData["ProductId3"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ComboName,ComboPrice,Description,Image,ProductId1,ProductId2,ProductId3")] ComboModel combo)
        {
            if (ModelState.IsValid)
            {
                await _comboService.AddComboAsync(combo);
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId1"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name", combo.ProductId1);
            ViewData["ProductId2"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name", combo.ProductId2);
            ViewData["ProductId3"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name", combo.ProductId3);
            return View(combo);
        }
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _comboService.GetComboByIdAsync(id);
            if (combo == null)
            {
                return NotFound();
            }
            ViewData["ProductId1"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            ViewData["ProductId2"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            ViewData["ProductId3"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            return View(combo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ComboId, ComboName,ComboPrice,Description,Image,ProductId1,ProductId2,ProductId3,IsActive")] ComboModel combo)
        {
            if (id != combo.ComboId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _comboService.UpdateComboAsync(combo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    var comboExist = await _comboService.GetComboByIdAsync(id);

                    if (comboExist == null)
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
            ViewData["ProductId1"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            ViewData["ProductId2"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            ViewData["ProductId3"] = new SelectList(await _productService.IsActiveProduct(), "ProductId", "Name");
            return View(combo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combo = await _comboService.GetComboByIdAsync(id);
            if (combo == null)
            {
                return NotFound();
            }

            return View(combo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var combo = await _comboService.GetComboByIdAsync(id);
            if (combo != null)
            {
                await _comboService.DeleteComboAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
