using ASP.NETCore_WithoutEF.Models;
using ASP.NETCore_WithoutEF.Repository.Contract;
using ASP.NETCore_WithoutEF.Repository.Implementation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace ASP.NETCore_WithoutEF.Controllers
{
	public class CartController : Controller
	{
		private readonly IGenericRepository<Product> _productRepo;
		private readonly IGenericRepository<ShoppingCart> _cartRepo;
		private readonly IGenericRepository<ApplicationUser> _userRepo;
		public ShoppingCartVM ShoppingCartVM { get; set; }
		public CartController(IGenericRepository<Product> productRepo, IGenericRepository<ShoppingCart> cartRepo, IGenericRepository<ApplicationUser> userRepo)
		{
			_productRepo = productRepo;
			_cartRepo = cartRepo;
			_userRepo = userRepo;
		}
		public async Task<IActionResult> Index()
		{
			ShoppingCartVM = new()
			{
				ShoppingCartList = await _cartRepo.List(),
				ProductList = await _productRepo.List(),
				OrderHeader = new()

			};
			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}
			return View(ShoppingCartVM);
		}

		public async Task<IActionResult> Summary()
		{
			var usersTask = _userRepo.List();
			var cartsTask = _cartRepo.List();
			var productsTask = _productRepo.List();

			await Task.WhenAll(usersTask, cartsTask, productsTask);

			List<ApplicationUser> applicationUsers = usersTask.Result;
			List<ShoppingCart> shoppingCarts = cartsTask.Result;
			List<Product> products = productsTask.Result;

			if (!applicationUsers.Any()) // Ensure at least one user exists
			{
				return BadRequest("No users found.");
			}

			var user = applicationUsers.First(); // Select a single user

			var ShoppingCartVM = new ShoppingCartVM
			{
				ShoppingCartList = shoppingCarts,
				ProductList = products,
				OrderHeader = new OrderHeader
				{
					Name = user.Name,
					StreetAddress = user.StreetAddress,
					City = user.City,
					State = user.State,
					PostalCode = user.PostalCode,
					PhoneNumber = user.PhoneNumber
				}
			};

			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

			return View(ShoppingCartVM);
		}


		public async Task<IActionResult> Plus(int id)
		{

			ShoppingCartVM cart = new ShoppingCartVM()
			{
				ShoppingCart = await _cartRepo.Detail(id),
				ShoppingCartId = id
			};

			cart.ShoppingCart.Count += 1;
			await _cartRepo.Edit(cart.ShoppingCart);


			return RedirectToAction(nameof(Index));

		}
		public async Task<IActionResult> Minus(int id)
		{

			ShoppingCartVM cart = new ShoppingCartVM()
			{
				ShoppingCart = await _cartRepo.Detail(id),
				ShoppingCartId = id
			};


			if (cart.ShoppingCart.Count <= 1)
			{
				//remove the cart
				await _cartRepo.Delete(id);
			}
			else
			{
				cart.ShoppingCart.Count -= 1;
			}
			await _cartRepo.Edit(cart.ShoppingCart);

			return RedirectToAction(nameof(Index));
		}
		public async Task<IActionResult> Remove(int id)
		{
			ShoppingCartVM cart = new ShoppingCartVM()
			{
				ShoppingCart = await _cartRepo.Detail(id),
				ShoppingCartId = id
			};

			//remove the cart
			await _cartRepo.Delete(id);
			return RedirectToAction(nameof(Index));

		}
		private double GetPriceBasedOnQuantity(ShoppingCart shoppingCart)
		{
			if (shoppingCart.Count <= 50)
			{
				return shoppingCart.Product.Price;
			}
			else
			{
				if (shoppingCart.Count <= 100)
				{
					return shoppingCart.Product.Price50;
				}
				else
				{
					return shoppingCart.Product.Price100;
				}
			}
		}
	}
}
