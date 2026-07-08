# GitHub upload steps

These steps upload the project to a new GitHub repository.

## 1. Create a new repository on GitHub

Repository name suggestion:

```text
it-inventory-manager-testview
```

Recommended settings:

```text
Public repository
Do not add README
Do not add .gitignore
Do not add license
```

## 2. Run these commands from PowerShell

Replace `YOUR_GITHUB_USER` with your GitHub username.

```powershell
cd "C:\Users\tiri1\Desktop\Sites\test view\ITInventoryManager\testview_it_inventory"

git init

git add .

git commit -m "Initial IT inventory manager submission"

git branch -M main

git remote add origin "https://github.com/YOUR_GITHUB_USER/it-inventory-manager-testview.git"

git push -u origin main
```

## 3. If the remote already exists

```powershell
git remote remove origin

git remote add origin "https://github.com/YOUR_GITHUB_USER/it-inventory-manager-testview.git"

git push -u origin main
```

## 4. Before sending

Run:

```powershell
dotnet build .\ITInventoryManager.csproj

dotnet run --project .\ITInventoryManager.csproj
```

Then verify that the GitHub repository includes:

```text
README.md
AI_USAGE.md
GITHUB_STEPS.md
publish-exe.ps1
ITInventoryManager.csproj
Program.cs
Models
Services
Forms
```
