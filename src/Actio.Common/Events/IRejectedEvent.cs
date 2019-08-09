namespace Actio.Common.Events
{
    interface IRejectedEvent : IEvent
    {
        string Reason { get; }
        string Code { get; }
    }
}