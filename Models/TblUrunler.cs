using System;
using System.Collections.Generic;

namespace StokTakipWebApi12B.Models;

public partial class TblUrunler
{
    public int UrunId { get; set; }

    public string UrunKodu { get; set; } = null!;

    public int KategoriId { get; set; }

    public string UrunAd { get; set; } = null!;

    public int BirimId { get; set; }

    public string? UrunAciklama { get; set; }

    public double? MinStok { get; set; }

    public double? MaksStok { get; set; }

    public virtual TblKategoriler Kategori { get; set; } = null!;

    public virtual ICollection<TblStokCikis> TblStokCikis { get; set; } = new List<TblStokCikis>();

    public virtual ICollection<TblStokGiris> TblStokGirises { get; set; } = new List<TblStokGiris>();
}
