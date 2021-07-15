using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<TblUser> GetUsers() => UserDAO.Instance.GetUserList();
        public TblUser GetUserById(int uID) => UserDAO.Instance.GetUserByID(uID);
        public TblUser Login(string email, string password) => UserDAO.Instance.Login(email, password);
        public void InsertUser(TblUser user) => UserDAO.Instance.AddNew(user);
        public void DeleteUser(int uID) => UserDAO.Instance.Remove(uID);
        public void UpdateUser(TblUser user) => UserDAO.Instance.Update(user);
    }
}
