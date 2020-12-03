set drive=%1


::This is copied by SotC Viewer from its local resources, so no need to generate it.


::for %%f in  (nico.dat xab xac) do type %%f.index 2>nul >nul || python index-win.py %drive%\%%f > %%f.index

::delete the binary (OCaml script) index output file, as the OCaml script adds the new hashtab at the start of the file and the old data remains at the end.
del index

::This has to be generated, as it contains NICO.DAT path, so it has to be changed to reflect the current path.
for %%f in  (nico.dat xab xac) do ocaml index.ml index %drive%\%%f < %%f.index
