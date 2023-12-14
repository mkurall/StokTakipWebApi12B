using System;
using System.Collections.Generic;

namespace StokTakipWebApi12B.Models;

public partial class TblTedarikciler
{
    public int TedarikciId { get; set; }

    public string FirmaAdi { get; set; } = null!;

    public string? YetkiliAdSoyad { get; set; }

    public string? Telefon { get; set; }

    public string? Mail { get; set; }

    public string? Adres { get; set; }

    public virtual ICollection<TblStokGiris> TblStokGirises { get; set; } = new List<TblStokGiris>();
}
