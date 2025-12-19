namespace UrlShortenerAPI.Models;

/// <summary>
/// Represents a shortened URL
/// </summary>
public class ShortenedUrl
{
    public string ShortCode { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int ClickCount { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }
    public string CreatedBy { get; set; } = "Anonymous";
    public List<ClickLog> Clicks { get; set; } = new();
}

/// <summary>
/// Represents a click/visit to a shortened URL
/// </summary>
public class ClickLog
{
    public DateTime ClickedAt { get; set; } = DateTime.UtcNow;
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
    public string Referer { get; set; } = string.Empty;
}
