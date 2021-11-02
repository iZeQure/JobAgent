namespace JobAgentClassLibrary.Common.Companies.Entities
{
    public class Company : ICompany
    {
        public int Id { get; set; }
        public int CVR { get; set; }
        public string Name { get; set; }
        public string ContactPerson { get; set; }
    }
}
