using Microsoft.AspNetCore.Mvc;
using StokTakipWebApi12B.Models;
using StokTakipWebApi12B.Protokol;

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

        [HttpPost]
        public ApiCevap KategoriEkle(TblKategoriler kategori)
        {
            ApiCevap cevap = new ApiCevap();

            ctx.TblKategorilers.Add(kategori);
            ctx.SaveChanges();
            cevap.BasariliMi = true;
            cevap.Data = kategori;

            return cevap;
        }

        [HttpPost]
        public ApiCevap KategoriListesiniGetir()
        {
            ApiCevap cevap = new ApiCevap();
            cevap.BasariliMi = true;
            cevap.Data = ctx.TblKategorilers.ToList();
            return cevap;
        }

        [HttpPost]
        public ApiCevap KategoriGuncelle(TblKategoriler kategori)
        {
            ApiCevap cevap = new ApiCevap();

            var kat = ctx.TblKategorilers.FirstOrDefault(x=>x.KategaoriId == kategori.KategaoriId);

            if(kat == null)
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Böyle bir kayıt bulunumadı. kategoriId: " + kategori.KategaoriId;
                return cevap;
            }

            cevap.BasariliMi = true;

            kat.KategoriAdi = kategori.KategoriAdi;
            ctx.SaveChanges();

            cevap.Data = kat;

            return cevap;
        }

        [HttpPost]
        public ApiCevap KategoriSil(int id)
        {
            ApiCevap cevap = new ApiCevap();

            var kat = ctx.TblKategorilers.FirstOrDefault(x => x.KategaoriId == id);

            if(kat==null)
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Böyle bir kayıt bulunumadı. kategoriId: " + id;
                return cevap;
            }

            ctx.TblKategorilers.Remove(kat);
            ctx.SaveChanges();
            cevap.BasariliMi=true;
            cevap.Data = true;

            return cevap;
        }

    }
}
