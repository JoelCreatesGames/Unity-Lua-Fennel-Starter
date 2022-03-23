local function unityFnlSearcher(name)
    if _G["__gamemodules_fnl__" .. name] ~= nil then
        return assert(fennel.eval(_G["__gamemodules_fnl__" .. name].text))
    end
end
table.insert(package.searchers, 2, unityFnlSearcher)