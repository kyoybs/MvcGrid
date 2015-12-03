window.onerror = function (data, url, line) {
    alert(data + "\r\n Error File: " + url + " -- Line: " + line);
}


 
$(function () {
    $.ajaxSetup({
        cache: false,
        beforeSend: function () {
            jq.loading();
        },
        complete: function () {
            jq.closeLoading();
        }
    })

    Vue.config.debug = true
})

$(document).ajaxError(function (event, xhr, options, exc) {
    jq.popMsg(xhr.responseText); 
});
 
var jq = {};

jq.get = function (selector) {
    var $ctl = $(selector);
    if ($ctl.size() == 0) {
        alert("Can't find selector: " + selector);
    }
    return $ctl;
}

jq.log = function (msg, len) {
    console.log(jq.left(msg, len, true));
}

jq.left = function (str, len, autoEllipsis) {
    if (str == null || str == "" || str.length <= len)
        return str;
    return str.toString().substring(0, len) + (autoEllipsis ? "..." : "");
}

jq.popMsg = function (msg) {
    var $mask = $("#msgmask");
    var $popmsg = $(".popmsg");
     
    if ($mask.size() == 0)
    {
        $mask = $('<div id="msgmask" class="mask" style="z-index:98;"><div class="title"> Click Here to Close this Message</div> </div>');
        $popmsg = $(' <div class="popmsg"></div>');
        $("body").append($mask).append($popmsg);

        $mask.find(".title").click(function () {
            $mask.hide();
            $popmsg.hide();
        });
    }
         
    $popmsg.html(msg);
    $mask.show();
    $popmsg.show();
}


jq.updateField = function (url, dataId, fieldName, fieldValue) {
    jq.log("------UPDATE FIELD -------------");
    jq.log(url);
    jq.log(fieldName);
    jq.log(dataId);
    jq.log(fieldValue);
    jq.log("------UPDATE FIELD END-------------");
    $.post(url, { fieldName: fieldName, fieldValue: val, dataId: id }, function (data) {
        if (data != "") {
            alert(data);
        }
    });
}

jq.popWindow = function (title, url, postData, callback) { 
    $.post(url, postData, function (data) {
        var windowIndex = $("_window_mask").size();
        var maskId = '_window_mask_' + windowIndex;
        var windowId = '_window_' + windowIndex;
        var mask = $("<div id='" + maskId + "' class='mask'></div>");
        $("body").append(mask);
        var pop = $("<div id='" + windowId + "'  class='window js-window'></div>");
        $("body").append(pop);
        var poptitle = $("<div id='_grid_title'  class='title'></div>");
        var popclose = $("<div class='close'>&Chi;</div>");
        popclose.click(function () {
            mask.remove();
            pop.remove();
        });
        pop.append(poptitle);
        poptitle.html(title);
        poptitle.append(popclose);

        var popbody = $("<div class='body'></div>");
        pop.append(popbody);

        pop[0]._popCallback = callback;

        popbody.html(data); 
        mask.show();
        pop.show();
    });
}

jq.loading = function () {
    if (typeof (jq.$loading) == "undefined") {
        jq.$loading = $("<div class='loading'>Loading...</div>");
        $("body").append(jq.$loading);
    }
    jq.$loading.show();
}


jq.closeLoading = function () {
    if (typeof (jq.$loading) != "undefined") {
        jq.$loading.hide();
    }
}

jq.showMsg = function (msg) { 
    if (typeof (jq.$msg) == "undefined") {
        jq.$msg = $("<table class='message'><tr><td>" + msg + "</td></tr></table>");
        $("body").click(function () {
            jq.$msg.hide();
        }).append(jq.$msg);
    }
    jq.$msg.find("td").html(msg);
    window.setTimeout(function () {
        jq.$msg.show();
    }, 50); 
}

$.fn.showError = function (errorMsg) {
    var $valueSpan = $(this);
    window.setTimeout(function () { 
        var $input = $valueSpan.find("input").addClass("error").change(function () {
            $(this).removeClass("error");
        }).focus();
        var $errorMsg = $valueSpan.find(".error-message");
        if ($errorMsg.size() == 0) {
            $errorMsg = $("<div class='error-message'></div>");
            $valueSpan.append($errorMsg);
        }
        $errorMsg.html(errorMsg);
        $errorMsg.show();
    }, 50); 
}

$.fn.hideError = function () {
    var $valueSpan = $(this);
    $valueSpan.find("input").removeClass("error");
    var $errorMsg = $valueSpan.find(".error-message");
    if($errorMsg.size()> 0)
    { 
        $errorMsg.hide();
    }
}

$.fn.getVueEl = function (modelName) {
    return $(this).find("[vue-model='" + modelName + "']"); 
}

$(function () {
    $("body").click(function () {
        $(".error-message").hide();
    }); 
});

jq.removeItem = function (array, item) {
    $.each(array, function (index, iobj) {
        if (iobj == item) {
            array.splice(index, 1);
            return;
        }
    });
}