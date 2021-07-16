using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class CommentDAO
    {
        private static CommentDAO instance = null;
        private static readonly object instanceLock = new object();
        public static CommentDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CommentDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<TblComment> GetCommentList()
        {
            var comments = new List<TblComment>();
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                comments = context.TblComments.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return comments;
        }
        public IEnumerable<TblComment> GetCommentByProductID(int pID)
        {
            IEnumerable<TblComment> com = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                com = context.TblComments.Where(m => m.ProductId == pID).ToList();
                foreach (TblComment co in com)
                {
                    co.User = context.TblUsers.SingleOrDefault(u => u.UserId == co.UserId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return com;
        }
        public IEnumerable<TblComment> GetOrderByUserID(int uID)
        {
            IEnumerable<TblComment> com = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                com = context.TblComments.Where(m => m.UserId == uID).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return com;
        }
        public void AddNew(TblComment com)
        {
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                context.TblComments.Add(com);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
