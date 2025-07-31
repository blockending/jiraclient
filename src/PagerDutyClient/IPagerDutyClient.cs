using System.Threading.Tasks;

namespace PagerDutyClient;

public interface IPagerDutyClient
{
    Task<PagerDutyIncidentList?> GetIncidentsAsync();
}
