javascript:var server="localhost:60024";var maxreqlength=1500;
var selectedText=_getSelectedText();
if(selectedText)
{
	_bufferText(escape(selectedText));
	_speakText
	void 0;
	function _getSelectedText()
	{
		if(window.getSelection)
		{
			return window.getSelection().toString()
		}
		else
		{
			if(document.getSelection)
			{
				return document.getSelection()
			}
			else
			{
			if(document.selection)
			{
				return document.selection.createRange().text
			}
		}
	}
	return null
}
function _formatCommand(b,a)
{
	return"http://"+server+"/"+b+"/dummy.gif"+a+"&timestamp="+new Date().getTime()
}
function _speakText()
{
	var a=new Image(1,1);
	a.onerror=function()
	{
        a.onerror.
		_showerror()
	};
	a.src=_formatCommand("speaktext","?source="+document.URL)
	
}

function _bufferText(f)
{
	var c="true";
	var b=Math.floor((f.length+maxreqlength-1)/maxreqlength);
	for(var d=0;d<b;d++)
	{
		var g=d*maxreqlength;
		var a=Math.min(f.length,g+maxreqlength);
		var e=new Image(1,1);
		e.onerror=function()
		{
			_showerror()
		};
		e.src=_formatCommand("buffertext","?totalreqs="+b+"&req="+(d+1)+"&text="+f.substring(g,a)+"&clear="+c);
		c="false"
	}
}
function _showerror()
{
	alert("BrowserSpeak is not running. You must start BrowserSpeak first."
)