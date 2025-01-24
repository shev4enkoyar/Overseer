using FluentValidation;
using Overseer.WebAPI.Application.Common.Interfaces;

namespace Overseer.WebAPI.Application.Common.Extensions;

internal static class PaginationValidationRules
{
    internal static IRuleBuilderOptions<T, int> ValidatePageNumber<T>(this IRuleBuilder<T, int> ruleBuilder)
        where T : IPaginatedQuery => ruleBuilder
        .GreaterThanOrEqualTo(1)
        .WithMessage("PageNumber must be greater than or equal to 1.");

    internal static IRuleBuilderOptions<T, int> ValidatePageSize<T>(this IRuleBuilder<T, int> ruleBuilder)
        where T : IPaginatedQuery => ruleBuilder
        .GreaterThanOrEqualTo(1)
        .WithMessage("PageSize must be greater than or equal to 1.");
}
