local function evaluator(modulename)
    return assert(fennel.eval(_G["__gamemodules_fnl__" .. modulename].text))
end

local function unityFnlSearcher(name)
    if _G["__gamemodules_fnl__" .. name] ~= nil then
        return evaluator
    end
end

table.insert(package.searchers, 2, unityFnlSearcher)