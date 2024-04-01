using System.Text.Json.Serialization;
using AggregateAndMicroService.Aggregates.Course;
using AggregateAndMicroService.Aggregates.User;
using AggregateAndMicroService.Contracts;
using AggregateAndMicroService.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

if (File.Exists("./config.json")) builder.Configuration.AddJsonFile("./config.json");
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

app.MapGet("/materials", async (AppDbContext context) =>
{
    var items = await context.Material.Take(10).ToListAsync();

    return Results.Ok(items);
});

app.MapPost("/materials", async (AppDbContext context) =>
{

    await context.SaveChangesAsync();
})

.WithName("GetWeatherForecast")
.WithOpenApi();

SeedData(app);

app.Run();


void SeedData(IHost app)
{
    using var scope = app.Services.CreateScope();
    using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
    if (!context.Material.Any())
    {

    }
    context.SaveChanges();
}


public class AppDbContext : DbContext
{
    public DbSet<Course> Material { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<CourseCompleting> CourseCompleting { get; set; }

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


        modelBuilder.Entity<Course>().HasKey(p => p.Id);

        modelBuilder.Entity<Course>().ToTable(nameof(Material));

        //modelBuilder.Entity<Material>().HasKey(r => r.Id);
        modelBuilder.Entity<Course>().Property(r => r.Id).ValueGeneratedNever()
            .HasConversion(materialId => materialId.Value, dbId => CourseId.Of(dbId));

        modelBuilder.Entity<Course>().OwnsOne(
            x => x.Status,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Status")
                    .IsRequired();
            }
        );

        modelBuilder.Entity<Course>().OwnsOne(
            x => x.StageCount,
            a => {
                a.Property(p => p.Value)
                    .HasColumnName("StageCount")
                    .HasDefaultValue(0);
            }
        );

        modelBuilder.Entity<Stage>().HasKey(e => e.Id);

        modelBuilder.Entity<Stage>().Property(e => e.Id).ValueGeneratedNever()
        .HasConversion(id => id.Value, dbId => StageId.Of(dbId));

        modelBuilder.Entity<Stage>().OwnsOne(
            x => x.Type,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Type")
                    .IsRequired();
            }
        );

        modelBuilder.Entity<Stage>().OwnsOne(
            x => x.Duration,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Duration")
                    .IsRequired();
            }
        );

        modelBuilder.Entity<Stage>().HasOne(e => e.Course)
            .WithMany(e => e.Stages)
            .HasForeignKey(e => e.CourseId);


        modelBuilder.Entity<CourseCompleting>().HasKey(e => e.Id);

        modelBuilder.Entity<CourseCompleting>().Property(e => e.Id).ValueGeneratedNever()
        .HasConversion(id => id.Value, dbId => CourseCompletingId.Of(dbId));


        modelBuilder.Entity<CourseCompleting>().OwnsOne(
            x => x.Status,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Status")
                    .IsRequired();
            });

        modelBuilder.Entity<CourseCompleting>().OwnsOne(
            x => x.Progress,
            a =>
            {
                a.Property(p => p.Value)
                    .HasColumnName("Progress")
                    .HasDefaultValue(0);
            });
        
        modelBuilder.Entity<CourseCompleting>().OwnsOne(
            x => x.StagesCountData,
            a => {
                a.Property(p => p.TotalStages)
                    .HasColumnName("TotalStages")
                    .HasDefaultValue(0);

                a.Property(p => p.CompletedStages)
                    .HasColumnName("CompletedStages")
                    .HasDefaultValue(0);
            }
        );
        
        modelBuilder.Entity<CourseCompleting>().HasOne(e => e.User)
            .WithMany(e => e.CourseCompletings)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<CourseCompleting>().HasOne(e => e.Course)
            .WithMany(e => e.Completings)
            .HasForeignKey(e => e.CourseId);
    
        modelBuilder.Entity<StageCourseCompleting>().HasKey(e => new { e.CourseCompletingId, e.StageId });

        modelBuilder.Entity<StageCourseCompleting>().OwnsOne(
            x => x.StageProgress,
            a => {
                a.Property(e => e.Value)
                    .HasColumnName("StageProgress")
                    .HasDefaultValue(0);
            }
        );

        modelBuilder.Entity<StageCourseCompleting>().HasOne(e => e.Stage)
            .WithMany(e => e.StageCourseCompletings)
            .HasForeignKey(e => e.StageId);

        modelBuilder.Entity<StageCourseCompleting>().HasOne(e => e.CourseCompleting)
            .WithMany(e => e.StageCourseCompletings)
            .HasForeignKey(e => e.CourseCompletingId);


    }

    
}