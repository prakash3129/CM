
// var icon =chrome.extension.getURL('icons/icon19.png');	
var recognizing = false;
var ignore_onend;


function injectButton(AjaxDelay) 
{
	//var icon = chrome.extension.getURL('shared/img/icon48_white.png');
	try
	{
		//alert('sc');
		setTimeout(function()
		{			
			$("textarea").each(function(index) 
			{
				//console.log($(this).attr("id"));				
				if($(this).attr("cols") != undefined)
				{
					//console.log("\tCols exist");
					if($(this).next().attr("id") != undefined)
					{
						var nxtElementID = $(this).next().attr("id");
						if(nxtElementID.indexOf('Merit_SST') == -1)
						{							
							var id = $(this).attr("id");
							$(this).after('<input type = "button" class="btn Merit_SST" id = Merit_SST_' + id + ' value = "Speak"/>');
							rec('Merit_SST_' + id);
						}
						else
						{
							//console.log('Already exist');
						}
					}
					else
					{
						var id = $(this).attr("id");
						$(this).after('<input type = "button" class="btn Merit_SST" id = Merit_SST_' + id + ' value = "Speak"/>');
						rec('Merit_SST_' + id);
					}
				}
				else
				{
					if($(this).next().attr("id") != undefined)
					{
						var nxtElementID = $(this).next().attr("id");
						if(nxtElementID.indexOf('Merit_SST')>-1)
							$(this).next().remove();
					}
				}
			});
		
			
						
			
			// $(".Merit_SST").each(function(index) 
			// {		
				// //console.log($(this).attr("SSTBTN"));
				// if($(this).attr("SSTBTN") === undefined)
				// {
					// $(this).click(function()
					// {
						// console.log("Speak Click:" + $._data(this, 'events').click);
						// $(this).attr("SSTBTN","True");
						// var txtAreaID = $(this).attr("id").replace('Merit_SST_','');
						// var btnID = $(this).attr("id");
						// //console.log($(this).attr('class'));
						// try
						// {	
							// if (!('webkitSpeechRecognition' in window)) 
							// {			
								// alert('Chrome version not supported. Upgrade your Chrome.');
							// } 
							// else 
							// {			
								// if (recognizing) 
								// {
									// recognition.stop();
									// return;
								// }			
								// recognition = new webkitSpeechRecognition();
								// recognition.continuous = true;
								// recognition.interimResults = true;		
								// recognition.onstart = function() 
								// {
									// recognizing = true;
									// //showInfo('info_speak_now');
									// //$("#" + btnID).val('Stop');
									// $(document.getElementById(btnID)).val('Stop');
								// };
								
								// recognition.onresult = function(event) 
								// {
									// try
									// {
										// recognizing = true;
										// var interim_transcript = '';
										// for (var i = event.resultIndex; i < event.results.length; ++i) 
										// {
											// if (event.results[i].isFinal) 
											// {
												// final_transcript += event.results[i][0].transcript;
											// } 
											// else 
											// {
												// interim_transcript += event.results[i][0].transcript;
											// }
										// }    
										
										// //console.log("#" + txtAreaID);
										// $(document.getElementById(txtAreaID)).val(interim_transcript);
										// //$("#" + txtAreaID).val(interim_transcript);	
										// if(final_transcript.length > 0)
											// $(document.getElementById(txtAreaID)).val(final_transcript);
											// //$("#" + txtAreaID).val(final_transcript);	
									// }
									// catch(err)
									// {		
										// alert(err.stack);
									// }
								// };
					  
								// recognition.onend = function() 
								// {
									// recognizing = false;
									// //$("#" + btnID).val('Speak');
									// $(document.getElementById(btnID)).val('Speak');
									// if (ignore_onend) 
									// {
										// return;
									// }  
								// };
								
								// recognition.onerror = function(event) 
								// {
									// //$("#" + btnID).val('Speak');
									// $(document.getElementById(btnID)).val('Speak');
									// console.log(event.error);
									// ignore_onend = true;
									// // if (event.error == 'no-speech') 
									// // {								
										// // //showInfo('info_no_speech');
										
									// // }
									// // if (event.error == 'audio-capture') 
									// // {
										// // $("#" + btnID).val('Speak');
										// // //showInfo('info_no_microphone');
										// // ignore_onend = true;
									// // }
									// // if (event.error == 'not-allowed') 
									// // {
										// // if (event.timeStamp - start_timestamp < 100) 
										// // {
											// // showInfo('info_blocked');
										// // } 
										// // else 
										// // {
											// // showInfo('info_denied');
										// // }
										// // ignore_onend = true;
									// // }
								// };
					   
								// final_transcript = '';
								// recognition.lang = 'en-IN';
								// recognition.start();  
								// ignore_onend = false;
							// }
						// }
						// catch(err)
						// {		
							// alert(err.stack);
						// } 
					// });
				// }
			
			// });
			
			
			
				
			$(".btn").each(function(index) 
			{
				// $(".btn").click(function ()
				// {
					
					if($(this).attr("id") != undefined)
					{
						var btnID = $(this).attr("id");												
						if(btnID.indexOf('NoteSection')>-1 && $(this).get(0).tagName == "INPUT" && $(this).attr("type").toLowerCase() === "button" && $(this).attr("SST") === undefined && $(this).attr("class") != "btn Merit_SST")
						{
							
							// console.log(btnID);
							// console.log($._data($(this)[0]).events);
							// var foo = $(this).data('events').click;
							// console.log(foo);				
							// if(foo.length <= 2)
							// {
								
								$(this).attr("SST","True");
								$(this).click(function ()
								{
									//console.log($._data(this, 'events').click);
									//console.log($(this).attr("class"));	
									setTimeout(function()
									{
										//console.log('1st');
										injectButton(100);
									},1000);
									
									setTimeout(function()
									{
										//console.log('2nd');
										injectButton(100);
									},2000);
									
									setTimeout(function()
									{
										//console.log('3rd');
										injectButton(100);
									},3000);
									
									setTimeout(function()
									{
										//console.log('4th');
										injectButton(100);
									},4000);
									
									setTimeout(function()
									{
										//console.log('5th');
										injectButton(100);
									},5000);
									
									setTimeout(function()
									{
										//console.log('6th');
										injectButton(100);
									},6000);									
								});
								
								
							// }
							// else
							// {
								// console.log('Event Already added')
							// }
							
						}	

					}					
				// });
			});
			
			
			
			
		},AjaxDelay);	
	}
	catch(err)
	{
		console.log(err.stack);
	}  
}

