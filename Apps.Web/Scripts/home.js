
$(function () {

    $('#tab_menu-tabrefresh').click(function () {
        /*重新设置该标签 */
        var url = $(".tabs-panels .panel").eq($('.tabs-selected').index()).find("iframe").attr("src");
        $(".tabs-panels .panel").eq($('.tabs-selected').index()).find("iframe").attr("src", url);
    });
    //在新窗口打开该标签
    $('#tab_menu-openFrame').click(function () {
        var url = $(".tabs-panels .panel").eq($('.tabs-selected').index()).find("iframe").attr("src");
        window.open(url);
    });
    //关闭当前
    $('#tab_menu-tabclose').click(function () {
        var currtab_title = $('.tabs-selected .tabs-inner span').text();
        $('#mainTab').tabs('close', currtab_title);
        if ($(".tabs li").length == 0) {
            //open menu
            $(".layout-button-right").trigger("click");
        }
        initTabs();
    });
    //全部关闭
    $('#tab_menu-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            if ($(this).parent().next().is('.tabs-close')) {
                var t = $(n).text();
                $('#mainTab').tabs('close', t);
            }
        });
        initTabs();
        //open menu
        $(".layout-button-right").trigger("click");
    });
    //关闭除当前之外的TAB
    $('#tab_menu-tabcloseother').click(function () {
        var currtab_title = $('.tabs-selected .tabs-inner span').text();
        $('.tabs-inner span').each(function (i, n) {
            if ($(this).parent().next().is('.tabs-close')) {
                var t = $(n).text();
                if (t != currtab_title)
                    $('#mainTab').tabs('close', t);
            }
        });
        initTabs();
    });
    //关闭当前右侧的TAB
    $('#tab_menu-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            $.messager.alert(index_lang_tip, index_NoTabsOnTheLeft, 'warning');
            return false;
        }
        nextall.each(function (i, n) {
            if ($('a.tabs-close', $(n)).length > 0) {
                var t = $('a:eq(0) span', $(n)).text();
                $('#mainTab').tabs('close', t);
            }
        });
        initTabs();
        return false;
    });
    //关闭当前左侧的TAB
    $('#tab_menu-tabcloseleft').click(function () {

        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            $.messager.alert(index_lang_tip, index_NoTabsOnTheRight, 'warning');
            return false;
        }
        prevall.each(function (i, n) {
            if ($('a.tabs-close', $(n)).length > 0) {
                var t = $('a:eq(0) span', $(n)).text();
                $('#mainTab').tabs('close', t);
            }
        });
        initTabs();
        return false;
    });
    /*为选项卡绑定右键*/
    $("#mainTab").tabs({
        onSelect: function (title, index) {
            initTabs();
        },
        onContextMenu: function (e) {
            /* 选中当前触发事件的选项卡 */
            var subtitle = $(this).text();
            $('#mainTab').tabs('select', subtitle);
            //显示快捷菜单
            e.preventDefault();
            //阻止冒泡
            $('#tab_menu').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
            return false;
        }
    })
    //加载第一个tabs
    addTab(index_lang_desktop, "/Home/Desktop", "fa fa-home");
    $("#mainTab .tabs ").attr("style", "height:34px;line-height:34px");
    $("#mainTab .tabs li").find("a:first").attr("style", "height:32px;line-height:32px");

    $('#showUserInfo').tooltip({
        content: $('<div></div>'),
        showEvent: 'click',
        deltaX: -70,
        onUpdate: function (content) {
            content.panel({
                width: 250,
                border: false,
                
                href: '/Home/TopInfo'
            });
        },
        onShow: function () {
            var t = $(this);
            t.tooltip('tip').unbind().bind('mouseenter', function () {
                t.tooltip('show');
            }).bind('mouseleave', function () {
                t.tooltip('hide');
            });
        }
    });
});


function initTabs() {
    $("#mainTab .tabs ").attr("style", "height:33px;line-height:33px");
    $("#mainTab .tabs li").find("a:first").attr("style", "height:32px;line-height:32px");
}

function Profile()
{
    addTab(index_lang_info, "../../" + _globalConfig.CurrentCulture + "/Home/Info", "fa fa-credit-card");
}

function SignOut()
{
    $.messager.confirm(index_lang_tip, index_YouWantToExitTheSystem, function (r) {
      if (r) {
          $.post("/Account/LogOut", function (data) {
              
          }, "json");
          window.location.href = '/Account/Index';
      }
   });
}

$(function () {
    
    //tabs页码bug
    $('#easyLayout').layout('panel', 'west').panel({
        onResize: function () {
            setTimeout(function () {
                initTabs()
            }, 100);
        }
    });
});
//tabs页码bug
$(window).resize(function () {
    setTimeout(function(){
        initTabs()
    }, 100);
});

function addTab(subtitle, url, icon) {
    if (!$("#mainTab").tabs('exists', subtitle)) {
        var closableFlag = true;
        if (url.indexOf("/Home/Desktop") > -1)
        {
            closableFlag = false;
        }


        $("#mainTab").tabs('add', {
            title: subtitle,
            content: '<iframe frameborder="0" src="' + url + '" scrolling="auto" style="width:100%; height:100%;overflow:hidden"></iframe>',
            closable: closableFlag,
            icon: icon
        });
    } else {
        $("#mainTab").tabs('select', subtitle);
        $("#tab_menu-tabrefresh").trigger("click");
    }
    //$(".layout-button-left").trigger("click");
    //tabClose();
}



function SetThemes() {
    $.messager.confirm(index_lang_tip, index_ChangeThemeWillReloadTheSystem, function (r) {
        if (r) {
            var theme = $('input[name="themes"]:checked').val();
            var menu = $('input[name="menustyle"]:checked').val();
            $.post("/Home/SetThemes", { theme: theme, menu: menu }, function (data) { window.location.reload(); }, "json");
        }
    });
}

$(function () {
    $("#SetThemes").click(function () {
        $("#ModalStyle").dialog({
            title: '个性化设置', 

        }).dialog('open');


    });
    $("#easyMod").click(function () {
        $('#easyLayout').layout('remove', 'north');
        $('#easyLayout').layout('remove', 'south');
    });
});

function fullSetButtonOut()
{
    if ($("#north").is(":hidden")) {
        return "<div class='fullSet'></div><div id='fullSetButton' class='fa fa-compress'></div>";
    } else {
        return "<div class='fullSet'></div><div id='fullSetButton' class='fa fa-expand'></div>";
    }
}
function fullSet()
{   
    $("#north").slideToggle("100", function () { $("#west").resize();});
}
