using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface ICommentRepository
    {
        IEnumerable<TblComment> getComments();
        IEnumerable<TblComment> GetCommentsByProduct(int pID);
        IEnumerable<TblComment> GetCommentsByUser(int uID);
        void AddComment(TblComment comment);
    }
}
