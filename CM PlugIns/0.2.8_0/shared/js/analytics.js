// Counts the main actions made with the extension to know which features
// are the most successful
//

function eventTrack(eventName) {
  url = "https://emailhunter.co/events?name="+eventName;
  $.ajax({
    url : url,
    type : 'GET',
    dataType : 'json',
    success : function(json){
      // Done!
    }
  });
}
