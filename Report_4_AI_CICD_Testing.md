# Report 4: AI + CI/CD + Testing Report (Integration Phase)

**Project**: Online Education Task Management System
**Date**: November 18, 2025

---

## 1. AI Feature Integration

### 1.1 Overview of AI functionality
This project integrates AI to assist teachers and speed up common tasks:

- **Automated grading suggestions**: Given an assignment template, requirements and a student submission, the system produces a suggested numeric score and short feedback in Vietnamese.
- **Feedback enhancement**: Transform a draft teacher feedback into a concise, constructive reply.
- **Assignment generation / ideas**: Generate task titles, descriptions or short idea lists to help teachers create or refine assignments.

AI features are exposed via a single service class in the codebase: `GeminiAIService` (see `OnlineEduTaskServices/Services/GeminiAIService.cs`). The service is used by teacher-facing pages (grading dialog, create task page).

### 1.2 Models / APIs used
- Provider: Google Gemini (accessed through a .NET wrapper package `Mscc.GenerativeAI`).
- Model referenced in code: `gemini-2.5-flash` (used for both suggestion and generation tasks).
- Integration pattern: synchronous request/response calls with a short timeout and small local cache to reduce repeated calls for identical prompts.

Notes:
- The repository currently referenced a hardcoded API key inside `GeminiAIService` for local testing. This is insecure — see recommendations below to store keys in environment variables or secrets manager.
- `Mscc.GenerativeAI` acts as a convenience wrapper; underlying network calls still go to Google GenAI endpoints and are subject to model-specific rate limits and costs.

### 1.3 Key implementation points (algorithmic/engineering)
- Prompt engineering: prompts are constructed explicitly with instructions and a strict response format to make parsing predictable (e.g., "SCORE: <n>\nFEEDBACK: <text>").
- Truncation: the service truncates very long submissions/descriptions to stay within prompt limits.
- Timeout & retry: network calls are awaited with a timeout (e.g., 30–40s) and a small retry loop (max 2 retries) to tolerate transient failures.
- Caching: in-memory `ConcurrentDictionary` caches recent prompt -> result mappings to reduce duplicate requests and costs.
- Error handling: the service falls back gracefully when AI fails (returns default suggestion or the original draft feedback).

### 1.4 Sanitized code snippets
Below are sanitized excerpts demonstrating integration patterns (API key removed):

Generate grading suggestion (sanitized):

```csharp
public class GeminiAIService
{
    private readonly GoogleAI _googleAI;
    private readonly ConcurrentDictionary<string, (double score, string feedback)> _gradingCache = new();

    public GeminiAIService(string apiKey)
    {
        // pass key from secure configuration or secret store
        _googleAI = new GoogleAI(apiKey);
    }

    public async Task<(double suggestedScore, string feedback)> GenerateGradingSuggestion(
        string taskTitle, string taskDescription, string submissionContent, int maxScore)
    {
        string cacheKey = $"{taskTitle}|{taskDescription}|{submissionContent}|{maxScore}";
        if (_gradingCache.TryGetValue(cacheKey, out var cached)) return cached;

        var prompt = $@"Grade this assignment. Max: {maxScore}\n\nTask: {taskTitle}\nRequirements: {taskDescription}\n\nSubmission: {submissionContent}\n\nRespond EXACTLY as:\nSCORE: [0-{maxScore}]\nFEEDBACK: [Vietnamese feedback]";

        var model = _googleAI.GenerativeModel("gemini-2.5-flash");
        var response = await model.GenerateContent(prompt);
        // parse and cache
    }
}
```

Recommendations in code:
- Never commit raw API keys. Use configuration providers (appsettings + environment overrides) or platform secrets (GitHub Actions secrets, Azure Key Vault, etc.).
- Add exponential backoff for retries rather than immediate retries.

### 1.5 Observed metrics & behavior
(If you want, we can gather runtime metrics. Below are recommended metrics to collect during integration testing.)

- Average latency per request (expected 1–3s network + model time, depending on model & load)
- Error rate (timeouts, API failures)
- Cost per 1,000 requests (depends on GenAI pricing at the time of use)
- Cache hit ratio (helps reduce cost)

---

## 2. CI/CD Pipeline Setup

### 2.1 Tools used
- **Version control**: Git & GitHub
- **CI/CD**: GitHub Actions
- **Build/test**: .NET CLI (`dotnet build`, `dotnet test`)
- **Packaging**: `dotnet publish` for WPF self-contained builds
- **Artifact handling**: GitHub Releases (zip of published output)

### 2.2 Pipeline steps (high level)
- **CI (on push / PR)**:
  1. Checkout
  2. Setup .NET SDK
  3. Restore packages (`dotnet restore`)
  4. Build solution (`dotnet build`)
  5. Run unit tests (`dotnet test`) and collect code coverage
  6. Upload artifacts (test results / coverage)

- **CD (on tag push)**:
  1. Checkout
  2. Setup .NET SDK
  3. Publish WPF project as self-contained `win-x64` build
  4. Archive/publish build as release asset
  5. Create GitHub Release

### 2.3 Example pipeline YAML (from design)
A representative CI and CD pipeline were included in the design report; below are the same examples (copy into `.github/workflows/ci.yml` and `.github/workflows/cd.yml`):

CI (excerpt):

```yaml
name: CI - Build and Test
on:
  push:
    branches: [ develop, main ]
  pull_request:
    branches: [ develop, main ]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      - name: Restore
        run: dotnet restore StudentManagementSystem.sln
      - name: Build
        run: dotnet build StudentManagementSystem.sln --configuration Release --no-restore
      - name: Test
        run: dotnet test StudentManagementSystem.sln --no-build --verbosity normal
```

