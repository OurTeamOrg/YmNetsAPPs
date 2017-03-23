using System.Collections.Generic;
using System.Linq;
using Apps.Web.Core;
using Apps.Spl.IBLL;
using Apps.Locale;
using System.Web.Mvc;
using Apps.Common;
using Apps.IBLL;
using Apps.Models.Spl;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;
using System;

namespace Apps.Web.Areas.Spl.Controllers
{
    public class PersonController : BaseController
    {
        [Dependency]
        public ISpl_PersonBLL m_BLL { get; set; }
        ValidationErrors errors = new ValidationErrors();

        [SupportFilter]
        public ActionResult Index()
        {
            ViewBag.Perm = GetPermission();
            return View();
        }
        [HttpPost]
        [SupportFilter(ActionName = "Index")]
        public JsonResult GetList(GridPager pager, string queryStr)
        {
            List<Spl_PersonModel> list = m_BLL.GetList(ref pager, queryStr);
            GridRows<Spl_PersonModel> grs = new GridRows<Spl_PersonModel>();
            grs.rows = list;
            grs.total = pager.totalRows;
            return Json(grs);
        }
        #region 创建
        [SupportFilter]
        public ActionResult Create()
        {
            ViewBag.Perm = GetPermission();
            return View();
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Create(Spl_PersonModel model)
        {
            model.Id = ResultHelper.NewId;
            model.CreateTime = ResultHelper.NowTime;
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Create(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "创建", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "创建", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.InsertFail));
            }
        }
        #endregion

        #region 修改
        [SupportFilter]
        public ActionResult Edit(string id)
        {
            ViewBag.Perm = GetPermission();
            Spl_PersonModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        [HttpPost]
        [SupportFilter]
        public JsonResult Edit(Spl_PersonModel model)
        {
            if (model != null && ModelState.IsValid)
            {

                if (m_BLL.Edit(ref errors, model))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name, "成功", "修改", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + model.Id + ",Name" + model.Name + "," + ErrorCol, "失败", "修改", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail));
            }
        }
        #endregion

        #region 详细
        [SupportFilter]
        public ActionResult Details(string id)
        {
            ViewBag.Perm = GetPermission();
            Spl_PersonModel entity = m_BLL.GetById(id);
            return View(entity);
        }

        #endregion

        #region 删除
        [HttpPost]
        [SupportFilter]
        public JsonResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (m_BLL.Delete(ref errors, id))
                {
                    LogHandler.WriteServiceLog(GetUserId(), "Id:" + id, "成功", "删除", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(1, Resource.DeleteSucceed));
                }
                else
                {
                    string ErrorCol = errors.Error;
                    LogHandler.WriteServiceLog(GetUserId(), "Id" + id + "," + ErrorCol, "失败", "删除", "Spl_Person");
                    return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail + ErrorCol));
                }
            }
            else
            {
                return Json(JsonHandler.CreateMessage(0, Resource.DeleteFail));
            }


        }
        #endregion

        #region 导入

        [HttpPost]
        [SupportFilter]
        public ActionResult Import(string filePath)
        {
            var personList = new List<Spl_PersonModel>();
             //校验数据is
            bool checkResult = m_BLL.CheckImportData(Utils.GetMapPath(filePath), personList, ref errors);
             //校验通过直接保存
             if (checkResult)
             {
                 m_BLL.SaveImportData(personList);
                 LogHandler.WriteServiceLog(GetUserId(), "导入成功", "成功", "导入", "Spl_Person");
                 return Json(JsonHandler.CreateMessage(1, Resource.InsertSucceed));
             }
             else
             {
                 string ErrorCol = errors.Error;
                 LogHandler.WriteServiceLog(GetUserId(), ErrorCol, "失败", "导入", "Spl_Person");
                 return Json(JsonHandler.CreateMessage(0, Resource.InsertFail + ErrorCol));
              }
        
        }

        [HttpPost]
        [SupportFilter(ActionName="Export")]
        public JsonResult CheckExportData()
        {
            List<Spl_PersonModel> list = m_BLL.GetList(ref setNoPagerAscById, "");
            if (list.Count().Equals(0))
            {
                return Json(JsonHandler.CreateMessage(0, "没有可以导出的数据"));
            }
            else
            {
                return Json(JsonHandler.CreateMessage(1, "可以导出"));
            }
        }

        [SupportFilter]
        public ActionResult Export()
        {
            var exportSpource = this.GetExportData();
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat(
                "Person",
                DateTime.Now.ToString("yyyyMMddHHmmss"),
                ".xlsx");

            return new ExportExcelResult
            {
                SheetName = "人员列表",
                FileName = exportFileName,
                ExportData = dt
            };
        }

        private JArray GetExportData()
        {
            List<Spl_PersonModel> list = m_BLL.GetList(ref setNoPagerAscById, "");
            JArray jObjects = new JArray();

            foreach (var item in list)
            {
                var jo = new JObject();
                jo.Add("Id", item.Id);
                jo.Add("Name", item.Name);
                jo.Add("Sex", item.Sex);
                jo.Add("Age", item.Age);
                jo.Add("IDCard", item.IDCard);
                jo.Add("Phone", item.Phone);
                jo.Add("Email", item.Email);
                jo.Add("Address", item.Address);
                jo.Add("CreateTime", item.CreateTime);
                jo.Add("Region", item.Region);
                jo.Add("Category", item.Category);
                jObjects.Add(jo);
            }
            return jObjects;
        }
        #endregion
    }
}
