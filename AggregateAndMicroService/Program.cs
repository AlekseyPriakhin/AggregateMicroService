using System.Text.Json.Serialization;
using AggregateAndMicroService.Aggregates.Material;
using AggregateAndMicroService.Aggregates.User;
using AggregateAndMicroService.Contracts;
using AggregateAndMicroService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

if(File.Exists("./config.json")) builder.Configuration.AddJsonFile("./config.json");
else throw new Exception("Файл конфигурации config.json отсутсвует!");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.ConfigureHttpJsonOptions(options => options.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));
builder.Services.AddScoped<IMaterialService, MaterialService>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = builder.Configuration["dbConnectionString"] ?? throw new Exception("Строка подключения отсутсвует");
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/materials", async (AppDbContext context) => {
    var items = await context.Material.Take(10).ToListAsync();

    return Results.Ok(items);
});

app.MapPost("/materials", async (AppDbContext context) => {
    var material = Material.Create(MaterialId.Of(Guid.NewGuid()), 
        MaterialType.Of(MaterialTypes.Webinar), 
        MaterialStatus.Of(Statuses.Draft),
        "Webinar title", "Webinar description", Duration.Of(TimeSpan.FromMinutes(120)));

        context.Material.Add(material);
        await context.SaveChangesAsync();
} )

.WithName("GetWeatherForecast")
.WithOpenApi();

SeedData(app);

app.Run();


void SeedData(IHost app) {
    using var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    if(!context.Material.Any()) {
        var material = Material.Create(MaterialId.Of(Guid.NewGuid()), 
        MaterialType.Of(MaterialTypes.Document), 
        MaterialStatus.Of(Statuses.Active),
        "Document title", "Document description");


        var webinar = Material.Create(MaterialId.Of(Guid.NewGuid()), 
        MaterialType.Of(MaterialTypes.Webinar), 
        MaterialStatus.Of(Statuses.Draft),
        "Webinar title", "Webinar description", Duration.Of(TimeSpan.FromMinutes(120)));

        context.Material.Add(material);
        context.Material.Add(webinar);
    }
        context.SaveChanges();
}


public class AppDbContext : DbContext
{
    public DbSet<Material> Material { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Participiant> Participiants { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        //Database.EnsureCreated();
    }

    public AppDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasKey(e => e.Id);

        modelBuilder.Entity<User>().Property(e => e.Id).ValueGeneratedNever()
        .HasConversion(id => id.Value, dbId => UserId.Of(dbId));


        modelBuilder.Entity<Material>().HasKey(p => p.Id);

        modelBuilder.Entity<Material>().ToTable(nameof(Material));

        //modelBuilder.Entity<Material>().HasKey(r => r.Id);
        modelBuilder.Entity<Material>().Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(materialId => materialId.Value, dbId => MaterialId.Of(dbId));

        modelBuilder.Entity<Material>().OwnsOne(
            x => x.Status,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Status")
                    .IsRequired();
            }
        );

        modelBuilder.Entity<Material>().OwnsOne(
            x => x.Type,
            a =>
            {
                a.Property(p => p.Value)
                    .HasConversion(type => type, type => type)
                    .HasColumnName("Type")
                    .IsRequired();
            }
        );

        modelBuilder.Entity<Material>().OwnsOne(
            x => x.Duration,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Duration");
            }
        );

        modelBuilder.Entity<Participiant>().HasKey(e => new { e.MaterialId, e.UserId });


        modelBuilder.Entity<Participiant>().OwnsOne(
            x => x.Status,
            e =>
            {
                e.Property(p => p.Value)
                    .HasColumnName("Status")
                    .IsRequired();
            }
        );

        modelBuilder.Entity<Participiant>().OwnsOne(
            x => x.Progress,
            e =>
            {
                e.Property(p => p.Value)
                    .HasColumnName("Progress");
            }
        );

        modelBuilder.Entity<Participiant>().HasOne(e => e.Material)
        .WithMany(e => e.Participiants)
        .HasForeignKey(e => e.MaterialId);

        modelBuilder.Entity<Participiant>().HasOne(e => e.User)
        .WithMany(e => e.Participiants)
        .HasForeignKey(e => e.UserId);


    }
}