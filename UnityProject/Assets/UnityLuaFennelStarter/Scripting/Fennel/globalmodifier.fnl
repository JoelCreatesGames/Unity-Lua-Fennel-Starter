; Fix lua's print to use one from our game
(fn _G.print [...]
  (each [_ x (ipairs [...])]
    (Game:Log (tostring x))))

; Convienience helpers for toggling language
(fn _G.lang [] 
  (set _G.usefennel (not _G.usefennel))
  ; TODO save state to save file so it persists beyond sesions
  (Game:ConsoleDisplay (.. "Now using " 
                            (if _G.usefennel "fennel" "lua")
                            " language")))
