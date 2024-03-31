using AggregateAndMicroService.Aggregates.Course;
using AggregateAndMicroService.Aggregates.User;
using AggregateAndMicroService.Contracts;
using AggregateAndMicroService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMaterialService, MaterialService>();


builder.Services.AddDbContext<AppDbContext>(options =>
{
    //var connectionString = builder.Configuration["dbConnectionString"] ?? throw new Exception("Строка подключения отсутсвует");
    options.UseNpgsql("host=localhost;port=5432;database=modelstore;username=postgres;password=postgres");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

var material = Course.Create(CourseId.Of(Guid.NewGuid()),
StageType.Of(StageTypes.Webinar),
 CourseStatus.Of(Statuses.Active),
 "Material Title", "Material Description", Duration.Of(TimeSpan.FromHours(2)));

app.MapGet("/weatherforecast", (IMaterialService service) =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}


public class AppDbContext : DbContext
{
    public DbSet<Course> Material { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<CourseCompleting> CourseCompleting { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        Database.EnsureCreated();
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


        modelBuilder.Entity<Course>().HasKey(p => p.Id);

        modelBuilder.Entity<Course>().ToTable(nameof(Material));

        //modelBuilder.Entity<Material>().HasKey(r => r.Id);
        modelBuilder.Entity<Course>().Property(r => r.Id).ValueGeneratedNever()
            .HasConversion<Guid>(materialId => materialId.Value, dbId => CourseId.Of(dbId));

        modelBuilder.Entity<Course>().OwnsOne(
            x => x.Status,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Status")
                    .IsRequired();
            }
        );

        /*  modelBuilder.Entity<Course>().OwnsOne(
             x => x.,
             a =>
             {
                 a.Property(p => p.Value)
                     .HasColumnName("Type")
                     .IsRequired();
             }
         );
  */
        /* modelBuilder.Entity<Course>().OwnsOne(
            x => x.Duration,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Duration")
                    .IsRequired();
            }
        ); */

        modelBuilder.Entity<Participiant>().HasKey(e => new { e.Id.MaterialId, e.Id.UserId });


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