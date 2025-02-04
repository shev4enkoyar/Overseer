using Overseer.WebAPI.Domain.Entities;

namespace Overseer.WebAPI.Application.Common.Interfaces;

public interface IContainerRepository
{
    Result AddContainer(Container container);
}
