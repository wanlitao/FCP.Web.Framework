using System.Threading.Tasks;
using System.Web;

namespace FCP.Web
{
    /// <summary>
    /// 登录服务 抽象类
    /// </summary>
    public abstract class AbstractLoginService<TUser, TResult> : UserSessionCookieService<TUser>, ILoginService<TUser, TResult>
        where TUser : class
        where TResult : class
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        protected TUser _currentUser;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sessionName"></param>
        public AbstractLoginService(string sessionName)
            : base(sessionName)
        {

        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <returns></returns>
        public virtual TResult login(UserBase userBase)
        {
            return login(userBase, false);  //默认页面登录 不需要解密 密码
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <param name="isCookieLogin">是否cookie登录</param>
        /// <returns></returns>
        public virtual TResult login(UserBase userBase, bool isCookieLogin)
        {
            TUser entity;
            return login(userBase, isCookieLogin, out entity);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <param name="isCookieLogin">是否cookie登录</param>
        /// <returns></returns>
        protected abstract TResult login(UserBase userBase, bool isCookieLogin, out  TUser entity);

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <returns></returns>
        public virtual async Task<TResult> loginAsync(UserBase userBase, HttpContextBase httpContext)
        {
            return await loginAsync(userBase, httpContext, false).ConfigureAwait(false);  //默认页面登录 不需要解密 密码
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <param name="isCookieLogin">是否cookie登录</param>
        /// <returns></returns>
        public abstract Task<TResult> loginAsync(UserBase userBase, HttpContextBase httpContext, bool isCookieLogin);

        /// <summary>
        /// 退出登录
        /// </summary>
        public virtual void exitLogin()
        {
            base.clear();
        }

        /// <summary>
        /// 转换为UserBase
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected abstract UserBase toUserBase(TUser entity);

        /// <summary>
        /// 通过UserBase获取
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <returns></returns>
        protected virtual TUser getByUserBase(UserBase userBase)
        {
            TUser entity;
            //从cookie中获取用户基本信息时 需对密码 进行解密
            TResult apiResult = login(userBase, true, out entity);

            return entity;
        }

        /// <summary>
        /// 通过UserBase获取
        /// </summary>
        /// <param name="userBase">用户基本信息</param>
        /// <returns></returns>
        protected virtual async Task<TUser> getByUserBaseAsync(UserBase userBase, HttpContextBase httpContext)
        {
            //从cookie中获取用户基本信息时 需对密码 进行解密
            TResult apiResult = await loginAsync(userBase, httpContext, true).ConfigureAwait(false);

            return getSessionModel(httpContext);
        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public virtual TUser getCurrentUser()
        {
            return base.getCurrentModel(getByUserBase);

        }

        /// <summary>
        /// 当前登录用户
        /// </summary>
        public virtual async Task<TUser> getCurrentUserAsync(HttpContextBase httpContext)
        {
            return await base.getCurrentModelAsync(getByUserBaseAsync, httpContext).ConfigureAwait(false);

        }
    }
}