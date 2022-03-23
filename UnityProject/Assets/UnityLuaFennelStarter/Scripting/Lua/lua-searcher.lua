local function unityLuaSearcher(name)
    if _G["__gamemodules_lua__" .. name] ~= nil then
        return assert(load(_G["__gamemodules_lua__" .. name].text))
    end
end
table.insert(package.searchers, 2, unityLuaSearcher)
