namespace GitHubClient;

public class GitHubOptions
{
    public string BaseUrl { get; set; } = "https://api.github.com/";
    public string PersonalAccessToken { get; set; } = string.Empty;
}
