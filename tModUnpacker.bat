:: Do not use double quote here
SET TMODFILE=DDmod

:: -------

mkdir "tmodFileExtracted\\%TMODFILE%"
tModUnpacker.exe "downloaded\\content\\1281930\\2815777479\\2025.10\\%TMODFILE%.tmod" "tmodFileExtracted\\%TMODFILE%\\"
:: downloaded/content/1281930/2815777479/2025.10
powershell -Command ^
  "Compress-Archive -Path 'tmodFileExtracted\%TMODFILE%\*' -DestinationPath 'tmodFileExtracted\%TMODFILE%\%TMODFILE%.zip' -Force"

git config user.name "github-actions[bot]"
git config user.email "github-actions[bot]@users.noreply.github.com"
git add "tmodFileExtracted\\%TMODFILE%\\"
git add "%TMODFILE%.zip"
git commit -m "Extracted %TMODFILE%.tmod"
git push
