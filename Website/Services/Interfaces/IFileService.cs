﻿using BlazorInputFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services.Interfaces
{
    public interface IFileService
    {
        public string GetSharedPath { get; }

        Task<string> UploadFileAsync(byte[] file);

        Task<byte[]> GetFileFromDirectoryAsync(string fileName);

        string EncodeFileToBase64(byte[] fileBytes);

        bool CheckFileExists(string fileName);

        /// <summary>
        /// Validates the extention of the file.
        /// </summary>
        /// <param name="contentType">Contains the MIME type of the file, specified the browser.</param>
        /// <returns>True if the type is checked as valid, else false.</returns>
        bool CheckForValidFileExtention(string contentType)
        {
            return contentType switch
            {
                "application/pdf" => true,
                _ => false,
            };
        }
    }
}