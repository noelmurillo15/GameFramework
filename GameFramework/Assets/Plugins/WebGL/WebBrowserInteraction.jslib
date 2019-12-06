var WebBrowserInteraction = {
    InitilaizeJsLib: function () 
    {
        console.log("WebBrowserInteraction::InitilaizeJsLib()")
        FS.syncfs(true, function (err) {        
            assert(!err);
            unityInstance.SendMessage('PersistantGameManager', 'LoadSettingsFromIndexedDB');
        });
    },
    SyncPersistantData : function()
    {
        console.log("WebBrowserInteraction::SyncPersistantData()")
        FS.syncfs(false, function (err) {
            assert(!err);
        });
    },
    QuitGame : function()
    {
        console.log("WebBrowserInteraction::QuitGame()");
        //unityInstance.Quit();
        //unityInstance = null;
    }
}
mergeInto(LibraryManager.library, WebBrowserInteraction);