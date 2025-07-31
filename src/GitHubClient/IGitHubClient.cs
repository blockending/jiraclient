using System.Threading.Tasks;

namespace GitHubClient;

public interface IGitHubClient
{
    Task<GitHubRepo?> GetRepoAsync(string owner, string repo);
}
