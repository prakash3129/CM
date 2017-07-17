// Launch popup
//
function launchPopup() {
  appendOverlay(function() {
    openPopup(function() {
      launchSearch();

      // Analytics
      eventTrack("Open LinkedIn popup");
    });

    addAccountInformation();
  });

  // Drag popup
  $("#eh_popup").draggable({ handle: ".eh_popup_drag" });

  // Close popup
  $("#eh_overlay, .eh_popup_close").click(function() {
    closePopup();
  });
  $(document).keyup(function(e) {
    if (e.keyCode == 27) {
      closePopup();
    }
  });
}


// Destroy popup and overlay
//
function closePopup() {
  $("#eh_popup").remove();
  $("#eh_overlay").remove();
  $("#eh_account_popup").remove();
}


// Append overlay on the page
//
function appendOverlay(callback) {
  var docHeight = $(document).height();

  $("body").append('<div id="eh_overlay"></div>');

  $("#eh_overlay")
    .height(docHeight)
    .css({
      'opacity' : 0.4,
      'position': 'absolute',
      'top': 0,
      'left': 0,
      'background-color': 'black',
      'width': '100%',
      'z-index': 11000
  });

  callback();
}


// Show account information
//
function addAccountInformation() {
  getAccountInformation(function(json) {
    if (json == "none") {
      $("body").append('<div id="eh_account_popup"><div class="eh_account_not_logged">Not logged in. <a target="_blank" href="https://emailhunter.co/users/sign_in?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=browser_popup">Sign in</a> or <a target="_blank" href="https://emailhunter.co/users/sign_up?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=browser_popup">Create a free account</a></div></div>');
    }
    else {
      $("body").append('<div id="eh_account_popup"><a class="eh_leads_link" target="_blank" href="https://emailhunter.co/leads?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=browser_popup">My leads</a><div class="eh_deploy_account_information">'+json.email+' <i class="fa fa-caret-down"></i></div><div style="clear: both;"></div><div class="eh_account_information_detail"><div class="eh_plan_name">'+json.plan_name+' plan</div>'+numberWithCommas(json.calls.used)+" / "+numberWithCommas(json.calls.available)+' requests<br/><a href="https://emailhunter.co/subscription?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=linkedin_popup" class="clear_cta" target="_blank">Upgrade my account</a></div></div>');
      $(".eh_deploy_account_information").click(function() {
        $(".eh_account_information_detail").slideToggle(200);
      });
    }
  })
}


// Open popup
//
function openPopup(callback) {
  var windowHeight = $(window).height();
  var windowWidth = $(window).width();

  $("body").append('<div id="eh_popup"><a href="https://emailhunter.co/chrome?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=linkedin_popup#faq" target="_blank"><i class="fa fa-question-circle eh_popup_question"></i></a><i class="fa fa-ellipsis-v eh_popup_drag"></i><div class="eh_popup_close">&times;</div><div class="eh_popup_name">' + window.first_name + ' ' + window.last_name + '</div><div id="eh_popup_error"></div><form id="eh_popup_ask_domain"><div id="eh_popup_ask_domain_message"></div><input placeholder="company.com" id="eh_popup_ask_domain_field" type="text" name="domain"><button class="clear_cta" type="submit">Find</button></form><div id="eh_popup_content_container"><div id="eh_popup_content"></div><div id="eh_email_action_message"></div></div><div class="eh_popup_confidence_score"></div><div id="eh_popup_results_link_container"></div><div id="eh_popup_results_show"><div class="eh_popup_found_email_addresses"></div><div class="eh_popup_parsed_email_addresses"></div></div><div id="eh_popup_legal_mention">Email Hunter\'s button and this popup are added by Email Hunter\'s Chrome extension. Email Hunter is not affiliated to LinkedIn.</div></div>');

  $("#eh_popup")
    .css({
      'position': 'fixed',
      'top': windowHeight / 2 - 150,
      'left': windowWidth / 2 - 300,
      'padding': '30px',
      'width': '560px',
      'z-index': 11001
  });

  callback();
}


