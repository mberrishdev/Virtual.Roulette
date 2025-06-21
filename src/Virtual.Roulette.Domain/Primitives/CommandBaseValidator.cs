using Virtual.Roulette.Domain.Validators;

namespace Virtual.Roulette.Domain.Primitives;

public class CommandBaseValidator : ICommandBase
{
    public void Validate()
    {
        if (GetType()
                .GetCustomAttributes(typeof(CommandValidationAttribute), true)
                .FirstOrDefault() is not CommandValidationAttribute commandValidationAttribute)
            return;

        var type = commandValidationAttribute.ValidatorType;
        CommandValidator.Validate(type, this);
    }
}