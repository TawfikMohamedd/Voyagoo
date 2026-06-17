using Voyagoo.Abstractions;
using Voyagoo.Contracts.Home;

namespace Voyagoo.Services
{
    public interface IHomeService
    {
        Task<Result<GetHomeResponse>> GetHomeAsync(CancellationToken cancellationToken = default);
    }
}
