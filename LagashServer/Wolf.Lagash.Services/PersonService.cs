﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wolf.Core.EntityFramework;
using Wolf.Lagash.Entities;
using Wolf.Lagash.Interfaces;

namespace Wolf.Lagash.Services
{
    public class PersonService : EFAdapterBase<Person>, IPersonService
    {
        public PersonService(DbContext Context) : base(Context)
        {
        }
    }
}