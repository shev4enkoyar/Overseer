using FluentValidation;
using Overseer.WebAPI.Application.Common.Rules;

namespace Overseer.WebAPI.Application.Projects.Queries.GetProjectsWithPagination;

internal sealed class GetProjectsWithPaginationQueryValidator : AbstractValidator<GetProjectsWithPaginationQuery>
{
    public GetProjectsWithPaginationQueryValidator()
    {
        RuleFor(static x => x.PageNumber)
            .ValidatePageNumber();

        RuleFor(static x => x.PageSize)
            .ValidatePageSize();
    }
}
