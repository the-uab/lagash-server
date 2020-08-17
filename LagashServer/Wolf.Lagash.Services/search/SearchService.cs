﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Wolf.Core.EntityFramework;
using Wolf.Lagash.Entities.search;
using Wolf.Lagash.Interfaces.search;

namespace Wolf.Lagash.Services.search
{
    public class SearchService : EFAdapterBase<Search>, ISearchService
    {
        public SearchService(DbContext Context) : base(Context)
        {
        }

        public List<Search> SearchItems(string typeSearch, bool isAll, string filter, string listAuthor, string listEditorial, string listYear, int page, int limit)
        {
            //exec dbo.lg_search_data typeSearch/isAll/filter/author/editorial/year/page/pagesize
            var listSearch = context.Database.SqlQuery<Search>(string.Format("exec dbo.lg_search_data '{0}','{1}','{2}', '{3}', '{4}','{5}',{6},{7}", typeSearch, isAll, filter, listAuthor, listEditorial, listYear, page, limit)).ToList(); //from .FromSql($ "RetrieveEmployeeRecord {id}").ToList();
            return listSearch;
        }
    }
}
