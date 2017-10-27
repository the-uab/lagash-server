﻿using LagashServer.helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wolf.Lagash.Entities;
using Wolf.Lagash.Interfaces;
using Wolf.Lagash.Services;

namespace LagashServer.Controllers.books
{
    public class BEjemplaresController : ApiController
    {
        private IEjemplaresService service = new EjemplaresService(new LagashContext());

        [Route("v2/books/{id}/ejemplares")]
        public IEnumerable<Ejemplar> Get(string id)
        {
            return service.Query(o => o.book_id == id);
        }

        [Route("v2/books/{id}/ejemplares")]
        public IHttpActionResult Post(Ejemplar item)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            
            try {
                service.Create(item);
                service.Commit();
            } catch (Exception e) {
                return new LagashActionResult(e.Message);
            }

            return CreatedAtRoute("DefaultApi", new { id = item._id }, item);
        }
    }
}
