﻿namespace JobAgentClassLibrary.Common.Categories.Entities
{
    public class Specialization : ISpecialization
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
}
