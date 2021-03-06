//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using Apps.Models;
using Apps.Common;
using Microsoft.Practices.Unity;
using System.Transactions;
using Apps.BLL.Core;
using Apps.Locale;
using Apps.DEF.IDAL;
using Apps.Models.DEF;
namespace Apps.DEF.BLL
{
	public class Virtual_DEF_TestJobsDetailBLL
	{
        [Dependency]
        public IDEF_TestJobsDetailRepository m_Rep { get; set; }

		public virtual List<DEF_TestJobsDetailModel> GetList(ref GridPager pager, string queryStr)
        {

            IQueryable<DEF_TestJobsDetail> queryData = null;
            if (!string.IsNullOrWhiteSpace(queryStr))
            {
                queryData = m_Rep.GetList(
								a=>a.VerCode.Contains(queryStr)
								|| a.Code.Contains(queryStr)
								|| a.Name.Contains(queryStr)
								|| a.Description.Contains(queryStr)
								
								
								);
            }
            else
            {
                queryData = m_Rep.GetList();
            }
            pager.totalRows = queryData.Count();
            //排序
            queryData = LinqHelper.SortingAndPaging(queryData, pager.sort, pager.order, pager.page, pager.rows);
            return CreateModelList(ref queryData);
        }
        public virtual List<DEF_TestJobsDetailModel> CreateModelList(ref IQueryable<DEF_TestJobsDetail> queryData)
        {

            List<DEF_TestJobsDetailModel> modelList = (from r in queryData
                                              select new DEF_TestJobsDetailModel
                                              {
													VerCode = r.VerCode,
													Code = r.Code,
													Name = r.Name,
													Description = r.Description,
													Result = r.Result,
													Sort = r.Sort,
          
                                              }).ToList();

            return modelList;
        }

        public virtual bool Create(ref ValidationErrors errors, DEF_TestJobsDetailModel model)
        {
            try
            {
			    DEF_TestJobsDetail entity = m_Rep.GetById(model.VerCode);
                if (entity != null)
                {
                    errors.Add(Resource.PrimaryRepeat);
                    return false;
                }
                entity = new DEF_TestJobsDetail(); 
				entity.VerCode = model.VerCode;
				entity.Code = model.Code;
				entity.Name = model.Name;
				entity.Description = model.Description;
				entity.Result = model.Result;
				entity.Sort = model.Sort;
  

                if (m_Rep.Create(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.InsertFail);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }



         public virtual bool Delete(ref ValidationErrors errors, string id)
        {
            try
            {
                if (m_Rep.Delete(id) == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

        public virtual bool Delete(ref ValidationErrors errors, string[] deleteCollection)
        {
            try
            {
                if (deleteCollection != null)
                {
                    using (TransactionScope transactionScope = new TransactionScope())
                    {
                        if (m_Rep.Delete(deleteCollection) == deleteCollection.Length)
                        {
                            transactionScope.Complete();
                            return true;
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

		
       

        public virtual bool Edit(ref ValidationErrors errors, DEF_TestJobsDetailModel model)
        {
            try
            {
                DEF_TestJobsDetail entity = m_Rep.GetById(model.VerCode);
                if (entity == null)
                {
                    errors.Add(Resource.Disable);
                    return false;
                }
                              				entity.VerCode = model.VerCode;
				entity.Code = model.Code;
				entity.Name = model.Name;
				entity.Description = model.Description;
				entity.Result = model.Result;
				entity.Sort = model.Sort;
 


                if (m_Rep.Edit(entity))
                {
                    return true;
                }
                else
                {
                    errors.Add(Resource.NoDataChange);
                    return false;
                }

            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
                ExceptionHander.WriteException(ex);
                return false;
            }
        }

      

        public virtual DEF_TestJobsDetailModel GetById(string id)
        {
            if (IsExists(id))
            {
                DEF_TestJobsDetail entity = m_Rep.GetById(id);
                DEF_TestJobsDetailModel model = new DEF_TestJobsDetailModel();
                              				model.VerCode = entity.VerCode;
				model.Code = entity.Code;
				model.Name = entity.Name;
				model.Description = entity.Description;
				model.Result = entity.Result;
				model.Sort = entity.Sort;
 
                return model;
            }
            else
            {
                return null;
            }
        }

        public virtual bool IsExists(string id)
        {
            return m_Rep.IsExist(id);
        }
		  public void Dispose()
        { 
            
        }

	}
}
