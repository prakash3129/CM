//
// Inject Email Hunter button on Linkedin profile
//

var icon =chrome.extension.getURL('icons/icon19.png');	

var EmailAssistvar = false;
 //ocalStorage['EmailAssist'] = false;

chrome.runtime.onMessage.addListener(
  function(request, sender, sendResponse) {
    
	//alert("Message recived on injection page");
	
	//alert(request.CheckState);
	
   if (request.CheckState == "True")
	{
		EmailAssistvar = true;
	}	
	else
	{
		EmailAssistvar = false;
		$("body").html($("body").html().replace('highlightCM',''));
	}
	
	//alert(request.greeting);
	
	
      
  });



function isSalesNavigator() {
  if ($(".logo").text().trim() == "Sales Navigator") { return true; }
  else { return false; }
}

function isRecruiter() {
  if ($(".product span").first().text().trim() == "Recruiter") { return true; }
  else { return false; }
}




function injectLinkedinButton(AjaxDelay) 
{
	//var icon = chrome.extension.getURL('shared/img/icon48_white.png');
	try
	{
	setTimeout(function()
	{
	
		$(".profile-aux .profile-actions").prepend('<a href="javascript:;" id="AddNewCon" class="button-primary"><img src="' + icon + '">Add Contact</a>');
		
		AddSearchProfiles_Button();
		
		$(".submit-advs").click(function (){
			injectLinkedinButton(2000);
		});
		
		$("#AddNewCon").click(function (){
			GrabProfile_Page();			
		});
		
		$("a").click(function (){
		if ($(this).attr('class') == 'page-link')
		{			
			injectLinkedinButton(2000);			
		}
		else if ($(this).attr('class') == 'campaignManager primary-action-button')
		{		
			GrabProfile_searchResults($(this).attr("id"));		
		}			
		});
		},AjaxDelay);
	}    
	catch(err)
	{
		//alert(err.stack);
		console.log(err.stack);
		}  
}


function AddSearchProfiles_Button()
{
	//alert('button');
	
	if($('.campaignManager').length==0)
	{
		$("#results>li").each(function(index) {
			var id = $(this).attr("data-li-entity-id");
			$(this).children("div").children(".srp-actions").prepend('<a href="javascript:;" style="margin: 5px 5px 5px 0;" id="'+id+'" class="campaignManager primary-action-button"><img style="margin:0px 0px -5px 0px;" src="' + icon + '"></a>');
		});
	}		
}


