// if you checked "fancy-settings" in extensionizr.com, uncomment this lines

// var settings = new Store("settings", {
//     "sample_setting": "This is how you use Store.js to remember values"
// });

var server = "localhost:60024"; // Change the port number for your app to something unique.
var maxreqlength = 1500; // This is a conservative limit that should work with all browsers.
var port = null;

//example of using a message handler from the inject scripts
chrome.extension.onMessage.addListener(
  function(request, sender, sendResponse) {
  	chrome.pageAction.show(sender.tab.id);
    sendResponse();
  });
  
  connect();
  
  
  function connect() {
var hostName = "com.cm.web";
alert("Connecting to native messaging host :: " + hostName);
port = chrome.runtime.connectNative(hostName);
port.onMessage.addListener(onNativeMessage);
port.onDisconnect.addListener(onDisconnected);
}

function appendMessage(text) {
alert(text);
}

function onNativeMessage(message) {
alert("Received message: " + JSON.stringify(message));
}

function onDisconnected() {
alert("Failed to connect: " + chrome.runtime.lastError.message);
port = null;
}

// chrome.contextMenus.create({title: "First Name", contexts:["selection"]});
// chrome.contextMenus.create({title: "Last Name", contexts:["selection"]});
// chrome.contextMenus.create({title: "Email", contexts:["selection"]});
// chrome.contextMenus.create({title: "Jobtitle", contexts:["selection"]});
// chrome.contextMenus.create({title: "Contact Link", contexts:["link"]});
//chrome.contextMenus.onClicked.addListener(onClickHandler);

createMenuItem({title: "First Name", contexts:["selection"]}, searchSelection);
createMenuItem({title: "Last Name", contexts:["selection"]}, searchSelection);
createMenuItem({title: "Srini", contexts:["selection"]}, searchSelection);
createMenuItem({title: "Jobtitle", contexts:["selection"]}, searchSelection);


function createMenuItem(creationObject, onclickHandler) {
    if (onclickHandler) {
        creationObject.onclick = function(onClickData, tab) {
            onclickHandler(onClickData, tab, creationObject);
        };
    }
    return chrome.contextMenus.create(creationObject);
}

function httpGet(theUrl)
{
    if (window.XMLHttpRequest)
    {// code for IE7+, Firefox, Chrome, Opera, Safari
        xmlhttp=new XMLHttpRequest();
    }
    else
    {// code for IE6, IE5
        xmlhttp=new ActiveXObject("Microsoft.XMLHTTP");
		alert('ACTIVEX');
    }	
	xmlhttp.open("GET", theUrl, false);	
    xmlhttp.onreadystatechange=function()
    {		
        if (xmlhttp.readyState==4 && xmlhttp.status==200)
        {
             alert(xmlhttp.responseText)
        }
    }    
    xmlhttp.send();    
}

// Usage:
function searchSelection(info, tab, creationData) {
   var selectedText = creationData.title + "  :  " + info.selectionText;
	if(selectedText)
	{
	

	
	httpGet('http://localhost:60024/speaktext/dummy.gif?&timestamp=1458215758839');
	//document.getElementById("p1").innerHTML = 'http://localhost:60024/speaktext/dummy.gif?&timestamp=1458215758839';
	//window.open('http://localhost:60024/speaktext/dummy.gif?&timestamp=1458215758839');
		
	_bufferText(escape(selectedText));
	//_speakText();
	}
}



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
	var selectedText = info.selectionText + info.title;
	if(selectedText)
	{
		_bufferText(escape(selectedText));
		//_speakText();
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

function readTextFile(file)
{
    var rawFile = new XMLHttpRequest();
    rawFile.open("GET", file, false);
    rawFile.onreadystatechange = function ()
    {
        if(rawFile.readyState === 4)
        {
            if(rawFile.status === 200 || rawFile.status == 0)
            {
                var allText = rawFile.responseText;
                alert(allText);
            }
        }
    }
    rawFile.send(null);
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
        //window.location.inner = _formatCommand("buffertext", "?totalreqs=" + reqs + "&req=" + (i + 1) + "&text=" + text.substring(start, end) + "&clear=" + clearExisting);

        //alert(window.location.innerHTML);

		//var html = document.getElementsByTagName('html')[0];
		//var text = html.innerHTML;
              

		//var abc = _formatCommand("buffertext", "?totalreqs=" + reqs + "&req=" + (i + 1) + "&text=" + text.substring(start, end) + "&clear=" + clearExisting); 				
        clearExisting = "false";
    }
}




function _showerror() 
{
    // Display the most likely reason for an error 
    alert("BrowserSpeak is not running. You must start BrowserSpeak first."); 
}