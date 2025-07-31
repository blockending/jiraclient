using System.Threading.Tasks;

namespace MetricsClientSample.Strategies;

public interface IApiClientStrategy
{
    Task RunAsync();
}
