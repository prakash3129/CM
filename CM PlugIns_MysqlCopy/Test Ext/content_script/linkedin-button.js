//
// Inject Email Hunter button on Linkedin profile
//

alert('sc');
function injectLinkedinButton() {
  var icon = chrome.extension.getURL('shared/img/icon48_white.png');

  if (isSalesNavigator()) {
    $(".profile-actions").prepend('<button disabled style="margin: 0 10px 0 0;" class="eh_linkedin_button eh_linked_connected"><img src="' + icon + '">Campaign Manager</button>');
  } else if(isRecruiter()) {
    $(".profile-actions").prepend('<button disabled style="margin: 0 10px 0 0;" class="eh_linkedin_button eh_linkedin_button_small eh_linked_connected"><img src="' + icon + '">Campaign Manager</button>');
  }
  else {
    $(".profile-aux .profile-actions").prepend('<button disabled style="margin: 5px 5px 5px 0;" class="eh_linkedin_button eh_linked_connected"><img src="' + icon + '">Campaign Manager</button>');
  }
}



//
// Start JS injection
//
chrome.extension.sendMessage({}, function(response) {
  var readyStateCheckInterval = setInterval(function() {
    if (isLoaded()) {
      clearInterval(readyStateCheckInterval);
      launchEmailHunter();
    }
  }, 20);
});


//
// Inject the button and start parsing
//

function launchEmailHunter() {
  injectLinkedinButton();
	
  // Parse the page (linkedin-dom.js)
  setTimeout(function(){
    parseLinkedinProfile();
    $(".eh_linkedin_button").prop("disabled", false);

    // Open popup on Linkedin profile
    $(".eh_linkedin_button").click(function() {
      launchPopup();
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
