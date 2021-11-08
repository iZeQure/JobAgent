using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.FormModels
{
    public class ShowContractModel
    {
        private string _contractName = string.Empty;
        private string _contractForCompanyName = string.Empty;
        private string _encodedContractData = string.Empty;
        private string _viewContractMessage = string.Empty;
        private bool _contractExists = false;
        private bool _contractIsLoading = false;

        public string ContractName { get => _contractName; set => _contractName = value; }
        public string ContractForCompanyName { get => _contractForCompanyName; set => _contractForCompanyName = value; }
        public string EncodedContractData { get => _encodedContractData; set => _encodedContractData = value; }
        public string ViewContractMessage { get => _viewContractMessage; set => _viewContractMessage = value; }
        public bool ContractExists { get => _contractExists; set => _contractExists = value; }
        public bool ContractIsLoading { get => _contractIsLoading; set => _contractIsLoading = value; }
    }
}
