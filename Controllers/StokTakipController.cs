using Microsoft.AspNetCore.Mvc;
using StokTakipWebApi12B.Models;

namespace StokTakipWebApi12B.Controllers
{
    [ApiController]
    [Route("api/v1/[action]")]
    public class StokTakipController : ControllerBase
    {
       StokTakipDbContext ctx;
        public StokTakipController(StokTakipDbContext dbContext)
        {
            this.ctx = dbContext;
        }

        [HttpGet]
        public string Test()
        {
            return "Api erişimi çalışıyor.";
        }

        [HttpGet]
        public IEnumerable<TblKullanicilar> KullanicilariGetir()
        {
            return ctx.TblKullanicilars.ToList();
        }
    }
}
