using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Repository.IRepository
{
    public interface IAccountRepository
    {
        public Account GetAccount(int id);
        public Account GetAccount(String username, String password);
    }
}
