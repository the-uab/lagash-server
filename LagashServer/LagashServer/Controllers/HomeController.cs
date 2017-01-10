﻿using LagashServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LagashServer.Controllers
{
    public class HomeController : ApiController
    {
        [Route("")]
        public Information Get()
        {
            return new Information("0.0.0");
        }
    }
}