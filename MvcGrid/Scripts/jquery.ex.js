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
