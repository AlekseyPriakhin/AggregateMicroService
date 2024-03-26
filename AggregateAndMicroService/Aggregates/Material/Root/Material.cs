using AggregateAndMicroService.Common;
using Microsoft.Net.Http.Headers;

namespace AggregateAndMicroService.Aggregates.Material;


public class Material : Aggregate<MaterialId> {

  private const int MIN_COMPLETE_PROGRESS = 90;
  private readonly MaterialType[] INSTANT_COMPLETABLE = [
    MaterialType.Of(MaterialTypes.Document),
    MaterialType.Of(MaterialTypes.Presentation)
  ];

  public MaterialType Type { get; private set;}
  public MaterialStatus Status { get; private set; }
  public string Title { get; private set; }
  public string Description { get; private set; }
  public Duration Duration { get; private set; }

  public static Material Create(MaterialId id, MaterialType type, MaterialStatus status, string title, string description, Duration duration) {

    if(status.Equals(MaterialStatus.Of(Statuses.Archived))) {
      throw new CreateWithArchivedStatusException();
    }

    if(type.Equals(MaterialType.Of(MaterialTypes.Webinar))) {
      throw new DurationRequiredException(type.Value.ToString());
    }

    var material = new Material {
      Id = id,
      Type = type,
      Status = status,
      Title = title,
      Duration = duration,
      Description = description
    };

    return material;
  }

  public void ChangeMaterialStatus(MaterialStatus status) {
    
  }

  public Participiant Begin(Guid userId) {
    var participiant = Participiant.Create(ParticipiantId.Of(Id.Value, userId), ParticipiantStatus.Of(ParticipiantStatuses.InProggess),
    Progress.Of(0));
    return participiant;
  }

  public void UpdateProgress(Participiant participiant, int progress) {
    if(INSTANT_COMPLETABLE.Any(e => e.Equals(Type))) {
      participiant.UpdateProgress(100);
      participiant.Complete();
      return;
    }

    var currentProggres = participiant.Progress.Value;
    var newProgress = progress + currentProggres > 100 ? 100 : progress + currentProggres;
    
    participiant.UpdateProgress(newProgress);
    
    if(newProgress > MIN_COMPLETE_PROGRESS && participiant.Status.Equals(ParticipiantStatus.Of(ParticipiantStatuses.InProggess)))
      participiant.Complete();
  }

  public void ChangeDuration(Duration duration, List<Participiant> participiants) {
    foreach (var item in participiants)
    {
      if(item.Status.Equals(ParticipiantStatus.Of(ParticipiantStatuses.Completed))) continue;

      if(item.Progress.Value > 0) {
        var currentProggres = item.Progress.Value; //85%
        var c = Duration.Value.Minutes; // 160%
       
      }
  
    }
    Duration = duration;
  }

}