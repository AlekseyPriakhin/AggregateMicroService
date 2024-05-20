using AggregateAndMicroService.Domain.Course;

using MediatR;

namespace AggregateAndMicroService.Application.IntegrationEvents;

public record CourseStatusChangeIntegrationEvent : IntegrationEvent<CourseIntegrationEventDto>, INotification
{
    public CourseStatusChangeIntegrationEvent() : base()
    {
    }
}
