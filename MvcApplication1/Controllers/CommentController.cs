using AttributeRouting.Web.Http;
using MvcApplication1.Models;
using NHibernate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using NHibernate.Criterion;

namespace MvcApplication1.Controllers
{
    public class CommentController : ApiController
    {
        [HttpGet]
        [GET("All")]
        public IEnumerable All()
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                try
                {
                    IEnumerable<Comment> comments = session.QueryOver<Comment>().OrderBy(p => p.lft).Desc.List();

                    return comments;
                }
                catch (Exception e)
                {
                    // show error code 
                    return null;
                }
            }
        }

        [HttpGet]
        [GET("thumb")]
        public string Thumb(int commentId, bool isUp)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                try
                {
                    var comment = session.QueryOver<Comment>().Where(p => p.CommentId == commentId).List()[0];
                    if (isUp)
                        comment.ThumbUpCount += 1;
                    else
                        comment.ThumbDownCount += 1;

                    using (var tx = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(comment);
                        tx.Commit();
                    }

                    return isUp ? comment.ThumbUpCount.ToString() : comment.ThumbDownCount.ToString();
                }
                catch (Exception e)
                {
                    // show error code 
                    return "error: " + e.Message;
                }
            }
        }

        // to demonstrate on of RESTs . Used for adding comments
        [HttpPost]
        [POST("Add")]
        public HttpResponseMessage Add([FromBody]CommentInputParrams newValues)
        {
            return Request.CreateResponse<Comment>(System.Net.HttpStatusCode.Created, AddComment(newValues));
        }

        [HttpGet]
        [GET("Destroy")]
        public void Destroy(string models)
        {
            using (ISession session = NHibernateSession.OpenSession())
            {
                try
                {
                    JArray jObject = JArray.Parse(models);
                    int lft = Int32.Parse(jObject[0].SelectToken("lft").ToString());
                    int rgt = Int32.Parse(jObject[0].SelectToken("rgt").ToString());
                    IEnumerable<Comment> comments = session.QueryOver<Comment>()
                        .Where(p => p.lft >= lft)
                        .And(p => p.rgt <= rgt)
                        .OrderBy(p => p.CommentId).Desc
                        .List();

                    using (var tx = session.BeginTransaction())
                    {
                        foreach (Comment cc in comments)
                        {
                            session.Delete(cc);
                        }
                        tx.Commit();
                    }

                    // here we can add updates of lft and rgt values if we want to avoid gaps in lfts/rgts sequences
                    // but same functionality is preserved without those updates 

                    //return comments;
                }
                catch (Exception e)
                {
                    // show error code 
                    var a = 1;
                }
            }

        }

        [HttpGet]
        [GET("Create")]
        public HttpResponseMessage Create(string models)
        {
            try
            {
                JArray jObject = JArray.Parse(models);
                string text = jObject[0].SelectToken("Text").ToString();
                return Request.CreateResponse<Comment>(System.Net.HttpStatusCode.Created, AddPost(text));
            }
            catch (Exception e)
            {
               return null;
            }
        }

        //[HttpGet]
        //[GET("Update")]
        //public void Update(string models)
        //{

        //    using (ISession session = NHibernateSession.OpenSession())
        //    {

        //        try
        //        {
        //            JArray jObject = JArray.Parse(models);
        //            int commentId = Int32.Parse(jObject[0].SelectToken("commentId").ToString());
        //            string text = jObject[0].SelectToken("Text").ToString();
        //            Comment comment = session.QueryOver<Comment>()
        //                .Where(p => p.CommentId >= commentId)
        //                .List()[0];

        //            using (var tx = session.BeginTransaction())
        //            {
        //                comment.Text = text;
        //                session.SaveOrUpdate(comment);
        //                tx.Commit();
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            var a = 1;
        //        }
        //    }

        //}

        public class CommentInputParrams
        {
            public int CommentId { get; set; }
            public string Name { get; set; }
            public string Text { get; set; }
        }

        //Private 
        private Comment AddComment(CommentInputParrams newValues)
        {

            using (ISession session = NHibernateSession.OpenSession())
            {
                try
                {
                    HttpContextWrapper context = (HttpContextWrapper)Request.Properties["MS_HttpContext"];
                    Comment parentComment = session.QueryOver<Comment>().Where(p => p.CommentId == newValues.CommentId).List()[0];
                    IEnumerable<Comment> forLftOnlyUpdate = session.QueryOver<Comment>()
                        .Where(p => p.lft >= parentComment.rgt)
                        .And(p => p.rgt < parentComment.rgt)
                        .List();

                    IEnumerable<Comment> forRgtOnlyUpdate = session.QueryOver<Comment>()
                       .Where(p => p.rgt >= parentComment.rgt)
                       .And(p => p.lft < parentComment.rgt)
                       .List();

                    IEnumerable<Comment> forBothUpdate = session.QueryOver<Comment>()
                       .Where(p => p.rgt >= parentComment.rgt)
                       .And(p => p.lft >= parentComment.rgt)
                       .List();

                    Comment newComment = new Comment();
                    newComment.Text = newValues.Text;

                    newComment.ParentCommentId = newValues.CommentId;
                    newComment.ReplayToUser = parentComment.Creator;
                    newComment.lft = parentComment.rgt;
                    newComment.rgt = parentComment.rgt + 1;
                    newComment.indent = parentComment.indent + 1;
                    newComment.ParentCommenterName = parentComment.CommenterName;
                    newComment.CreationDate = DateTime.Now;
                    if (context.User.Identity.IsAuthenticated)
                    {
                        newComment.Creator = session.QueryOver<UserProfile>().Where(u => u.UserName == context.User.Identity.Name).List()[0];
                        newComment.CommenterName = context.User.Identity.Name;
                    }
                    else
                    {
                        newComment.Creator = null;
                        newComment.CommenterName = newValues.Name;
                    }

                    using (var tx = session.BeginTransaction())
                    {
                        // Enable batch updates in nHibernate congfig (performance hint)
                        // ADO.Net is faster on this matter because we can use update statment on multiple rows
                        // Maybe this can be accomplished through nHibernate but I have no time to read best practises in such short notice 
                        foreach (Comment cc in forLftOnlyUpdate)
                        {
                            cc.lft += 2;
                            session.SaveOrUpdate(cc);
                        }

                        foreach (Comment cc in forRgtOnlyUpdate)
                        {
                            cc.rgt += 2;
                            session.SaveOrUpdate(cc);
                        }

                        foreach (Comment cc in forBothUpdate)
                        {
                            cc.lft += 2;
                            cc.rgt += 2;
                            session.SaveOrUpdate(cc);
                        }

                        session.SaveOrUpdate(newComment);
                        tx.Commit();
                    }

                    return newComment;
                }
                catch (Exception e)
                {
                    // show error code 
                    return null;
                }
            }

        }
        private Comment AddPost(string text)
        {
            Comment newComment = new Comment();
            using (ISession session = NHibernateSession.OpenSession())
            {
               
                HttpContextWrapper context = (HttpContextWrapper)Request.Properties["MS_HttpContext"];

                int lft = (int)session.CreateCriteria<Comment>().SetProjection(Projections.Max("rgt")).UniqueResult();
               
                newComment.Text = text;
                newComment.lft = lft + 1;
                newComment.rgt = lft + 2;
                newComment.indent = 0;
                newComment.CreationDate = DateTime.Now;
                newComment.Creator = session.QueryOver<UserProfile>().Where(u => u.UserName == context.User.Identity.Name).List()[0];
                newComment.CommenterName = context.User.Identity.Name;

                using (var tx = session.BeginTransaction())
                {
                    session.SaveOrUpdate(newComment);
                    tx.Commit();
                }

            }
            return newComment;
        }

    }
}
