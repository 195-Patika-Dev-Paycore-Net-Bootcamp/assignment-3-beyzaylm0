using Assignment3.Model;
using NHibernate;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Context
{
    public class MapperSessionVehicle : IMapperSessionVehicle
    {
        private readonly ISession session;
        private ITransaction transaction;

        public MapperSessionVehicle(ISession session)
        {
            this.session = session;
        }

        public IQueryable<Vehicle> vehicle => session.Query<Vehicle>();


        public void BeginTransaction()
        {
            transaction = session.BeginTransaction();
        }

        public void Commit()
        {
            transaction.Commit();
        }

        public void Rollback()
        {
            transaction.Rollback();
        }

        public void CloseTransaction()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }
        }

        public void Save(Vehicle entity)
        {
            session.Save(entity);
        }
        public void Update(Vehicle entity)
        {
            session.Update(entity);
        }
        public void Delete(Vehicle entity)
        {
            session.Delete(entity);
        }
    }
}
