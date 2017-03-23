$(function () {
    $("#LoginSys").click(function () {
        LoginSys();
    });
    $("#UserName").keydown(function (e) {
        var curkey = e.which;
        if (curkey == 13) {
            LoginSys();
            return false;
        }
    });
    $("#Password").keydown(function (e) {
        var curkey = e.which;
        if (curkey == 13) {
            LoginSys();
            return false;
        }
    });
    $("#ValidateCode").keydown(function (e) {
        var curkey = e.which;
        if (curkey == 13) {
            LoginSys();
            return false;
        }
    });

    $(window).resize(function () {
        if ($(window).height() < 500) {
            $(".top_div").height(150);
        } else {
            $(".top_div").height(300);
        }
    });

});

function LoginSys() {
    $("#mes").html("");
    $("#UserName").removeClass("input-validation-error");
    $("#Password").removeClass("input-validation-error");
    $("#ValidateCode").removeClass("input-validation-error");
    if ($.trim($("#UserName").val()) == "") {
        $("#UserName").addClass("input-validation-error").focus();
        $("#mes").html("用户名不能为空！");
        return;
    }
    if ($.trim($("#Password").val()) == "") {
        $("#Password").addClass("input-validation-error").focus();
        $("#mes").html("密码不能为空！");
        return;
    }
    if ($.trim($("#ValidateCode").val()) == "") {
        $("#ValidateCode").addClass("input-validation-error").focus();
        $("#mes").html("验证码不能为空！");
        return;
    }
    $("#Loading").show();

    $.post('/Account/Login', { UserName: $("#UserName").val(), Password: $("#Password").val(), Code: $("#ValidateCode").val() },
    function (data) {

        if (data.type == "1") {
            var url = getQueryString("url");
            if (url != null)
            {
                window.location = url;
            }
            else
            {
                window.location = "/"+$("#local").val()+"/Home/Index"
            }
        } else {
            $("#mes").html(data.message);
        }
        $("#Loading").hide();
    }, "json");
    return false;
}
function ChangeDb() {
    $.post('Account/ChangeDb', { name: $("#dbname").val() });
}