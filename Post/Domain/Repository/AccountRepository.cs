using Microsoft.Identity.Client;
using Post.Domain.Repository.IRepository;
using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Repository
{
    class AccountRepository : IAccountRepository
    {
        private PostContext _context;
        public AccountRepository(PostContext context)
        {
            _context = context;
        }

        public Account GetAccount(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public Account GetAccount(string username, string password)
        {
            return _context.Accounts.FirstOrDefault(a => a.Username.Equals(username) && a.Password.Equals(password));
        }
    }
}
