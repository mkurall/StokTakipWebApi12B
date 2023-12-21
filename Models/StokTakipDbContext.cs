using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StokTakipWebApi12B.Models;

public partial class StokTakipDbContext : DbContext
{
    public StokTakipDbContext()
    {
    }

    public StokTakipDbContext(DbContextOptions<StokTakipDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblKategoriler> TblKategorilers { get; set; }

    public virtual DbSet<TblKullanicilar> TblKullanicilars { get; set; }

    public virtual DbSet<TblMusteriler> TblMusterilers { get; set; }

    public virtual DbSet<TblStokCikis> TblStokCikis { get; set; }

    public virtual DbSet<TblStokGiris> TblStokGirises { get; set; }

    public virtual DbSet<TblTedarikciler> TblTedarikcilers { get; set; }

    public virtual DbSet<TblUrunler> TblUrunlers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=stok_takip_db;Integrated Security=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblKategoriler>(entity =>
        {
            entity.HasKey(e => e.KategaoriId);

            entity.ToTable("TblKategoriler");

            entity.Property(e => e.KategoriAdi).HasMaxLength(50);
        });

        modelBuilder.Entity<TblKullanicilar>(entity =>
        {
            entity.HasKey(e => e.KullaniciId);

            entity.ToTable("TblKullanicilar");

            entity.Property(e => e.KullaniciAd).HasMaxLength(50);
            entity.Property(e => e.Parola).HasMaxLength(50);
        });

        modelBuilder.Entity<TblMusteriler>(entity =>
        {
            entity.HasKey(e => e.MusteriId);

            entity.ToTable("TblMusteriler");

            entity.Property(e => e.Adres).HasMaxLength(100);
            entity.Property(e => e.FirmaAdi).HasMaxLength(100);
            entity.Property(e => e.Mail).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(15);
            entity.Property(e => e.YetkiliAdSoyad).HasMaxLength(30);
        });

        modelBuilder.Entity<TblStokCikis>(entity =>
        {
            entity.HasKey(e => e.IslemId);

            entity.Property(e => e.Tarih).HasColumnType("datetime");

            entity.HasOne(d => d.IslemYapanPersonel).WithMany(p => p.TblStokCikis)
                .HasForeignKey(d => d.IslemYapanPersonelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblStokCikis_TblKullanicilar");

            entity.HasOne(d => d.Musteri).WithMany(p => p.TblStokCikis)
                .HasForeignKey(d => d.MusteriId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblStokCikis_TblMusteriler");

            entity.HasOne(d => d.Urun).WithMany(p => p.TblStokCikis)
                .HasForeignKey(d => d.UrunId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblStokCikis_TblUrunler");
        });

        modelBuilder.Entity<TblStokGiris>(entity =>
        {
            entity.HasKey(e => e.IslemId);

            entity.ToTable("TblStokGiris");

            entity.Property(e => e.Tarih).HasColumnType("datetime");

            entity.HasOne(d => d.IslemYapanPersonel).WithMany(p => p.TblStokGirises)
                .HasForeignKey(d => d.IslemYapanPersonelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblStokGiris_TblKullanicilar");

            entity.HasOne(d => d.Tedarikci).WithMany(p => p.TblStokGirises)
                .HasForeignKey(d => d.TedarikciId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblStokGiris_TblTedarikciler");

            entity.HasOne(d => d.Urun).WithMany(p => p.TblStokGirises)
                .HasForeignKey(d => d.UrunId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblStokGiris_TblUrunler");
        });

        modelBuilder.Entity<TblTedarikciler>(entity =>
        {
            entity.HasKey(e => e.TedarikciId);

            entity.ToTable("TblTedarikciler");

            entity.Property(e => e.Adres).HasMaxLength(100);
            entity.Property(e => e.FirmaAdi).HasMaxLength(100);
            entity.Property(e => e.Mail).HasMaxLength(50);
            entity.Property(e => e.Telefon).HasMaxLength(15);
            entity.Property(e => e.YetkiliAdSoyad).HasMaxLength(30);
        });

        modelBuilder.Entity<TblUrunler>(entity =>
        {
            entity.HasKey(e => e.UrunId);

            entity.ToTable("TblUrunler");

            entity.Property(e => e.UrunAciklama).HasMaxLength(100);
            entity.Property(e => e.UrunAd).HasMaxLength(50);
            entity.Property(e => e.UrunKodu).HasMaxLength(10);

            entity.HasOne(d => d.Kategori).WithMany(p => p.TblUrunlers)
                .HasForeignKey(d => d.KategoriId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TblUrunler_TblKategoriler");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
