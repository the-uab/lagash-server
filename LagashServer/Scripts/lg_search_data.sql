--/////////////////////////////////////////////////////////////////////////////////////////////////
-- drop procedure dbo.lg_search_data
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[lg_search_data]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[lg_search_data]
GO

CREATE PROCEDURE [dbo].[lg_search_data]
	@typeSearch as varchar(50) ='THESES',
	@isAll as Bit = 1,
	@filter as varchar(100)= '',
	@autorList as nvarchar(max),
	@editorialList as nvarchar(max),
	@yearList as nvarchar(max),
	/********   pager  ****/
    @page			int =1,
    @numRows			int =10
    /******** end pager****/
AS
BEGIN
declare @startRowIndex int,
		@total int
 SET @startRowIndex = ((@page - 1) * @numRows) + 1;		


declare @list table(
	Id    VARCHAR(36),
    Title VARCHAR(150),
	Autor VARCHAR(150),
	AutorCode varchar(150),
	Editorial varchar(150),
	MaterialCode varchar(150),
	MaterialYear varchar(150),
	Image VARCHAR(100),
	RowNumber int identity
);

if(@isAll = 1 or @typeSearch='BOOKS')
begin
	insert into @list
	select 
		b._id,
		b.title Title,
		a.first_name +' - ' + a.last_name as autor,
		b.code_author,
		e.name,
		b.code_material,
		b.year,
		b.image Image 
	from Books b 
	left join AuthorMaps am on b._id = am.material_id
	left join Authors a on a._id = am.author_id
	left join Editorials e on b.editorial_id = e._id

	where 
		(
		title like ('%'+ @filter +'%') 
		or a.first_name like ('%'+ @filter +'%'))
		and (
			(@yearList ='' and b.year LIKE CAST(@filter AS NVARCHAR) + '%' ) or
			(@yearList !='' and b.year in( select value from STRING_SPLIT (@yearList, ',')))
		)
		and (
			(@editorialList ='' ) or
			(@editorialList !='' and e.name in( select value from STRING_SPLIT (@editorialList, ',')))
		)
		
		and (
			(@autorList = '' ) or
			(@autorList !='' and a.first_name in( select value from STRING_SPLIT (@autorList, ',')))
		)
end

if(@isAll = 1 or @typeSearch='THESES')
begin
	insert into @list
	select 
		t._id,
		t.title Title, 
		a.first_name +' - ' + a.last_name as autor,
		t.code_author,
		e.name,
		t.code_material,
		t.year,
		t.image Image
	from Theses t 
	left join AuthorMaps am on t._id = am.material_id
	left join Authors a on a._id = am.author_id
	left join Editorials e on t.editorial_id = e._id

	where (title like ('%'+ @filter +'%') 
		or a.first_name like ('%'+ @filter +'%'))
		and (
			(@yearList ='' and t.year LIKE CAST(@filter AS NVARCHAR) + '%' ) or
			(@yearList !='' and t.year in( select value from STRING_SPLIT (@yearList, ',')))
		)
		and (
			(@editorialList ='' ) or
			(@editorialList !='' and e.name in( select value from STRING_SPLIT (@editorialList, ',')))
		)
		
		and (
			(@autorList = '' ) or
			(@autorList !='' and a.first_name in( select value from STRING_SPLIT (@autorList, ',')))
		)
end

if(@isAll = 1 or @typeSearch='MAGAZINES')
begin
	insert into @list
	select 
		m._id,
		m.title Title,
		'' as autor, 
		m.code_author,
		e.name,
		m.code_material,
		m.year,
		m.image Image 
	from Magazines m
	left join Editorials e on m.editorial_id = e._id

	where m.title like ('%'+ @filter +'%') 
	and (
			(@yearList ='' and m.year LIKE CAST(@filter AS NVARCHAR) + '%' ) or
			(@yearList !='' and m.year in( select value from STRING_SPLIT (@yearList, ',')))
		)
		and (
			(@editorialList ='' ) or
			(@editorialList !='' and e.name in( select value from STRING_SPLIT (@editorialList, ',')))
		)

end

if(@isAll = 1 or @typeSearch='NEWSPAPERS')
begin
	insert into @list
	select 
		e._id,
		n.title Title,
		a.first_name +' - ' + a.last_name as autor, 
		n.code_author,
		e.name,
		n.code_material,
		n.year,
		e.image Image
	from Newspapers n
	left join AuthorMaps am on n._id = am.material_id
	left join Authors a on a._id = am.author_id
	left join Editorials e on n.editorial_id = e._id
	where n.title like ('%'+ @filter +'%') 
	and (
			(@yearList ='' and n.year LIKE CAST(@filter AS NVARCHAR) + '%' ) or
			(@yearList !='' and n.year in( select value from STRING_SPLIT (@yearList, ',')))
		)
		and (
			(@editorialList ='' ) or
			(@editorialList !='' and e.name in( select value from STRING_SPLIT (@editorialList, ',')))
		)
end

set @total = (select count(*) from @list)
select  *, @total Total from @list where RowNumber BETWEEN @startRowIndex AND @startRowIndex + @numRows - 1 

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
-- 
--exec dbo.lg_search_data typeSearch/isAll/filter/author/editorial/year/page/pagesize     
--exec dbo.lg_search_data '', 1, '','ronald','','', 1, 10
