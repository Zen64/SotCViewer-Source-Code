open Int32
open Printf

let getindex path =
  try
    let ic = open_in_bin path in
    let index = input_value ic in
    close_in ic;
    index
  with _ ->
    []
;;


let main indexpath container =
  let index = getindex indexpath in
  let hash = Hashtbl.create 100 in
  let rec loop () =
    match try Some (input_line stdin) with End_of_file -> None with
    | None -> ()
    | Some line ->
        let offs, size, path =
          Scanf.sscanf line "%lx %d %s" (*originally %x*)
            (fun offs size path -> (offs, size, path))
        in
		  let  offset = Int32.to_int offs in (*This and the above change %x->%lx doen't do much, but at least it prevents the code crashing(err) in sscanf when an offset value >= 0x80000000 is found.*) 
		  (*An actual way to fix it would be to make the hashtab use Int32 (%lx), but that changes the actual hashtab and so requires modifications to Dormin Viewer (re-compiling it).*)
		  if false then eprintf "offset in %lx  out %x \n" offs offset;   
        let name = Filename.basename path in
        if not (Hashtbl.mem hash name)
        then
          Hashtbl.add hash name (offset, size) (*If offset is changed to offs (Int32 directly to hashtab) - works, but DorminViewer expects int so it can't read such a hashtab index file.*)
        ;
        loop ()
  in
  loop ();
  let oc = open_out_bin indexpath in
  output_value oc ((container, hash) :: index);
  close_out oc;
;;

let _ = main Sys.argv.(1) Sys.argv.(2);;
