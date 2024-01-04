using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StokTakipWebApi12B.Models;
using StokTakipWebApi12B.Protokol;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace StokTakipWebApi12B.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[action]")]
    public class StokTakipController : ControllerBase
    {
        StokTakipDbContext ctx;
        public StokTakipController(StokTakipDbContext dbContext)
        {
            this.ctx = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public string Test()
        {
            return "Api erişimi çalışıyor.";
        }

        [HttpPost]
        [AllowAnonymous]
        public ApiCevap OturumAc(string kullaniciAdi, string parola)
        {
            var kullanici = ctx.TblKullanicilars.FirstOrDefault(x => x.KullaniciAd == kullaniciAdi && x.Parola == parola);
            ApiCevap cevap = new ApiCevap();
            if (kullanici == null)
            {
                cevap.BasariliMi = false;
                cevap.HataMesaji = "Kullanıcı adı yada parola hatalı.";
            }
            else
            {
                //Burada kullanıcı için bilet düzenlenecek
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Bu gizli bir kelime"));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                DateTime now = DateTime.Now;

                System.Security.Claims.Claim[] claims =
                {
                    new System.Security.Claims.Claim("kullanici_id", kullanici.KullaniciId.ToString()),
                    new System.Security.Claims.Claim("kullanici_ad", kullanici.KullaniciAd),
                    new System.Security.Claims.Claim("kullanici_yetki", kullanici.Yetki.GetValueOrDefault().ToString()),

                };

                var token = new JwtSecurityToken("stoktakip.com", "stokstakip.com",
              claims,
              notBefore: now,//başlangıç tarihi
              expires: now.AddDays(7),//bitiş tarihi
              signingCredentials: creds);


                //Bana bu access token gerekli
                string access = new JwtSecurityTokenHandler().WriteToken(token);

                cevap.BasariliMi = true;
                cevap.Data = access;

            }

            return cevap;
        }


        [HttpGet]
        
        public ApiCevap KullanicilariGetir()
        {
            ApiCevap cevap = new ApiCevap();
            cevap.Data = ctx.TblKullanicilars.ToList();
            cevap.BasariliMi = true;
            return cevap;
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
