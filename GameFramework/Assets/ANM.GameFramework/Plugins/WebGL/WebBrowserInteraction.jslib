/*
 * WebBrowserInteraction.jslib - A WebGL Plugin that allows JavaScript functionality to be invoked from C#
 * Created by : Allan N. Murillo
 * Last Edited : 12/18/2020
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
        console.log('[JS_LIB]: Fullscreen ON');
        //unityInstance.SetFullscreen(1);
    },
    CancelFullscreen : function()
    {
        console.log('[JS_LIB]: Fullscreen OFF');
        //unityInstance.SetFullscreen(0);
    },
    LostFocus : function()
    {
        unityInstance.SendMessage('PersistentGameManager', 'RaisePause');
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
                //CloseGame();
            }
        });
    }
}
mergeInto(LibraryManager.library, WebBrowserInteraction);
