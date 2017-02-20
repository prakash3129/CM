// if you checked 'fancy-settings' in extensionizr.com, uncomment this lines

// var settings = new Store('settings', {
//     'sample_setting': 'This is how you use Store.js to remember values'
// });

var server = 'localhost:60024'; // Change the port number for your app to something unique.
var maxreqlength = 1500; // This is a conservative limit that should work with all browsers.
var port = null;

//var Menus = new Array();
var CM_Fields = '';
var CM_Reply = '';
var CM_ProjectName = '';
var CM_CompanyID = '';
var CM_CompanyName = '';
var CM_Agent = '';
var Exceptions = '';


//example of using a message handler from the inject scripts
chrome.extension.onMessage.addListener(
  function(request, sender, sendResponse) {
  	chrome.pageAction.show(sender.tab.id);
    sendResponse();
  });





//localStorage['CM_Fields'] = '';
//localStorage['CM_ProjectName'] = '';
//localStorage['CM_CompanyID'] = '';
//localStorage['CM_CompanyName'] = '';
//localStorage['CM_Agent'] = '';
ResetAllValues();

Reload("cmd_ReloadFields::{Reload}", 'Soft');



//chrome.contextMenus.create(
//{
//    id: 'cmd_ReloadFields',
//    type: 'normal',
//    title: 'Reload Fields',
//    contexts: ['all']
//});

function ResetAllValues() {

    localStorage['CM_Fields'] = '';
    localStorage['CM_ProjectName'] = '';
    localStorage['CM_CompanyID'] = '';
    localStorage['CM_CompanyName'] = '';
    localStorage['CM_Agent'] = '';
    CM_Fields = '';
    CM_Reply = '';
    CM_ProjectName = '';
    CM_CompanyID = '';
    CM_CompanyName = '';
    CM_Agent = '';
    Exceptions = '';
}

function abc()
{
alert('called');
}

function onClickHandler(info, tab)
{
    var TexttoPass = '';
    //alert(info.menuItemId);
    try {

        //alert(localStorage["inputText"]);               

        if (typeof info.selectionText != 'undefined' && info.selectionText.length > 0)
        {
            TexttoPass = info.selectionText;
        }
        else if (typeof info.linkUrl != 'undefined' && info.linkUrl.length > 0)
        {
            TexttoPass = info.linkUrl;
        }
        else if (typeof info.pageUrl != 'undefined' && info.pageUrl.length > 0)
        {
            TexttoPass = info.pageUrl;
        }
    }
    catch (err)
    {
        Exceptions += "||" + err.stack + "TimeStamp:" + Timezone() + "|Agent:" + CM_Agent + "||";
        show('Campaign Manager', err.stack, 5000);
    }
    //alert(TexttoPass);

    if (TexttoPass.trim().length > 0)
    {
        Reload(info.menuItemId + "::{" + TexttoPass + "}" , 'User');
    }
};

chrome.contextMenus.onClicked.addListener(onClickHandler);

//chrome.contextMenus.create({ type: 'normal', title: 'group1 r1', contexts: ['selection'] });
//chrome.contextMenus.create({ type: 'normal', title: 'group1 r2', contexts: ['selection'] });
//chrome.contextMenus.create({ type: 'separator', contexts:['selection'] });
//chrome.contextMenus.create({ type: 'normal', title: 'group2 r1', contexts: ['selection'] });
//chrome.contextMenus.create({ type: 'normal', title: 'group2 r2', contexts: ['selection'] });

//createMenuItem({title: 'First Name', contexts:['selection']}, searchSelection);
//createMenuItem({ title: 'Last Name', contexts: ['selection'] }, searchSelection);
//chrome.contextMenus.create({ type: 'separator', contexts: ['selection'] });
//createMenuItem({title: 'Srini', contexts:['selection']}, searchSelection);
//createMenuItem({title: 'Jobtitle', contexts:['selection']}, searchSelection);


function createMenuItem(creationObject, onclickHandler) {
    if (onclickHandler) {
        creationObject.onclick = function(onClickData, tab) {
            onclickHandler(onClickData, tab, creationObject);
        };
    }
    return chrome.contextMenus.create(creationObject);
}


