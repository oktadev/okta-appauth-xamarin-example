using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using OktaDemo.XF.Models;

namespace OktaDemo.XF.Interfaces
{
    public interface ILoginProvider
    {
        Task<AuthInfo> LoginAsync();
    }
}