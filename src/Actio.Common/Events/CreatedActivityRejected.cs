using System;
using System.Collections.Generic;
using System.Text;

namespace Actio.Common.Events
{
    public class CreatedActivityRejected : IRejectedEvent
    {
        public Guid Id { get; }
        public string Reason { get; }
        public string Code { get; }
        public CreatedActivityRejected()
        {

        }
        public CreatedActivityRejected(Guid id,string code, string reason)
        {
            Id = id;
            Code = code;
            Reason = reason;
        }
    }
}
