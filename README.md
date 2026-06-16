# InfoTrack Solicitor Search

A .NET 8 Web API + Vue SPA application that scrapes solicitor contact details from solicitors.com by location and presents the results as a report.

## Overview

This application automates the extraction of solicitor contact details for InfoTrack sales research.

Users can:
- Select and adjust search locations
- Submit a conveyancing search
- Scrape solicitor listings
- Parse solicitor details
- View a report summary
- Sort results by key fields

---

# Features

## Backend

- ASP.NET Core Web API (.NET 8)
- Clean separation of concerns
- Dependency Injection
- HttpClient based scraper
- Custom HTML parsing (no third-party scraping libraries)
- Regex based extraction
- CancellationToken support
- Report generation
- Mock scraper support for development/testing

## Frontend

- Vue 3 SPA
- TypeScript
- Axios API communication
- Dynamic location management
- Sortable solicitor table
- Report summary cards
- Responsive simple UI

## Repository Structure

This repository contains:

- InfoTrack.Api - ASP.NET Core Web API
- InfoTrack.Application - Application services and contracts
- InfoTrack.Domain - Domain models
- InfoTrack.Infrastructure - Scraping and parsing implementations
- infotrack-ui - Vue SPA frontend

---

# Architecture

```
                    +----------------+
                    |    Vue SPA     |
                    |   Frontend     |
                    +--------+-------+
                             |
                             | HTTP
                             |
                    +--------v-------+
                    | ASP.NET Core   |
                    | Web API        |
                    +--------+-------+
                             |
                             |
                    +--------v-------+
                    | Application    |
                    | Services       |
                    +--------+-------+
                             |
              +--------------+--------------+
              |                             |
              v                             v

    +------------------+          +----------------+
    | Solicitor        |          | Solicitor      |
    | Scraper          |          | Parser         |
    | HttpClient       |          | HTML Parsing   |
    +------------------+          +----------------+
              |
              |
              v

    +--------------------------------+
    | solicitors.com                 |
    | conveyancing + location pages  |
    +--------------------------------+
```

---

# Project Structure

```
InfoTrack
|
├── InfoTrack.Api
│   └── Controllers
│
├── InfoTrack.Application
│   ├── Interfaces
│   └── Services
│
├── InfoTrack.Domain
│   ├── Entities
│   └── Extensions
│
├── InfoTrack.Infrastructure
│   ├── Scrapers
│   └── Parsers
│
└── infotrack-ui
    |
    └── src
        ├── Models
        ├── Services
```

---

# Running The Application

## Requirements

Backend:

- .NET 8 SDK

Frontend:

- Node.js
- npm

---

# Backend Setup

Navigate to the API project:

```bash
cd InfoTrack.Api
```

Restore packages:

```bash
dotnet restore
```

Run:

```bash
dotnet run
```

The API runs on:

```
https://localhost:7152
http://localhost:5006
```

Swagger UI is available in development at: http://localhost:5006/swagger/index.html

---

# Frontend Setup

Navigate to:

```bash
cd infotrack-ui
```

Install dependencies:

```bash
npm install
```

Start development server:

```bash
npm run dev
```

The frontend runs on:

```
http://localhost:5173
```

---

# API Endpoints

## Health Check

```
GET /api/health
```

Example response:

```
Live
```

---

## Solicitor Search

```
POST /api/solicitors/getByLocations
```

Example request:

```json
[
  "London",
  "Manchester",
  "Leeds"
]
```

Returns solicitor results.

---

## Report Search

```
POST /api/report/getByLocations
```

Example request:

```json
[
  "London",
  "Birmingham",
  "Liverpool"
]
```

Returns:

- Solicitor list
- Total solicitor count
- Average rating
- Top rated solicitor

---

# Design Decisions

## Scraping

The scraping logic is isolated behind:

```
ISolicitorScraper
```

The scraper is responsible only for:

- Building target URLs
- Downloading HTML
- Handling HTTP communication


This keeps external website concerns separated from application logic.

---

## Parsing

HTML parsing is isolated behind:

```
ISolicitorParser
```

The parser converts raw HTML into domain entities.

This allows:

- Different data sources
- Easier testing
- Cleaner business logic

---

## Reporting

The API creates a report model containing useful insights:

- Total solicitors found
- Average rating
- Highest rated solicitor

The intention is to turn raw scraped data into useful information for sales research.

---

## Frontend Sorting

Sorting is handled client-side.

Reasoning:

- The API already returns the required dataset
- Avoids unnecessary API calls
- Keeps the API simple
- Provides instant UI feedback

---

# Future Improvements

With additional time, possible improvements:

- Add automated tests
- Add API error middleware
- Add retry policies for scraping failures
- Move default locations into configuration
- Add persistence for previous searches
- Add historical reporting
- Add scheduled scraping
- Add pagination
- Add additional solicitor data sources

---

# Notes

The application intentionally avoids third-party scraping libraries to demonstrate:

- HTTP communication
- HTML processing
- Regex usage
- Separation of responsibilities
- Clean architecture principles
