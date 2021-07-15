using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IUserRepository
    {
        IEnumerable<TblUser> GetUsers();
        TblUser GetUserById(int uID);
        TblUser Login(string email, string password);
        void InsertUser(TblUser user);
        void DeleteUser(int uID);
        void UpdateUser(TblUser user);
    }
}