function Timezone()
{
    var offset = new Date().getTimezoneOffset();
    var minutes = Math.abs(offset);
    var hours = Math.floor(minutes / 60);
    var prefix = offset < 0 ? "+" : "-";
    return new Date().toLocaleString() + ":" + prefix + hours;
}



	
function show(title, message, timeout) {
try {
    // if (Notification.permission !== 'granted')
	 // {
		
		// Notification.requestPermission();
	 // }
	// else
	// {		
		// var notification = new Notification('Notification title', {
      // icon: 'chrome-extension://hojnpokbnekifeafedbhopdbgcoolnle/icons/icon48.png',
      // body: 'Campaign Manager Not reachable!',
    // });	
	// }
    var self = this;
    var isClosed = false;
    var notificationId = 'chNotify_' + Math.random();

    chrome.notifications.create(notificationId, {
        type: 'basic',
        title: title,
        message: message,
        iconUrl: '/icons/icon48.png'
    }, function (nId) {
    });

    //chrome-extension://' + chrome.runtime.id + '/icons/icon48.png

    setTimeout(function () {
        if (!isClosed)
            chrome.notifications.clear(notificationId, function (wasCleared) {
            });
    }, timeout);
} catch (e) {
    alert(e.message);
}
}

//// Usage:
//function searchSelection(info, tab, creationData)
//{
//   var selectedText = creationData.title + '  :  ' + info.selectionText;
//	if(selectedText)
//	{
	
//	//httpGet('http://localhost:60024/buffertext/dummy.gif?&timestamp=1458215758839');
		

	
//	}
//}

  // // Set up context menu at install time.
// chrome.runtime.onInstalled.addListener(function() 
// {
  // var context = 'selection';
  // var title = 'Bing for Selected Text';
  // var id = chrome.contextMenus.create({'title': title, 'contexts':[context],'id': 'context' + context});
// });
  
  // // Set up context menu at install time.
// chrome.runtime.onInstalled.addListener(function() 
// {
  // var context = 'selection';
  // var title = 'Google for Selected Text';
  // var id = chrome.contextMenus.create({'title': title, 'contexts':[context],'id': 'context' + context});
// });



// // add click event
// chrome.contextMenus.onClicked.addListener(onClickHandler);

// // The onClicked callback function.
// function onClickHandler(info, tab) {
  // var sText = info.selectionText;
  // var url = 'https://www.google.com/search?q=' + encodeURIComponent(sText);  
  // window.open(url, '_blank');
// };



function _formatCommand(command, args)
{
    // Add a timestamp to ensure the URL is always unique and hence
    // will never be cached by the browser.	
    alert('http://' + server + '/' + command + '/dummy.gif' + args + '&timestamp=' + new Date().getTime());
	return 'http://' + server + '/' + command + '/dummy.gif' + args + '&timestamp=' + new Date().getTime();
}



function ExecuteURL(URL) {
    try {
        // alert(theUrl);
        xmlhttp = new XMLHttpRequest();
        xmlhttp.open('GET', URL, false);
        var Res = '';
        xmlhttp.onreadystatechange = function () {
            if (xmlhttp.readyState == 4 && xmlhttp.status == 200) {
                Res = xmlhttp.responseText;
            }
        }
        //alert('send');
        xmlhttp.send();
        //alert('sent');
        Exceptions = '';
        return Res;
    }
    catch (err) {        
        Exceptions += "||" + err.stack + "TimeStamp:" + Timezone() + "|Agent:" + CM_Agent + "||";
        return "Error";        
    }

}



function ReloadMenu()
{
    chrome.contextMenus.removeAll(function () { console.log("contextMenus.removeAll callback"); });
    if (CM_Fields.length > 0) {        
        var Fields = CM_Fields.split("|");
        var Table = '';
        for (i = 0; i < Fields.length; i++) {
            var FieldsConfig = Fields[i].split("~");
            var Context = FieldsConfig[2].split(",");
            // alert("Context:" + Context);

            if (Table != FieldsConfig[0]) {

                Table = FieldsConfig[0];
                chrome.contextMenus.create({ type: 'separator', contexts: ['selection', 'link', 'page'] });
            }

            chrome.contextMenus.create(
            {
                id: 'chMnu_' + FieldsConfig[0] + '____' + FieldsConfig[1],
                type: 'normal',
                title: FieldsConfig[1],
                contexts: Context
            });
        }

        chrome.contextMenus.create({ type: 'separator', contexts: ['selection', 'link', 'page'] });
        chrome.contextMenus.create({ id: 'cmd_ReloadFields', type: 'normal', title: 'Refresh Menu', contexts: ['all'] });
        chrome.contextMenus.create({ id: 'cmd_AddContact', type: 'normal', title: 'New Contact', contexts: ['all'] });
    }
    else
    {
        chrome.contextMenus.create
        (
            {
                id: 'cmd_ReloadFields',
                type: 'normal',
                title: 'Refresh Menu',
                contexts: ['all']
            }
        );
        
    }
}

