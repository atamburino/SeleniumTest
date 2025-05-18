# SeleniumTestDemo

## Project Overview

This repository is a reference system for UI and performance testing, designed to align with modern Test Engineering best practices and the following stack:

- .NET C# (test automation)
- Selenium (UI automation)
- xUnit/SpecFlow (test frameworks, BDD)
- JMeter/k6 (performance testing)
- PostgreSQL (test data)
- AWS (cloud integration)
- New Relic, Splunk (monitoring)
- Git, GitHub Actions (CI/CD)

---

## Folder Structure

```
/tests/         # Selenium and future xUnit/SpecFlow test classes
/pages/         # Page Object Model classes (to be added)
/features/      # Gherkin feature files (to be added)
/performance/   # JMeter/k6 scripts (to be added)
/db/            # SQL scripts for test data (to be added)
/docs/          # Notes, best practices, interview Q&A
/util/          # Helpers, WebDriver factory, etc. (to be added)
```

---

## How to Run Selenium Tests

1. **Install dependencies:**
   - Ensure you have .NET 8.0 SDK installed.
   - Chrome browser must be installed (for ChromeDriver).
2. **Restore packages:**
   ```bash
   dotnet restore
   ```
3. **Run the tests:**
   ```bash
   dotnet run
   ```
   This will execute the Selenium tests in `/tests/` (currently `HomeNetNavTest` and `OpenAIChatTest`).

---

## Next Steps (Planned Improvements)

- [ ] Add xUnit/SpecFlow for structured and BDD-style tests
- [ ] Implement Page Object Model in `/pages/`
- [ ] Add Gherkin feature files in `/features/`
- [ ] Add performance scripts in `/performance/`
- [ ] Add SQL scripts and DB integration in `/db/`
- [ ] Add CI/CD with GitHub Actions
- [ ] Add documentation for AWS, New Relic, Splunk integration

---

## Notes & Best Practices

- Use Page Object Model for maintainable UI tests
- Use BDD (SpecFlow) for readable, collaborative test scenarios
- Store logs and artifacts for traceability
- Integrate with CI/CD for automated test runs
- Monitor test health and performance with New Relic/Splunk

---

## Interview Reference

This repo is structured to demonstrate:
- Modern test automation patterns
- Integration with company-standard tools
- Readable, maintainable, and scalable test code

See `/docs/` for more notes and Q&A. 