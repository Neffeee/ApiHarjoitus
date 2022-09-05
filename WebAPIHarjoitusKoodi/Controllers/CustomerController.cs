using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIHarjoitusKoodi.Models;

namespace WebAPIHarjoitusKoodi.Controllers
{
    [Route("nw/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        [Route("")]
         public List<Customer> GetAllCustomers()
        {
            NorthwindOriginalContext context = new NorthwindOriginalContext();
            List<Customer> customers = context.Customers.ToList();
            return customers;
        }

        [HttpGet]
        [Route("{id}")]
        public Customer GetOneCustomer(string id)
        {
            NorthwindOriginalContext db = new NorthwindOriginalContext();
            Customer customer = db.Customers.Find(id);
             return customer;

        }
        [HttpGet]
        [Route("country/{key}")]
        public List<Customer> GetCustomers(string key)
        {
            NorthwindOriginalContext db = new NorthwindOriginalContext();

            var someCustomers = from c in db.Customers
                                where c.Country == key
                                select c;

            return someCustomers.ToList();
        }


        [HttpPost] //<-- filtteri, joka sallii vain POST-metodit (eli HTTP verbit)
        [Route("")] //<-- Routen placeholder
        public ActionResult PostCreateNew([FromBody] Customer asiakas)
        {
            NorthwindOriginalContext context = new();

            try
            {
                context.Customers.Add(asiakas);
                context.SaveChanges();

                return Ok(asiakas.CustomerId); // Palauttaa vasta luodun uuden asiakkaan avaimen arvon.
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen asiakasta lisättäessä, ota yhteyttä kuruun!");
            }
            finally
            {
                context.Dispose();
            }
        }

        [HttpPut]//<-- Rest-terminologian mukaan tietojen päivitys
        [Route("{key}")] //<-- Routemääritys asiakasavaimella key=CustomerID

        public ActionResult PutEdit(string key, [FromBody] Customer newData) // Key on routessa ja toinen para (dataobjekti)tulee HTTP Bodyssä
        {
            NorthwindOriginalContext context = new();

            try
            {
                Customer customer = context.Customers.Find(key);
                if (customer != null)
                {
                    customer.CompanyName = newData.CompanyName;
                    customer.ContactName = newData.ContactName;
                    customer.ContactTitle = newData.ContactTitle;
                    customer.Country = newData.Country;
                    customer.Address = newData.Address;
                    customer.City = newData.City;
                    customer.PostalCode = newData.PostalCode;
                    customer.Phone = newData.Phone;
                    context.SaveChanges();
                    return Ok(customer.CustomerId);

                }
                else
                {
                    return NotFound("Päivitettävää asiakasta ei löytynyt!");
                }

            }
            catch(Exception)
            {
                return BadRequest("Jokin meni pieleen asiakasta päivitettäessä, ota yhteyttä kuruun!");
            }
            finally
            {
                context.Dispose();
            }
        }

        
    }
}
