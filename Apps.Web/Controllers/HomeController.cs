using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Apps.Models.Sys;
using Apps.Models;
using Microsoft.Practices.Unity;
using Apps.IBLL;
using Apps.Common;
using System.Globalization;
using System.Threading;
using System.Text;
using System;
using Apps.Web.Core;
using Apps.Locale;
using Apps.Core.OnlineStat;
namespace Apps.Web.Controllers
{
    public class HomeController : BaseController
    {
        #region UI框架
        [Dependency]
        public IHomeBLL homeBLL { get; set; }
        [Dependency]
        public ISysModuleBLL m_BLL { get; set; }
        private SysConfigModel siteConfig = new Apps.BLL.SysConfigBLL().loadConfig(Utils.GetXmlMapPath("Configpath"));
        ValidationErrors errors = new ValidationErrors();
        [Dependency]
        public ISysUserConfigBLL userConfigBLL { get; set; }

        public ActionResult Index()
        {
            if (Session["Account"] != null)
            {
                //获取是否开启WEBIM
                ViewBag.IsEnable = siteConfig.webimstatus;
                //获取信息间隔时间
                ViewBag.NewMesTime = siteConfig.refreshnewmessage;
                //系统名称
                ViewBag.WebName = siteConfig.webname;
                //公司名称
                ViewBag.ComName = siteConfig.webcompany;
                //版权
                ViewBag.CopyRight = siteConfig.webcopyright;
                //在线人数
                //OnlineUserRecorder recorder = HttpContext.Cache[OnlineHttpModule.g_onlineUserRecorderCacheKey] as OnlineUserRecorder;
                ViewBag.OnlineCount = 100;// recorder.GetUserList().Count;
                AccountModel account = new AccountModel();
                account = (AccountModel)Session["Account"];
                return View(account);
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }


        }
        /// <summary>
        /// 获取导航菜单
        /// </summary>
        /// <param name="id">所属</param>
        /// <returns>树</returns>
        public JsonResult GetTreeByEasyui(string id)
        {
            //加入本地化
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            string infoName = info.Name;
            if (Session["Account"] != null)
            {
                //加入本地化
                AccountModel account = (AccountModel)Session["Account"];
                List<SysModuleModel> list = homeBLL.GetMenuByPersonId(account.Id, id);
                var json = from r in list
                           select new SysModuleNavModel()
                           {
                               id = r.Id,
                               text = infoName.IndexOf("zh") > -1 || infoName == "" ? r.Name : r.EnglishName,     //text
                               attributes = (infoName.IndexOf("zh") > -1 || infoName == "" ? "zh-CN" : "en-US") + "/" + r.Url,
                               iconCls = r.Iconic,
                               state = (m_BLL.GetList(r.Id).Count > 0) ? "closed" : "open"

                           };


                return Json(json);
            }
            else
            {
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SetThemes(string theme, string menu)
        {
            SysUserConfigModel entity = userConfigBLL.GetById("themes", GetUserId());
            if (entity != null)
            {
                entity.Value = theme;
                userConfigBLL.Edit(ref errors, entity);
            }
            else
            {
                entity = new SysUserConfigModel()
                {
                    Id = "themes",
                    Name = "用户自定义主题",
                    Value = theme,
                    Type = "themes",
                    State = true,
                    UserId = GetUserId()
                };
                userConfigBLL.Create(ref errors, entity);

            }
            Session["themes"] = theme;


            SysUserConfigModel entityMenu = userConfigBLL.GetById("menu", GetUserId());
            if (entityMenu != null)
            {
                entityMenu.Value = menu;
                userConfigBLL.Edit(ref errors, entityMenu);
            }
            else
            {
                entityMenu = new SysUserConfigModel()
                {
                    Id = "menu",
                    Name = "用户自定义菜单",
                    Value = menu,
                    Type = "menu",
                    State = true,
                    UserId = GetUserId()
                };
                userConfigBLL.Create(ref errors, entityMenu);

            }

            Session["menu"] = menu;
            return Json("1", JsonRequestBehavior.AllowGet);
        }


        public ActionResult TopInfo()
        {
            if (Session["Account"] != null)
            {
                AccountModel account = new AccountModel();
                account = (AccountModel)Session["Account"];
                return View(account);
            }
            return View();
        }

        #endregion

        #region js配置

        public JavaScriptResult ConfigJs()
        {
            CultureInfo info = Thread.CurrentThread.CurrentCulture;
            StringBuilder sb = new StringBuilder();
            sb.Append("var globalConfig = {");
            sb.Append("    InitConfig: function () {");
            sb.Append("       var Config = {};");
            sb.AppendFormat("       Config.CurrentCulture = \"{0}\";", info.Name);
            sb.Append("       return Config;}};");
            sb.Append("var _globalConfig = globalConfig.InitConfig();");
            return JavaScript(sb.ToString());
        }

        #endregion


        [Dependency]
        public ISysUserBLL userBLL { get; set; }
        [Dependency]
        public ISysStructBLL structBLL { get; set; }
        [Dependency]
        public ISysAreasBLL areasBLL { get; set; }
        [Dependency]
        public ISysUserBLL sysUserBLL { get; set; }
        [Dependency]
        public IAccountBLL accountBLL { get; set; }
        #region 我的资料
        public ActionResult Info()
        {
            SysUserModel entity = sysUserBLL.GetById(GetUserId());
            //防止读取错误
            string CityName, ProvinceName, VillageName, DepName, PosName;
            try
            {
                CityName = !string.IsNullOrEmpty(entity.City) ? areasBLL.GetById(entity.City).Name : "";
                ProvinceName = !string.IsNullOrEmpty(entity.Province) ? areasBLL.GetById(entity.Province).Name : "";
                VillageName = !string.IsNullOrEmpty(entity.Village) ? areasBLL.GetById(entity.Village).Name : "";
                DepName = !string.IsNullOrEmpty(entity.DepId) ? structBLL.GetById(entity.DepId).Name : "";
                PosName = !string.IsNullOrEmpty(entity.PosId) ? structBLL.GetById(entity.PosId).Name : "";
            }
            catch
            {
                CityName = "";
                ProvinceName = "";
                VillageName = "";
                DepName = "";
                PosName = "";
            }
            SysUserEditModel info = new SysUserEditModel()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                TrueName = entity.TrueName,
                Card = entity.Card,
                MobileNumber = entity.MobileNumber,
                PhoneNumber = entity.PhoneNumber,
                QQ = entity.QQ,
                EmailAddress = entity.EmailAddress,
                OtherContact = entity.OtherContact,
                Province = entity.Province,
                City = entity.City,
                Village = entity.Village,
                Address = entity.Address,
                State = entity.State,
                CreateTime = entity.CreateTime,
                CreatePerson = entity.CreatePerson,
                Sex = entity.Sex,
                Birthday = ResultHelper.DateTimeConvertString(entity.Birthday),
                JoinDate = ResultHelper.DateTimeConvertString(entity.JoinDate),
                Marital = entity.Marital,
                Political = entity.Political,
                Nationality = entity.Nationality,
                Native = entity.Native,
                School = entity.School,
                Professional = entity.Professional,
                Degree = entity.Degree,
                DepId = entity.DepId,
                PosId = entity.PosId,
                Expertise = entity.Expertise,
                JobState = entity.JobState,
                Photo = entity.Photo,
                Attach = entity.Attach,
                RoleName = userBLL.GetRefSysRole(GetUserId()),
                CityName = CityName,
                ProvinceName = ProvinceName,
                VillageName = VillageName,
                DepName = DepName,
                PosName = PosName
            };
            return View(info);
        }


        [HttpPost]
        public JsonResult EditPwd(string oldPwd, string newPwd)
        {
            SysUser user = accountBLL.Login(GetUserId(), ValueConvert.MD5(oldPwd));
            if (user == null)
            {
                return Json(JsonHandler.CreateMessage(0, "旧密码不匹配！"), JsonRequestBehavior.AllowGet);
            }
            SysUserEditModel editModel = new SysUserEditModel();
            editModel.Id = GetUserId();
            editModel.Password = ValueConvert.MD5(newPwd);

            if (userBLL.EditPwd(ref errors, editModel))
            {
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + GetUserId() + ",密码:********", "成功", "初始化密码", "用户设置");
                return Json(JsonHandler.CreateMessage(1, Resource.EditSucceed), JsonRequestBehavior.AllowGet);
            }
            else
            {
                string ErrorCol = errors.Error;
                LogHandler.WriteServiceLog(GetUserId(), "Id:" + GetUserId() + ",,密码:********" + ErrorCol, "失败", "初始化密码", "用户设置");
                return Json(JsonHandler.CreateMessage(0, Resource.EditFail + ":"+ErrorCol), JsonRequestBehavior.AllowGet);
            }
        }
        #endregion


