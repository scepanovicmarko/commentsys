using System;

namespace MvcApplication1.Models
{
    public class Comment
    {

        // Creator and ReplayToUser I could probably make nHibernate generate inner/left join (constrained many-to-one mapping) 
        // instead of using two separate queries. 
        // User table is small one so this doosn't have big impact on performance (performance hint)
        public virtual int CommentId { get; set; }
        public virtual UserProfile Creator { get; set; }
        public virtual int? ParentCommentId { get; set; }
        public virtual string Text { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual int ThumbUpCount { get; set; }
        public virtual int ThumbDownCount { get; set; }
        public virtual int lft { get; set; }
        public virtual int rgt { get; set; }
        public virtual int indent { get; set; }
        public virtual UserProfile ReplayToUser { get; set; }
        public virtual int ApprovedInd { get; set; }
        public virtual string CommenterName { get; set; }
        public virtual string ParentCommenterName { get; set; }
    }
}
