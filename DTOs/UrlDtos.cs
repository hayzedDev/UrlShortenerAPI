namespace UrlShortenerAPI.DTOs;

public class CreateShortUrlDto
{
    public string OriginalUrl { get; set; } = string.Empty;
    public string? CustomCode { get; set; }
    public string? Title { get; set; }
    public int? ExpiresInDays { get; set; }
}

public class ShortUrlDto
{
    public string ShortCode { get; set; } = string.Empty;
    public string ShortUrl { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int ClickCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class UrlStatsDto
{
    public string ShortCode { get; set; } = string.Empty;
    public string OriginalUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int TotalClicks { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public List<ClickStatsDto> RecentClicks { get; set; } = new();
    public Dictionary<string, int> ClicksByHour { get; set; } = new();
}

public class ClickStatsDto
{
    public DateTime ClickedAt { get; set; }
    public string IpAddress { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}
