﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using System.Web.Http.Description;
using LagashServer.helper;
using LagashServer.Controllers.helpers;
using Wolf.Lagash.Entities.thesis;
using Wolf.Lagash.Services.thesis;
using Wolf.Lagash.Interfaces.thesis;

namespace LagashServer.Controllers.v1.thesis
{
    [Authorize]
    [RoutePrefix("v1/theses")]
    public class ThesisController : ApiController
    {
        private IThesisService service = new ThesisService(new LagashContext());

        [Route("")]
        public IEnumerable<Thesis> Get()
        {
            return service.GetAll();
        }

        [Route("")]
        public IHttpActionResult Post(Thesis item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                service.Create(item);
                service.Commit();
            }
            catch (Exception e)
            {
                return new LagashActionResult(e.Message);
            }
            return Ok(item);
        }

        [Route("{id}")]
        [ResponseType(typeof(Thesis))]
        public IHttpActionResult Get(string id)
        {
            Thesis item = service.FindById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("{id}")]
        [ResponseType(typeof(Thesis))]
        public IHttpActionResult Delete(string id)
        {
            Thesis item = service.FindById(id);
            if (item == null)
            {
                return NotFound();
            }
            service.Delete(item);
            service.Commit();
            return Ok(item);
        }

        [Route("{id}")]
        [ResponseType(typeof(void))]
        public IHttpActionResult Put(string id, Thesis item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (id != item._id)
            {
                return new LagashActionResult("should provide a valid _id");
            }
            try
            {
                Thesis ejemplar = service.FindOne(o => o.code_material == item.code_material);
                if (ejemplar != null && ejemplar._id != id)
                {
                    return new LagashActionResult("El codigo ya esta registrado");
                }
                service.discart(ejemplar);
                service.Update(item);
                service.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!service.exists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return Ok(item);
        }

        [Route("page/{page}/limit/{limit}")]
        public IEnumerable<Thesis> Get(int page, int limit)
        {
            return service.GetPage(page, limit, o => o.created);
        }

        [Route("page/{page}/limit/{limit}/search")]
        public IEnumerable<Thesis> GetFind(int page, int limit, string search)
        {
            if (search == null) search = "";
            return service.Where(page, limit, (o) =>
            {
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
        public IEnumerable<Thesis> GetItems(string id, int page, int limit, string search)
        {
            if (search == null) search = "";
            return service.Where(page, limit, (o) =>
            {
                return o.catalog_id != null && o.catalog_id.Equals(id) && o.title.ToLower().Contains(search.ToLower());
            }, o => o.created);
        }
    }
}