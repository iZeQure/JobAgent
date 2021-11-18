using BlazorWebsite.Data.FormModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorWebsite.Shared.Components.Modals.StaticSearchFilterModals
{
    public partial class EditStaticSearchFilterModal
    {
        [Parameter] public StaticSearchFilterModel Model { get; set; }
    }
}
