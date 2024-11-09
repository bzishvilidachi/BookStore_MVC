using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.Models.ViewModels
{
	public class AddcartVM
	{
		public IEnumerable<Product> products { get; set; }

		public ShoppingCart ShoppingCart { get; set; }
	}
}
