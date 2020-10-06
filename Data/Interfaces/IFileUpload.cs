﻿using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Interfaces
{
    public interface IFileUpload
    {
        Task<bool> UploadFileAsync(IFileListEntry file);
    }
}