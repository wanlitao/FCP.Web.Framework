using FCP.Util;
using System.Web;

namespace FCP.Web
{
    /// <summary>
    /// Session 核心
    /// </summary>
    public static class SessionUtil
    {
        /// <summary>
        /// 判断Session是否存在
        /// </summary>
        /// <param name="SessionName"></param>
        /// <returns></returns>
        public static bool isSession(string sessionName)
        {
            return isSession(sessionName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 判断Session是否存在
        /// </summary>
        /// <param name="sessionName"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static bool isSession(string sessionName, HttpContextBase httpContext)
        {
            return httpContext.Session[sessionName] == null ? false : true;
        }

        /// <summary>
        /// 读取Session
        /// </summary>
        /// <param name="SessionName"></param>
        /// <returns></returns>
        public static string getSessionValue(string sessionName)
        {
            return getSessionValue(sessionName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 读取Session
        /// </summary>
        /// <param name="sessionName"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static string getSessionValue(string sessionName, HttpContextBase httpContext)
        {
            return isSession(sessionName, httpContext) ? httpContext.Session[sessionName].ToString() : string.Empty;
        }

        /// <summary>
        /// 保存Session
        /// </summary>
        /// <param name="KeyName"></param>
        /// <param name="Value"></param>
        public static void saveSession(string keyName, string Value)
        {

            saveSession(keyName, Value, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 保存Session
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="Value"></param>
        /// <param name="httpContext"></param>
        public static void saveSession(string keyName, string Value, HttpContextBase httpContext)
        {
            httpContext.Session[keyName] = Value;
        }

        /// <summary>
        /// 序列化对象后保存到session
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="obj"></param>
        public static void setSeeion(string keyName, object obj)
        {
            setSeeion(keyName, obj, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 序列化对象后保存到session
        /// </summary>
        /// <param name="keyName"></param>
        /// <param name="obj"></param>
        /// <param name="httpContext"></param>
        public static void setSeeion(string keyName, object obj, HttpContextBase httpContext)
        {
            saveSession(keyName, SerializerUtil.serializeObject(obj), httpContext);
        }

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="SessionName"></param>
        public static void delSession(string SessionName)
        {
            delSession(SessionName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="SessionName"></param>
        /// <param name="httpContext"></param>
        public static void delSession(string SessionName, HttpContextBase httpContext)
        {
            if (isSession(SessionName, httpContext)) httpContext.Session.Remove(SessionName);
        }

        /// <summary>
        /// 获取Session并转换对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="sessionName">session名称</param>
        /// <returns></returns>
        public static T forSession<T>(string sessionName)
        {
            return forSession<T>(sessionName, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 获取Session并转换对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sessionName"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public static T forSession<T>(string sessionName, HttpContextBase httpContext)
        {
            string sessionValues = SessionUtil.getSessionValue(sessionName, httpContext);
            return SerializerUtil.deserializeT<T>(sessionValues);
        }
    }
}