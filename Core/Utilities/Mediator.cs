using System;
using Live2k.Core.Model.Base;
using MongoDB.Driver;

namespace Live2k.Core.Utilities
{
    public class Mediator
    {
        public Mediator(User sessionUser, DocumentCounterReposity counterReposity)
        {
            SessionUser = sessionUser ?? throw new ArgumentNullException(nameof(sessionUser));
            CounterReposity = counterReposity ?? throw new ArgumentNullException(nameof(counterReposity));
        }

        public User SessionUser { get; }
        public DocumentCounterReposity CounterReposity { get; }
    }
}
