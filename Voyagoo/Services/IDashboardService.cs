using Voyagoo.Abstractions;
using Voyagoo.Contracts.Dashboard;

namespace Voyagoo.Services
{
    public interface IDashboardService
    {
        Task<Result<GetDashboardResponse>> GetDashboardAsync(CancellationToken cancellationToken = default);
    }
}
