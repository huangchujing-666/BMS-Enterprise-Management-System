


/*************************************************************************************
    * 文 件 名：       图片放大
    * 创建时间：       2015-04-14 10:16:55
    * 作    者：       lcg
    * 说    明：
    * 修改时间：       2015-04-14
    * 修 改 人：       lcg
*************************************************************************************/


function show(url, id) {
    $("#productImg").attr("src", url);
    $("#productImg").show();
    var objDiv = $("#" + id + "");
    $(objDiv).css("display", "block");
    $(objDiv).css("left", event.clientX + 10 + $(document).scrollLeft());
    console.log(event.clientY)
    $(objDiv).css("top", event.clientY - ($("#productImg").height()/2)-20 + $(document).scrollTop());
}

function hide(id) {
    var objDiv = $("#" + id + "");
    $(objDiv).css("display", "none");
}