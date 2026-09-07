using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class RedisKeepAliveService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public RedisKeepAliveService(IServiceScopeFactory scopeFactory)
        => _scopeFactory = scopeFactory;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();
            var cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            await cache.SetStringAsync("keepalive", DateTime.UtcNow.ToString(), ct);
            await Task.Delay(TimeSpan.FromHours(12), ct);
        }
    }
}