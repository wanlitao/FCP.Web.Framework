using System;
using System.Linq.Expressions;

namespace FCP.Web.Api
{
    internal static class TypeActivator
    {
        internal static Func<TBase> Create<TBase>(Type instanceType) where TBase : class
        {
            if (instanceType == null)
                throw new ArgumentNullException(nameof(instanceType));

            var newInstanceExpression = Expression.New(instanceType);
            return Expression.Lambda<Func<TBase>>(newInstanceExpression).Compile();
        }

        internal static Func<TInstance> Create<TInstance>() where TInstance : class
        {
            return Create<TInstance>(typeof(TInstance));
        }

        internal static Func<object> Create(Type instanceType)
        {
            return Create<object>(instanceType);
        }
    }
}
