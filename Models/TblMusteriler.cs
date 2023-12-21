using System;
using System.Collections.Generic;

namespace StokTakipWebApi12B.Models;

public partial class TblMusteriler
{
    public int MusteriId { get; set; }

    public string FirmaAdi { get; set; } = null!;

    public string? YetkiliAdSoyad { get; set; }

    public string? Telefon { get; set; }

    public string? Mail { get; set; }

    public string? Adres { get; set; }

    public virtual ICollection<TblStokCikis> TblStokCikis { get; set; } = new List<TblStokCikis>();
}
