::Generated - for unknown versions.
::You can copy the files in index/ index, nico.dat.index, xab.index, xac.index and the files in Resources/version_index/gen to Resources/version_index/<folderForThisVer> and also add the necessry version data to gameVer.txt to add a new game version.

set drive=%1

::for %%f in  (nico.dat) do type %%f.index 2>nul >nul || python index-win.py %drive%\%%f > %%f.index

if a == a (
for %%f in  (nico.dat xab xac) do (
	del %%f.index
	if exist %drive%\%%f  sotcViewerIndexGen %drive%\%%f %%f.index 0x7FFFFFFF
))
	::The 0x7FFFFFFF limits the offsets to that value, so that they are compatible with the ocaml index script.
	::However this does result in inability to access some models.
	::Delete the output index file, else the OCaml script (now needless workaround was added below) and sotcViewerResListsGen will generate based on old files.


::delete the binary (OCaml script) index output file, as the OCaml script adds the new hashtab at the start of the file and the old data remains at the end. Using a name with no extension like "index" is somewhat worrisome, but the delete func seem to mind the extensions too, unlike run funcs.
::The check for the NICO.DAT/XAB/XAC existing is mandatory, else this may happen to use old files, leftover from previous PAL version loadings.
if a == a (
del index
for %%f in  (nico.dat xab xac) do if exist %drive%\%%f ocaml index.ml index %drive%\%%f < %%f.index
)


@echo off
set initDir=%cd%
cd ..\

::Sometimes (always?) @ before a cd command disables it.
::Can't have comments between ( and ) of an 'if' or anything else - they sometimes cause problems - cause the next code not to execute.

::Delete the bosses' animations, so that they don't get listed for versions lacking some bosses
if a == a (
cd Resources\version_index\gen
for %%f in (boss_A bird_A kame_A monkey_A griffin_A snake_C knight_A phoenix_A yamori_B eel_B yamori_A spider_A leo_A poseidon_A cerberus_A worm_A minotaur_A sirius_A kirin_A buddha_A devil_A narga_A roc_A minotaur_B minotaur_C evis_A narga_A_head) do (
	if exist %%f.txt del %%f.txt
)
)


cd %initDir%
cd ..\
@echo on

::Mind that sotcViewerResListsGen does not delete the files for bosses it does not see in the index file.
index\sotcViewerResListsGen Resources index\nico.dat.index index\xab.index index\xac.index


@cd %initDir%

::pause