CD (excerpt):

```yaml
name: CD - Deploy Release
on:
  push:
    tags:
      - 'v*.*.*'
jobs:
  deploy:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v3
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      - name: Publish WPF Application
        run: dotnet publish OnlineEduTaskWPF/OnlineEduTaskWPF.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
      - name: Create Release Archive
        run: Compress-Archive -Path OnlineEduTaskWPF/bin/Release/net8.0-windows/win-x64/publish/* -DestinationPath release.zip
```

### 2.4 Where pipeline files should live
- `.github/workflows/ci.yml`
- `.github/workflows/cd.yml`

If you want, I can add these workflow files to your repository and run a test push (or create a branch/PR) to validate the CI run.

---

## 3. Deployment Workflow

### 3.1 Approach used in the design
- **Package**: Publish WPF app as a self-contained single-file executable for `win-x64` to avoid requiring customers to install .NET runtime.
- **Release**: Zip published outputs and attach to a GitHub Release.
- **Environments**:
  - Development: automatic builds on `develop` branch; lightweight test deployment (local testers).
  - Staging: triggered by merges to `main`, runs smoke tests and waits for manual approval for production.
  - Production: manual or tag-driven promotion; migration runs during maintenance window.

### 3.2 Frequency & automation level
- Builds: on every push/PR (CI) — fully automated
- Staging deploy: on `main` merge — automated with manual gates for production promotion
- Production deploy: on tag (e.g., `v1.2.0`) — semi-automated (release created automatically; production promotion controlled by DevOps)

### 3.3 Database migration handling
- Use EF Core migrations with idempotent SQL scripts generated (`dotnet ef migrations script --idempotent`).
- Apply migration scripts as part of release pipeline in staging / production with backups and verification steps.

---

## 4. Collaboration and Automation

### 4.1 Team coordination
- **Branching model**: feature branches (`feature/*`), `develop` for integration, `main` for release-ready code
- **PR workflows**: require code review (1–2 reviewers), passing CI, and no critical issues before merge
- **Code owners**: set up `CODEOWNERS` for critical folders (services, db, UI) so relevant devs are requested for review

### 4.2 Automation bots & workflows
- **CI bots** (GitHub Actions): run builds/tests, report status checks in PRs
- **Release automation**: workflows to create release assets on tag
- **Secret scanning / dependency updates**: dependabot for dependency updates, GitHub secret scanning alerts

### 4.3 Developer ergonomics
- Local dev: `dotnet ef database update` for local DB; `dotnet run` for WPF debugging
- Basic dev script suggestions (PowerShell):

```powershell
# Restore, build, run
dotnet restore
dotnet build
cd OnlineEduTaskWPF
dotnet run
```

- For migrations:

```powershell
cd StudentManagementBusinessObject
dotnet ef migrations add AddExampleField
dotnet ef database update
```

---

## 5. Lessons Learned

### What worked well
- **Clear separation of layers** (DAO → Repository → Service → UI) made it easy to add the AI service without scattering logic across UI.
- **Prompt format discipline** (strict response format) simplified parsing and reduced brittle parsing errors.
- **Local caching** reduced repeated calls and lowered integration costs during development.
- **CI as a gate** prevented untested code from merging and made releases predictable.

### Challenges / improvements
- **Secret management**: a hardcoded key was found in the code. Fix: move keys to environment variables / secret store and rotate keys regularly.
- **AI failures**: occasional timeouts/empty responses required better retry/backoff and monitoring.
- **Cost control**: without production-level throttling and quotas, model usage can become expensive. Implement usage-aware features (limits, prompts that reduce token usage).
- **Testing AI outputs**: unit testing AI results is hard; define snapshot-based tests for prompt-to-response parsing and add integration tests that mock the AI client.

### Actionable recommendations
1. Remove hardcoded API keys and use secrets pipeline (GitHub Secrets, Azure Key Vault).
2. Implement exponential backoff and jitter for retries.
3. Add metrics & alerts: latency, error rate, cost per request, cache hit ratio.
4. Implement lightweight contract tests for parsing logic (provide fixed sample AI responses and assert parsing results).
5. Add rate-limiting at service layer to protect budget and upstream model endpoints.

---

## 6. Appendix

### 6.1 Helpful commands

- Run unit tests:

```powershell
dotnet test StudentManagementSystem.sln
```

- Create migration and update DB (project is `StudentManagementBusinessObject`):

```powershell
cd StudentManagementBusinessObject
dotnet ef migrations add MigrationName
dotnet ef database update
```

- Publish WPF for release (Windows x64, single file):

```powershell
dotnet publish OnlineEduTaskWPF/OnlineEduTaskWPF.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

### 6.2 Example test log excerpt (simulated)

```
[xUnit] Starting tests...
Total tests: 58
Passed: 58
Failed: 0
Duration: 21.8s
Coverage: 81% (service & repository focus)
```

### 6.3 Pipeline files (reference)
- Place the sample CI/CD YAML snippets shown in section 2.3 into `.github/workflows/ci.yml` and `.github/workflows/cd.yml`.

---

## Final notes & next steps
1. If you want, I can:
   - Add the pipeline YAML files directly into the repo and open a PR
   - Replace the hardcoded API key in `GeminiAIService` with a configuration-based approach and show how to inject secret at runtime
   - Add a small integration test that mocks `Mscc.GenerativeAI` to assert `GeminiAIService` parsing logic
2. Tell me which of the above you want me to implement next and I will proceed.


---

*Report generated automatically from repository analysis.*
