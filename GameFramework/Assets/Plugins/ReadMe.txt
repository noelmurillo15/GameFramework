In order to create a .jslib file, you need to create a js script.

Basic structure of a .jslib file :
var WebGL_JsLib = {
    SyncJS: function () {
        window.alert("Hello, world!");
        console.log("This is SyncJS")
    },
    Close: function () {
        if ((typeof window.wsclient !== "undefined") && (window.wsclient !== null))
            window.wsclient.close();
    }
}
mergeInto(LibraryManager.library, WebGL_JsLib);


then SaveAs a .jslib file and it should automatically update the file extension


If you did it correctly, when you go back to the Unity Editor and it reloads, the .jslib file icon should be changed automatically to an Unity Extension Icon


Use Case in C# :

#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void SyncJS();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void Close();
#endif