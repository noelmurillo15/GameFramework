# GameFramework Standalone (New Input System)
 - Basic framework for any game (Splash Scene, MainMenu Scene, Credits Scene, "Level 1" Scene )
 - JSON Settings file is saved to : "AppData/LocalLow/{*Company Name*}/{*Application Name*}/GameSettings.json"
 - In-Game pause menu fully functional
 - All of the UI in the gameframework soley depends on Unity's new Input System preview package
 - Input Handler responsible for GameFramework UI Input is a Scriptable Object that persists throughout scenes
 - UI works with keyboard, mouse & xbox gamepad
 
# GameFramework WebGL
 - Created a .jslib plugin for web browser interaction via Javascript
 - The plugin is able to send messages from a web browsers Javascript to Unity GameEngine ( and vice versa ) @ runtime 
 - JSON Settings file is saved to any web browser's Indexed Database via the .jslib plugin
 
# Other Comments
 - meant to be a foundation for any game (Build Tested & Found Working on Windows, Mac & WebGL)
 - originally intended just for me to make fast prototypes
