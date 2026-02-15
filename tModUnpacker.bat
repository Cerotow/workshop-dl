setlocal enabledelayedexpansion

set TMODFILE=

for /f "usebackq tokens=2 delims==" %%A in ("input.sh") do (
    set TMODFILE=%%A
    goto done
)
set TMODFILE=%TMODFILE:"=%

set TMODFILE=%TMODFILE:.tmod=%

mkdir tmodFileExtracted\%TMODFILE%

mkdir "tmodFileExtracted\\%TMODFILE%"
tModUnpacker.exe "downloaded\\content\\1281930\\2966457992\\2023.8\\%TMODFILE%.tmod" "tmodFileExtracted\\%TMODFILE%\\"
:: downloaded/content/1281930/2966457992/2023.8
powershell -Command ^
  "Compress-Archive -Path 'tmodFileExtracted\%TMODFILE%\*' -DestinationPath 'tmodFileExtracted\%TMODFILE%\%TMODFILE%.zip' -Force"

git config user.name "github-actions[bot]"
git config user.email "github-actions[bot]@users.noreply.github.com"
git add "tmodFileExtracted\\%TMODFILE%\\"
git add "%TMODFILE%.zip"
git commit -m "Extracted %TMODFILE%.tmod"
git push
