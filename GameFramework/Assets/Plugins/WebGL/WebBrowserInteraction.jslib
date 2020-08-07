/*
 * WebBrowserInteraction.jslib - A WebGL Plugin that allows JavaScript functionality to be invoked from C#
 * Created by : Allan N. Murillo
 * Last Edited : 8/5/2020
 */

var WebBrowserInteraction = {
    InitializeJsLib: function ()
    {
        console.log('[JS_LIB]: Initializing!');
        FS.mkdir('/GameData');
        FS.mount(IDBFS, {}, '/GameData');
        FS.syncfs(true, function (err)
        {
            assert(!err);
            console.log('[JS_LIB]: syncing with Indexed Database');
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
        window.alert("Game is Paused. Press OK to continue!");
    },
    OpenNewTab : function(url)
    {
        url = Pointer_stringify(url);
        window.open(url, 'blank');
    },
    QuitGame : function()
    {
        FS.syncfs(false, function (err)
        {
            assert(!err);

            if(err){
                console.log('[JS_LIB]: sync with IDB has Failed!');
            }
            else{
                console.log('[JS_LIB]: Finished sync with IDB - Closing Game');
                unityInstance.SetFullscreen(0);
                CloseGame();
            }
        });
    }
}
mergeInto(LibraryManager.library, WebBrowserInteraction);
