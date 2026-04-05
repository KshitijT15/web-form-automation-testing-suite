# Web Form Automation Testing Suite

An end-to-end test automation project built with **C++**, **ASP.NET Core**, and **Selenium WebDriver** — simulating a real-world QA engineering workflow where a data engine feeds an automated test suite against a live web application.

---

## Live Demo

> Deployed on Railway — (https://web-form-automation-testing-suite-production.up.railway.app/)

---

## Architecture
```
CppDataEngine/          → Generates random valid + invalid test users → test_data.json
WebFormApp/             → ASP.NET Core MVC registration form (the target app)
SeleniumTests/          → Reads test_data.json, automates the browser, asserts results
```
```
C++ Engine  ──► test_data.json ──► Selenium Tests ──► Chrome Browser ──► WebFormApp
```

---

## Tech Stack

| Layer | Technology | Purpose |
|---|---|---|
| Data Engine | C++ (OOP, STL, file I/O) | Auto-generate test cases |
| Web App | ASP.NET Core MVC | Registration form with server-side validation |
| Test Suite | Selenium WebDriver + NUnit | Browser automation and assertions |
| Deployment | Docker + Railway | Free cloud hosting |

---

## Features

- **Auto-generated test data** — C++ engine produces random valid and invalid users on every run
- **8 dynamic test cases** — 3 valid users, 2 invalid emails, 2 short passwords, 1 empty name
- **End-to-end integration** — Selenium reads C++ output and drives a live .NET web app
- **Server-side validation** — ASP.NET DataAnnotations with per-field error messages
- **Explicit waits** — `WebDriverWait` used throughout, no fragile `Thread.Sleep` calls
- **Docker deployment** — containerized and deployed to Railway

---

## Project Structure
```
WebFormAutomation/
├── CppDataEngine/
│   ├── User.h                    ← struct with name, email, phone, password, failField
│   ├── TestDataGenerator.h/cpp   ← generates random test users
│   ├── main.cpp
│   └── test_data.json            ← auto-generated output
│
├── WebFormApp/
│   ├── Models/UserModel.cs       ← validation rules (Required, EmailAddress, MinLength)
│   ├── Controllers/Registration  ← GET + POST actions
│   ├── Views/Registration/       ← Razor form with id= attributes for Selenium
│   └── Dockerfile
│
├── SeleniumTests/
│   ├── RegistrationTests.cs      ← 8 auto-discovered test cases
│   └── TestData/test_data.json   ← copied from CppDataEngine
│
└── README.md
```

---

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [g++ compiler](https://www.mingw-w64.org/) (MinGW on Windows)
- Google Chrome + matching [ChromeDriver](https://chromedriver.chromium.org/)

---

### Step 1 — Generate test data (C++)
```bash
cd CppDataEngine
g++ main.cpp TestDataGenerator.cpp -o DataEngine
./DataEngine.exe
```

Output: `Generated 8 test cases in test_data.json`

---

### Step 2 — Start the web app (.NET)
```bash
cd WebFormApp
dotnet run
# App runs at http://localhost:5148
```

---

### Step 3 — Copy JSON and run Selenium tests
```bash
copy CppDataEngine\test_data.json SeleniumTests\TestData\test_data.json

cd SeleniumTests
dotnet test
```

Expected output:
```
Test summary: total: 8, failed: 0, succeeded: 8, duration: 45s
```

---

## Test Cases

Tests are **auto-discovered** from `test_data.json` — no hardcoded test cases.

| # | Test Name | Input | Expected |
|---|---|---|---|
| 1-3 | `Valid: Rahul Sharma (rahul@gmail.com)` | Valid data | Success message |
| 4-5 | `Invalid [email]: notanemail` | Bad email | Email error shown |
| 6-7 | `Invalid [password]: rahul@gmail.com` | 3-char password | Password error shown |
| 8 | `Invalid [name]: (empty)` | Empty name | Name required error |

---

## Key Concepts Demonstrated

**C++**
- OOP with classes and structs
- `vector`, `ofstream`, `srand` for random generation
- Manual JSON serialization

**ASP.NET Core**
- MVC pattern — Model, View, Controller separation
- `DataAnnotations` for declarative server-side validation
- Razor tag helpers (`asp-for`, `asp-validation-for`)
- `novalidate` to force server-side validation flow

**Selenium WebDriver**
- `By.Id` locators — stable element targeting
- `WebDriverWait` — explicit waits over implicit
- `TestCaseSource` — data-driven dynamic test generation
- `[SetUp]` / `[TearDown]` — browser lifecycle management
- `IJavaScriptExecutor` — direct JS execution when needed

---

## Deployment

The web app is containerized with Docker and deployed to Railway.
```bash
# Build locally
docker build -t webformapp ./WebFormApp
docker run -p 8080:8080 webformapp
```

Railway auto-deploys on every `git push` to `main`.

---

## What I Learned

- How Selenium's explicit waits differ from implicit waits and why it matters for flaky tests
- Why `novalidate` is needed when testing server-side validation through Selenium
- How to use `TestCaseSource` in NUnit to generate tests dynamically from external data
- How Docker + Railway enables free, push-to-deploy cloud hosting for .NET apps

---

## Author

**Kshitij Thorat**  
[GitHub](https://github.com/YOUR_USERNAME) · [LinkedIn](https://linkedin.com/in/YOUR_PROFILE)
