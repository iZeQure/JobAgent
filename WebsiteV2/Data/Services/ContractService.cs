using BlazorServerWebsite.Data.Services.Abstractions;
using ObjectLibrary.Common;
using SecurityLibrary.Interfaces;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services
{
    public class ContractService : BaseService<IContractRepository, Contract>, IContractService
    {
        private readonly IFileAccess _contractFileAccess;

        public ContractService(IContractRepository contractRepository, IFileAccess contractFileAccess) : base(contractRepository)
        {
            _contractFileAccess = contractFileAccess;
        }

        public bool CheckFileExists(string fileName)
        {
            return _contractFileAccess.CheckFileExists(fileName);
        }

        public override async Task<int> CreateAsync(Contract createEntity, CancellationToken cancellation)
        {
            return await Repository.CreateAsync(createEntity, cancellation);
        }

        public override async Task<int> DeleteAsync(Contract deleteEntity, CancellationToken cancellation)
        {
            return await Repository.DeleteAsync(deleteEntity, cancellation);
        }

        public string EncodeFileToBase64(byte[] fileBytes)
        {
            return _contractFileAccess.EncodeFileToBase64(fileBytes);
        }

        public override async Task<IEnumerable<Contract>> GetAllAsync(CancellationToken cancellation)
        {
            return await Repository.GetAllAsync(cancellation);
        }

        public override async Task<Contract> GetByIdAsync(int id, CancellationToken cancellation)
        {
            return await Repository.GetByIdAsync(id, cancellation);
        }

        public async Task<byte[]> GetFileFromDirectoryAsync(string fileName, CancellationToken cancellation)
        {
            return await _contractFileAccess.GetFileFromDirectoryAsync(fileName, cancellation);
        }

        public override async Task<int> UpdateAsync(Contract updateEntity, CancellationToken cancellation)
        {
            return await Repository.UpdateAsync(updateEntity, cancellation);
        }

        public async Task<string> UploadFIleAsync(byte[] fileBytes, CancellationToken cancellation)
        {
            return await _contractFileAccess.UploadFIleAsync(fileBytes, cancellation);
        }
    }
}
