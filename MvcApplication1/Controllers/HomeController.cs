using System;
using System.Linq;
using System.Web.Mvc;
using NHibernate;
using NHibernate.Linq;
using MvcApplication1.Models;
using System.Collections.Generic;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        public IList<Comment>  comments;  
        public ActionResult Index()
        {
            ViewBag.Message = "Home page containing news list.";

            using (ISession session = NHibernateSession.OpenSession())
            {
                try
                {
                    comments = session.Query<Comment>().Where(p => p.indent == 0).ToList();
                }
                catch (Exception e)
                {
                    return null;
                    // handel errors here  
                }
            }

            return View(comments);
        }

        public ActionResult Comments(int news)
        {
            ViewBag.Message = "Comment list for specific news.";

            // Here I should instantiate sessionFactory on higher level (preformance hint)
            using (ISession session = NHibernateSession.OpenSession())
            {
                try
                {
                    //SELECT * FROM comments WHERE CommentId =:commentId
                    //Also, I could pass left and right values from form-end directly to avoid this database call
                    //but that would require front-end updates on news list after each add comment action 
                    var root = session.Query<Comment>().Where(p => p.CommentId == news).ToList()[0];

                    //SELECT * FROM comments WHERE lft BETWEEN :lft AND :rgt ORDER BY lft ASC
                    // We didn't specify whether the comment system should be ordered ASC or DESC, so I choosed ascendng orientation.
                    comments = session.Query<Comment>()
                        .Where(p => p.lft >= root.lft)
                        .Where(p => p.lft <= root.rgt).OrderBy(p => p.lft)
                        .ToList();
                }
                catch (Exception e)
                {
                    // show error code 
                }
            }

            return View(comments);
        }

        public ActionResult Administration()
        {
            

            ViewBag.Message = "Administer your posts.";
            if (Request.LogonUserIdentity.IsAuthenticated)
                return View();
            else return null;
        }
    }
}
