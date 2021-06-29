using BlazorServerWebsite.Data.FormModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Shared.Components.Modals.Contract
{
    public partial class ShowContractModal
    {
        [Parameter] public ShowContractModel Model { get; set; }
    }
}