// Launch email search
//
function launchSearch() {
  if (window.last_company.length) {

    // Looking for domain name
    mainMessagePopup('Looking for ' + window.first_name + '\'s email address...', true);
    getWebsite(function(website) {
      if (typeof website !== "undefined") {
        window.domain = cleanDomain(website);

        $('#eh_popup_results_link_container').html('<div class="eh_popup_results_message">Looking for ' + window.domain + ' email addresses...</div>');

        // Use or not API key
        chrome.storage.sync.get('api_key', function(value){
          if (typeof value["api_key"] !== "undefined" && value["api_key"] !== "") {
            api_key = value["api_key"];
          }
          else { api_key = ''; }

          // Generate the email
          generate_email_endpoint = 'https://api.emailhunter.co/v1/generate?domain=' + window.domain + '&first_name=' + window.first_name + '&last_name=' + window.last_name + '&position=' + window.position + '&company=' + window.last_company;
          apiCall(api_key, generate_email_endpoint, function(email_json) {

            // We count call to measure use
            countCall();

            // Count how much email addresses there is on the domain
            count_endpoint = 'https://api.emailhunter.co/v1/email-count?domain=' + window.domain;
            apiCall(api_key, count_endpoint, function(count_json) {

              // If email addresses has NOT been found
              if (email_json.email == null) {

                // Maybe try to remove a subdomain if there is one
                if (withoutSubDomain(window.domain)) {
                  window.domain = withoutSubDomain(window.domain);
                  launchSearch();
                }
                else {
                  mainMessagePopup("No result.");
                  showResultsCountMessage(count_json.count);
                  $("#eh_popup_results_show").slideDown(300);

                  // If we have at least one email on the domain, we show it to help
                  if (count_json.count > 0) {
                    showEmailList();
                  }

                  // Maybe there are email addresses directly on the profile! Let's show them :)
                  showParsedEmailAddresses();
                }
              }

              // If email has been found
              else {
                showFoundEmailAddress(email_json, count_json);
                showParsedEmailAddresses();
                $("#eh_popup_results_show").slideDown(300);
              }

            askNewDomainListener();
            });
          });
        });
      }
      else {
        askDomainName(true);
      }
    });
  }
  else {
    if (typeof window.profile_main_content == "undefined") {
      showError("You don't have access to this profile.");
      $(".eh_popup_name").text("No access");
    } else {
      showError(window.first_name + ' has no current professional experience.');
    }
  }
}


// Show the main email address found
//
function showFoundEmailAddress(email_json, count_json) {
  mainMessagePopup(email_json.email);
  addCopyButton(email_json.email);
  showConfidence(email_json.score);

  chrome.storage.sync.get('api_key', function(value){
    if (typeof value["api_key"] !== "undefined" && value["api_key"] !== "") {
      api_key = value["api_key"];
    }
    else { api_key = ''; }

    addSaveButton(email_json.email, api_key);
  });

  if (count_json.count > 1) { es = 'es' }
  else { es = '' }
  $('#eh_popup_results_link_container').html('<a class="eh_popup_results_link" href="https://emailhunter.co/search/' + window.domain + '?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=linkedin_popup" target="_blank">' + count_json.count + ' email address' + es + ' for ' + window.domain + '<i class="fa fa-external-link"></i></a> <span class="eh_popup_separator">•</span> <span class="eh_popup_ask_domain">Try with an other domain name</span>');

  $('#eh_popup_results_link_container').slideDown(300);
}

// Add a copy button to copy the email address
//
function addCopyButton(email) {
  $("<div id='eh_copy_email_button' class='fa fa-files-o' data-toggle='tooltip' data-placement='top' title='Copy'></div>").insertBefore( "#eh_email_action_message" );
  $('#eh_copy_email_button').tooltip();

  $("#eh_copy_email_button").click(function() {
    executeCopy(email);
    displayActionMessage("Copied!");
    console.log("\""+email+"\" copied in the clipboard!");
  })
}


// Add a copy button to copy the email address
//
function addSaveButton(email, api_key) {
  $("<div id='eh_save_email_button' class='fa fa-floppy-o' data-toggle='tooltip' data-placement='top' title='Save the lead'></div>").insertBefore( "#eh_email_action_message" );
  $('#eh_save_email_button').tooltip();

  $("#eh_save_email_button").click(function() {
    $('#eh_save_email_button').tooltip("hide");
    $(this).remove();
    $("<div class='fa fa-spinner fa-spin eh_save_lead_loader'></div>").insertBefore("#eh_email_action_message");

    saveLead(email, api_key, function() {
      $(".eh_save_lead_loader").removeClass("fa-spinner fa-spin").addClass("fa-floppy-o");
      displayActionMessage("Saved!");
      console.log(email + " saved in leads!");
    });
  })
}


// Message displayed near actions
//
function displayActionMessage(message) {
  $("#eh_email_action_message").text(message);

  setTimeout(function(){
    $("#eh_email_action_message").text("");
  }, 2000);
}


// Show the number of email addresses found on a domain name
//
function showResultsCountMessage(results_number) {
  if (results_number == 0) {
    $(".eh_popup_found_email_addresses").append('<p>Nothing found on <strong>' + window.domain + '</strong>. Maybe <span class="eh_popup_ask_domain">try another domain name</span>?</p>');
  } else if (results_number == 1) {
    $(".eh_popup_found_email_addresses").append('<p>One email address found on ' + window.domain + ':</p>');
  } else {
    $(".eh_popup_found_email_addresses").append('<p>' + results_number + ' email addresses found on ' + window.domain + ':</p>');
  }
}


