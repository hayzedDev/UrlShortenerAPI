using Microsoft.AspNetCore.Mvc;
using UrlShortenerAPI.DTOs;
using UrlShortenerAPI.Services;

namespace UrlShortenerAPI.Controllers;

/// <summary>
/// Controller for URL shortening operations
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UrlsController : ControllerBase
{
    private readonly UrlShortenerService _urlService;

    public UrlsController(UrlShortenerService urlService)
    {
        _urlService = urlService;
    }

    /// <summary>
    /// Create a shortened URL
    /// </summary>
    [HttpPost]
    public ActionResult<ShortUrlDto> CreateShortUrl([FromBody] CreateShortUrlDto dto)
    {
        var result = _urlService.CreateShortUrl(dto);

        if (result == null)
        {
            return BadRequest(new { message = "Invalid URL or custom code already exists" });
        }

        return CreatedAtAction(nameof(GetUrlInfo), new { shortCode = result.ShortCode }, result);
    }

    /// <summary>
    /// Get information about a shortened URL
    /// </summary>
    [HttpGet("{shortCode}")]
    public ActionResult<ShortUrlDto> GetUrlInfo(string shortCode)
    {
        var url = _urlService.GetShortUrlInfo(shortCode);

        if (url == null)
        {
            return NotFound(new { message = "Short URL not found" });
        }

        return Ok(url);
    }

    /// <summary>
    /// Get analytics/statistics for a shortened URL
    /// </summary>
    [HttpGet("{shortCode}/stats")]
    public ActionResult<UrlStatsDto> GetStats(string shortCode)
    {
        var stats = _urlService.GetUrlStats(shortCode);

        if (stats == null)
        {
            return NotFound(new { message = "Short URL not found" });
        }

        return Ok(stats);
    }

    /// <summary>
    /// Get all shortened URLs
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<ShortUrlDto>> GetAllUrls()
    {
        var urls = _urlService.GetAllUrls();
        return Ok(urls);
    }

    /// <summary>
    /// Delete a shortened URL
    /// </summary>
    [HttpDelete("{shortCode}")]
    public ActionResult DeleteUrl(string shortCode)
    {
        var success = _urlService.DeleteShortUrl(shortCode);

        if (!success)
        {
            return NotFound(new { message = "Short URL not found" });
        }

        return Ok(new { message = "Short URL deleted successfully" });
    }
}

/// <summary>
/// Controller for redirect functionality
/// </summary>
[ApiController]
public class RedirectController : ControllerBase
{
    private readonly UrlShortenerService _urlService;

    public RedirectController(UrlShortenerService urlService)
    {
        _urlService = urlService;
    }

    /// <summary>
    /// Redirect to the original URL
    /// </summary>
    [HttpGet("/{shortCode}")]
    public IActionResult RedirectToUrl(string shortCode)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = Request.Headers["User-Agent"].ToString();
        var referer = Request.Headers["Referer"].ToString();

        var originalUrl = _urlService.GetOriginalUrl(shortCode, ipAddress, userAgent, referer);

        if (originalUrl == null)
        {
            return NotFound(new { message = "Short URL not found or has expired" });
        }

        return Redirect(originalUrl);
    }
}