function CM_Response(CM_ResponseText)
{
    switch (CM_ResponseText)
    {
        case 'Received':
            //Do Nothing
            break;

        case 'Project':
            show('Campaign Manager', 'Project not logged in.', 4000);
            break;

        case 'Company':
            show('Campaign Manager', 'Company not opened.', 4000);
            break;

        case 'CompleteSurvey':
            show('Campaign Manager', 'Complete Survey company cannot be edited.', 4000);
            break;

        case 'Freeze':
            show('Campaign Manager', 'Selected contact is freezed.', 4000);
            break;

        case 'Reload':
            if(CM_Fields.length > 0)
                show('Campaign Manager', 'Menu refreshed.', 1000);
            else
                show('Campaign Manager', 'Selected project is not configured for Chrome communication.', 6000);
            break;

        case 'CMException':
            show('Campaign Manager', 'Error occured in Campaign Manager', 4000);
            break;

        case 'ContactAdded':
            show('Campaign Manager', 'New contact added.', 1000);
            break;

        case 'NotReceived':
            show('Campaign Manager', 'Data transfer unsuccessful.', 4000);
            break;

        case 'SaveBusy':
            show('Campaign Manager', 'Data saving in progress.', 4000);
            break;

        case 'ContactNotAdded':
            show('Campaign Manager', 'Cannot add contact.', 5000);
            break;
    }
}

 
function Reload(text, TriggerType)
{
    try
    {
		
        text = escape(text + 'ChromeException::{' + Exceptions + '}');
        var clearExisting = 'true';
        var reqs = Math.floor((text.length + maxreqlength - 1) / maxreqlength);
        var Response = '';
        for (var i = 0; i < reqs; i++)
        {
            var start = i * maxreqlength;
            var end = Math.min(text.length, start + maxreqlength);
            
            Response = ExecuteURL('http://localhost:60024/?totalreqs=' + reqs + '&req=' + (i + 1) + '&text=' + text.substring(start, end) + '&clear=' + clearExisting);
            
            //httpGet(_formatCommand('buffertext', '?totalreqs=' + reqs + '&req=' + (i + 1) + '&text=' + text.substring(start, end) + '&clear=' + clearExisting));
            clearExisting = 'false';
        }
        
        if (Response == 'Error') {
            ResetAllValues();
            ReloadMenu();
            show('Campaign Manager', 'Campaign Manager is not reachable!', 5000);
        }
        else {
            //alert(Response);
            if (Response.trim().length > 0 && Response.indexOf('}') != -1 && Response.split("}").length > 6)
            {                                
                var ResponseSplitArr = Response.split("}");

                ResponseSplitArr[0] = ResponseSplitArr[0].replace("Response:{", "").trim();
                ResponseSplitArr[1] = ResponseSplitArr[1].replace("Fields:{", "").trim();
                ResponseSplitArr[2] = ResponseSplitArr[2].replace("ProjectName:{", "").trim();
                ResponseSplitArr[3] = ResponseSplitArr[3].replace("CompanyID:{", "").trim();
                ResponseSplitArr[4] = ResponseSplitArr[4].replace("CompanyName:{", "").trim();
                ResponseSplitArr[5] = ResponseSplitArr[5].replace("Agent:{", "").trim();

                //alert(CM_Fields);
                //alert(ResponseSplitArr[1]);
                if (CM_Fields != ResponseSplitArr[1])
                {
                    CM_Fields = ResponseSplitArr[1];                    
                    localStorage['CM_Fields'] = CM_Fields;
                    ReloadMenu();
                }

                if (CM_Fields.length == 0) {
                    chrome.contextMenus.removeAll(function () { console.log("contextMenus.removeAll callback"); });
                    chrome.contextMenus.create({ id: 'cmd_ReloadFields', type: 'normal', title: 'Refresh Menu', contexts: ['all'] });                    
                    localStorage['CM_Fields'] = '';
                }
                
                if (CM_ProjectName != ResponseSplitArr[2]) {
                    CM_ProjectName = ResponseSplitArr[2];                    
                    localStorage['CM_ProjectName'] = CM_ProjectName;
                }
                
                if (CM_CompanyID != ResponseSplitArr[3]) {
                    CM_CompanyID = ResponseSplitArr[3];
                    CM_CompanyName = ResponseSplitArr[4];
                    localStorage['CM_CompanyID'] = CM_CompanyID;
                    localStorage['CM_CompanyName'] = CM_CompanyName;                    
                }

                
                if (CM_Agent != ResponseSplitArr[5]) {
                    CM_Agent = ResponseSplitArr[5];
                    localStorage['CM_Agent'] = CM_Agent;                    
                }

                if (TriggerType == 'User')
                    CM_Response(ResponseSplitArr[0]);
            }
            else
                show('Campaign Manager', 'Campaign Manager not responding!', 5000);
        }
    }
    catch (err) {
        Exceptions += "||" + err.stack + "TimeStamp:" + Timezone() + "|Agent:" + CM_Agent + "||";
        alert(err.stack);
        show('Campaign Manager', 'Campaign Manager is not reachable!', 5000);
    }

}



