﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Wolf.Lagash.Services;
using Wolf.Lagash.Entities;
using Wolf.Lagash.Interfaces;
using LagashServer.helper;
using Wolf.Lagash.Entities.books;
using Wolf.Lagash.Entities.history;
using Wolf.Lagash.Services.history;
using Wolf.Lagash.Interfaces.history;

namespace LagashServer.Controllers.v1.books
{
    [RoutePrefix("v1/ejemplares")]
    public class EjemplaresController : ApiController
    {
        private IEjemplarService service = new EjemplarService(new LagashContext());

        [Route("")]
        public IEnumerable<Ejemplar> Get()
        {
            return service.GetAll();
        }

        [Route("")]
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

        [Route("{id}")]
        public IHttpActionResult Get(String id)
        {
            Ejemplar item = service.FindById(id);
            if (item == null) {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(String id)
        {
            Ejemplar item = service.FindById(id);
            if (item == null) {
                return NotFound();
            }
            service.Delete(item);
            service.Commit();
            return Ok(item);
        }

        [Route("{id}")]
        public IHttpActionResult Put(String id, Ejemplar item)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (id != item._id) {
                return new LagashActionResult("should provide a valid _id");
            }
            service.Update(item);
            try {
                service.Commit();
            } catch (DbUpdateConcurrencyException) {
                if (!service.exists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }
            return Ok(item);
        }
    }
}