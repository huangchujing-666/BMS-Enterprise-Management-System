
/*************************************************************************************
    * 文 件 名：       覆盖页面层
    * 创建时间：       2015-11-30 10:16:55
    * 作    者：       lcg
    * 说    明：
    * 修改时间：       2015-11-30
    * 修 改 人：       lcg
*************************************************************************************/


(function ($) {
    var coverPageDivShow = function coverPageDivShow() { }
    var coverPageDivHidden = function coverPageDivHidden() { }

    $.coverPageDivShow = function () {
        var demo = "<div id='popDiv' class='mydiv' style='display: none; width: 400px; height: 300px; left: 45%; top: 40%;'>请稍等，正在加载... ...</div><div id='bg' class='bg' style='display: none;'></div>";
        document.getElementById("cover").innerHTML =demo;

        document.getElementById('popDiv').style.display = 'block';
        document.getElementById('bg').style.display = 'block';
    };
    $.coverPageDivHidden = function CustomHiddenDiv(obj) {
        document.getElementById('popDiv').style.display = 'none';
        document.getElementById('bg').style.display = 'none';
        if (obj=="1") {
            alert("下载成功");         //1为下载成功，2为下载失败，其他为查询的遮盖层
        } else if(obj=="2") {
            alert("下载失败");         //1为下载成功，2为下载失败，其他为查询的遮盖层
        } else {
            //1为下载成功，2为下载失败，其他为查询的遮盖层
        }
    }
})(jQuery)































