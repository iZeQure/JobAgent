﻿using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Common.Roles.Entities
{
    public interface IRole : IAggregateRoot, IEntity<int>
    {
        public string Name { get; }
        public string Description { get; }
    }
}
