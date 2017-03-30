//
// Inject Email Hunter button on Linkedin profile
//





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
		// if(AjaxDelay)
		// {
			// var clsName = $("#srp_main_").attr('class');			
			 
			// if(clsName == 'non-artdeco')
			// {
				// //while ($("#srp_main_").attr('class') == 'non-artdeco')
				// //{
					// //alert('non-artdeco passed');
					
					// // while ($("#srp_main_").attr('class') == 'non-artdeco loading')
					// // {
						
					// // }
					
				// //}
			// }
			// else if(clsName == 'non-artdeco loading')
			// {
				// while ($("#srp_main_").attr('class') == 'non-artdeco loading')
				// {}
			// }			
			// // alert($("#srp_main_").attr('class'));
							
			// // while ($("#srp_main_").attr('class') == 'non-artdeco loading')
			// // {
				// // alert('ss');
			// // }
			
		// }
		
		var icon =chrome.extension.getURL('icons/icon19.png');
		if (isSalesNavigator()) 
		{	
		$(".profile-actions").prepend('<a href="javascript:;" id="AddNewCon" class="button-primary"><img src="' + icon + '">Add Contact</a>');
		} 
		else if(isRecruiter()) 
		{	
			$(".profile-actions").prepend('<a href="javascript:;" id="AddNewCon" class="button-primary"><img src="' + icon + '">Add Contact</a>');
		}
		else 
		{	
			$(".profile-aux .profile-actions").prepend('<a href="javascript:;" id="AddNewCon" class="button-primary"><img src="' + icon + '">Add Contact</a>');
		}

		$("#results>li").each(function(index) {
			var id = $(this).attr("data-li-entity-id");
			$(this).children("div").children(".srp-actions").prepend('<a href="javascript:;" style="margin: 5px 5px 5px 0;" id="'+id+'" class="campaign primary-action-button"><img style="margin:0px 0px -5px 0px;" src="' + icon + '"></a>');
		});

		
		$(".submit-advs").click(function (){
			injectLinkedinButton(2000);
		});
		
		$("a").click(function (){
		if ($(this).attr('class') == 'page-link')
		{
			injectLinkedinButton(2000);
		}
		else if ($(this).attr('class') == 'campaign primary-action-button')
		{
			try
			{			
				var x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] a[class = 'title main-headline']";		
				alert('Name : ' + $(x).text());		
				x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] .bd .demographic bdi";
				alert('Location : ' + $(x).text());
				x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] .bd .demographic dd";
				alert('Industry : ' + $(x).last().text());
				x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] .bd .snippet .title";
				alert('Jobtitle : ' + $(x).last().text());		
				x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] .bd .primary-action-button.label";
				alert('Name : ' + $(x).attr('data-li-connect-href'));		
			}
			catch(err)
			{
			alert(err.stack);
			}
		}
			
		});
		
		
		},AjaxDelay);
	  //alert($("#results>li").attr("data-li-entity-id"));
	  
	}  
  //$(".profile-aux .profile-actions").prepend('<button disabled style="margin: 5px 5px 5px 0;" class="eh_linkedin_button eh_linked_connected"><img style="margin:4px 6px 0px 0px;" src="' + icon + '"></img><font style="margin-bottom:6px">Campaign
	catch(err)
	{alert(err.stack);}  
}


//
// Start JS injection
//
chrome.extension.sendMessage({}, function(response) {
  var readyStateCheckInterval = setInterval(function() {
    //if (isLoaded()) {		
      clearInterval(readyStateCheckInterval);	  
      launchEmailHunter();
    //}
  }, 20);
});


//
// Inject the button and start parsing
//

function launchEmailHunter() {
  injectLinkedinButton(100);

  // Parse the page (linkedin-dom.js)
  setTimeout(function(){
    parseLinkedinProfile();
    $(".eh_linkedin_button").prop("disabled", false);
	

    // Open popup on Linkedin profile
    $("a").click(function (){
		if ($(this).attr('class') == 'campaign primary-action-button')
		{
			try
			{
			
			var x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] a[class = 'title main-headline']";		
			alert('Name : ' + $(x).text());		
			x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] .bd .demographic bdi";
			alert('Location : ' + $(x).text());
			x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] .bd .demographic dd";
			alert('Industry : ' + $(x).last().text());
			x = "li[data-li-entity-id='" + $(this).attr("id")+ "'] .bd .snippet .title";
			alert('Jobtitle : ' + $(x).last().text());
			
			//alert('data-li-entity-id =' + $(this).attr("id"));
			// $(".profile-aux .profile-actions")
				// alert($(this).attr("id"));
				
			}
			catch(err)		
			{
			alert(err.stack);
			}
		}
	});
  }, parsingDuration());
}


