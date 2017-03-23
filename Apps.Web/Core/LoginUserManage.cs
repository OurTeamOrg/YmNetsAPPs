using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using Apps.Models.Sys;
using System.Web.Configuration;
using Apps.Common;
namespace Apps.Web.Core
{
    public static class LoginUserManage
    {
        /// <summary>
        ///  验证用户状态是否改变
        /// </summary>
        /// <param name="sessId">sessionid</param>
        /// <param name="name">session</param>
        /// <returns></returns>
        public static bool IsChange(String sessId, String name)
        {
            Boolean bResult = false;
            NameValueCollection loginUsers = HttpContext.Current.Application["__loginUsers"] as NameValueCollection;
            if (loginUsers != null)
            {
                String oldSessId = loginUsers.GetValues(name)[0];
                if (!String.IsNullOrEmpty(oldSessId) && !sessId.Equals(oldSessId))
                {
                    bResult = true;
                }
            }
            return bResult;
        }
        /// <summary>
        /// 验证是否有单机限制
        /// </summary>
        /// <param name="account">用户信息</param>
        /// <returns>验证结果：true有限制，false没有</returns>
        public static bool ValidateRelogin(AccountModel account)
        {
            bool bResult = false;
            if (account != null)
            {
                SysConfigModel siteConfig = new Apps.BLL.SysConfigBLL().loadConfig(Utils.GetXmlMapPath("Configpath"));
                if (siteConfig.issinglelogin == 1)
                {
                    if (IsChange(HttpContext.Current.Session.SessionID, account.Id))
                    {
                        //同一帐号已经在其他机子登录
                        bResult = true;
                        RedirectUrl();
                    }
                }
            }
            return bResult;
        }
        /// <summary>
        /// 验证是否已经登录
        /// </summary>
        /// <param name="account">account</param>
        /// <returns>验证结果：true已经登录，false没有登录</returns>
        public static bool ValidateIsLogined(AccountModel account)
        {

            if (account == null)
            {
                RedirectUrl();
            }

            return true;
        }
        /// <summary>
        /// 失效（错误）窗口提示
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="url">URL地址，可选参数，为空则只弹出对话框，而不刷新页面</param>
        public static void RedirectUrl()
        {

            string href = HttpContext.Current.Request.Url.ToString();
            HttpContext.Current.Response.Redirect("/Account/index?url=" + href);
            HttpContext.Current.Response.End();
          
        }
    }
}