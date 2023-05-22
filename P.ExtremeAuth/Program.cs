using Microsoft.EntityFrameworkCore;
using P.ExtremeAuth.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var connectionString = builder.Configuration.GetConnectionString("ExtremeAuth");

builder.Services.AddDbContext<P.ExtremeAuth.Data.DbContext>(options =>
{
    options.UseSqlServer(connectionString, opt => opt.MigrationsAssembly(Constants.MigrationAssembly));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<P.ExtremeAuth.Data.DbContext>();
    db.Database.Migrate();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