function GrabProfile_Page()
{
	try
		{			
			var Selector = ".full-name";
			var Name = $.trim($(Selector).text());
			Selector = "#location .locality a";
			var Location = $.trim($(Selector).text());
			Selector = "#location .industry a";
			var Industry = $.trim($(Selector).text());
			Selector = ".editable-item.section-item.current-position h4 a";
			var Jobtitle = $.trim($(Selector).first().text());
			Selector = ".profile-card-extras dd a";
			var ContactLink = $.trim($(Selector).attr("href"));				
			Selector = "#email li a";
			var Email = $.trim($(Selector).text());
			
			var FName = '';	
			var LName = '';
			var City = '';
			var State = '';
			var Country = '';
			Name = $.trim(Name.split('(')[0]).replace('LinkedIn Member','');	
			Name = $.trim(cleanName(Name));			
			if(Name.split(' ').length == 2)
			{				
				FName = Name.split(' ')[0];
				LName = Name.split(' ')[1];					
			}
			else if(Name.split(' ').length == 1)
			{
				FName = Name;
			}
			else
			{
				FName = $.trim(Name.substring(0,Name.lastIndexOf(' ')));
				LName = $.trim(Name.substring(Name.lastIndexOf(' ')));					
			}
			
			if(ContactLink.length == 0)
			{
				ContactLink = window.location.href;
			}
			
				if(Location.length > 0)
			{				
				var LocArr = Location.split(',');
				if(LocArr.length == 2)
				{
					if(LocArr[0].lastIndexOf(' Area') > -1)
					{
						City = $.trim(LocArr[0].replace(' Area',''));
						Country = $.trim(LocArr[1]);
					}
					else if(LocArr[1].lastIndexOf(' Area') > -1)
					{
						City = $.trim(LocArr[0]);
						State = $.trim(LocArr[1].replace(' Area',''));						
					}	
				}
				else if(LocArr.length == 1)
				{
					if(LocArr[0].lastIndexOf(' Area') > -1)
					{
						City = $.trim(LocArr[0].replace(' Area',''));
					}
					else
					{
						Country = $.trim(LocArr[0]);
					}
				}
				else if(LocArr.length > 2)
				{
					
					City = $.trim(LocArr[0].replace(' Area',''));
					//State = $.trim(LocArr[1]).replace(' Area',''));
					Country = $.trim(LocArr[2]);
				}
			}
			//console.log('cn_fn:~:' + FName + '|~|cn_ln:~:' + LName + '|~|cn_city:~:' + City + '|~|cn_state:~:' + State + '|~|cn_country:~:' + Country + '|~|cn_ind:~:' + Industry + '|~|cn_jt:~:' + Jobtitle + '|~|cn_lnk:~:' + ContactLink);
							
			chrome.runtime.sendMessage({CMdata:'cn_fn:~:' + FName + '|~|cn_ln:~:' + LName + '|~|cn_city:~:' + City + '|~|cn_state:~:' + State + '|~|cn_country:~:' + Country + '|~|cn_ind:~:' + Industry + '|~|cn_jt:~:' + Jobtitle + '|~|cn_lnk:~:' + ContactLink});
		}
		catch(err)
		{
			//alert(err.stack);
			console.log(err.stack);
		}
}


