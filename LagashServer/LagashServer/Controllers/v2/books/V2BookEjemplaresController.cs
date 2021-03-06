﻿using LagashServer.helper;
using System;
using System.Collections.Generic;
using System.Web.Http;
using Wolf.Lagash.Entities.books;
using Wolf.Lagash.Interfaces.books;
using Wolf.Lagash.Services.books;

namespace LagashServer.Controllers.v2.books
{
    [Authorize]
    [RoutePrefix("v2/books")]
    public class V2BookEjemplaresController : ApiController
    {
        private IBookEjemplarService service = new BookEjemplarService(new LagashContext());

        [Route("{id}/ejemplares")]
        public IEnumerable<BookEjemplar> Get(string id)
        {
            return service.get_asc(o => o.material_id == id, o => o.order);
        }

        [Route("{id}/ejemplares")]
        public IHttpActionResult Post(BookEjemplar item)
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
            return Ok(item);
        }
    }
}
