$(function () {
    $.ajaxSetup({
        cache: false,
        beforeSend: function () {
            alert("Ajax Start!");
        }
    })
})

$(document).ajaxError(function (event, xhr, options, exc) {
    jq.showMsg(xhr.responseText);
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

jq.showMsg = function (msg) {
    $(".mask").click(function () {
        $(".mask").hide();
        $(".popmsg").hide();
    });
    $(".popmsg").html(msg);
    $(".mask").show();
    $(".popmsg").show();
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

jq.popWindow = function (title, url, id) {
    jq.log("------SHOW ROW DETAIL  -------------");
    jq.log(url);
    jq.log(id);
    jq.log("------SHOW ROW DETAIL END-------------");
    $.post(url, { dataId: id }, function (data) {
        var windowIndex = $("_window_mask").size();
        var maskId = '_window_mask_' + windowIndex;
        var windowId = '_window_' + windowIndex;
        var mask = $("<div id='" + maskId + "' class='mask'></div>");
        $("body").append(mask);
        var pop = $("<div id='" + windowId + "'  class='window'></div>");
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