// Search for email addresses on a string (in this case, the page body)
//
function parseProfileEmailAddresses(string) {
  return string.match(/([a-zA-Z][\w+\-.]+@[a-zA-Z\d\-]+(\.[a-zA-Z]+)*\.[a-zA-Z]+)/gi);
}


// Display the list of email addresses directly found on the profile
//
function showParsedEmailAddresses() {
  if (typeof window.profile_main_content != "undefined") {
    email_addresses = parseProfileEmailAddresses(window.profile_main_content);
    if (email_addresses != null && email_addresses.length > 0) {
      var unique_email_addresses = [];
      $.each(email_addresses, function(i, el){
        if($.inArray(el, unique_email_addresses) === -1) unique_email_addresses.push(el);
      });

      $(".eh_popup_parsed_email_addresses").append("<hr>");
      if (unique_email_addresses.length == 1) {
        $(".eh_popup_parsed_email_addresses").append('<p>One email address found on the profile:</p>');
      }
      else {
        $(".eh_popup_parsed_email_addresses").append('<p>' + unique_email_addresses.length + ' email addresses found on this profile:</p>');
      }

      $.each(unique_email_addresses.slice(0,5), function(email_key, email_val) {
        $(".eh_popup_parsed_email_addresses").append('<div class="eh_popup_email_list">' + email_val + '</div>');
      });
    }
  }
}

// Show a list of email addresses found on the domain name
//
function showEmailList() {
  domain_search_endpoint = 'https://api.emailhunter.co/v1/search?domain=' + window.domain;
  apiCall(api_key, domain_search_endpoint, function(domain_json) {
    $.each(domain_json.emails.slice(0,5), function(email_key, email_val) {
      $(".eh_popup_found_email_addresses").append('<div class="eh_popup_email_list">' + email_val.value + '</div>');
    });

    $(".eh_popup_found_email_addresses").append('<div class="eh_popup_email_list"><a class="eh_popup_results_link" href="https://emailhunter.co/search/' + window.domain + '?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=linkedin_popup" target="_blank">See results for ' + window.domain + '<i class="fa fa-external-link"></i></a> <span class="eh_popup_separator">•</span> <span class="eh_popup_ask_domain">Try with another domain name</span></div>');
    askNewDomainListener();
  });

  $("#eh_popup_results_show").slideDown(300);
}


// Ask a new domain on click
//
function askNewDomainListener() {
  $(".eh_popup_ask_domain").click(function () {
    $("#eh_popup_results_link_container").hide();
    $("#eh_popup_results_show").hide();
    askDomainName(false);
  });
}


// Show the main message in popup on LinkedIn
//
function mainMessagePopup(message, loader) {
  console.log(message);
  loader = loader || false;
  if (loader == true) {
    loader_html = '<i class="fa fa-spinner fa-spin loader"></i>';
  }
  else { loader_html = ''; }

  $("#eh_popup_content").html(loader_html + message);
}


// Show confidence score
//
function showConfidence(score) {
  $(".eh_popup_confidence_score").html('<div class="eh_popup_confidence">' + score + '% confidence</div><div class="eh_popup_confidence_bar"><div class="eh_popup_confidence_level" style="width: ' + score + '%;"></div></div>');
  $(".eh_popup_confidence_score").show();
}


// Ask for the domain name
// Appends in two cases :
// - no domain name has been found
// - a domain has been found but gives no result
//
function askDomainName(showMessage) {
  $(".eh_popup_confidence_score").slideUp(300);

  $("#eh_popup_content_container").slideUp(300, function() {
    $("#eh_popup_ask_domain").slideDown(300, function() {
      $("#eh_popup_ask_domain_field").focus();
    });

    if (showMessage == true) {
      if (typeof window.domain == "undefined") {
        $("#eh_popup_ask_domain_message").html('We couldn\'t find <strong>' + window.last_company + '</strong> website. Please enter the domain name to launch the search. <a href="https://google.com/search?q= ' + window.last_company + '" target="_blank">Search the website on Google &#187;</a>');
      }
      else {
        $("#eh_popup_ask_domain_message").text('No email found with <strong>' + window.domain + '</strong>. Maybe try another domain name?');
      }
    }

    $("#eh_popup_ask_domain").submit(function() {
      $("#eh_popup_ask_domain").slideUp(300);
      $("#eh_popup_content_container").slideDown(300);
      window.domain = $("#eh_popup_ask_domain_field").val();
      launchSearch();

      return false;
    });
  });
}


