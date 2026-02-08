:: Do not use double quote here
SET TMODFILE=CoolerItemVisualEffect

:: -------

mkdir "tmodFileExtracted\\%TMODFILE%"
tModUnpacker.exe "downloaded\\content\\1281930\\2829306780\\2025.11\\%TMODFILE%.tmod" "tmodFileExtracted\\%TMODFILE%\\"
:: downloaded/content/1281930/2829306780/2025.11 
powershell -Command ^
  "Compress-Archive -Path 'tmodFileExtracted\%TMODFILE%\*' -DestinationPath '%TMODFILE%.zip' -Force"

git config user.name "github-actions[bot]"
git config user.email "github-actions[bot]@users.noreply.github.com"
git add "tmodFileExtracted\\%TMODFILE%\\"
git add "tmodFileExtracted\\%TMODFILE%\\%TMODFILE%.zip"
git commit -m "Extracted %TMODFILE%.tmod"
git push
