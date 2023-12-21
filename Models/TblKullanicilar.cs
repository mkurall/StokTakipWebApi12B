using System;
using System.Collections.Generic;

namespace StokTakipWebApi12B.Models;

public partial class TblKullanicilar
{
    public int KullaniciId { get; set; }

    public string KullaniciAd { get; set; } = null!;

    public string Parola { get; set; } = null!;

    public int? Yetki { get; set; }

    public virtual ICollection<TblStokCikis> TblStokCikis { get; set; } = new List<TblStokCikis>();

    public virtual ICollection<TblStokGiris> TblStokGirises { get; set; } = new List<TblStokGiris>();
}
