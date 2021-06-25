using ObjectLibrary.Common;
using SqlDataAccessLibrary.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServerWebsite.Data.Services.Abstractions
{
    /// <summary>
    /// Represents a genric class for services.
    /// </summary>
    /// <typeparam name="TBaseRepository"></typeparam>
    public abstract class BaseService<TBaseRepository>
    {
        private readonly TBaseRepository _repository;

        /// <summary>
        /// Instantiates a service with the gerric <see cref="TBaseRepository"/>.
        /// </summary>
        /// <param name="repository">A repository uesd within the service.</param>
        public BaseService (TBaseRepository repository)
        {
            _repository = repository;
        }

        public TBaseRepository Repository { get { return _repository; } }
    }
}
