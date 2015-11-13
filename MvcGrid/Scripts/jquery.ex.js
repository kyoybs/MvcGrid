$(function () {
    $.ajaxSetup({cache:false})
})

$(document).ajaxError(function (event, xhr, options, exc) {
    jq.showMsg(xhr.responseText);
});

var jq = {};

jq.get = function(selector){
    var $ctl = $(selector);
    if($ctl.size() == 0)
    {
        alert("Can't find selector: " + selector); 
    }
    return $ctl;
}

jq.log = function(msg, len){
    console.log(jq.left(msg,len , true));
}

jq.left = function(str, len, autoEllipsis){
    if(str == null || str == "" || str.length <= len)
        return str;
    return str.substring(0,len) + (autoEllipsis?"...":"");
}

jq.showMsg = function (msg) {
    $(".mask").click(function () {
        $(".mask").hide();
        $(".popmsg").hide();
    });
    $(".popmsg").html(msg);
    $(".mask").show();
    $(".popmsg").show();
}


jq.updateGridField = function (url, $td) {
    var fieldName = $td.attr("data-field");
    var id = $td.closest("tr").attr("data-id");
    var val = $.trim($td.html().replace(/<br>/, '\r\n'));
    jq.log("------UPDATE FIELD -------------");
    jq.log(url);
    jq.log(fieldName);
    jq.log(id);
    jq.log(val);
    jq.log("------UPDATE FIELD END-------------");
    $.post(url, { fieldName: fieldName, fieldValue:val, dataId: id }, function (data) {
        if (data != "") {
            alert(data); 
        } 
    });
}