using Assignment3.Model;
using System.Collections.Generic;
using System.Linq;

namespace Assignment3.Context
{
    public interface IMapperSessionContainer
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        void CloseTransaction();
        void Save(Container entity);
        void Update(Container entity);
        void Delete(Container entity);
        IQueryable<Container> container { get; }
    }
}
