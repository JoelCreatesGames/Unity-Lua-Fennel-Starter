; Fix lua's print to use one from our game (wrapping Unity's Debug.Log)
(fn _G.print [...]
  (each [_ x (ipairs [...])]
    (Game:Log (tostring x))))

; Toggle the language and inform the user
(fn _G.lang [] 
  (set _G.usefennel (not _G.usefennel))
  ; TODO save state to save file so it persists beyond sesions
  (Game:ConsoleDisplay (.. "Now using " 
                            (if _G.usefennel "fennel" "lua")
                            " language")))
