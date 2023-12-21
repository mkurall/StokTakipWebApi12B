using System;
using System.Collections.Generic;

namespace StokTakipWebApi12B.Models;

public partial class TblStokCikis
{
    public int IslemId { get; set; }

    public int UrunId { get; set; }

    public int MusteriId { get; set; }

    public double BirimFiyat { get; set; }

    public double Adet { get; set; }

    public DateTime Tarih { get; set; }

    public int IslemYapanPersonelId { get; set; }

    public virtual TblKullanicilar IslemYapanPersonel { get; set; } = null!;

    public virtual TblMusteriler Musteri { get; set; } = null!;

    public virtual TblUrunler Urun { get; set; } = null!;
}
