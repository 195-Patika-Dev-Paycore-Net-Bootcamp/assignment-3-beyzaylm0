using Assignment3.Model;
using System.Linq;

namespace Assignment3.Context
{
    public interface IMapperSessionVehicle
    {
        void BeginTransaction();
        void Commit();
        void Rollback();
        void CloseTransaction();
        void Save(Vehicle entity);
        void Update(Vehicle entity);
        void Delete(Vehicle entity);

        IQueryable<Vehicle> vehicle { get; }
    }
}
