using System.Threading.Tasks;

namespace JiraClient;

public interface IJiraClient
{
    Task<string> GetIssueAsync(string issueKey);
}