function GrabProfile_searchResults(ResultID)
{
		try
		{			
			var Selector = "li[data-li-entity-id='" + ResultID+ "'] a[class = 'title main-headline']";
			var Name = $.trim($(Selector).text());
			Selector = "li[data-li-entity-id='" + ResultID + "'] .bd .demographic bdi";
			var Location = $.trim($(Selector).text());
			Selector = "li[data-li-entity-id='" + ResultID + "'] .bd .demographic dd";
			var Industry = $.trim($(Selector).last().text());
			Selector = "li[data-li-entity-id='" + ResultID + "'] .bd .snippet .title";
			var Jobtitle = $.trim($(Selector).last().text());
			Selector = "li[data-li-entity-id='" + ResultID + "'] a[class = 'title main-headline']";
			var ContactLink = $.trim($(Selector).attr("href"));
				
			var FName = '';	
			var LName = '';
			var City = '';
			var State = '';
			var Country = '';
			
			Name = $.trim(Name.split('(')[0]).replace('LinkedIn Member','');
			Name = $.trim(cleanName(Name));
			if(Name.split(' ').length == 2)
			{
				FName = Name.split(' ')[0]
				LName = Name.split(' ')[1]					
			}
			else if(Name.split(' ').length == 1)
			{
				FName = Name
			}
			else
			{
				Selector = "li[data-li-entity-id='" + ResultID + "'] .srp-actions .primary-action-button.label";
				var jasonContent = $.trim($(Selector).attr('data-li-connect-href'));									
				if(jasonContent.indexOf('firstName=') > -1 && jasonContent.indexOf('lastName=') > -1)
				{
				   FName =  $.trim(jasonContent.substring(jasonContent.indexOf('firstName=') + 10, jasonContent.indexOf('lastName=') - 1).replace('&','').replace('+',' '));
				   LName =  $.trim(jasonContent.substring(jasonContent.indexOf('lastName=') + 9, jasonContent.indexOf('isAjax=') -1).replace('&','').replace('+',' '));					   
					if(FName.indexOf('%') > -1 || LName.indexOf('%') > -1)
					{
						FName = $.trim(Name.substring(0,Name.lastIndexOf(' ')));
						LName = $.trim(Name.substring(Name.lastIndexOf(' ')));							
					}
				}
				else
				{
					FName = Name
				}
			}
			
			if(Jobtitle.indexOf(' at ') > -1)
			{
				Jobtitle = $.trim(Jobtitle.substring(0,Jobtitle.lastIndexOf(' at ')));					
			}
			else if(Jobtitle.indexOf(' - ') > -1)
			{
				Jobtitle = $.trim(Jobtitle.substring(0,Jobtitle.lastIndexOf(' - ')));					
			}
			else if(Jobtitle.indexOf('-') > -1)
			{
				Jobtitle = $.trim(Jobtitle.substring(0,Jobtitle.lastIndexOf('-')));					
			}
			
			if(Location.length > 0)
			{				
				var LocArr = Location.split(',');
				//console.log(LocArr);
				if(LocArr.length == 1)
				{
					if(LocArr[0].lastIndexOf(' Area') > -1)					
						City = $.trim(LocArr[0].replace(' Area',''));					
					else					
						Country = $.trim(LocArr[0]);					
				}
				else if(LocArr.length == 2)
				{
					if(LocArr[0].lastIndexOf(' Area') > -1)
					{
						City = $.trim(LocArr[0].replace(' Area',''));
						Country = $.trim(LocArr[1]);
					}
					else if(LocArr[1].lastIndexOf(' Area') > -1)
					{
						City = $.trim(LocArr[0]);
						State = $.trim(LocArr[1].replace(' Area',''));						
					}					
				}				
				else if(LocArr.length > 2)
				{
					
					City = $.trim(LocArr[0].replace(' Area',''));
					//State = $.trim(LocArr[1].replace(' Area',''));
					Country = $.trim(LocArr[2]);
				}
			}
			
			chrome.runtime.sendMessage({CMdata:'cn_fn:~:' + FName + '|~|cn_ln:~:' + LName + '|~|cn_city:~:' + City + '|~|cn_state:~:' + State + '|~|cn_country:~:' + Country + '|~|cn_ind:~:' + Industry + '|~|cn_jt:~:' + Jobtitle + '|~|cn_lnk:~:' + ContactLink});
			
			// chrome.runtime.sendMessage({greeting: "thangaprakash"}, function(response) {
			  // console.log(response.farewell);
			// });			
		}
		catch(err)
		{
			//alert(err.stack);
			console.log(err.stack);
		}
}

    


var activeTab = (function(){
    var stateKey, eventKey, keys = {
        hidden: "visibilitychange",
        webkitHidden: "webkitvisibilitychange"
    };

    for (stateKey in keys) {
        if (stateKey in document) {
            eventKey = keys[stateKey];
            break;
        }
    }

    return function(c) {
        if (c) document.addEventListener(eventKey, c);
        return !document[stateKey];
    }
})();



