using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAPIHarjoitusKoodi.Models;

namespace WebAPIHarjoitusKoodi.Controllers
{
    [Route("omareitti/[controller]")]
    [ApiController]
    public class HenkilotController : ControllerBase
    {
        [HttpGet]
        [Route("merkkijono/{nimi}")]

        public string MerkkiJono(string nimi)
        {
            return "Päivää " + nimi + "!";
        }
        [HttpGet]
        [Route("paivamaara")]

        public DateTime Pvm()
        {
            return DateTime.Now;
        }

        [HttpGet]
        [Route("olio")]
        public Henkilo Olio()
        {
            return new Henkilo()
            {
                Nimi = "Paavo Pesusieni",
                Osoite = "Vesipolku 11",
                Ika = 11
            } ;
        }
        [HttpGet]
        [Route("oliolista")]
        public List<Henkilo> OlioLista()
        {
            List<Henkilo> henkilot = new List<Henkilo>()
            {
                new Henkilo()
                {
                    Nimi = "Paavo Pesusieni",
                    Osoite = "Vesipolku 11",
                    Ika = 11
                },
                new Henkilo()
                {
                    Nimi = "Ville Veikko",
                    Osoite = "Varisniitty 23",
                    Ika = 22
                },
                new Henkilo()
                {
                    Nimi = "Kimmo Kievari",
                    Osoite = "Ripalitie 331",
                    Ika = 33
                },
            };
            return henkilot;
        }
        
    }
}
