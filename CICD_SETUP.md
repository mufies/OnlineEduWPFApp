# CI/CD Setup Guide

## GitHub Actions Workflows

Tôi đã tạo 2 workflows trong `.github/workflows/`:

### 1. CI Pipeline (`ci.yml`)
**Trigger**: Khi push hoặc tạo PR vào branch `develop` hoặc `main`

**Steps**:
1. Checkout code
2. Setup .NET 8.0
3. Restore dependencies
4. Build solution (Release mode)
5. Run unit tests với code coverage
6. Upload test results và coverage reports

### 2. CD Pipeline (`cd.yml`)
**Trigger**: Khi push tag theo format `v*.*.*` (ví dụ: `v1.0.0`, `v1.2.3`)

**Steps**:
1. Checkout code
2. Setup .NET 8.0
3. Build solution
4. Publish WPF app (self-contained, single file cho Windows x64)
5. Tạo archive (zip)
6. Tạo GitHub Release với file zip đính kèm

## Cách sử dụng

### Chạy CI (tự động)
```powershell
# Push code lên branch develop hoặc main
git add .
git commit -m "Your commit message"
git push origin main
```

CI sẽ tự động chạy và báo kết quả trong tab "Actions" của GitHub repo.

### Chạy CD (deploy release)
```powershell
# Tạo và push tag
git tag v1.0.0
git push origin v1.0.0
```

CD pipeline sẽ:
- Build và publish ứng dụng
- Tạo file zip
- Tạo GitHub Release tự động

## Setup GitHub Secrets (Optional)

### Để dùng AI features trong CI/CD:
1. Vào GitHub repo → Settings → Secrets and variables → Actions
2. Thêm secret mới:
   - Name: `GEMINI_API_KEY`
   - Value: Your Gemini API key

### Để dùng Code Coverage (Optional):
1. Đăng ký tài khoản tại [codecov.io](https://codecov.io)
2. Link GitHub repo
3. Thêm secret:
   - Name: `CODECOV_TOKEN`
   - Value: Token từ Codecov

## Local Testing

### Test build như CI:
```powershell
dotnet restore StudentManagementSystem.sln
dotnet build StudentManagementSystem.sln --configuration Release
dotnet test StudentManagementSystem.sln --configuration Release
```

### Test publish như CD:
```powershell
dotnet publish OnlineEduTaskWPF/OnlineEduTaskWPF.csproj -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```

Output sẽ ở: `OnlineEduTaskWPF/bin/Release/net8.0-windows/win-x64/publish/`

## Branch Strategy

```
main (production-ready)
  ↑
develop (integration)
  ↑
feature/* (individual features)
```

**Workflow**:
1. Tạo feature branch từ `develop`: `git checkout -b feature/your-feature`
2. Code và commit
3. Push và tạo PR vào `develop`
4. CI chạy tự động
5. Sau khi review và approve → merge vào `develop`
6. Khi sẵn sàng release → merge `develop` vào `main`
7. Tag version trên `main` → CD tự động deploy

## Status Badges

Thêm vào README.md:

```markdown
![CI](https://github.com/mufies/OnlineEduWPFApp/actions/workflows/ci.yml/badge.svg)
![CD](https://github.com/mufies/OnlineEduWPFApp/actions/workflows/cd.yml/badge.svg)
```

## Troubleshooting

### CI fails với "Solution file not found"
- Đảm bảo file `StudentManagementSystem.sln` tồn tại ở root directory
- Check đường dẫn trong workflow YAML

### CD không tạo release
- Đảm bảo push tag đúng format: `v1.0.0` (phải có chữ 'v' đầu tiên)
- Check GitHub Actions có permission tạo release (Settings → Actions → General → Workflow permissions → Read and write)

### AI Service lỗi trong production
- Set environment variable `GEMINI_API_KEY` trên server/container
- Windows: `$env:GEMINI_API_KEY="your-key"`
- Linux: `export GEMINI_API_KEY="your-key"`

## Security Notes

✅ API key đã được update để đọc từ environment variable
✅ Không commit API keys vào code
✅ Sử dụng GitHub Secrets cho sensitive data
✅ Fallback key chỉ dùng cho local development

## Next Steps

1. [ ] Commit và push workflows lên GitHub
2. [ ] Set up GitHub Secrets (GEMINI_API_KEY, CODECOV_TOKEN)
3. [ ] Enable Actions permissions cho repo
4. [ ] Test CI bằng cách tạo PR
5. [ ] Test CD bằng cách push tag
