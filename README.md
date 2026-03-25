# SmartSummarizer

AI-Powered Text Summarization Application for .NET

[![.NET Version](https://img.shields.io/badge/.NET-7.0-512BD4?logo=.net&logoColor=white)](https://dotnet.microsoft.com/)
[![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-0078D4?logo=windows&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/desktop/winforms/)
[![Google Gemini](https://img.shields.io/badge/API-Google%20Gemini-4285F4?logo=google&logoColor=white)](https://ai.google.dev/gemini-api)
[![License](https://img.shields.io/badge/License-Apache%202.0-blue.svg)](LICENSE.txt)
[![C#](https://img.shields.io/badge/C%23-97.6%25-239120?logo=csharp&logoColor=white)](https://github.com/Saree-tech/SmartSummarizer/search?l=c%23)

---

## Overview

**SmartSummarizer** is a Windows Forms application that leverages Google's Gemini AI API to generate intelligent, context-aware summaries of lengthy texts. Built with .NET 7.0, it provides an intuitive interface for users to extract key information from articles, research papers, reports, and other text content.

Key capabilities:
- Reduces reading time by extracting core ideas.
- Preserves context and meaning in generated summaries.
- Provides both AI-powered and fallback summarization logic.
- Offers a responsive, user-friendly desktop experience.

---

## Features

| Feature | Description |
|---------|-------------|
| AI-Powered Summarization | Uses Google Gemini 1.5 Flash for high-quality summaries |
| Keyboard Shortcuts | Ctrl+S to summarize, Ctrl+C to clear input |
| API Testing Tools | Built-in validation for API connectivity and model listing |
| Fallback Summarizer | Extractive summarization when API is unavailable |
| Async Operations | Non-blocking API calls keep UI responsive |
| Visual Feedback | Progress indicators and status messages |
| Error Handling | Comprehensive management with user-friendly messages |
| Docker Support | Containerization for consistent deployment environments |

---

## Technologies Used

| Component | Technology | Version | Purpose |
|-----------|------------|---------|---------|
| Framework | .NET | 7.0 | Application runtime and libraries |
| UI | Windows Forms | 7.0 | Desktop user interface |
| Language | C# | 10.0 | Core programming language |
| AI Model | Google Gemini | 1.5 Flash | Text summarization engine |
| HTTP Client | System.Net.Http | Built-in | API communication |
| JSON Processing | Newtonsoft.Json | 13.0.3 | Request/response parsing |
| Version Control | Git | Latest | Source code management |
| Containerization | Docker | 20.10+ | Application packaging |

---

## Prerequisites

Before installing SmartSummarizer, ensure you have:

### Required Software
- Windows 10 or 11 (64-bit recommended)
- .NET 7.0 SDK or .NET 7.0 Runtime
- Visual Studio 2022 (for development) or any C# IDE

### API Requirements
- Google Gemini API Key (free tier available)
  - Sign up at Google AI Studio: [https://aistudio.google.com/](https://aistudio.google.com/)
  - 15 requests per minute (RPM) free quota

### Optional
- Docker Desktop (for containerized deployment)
- Git (for version control)

---

## Installation

### Method 1: Clone and Build

```bash
git clone [https://github.com/Saree-tech/SmartSummarizer.git](https://github.com/Saree-tech/SmartSummarizer.git)
cd SmartSummarizer
dotnet restore
dotnet build --configuration Release
dotnet run
````

### Method 2: Open in Visual Studio

1.  Open `SmartSummarizer.sln` in Visual Studio 2022.
2.  Restore NuGet packages (auto-restores).
3.  Build the solution (Ctrl+Shift+B).
4.  Run (F5).

-----

## Configuration

### Adding the API Key

1.  Open `GeminiService.cs`.
2.  Locate the line: `string apiKey = "YOUR_API_KEY_HERE";`.
3.  Replace with your actual key from Google AI Studio.

-----

## Project Structure

```
SmartSummarizer/
│
├── SmartSummarizer.sln          # Visual Studio solution file
├── SmartSummarizer.csproj       # Project configuration
├── Program.cs                   # Application entry point
├── Form1.cs                     # Main UI logic and event handlers
├── Form1.Designer.cs            # UI component layout
├── GeminiService.cs             # API integration and HTTP client
│
├── .gitignore                   # Git ignore rules
├── README.md                    # Project documentation
├── LICENSE.txt                  # Apache 2.0 License
└── Dockerfile                   # Containerization configuration
```

-----

## Docker Support

### Build and Run

```bash
# Build image
docker build -t smart-summarizer .

# Run container
docker run -it --rm smart-summarizer
```

-----

## License

This project is licensed under the **Apache License, Version 2.0**. See the [Apache 2.0](https://github.com/Comcast/Comcast.github.io/blob/main/LICENSE-Apache-2.0) file for the full license text and permissions.

-----

## Contact

**Sareen Fatima (Saree-tech)**

  - GitHub: [github.com/Saree-tech](https://github.com/Saree-tech)
  - Repository: [SmartSummarizer](https://github.com/Saree-tech/SmartSummarizer)

-----

*Developed with .NET and Google Gemini*
