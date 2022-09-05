using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIHarjoitusKoodi.Models;

namespace WebAPIHarjoitusKoodi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentationController : ControllerBase
    {
        // Get api/Documentation/"keycode"
        [HttpGet]
        [Route("{key}")]
        // Hakee tallennetut eri toiminnallisuudet
        public ActionResult GetDoc(string key)
        {
            NorthwindOriginalContext context = new NorthwindOriginalContext();

            List<Documentation> privateDocList = (from d in context.Documentations
                                                  where d.Keycode == key
                                                  select d).ToList();

            if (privateDocList.Count > 0)
            {
                return Ok(privateDocList);
            }
            else
            {
                return BadRequest("Antamallasi koodilla ei löydy dokumentaatiota, päiväys: " + DateTime.Now.ToString());
            }

        }
    }
}
