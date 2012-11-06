

namespace Honey.Controllers
{
	public class ProductController
	{
		// GET/api/Product
		public string Get(object category, object priceFrom, object priceTo)
		{
			return "value";
		}		
	// GET/api/Product/5
		public string Get(int id)
		{
			return "value";
		}		
	// DELETE/api/Product/5
		public string Delete(int id)
		{
			return "value";
		}		
	}
}
