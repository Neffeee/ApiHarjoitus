using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIHarjoitusKoodi.Models;

namespace WebAPIHarjoitusKoodi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        // Hakee kaikki työntekijät Employee taulusta
        public List<Employee> GetAllEmployees()
        {
            NorthwindOriginalContext context = new();
            List<Employee> employees = context.Employees.ToList();
            return employees;
        }

        // Hakee työntekijän EmployeeId:n perusteella.

        [HttpGet]
        [Route("{id}")]

        public ActionResult GetOneEmployee(int id)
        {
            NorthwindOriginalContext context = new();
            Employee employee = context.Employees.Find(id);
            return Ok(employee);
        }
        // Hakee työntekijät Cityn mukaan.
        [HttpGet]
        [Route("city/{key}")]

        public ActionResult GetEmployeeByCity(string key)
        {
            NorthwindOriginalContext context = new();
            
            var eachCity = from e in context.Employees
                  where e.City == key
                  select e;

            return Ok(eachCity.ToList());
        }

        // Luo uuden työntekijän

        [HttpPost]
        [Route("")]

        public ActionResult AddNewEmployee([FromBody] Employee newEmployee)
        {
            NorthwindOriginalContext context = new();

            try
            {
                context.Employees.Add(newEmployee);
                context.SaveChanges();
                return Ok("Työntekijä " + newEmployee.FirstName + " " + newEmployee.LastName + " lisätty työntekijä listaan!");
            }
            catch (Exception)
            {
                return BadRequest("Tapahtui jotain odottamatonta. Ota yhteys ylläpitoon!");
            }
            finally
            {
                context.Dispose();
            }             
        }

        // Muokkaa annettua tietoa 

        [HttpPut]
        [Route("{id}")]

        public ActionResult EditEmployee(int id, [FromBody] Employee newData)
        {
            NorthwindOriginalContext context = new();
            try
            {
                Employee employee = context.Employees.Find(id);
                if(employee != null)
                {
                    employee.LastName = newData.LastName;
                    employee.FirstName = newData.FirstName;
                    employee.Title = newData.Title;
                    employee.TitleOfCourtesy = newData.TitleOfCourtesy;
                    employee.BirthDate = newData.BirthDate;
                    employee.HireDate = newData.HireDate;
                    employee.Address = newData.Address;
                    employee.City = newData.City;
                    employee.Region = newData.Region;
                    employee.PostalCode = newData.PostalCode;
                    employee.Country = newData.Country;
                    employee.HomePhone = newData.HomePhone;
                    employee.Extension = newData.Extension;
                    context.SaveChanges();
                    return Ok("Työntekijä " + employee.FirstName + " " + employee.LastName + " tietoja päivitetty!");
                }
                else
                {
                    return NotFound("Työntekijää ei löytynyt!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni vikaan ota yhteys ylläpitoon! :)");
            }
            finally
            {
                context.Dispose();
            }
        }
        // Poistaa annetun Id:n
        [HttpDelete]
        [Route("{id}")]

        public ActionResult DeleteEmployee(int id)
        {
            NorthwindOriginalContext context = new();
            Employee employee = context.Employees.Find(id);

            if (employee != null)
            {
                context.Employees.Remove(employee);
                context.SaveChanges();
                return Ok("Työntekijä " + employee.FirstName + " " + employee.LastName + " poistettiin tietokannasta."); 
            }
            else
            {
                return NotFound("Työntekijää" + employee.FirstName + " " + employee.LastName + " ei löytynyt!");
            }
        }

    }
}
