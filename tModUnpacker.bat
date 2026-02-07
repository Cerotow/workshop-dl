:: Do not use double quote here
SET TMODFILE=MEAC

:: -------

mkdir "tmodFileExtracted\\%TMODFILE%"
tModUnpacker.exe "downloaded\\content\\1281930\\2858396998\\2025.3\\%TMODFILE%.tmod" "tmodFileExtracted\\%TMODFILE%\\"
:: downloaded/content/1281930/2858396998/2025.3
powershell -Command ^
  "Compress-Archive -Path 'tmodFileExtracted\%TMODFILE%\*' -DestinationPath '%TMODFILE%.zip' -Force"

git config user.name "github-actions[bot]"
git config user.email "github-actions[bot]@users.noreply.github.com"
git add "tmodFileExtracted\\%TMODFILE%\\"
git add "tmodFileExtracted\\%TMODFILE%\\%TMODFILE%.zip"
git commit -m "Extracted %TMODFILE%.tmod"
git push