// Throw an error
//
function showError(error) {
  $("#eh_popup_content_container").slideUp(300);
  $("#eh_popup_error").html(error).slideDown(300);
}


// Finds the domain name of the last experience or returns false
//
function getWebsite(callback) {
  if (typeof window.domain == "undefined") {
    if (typeof window.last_company != "undefined" && typeof window.last_company_path != "undefined") {
      if (window.last_company_path.indexOf("linkedin.com") > -1) {
        linkedin_company_page = window.last_company_path;
      } else {
        linkedin_company_page = "https://www.linkedin.com" + window.last_company_path;
      }
      $.ajax({
        url : linkedin_company_page,
        type : 'GET',
        success : function(response){
          website = websiteFromCompanyPage(response);
          if (website == "http://" || website == false) {
            askDomainName(true);
          }
          else {
            callback(website);
          }
        },
        error : function() {
          askDomainName(true);
        }
      });
    }
    else {
      askDomainName(true);
    }
  }
  else {
    callback(window.domain);
  }
}


// Ajax API call
// Use the API key if it is defined. If there is a limitation issue, show the right limitation message
//
function apiCall(api_key, endpoint, callback) {

  if (api_key != '') {
    api_key_param = '&api_key=' + api_key;
  }
  else if (endpoint.indexOf("email-count") == -1) {
    endpoint = endpoint.replace("https://api.emailhunter.co/v1/", "https://api.emailhunter.co/trial/v1/");
    api_key_param = '';
  } else {
    api_key_param = '';
  }

  $.ajax({
    url : endpoint + api_key_param,
    headers: {"Email-Hunter-Origin": "chrome_extension"},
    type : 'GET',
    dataType : 'json',
    success : function(json){
      callback(json);
    },
    error: function(xhr) {
      if (xhr.status == 400) {
        showError('Sorry, something went wrong on the query.');
      }
      else if (xhr.status == 401) {
        showError('Email Hunter Chrome extension seems not to be associated to your account. Please sign in to continue.<br/><br/><a href="https://emailhunter.co/users/sign_in?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=linkedin_popup" class="clear_cta" target="_blank">Sign in</a>');
      }
      else if (xhr.status == 429) {
        if (api_key != '') {
          showError('You\'ve reached your monthly quota. Please upgrade your account to continue using Email Hunter.<br/><br/><a href="https://emailhunter.co/subscription?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=linkedin_popup" class="clear_cta" target="_blank">Upgrade my account</a>');
        }
        else {
          showError('You\'ve reached your daily limit, please connect to your Email Hunter account to continue. It\'s free and takes 30 seconds.<br/><br/><a href="https://emailhunter.co/users/sign_up?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=linkedin_popup" class="clear_cta" target="_blank">Create a free account</a><a href="https://emailhunter.co/users/sign_in?utm_source=chrome_extension&utm_medium=extension&utm_campaign=extension&utm_content=linkedin_popup" class="eh_popup_signin_link" target="_blank">Sign in</a>');
        }
      }
      else {
        showError('Sorry, something went wrong. Please try again later.');
      }
    }
  });
}

// Save lead in Ajax
//
function saveLead(email, api_key, callback) {
  $.ajax({
    url : "https://api.emailhunter.co/v1/lead?first_name="+ window.first_name + "&last_name=" + window.last_name + "&company=" + window.last_company + "&position=" + window.position + "&email=" + email + "&website=http://" + window.domain + "&source=Email Hunter (LinkedIn)&linkedin_url=" + window.linkedin_url + "&api_key=" + api_key,
    headers: {"Email-Hunter-Origin": "chrome_extension"},
    type : 'POST',
    dataType : 'json',
    success : function(json){
      callback(json);
    }
  });
}

// Copy in email in LinkedIn popup
//
function executeCopy(text) {
    var input = document.createElement('textarea');
    $("#eh_popup").prepend(input);
    input.value = text;
    input.focus();
    input.select();
    document.execCommand('Copy');
    input.remove();
}

// Clean domain functions
//
function cleanDomain(website){
  domain = website.toLowerCase();
  domain = domain.allReplace({'https://': '', 'http://': '', 'www.': ''});
  domain = cleanUrlEnd(domain);

  return domain;
}

function cleanUrlEnd(str) {
  if (str.indexOf('/') != -1) {
    str = str.substring(0, str.indexOf('/'));
  }
  if (str.indexOf('?') != -1) {
    str = str.substring(0, str.indexOf('?'));
  }

  return str;
}

String.prototype.allReplace = function(obj) {
  var retStr = this
  for (var x in obj) {
    retStr = retStr.replace(new RegExp(x, 'g'), obj[x])
  }
  return retStr
}
