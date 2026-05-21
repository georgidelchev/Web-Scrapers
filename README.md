<div align="center">

# Web Scrapers

**A collection of C# web scrapers targeting Bulgarian websites — extracting recipes, news, jokes, and geographic data.**

[![Language](https://img.shields.io/badge/C%23-100%25-239120?style=flat-square&logo=csharp&logoColor=white)](https://github.com/georgidelchev/Web-Scrapers/search?l=c%23)
[![License](https://img.shields.io/badge/License-MIT-6366F1?style=flat-square)](LICENSE)
[![Stars](https://img.shields.io/github/stars/georgidelchev/Web-Scrapers?style=flat-square&color=F59E0B)](https://github.com/georgidelchev/Web-Scrapers/stargazers)
[![Forks](https://img.shields.io/github/forks/georgidelchev/Web-Scrapers?style=flat-square&color=22C55E)](https://github.com/georgidelchev/Web-Scrapers/network/members)

<br/>

*Four purpose-built scrapers for four different Bulgarian sites —*
*turning HTML into structured, usable data.*

<br/>

</div>

---

## 📖 About

This repository is a collection of focused C# web scrapers, each targeting a specific Bulgarian website to extract structured data for use in other projects. The scrapers use **AngleSharp** for HTML parsing — the same library used in [AYN](https://github.com/georgidelchev/AYN-) and [novini4ka](https://github.com/georgidelchev/novini4ka) — to traverse the DOM and extract exactly the data needed.

Each scraper is self-contained, simple to run, and outputs clean, normalised data ready for database seeding or further processing.

---

## 🗂️ Scrapers

### 🍳 01 — Gotvach.bg Scraper

> Scrapes **[gotvach.bg](https://gotvach.bg)** — one of Bulgaria's most popular cooking and recipe portals.

Extracts recipe data including titles, ingredients, preparation steps, categories, and images — useful for seeding recipe databases or building food-related applications.

📁 [Browse →](./01%20-%20%5BGotvach.Bg%20Scraper%5D)

---

### 📰 02 — BtvNovinite Scraper

> Scrapes **[btvnovinite.bg](https://btvnovinite.bg)** — the online news portal of bTV, Bulgaria's largest private television network.

Extracts news articles, headlines, publication dates, categories, and article content — used to seed the [novini4ka](https://github.com/georgidelchev/novini4ka) news aggregation platform.

📁 [Browse →](./02%20-%20%5BBtvNovinite%20Scraper%5D)

---

### 😂 03 — Vicove.com Scraper

> Scrapes **[vicove.com](https://vicove.com)** — a popular Bulgarian jokes and humour website.

Extracts jokes by category — the data feeds the [ViceIO](https://github.com/georgidelchev/ViceIO) entertainment platform and the [PrograMEMEin'](https://github.com/georgidelchev/programemein) meme automation app.

📁 [Browse →](./03%20-%20%5BVicoveComScraping%5D)

---

### 🗺️ 04 — Bulgarian Neighbourhoods Scraper

> Scrapes geographic reference data — Bulgarian neighbourhoods, quarters, and settlements.

Extracts structured location data used for address validation, delivery integrations, and geographic filtering in applications like [AYN](https://github.com/georgidelchev/AYN-) and the [EcontAPI Wrapper](https://github.com/georgidelchev/EcontAPI-Wrapper).

📁 [Browse →](./04%20-%20%5BBulgarianNeighborhoods%20Scraper%5D)

---

## 🛠️ Tech Stack

| Component | Technology |
|-----------|-----------|
| Language | C# (.NET) |
| HTML Parsing | AngleSharp |
| HTTP | `System.Net.Http.HttpClient` |
| IDE | Visual Studio |

---

## 🚀 Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/georgidelchev/Web-Scrapers.git
   cd Web-Scrapers
   ```

2. **Open any scraper's `.sln` file** in Visual Studio or Rider.

3. **Configure the output** — by default, scraped data is written to the console or a JSON/database output. Check the specific scraper's `Program.cs` for configuration options.

4. **Run the scraper**
   ```bash
   dotnet run
   ```

> **Note:** Web scraping should always be done responsibly. Add delays between requests, respect `robots.txt`, and do not overload target servers.

---

## 🔗 Used In

These scrapers were built to feed data into other projects in this portfolio:

| Scraper | Used In |
|---------|---------|
| BtvNovinite | [novini4ka](https://github.com/georgidelchev/novini4ka) — news aggregation platform |
| Vicove.com | [ViceIO](https://github.com/georgidelchev/ViceIO) — entertainment platform |
| Vicove.com | [PrograMEMEin'](https://github.com/georgidelchev/programemein) — Instagram meme bot |
| Bulgarian Neighbourhoods | [AYN](https://github.com/georgidelchev/AYN-) — marketplace platform |
| Bulgarian Neighbourhoods | [EcontAPI Wrapper](https://github.com/georgidelchev/EcontAPI-Wrapper) — delivery integration |

---

## 👤 Author

**Georgi Delchev**

[![GitHub](https://img.shields.io/badge/GitHub-georgidelchev-181717?style=flat-square&logo=github)](https://github.com/georgidelchev)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-delchevgeorgi-0A66C2?style=flat-square&logo=linkedin)](https://www.linkedin.com/in/delchevgeorgi/)
[![Facebook](https://img.shields.io/badge/Facebook-georgi.d99-1877F2?style=flat-square&logo=facebook)](https://www.facebook.com/georgi.d99/)

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).

---

<div align="center">
<sub>Made with ☕ and AngleSharp · scrape responsibly</sub>
</div>
