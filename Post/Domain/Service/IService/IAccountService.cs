using Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Post.Domain.Service.IService
{
    public interface IAccountService
    {
        public void Login(String username, String password);
    }
}
