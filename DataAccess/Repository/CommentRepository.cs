using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class CommentRepository : ICommentRepository
    {
        public IEnumerable<TblComment> getComments() => CommentDAO.Instance.GetCommentList();
        public IEnumerable<TblComment> GetCommentsByProduct(int pID) => CommentDAO.Instance.GetCommentByProductID(pID);
        public IEnumerable<TblComment> GetCommentsByUser(int uID) => CommentDAO.Instance.GetOrderByUserID(uID);
        public void AddComment(TblComment comment) => CommentDAO.Instance.AddNew(comment);
    }
}
