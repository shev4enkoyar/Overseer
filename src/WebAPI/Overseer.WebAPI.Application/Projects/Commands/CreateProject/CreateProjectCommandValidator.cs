using FluentValidation;
using Overseer.WebAPI.Application.Common.Rules;

namespace Overseer.WebAPI.Application.Projects.Commands.CreateProject;

internal sealed class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(static x => x.Name)
            .ValidateProjectName();

        RuleFor(static x => x.Description)
            .ValidateProjectDescription();
    }
}
