using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIHarjoitusKoodi.Models;

namespace WebAPIHarjoitusKoodi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        //private readonly NorthwindOriginalContext db = new();

        // Dependency Injection tyyli

        private readonly NorthwindOriginalContext db;

        public UsersController(NorthwindOriginalContext dbparam)
        {
            db = dbparam;
        }

        [HttpGet]
        // Hakee kaikki käyttäjät tietokannasta
        public ActionResult GetAll()
        {
            var users = db.Users;
            foreach(var user in users)
            {
                user.Password = null;
            }
            return Ok(users);
        }

        // Uuden käyttäjän lisääminen
        [HttpPost]

        public ActionResult PostCreateNew([FromBody] User u)
        {
            try
            {
                db.Users.Add(u);
                db.SaveChanges();
                return Ok("Lisättiin käyttäjä " + u.Username);
            }
            catch (Exception e)
            {
                return BadRequest("Lisääminen ei onnistunut. Tässä lisätietoa: " + e);
            }
        }

        [HttpPut]
        [Route("{id}")]

        public ActionResult PutEditOld (int id, [FromBody] User newData)
        {
            try
            {
                
                User user = db.Users.Find(id);
                if(user != null)
                {
                    user.Firstname = newData.Firstname;
                    user.Lastname = newData.Lastname;
                    user.Email = newData.Email;
                    user.Username = newData.Username;
                    user.AcceslevelId = newData.AcceslevelId;
                    user.Password = user.Password;
                    db.SaveChanges();
                    return Ok(user.Username);
                }
                else
                {
                    return NotFound("Päivitettävää käyttäjää ei löytynyt!");
                }
            }
            catch (Exception)
            {
                return BadRequest("Jokin meni pieleen!");
            }
            finally
            {
                db.Dispose();
            }
        }

        [HttpDelete]
        [Route ("{id}")]

        public ActionResult DeleteSingle (int id)
        {
            User user = db.Users.Find(id);

            if (user != null)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                return Ok("Käyttäjä " + user.Username + " poistettiin tietokannasta!");
            }
            else
            {
                return NotFound("Käyttäjää " + user.Username + " ei löytynyt tietokannasta!");
            }
        }
    }
}
