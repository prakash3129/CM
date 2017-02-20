// BrowserSpeak
//
// by Mark Gladding
// Copyright 2009 Tumbywood Software
// http://www.text2go.com
//
// You are free to reuse this code in any commercial or non-commercial work.
//
// A bookmarklet to send a pausespeaking command to the BrowserSpeak application.

var server = "localhost:60024"; // Change the port number for your app to something unique.

_pauseSpeaking();
void 0; // Return from bookmarklet, ensuring no result is displayed.

function _pauseSpeaking()
{
    var image = new Image(1,1); 
    image.onerror = function() { _showerror(); };
    image.src = _formatCommand("pausespeaking"); 
}

function _formatCommand(command)
{
    return "http://" + server + "/" + command + "/dummy.gif" + "?timestamp=" + new Date().getTime(); 
}
    
function _showerror() 
{
    // Display the most likely reason for an error 
    alert("BrowserSpeak is not running. You must start BrowserSpeak first."); 
}

