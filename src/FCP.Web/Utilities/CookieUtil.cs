using FCP.Util;
using System;
using System.Web;

namespace FCP.Web
{
    /// <summary>
    /// Cookie 核心
    /// </summary>
    public static class CookieUtil
    {
        /// <summary>
        /// 判断Cookie是否存在
        /// </summary>
        /// <param name="CookieName"></param>
        /// <returns></returns>
        public static bool isCookie(string cookieName)
        {
            return isCookie(cookieName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 判断Cookie是否存在
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool isCookie(string cookieName, HttpContextBase httpContext)
        {
            return httpContext.Request.Cookies[cookieName] == null ? false : true;
        }

        /// <summary>
        /// 读取Cookie
        /// </summary>
        /// <param name="CookName">要读取的Cookie名称</param>
        /// <returns></returns>
        public static HttpCookie getCookie(string cookieName)
        {
            return getCookie(cookieName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 读取Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static HttpCookie getCookie(string cookieName, HttpContextBase httpContext)
        {
            return httpContext.Request.Cookies[cookieName];
        }


        /// <summary>
        /// 读取Cookie值
        /// </summary>
        /// <param name="CookieName"></param>
        /// <returns></returns>
        public static string get(string cookieName)
        {
            return get(cookieName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 读取Cookie值
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string get(string cookieName, HttpContextBase httpContext)
        {
            HttpCookie myCookie = getCookie(cookieName, httpContext);
            return (myCookie != null) ? myCookie.Value : string.Empty;
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        public static void clear(string cookieName)
        {
            clear(cookieName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 清除Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="httpContext"></param>
        public static void clear(string cookieName, HttpContextBase httpContext)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            myCookie.Expires = new DateTime(0x7bf, 5, 0x15);
            saveCookie(myCookie, httpContext);
        }

        /// <summary>   
        /// 保存Cookie
        /// </summary>   
        /// <param name="CookieName">Cookie名称</param>   
        /// <param name="CookieValue">Cookie值</param>   
        /// <param name="CookieTime">Cookie过期时间(小时),0为关闭页面失效</param>
        public static void saveCookie(string cookieName, string cookieValue, int cookieTime)
        {
            saveCookie(cookieName, cookieValue, cookieTime, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>   
        /// 保存Cookie
        /// </summary>   
        /// <param name="CookieName">Cookie名称</param>   
        /// <param name="CookieValue">Cookie值</param>   
        /// <param name="CookieTime">Cookie过期时间(小时),0为关闭页面失效</param>
        public static void saveCookie(string cookieName, string cookieValue, int cookieTime, HttpContextBase httpContext)
        {
            HttpCookie myCookie = new HttpCookie(cookieName);
            myCookie.Value = cookieValue;
            if (cookieTime != 0)
            {
                myCookie.Expires = DateTime.Now.AddHours(cookieTime);
            }
            saveCookie(myCookie, httpContext);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="myCookie"></param>
        public static void saveCookie(HttpCookie myCookie)
        {
            saveCookie(myCookie, new HttpContextWrapper(HttpContext.Current));
        }


        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="myCookie"></param>
        /// <param name="httpContext"></param>
        public static void saveCookie(HttpCookie myCookie, HttpContextBase httpContext)
        {
            if (getCookie(myCookie.Name, httpContext) != null)
            {
                httpContext.Response.Cookies.Remove(myCookie.Name); //myCookie.Path = "/";
            }
            httpContext.Response.Cookies.Add(myCookie);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="serializeString">序列化字符串</param>
        public static void setCookie(string cookieName, string serializeString)
        {
            setCookie(cookieName, serializeString, 0, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="serializeString"></param>
        /// <param name="httpContext"></param>
        public static void setCookie(string cookieName, string serializeString, HttpContextBase httpContext)
        {
            setCookie(cookieName, serializeString, 0, httpContext);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="serializeString">序列化字符串</param>
        /// <param name="cookHourTime">过期时间 小时</param>
        public static void setCookie(string cookieName, string serializeString, int cookHourTime)
        {
            setCookie(cookieName, serializeString, cookHourTime, EncryptType.Des, new HttpContextWrapper(HttpContext.Current));
        }


        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="serializeString">序列化字符串</param>
        /// <param name="cookHourTime">过期时间 小时</param>
        public static void setCookie(string cookieName, string serializeString, int cookHourTime, HttpContextBase httpContext)
        {
            setCookie(cookieName, serializeString, cookHourTime, EncryptType.Des, httpContext);
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieName">Cookie名称</param>
        /// <param name="serializeString">序列化字符串</param>
        /// <param name="cookHourTime">过期时间 小时</param>
        /// <param name="encryptType">加密方式</param>
        public static void setCookie(string cookieName, string serializeString, int cookHourTime, EncryptType encryptType)
        {
            setCookie(cookieName, serializeString, cookHourTime, encryptType, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 设置Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="serializeString"></param>
        /// <param name="cookHourTime"></param>
        /// <param name="encryptType"></param>
        /// <param name="httpContext"></param>
        public static void setCookie(string cookieName, string serializeString, int cookHourTime, EncryptType encryptType, HttpContextBase httpContext)
        {
            string enString = EncryptHelper.encrypt(serializeString, encryptType);
            saveCookie(cookieName, enString, cookHourTime, httpContext);
        }

        /// <summary>
        /// 获取Cookie并转换对象 使用DES加密方式
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="cookieName">cookie名称</param>
        /// <returns></returns>
        public static T forCookie<T>(string cookieName)
        {
            return forCookie<T>(cookieName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 获取Cookie并转换对象 使用DES加密方式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cookieName"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static T forCookie<T>(string cookieName, HttpContextBase httpContext)
        {
            return forCookie<T>(cookieName, EncryptType.Des, httpContext);
        }

        /// <summary>
        /// 获取Cookie并转换对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="encryptType">加密类型</param>
        /// <returns></returns>
        public static T forCookie<T>(string cookieName, EncryptType encryptType)
        {
            return forCookie<T>(cookieName, encryptType, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 获取Cookie并转换对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cookieName"></param>
        /// <param name="encryptType"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static T forCookie<T>(string cookieName, EncryptType encryptType, HttpContextBase httpContext)
        {
            string cookieValues = get(cookieName, httpContext);
            if (cookieValues.isNullOrEmpty())
            {
                return default(T);
            }
            cookieValues = EncryptHelper.decrypt(cookieValues, encryptType);
            return SerializerFactory.BinarySerializer.DeserializeString<T>(cookieValues);
        }
    }
}