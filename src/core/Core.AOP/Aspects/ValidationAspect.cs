using Castle.DynamicProxy;
using Core.AOP.Interceptors;
using Core.CrossCuttingConcerns.Exceptions.ExceptionTypes;
using FluentValidation;
using ValidationException = Core.CrossCuttingConcerns.Exceptions.ExceptionTypes.ValidationException;

namespace Core.AOP.Aspects;


public class ValidationAspect : MethodInterception
{
    private readonly Type _validatorType;

    public ValidationAspect(Type validatorType)
    {
        if (!typeof(IValidator).IsAssignableFrom(validatorType))
        {
            throw new Exception($"The type {validatorType.Name} is not a validator class.");
        }

        _validatorType = validatorType;
    }

    protected override void OnBefore(IInvocation invocation)
    {
        var validator = (IValidator)Activator.CreateInstance(_validatorType);
        var entityType = _validatorType.BaseType?.GetGenericArguments()[0];
        var entities = invocation.Arguments.Where(arg => arg?.GetType() == entityType);

        foreach (var entity in entities)
        {
          
            ValidationContext<object> context = new(entity);
            var result = validator.Validate(context);

            if (!result.IsValid)
            {
              
                var errors = result.Errors
                    .GroupBy(
                        failure => failure.PropertyName,
                        (propertyName, failures) => new ValidationExceptionModel
                        {
                            Property = propertyName,
                            Errors = failures.Select(f => f.ErrorMessage)
                        })
                    .ToList();

                throw new ValidationException(errors);
            }
        }
    }
}