function rec(ID)
{	
	$(document.getElementById(ID)).click(function()
					{
						console.log("Speak Click:" + $._data(this, 'events').click);
						$(this).attr("SSTBTN","True");
						var txtAreaID = $(this).attr("id").replace('Merit_SST_','');
						var btnID = $(this).attr("id");
						//console.log("textarea#" + txtAreaID);
						var txtValue = $(document.getElementById(txtAreaID)).val();
						//var txtValue = $("textarea#" + txtAreaID).val();						
						console.log("Val:" + txtValue);
						try
						{	
							if (!('webkitSpeechRecognition' in window)) 
							{			
								alert('Chrome version not supported. Upgrade your Chrome.');
							}
							else 
							{			
								if (recognizing) 
								{
									recognition.stop();
									return;
								}			
								recognition = new webkitSpeechRecognition();
								recognition.continuous = true;
								recognition.interimResults = true;		
								recognition.onstart = function() 
								{
									recognizing = true;
									//showInfo('info_speak_now');
									//$("#" + btnID).val('Stop');
									$(document.getElementById(btnID)).val('Stop');
								};
								
								recognition.onresult = function(event) 
								{
									try
									{
										recognizing = true;
										var interim_transcript = '';
										for (var i = event.resultIndex; i < event.results.length; ++i) 
										{
											if (event.results[i].isFinal) 
											{
												final_transcript += event.results[i][0].transcript;
											} 
											else 
											{
												interim_transcript += event.results[i][0].transcript;
											}
										}    
										
										//console.log("#" + txtAreaID);
										
										if(txtValue.length > 0)
										{
											$(document.getElementById(txtAreaID)).val(txtValue + " " + interim_transcript);
											
											if(final_transcript.length > 0)
												$(document.getElementById(txtAreaID)).val(txtValue + " " + final_transcript);
										}
										else
										{
											$(document.getElementById(txtAreaID)).val(interim_transcript);											
											if(final_transcript.length > 0)
												$(document.getElementById(txtAreaID)).val(final_transcript);
										}
										
										//$("#" + txtAreaID).val(interim_transcript);
										
										
											//$("#" + txtAreaID).val(final_transcript);	
									}
									catch(err)
									{		
										console.log(err.stack);
									}
								};
					  
								recognition.onend = function() 
								{
									recognizing = false;
									//$("#" + btnID).val('Speak');
									$(document.getElementById(btnID)).val('Speak');
									if (ignore_onend) 
									{
										return;
									}  
								};
								
								recognition.onerror = function(event) 
								{
									//$("#" + btnID).val('Speak');
									$(document.getElementById(btnID)).val('Speak');
									console.log(event.error);
									ignore_onend = true;
									// if (event.error == 'no-speech') 
									// {								
										// //showInfo('info_no_speech');
										
									// }
									// if (event.error == 'audio-capture') 
									// {
										// $("#" + btnID).val('Speak');
										// //showInfo('info_no_microphone');
										// ignore_onend = true;
									// }
									// if (event.error == 'not-allowed') 
									// {
										// if (event.timeStamp - start_timestamp < 100) 
										// {
											// showInfo('info_blocked');
										// } 
										// else 
										// {
											// showInfo('info_denied');
										// }
										// ignore_onend = true;
									// }
								};
					   
								final_transcript = '';
								recognition.lang = 'en-IN';
								recognition.start();  
								ignore_onend = false;
							}
						}
						catch(err)
						{		
							console.log(err.stack);
						} 
					});
	
}



  
var final_transcript = '';
var recognition;





try
{

//
// Start JS injection
//

//if(document.URL.indexOf('stackoverflow.com/questions')>-1)
if(document.URL.indexOf('salesforce.com')>-1 || document.URL.indexOf('visual.force.com')>-1)
{

	 injectButton(2000);
	 injectButton(4000);
	injectButton(6000);
		
	// if(document.URL.indexOf('stackoverflow.com/questions')>-1)
	// {	
		// setInterval(function(){AddSearchProfiles_Button();}, 5000);
	// }
	
	//injectButton(6000);
}

}
catch(err)
{
//alert(err.stack);
console.log(err.stack);
}



