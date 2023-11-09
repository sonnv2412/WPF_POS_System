using Post.Domain.Repository.IRepository;
using Post.Domain.Service.IService;
using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Service
{
    public class AccountService : IAccountService
    {
        public static Account account;
        private IAccountRepository accountRepository;
        public AccountService(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }

        public void Login(string username, string password)
        {
            Account acc = accountRepository.GetAccount(username, password);
            if (acc != null)
            {
                account = acc;
            }
            else
                account = null;
        }


    }
}
