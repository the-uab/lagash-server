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
using Wolf.Lagash.Entities;
using LagashServer.helper;
using Wolf.Lagash.Entities.books;
using Wolf.Lagash.Entities.helper.ejemplar;
using Wolf.Lagash.Entities.helper.author;
using Wolf.Lagash.Services.books;
using Wolf.Lagash.Services.helpers.author;
using Wolf.Lagash.Interfaces.books;
using Wolf.Lagash.Interfaces.helpers.author;

namespace LagashServer.Controllers.v3
{
    [RoutePrefix("v3/browser/books")]
    public class V3BooksController : ApiController
    {
        private IBookService service_books = new BookService(new LagashContext());
        private IBookCatalogService service_catalogs = new BookCatalogService(new LagashContext());
        private IAuthorService service_authors = new AuthorService(new LagashContext());
        private IAuthorMapService service_authors_map = new AuthorMapService(new LagashContext());
        private IBookEjemplarService service_ejemplares = new BookEjemplarService(new LagashContext());
        private IAuthorService service_author = new AuthorService(new LagashContext());
        private IAuthorMapService service_author_map = new AuthorMapService(new LagashContext());

        [Route("{id}")]
        public IHttpActionResult Get(string id)
        {
            Book item = service_books.FindById(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [Route("{id}/ejemplares")]
        public IEnumerable<Ejemplar> GetEjemplares(string id)
        {
            return service_ejemplares.get_asc(o => o.material_id == id && o.enabled == true, o => o.order);
        }

        [Route("page/{page}/limit/{limit}")]
        public IEnumerable<Book> GetPagination(int page, int limit, string type, string search)
        {
            if (search == null) search = "";
            Func<Book, bool> where = null;
            switch (type)
            {
                case "ALL":
                    where = (o) =>
                    {
                        return o.title.ToLower().Contains(search.ToLower()) || o.tags != null && o.tags.ToLower().Contains(search.ToLower());
                    };
                    break;
                case "TITLE":
                    where = (o) =>
                    {
                        return o.title.ToLower().Contains(search.ToLower());
                    };
                    break;
                case "SUBJECT":
                    where = (o) =>
                    {
                        return o.tags != null && o.tags.ToLower().Contains(search.ToLower());
                    };
                    break;
                case "AUTHOR":
                    return find_by_autors(page, limit, search);
                default:
                    Console.WriteLine("Default case");
                    break;
            }
            return service_books.search(page, limit, where);
        }

        private IEnumerable<Book> find_by_autors(int page, int limit, string search)
        {
            List<AuthorMap> list = new List<AuthorMap>();
            List<Author> authores = service_author.get_desc((o) =>
            {
                return o.first_name.ToLower().Contains(search.ToLower()) || o.last_name.ToLower().Contains(search.ToLower());
            }, o => o.created).ToList();
            authores.ForEach((author) =>
            {
                list.AddRange(service_author_map.get_desc(o => o.author_id == author._id, o => o.created).ToList());
            });
            List<Book> items = new List<Book>();
            int index = (page - 1) * limit;
            for (int i = index; i < page * limit; i++)
            {
                AuthorMap map = list.ElementAtOrDefault(i);
                if (map != null)
                {
                    items.Add(service_books.FindById(map.material_id));
                }
            }
            return items;
        }

        [Route("catalogs/page/{page}/limit/{limit}")]
        public IEnumerable<BookCatalog> GetCatalogs(int page, int limit)
        {
            return service_catalogs.Where(page, limit, (o) =>
            {
                return o.enabled == true;
            }, o => o.created);
        }

        [Route("catalogs/{id}")]
        public IEnumerable<Book> GetCatalogs(string id)
        {
            return service_books.get_desc(o => o.catalog_id == id, o => o.created);
        }

        [Route("{id}/authors")]
        public IEnumerable<Author> GetAuthors(string id)
        {
            IEnumerable<AuthorMap> items = service_authors_map.Query(o => o.material_id == id);
            List<Author> result = new List<Author>();
            foreach (var item in items)
            {
                Author author = service_authors.FindById(item.author_id);
                author.map = item;
                result.Add(author);
            }
            return result;
        }
    }
}