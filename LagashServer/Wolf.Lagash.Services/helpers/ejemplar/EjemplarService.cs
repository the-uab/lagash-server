﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Wolf.Core.EntityFramework;
using Wolf.Lagash.Entities.helper.ejemplar;
using Wolf.Lagash.Interfaces.helpers.ejemplar;

namespace Wolf.Lagash.Services.helpers.ejemplar
{
    public class EjemplarService : EFAdapterBase<Ejemplar>, IEjemplarService
    {
        public EjemplarService(DbContext Context) : base(Context)
        {
        }

        public bool exists(string id)
        {
            return context.Set<Ejemplar>().Count(e => e._id == id) > 0;
        }

        public IEnumerable<Ejemplar> select(int start, int end)
        {
            return context.Set<Ejemplar>().OrderByDescending(o => o.inventory).Where(o => o.inventory <= start && o.inventory >= end);
        }

        public Ejemplar next()
        {
            return context.Set<Ejemplar>().OrderByDescending(o => o.inventory).FirstOrDefault();
        }
    }
}
