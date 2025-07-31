using System.Threading.Tasks;

namespace JiraClient;

public interface IJiraClient
{
    /// <summary>
    /// Retrieves an issue from Jira.
    /// </summary>
    /// <param name="issueKey">The issue key, e.g. "TEST-1".</param>
    /// <returns>The deserialized <see cref="JiraIssue"/>.</returns>
    Task<JiraIssue> GetIssueAsync(string issueKey);
}