        #region webpart
        [Dependency]
        public IWebpartBLL webPartBLL { get; set; }


        public ActionResult Desktop()
        {
            SysUserConfig ss = webPartBLL.GetByIdAndUserId("webpart", GetUserId());
            if (ss != null)
            {
                ViewBag.Value = ss.Value;
            }
            else
            {
                ViewBag.Value = "";
            }
            return View();
        }
        [HttpPost]
        public JsonResult GetPartData1()
        {

            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartData2()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartData3()
        {

            return Json("环境： VS2013+SQL2012版本，兼容 VS2012版本以上", JsonRequestBehavior.AllowGet);
            ////获取条数
            //int rows = 5;
            //SysSettings set = settingBLL.GetById("WP0001");
            //if (set != null)
            //{
            //    try
            //    {
            //        rows =int.Parse(set.Parameter);
            //    }
            //    catch {
            //        rows = 5;
            //    }
            //}
            //List<P_MIS_GetInfo_Result> list = webPartBLL.GetPartData3(rows, GetUserId());
            //StringBuilder sb = new StringBuilder("");
            //sb.Append("<table style=\"width:100%\">");
            //foreach (var i in list)
            //{
            //    sb.AppendFormat("<tr><td class=\"infolist-icon\"><a class=\"a-default\" href=\"javascript:ShowInfo('{0}','{1}')\">{2}</a></td><td style=\"width:75px\">[{3}]</td></tr>", i.Title, i.Id, i.Title, i.CreateTime.ToShortDateString());
            //}
            //sb.Append("</table>");
            //return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartData4()
        {
            return Json("<span style='color:#b200ff'>语言版本进行大部分翻译，其他未翻译部分需要自行翻译<span>", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartData5()
        {
            return Json("", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartData6()
        {

            // 获取在线用户记录器
            OnlineUserRecorder recorder = HttpContext.Cache[OnlineHttpModule.g_onlineUserRecorderCacheKey] as OnlineUserRecorder;
            StringBuilder sb = new StringBuilder("");
            if (recorder == null)
                return Json("在线用户", JsonRequestBehavior.AllowGet);

            //// 绑定在线用户列表
            IList<OnlineUser> userList = recorder.GetUserList();
            sb.AppendFormat("在线用户：<br>");
            foreach (var OnlineUser in userList)
            {
                sb.AppendFormat(OnlineUser.UserName + "<br>");
            }
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartData7()
        {
            return Json("<span style='color:#ff6a00'></span>", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartData8()
        {
            //List<P_Mis_FileGetMyReadFile_Result> list = webPartBLL.GetPartData8(5, GetCurrentId());
            StringBuilder sb = new StringBuilder("");
            //sb.Append("<table style=\"width:100%\">");
            //foreach (var i in list)
            //{
            //    sb.AppendFormat("<tr><td class=\"sharelist-icon\"><a href=\"javascript:ShowFile('{0}','{1}')\">{2}</a></td><td style=\"width:75px\">[{3}]</td><td style='width:30px;'>{4}</td></tr>", "文件查看", "/Mis/File/File?id=" + i.Id, i.Name, Convert.ToDateTime(i.CreateTime).ToShortDateString(), " <a  class='a-default'  href=\"/Mis/File/DownFile/" + i.Id + "\">下载</a>");
            //}
            //sb.Append("</table>");
            return Json(sb.ToString(), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPartData9()
        {
            return Json("备忘录", JsonRequestBehavior.AllowGet);
        }
        ValidationErrors validationErrors = new ValidationErrors();
        [ValidateInput(false)]
        public JsonResult SaveHtml(string html)
        {
            webPartBLL.SaveHtml(ref validationErrors, GetUserId(), html);
            return Json("保存成功!", JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}