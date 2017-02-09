using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Twilio.OwlFinance.Domain.Extensions
{
    public static class DynamicExtensions
    {
        public static T FromDynamic<T>(this IDictionary<string, object> dictionary)
        {
            var bindings = new List<MemberBinding>();
            var properties = typeof(T)
                .GetProperties()
                .Where(p => p.CanWrite);

            foreach (var sourceProperty in properties)
            {
                var key = dictionary.Keys
                    .SingleOrDefault(k => k.Equals(sourceProperty.Name, StringComparison.OrdinalIgnoreCase));
                if (!string.IsNullOrEmpty(key))
                {
                    bindings.Add(Expression.Bind(sourceProperty, Expression.Constant(dictionary[key])));
                }
            }

            var newExpression = Expression.New(typeof(T));
            var initExpression = Expression.MemberInit(newExpression, bindings);
            var obj = Expression
                .Lambda<Func<T>>(initExpression)
                .Compile()
                .Invoke();
            return obj;
        }

        public static dynamic ToDynamic<T>(this T value)
        {
            var expando = new ExpandoObject() as IDictionary<string, object>;
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                expando.Add(property.Name, property.GetValue(value));
            }

            return expando;
        }
    }
}
