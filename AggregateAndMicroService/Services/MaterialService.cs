using AggregateAndMicroService.Aggregates.Material;
using AggregateAndMicroService.Common;
using AggregateAndMicroService.Contracts;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AggregateAndMicroService.Services;

public class MaterialService : IMaterialService {

  private readonly AppDbContext _dbContext;

  public MaterialService(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }
  public async Task<bool> ChangeDuration(Guid id, TimeSpan duration) {

    var material = await _dbContext.Material.FindAsync(id) ?? throw new NotFoundException(id);
    var participiants = _dbContext.Participiants.Where(e => e.MaterialId == id);

    foreach (var item in participiants)
    {
      if (item.Status.Equals(ParticipiantStatus.Of(ParticipiantStatuses.Completed))) continue;

      if (item.Progress.Value > 0 && material.IsInstantCompletable)
      {
        if(material.Duration is null) throw new DurationRequiredException(material.Type.Value.ToString()); 
        var currentProggres = item.Progress.Value;
        var currentDuration = material.Duration.Value.Minutes;

        var currentProggresInMinutes = currentDuration / 100 * currentProggres;
        var newProgress = currentProggresInMinutes / duration.Minutes * 100;

        item.UpdateProgress(newProgress);

        if (newProgress > Material.MIN_COMPLETE_PROGRESS && item.Status.Equals(ParticipiantStatus.Of(ParticipiantStatuses.InProggess)))
        {
          item.Complete();
          continue;
        }

        if (newProgress < Material.MIN_COMPLETE_PROGRESS && item.Status.Equals(ParticipiantStatus.Of(ParticipiantStatuses.Completed)))
        {
          item.UpdateProgress(Material.MIN_COMPLETE_PROGRESS);
        }

      }

    }

    material.ChangeDuration(Duration.Of(duration), participiants);

    await _dbContext.SaveChangesAsync();

    return true;
  }
}