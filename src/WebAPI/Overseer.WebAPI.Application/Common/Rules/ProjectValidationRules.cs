using FluentValidation;

namespace Overseer.WebAPI.Application.Common.Rules;

internal static class ProjectValidationRules
{
    private const int MaxProjectDescriptionLength = 300;

    internal static IRuleBuilderOptions<T, string> ValidateProjectName<T>(this IRuleBuilder<T, string> ruleBuilder) =>
        ruleBuilder
            .NotEmpty()
            .WithMessage("Project name is required and cannot be empty.");

    internal static IRuleBuilderOptions<T, string?> ValidateProjectDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder) =>
        ruleBuilder
            .MaximumLength(MaxProjectDescriptionLength)
            .WithMessage($"Project description must not exceed {MaxProjectDescriptionLength} characters.");
}