//
// Is the profile ready?
//

function isLoaded() {	
  if (isRecruiter() && $(".send-inmail-split-button").length) {
    return true;
  }
  else if (isRecruiter() == false &&  document.readyState === "complete") {
    return true;
  }
  else {
    return false;
  }
    
}




//
// Time to wait to make sure the page is parsed
//

function parsingDuration() {
  if (isRecruiter()) { return 1000; }
  else { return 0; }
}


//
// --- linkedin-dom.js ---
//
// Every element depending on Linkedin DOM is put in this file.
// This should be updated on regular basis to make sure it works in every cases.
// Linkedin DOM changes depending on the type of account (free or premium
// versions like Sales Navigator or Recruiting)
//


//
// Get first name, last name
//

function getFullName() {
  if (isRecruiter()) {
    var full_name = $("title").text();
  }
  else {
    var full_name = $("title").text().substring(0, $("title").text().indexOf(" |"));
  }

  return cleanName(full_name);
}


//
// Get last company
//

function getLastCompany() {
  if (isSalesNavigator()) {
    last_company = $(".company-name").first().text();
  } else if (isRecruiter()) {
    last_company = $(".position-header h5").first().text();
  }
  else {
    last_company = $(".current-position h5:last-child a").first().text();
  }

  return last_company;
}

function getLastCompanyPath() {
  if (isSalesNavigator()) {
    last_company_path = $(".company-name a").first().attr("href");
  } else if (isRecruiter()) {
    if (typeof($(".position-header h5 a").first().attr("href")) != "undefined" &&
        $(".position-header h5 a").first().attr("href").indexOf("search?") == -1) {
      last_company_path = $(".position-header h5 a").first().attr("href");
    }
    else {
      last_company_path = undefined;
    }
  }
  else {
    last_company_path = $(".current-position .new-miniprofile-container a").first().attr("href");
  }

  return last_company_path;
}


//
// Get position
//

function getPosition() {
  if (isSalesNavigator()) {
    position = $(".position-title").first().text(); // TO DO
  }
  else if (isRecruiter()) {
    position = $(".position-header h4 a").first().text();
  }
  else {
    position = $(".current-position h4 a").first().text();
  }

  return position;
}

//
// Get LinkedIn URL
//

function getLinkedinUrl() {
  if (isSalesNavigator()) {
    url = $(".linkedin-logo").next().find("a").text();
  }
  else if (isRecruiter()) {
    url = "https://www.linkedin.com" + $(".public-profile a").attr("href");
  }
  else {
    url = $(".public-profile a").text();
  }

  return url;
}


//
// Profile main content
// Used to find email addresses directly available on the profile.
//
// Recruiter : $("#profile-ugc")
// Sales Navigator : $("#background")
// Free LinkedIn : $("#background")
//

function getMainProfileContent() {
  if (isRecruiter()) {
    profile_main_content = $("#profile-ugc").html();
  } else {
    profile_main_content = $("#background").html();
  }

  return profile_main_content;
}


//
// Website parse in company page
//

function websiteFromCompanyPage(html) {
  if (isSalesNavigator()) {
    html = $(html).find("code").last().html();
    json = html.replace("<!--", "").replace("-->", "");
    return JSON.parse(json)["account"]["website"];
  } else if(isRecruiter()) {
    html = $(html).find("#page-data").html();
    json = html.replace("<!--", "").replace("-->", "");
    return JSON.parse(json)["company"]["websiteUrl"];
  }
  else {
    if (typeof $(html).find(".website a").text() != "undefined" && $(html).find(".website a").text() != "") {
      return $(html).find(".website a").text();
    }
    else {
      html = $(html).find("code").last().html()
      json = html.replace("<!--", "").replace("-->", "");
      return JSON.parse(json)["website"];
    }
  }
}


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


//
// Launch the parsing when everything is ready (in linkedin-button.js)
//

function parseLinkedinProfile() {
  full_name_array = getFullName().split(" ");

  // First name
  window.first_name = full_name_array[0];
  full_name_array.shift();

  // Last name
  window.last_name = full_name_array.join(" ");

  // Position
  window.position = getPosition();

  // Company name
  window.last_company = getLastCompany();

  // Company path
  window.last_company_path = getLastCompanyPath();

  // Main content
  window.profile_main_content = getMainProfileContent();

  // LinkedIn URL
  window.linkedin_url = getLinkedinUrl();
}
