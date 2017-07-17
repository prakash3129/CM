//
// Determine whether the user is on a premium version of LinkedIn.
// These functions are used to parse correctly the profiles and inject correctly
// the button.
//

function isSalesNavigator() {
  if ($(".logo").text().trim() == "Sales Navigator") { return true; }
  else { return false; }
}

function isRecruiter() {
  if ($(".product span").first().text().trim() == "Recruiter") { return true; }
  else { return false; }
}
