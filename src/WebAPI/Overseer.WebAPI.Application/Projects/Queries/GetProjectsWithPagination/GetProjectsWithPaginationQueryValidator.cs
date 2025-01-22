using FluentValidation;

namespace Overseer.WebAPI.Application.Projects.Queries.GetProjectsWithPagination;

public class GetProjectsWithPaginationQueryValidator : AbstractValidator<GetProjectsWithPaginationQuery>
{
    public GetProjectsWithPaginationQueryValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");
        
        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
    }
}