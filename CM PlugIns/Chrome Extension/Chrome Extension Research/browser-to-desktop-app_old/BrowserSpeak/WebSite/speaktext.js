// BrowserSpeak
//
// by Mark Gladding
// Copyright 2009 Tumbywood Software
// http://www.text2go.com
//
// You are free to reuse this code in any commercial or non-commercial work.
//
// A bookmarklet to send a speaktext command to the BrowserSpeak application.

var server = "localhost:60024"; // Change the port number for your app to something unique.
var maxreqlength = 1500; // This is a conservative limit that should work with all browsers.

var selectedText = _getSelectedText();
if(selectedText)
{
    _bufferText(escape(selectedText));
    _speakText();
}
void 0; // Return from bookmarklet, ensuring no result is displayed.

function _getSelectedText()
{
    // Get the current text selection using a cross-browser compatible technique.
    if (window.getSelection) 
        return window.getSelection().toString();
    else if (document.getSelection) 
        return document.getSelection(); 
    else if (document.selection) 
        return document.selection.createRange().text; 
    return null;
}

function _formatCommand(command, args)
{
    // Add a timestamp to ensure the URL is always unique and hence
    // will never be cached by the browser.
    return "http://" + server + "/" + command + "/dummy.gif" + args + "&timestamp=" + new Date().getTime(); 
}

function _speakText()
{
    var image = new Image(1,1); 
    image.onerror = function() { _showerror(); };
    image.src = _formatCommand("speaktext", "?source=" + document.URL); 
}
    
function _bufferText(text)
{
    var clearExisting = "true"; 
    var reqs = Math.floor((text.length + maxreqlength - 1) / maxreqlength);
    for(var i = 0; i < reqs; i++)
    {
        var start = i * maxreqlength;
        var end = Math.min(text.length, start + maxreqlength);
        var image = new Image(1,1); 
        image.onerror = function() { _showerror(); };
        image.src = _formatCommand("buffertext", "?totalreqs=" + reqs + "&req=" + (i + 1) + "&text=" + text.substring(start, end) + "&clear=" + clearExisting); 
        clearExisting = "false";
    }
}

function _showerror() 
{
    // Display the most likely reason for an error 
    alert("BrowserSpeak is not running. You must start BrowserSpeak first."); 
}

