using System.Web;
using NHibernate;
using NHibernate.Cfg;
using System;

namespace MvcApplication1.Models
{
    public class NHibernateSession
    {
        public static ISession OpenSession()
        {
            var configuration = new Configuration();
            var configurationPath = HttpContext.Current.Server.MapPath(@"~\Models\hibernate.cfg.xml");
            configuration.Configure(configurationPath);
            configuration.AddFile(HttpContext.Current.Server.MapPath(@"~\Models\Comment.hbm.xml"));
            configuration.AddFile(HttpContext.Current.Server.MapPath(@"~\Models\UserProfile.hbm.xml"));
            ISessionFactory sessionFactory = configuration.BuildSessionFactory();
            return sessionFactory.OpenSession();
        }
    }
}