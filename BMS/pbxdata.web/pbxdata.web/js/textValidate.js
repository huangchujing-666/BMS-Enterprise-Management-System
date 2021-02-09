//限制文本框只能输入数字
function IsNumber(id, isAge) {
    if ($(id).val().length > 3 && isAge) {
        $(id).val($(id).val().substr(0, 3));
        return;
    }
    if ($(id).val() == "") return;
    //alert($(id).val().length);
    if ($(id).val().Legth > 1) {
        var str = $(id).val().replace("/\D/g", "");
        $(id).val(parseInt(str));
    }
    else {
        //alert($(id).val());
        //var gex = new RegExp("/\d");
        //var bool = gex.test(parseInt($(id).val()));
        ////alert(bool);
        var bool = parseInt($(id).val());
        //alert(bool);
        if (bool.toString() == "NaN" || bool == 0)
            $(id).val("");
        else

            $(id).val(parseInt(bool));
    }

}
//验证文本不为空
function IsNoNull(myThis, mess, messCtrId) {
    if ($(myThis).val() == "" || $(myThis).val() == null) {
        $("#" + messCtrId).html(mess);
        return false;
    }
    else {
        $("#" + messCtrId).empty();
        return true;
    }
}
function IsPhone(id) {
    if ($(id).val().length > 13) {

        $(id).val($(id).val().substr(0, 13));
        return;
    }
    //if ($(id).val() == "") return;
    //alert($(id).val().length);
    //if ($(id).val().Legth > 1) {
    //    var str = $(id).val().replace("/\D/g", "");
    //    $(id).val(str);
    //}
    //else {
    //    //alert($(id).val());
    //    //var gex = new RegExp("/\d");
    //    //var bool = gex.test(parseInt($(id).val()));
    //    ////alert(bool);
    //    var bool = parseInt($(id).val());
    //    alert(bool);
    //    if (bool != "NaN" && bool != 0)
    //        $(id).val(bool);
    //    else
    //        $(id).val("");
    //}
}
//删除空格
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
//判断输入内容是否为空    
function IsNull(id) {
    var tem = $("#" + id).val().trim();
   // alert(id);
    if (tem.length == 0) {
        $("#sp_" + id).html("文本框不能为空或者为空格！");
        //alert('对不起，文本框不能为空或者为空格!');//请将“文本框”改成你需要验证的属性名称!    
        return false;
    }
    else {
        $("#sp_" + id).html($("#sp_" + id).html().replace("文本框不能为空或者为空格！", ""));
        return true;
    }
}

//判断日期类型是否为YYYY-MM-DD格式的类型    
function IsDate(id) {
    var tem = $("#" + id).val().trim();
    if (str.length != 0) {
        var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/;
        var r = str.match(reg);
        if (r == null) {
            $("#sp_" + id).html("您输入的日期格式不正确！");
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的日期格式不正确！", ""));
            return true;
        }
    }
}

//判断日期类型是否为YYYY-MM-DD hh:mm:ss格式的类型    
function IsDateTime(id) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
        var r = tem.match(reg);
        if (r==null) {
            $("#sp_" + id).html("您输入的日期格式不正确！");
           
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的日期格式不正确！", ""));
            return true;
        }
           
    }
}

//判断日期类型是否为hh:mm:ss格式的类型    
function IsTime(id) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^((20|21|22|23|[0-1]\d)\:[0-5][0-9])(\:[0-5][0-9])?$/
        if (!reg.test(tem)) {
            $("#sp_" + id).html("您输入的日期格式不正确！");
            // alert("对不起，您输入的日期格式不正确!");//请将“日期”改成你需要验证的属性名称! 
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的日期格式不正确！", ""));
            return true;
        }
    }
}

//判断输入的字符是否为英文字母    
function IsLetter(id) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^[a-zA-Z]+$/;
        if (!reg.test(tem)) {
            $("#sp_" + id).html("您输入的英文字母类型格式不正确！");
            // alert("对不起，您输入的英文字母类型格式不正确!");//请将“英文字母类型”改成你需要验证的属性名称!   
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的英文字母类型格式不正确！", ""));
            return true;
        }
    }
}

//判断输入的字符是否为整数    
function IsInteger(id) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^[-+]?\d*$/;
        if (!reg.test(tem)) {
            $("#sp_" + id).html("您输入的整数类型格式不正确！");
            //alert("对不起，您输入的整数类型格式不正确!");//请将“整数类型”要换成你要验证的那个属性名称！    
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的整数类型格式不正确！", ""));
            return true;
        }
    }
}

//判断输入的字符是否为双精度    
function IsDouble(id) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^[-\+]?\d+(\.\d+)?$/;
        if (!reg.test(tem)) {
            $("#sp_" + id).html("您输入的双精度类型格式不正确！");
            //alert("对不起，您输入的双精度类型格式不正确!");//请将“双精度类型”要换成你要验证的那个属性名称！    
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的双精度类型格式不正确！", ""));
            return true;
        }
    }
}


