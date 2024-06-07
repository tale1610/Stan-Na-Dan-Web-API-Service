using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Cfg;
using NHibernate;

namespace StanNaDanLibrary;

static class DataLayer
{
    private static ISessionFactory? factory;
    private static object lockObj;

    static DataLayer()
    {
        factory = null;
        lockObj = new();
    }

    public static ISession? GetSession()
    {
        if (factory == null)//ovde samo brzo proveravamo da l je vec kreiran factory i ako jeste da ga odmah vrati da ne trosi vreme u grananju
        {
            lock (lockObj)//obezbedjujemo da samo jedna nit moze da upadne u kriticnu sekciju, klasican singleton
            {
                if (factory == null)
                {
                    factory = CreateSessionFactory();
                }
            }
        }
        return factory?.OpenSession();
    }

    private static ISessionFactory? CreateSessionFactory()
    {
        try
        {
            var cfg = OracleManagedDataClientConfiguration.Oracle10.ShowSql().ConnectionString(c => c.Is("Data Source=gislab-oracle.elfak.ni.ac.rs:1521/SBP_PDB; User Id=S18958;Password=bazelab"));
            //ovo smo mogli da uradimo i u JSON file kao na web sto je mozda i bolje i mozda da zamenimo kasnije

            return Fluently.Configure().Database(cfg).Mappings(m => m.FluentMappings.AddFromAssemblyOf<Poslovnica>()).BuildSessionFactory();
            //ovde je dovoljno da navedemo samo jednu klasu i on ce na osnovu njenog assemblyja da zna gde su sva ostala
        }
        catch (Exception e)
        {
            string error = e.HandleError();
            return null;
        }
    }
}