function EmailAssist()
{
//alert('email');
	try
	{
	
		//var Chunk = ; //Fetching Entire Body
		//$('em').contents().unwrap();
		//$("body").highlight('/\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}\b/ig');
		//return;
		
		//var EmailChunk = $("body").html().match(/\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}\b/ig);		
		
		//alert(localStorage['EmailAssist']);
				
		if(EmailAssistvar)
		{
			var EmailChunk = $("body").html().replace('<wbr>','').match(/[A-Z0-9._%+-\s<>/]+@[A-Z0-9.-<>/]+\.[A-Z]{2,6}/ig);		
			if(EmailChunk != null && EmailChunk != undefined && EmailChunk.length > 0)
			{
				var EmailChunk1 = $("body").text().match(/\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}\b/ig);
				//console.log(EmailChunk1);
				EmailChunk = EmailChunk.concat(EmailChunk1);
				//console.log(EmailChunk);
				//console.log(EmailChunk);			
				$('.highlightCM').removeHighlight();
				//if(EmailChunk.length != $('.highlightCM').length)
				{
					var Emails = [];
					$.each(EmailChunk, function(i, e)
					{
						if(e.startsWith('>'))
							e =  e.slice( 1 );
						if ($.inArray($.trim(e), Emails) == -1)
							Emails.push($.trim(e));
					});
					
					console.log(Emails);
					
					for (var property in Emails)
					{
						if(Emails[property].indexOf('<') > -1)
						{	
							try
							{
							//alert(Emails[property]);
							 $("body").html($("body").html().replace('<wbr>','').replace(Emails[property],"<span class = 'highlightCM'>" + Emails[property] + "</span>"));
							}
							catch(err){}
						}
						else
							$("body").highlight(Emails[property]);
						//highlightInElement(Emails[property]);
						// var src_str = $("body").html();	
						// var term = Emails[property];
						// term = term.replace(/(\s+)/,"(<[^>]+>)*$1(<[^>]+>)*");
						// var pattern = new RegExp("(" + term + ")", "gi");

						// src_str = src_str.replace(pattern, "<mark>$1</mark>");
						// src_str = src_str.replace(/(<mark>[^<>]*)((<[^>]+>)+)([^<>]*<\/mark>)/,"$1</mark>$2<mark>$4");
						// //console.log(src_str);
						// $("body").html(src_str);	


					
									
					}
					
					
					// console.log('Highlight:' + $('.highlightCM').length);
					// console.log('Emails:' + Emails.length);	
				}
			}
		}
	}
	catch(err)
	{
		//alert(err.stack);
		console.log(err.stack);
	}
	
	
	
	
}


try
{

//
// Start JS injection
//
//alert('link');
if(document.URL.indexOf('linkedin.com')>-1)
{

	injectLinkedinButton(100);
	if(document.URL.indexOf('linkedin.com/vsearch/p?')>-1)
	{	
		setInterval(function(){AddSearchProfiles_Button();}, 5000);
	}
}


// try
// {
// var timeout = null;
// document.addEventListener("DOMSubtreeModified", function() 
// {
// alert('tree changed');
    // if(timeout) 
	// {
        // clearTimeout(timeout);
    // }
    // if(activeTab() == true) 
	// {
	    // timeout = setTimeout(EmailAssist, 500);
	// }
// }, false);
// }
// catch(err)
// {
// alert(err.stack);
// }

//setInterval(function(){console.log(document.readyState);}, 100);


	// document.onreadystatechange = function () {
		// console.log(document.readyState);
		// if (document.readyState == "complete") {
			// setTimeout(EmailAssist, 3000);
		// }
	// }


	window.addEventListener('focus', function() {
		//alert(localStorage['EmailAssist']);
		//if(localStorage['EmailAssist'])
			EmailAssist();
	});

	// var timeout = null;
	// document.addEventListener("DOMSubtreeModified", function() {
		// if(timeout) {
			// clearTimeout(timeout);
		// }
		// if(activeTab() == true) {
			// timeout = setTimeout(EmailAssist, 2000);
		// }
	// }, false);
}
catch(err)
{
//alert(err.stack);
console.log(err.stack);
}

//setInterval(function(){EmailAssist();}, 2000);

//EmailAssist();
	  

//
// Clean the name by removing some titles
//

function cleanName(full_name) {
  String.prototype.allReplace = function(obj) {
      var retStr = this;
      for (var x in obj) {
          retStr = retStr.replace(new RegExp(x, 'g'), obj[x]);
      }
      return retStr;
  };

  return full_name.allReplace(
    {
      ',? Jr.?': '',
      ',? Sr.?': '',
      ',? MBA': '',
      ',? CPA': '',
      ',? PhD': '',
      ',? MD': '',
      ',? MHA': '',
      ',? CGA': '',
      ',? ACCA': '',
      ',? PMP': '',
      ',? MSc': ''
    }
  );
}

