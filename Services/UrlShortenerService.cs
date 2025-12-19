using UrlShortenerAPI.DTOs;
using UrlShortenerAPI.Models;
using System.Text;

namespace UrlShortenerAPI.Services;

/// <summary>
/// Service for managing URL shortening
/// </summary>
public class UrlShortenerService
{
    private readonly Dictionary<string, ShortenedUrl> _urls = new();
    private readonly string _baseUrl;
    private readonly object _lock = new();

    public UrlShortenerService(IConfiguration configuration)
    {
        _baseUrl = configuration["BaseUrl"] ?? "http://localhost:5002";
        SeedData();
    }

    private void SeedData()
    {
        // Add some sample URLs
        var sampleUrls = new[]
        {
            new ShortenedUrl
            {
                ShortCode = "github",
                OriginalUrl = "https://github.com",
                Title = "GitHub - Where the world builds software",
                ClickCount = 42
            },
            new ShortenedUrl
            {
                ShortCode = "dotnet",
                OriginalUrl = "https://dotnet.microsoft.com",
                Title = ".NET Official Website",
                ClickCount = 28
            }
        };

        foreach (var url in sampleUrls)
        {
            _urls[url.ShortCode] = url;
        }
    }

    public ShortUrlDto? CreateShortUrl(CreateShortUrlDto dto)
    {
        lock (_lock)
        {
            if (!Uri.TryCreate(dto.OriginalUrl, UriKind.Absolute, out _))
            {
                return null;
            }

            string shortCode;

            if (!string.IsNullOrEmpty(dto.CustomCode))
            {
                // Use custom code if provided
                if (_urls.ContainsKey(dto.CustomCode))
                {
                    return null; // Custom code already exists
                }
                shortCode = dto.CustomCode;
            }
            else
            {
                // Generate random short code
                shortCode = GenerateShortCode();
                while (_urls.ContainsKey(shortCode))
                {
                    shortCode = GenerateShortCode();
                }
            }

            var shortenedUrl = new ShortenedUrl
            {
                ShortCode = shortCode,
                OriginalUrl = dto.OriginalUrl,
                Title = dto.Title ?? dto.OriginalUrl,
                ExpiresAt = dto.ExpiresInDays.HasValue
                    ? DateTime.UtcNow.AddDays(dto.ExpiresInDays.Value)
                    : null
            };

            _urls[shortCode] = shortenedUrl;

            return MapToDto(shortenedUrl);
        }
    }

    public string? GetOriginalUrl(string shortCode, string? ipAddress = null, string? userAgent = null, string? referer = null)
    {
        lock (_lock)
        {
            if (!_urls.TryGetValue(shortCode, out var shortenedUrl))
            {
                return null;
            }

            // Check if expired
            if (shortenedUrl.ExpiresAt.HasValue && shortenedUrl.ExpiresAt.Value < DateTime.UtcNow)
            {
                return null;
            }

            // Log the click
            shortenedUrl.ClickCount++;
            shortenedUrl.Clicks.Add(new ClickLog
            {
                IpAddress = ipAddress ?? "Unknown",
                UserAgent = userAgent ?? "Unknown",
                Referer = referer ?? "Direct"
            });

            // Keep only last 100 clicks to save memory
            if (shortenedUrl.Clicks.Count > 100)
            {
                shortenedUrl.Clicks = shortenedUrl.Clicks.Skip(shortenedUrl.Clicks.Count - 100).ToList();
            }

            return shortenedUrl.OriginalUrl;
        }
    }

    public ShortUrlDto? GetShortUrlInfo(string shortCode)
    {
        if (_urls.TryGetValue(shortCode, out var shortenedUrl))
        {
            return MapToDto(shortenedUrl);
        }
        return null;
    }

    public UrlStatsDto? GetUrlStats(string shortCode)
    {
        if (!_urls.TryGetValue(shortCode, out var shortenedUrl))
        {
            return null;
        }

        var stats = new UrlStatsDto
        {
            ShortCode = shortenedUrl.ShortCode,
            OriginalUrl = shortenedUrl.OriginalUrl,
            Title = shortenedUrl.Title,
            TotalClicks = shortenedUrl.ClickCount,
            CreatedAt = shortenedUrl.CreatedAt,
            ExpiresAt = shortenedUrl.ExpiresAt,
            RecentClicks = shortenedUrl.Clicks
                .OrderByDescending(c => c.ClickedAt)
                .Take(10)
                .Select(c => new ClickStatsDto
                {
                    ClickedAt = c.ClickedAt,
                    IpAddress = c.IpAddress,
                    UserAgent = c.UserAgent
                })
                .ToList(),
            ClicksByHour = shortenedUrl.Clicks
                .GroupBy(c => c.ClickedAt.Hour)
                .ToDictionary(g => $"{g.Key:00}:00", g => g.Count())
        };

        return stats;
    }

    public IEnumerable<ShortUrlDto> GetAllUrls()
    {
        return _urls.Values.Select(MapToDto).OrderByDescending(u => u.CreatedAt).ToList();
    }

    public bool DeleteShortUrl(string shortCode)
    {
        lock (_lock)
        {
            return _urls.Remove(shortCode);
        }
    }

    private string GenerateShortCode(int length = 6)
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        var result = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }

        return result.ToString();
    }

    private ShortUrlDto MapToDto(ShortenedUrl url)
    {
        return new ShortUrlDto
        {
            ShortCode = url.ShortCode,
            ShortUrl = $"{_baseUrl}/{url.ShortCode}",
            OriginalUrl = url.OriginalUrl,
            Title = url.Title,
            ClickCount = url.ClickCount,
            CreatedAt = url.CreatedAt,
            ExpiresAt = url.ExpiresAt
        };
    }
}
