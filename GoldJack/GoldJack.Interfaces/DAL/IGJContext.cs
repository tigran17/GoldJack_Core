using GoldJack.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoldJack.Interfaces.DAL
{
    public interface IGJContext : IDisposable
    {
        Task SaveAsync();
    }
}
