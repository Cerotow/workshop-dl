:: Do not use double quote here
SET TMODFILE=MEAC

:: -------

mkdir "tmodFileExtracted\\%TMODFILE%"
tModUnpacker.exe "downloaded\\2025.3\\%TMODFILE%.tmod" "tmodFileExtracted\\%TMODFILE%\\"

powershell -Command ^
  "Compress-Archive -Path 'tmodFileExtracted\%TMODFILE%\*' -DestinationPath '%TMODFILE%.zip' -Force"

git config user.name "github-actions[bot]"
git config user.email "github-actions[bot]@users.noreply.github.com"
git add "tmodFileExtracted\\%TMODFILE%\\"
git commit -m "Extracted %TMODFILE%.tmod"
git push
