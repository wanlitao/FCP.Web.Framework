using System.Threading.Tasks;
using System.Web;

namespace FCP.Web
{
    /// <summary>
    /// 登录服务 接口
    /// </summary>
    public interface ILoginService<TUser, TResult>
        where TUser : class
        where TResult : class
    {
        #region 登录相关
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <returns></returns>
        TResult login(UserBase userBase);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <param name="isCookieLogin">是否Cookie登录</param>
        /// <returns></returns>
        TResult login(UserBase userBase, bool isCookieLogin);


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <returns></returns>
        Task<TResult> loginAsync(UserBase userBase, HttpContextBase httpContext);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <param name="isCookieLogin">是否对密码解密</param>
        /// <returns></returns>
        Task<TResult> loginAsync(UserBase userBase, HttpContextBase httpContext, bool isCookieLogin);

        /// <summary>
        /// 退出登录
        /// </summary>
        void exitLogin();

        /// <summary>
        /// 当前登录用户
        /// </summary>
        TUser getCurrentUser();

        /// <summary>
        /// 当前登录用户
        /// </summary>
        Task<TUser> getCurrentUserAsync(HttpContextBase httpContext);
        #endregion

        #region Session和Cookie
        /// <summary>
        /// session对象
        /// </summary>
        TUser sessionModel { get; }

        TUser getSessionModel(HttpContextBase httpContext);

        /// <summary>
        /// cookie对象
        /// </summary>
        UserBase cookieModel { get; }

        UserBase getCookieModel(HttpContextBase httpContext);

        /// <summary>
        /// 获取当前Session状态
        /// </summary>
        /// <returns></returns>
        bool isSession { get; }

        bool isContextSession(HttpContextBase httpContext);

        /// <summary>
        /// 获取当前Cookie状态
        /// </summary>
        /// <returns></returns>
        bool isCookie { get; }

        bool isContextCookie(HttpContextBase httpContext);
        #endregion
    }
}