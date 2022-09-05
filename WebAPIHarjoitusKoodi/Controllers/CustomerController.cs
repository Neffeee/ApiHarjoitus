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
                return BadRequest("Jokin meni pieleen asiakasta lisättäesttä, ota yhteyttä kuruun!");
            }
            finally
            {
                context.Dispose();
            }
        }
        
    }
}
