using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Apps.Models;
using Apps.Common;
using System.Transactions;
using Apps.Models.Sys;
using Apps.IBLL;
using Apps.IDAL;
using Apps.BLL.Core;
using Apps.Locale;

namespace Apps.BLL
{
    public partial class SysPositionBLL
    {
     
        [Dependency]
        public ISysStructRepository depRep { get; set; }
    
        public List<SysPositionModel> GetPosListByDepId(ref GridPager pager,string depId)
        {

            IQueryable<SysPosition> queryData = null;
            if (!string.IsNullOrWhiteSpace(depId))
            {
                if(depId=="root")
                    queryData = m_Rep.GetList();
                else
                queryData = m_Rep.GetList(a => a.DepId == depId);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
            pager.totalRows = queryData.Count();
            //queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
    }
}
