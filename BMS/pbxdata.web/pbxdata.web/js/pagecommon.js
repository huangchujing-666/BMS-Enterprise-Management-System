

/*************************************************************************************
    * 文 件 名：       页面辅助类
    * 创建时间：       2015-10-21 10:16:55
    * 作    者：       lcg
    * 说    明：
    * 修改时间：       2015-10-21
    * 修 改 人：       lcg
*************************************************************************************/

(function ($) {

    //全选
    var selectAllFunc = function selectCheckAll() { }
    var count = 0;
    $.selectAllFunc = function (obj) {
        var c = $(obj);
        c.each(function (i, n) {
            if (count % 2 == 0) {
                $(this).attr("checked", "checked");
            } else {
                $(this).removeAttr("checked");
            }
        })
        count++;
    };







})(jQuery);



































































