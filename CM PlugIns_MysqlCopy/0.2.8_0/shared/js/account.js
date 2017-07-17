function getAccountInformation(callback) {
  chrome.storage.sync.get('api_key', function(value){
    if (typeof value["api_key"] !== "undefined" && value["api_key"] !== "") {
      url = "https://api.emailhunter.co/v1/account?api_key="+value["api_key"];
      $.ajax({
        url : url,
        type : 'GET',
        dataType : 'json',
        success : function(json){
          return callback(json);
        }
      });
    }
    else {
      callback("none");
    }
  });
}
