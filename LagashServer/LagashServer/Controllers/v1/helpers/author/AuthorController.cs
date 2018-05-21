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
using LagashServer.Controllers.helpers;
using Wolf.Lagash.Interfaces.map;
using Wolf.Lagash.Entities.map;

namespace LagashServer.Controllers.v1.books
{
    [RoutePrefix("v1/author")]
    public class AuthorController : ApiController
    {
        private IAuthorService service = new AuthorService(new LagashContext());
        private IAuthorMapService service_map = new AuthorMapService(new LagashContext());

        [Route("")]
        public IEnumerable<Author> Get()
        {
            return service.GetAll();
        }

        [Route("")]
        public IHttpActionResult Post(Author item)
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
            Author item = service.FindById(id);
            if (item == null) {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("{id}")]
        public IHttpActionResult Delete(String id)
        {
            Author item = service.FindById(id);
            if (item == null) {
                return NotFound();
            }
            service.Delete(item);
            service.Commit();
            return Ok(item);
        }

        [Route("{id}")]
        public IHttpActionResult Put(String id, Author item)
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
        
        [Route("page/{page}/limit/{limit}/search")]
        public IEnumerable<Author> GetFind(int page, int limit, string search)
        {
            if (search == null) search = "";
            return service.Where(page, limit, (u) => {
                return u.first_name.Contains(search) || u.last_name.Contains(search);
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

        [Route("page/{page}/limit/{limit}")]
        public IEnumerable<Author> Get(int page, int limit)
        {
            return service.GetPage(page, limit, o => o.created);
        }

        [Route("find")]
        public IEnumerable<Author> GetFind(string resource_id)
        {
            IEnumerable<AuthorMap> items = service_map.Query(o => o.resource_id == resource_id);
            List <Author> result = new List<Author>();
            foreach (var item in items)
            {
                Author author = service.FindById(item.author_id);
                author.map = item;
                result.Add(author);
            }
            return result;
        }
    }
}