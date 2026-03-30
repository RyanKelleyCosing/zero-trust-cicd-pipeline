# 🔐 Zero-Trust CI/CD Pipeline

[![Security Scan](https://img.shields.io/badge/Security-Passed-green?logo=github-actions)](.)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-purple?logo=dotnet)](https://dotnet.microsoft.com/)

A production-ready CI/CD pipeline implementing **zero-trust security principles**. The pipeline automatically fails if any security vulnerabilities are detected, ensuring only secure code reaches production.

## 🎯 What This Project Demonstrates

- **Shift-left security**: Security scanning integrated at the earliest stage of development
- **Automated vulnerability detection**: No manual security reviews needed for common issues
- **Policy-as-code**: Security rules enforced automatically, not by process
- **Defense in depth**: Multiple scanning layers (secrets, SAST, dependencies, containers)

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────────────────┐
│                         GitHub Actions Workflow                         │
├─────────────────────────────────────────────────────────────────────────┤
│                                                                         │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐              │
│  │   Secret     │    │    SAST      │    │ Dependency   │              │
│  │  Detection   │───▶│   Scanning   │───▶│   Scanning   │              │
│  │  (Gitleaks)  │    │  (CodeQL)    │    │   (Trivy)    │              │
│  └──────────────┘    └──────────────┘    └──────────────┘              │
│         │                   │                   │                       │
│         ▼                   ▼                   ▼                       │
│  ┌────────────────────────────────────────────────────────┐            │
│  │              Security Gate (All Must Pass)              │            │
│  └────────────────────────────────────────────────────────┘            │
│                              │                                          │
│                              ▼                                          │
│  ┌──────────────┐    ┌──────────────┐    ┌──────────────┐              │
│  │    Build     │───▶│  Container   │───▶│   Deploy     │              │
│  │   .NET App   │    │    Scan      │    │  to Azure    │              │
│  └──────────────┘    └──────────────┘    └──────────────┘              │
│                                                                         │
└─────────────────────────────────────────────────────────────────────────┘
```

## 🚀 Quick Start

### Prerequisites
- GitHub account
- Azure subscription (for deployment - optional for testing)
- Fork this repository

### Running the Pipeline
1. Push any code change to trigger the pipeline
2. Create a PR to see security scan results as comments
3. Check the Actions tab for detailed scan reports

## 🔒 Security Features

| Feature | Tool | What It Detects |
|---------|------|-----------------|
| Secret Scanning | Gitleaks | API keys, passwords, tokens in code |
| SAST | CodeQL | SQL injection, XSS, code vulnerabilities |
| Dependency Scan | Trivy | Known CVEs in NuGet packages |
| Container Scan | Trivy | Vulnerabilities in Docker images |
| License Check | Trivy | Non-compliant open source licenses |

## 📁 Project Structure

```
├── .github/
│   └── workflows/
│       └── security-pipeline.yml    # Main CI/CD pipeline
├── src/
│   └── SecureApi/                   # Sample .NET 8 Web API
│       ├── Program.cs
│       ├── Controllers/
│       └── SecureApi.csproj
├── tests/
│   └── SecureApi.Tests/             # Unit tests
├── infra/
│   └── main.bicep                   # Azure infrastructure
├── .gitleaks.toml                   # Secret scanning config
├── Dockerfile                       # Container definition
└── README.md
```

## 🧪 Testing the Security Gates

### Test 1: Secret Detection
Create a file with a fake API key - the pipeline will fail:
```csharp
// This will be detected and blocked
string apiKey = "AKIAIOSFODNN7EXAMPLE";
```

### Test 2: Vulnerable Dependency
Add an old package version with known CVEs - the pipeline will fail:
```xml
<PackageReference Include="Newtonsoft.Json" Version="9.0.1" />
```

## 🌐 Deployment

The pipeline deploys to Azure App Service only when all security checks pass:

```bash
# Required GitHub Secrets:
AZURE_CREDENTIALS    # Service principal JSON
AZURE_SUBSCRIPTION   # Azure subscription ID
```

## 📊 Compliance & Reporting

- **SARIF Reports**: Security findings in GitHub Security tab
- **PR Comments**: Automatic security summary on pull requests
- **Badges**: Real-time status badges for README

## 🔧 Configuration

### Customize Security Rules
Edit `.gitleaks.toml` to add custom secret patterns:
```toml
[[rules]]
description = "Custom API Key"
regex = '''my-company-key-[a-zA-Z0-9]{32}'''
```

## 📈 Why This Matters

This pipeline prevents security incidents **before** they happen:
- ⏱️ **Faster feedback**: Developers know immediately if code is insecure
- 💰 **Cost savings**: Fix issues in dev, not production
- 🛡️ **Compliance**: Automated evidence for audits
- 🚫 **Zero trust**: Never assume code is safe

## 📝 License

MIT License - see [LICENSE](LICENSE) for details.

---

**Built with ❤️ for secure DevOps practices**

