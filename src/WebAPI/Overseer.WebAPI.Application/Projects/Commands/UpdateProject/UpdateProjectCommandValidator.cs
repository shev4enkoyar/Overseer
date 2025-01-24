using FluentValidation;
using Overseer.WebAPI.Application.Common.Rules;

namespace Overseer.WebAPI.Application.Projects.Commands.UpdateProject;

internal sealed class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(static x => x.Name)
            .ValidateProjectName();

        RuleFor(static x => x.Description)
            .ValidateProjectDescription();
    }
}
