using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        //Injeção de dependência.
        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();

            return View(list);
        }

        //CREATE
        // public IActionResult Create => View();
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var ViewModel = new SellerFormViewModel { Departments = departments };
            return View(ViewModel);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller) 
        {   //if implementado apenas após as validações dos campos de criação
            //if (!ModelState.IsValid)
            //{
               // var departments = await _departmentService.FindAllAsync();
               // var viewModel = new SellerFormViewModel { Seller = seller, Departments = departments };
               // return View(viewModel);
           // }
            await _sellerService.InsertAsync(seller);
            return RedirectToAction("Index");
        }

        //DELETE
        public async Task<IActionResult> Delete(int? id) 
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }
            
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int  id)
        {
            await _sellerService.RemoveAsync(id);
            return RedirectToAction("Index");
        }

        //DETALHES
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if (obj == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }

            return View(obj);
        }

        //Edit
        //Abre a tela para editar o vendedor
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not provided!" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if(obj == null)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = "Id not found!" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            //if implementado apenas após as validações dos campos de criação
            //if (!ModelState.IsValid)
           // {
               // var departments = await _departmentService.FindAllAsync();
               // var viewModel = new SellerFormViewModel {Seller = seller, Departments = departments};
               // return View(viewModel);
            //}
            if (id != seller.Id)
            {
                //return BadRequest();
                return RedirectToAction(nameof(Error), new { message = "Id mesmatch!" });
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                //return NotFound();
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
            catch (DbConcurrencyException e)
            {
                //return BadRequest();
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        //Ação de erro
        public IActionResult Error(string message) 
        {
            var viewModel = new ErrorViewModel 
            { 
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };
            return View(viewModel);
        }
    }
}
