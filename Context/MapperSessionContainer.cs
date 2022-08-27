using Assignment3.Model;
using NHibernate;
using System.Collections.Generic;
using System.Linq;

namespace Assignment3.Context
{
    public class MapperSessionContainer : IMapperSessionContainer
    {
        private readonly ISession session;
        private ITransaction transaction;

        public MapperSessionContainer(ISession session)
        {
            this.session = session;
        }

        public IQueryable<Container> container => session.Query<Container>();


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

        public void Save(Container entity)
        {
            session.Save(entity);
        }
        public void Update(Container entity)
        {
            session.Update(entity);
        }
        public void Delete(Container entity)
        {
            session.Delete(entity);
        }

    }
}
