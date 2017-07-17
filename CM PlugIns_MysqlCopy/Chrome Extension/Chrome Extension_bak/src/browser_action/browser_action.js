

//alert(localStorage['CM_Fields']);

try {
//    alert(document.getElementById("1").value);

    document.getElementById("CM_ProjectName").innerHTML = localStorage['CM_ProjectName'];
    //document.getElementById("CM_CompanyID").innerHTML = localStorage['CM_CompanyID'];
    document.getElementById("CM_CompanyName").innerHTML = localStorage['CM_CompanyName'];
    document.getElementById("CM_Agent").innerHTML = localStorage['CM_Agent'];

    //alert(document.getElementById("23").innerHTML);

}
catch (err) {
alert(err.stack);
}
//alert(localStorage['CM_CompanyID']);
//alert(localStorage['CM_CompanyName']);
//alert(localStorage['CM_Agent']);

