# פקודות מסודרות מהתחלה עד הגשה

להריץ לפי הסדר מתוך PowerShell.

## 1. כניסה לתיקיית הפרויקט

```powershell
cd "C:\Users\tiri1\Desktop\Sites\test view\ITInventoryManager\testview_it_inventory"
```

## 2. בדיקת Build והרצה

```powershell
dotnet restore .\ITInventoryManager.csproj

dotnet build .\ITInventoryManager.csproj

dotnet run --project .\ITInventoryManager.csproj
```

## 3. יצירת EXE

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

.\publish-exe.ps1
```

בדיקה:

```powershell
.\publish\win-x64\ITInventoryManager.exe
```

## 4. יצירת חבילת הגשה

```powershell
.\create-submission-zip.ps1
```

התוצרים יהיו כאן:

```text
C:\Users\tiri1\Desktop\Sites\test view\ITInventoryManager\testview_it_inventory\submission
```

## 5. העלאה ל־GitHub

להחליף את YOUR_GITHUB_USER בשם המשתמש שלך.

```powershell
git init

git add .

git commit -m "Initial IT inventory manager submission"

git branch -M main

git remote add origin "https://github.com/YOUR_GITHUB_USER/it-inventory-manager-testview.git"

git push -u origin main
```

אם כבר עשית `git init` או remote קיים, לא להריץ שוב בלי לבדוק. במקרה כזה להריץ:

```powershell
git status

git remote -v
```

## 6. אחרי תיקונים נוספים

```powershell
git status

git add .

git commit -m "Finalize submission docs and publish scripts"

git push
```
