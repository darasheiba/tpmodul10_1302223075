using System;
using System.Collections.Generic;
using static tpmodul10_1302223075.Mahasiswa;
class DBMahasiswa : DbContext
{
    public DBMahasiswa(DbContextOptions<DBMahasiswa> options)
        : base(options) { }
    public DbSet<Mahasiswa> mhs => Set<Mahasiswa>();
}
