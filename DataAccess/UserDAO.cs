using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    class UserDAO
    {
        private static UserDAO instance = null;
        private static readonly object instanceLock = new object();
        public static UserDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }
        public IEnumerable<TblUser> GetUserList()
        {
            var users = new List<TblUser>();
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                users = context.TblUsers.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return users;
        }
        public TblUser GetUserByID(int uID)
        {
            TblUser user = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                user = context.TblUsers.SingleOrDefault(m => m.UserId == uID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }

        public TblUser Login(string email, string password)
        {
            TblUser user = null;
            try
            {
                using var context = new Apple_Accessory_StoreContext();
                user = context.TblUsers.Where(a => a.UserEmail.Equals(email) && a.UserPassword.Equals(password)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return user;
        }
        public void AddNew(TblUser user)
        {
            try
            {
                TblUser _user = GetUserByID(user.UserId);
                if (_user == null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblUsers.Add(user);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The User is already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Update(TblUser user)
        {
            try
            {
                TblUser _user = GetUserByID(user.UserId);
                if (_user != null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblUsers.Update(user);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The User is not already existed");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public void Remove(int uID)
        {
            try
            {
                TblUser user = GetUserByID(uID);
                if (user != null)
                {
                    using var context = new Apple_Accessory_StoreContext();
                    context.TblUsers.Remove(user);
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("The User does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
