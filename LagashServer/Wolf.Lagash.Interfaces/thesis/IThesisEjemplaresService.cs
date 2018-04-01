﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Core.Interfaces;
using Wolf.Lagash.Entities;

namespace Wolf.Lagash.Interfaces
{
    public interface IThesisEjemplaresService : IAdapterBase<ThesisEjemplar>
    {
        bool exists(String id);
    }
}
