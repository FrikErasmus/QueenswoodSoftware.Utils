# QueenswoodSoftware.Utils

> A comprehensive C# utility library by **Queenswood Software** — covering API calls, SQL, mail, encryption, file handling, validation, pagination, compression, JWT, CSV, and much more.

[![NuGet](https://img.shields.io/nuget/v/QueenswoodSoftware.Utils)](https://www.nuget.org/packages/QueenswoodSoftware.Utils)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE.txt)
[![.NET](https://img.shields.io/badge/.NET-10.0-blue)](https://dotnet.microsoft.com)

---

## Installation

```bash
dotnet add package QueenswoodSoftware.Utils
```

Or via the NuGet Package Manager:

```
Install-Package QueenswoodSoftware.Utils
```

---

## Table of Contents

- [Classes](#classes)
- [Helpers](#helpers)
- [Utils](#utils)
- [Models](#models)
- [Configuration](#configuration)
- [Requirements](#requirements)
- [License](#license)

---

## Classes

### `Call` — RestSharp API Client

Basic-auth REST client using RestSharp.

```csharp
var call = new Call(configuration);
var result = await call.Api("/endpoint", payload, Method.Post);
```

---

### `ApiCall` — HttpClient API Client

Three overloads covering Bearer token, JWT + UserId header, and Basic auth.

```csharp
var api = new ApiCall();

// Bearer token
var result = await api.ApiPost(url, "controller", "method", payload, token);

// JWT + UserId
var result = await api.Post(url, "controller", "method", payload, new List<string> { userId, jwtToken });

// Basic auth
var result = await api.Post(url, apiBase, "method", payload, new User { Username = "u", Password = "p" });
```

---

### `Mail` — SMTP Email Sender

Sends emails via SMTP with SSL, credentials, multiple recipients, CC, BCC, and attachments.

```csharp
var mail = new Mail(configuration);
await mail.SendMail(new MailObject
{
    To = [new MailAddress("recipient@example.com")],
    Subject = "Hello",
    Body = "<p>Hello World</p>"
});

var result = await mail.TrySendMailAsync(new MailObject
{
    To = [new MailAddress("recipient@example.com")],
    Subject = "Hello",
    Body = "<p>Hello World</p>"
}, cancellationToken);
if (!result.Success)
    Console.WriteLine(result.ShortDescription);
```

---

### `SqlHelper` — SQL Server Helper

Simplifies SQL Server reads and transactions.

```csharp
var dt = SqlHelper.SqlSelect("SELECT * FROM Users WHERE Id = @Id",
    new SqlParameter("@Id", userId), connectionString);

SqlHelper.SqlTransaction("UPDATE Users SET Name = @Name WHERE Id = @Id",
    new[] { new SqlParameter("@Name", name), new SqlParameter("@Id", id) }, connectionString);
```

---

### `DataToExcel`

```csharp
DataToExcel.DataTableToExcel(table, "Export", "Sheet1");
DataToExcel.DataTablesToExcel(new[]
{
    ("Orders", ordersTable),
    ("Lines", linesTable)
});
```

---

## Helpers

### `StringExtensions`

```csharp
"hello world".Truncate(8);                     // "hello..."
"hello world".ToPascalCase();                  // "HelloWorld"
"HelloWorld".ToSnakeCase();                    // "hello_world"
"hello world".ToCamelCase();                   // "helloWorld"
"Hello World".WordCount();                     // 2
"Hello World!".RemoveSpecialCharacters();      // "Hello World"
"hello".Repeat(3);                             // "hellohellohello"
"hello world".ToTitleCase();                   // "Hello World"
"hello world".ContainsIgnoreCase("WORLD");     // true
```

---

### `EnumHelper`

```csharp
public enum Status { [Description("Active User")] Active, Inactive }

EnumHelper.GetDescription(Status.Active);          // "Active User"
EnumHelper.ParseOrDefault<Status>("active");       // Status.Active
EnumHelper.ToNameValueList<Status>();              // [{ Name="Active User", Value="0" }, ...]
EnumHelper.FromDescription<Status>("Active User"); // Status.Active
```

---

### `JsonHelper`

```csharp
var json = JsonHelper.Serialize(myObject, prettyPrint: true);
var obj  = JsonHelper.Deserialize<MyClass>(json);

if (JsonHelper.TryDeserialize<MyClass>(json, out var result)) { }

JsonHelper.IsValid(json);       // true / false
JsonHelper.PrettyPrint(json);   // formatted JSON string
var copy = JsonHelper.Clone(myObject);
```

---

### `PaginationHelper`

```csharp
var paged = PaginationHelper.Paginate(myList, page: 2, pageSize: 10);

paged.Items        // List<T> for the current page
paged.TotalCount   // total records
paged.TotalPages   // total pages
paged.HasNextPage  // bool
paged.HasPreviousPage // bool
```

---

### `CollectionHelper`

```csharp
// Split into batches
foreach (var batch in myList.Chunk(50)) { }

// Distinct by property
var unique = myList.DistinctBy(x => x.Id);

// Partition into two lists
var (active, inactive) = myList.Partition(x => x.IsActive);

// Flatten nested lists
var flat = nestedList.Flatten();

// Null-safe empty check
if (myList.IsNullOrEmpty()) { }

// Safe dictionary (skips duplicate keys)
var dict = myList.ToDictionarySafe(x => x.Id, x => x.Name);
```

---

### `CsvWriter`

```csharp
// From DataTable
string csv = CsvWriter.DataTableToCsv(dt);
byte[] bytes = CsvWriter.ToBytes(csv);

// From List<T>
string csv = CsvWriter.ListToCsv(myList, delimiter: ";", includeHeaders: true);
```

---

### `HtmlHelper`

```csharp
HtmlHelper.StripTags("<p>Hello <b>World</b></p>");  // "Hello World"
HtmlHelper.Encode("<script>");                        // "&lt;script&gt;"
HtmlHelper.Decode("&lt;p&gt;");                      // "<p>"
HtmlHelper.TextToHtml("Line1\nLine2");               // "Line1<br/>Line2"
HtmlHelper.ContainsHtml("<b>bold</b>");              // true
HtmlHelper.Sanitize(html, ["p", "b", "i"]);         // strips disallowed tags
```

---

### `CompressionHelper`

```csharp
byte[] compressed = CompressionHelper.Compress("large text content");
string original   = CompressionHelper.Decompress(compressed);

// Base64 round-trip
string b64 = CompressionHelper.CompressToBase64("large text content");
string text = CompressionHelper.DecompressFromBase64(b64);
```

---

### `ExceptionHelper`

```csharp
ExceptionHelper.GetFullMessage(ex);     // "Outer → Inner → Root cause"
ExceptionHelper.FormatForLogging(ex);   // type + message + stack trace
ExceptionHelper.IsTransient(ex);        // true for network/IO/timeout errors

var value = ExceptionHelper.TryGet(() => riskyOperation(), defaultValue: 0);

foreach (var e in ExceptionHelper.Flatten(ex)) { }
```

---

### `RandomHelper`

```csharp
RandomHelper.AlphaNumeric(16);   // "aB3kR9mZpQ2xNwYt"
RandomHelper.Numeric(6);          // "482931"
RandomHelper.WithSpecialChars(12); // "aB3!kR@mZpQ#"
RandomHelper.Hex(8);              // "3f9a1b2c"
RandomHelper.Between(1, 100);     // 42
RandomHelper.Pick(myList);        // random item
RandomHelper.Chance(30);          // true ~30% of the time
```

---

### `TokenHelper` — JWT Inspector

Parse and inspect JWT tokens without signature validation.

```csharp
TokenHelper.GetClaim(token, "email");    // "user@example.com"
TokenHelper.GetSubject(token);           // subject claim
TokenHelper.GetEmail(token);             // email or unique_name claim
TokenHelper.GetExpiry(token);            // DateTime?
TokenHelper.IsExpired(token);            // bool
TokenHelper.IsValid(token);              // readable + not expired
TokenHelper.GetClaims(token);            // IDictionary<string, object?>
```

---

### `DataEncryption`

Config-driven AES encryption with key rotation support.

```csharp
string encrypted = DataEncryption.Encrypt(config, plainText);

if (DataEncryption.TryDecrypt(config, cipherText, out var plain)) { }

bool needsMigration = DataEncryption.UsesLegacyKeyOnly(config, cipherText);
```

`Secure` resolves its AES key from `LIBRARY_ENCRYPTION_KEY`, then `Secure.ConfiguredKey`, then a legacy default.

```csharp
Secure.ConfiguredKey = configuration["Security:LibraryEncryptionKey"];
```

`DataEncryption` requires `appsettings.json`:
```json
{ "Security": { "EncryptionKey": "your-key", "LegacyEncryptionKey": "old-key" } }
```

---

### `HashHelper`

```csharp
HashHelper.Sha256("input");           // hex string
HashHelper.Md5("input");              // hex string
HashHelper.Hmac("input", "secret");   // base64 HMAC-SHA256
```

---

### `RetryHelper`

```csharp
var result = await RetryHelper.ExecuteAsync(
    () => CallExternalApi(),
    retries: 3,
    delayMs: 500);
```

---

## Utils

### `NumberUtils`

```csharp
NumberUtils.FormatCurrency(1234.5m);        // "R1,234.50"
NumberUtils.FormatCurrency(1234.5m, "$");   // "$1,234.50"
NumberUtils.Clamp(150m, 0m, 100m);          // 100
NumberUtils.Percentage(25m, 200m);          // 12.50
NumberUtils.PercentageOf(15m, 200m);        // 30.00
NumberUtils.FormatFileSize(1048576);        // "1.00 MB"
NumberUtils.IsNumeric("123.45");            // true
NumberUtils.SafeInt("abc", 0);              // 0
NumberUtils.SafeDecimal("3.14");            // 3.14
```

---

### `DateTimeUtils`

```csharp
DateTimeUtils.StartOfDay(DateTime.Now);
DateTimeUtils.EndOfDay(DateTime.Now);
DateTimeUtils.IsWeekend(DateTime.Now);
DateTimeUtils.ToFriendly(DateTime.Now);    // "2 hours ago"
```

---

### `TimeZoneUtils`

```csharp
TimeZoneUtils.ToSouthAfrica(DateTime.UtcNow);   // SAST datetime
TimeZoneUtils.SouthAfricaNow();                  // current SAST time
TimeZoneUtils.ToUtc(localTime, "South Africa Standard Time");
TimeZoneUtils.Convert(time, fromZone, toZone);
TimeZoneUtils.IsValidTimeZoneId("UTC");          // true
```

---

### `EnvironmentUtils`

```csharp
EnvironmentUtils.GetEnvironmentName();   // "Development" / "Production"
EnvironmentUtils.IsDevelopment();        // bool
EnvironmentUtils.IsProduction();         // bool
EnvironmentUtils.IsRunningInContainer(); // bool
EnvironmentUtils.MachineName;            // "MY-PC"
EnvironmentUtils.FormatWorkingSet();     // "128.45 MB"
EnvironmentUtils.GetVariable("MY_VAR", "default");
```

---

### `FileUtils`

```csharp
FileUtils.ReadAllText(path);
FileUtils.WriteAllText(path, content);
FileUtils.EnsureDirectory(path);
FileUtils.GetExtension(filename);
FileUtils.IsImage(filename);
```

---

### `ValidationUtils`

```csharp
ValidationUtils.IsEmail("user@example.com");    // true
ValidationUtils.IsUrl("https://example.com");   // true
ValidationUtils.IsPhoneNumber("+27821234567");  // true
```

---

### `RegexUtils`

```csharp
RegexUtils.IsMatch(input, pattern);
RegexUtils.ExtractMatches(input, pattern);
RegexUtils.Replace(input, pattern, replacement);
```

---

### `DataTableToHtmlTable`

```csharp
string html = DataTableToHtmlTable.DataTableToHtml(dt);
// Returns a styled <table> ready for email or web rendering
```

---

## Models

| Model | Properties |
|---|---|
| `ResponseMessage` | `Success`, `ShortDescription`, `Data` |
| `MailObject` | `From`, `To`, `Cc`, `Bcc`, `Subject`, `Body`, `Attachments` |
| `NameValue` | `Name`, `Value` |
| `Location` | `Latitude`, `Longitude`, `Address` |
| `Tokens` | `AccessToken`, `RefreshToken` |
| `Upload` | `FileName`, `ContentType`, `Data` |
| `Crud` | `Id`, `CreatedBy`, `CreatedDate`, `UpdatedBy`, `UpdatedDate` |
| `ResponseMessage` | `Success`, `ShortDescription` |
| `ScheduleConfig` | `ScheduleId`, `ReportName`, `SendTime`, `FrequencyType`, `CronExpression` |
| `Login` | `Username`, `Password` |

---

## Configuration

Add to your `appsettings.json` as needed:

```json
{
  "ApiUrl": "https://your-api.example.com/",
  "Auth": {
    "User": "api-username"
  },
  "Mail": {
    "Server": "smtp.example.com",
    "Port": "587",
    "From": "no-reply@example.com",
    "Name": "My Application",
    "UserName": "smtp-user",
    "Password": "smtp-password"
  },
  "Security": {
    "EncryptionKey": "your-32-character-key-here",
    "LegacyEncryptionKey": "old-key-for-migration"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MyDb;Trusted_Connection=True;"
  }
}
```

---

## Requirements

- .NET 10.0 or later

---

## License

MIT — © 2025 Queenswood Software
