/*************************************************************************************
    * 文 件 名：       文本下拉提示
    * 创建时间：       2015-10-23 10:16:55
    * 作    者：       hjb
    * 说    明：
    * 修改时间：       2015-10-23
    * 修 改 人：       hjb
*************************************************************************************/

(function ($) {
    var getDropDownList = function getDropDownList() { };
    $.getDropDownList =
    function (e) {
        
        var obj = $(e);
        obj.click(function () {     
            $("#dd").remove();
            $("body").append("<div id='dd'><ul id='menu'></ul></div>");
            $("#dd").css(
                {
                    "position": "absolute",
                    "background-color": "white",
                    "width": obj.width()+10,
                    "float": "left",
                    "top": obj.offset().top + 22,
                    "left": obj.offset().left,
                }
                );
            $("#dd ul").css(
                {

                    "list-style-type":"none",
                    "margin-left": "0",
                    "line-height": "20px"
                }
                );
         
            if (obj.val() != "") {

                $("#dd").css(
                {
                    "overflow-y": "hidden",
                }
                );
                $("#dd").show();
            } else {
                $("#dd").css(
                 {

                     "overflow-y": "scroll",
                     "height": "295px",
                     "border": "1px solid"
                 }
             );
                $("#dd").show();

            }
            obj.blur(function ()
            {
                $("#dd").hide();
            }
            )
            //obj.live("blur", function (event) {
            //    if (obj.val() != "") {
            //        $("#dd").hide(1000);
            //    }
            //    //$("#dd").hide(1000)
                
            //}

                //)    
        });

        
        

    }
})(jQuery);

//在Ajax中append li 标签后 getCss() 获取li 的样式
function getCss(e) {
    $("#menu li").mouseover(function () {
        $(this).css(
            {
                "background-color": "skyblue"
            }
            )
    }
       );
    $("#menu li").mouseout(function () {
        $(this).css(
           {
               "background-color": "white"
           }
           )
    }
    );
    $("#menu li").click(function ()
    {
        var v = $(this).attr("title");
        $("#" + e).val(v);
        $("#dd").hide();
      //  阻止事件冒泡
    })
    $("#dd ul li").css(
             {
                 "margin-left": "-35px",
                 "line-height": "20px"
             }
             );

    $("#dd ul li").mouseover(function () {
        $("#" + e).unbind("blur");
        $(this).css(
            {
                "background-color": "skyblue",
            
            }
            );
    }
    );

    $("#dd ul li").mouseout(function () {
        $("#" + e).bind("blur", function () { $("#dd").hide() });
    });


    $("#dd").mouseout(function ()
    {

    }
    )
    $("#dd ul li").mouseout(function () {
      
        $(this).css({
            "background-color": "white"
        });
    }
    );
}

