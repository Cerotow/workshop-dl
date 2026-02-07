:: Do not use double quote here
SET TMODFILE=MEAC

:: -------

mkdir "tmodFileExtracted\\%TMODFILE%"
tModUnpacker.exe "tmodFile\\%TMODFILE%.tmod" "tmodFileExtracted\\%TMODFILE%\\"

git config user.name "github-actions[bot]"
git config user.email "github-actions[bot]@users.noreply.github.com"
:: git add "tmodFileExtracted\\%TMODFILE%\\"
git add "downloaded\\2025.3\\%TMODFILE%\\" 
git commit -m "Extracted %TMODFILE%.tmod"
git push
