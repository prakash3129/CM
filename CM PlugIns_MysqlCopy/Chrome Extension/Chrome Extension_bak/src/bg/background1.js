// if you checked "fancy-settings" in extensionizr.com, uncomment this lines

// var settings = new Store("settings", {
//     "sample_setting": "This is how you use Store.js to remember values"
// });

var server = "localhost:60024"; // Change the port number for your app to something unique.
var maxreqlength = 1500; // This is a conservative limit that should work with all browsers.

//example of using a message handler from the inject scripts
chrome.extension.onMessage.addListener(
  function(request, sender, sendResponse) {
  	chrome.pageAction.show(sender.tab.id);
    sendResponse();
  });
  
chrome.contextMenus.create({title: "First Name", contexts:["selection"]});
chrome.contextMenus.create({title: "Last Name", contexts:["selection"]});
chrome.contextMenus.create({title: "Email", contexts:["selection"]});
chrome.contextMenus.create({title: "Jobtitle", contexts:["selection"]});
chrome.contextMenus.create({title: "Contact Link", contexts:["link"]});
chrome.contextMenus.onClicked.addListener(onClickHandler);


  // // Set up context menu at install time.
// chrome.runtime.onInstalled.addListener(function() 
// {
  // var context = "selection";
  // var title = "Bing for Selected Text";
  // var id = chrome.contextMenus.create({"title": title, "contexts":[context],"id": "context" + context});
// });
  
  // // Set up context menu at install time.
// chrome.runtime.onInstalled.addListener(function() 
// {
  // var context = "selection";
  // var title = "Google for Selected Text";
  // var id = chrome.contextMenus.create({"title": title, "contexts":[context],"id": "context" + context});
// });



// // add click event
// chrome.contextMenus.onClicked.addListener(onClickHandler);

// // The onClicked callback function.
// function onClickHandler(info, tab) {
  // var sText = info.selectionText;
  // var url = "https://www.google.com/search?q=" + encodeURIComponent(sText);  
  // window.open(url, '_blank');
// };

function onClickHandler(info, tab) {
	var selectedText = info.selectionText;
	if(selectedText)
	{
		_bufferText(escape(selectedText));
		_speakText();
	}
};

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