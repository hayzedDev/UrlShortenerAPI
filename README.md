# URL Shortener API

A fast and efficient URL shortening service with click tracking and analytics, built with .NET 10. Like bit.ly but with your own branding!

## 🚀 Features

- **URL Shortening**: Convert long URLs into short, shareable links
- **Custom Short Codes**: Create memorable custom short URLs
- **Click Tracking**: Monitor every click on your shortened URLs
- **Analytics Dashboard**: View detailed statistics and click patterns
- **URL Expiration**: Set automatic expiration dates for links
- **Automatic Redirect**: Seamless redirection to original URLs
- **Real-time Statistics**: Hour-by-hour click analytics

## 📋 API Endpoints

### URLs Management

- `POST /api/urls` - Create a shortened URL
- `GET /api/urls` - Get all shortened URLs
- `GET /api/urls/{shortCode}` - Get URL information
- `GET /api/urls/{shortCode}/stats` - Get analytics for a URL
- `DELETE /api/urls/{shortCode}` - Delete a shortened URL

### Redirection

- `GET /{shortCode}` - Redirect to original URL (tracks the click)

## 🛠️ Technologies Used

- **.NET 10** - Latest .NET framework
- **ASP.NET Core** - Web API framework
- **Swagger/OpenAPI** - Interactive API documentation
- **In-Memory Analytics** - Real-time click tracking

## 📦 Installation & Running

### Prerequisites

- .NET 10 SDK

### Steps

1. **Navigate to project**

   ```bash
   cd UrlShortenerAPI
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Run the application**

   ```bash
   dotnet run --urls="http://localhost:5002"
   ```

4. **Access the API**
   - Swagger UI: `http://localhost:5002`
   - API Base: `http://localhost:5002/api`

## 📝 Sample Data

The API comes with 2 pre-loaded shortened URLs:

- `github` → https://github.com
- `dotnet` → https://dotnet.microsoft.com

## 🧪 Testing the API

### Shorten a URL

```bash
curl -X POST http://localhost:5002/api/urls \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://www.microsoft.com/en-us/some-very-long-url-that-needs-shortening",
    "title": "Microsoft Homepage"
  }'
```

**Response:**

```json
{
  "shortCode": "abc123",
  "shortUrl": "http://localhost:5002/abc123",
  "originalUrl": "https://www.microsoft.com/en-us/some-very-long-url-that-needs-shortening",
  "title": "Microsoft Homepage",
  "clickCount": 0,
  "createdAt": "2025-12-19T08:00:00Z"
}
```

### Create Custom Short URL

```bash
curl -X POST http://localhost:5002/api/urls \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://example.com",
    "customCode": "mylink",
    "title": "My Custom Link"
  }'
```

### Create URL with Expiration

```bash
curl -X POST http://localhost:5002/api/urls \
  -H "Content-Type: application/json" \
  -d '{
    "originalUrl": "https://example.com/event",
    "title": "Limited Time Event",
    "expiresInDays": 7
  }'
```

### Access Shortened URL

Simply visit or curl:

```bash
curl -L http://localhost:5002/abc123
```

### Get URL Statistics

```bash
curl http://localhost:5002/api/urls/abc123/stats
```

**Response:**

```json
{
  "shortCode": "abc123",
  "originalUrl": "https://example.com",
  "title": "Example Site",
  "totalClicks": 42,
  "createdAt": "2025-12-19T08:00:00Z",
  "recentClicks": [
    {
      "clickedAt": "2025-12-19T10:30:00Z",
      "ipAddress": "192.168.1.1",
      "userAgent": "Mozilla/5.0..."
    }
  ],
  "clicksByHour": {
    "08:00": 5,
    "09:00": 12,
    "10:00": 8
  }
}
```

### Get All URLs

```bash
curl http://localhost:5002/api/urls
```

### Delete a URL

```bash
curl -X DELETE http://localhost:5002/api/urls/abc123
```

## 🏗️ Project Structure

```
UrlShortenerAPI/
├── Controllers/          # API endpoint handlers
│   └── UrlsController.cs
├── Models/              # Data models
│   └── ShortenedUrl.cs
├── DTOs/                # Data Transfer Objects
│   └── UrlDtos.cs
├── Services/            # Business logic
│   └── UrlShortenerService.cs
└── Program.cs           # Application entry point
```

## 💡 Key Features Explained

### Random Short Code Generation

- Generates 6-character alphanumeric codes
- Automatically checks for collisions
- Example: `aB3xYz`

### Custom Short Codes

- Users can choose memorable codes
- Example: `meeting2025`, `promo`, `sale`

### Click Tracking

Every redirect captures:

- IP Address
- User Agent (browser/device info)
- Referer (where the click came from)
- Timestamp

### Analytics

- Total clicks per URL
- Recent clicks list
- Clicks grouped by hour of day
- Keeps last 100 clicks per URL for memory efficiency

### URL Expiration

- Optional expiration dates
- Expired URLs return 404
- Perfect for time-limited campaigns

## 🎯 Use Cases

1. **Marketing Campaigns**: Track campaign performance
2. **Social Media**: Share clean, short links
3. **QR Codes**: Create scannable short URLs
4. **Event Management**: Time-limited registration links
5. **Analytics**: Understand user behavior

## 🔐 Security & Best Practices

For production deployment:

- Add authentication for URL creation
- Implement rate limiting
- Add URL validation and sanitization
- Block malicious URLs
- Add CAPTCHA for public endpoints
- Use a real database (not in-memory)
- Add URL preview feature

## 🌐 Deployment

### Configure Base URL

Update `appsettings.json`:

```json
{
  "BaseUrl": "https://yourdomain.com"
}
```

### Deploy to Railway/Render/Azure

Same process as other APIs - the short URLs will automatically use your deployed domain!

## 📈 Scaling Considerations

- Current implementation uses in-memory storage
- For production, use:
  - Database (PostgreSQL, MongoDB)
  - Redis for caching
  - CDN for global distribution

## 📧 Contact

Created by **Hayzed Dev**

- Email: hayzeddev@example.com

## 📄 License

MIT License - Free to use for learning and portfolio!

---

## 🎨 Fun Facts

- The shortest possible URL length is just 6 characters!
- With 62 characters (a-z, A-Z, 0-9), we can create 56+ billion unique URLs
- Bit.ly processes over 2 billion clicks per month
- Your API can handle similar scale with proper infrastructure!
