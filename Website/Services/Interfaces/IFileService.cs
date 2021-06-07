﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(byte[] file);

        Task<byte[]> GetFileFromDirectoryAsync(string fileName);

        string EncodeFileToBase64(byte[] fileBytes);

        bool CheckFileExists(string fileName);
    }
}