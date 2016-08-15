using System;
using FCP.Util;
using System.Threading.Tasks;
using System.Web;

namespace FCP.Web
{
    /// <summary>
    /// 用户会话服务
    /// </summary>
    public class UserSessionCookieService<T>
    {
        /// <summary>
        /// session与cookie记录名称
        /// </summary>
        private string _infoCacheName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cacheName"></param>
        public UserSessionCookieService(string cacheName)
        {
            this._infoCacheName = "fcp_" + cacheName;
        }


        /// <summary>
        /// 将对象储存到session和cookie中
        /// </summary>
        /// <param name="model">对象</param>
        /// <param name="isInCookie">是否储存到cookie</param>
        /// <param name="toUserBase">将对象转换为UserBase</param>
        public void saveModelToSessionAndCookie(T model, bool isInCookie,
            Func<T, UserBase> toUserBase)
        {
            saveModelToSessionAndCookie(model, isInCookie, toUserBase, new HttpContextWrapper(HttpContext.Current));
        }

        /// <summary>
        /// 将对象储存到session和cookie中
        /// </summary>
        /// <param name="model">对象</param>
        /// <param name="isInCookie">是否储存到cookie</param>
        /// <param name="toUserBase">将对象转换为UserBase</param>
        public void saveModelToSessionAndCookie(T model, bool isInCookie,
            Func<T, UserBase> toUserBase, HttpContextBase httpContext)
        {
            string SerializeString = SerializerFactory.BinarySerializer.SerializeString(model);
            SessionUtil.saveSession(this._infoCacheName, SerializeString, httpContext);
            if (isInCookie)
            {
                UserBase userBase = toUserBase(model);
                CookieUtil.setCookie(this._infoCacheName, SerializerFactory.BinarySerializer.SerializeString(userBase), httpContext);
            }
        }

        /// <summary>
        /// 获取当前模型
        /// </summary>
        /// <returns></returns>
        public T getCurrentModel(Func<UserBase, T> toUserModel)
        {
            if (this.isSession)
            {
                return sessionModel;
            }
            if (this.isCookie)
            {
                UserBase userBase = cookieModel;
                if (userBase != null && !userBase.ID.isNullOrEmpty())
                {
                    if (toUserModel != null)
                    {
                        return toUserModel(userBase);
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// 获取当前模型
        /// </summary>
        /// <returns></returns>
        public async Task<T> getCurrentModelAsync(Func<UserBase, HttpContextBase, Task<T>> toUserModel, HttpContextBase httpContext)
        {
            if (this.isContextSession(httpContext))
            {
                return getSessionModel(httpContext);
            }
            if (this.isContextCookie(httpContext))
            {
                UserBase userBase = getCookieModel(httpContext);
                if (userBase != null && !userBase.ID.isNullOrEmpty())
                {
                    if (toUserModel != null)
                    {
                        return await toUserModel(userBase, httpContext).ConfigureAwait(false);
                    }
                }
            }
            return default(T);
        }

        /// <summary>
        /// 清空
        /// </summary>
        public void clear()
        {
            SessionUtil.delSession(this._infoCacheName);
            CookieUtil.clear(this._infoCacheName);
        }
        /// <summary>
        /// session对象
        /// </summary>
        public T sessionModel
        {
            get { return SessionUtil.forSession<T>(this._infoCacheName); }
        }

        /// <summary>
        /// session对象
        /// </summary>
        public T getSessionModel(HttpContextBase httpContext)
        {
            return SessionUtil.forSession<T>(this._infoCacheName, httpContext);
        }

        /// <summary>
        /// cookie对象
        /// </summary>
        public UserBase cookieModel
        {
            get { return CookieUtil.forCookie<UserBase>(this._infoCacheName); }
        }

        /// <summary>
        /// cookie对象
        /// </summary>
        public UserBase getCookieModel(HttpContextBase httpContext)
        {
            return CookieUtil.forCookie<UserBase>(this._infoCacheName, httpContext);
        }

        /// <summary>
        /// 获取当前Session状态
        /// </summary>
        /// <returns></returns>
        public bool isSession
        {
            get { return SessionUtil.isSession(this._infoCacheName); }
        }

        /// <summary>
        /// 获取当前Session状态
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public bool isContextSession(HttpContextBase httpContext)
        {
            return SessionUtil.isSession(this._infoCacheName, httpContext);
        }

        /// <summary>
        /// 获取当前Cookie状态
        /// </summary>
        /// <returns></returns>
        public bool isCookie
        {
            get { return CookieUtil.isCookie(this._infoCacheName); }
        }

        /// <summary>
        /// 获取当前Cookie状态
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public bool isContextCookie(HttpContextBase httpContext)
        {
            return CookieUtil.isCookie(this._infoCacheName, httpContext);
        }
    }
}
