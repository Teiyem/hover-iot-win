using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Hover.Helpers;

#region Attribution
// Code attribution: Original Author Luke Malpass (Angel Six)
#endregion

/// <summary>
/// A helper for expressions.
/// </summary>
public static class ExpressionHelpers
{
    /// <summary>
    /// Compiles an expression and gets the functions return value
    /// </summary>
    /// <typeparam name="T">The type of return value</typeparam>
    /// <param name="lambda">The expression to compile</param>
    /// <returns></returns>
    public static T GetPropertyValue<T>(this Expression<Func<T>> lambda)
    {
        return lambda.Compile().Invoke();
    }

    /// <summary>
    /// Compiles an expression and gets the functions return value.
    /// </summary>
    /// <typeparam name="T">The type of return value.</typeparam>
    /// <typeparam name="In">The type of input.</typeparam>
    /// <param name="lambda">The expression to compile.</param>
    /// <param name="input">The input to the expression.</param>
    /// <returns></returns>
    public static T GetPropertyValue<In, T>(this Expression<Func<In, T>> lambda, In input)
    {
        return lambda.Compile().Invoke(input);
    }

    /// <summary>
    /// Sets the underlying properties value to the given value
    /// from an expression that contains the property.
    /// </summary>
    /// <typeparam name="T">The type of value to set.</typeparam>
    /// <param name="lambda">The expression.</param>
    /// <param name="value">The value to set the property to.</param>
    public static void SetPropertyValue<T>(this Expression<Func<T>> lambda, T value)
    {
        var expression = lambda.Body as MemberExpression;

        var propertyInfo = (PropertyInfo) expression?.Member;
        if (expression?.Expression == null) return;
        var target = Expression.Lambda(expression.Expression).Compile().DynamicInvoke();

        propertyInfo.SetValue(target, value);
    }


    /// <summary>
    /// Sets the underlying properties value to the given value
    /// from an expression that contains the property.
    /// </summary>
    /// <typeparam name="T">The type of value to set.</typeparam>
    /// <typeparam name="In">The type of input.</typeparam>
    /// <param name="lambda">The expression.</param>
    /// <param name="value">The value to set the property to.</param>
    /// <param name="input">The input to the expression.</param>
    public static void SetPropertyValue<In, T>(this Expression<Func<In, T>> lambda, T value, In input)
    {
        if (lambda.Body is not MemberExpression expression) return;
        var propertyInfo = (PropertyInfo)expression.Member;

        propertyInfo.SetValue(input, value);
    }
}