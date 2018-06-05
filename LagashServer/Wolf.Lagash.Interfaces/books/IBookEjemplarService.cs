﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Core.Interfaces;
using Wolf.Lagash.Entities;

namespace Wolf.Lagash.Interfaces
{
    public interface IBookEjemplarService : IAdapterBase<BookEjemplar>
    {
        bool exists(String id);
        IEnumerable<BookEjemplar> select(int start, int end);
        BookEjemplar next();
    }
}
