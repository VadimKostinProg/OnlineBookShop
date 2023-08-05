using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.Infrastructure.DBInitializer
{
    public interface IDBInitializer
    {
        public Task InitializeDB();
    }
}
