using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using Domain.Extensions;
using ErrorOr;

namespace Domain.Primitives;

/// <summary>
/// Value object base class
/// </summary>
/// <typeparam name="TValue"></typeparam>
/// <typeparam name="TValueObject"></typeparam>
[SuppressMessage("Performance", "CA1810:Initialize reference type static fields inline")]
[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
public abstract class ValueObject<TValue, TValueObject> where TValueObject : ValueObject<TValue, TValueObject>, new()
{
    private static readonly Func<TValueObject> Factory;

    static ValueObject()
    {
        var constructorInfo = typeof(TValueObject)
            .GetTypeInfo()
            .DeclaredConstructors
            .First();

        var expressions = Array.Empty<Expression>();
        var newExpression = Expression.New(constructor: constructorInfo, arguments: expressions);
        var lambdaExpression = Expression.Lambda(delegateType: typeof(Func<TValueObject>), body: newExpression);

        Factory = (Func<TValueObject>) lambdaExpression.Compile();
    }

    public TValue Value { get; set; }

    public static TValueObject From(TValue item)
    {
        var x = Factory();
        x.Value = item;
        x.Validate();

        return x;
    }
    
    public static bool operator ==(ValueObject<TValue, TValueObject> a, ValueObject<TValue, TValueObject> b)
    {
        if (a is null && b is null)
            return true;

        if (a is null || b is null)
            return false;

        return a.Equals(obj: b);
    }

    public static bool operator !=(ValueObject<TValue, TValueObject> a, ValueObject<TValue, TValueObject> b)
    {
        return !(a == b);
    }
 
    /// <summary>
    /// Try to create a value object from a value return true if valid and the object.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="thisValue"></param>
    /// <returns></returns>
    public static bool TryFrom(TValue item, out TValueObject thisValue)
    {
        var x = Factory();
        x.Value = item;

        thisValue = x.TryValidate()
            ? x
            : null;

        return thisValue != null;
    }
    /// <summary>
    /// Compare the value object with another object
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is null)
            return false;

        if (ReferenceEquals(objA: this, objB: obj))
            return true;

        return obj.GetType() == GetType() && Equals(other: (ValueObject<TValue, TValueObject>) obj);
    }

    /// <summary>
    /// calculates the hash code based on the value
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
        return EqualityComparer<TValue>.Default.GetHashCode(obj: Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }

    protected virtual bool Equals(ValueObject<TValue, TValueObject> other)
    {
        return EqualityComparer<TValue>.Default.Equals(x: Value, y: other.Value);
    }
    /// <summary>
    /// try to validate the value object return true if valid.
    /// </summary>
    /// <returns></returns>
    protected virtual bool TryValidate()
    {
        try
        {
            this.Validate();
        }
        catch (Exception e)
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// Validate the value object throw exception if invalid.
    /// </summary>
    protected virtual void Validate()
    {
    }
/// <summary>
/// Extending the ValueObject with ErrorOr function 
/// </summary>
/// <param name="value">The wrapped object</param>
/// <returns>ErrorOr wrapped value if validated successfully or ErrorOr error with exception details</returns>
    public static ErrorOr<TValue> MakeErrorOr(TValue value)
    {
        try
        {
            var x = Factory();
            x.Value = value;
            x.Validate();
        }
        catch (Exception e)
        {
            return e.AsErrorType();
        }

        return value.ToErrorOr();

    }
/// <summary>
/// Extending the ValueObject with ErrorOr function for chaining in a functional way
/// </summary>
/// <param name="eo">The ErrorOr object from the previous operation</param>
/// <returns>Either the same object if valid or and ErrorOr error with exception details</returns>
    public static ErrorOr<TValue> TestErrorOr(ErrorOr<TValue> eo)
    {
        
        try
        {
            var x = Factory();
            x.Value = eo.Value;
            x.Validate();
        }
        catch (Exception e)
        {
            return e.AsErrorType();
        }

        return eo;
    }
    
/// <summary>
/// Extending the ValueObject with ErrorOr function for chaining in a functional way
/// </summary>
/// <param name="eo">The ErrorOr object from the previous operation</param>
/// <returns>Either the same object if valid or and ErrorOr error with exception details</returns>
    public static ErrorOr<Success> TestErrorOrSuccess(TValue value)
    {
        
        try
        {
            var x = Factory();
            x.Value = value;
            x.Validate();
        }
        catch (Exception e)
        {
            return e.AsErrorType();
        }

        return Result.Success;
    }
}