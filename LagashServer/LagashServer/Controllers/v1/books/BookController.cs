﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using LagashServer.helper;
using Wolf.Lagash.Entities.books;
using LagashServer.Controllers.helpers;
using Wolf.Lagash.Services.books;
using Wolf.Lagash.Interfaces.books;

namespace LagashServer.Controllers.v1.books
{
    [Authorize]
    [RoutePrefix("v1/books")]
    public class BookController : ApiController
    {
        private IBookService service = new BookService(new LagashContext());
        
        [Route("")]
        public IEnumerable<Book> Get()
        {
            return service.GetAll();
        }

        [Route("")]
        public IHttpActionResult Post(Book item)
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

        [Route("{id}")]
        public IHttpActionResult Get(String id)
        {
            Book item = service.FindById(id);
            if (item == null) {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(String id)
        {
            Book item = service.FindById(id);
            if (item == null) {
                return NotFound();
            }
            service.Delete(item);
            service.Commit();
            return Ok(item);
        }

        [Route("{id}")]
        public IHttpActionResult Put(String id, Book item)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            if (id != item._id) {
                return new LagashActionResult("should provide a valid _id");
            }
            
            try {
                //Book ejemplar = service.FindOne(o => o.code == item.code);
                //if (ejemplar != null && ejemplar._id != id)
                //{
                //    return new LagashActionResult("El codigo ya esta registrado");
                //}
                //service.discart(ejemplar);
                service.Update(item);
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

        [Route("page/{page}/limit/{limit}")]
        public IEnumerable<Book> Get(int page, int limit)
        {
            return service.GetPage(page, limit, o => o.created);
        }

        [Route("page/{page}/limit/{limit}/search")]
        public IEnumerable<Book> GetFind(int page, int limit, string search)
        {
            if (search == null) search = "";
            return service.Where(page, limit, (o) => {
                return o.title.ToLower().Contains(search.ToLower()) || o.code_material.Contains(search.ToLower());
            }, o => o.created);
        }

        [Route("size")]
        public Size GetSize()
        {
            return new Size()
            {
                total = service.Count()
            };
        }

        [Route("catalog/{id}/page/{page}/limit/{limit}")]
        public IEnumerable<Book> GetItems(string id, int page, int limit, string search)
        {
            if (search == null) search = "";
            return service.Where(page, limit, (o) => {
                return o.catalog_id != null && o.catalog_id.Equals(id) && o.title.ToLower().Contains(search.ToLower());
            }, o => o.created);
        }
    }
}