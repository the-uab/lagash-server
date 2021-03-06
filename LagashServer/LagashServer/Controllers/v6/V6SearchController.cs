﻿using System;
using System.Data;
using System.Web.Http;
using LagashServer.helper;
using System.Linq;
using Wolf.Lagash.Services.search;
using Wolf.Lagash.Interfaces.search;

namespace LagashServer.Controllers.v6
{
    [RoutePrefix("v6/search")]
    public class V6SearchController : ApiController
    {
        private ISearchService service = new SearchService(new LagashContext());
       
        [Route("page/{page}/limit/{limit}")]
        public IHttpActionResult Get(int page, int limit, string type, string listAuthor, string listDestriptor, string listIndexer, string listEditorial, 
                string listYear, bool isAll, string search)
        {
            try
            {
                var listSearch = service.SearchItems(type, isAll, search, listAuthor, listEditorial, listYear, listDestriptor, listIndexer,  page, limit);

                var years = listSearch.Where(i => !String.IsNullOrEmpty( i.MaterialYear)).Select(i => i.MaterialYear).Distinct();
                var authors = listSearch.Where(i => !String.IsNullOrEmpty(i.Autor)).Select(i => i.Autor).Distinct();
                var editorials = listSearch.Where(i => !String.IsNullOrEmpty(i.Editorial)).Select(i => i.Editorial).Distinct();

                int totalRows = 0;

                if (listSearch?.Count > 0)
                {
                    totalRows = listSearch.FirstOrDefault().Total;
                }
                return Ok(new { totalRows, years, authors, editorials, data = listSearch });
            }
            catch (Exception e)
            {
                return Ok(new { error = true });
            }
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok("running");
        }
    }
}