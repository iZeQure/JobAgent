using Dapper.Contrib.Extensions;

namespace JobAgentClassLibrary.Common.Companies.Entities.EntityMaps
{
    [Table("CompanyInformation")]
    public class CompanyInformation : ICompany
    {
        public int Id => CompanyId;

        public string Name => CompanyName;

        [Key]
        public int CompanyId { get; set; }
        public string CompanyName {get;set;}

    }
}
