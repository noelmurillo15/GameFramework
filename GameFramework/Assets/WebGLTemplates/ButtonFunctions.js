/*
 * ButtonFunctions.js - Used to load/close an iFrame for the Unity Game
 * Used by the custom index.html & the JsLib Plugin
 * Created by : Allan N. Murillo
 * Last Edited : 2/12/2020
 */

function LoadGame() {
    let myFrame = document.getElementById('iMainFrame');
    if(myFrame){
        myFrame.src = "./game.html";
    }
}

function CloseGame() {
    let myFrame = parent.document.getElementById("iMainFrame");
    if(myFrame && myFrame.src !== "")
        myFrame.src = "";
}
