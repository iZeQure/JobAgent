﻿using Dapper.Contrib.Extensions;
using System.Collections.Generic;

namespace JobAgentClassLibrary.Common.Categories.Entities.EntityMaps
{
    [Table("CategoryInformation")]
    public class CategoryInformation : ICategory
    {
        public int Id => CategoryId;

        public string Name => CategoryName;

        public List<ISpecialization> Specializations { get; set; }

        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }
    }
}
