/*
 * WebBrowserInteraction.jslib - A WebGL Plugin that allows JavaScript functionality to be invoked from C#
 * Created by : Allan N. Murillo
 * Last Edited : 2/17/2020
 */

var WebBrowserInteraction = {
    InitializeJsLib: function () 
    {
        FS.mkdir('/GameData');   
        FS.mount(IDBFS, {}, '/GameData');                
        FS.syncfs(true, function (err) 
        {       
            unityInstance.SendMessage('PersistentGameManager', 'LoadSettingsFromIndexedDb');
        });     
    },
    WindowFullscreen : function()
    {
        unityInstance.SetFullscreen(1);
    },
    CancelFullscreen : function()
    {
        unityInstance.SetFullscreen(0);
    },
    LostFocus : function()
    {
        window.alert("Game is now paused, Press OK to continue!");
    },
    QuitGame : function()
    {
        FS.syncfs(false, function (err) 
        {
            unityInstance.SetFullscreen(0);      
            CloseGame();            
        });        
    }        
}
mergeInto(LibraryManager.library, WebBrowserInteraction);