//判断输入的字符是否为:a-z,A-Z,0-9    
function IsString(id) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^[a-zA-Z0-9_]+$/;
        if (!reg.test(tem)) {
            $("#sp_" + id).html("您输入的中文格式不正确！请输入a-z,A-Z,0-9,_字符类型！");
            // alert("对不起，您输入的字符串类型格式不正确!");//请将“字符串类型”要换成你要验证的那个属性名称！    
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的中文格式不正确！请输入a-z,A-Z,0-9,_字符类型！", ""));
            return true;
        }
    }
}

//判断输入的字符是否为中文    
function IsChinese(id) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^[\u0391-\uFFE5]+$/;
        if (!reg.test(tem)) {
            $("#sp_" + id).html("您输入的中文格式不正确！");
            //alert("对不起，您输入的字符串类型格式不正确!");//请将“字符串类型”要换成你要验证的那个属性名称！    
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的中文格式不正确！", ""));
            return true;
        }
    }
}

//判断输入的EMAIL格式是否正确    
function IsEmail(id) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
        if (!reg.test(tem)) {
            $("#sp_" + id).html("您输入的邮箱格式不正确！");
            //alert("对不起，您输入的字符串类型格式不正确!");//请将“字符串类型”要换成你要验证的那个属性名称！ 
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的邮箱格式不正确！", ""));
            return true;
        }
    }
}

//判断输入的邮编(只能为六位)是否正确    
function IsZIP(id) {
    var tem = $("#" + id).val().trim();
    if (str.length != 0) {
        var reg = /^\d{6}$/;
        if (!reg.test(tem)) {
            $("#sp_" + id).html("您输入的邮编格式不正确！");
            // alert("对不起，您输入的字符串类型格式不正确!");//请将“字符串类型”要换成你要验证的那个属性名称！ 
            return false;
        }
        else {
            $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的邮编格式不正确！", ""));
            return true;
        }
    }
}

//判断输入的数字不大于某个特定的数字    
function MaxValue(id,MaxInt) {
    var tem = $("#" + id).val().trim();
    if (tem.length != 0) {
        var reg = /^[-+]?\d*$/;
        if (!reg.test(tem)) {//判断是否为数字类型    
            if (val > parseInt(MaxInt)) //“123”为自己设定的最大值    
            {
                $("span #sp_" + id).html("您输入的数字超出范围！");
                //alert('对不起，您输入的数字超出范围');//请将“数字”改成你要验证的那个属性名称！    
                return false;
            }
            else {
                $("#sp_" + id).html($("#sp_" + id).html().replace("您输入的数字超出范围！", ""));
                return true;
            }
        }
        else {
            return false;
        }
    }
}
//验证是否为固定电话或手机
function IsPhoneOrMobile(id) {
    var bol = IsPhone1(id);
   // alert(bol.toString());
    if (!bol) {
        bol = IsMobile(id);
        if (bol) {
            $("#sp_" + id).html($("#sp_" + id).html().replace("电话号码格式错误！", ""));
        }
        return bol;
    }
    return bol;
}

//验证电话号码
function IsPhone1(id) {
    var tem = $("#" + id).val().trim();
    var reg = /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/;
    if (!reg.test(tem)) {
        $("#sp_" + id).html("电话号码格式错误！");
        return false;
    }
    else {
        $("#sp_" + id).html($("#sp_" + id).html().replace("电话号码格式错误！", ""));
        return true;
    }
}
//验证手机号
function IsMobile(id) {
    var tem = $("#" + id).val().trim();
    var reg = /^((\(\d{2,3}\))|(\d{3}\-))?1\d{1}\d{9}$/;
    if (!reg.test(tem)) {
        $("#sp_" + id).html("手机号码格式错误！");
        return false;
    }
    else {
        $("#sp_" + id).html($("#sp_" + id).html().replace("手机号码格式错误！", ""));
        return true;
    }
}
function IsIp(id) {
    var tem = $("#" + id).val().trim();
    var reg = /^(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])\.(\d{1,2}|1\d\d|2[0-4]\d|25[0-5])(,[0-9]{0,5})?$/;
    
    if (!reg.test(tem)) {
        $("#sp_" + id).html("ip地址格式错误！");
        return false;
    }
    else {
        $("#sp_" + id).html($("#sp_" + id).html().replace("ip地址格式错误！", ""));
        return true;
    }
}
//Phone : /^((\(\d{2,3}\))|(\d{3}\-))?(\(0\d{2,3}\)|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/    
//   Mobile : /^((\(\d{2,3}\))|(\d{3}\-))?13\d{9}$/    