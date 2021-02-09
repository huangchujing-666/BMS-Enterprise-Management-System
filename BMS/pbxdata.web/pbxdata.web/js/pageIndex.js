

/*************************************************************************************
    * 文 件 名：       分页
    * 创建时间：       2015-04-14 10:16:55
    * 作    者：       lcg
    * 说    明：
    * 修改时间：       2015-04-14
    * 修 改 人：       lcg
*************************************************************************************/


(function ($) {
    var PageFunc = function PageFunc() { }
    $.PageFunc = function (url, menuId, params) {
        var pageHtml = "";
        pageHtml += "<span style='font-size: 12px; color: #808080;'>";
        pageHtml += "<select id='selPages' style='width:60px;' onchange=\"selPage('" + url + "'," + menuId + "," + params + ")\">";
        pageHtml += "<option>5</option>";
        pageHtml += "<option selected='selected'>10</option>";
        pageHtml += "<option>20</option>";
        pageHtml += "<option>50</option>";
        pageHtml += "<option>100</option>";
        pageHtml += "</select>";
        pageHtml += "</span>";
        pageHtml += "<a href='#' onclick=\"TurnPage('" + url + "',1," + menuId + "," + params + ")\">首页</a>";
        pageHtml += "<a href='#' onclick=\"TurnPage('" + url + "',2," + menuId + "," + params + ")\">上一页</a>";
        pageHtml += "<input type='text' id='txtTiao' name='txtTiao' value='1' style='width: 35px;'/>";
        pageHtml += "<a href='#' onclick=\"TurnPage('" + url + "',3," + menuId + "," + params + ")\">下一页</a>";
        pageHtml += "<a href='#' onclick=\"TurnPage('" + url + "',4," + menuId + "," + params + ")\">尾页</a>";
        pageHtml += "<a href='#' onclick=\"TurnPage('" + url + "',5," + menuId + "," + params + ")\">跳转</a>";
        return pageHtml;
    };
})(jQuery);


function TurnPage(url, obj, menuId, params) {//-----翻页---主货号表下表
    if ($("#txtTiao").val() == "") {
        return false;
    }
    var pages = $("#selPages").val();
    switch (obj) {
        case 1:
            pageDataIndex(url, menuId, 1, pages,params);//----首页
            $("#txtTiao").val(1);
            $("#PageNum").html(1)
            break;
        case 2:;//----上一页
            if ($("#txtTiao").val() == "1") {
                return false;
            }
            else {
                pageDataIndex(url, menuId, $("#txtTiao").val() * 1 - 1, pages, params)
                $("#txtTiao").val($("#txtTiao").val() * 1 - 1);
                $("#PageNum").html($("#txtTiao").val())
            }
            break;
        case 3://----下一页
            if ($("#txtTiao").val() == $("#PageCount").html()) {
                return false;
            }
            else {
                pageDataIndex(url, menuId, $("#txtTiao").val() * 1 + 1, pages, params);
                $("#txtTiao").val($("#txtTiao").val() * 1 + 1);
                $("#PageNum").html($("#txtTiao").val())
            }

            break;
        case 4: pageDataIndex(url, menuId, $("#PageCount").html(), pages, params);//----尾页

            $("#txtTiao").val($("#PageCount").html());
            $("#PageNum").html($("#PageCount").html())
            break;
        case 5: pageDataIndex(url, menuId, $("#txtTiao").val(), pages, params);//----跳转
            $("#PageNum").html($("#txtTiao").val());
            break;

    }
}

function pageDataIndex(url, menuId, pageIndex, pageSize, params1) {
    var params = JSON.stringify(params1).replace(/\"/g, "'");
    $.post(url, { menuId: menuId, pageIndex: pageIndex, pageSize: pageSize, params: params }, function (data) {
        var ss = data.split("-----"); $(".mytable").html(""); $(".mytable").append(ss[0]); $("#PageCount").html(ss[1]); $("#AllCounts").html(ss[2]);
    })
}

function pageDataIndex1(url, menuId, pageIndex, pageSize) {
    $.post(url, { menuId: menuId, pageIndex: pageIndex, pageSize: pageSize }, function (data) { var ss = data.split("-----"); $(".mytable").html(""); $(".mytable").append(ss[0]); $("#PageCount").html(ss[1]); $("#AllCounts").html(ss[2]); })
}






