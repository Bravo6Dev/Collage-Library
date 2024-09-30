using DataLayer.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Reposertory
{
    public interface IAuth
    {
        public Task<AuthEF> RegisterAsync(RegisterEF Model);
        public Task<AuthEF> LoginAsync(LoginEF Model);
    }
}
