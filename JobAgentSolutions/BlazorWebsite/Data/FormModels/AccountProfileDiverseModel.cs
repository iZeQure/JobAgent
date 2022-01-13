using JobAgentClassLibrary.Core.Entities;

namespace BlazorWebsite.Data.FormModels
{
    public class AccountProfileDiverseModel : BaseModel
    {
        public int ConsultantAreaIdToBeAssigned { get; set; }
        public int ConsultantAreaIdToBeRemoved { get; set; }
    }
}
