using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WebAPIHarjoitusKoodi.Models;

namespace WebAPIHarjoitusKoodi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Haetaan kaikki tuotteet
        [HttpGet]
        [Route("")]

        public List<Product> GetAllProducts()
        {
            NorthwindOriginalContext context = new NorthwindOriginalContext();
            List<Product> products = context.Products.ToList();
            return products;
        }

        // Haetaan yhden tuotteen tiedot annetulla tuote-id:llä
        [HttpGet]
        [Route("{id}")]

        public Product GetOneProduct(int id)
        {
            NorthwindOriginalContext context = new();
            Product product = context.Products.Find(id);
            return product;
        }

        // Hakee tuotteet annetun toimittajan id:n mukaan.
        [HttpGet]
        [Route("supplierid/{key}")]
        public ActionResult GetProductBySupplierId(int key)
        {
            NorthwindOriginalContext context = new();

            var someProduct = from p in context.Products
                              where p.SupplierId == key
                              select p;


            return Ok(someProduct.ToList());


        }
        // Lisää/luo uuden tuotteen.
        // Tuotetta lisättäessä, ei käytetä productId:tä. Sillä SQL database luo jokaiselle uudelle tuotteelle oman uuden id:n.
        [HttpPost]
        [Route("")]
        public ActionResult PostCreateNew([FromBody] Product tuote)
        {
            NorthwindOriginalContext context = new();

            try
            {
                context.Products.Add(tuote);
                context.SaveChanges();

                return Ok("Lisätty tuote " + tuote.ProductId + " " + tuote.ProductName);

            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen lisättäessä uutta tuotetta, ota yhteys ylläpitoon!");
            }
            finally
            {
                context.Dispose();
            }
        }


        [HttpPut]
        [Route("{id}")]

        public ActionResult Edit(int id, [FromBody] Product newData)
        {
            NorthwindOriginalContext context = new();

            try
            {
                Product product = context.Products.Find(id);
                if (product != null)
                {
                    product.ProductId = newData.ProductId;
                    product.ProductName = newData.ProductName;
                    product.SupplierId = newData.SupplierId;
                    product.CategoryId = newData.CategoryId;
                    product.QuantityPerUnit = newData.QuantityPerUnit;
                    product.UnitPrice = newData.UnitPrice;
                    product.UnitsInStock = newData.UnitsInStock;
                    product.UnitsOnOrder = newData.UnitsOnOrder;
                    product.ReorderLevel = newData.ReorderLevel;
                    product.ImageLink = newData.ImageLink;
                    context.SaveChanges();
                    return Ok("Päivitit tuotteen " + product.ProductName + "!");
                }
                else
                {
                    return NotFound("Päivitettävää tuotetta ei löytynyt!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen tuotetta päivitettäessä. Ota yhteys ylläpitoon!");
            }
            finally
            {
                context.Dispose();
            }
        }

        // Tuotteen poisto

        [HttpDelete]
        [Route("{id}")]

        public ActionResult DeleteProduct(int id)
        {
            NorthwindOriginalContext context = new();
            Product product = context.Products.Find(id);

            if(product != null)
            {
                context.Products.Remove(product);
                context.SaveChanges();
                return Ok("Tuote " + id + " " + product.ProductName + " poistettiin tietokannasta onnistuunesti!");
            }
            else
            {
                return NotFound("Tuotetta " + id + " ei löytynyt tietokannasta!");
            }
        }


    }
}
