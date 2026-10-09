using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Unosoft.EFactoringManager.EntityFrameworkCore;

public static class EFactoringManagerDbContextConfigurer
{
    public static void Configure(DbContextOptionsBuilder<EFactoringManagerDbContext> builder, string connectionString)
    {
        builder.UseSqlServer(connectionString);
    }

    public static void Configure(DbContextOptionsBuilder<EFactoringManagerDbContext> builder, DbConnection connection)
    {
        builder.UseSqlServer(connection);
    }
}
