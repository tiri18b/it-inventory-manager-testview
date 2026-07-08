# צ׳קליסט סופי להגשת משימת הבית ל־Testview

מטרת הצ׳קליסט היא לוודא שההגשה נראית מסודרת, מקצועית וקלה לבדיקה.

## 1. בדיקת הרצה מקומית

להריץ מתוך תיקיית הפרויקט:

```powershell
cd "C:\Users\tiri1\Desktop\Sites\test view\ITInventoryManager\testview_it_inventory"

dotnet restore .\ITInventoryManager.csproj

dotnet build .\ITInventoryManager.csproj

dotnet run --project .\ITInventoryManager.csproj
```

לבדוק ידנית:

1. האפליקציה נפתחת תקין.
2. נטענים פריטי דוגמה.
3. ניתן להוסיף פריט חדש.
4. ניתן לערוך פריט קיים.
5. ניתן למחוק פריט בדיקה.
6. החיפוש עובד לפי עובד, דגם, מספר סידורי או תג ציוד.
7. הסינון לפי סטטוס עובד.
8. הסינון לפי סוג ציוד עובד.
9. אחרי סגירה ופתיחה מחדש הנתונים נשמרים.

## 2. יצירת גרסת EXE

אם PowerShell חוסם סקריפטים, להריץ קודם:

```powershell
Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
```

ואז:

```powershell
cd "C:\Users\tiri1\Desktop\Sites\test view\ITInventoryManager\testview_it_inventory"

.\publish-exe.ps1
```

לאחר מכן לבדוק שנוצרה תיקייה:

```text
publish\win-x64
```

ולנסות לפתוח את הקובץ:

```text
publish\win-x64\ITInventoryManager.exe
```

## 3. הכנת קבצי הגשה

להריץ:

```powershell
cd "C:\Users\tiri1\Desktop\Sites\test view\ITInventoryManager\testview_it_inventory"

Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass

.\create-submission-zip.ps1
```

הסקריפט ייצור תיקייה בשם:

```text
submission
```

בתוכה יהיו קבצי ZIP מוכנים לשליחה:

```text
Testview_ITInventory_Source_Yehuda_Benisti.zip
Testview_ITInventory_EXE_Yehuda_Benisti.zip
```

אם עדיין לא יצרת EXE, יהיה רק ZIP של קוד המקור.

## 4. העלאה ל־GitHub

מומלץ ליצור Repository בשם:

```text
it-inventory-manager-testview
```

ואז להריץ לפי `GITHUB_STEPS.md`.

לפני Commit לוודא שאין בתיקייה קבצים מיותרים כמו:

```text
bin
obj
publish
Data
submission
```

הם אמורים להיות מוחרגים דרך `.gitignore`.

## 5. סרטון הצגה

אורך מומלץ: 4 עד 5 דקות.

מבנה הסרטון:

1. פתיחה קצרה.
2. הסבר מה המערכת עושה.
3. הצגת המסך הראשי.
4. חיפוש וסינון.
5. הוספה של פריט חדש.
6. עריכה ושינוי סטטוס או שיוך לעובד.
7. מחיקה של פריט בדיקה.
8. הסבר קצר על שמירת נתונים ב־JSON.
9. הסבר איך נעזרת ב־AI.
10. מה היית מוסיף בהמשך כדי לייעל עבודה בחברה.

הטקסט המלא נמצא בקובץ:

```text
VIDEO_SCRIPT_HE.md
```

## 6. מה לשלוח למיכל

מומלץ לשלוח:

1. קישור ל־GitHub.
2. קובץ ZIP של קוד המקור.
3. קובץ ZIP של גרסת EXE, אם רוצים להקל עליהם להריץ.
4. קישור לסרטון.
5. משפט קצר שמסביר שהוספת README ו־AI_USAGE.

נוסח מייל מוכן נמצא בקובץ:

```text
SUBMISSION_EMAIL_HE.md
```

## 7. משפט חשוב להגשה

המטרה היא להראות שבנית פתרון פשוט, עובד וברור, ולא מערכת מנופחת מדי.

המסר המרכזי:

בחרתי לבנות גרסה ראשונה פשוטה כדי לעמוד בזמן המשימה ולהציג בסיס עובד. במקביל תכננתי איך אפשר להרחיב את המערכת לכלי פנימי שמייעל את העבודה של צוות IT והעובדים בחברה.
