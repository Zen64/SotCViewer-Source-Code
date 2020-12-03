(* XXX: This is the test-file! *)

open Printf
open String
open Str
open List
open Int32

#load "str.cma";;

let getindex path =
  try
    let ic = open_in_bin path in
    let index = input_value ic in
    close_in ic;
    index
  with _ ->
    []
;;

(*  if true then Format.eprintf "%s " line;*)

(*let fin () = print_string fin\n ;;*)
(*val fin : unit -> unit = <fun>*)
(*
 "pop     esi"
  |> String.split_on_char ' '
  |> List.filter (fun s -> s <> "");;
(*- : string list = ["pop"; "esi"]*)
*)

(*open Core.Std*)
(*
let line = "hate   Ocaml !"
let tokenize line = String.split line ~on: ' ' |> List.dedup
*)
(*
let python_split x =
  String.split ~on:[ ' ' ; '\t' ; '\n' ; '\r' ]
 (*x |> List.filter ~f:(fun x -> x <> "")*)
;;
*)



(*
http://www.ffconsultancy.com/ocaml/bunny/index.html - there may be a way, but I'll loose a whole day on this stupid thing...

let split = Str.split (Str.regexp_string " ");;
val split : string -> string list = <fun>
*)


let split = Str.split (Str.regexp_string " ");;
(*val split : string -> string list = <fun>
*)


(* XXX: Part of the problem seems to be that the MSB of u32 is used for something so it is kind of an unsigned 31-bit variable. So when conv to Int32 it actually becomes u64 or s64 and the storing in the hashtbl becomes totally different - value 0x12 before it rather than 0x02 and then 8(?) rather than 4 bytes. 
Also mind that hashtbl offsets do matter and this is how entries are found. *)


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
		  let offset = Int32.to_int offs in (*This and the above change %x->%lx doen't do much, but at least it prevents the code crashing(err) in sscanf when an offset value >= 0x80000000 is found.*) 
		  (*An actual way to fix it would be to make the hashtab use Int32 (%lx), but that changes the actual hashtab and so requires modifications to Dormin Viewer (re-compiling it).*)
		  if true then eprintf "offset in %lx  out %x \n" offs offset;
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
