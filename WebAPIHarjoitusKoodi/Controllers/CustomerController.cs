using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIHarjoitusKoodi.Models;

namespace WebAPIHarjoitusKoodi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("nw/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        //private readonly NorthwindOriginalContext db = new();

        // Dependency Injection tyyli
        private readonly NorthwindOriginalContext db;

        public CustomerController(NorthwindOriginalContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        [Route("")]
         public List<Customer> GetAllCustomers()
        {           
            List<Customer> customers = db.Customers.ToList();
            return customers;
        }

        [HttpGet]
        [Route("{id}")]
        public Customer GetOneCustomer(string id)
        {
            Customer customer = db.Customers.Find(id);
             return customer;

        }
        [HttpGet]
        [Route("country/{key}")]
        public List<Customer> GetCustomers(string key)
        {

            var someCustomers = from c in db.Customers
                                where c.Country == key
                                select c;

            return someCustomers.ToList();
        }


        [HttpPost] //<-- filtteri, joka sallii vain POST-metodit (eli HTTP verbit)
        [Route("")] //<-- Routen placeholder
        public ActionResult PostCreateNew([FromBody] Customer asiakas)
        {

            try
            {
                db.Customers.Add(asiakas);
                db.SaveChanges();

                return Ok(asiakas.CustomerId); // Palauttaa vasta luodun uuden asiakkaan avaimen arvon.
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen asiakasta lisättäessä, ota yhteyttä kuruun!");
            }
            finally
            {
                db.Dispose();
            }
        }

        [HttpPut]//<-- Rest-terminologian mukaan tietojen päivitys
        [Route("{key}")] //<-- Routemääritys asiakasavaimella key=CustomerID

        public ActionResult PutEdit(string key, [FromBody] Customer newData) // Key on routessa ja toinen para (dataobjekti)tulee HTTP Bodyssä
        {

            try
            {
                Customer customer = db.Customers.Find(key);
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
                    db.SaveChanges();
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
                db.Dispose();
            }
        }

        [HttpDelete]
        [Route("{key}")]

        public ActionResult DeleteSingle(string key)
        {
            Customer asiakas = db.Customers.Find(key);

            if (asiakas != null)
            {
                db.Customers.Remove(asiakas);
                db.SaveChanges();
                return Ok("Asiakas " + key + " poistettiin tietokannasta!");
            } 
            else
            {
                return NotFound("Asiakasta " + key + " ei löydy!");
            }
        }
        
    }
}
