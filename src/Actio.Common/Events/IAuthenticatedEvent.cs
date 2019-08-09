using System;

namespace Actio.Common.Events
{
    interface IAuthenticatedEvent : IEvent
    {
        Guid UserId { get; }
    }
}