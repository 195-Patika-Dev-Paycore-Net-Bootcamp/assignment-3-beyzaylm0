using Assignment3.Model;
using NHibernate;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Assignment3.Mapping
{
    public class ContainerMap :ClassMapping<Container>
    {
        public ContainerMap()
        {
            Id(x => x.id, x =>
            {
                x.Type(NHibernateUtil.Int64);
                x.Column("id");
                x.UnsavedValue(0);
                x.Generator(Generators.Increment);
            });

            Property(b => b.containername, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.StringClob);
                x.NotNullable(true);
            });
            Property(b => b.latitude, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
            });
            Property(b => b.longitude, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.Double);
                x.NotNullable(true);
            });
            Property(b => b.vehicleid, x =>
            {
                x.Length(50);
                x.Type(NHibernateUtil.Int64);
                x.NotNullable(true);
            });
            Table("container");
        }
    }
}

