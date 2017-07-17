

//alert(localStorage['CM_Fields']);

try {
//    alert(document.getElementById("1").value);

    // document.getElementById("CM_ProjectName").innerHTML = localStorage['CM_ProjectName'];
    // //document.getElementById("CM_CompanyID").innerHTML = localStorage['CM_CompanyID'];
    // document.getElementById("CM_CompanyName").innerHTML = localStorage['CM_CompanyName'];
    // document.getElementById("CM_Agent").innerHTML = localStorage['CM_Agent'];
	

	$('#CM_ProjectName').html(localStorage['CM_ProjectName']);
	//$('#CM_CompanyID').html(localStorage['CM_CompanyID']);
	$('#CM_CompanyName').html(localStorage['CM_CompanyName']);
	$('#CM_Agent').html(localStorage['CM_Agent']);

	//alert(localStorage['EmailAssist']);
	
	if(localStorage['EmailAssist'] == '1')
	{
		$('#Chkb').attr('checked',true)
	}
	else
	{
		$('#Chkb').attr('checked',false)
	}
	
	

        // do stuff here. It will fire on any checkbox change
		
		


	
	// $('#EmailAssist').on('change', function() { 
    // // From the other examples
	
	// alert('check change');
    // // if (!this.checked) {
        // // var sure = confirm("Are you sure?");
        // // this.checked = !sure;
        // // $('#textbox1').val(sure.toString());
    // // }
// });
    //alert(document.getElementById("23").innerHTML);

	$("#Chkb").change(function (){			
	if($(this).is(":checked"))
	{
				
			// chrome.tabs.query
			// (
				// {active: true, currentWindow: true}, function(tabs) 
				// {
					// chrome.tabs.sendMessage(tabs[0].id, {CheckState: "True"}, function(response) {});
				// }
			// );
			
			
			chrome.tabs.query({}, function(tabs) 
				{
					var message = {CheckState: "True"};
					for (var i=0; i<tabs.length; ++i) 
					{
						chrome.tabs.sendMessage(tabs[i].id, message);
					}
				}
			);
			
			localStorage['EmailAssist'] = '1';
			
			
	}
	else
	{
		
			chrome.tabs.query({}, function(tabs) 
				{
					var message = {CheckState: "False"};
					for (var i=0; i<tabs.length; ++i) 
					{
						chrome.tabs.sendMessage(tabs[i].id, message);
					}
				}
			);
			
			localStorage['EmailAssist'] = '0';
	}	
});

}
catch (err) {
alert(err.stack);
}
//alert(localStorage['CM_CompanyID']);
//alert(localStorage['CM_CompanyName']);
//alert(localStorage['CM_Agent']);



