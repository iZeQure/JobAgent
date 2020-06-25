using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Data.Repository.Interface
{
    interface IRepository<T> where T : BaseEntity
    {
        void Create(T create);
        void Update(T update);
        void Remove(int id);
        T GetById(int id);
        IEnumerable<T> GetAll();
    }
}
