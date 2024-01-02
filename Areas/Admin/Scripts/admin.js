function countChecked() {
    "all" === checkState && $(".bulk_action input[name='table_records']").iCheck("check"),
    "none" === checkState && $(".bulk_action input[name='table_records']").iCheck("uncheck");
    var e = $(".bulk_action input[name='table_records']:checked").length;
    e ? ($(".column-title").hide(),
    $(".bulk-actions").show(),
    $(".action-cnt").html(e + " Records Selected")) : ($(".column-title").show(),
    $(".bulk-actions").hide())
}
var CURRENT_URL = window.location.href.split("?")[0]
  , $BODY = $("body")
  , $MENU_TOGGLE = $("#menu_toggle")
  , $SIDEBAR_MENU = $("#sidebar-menu")
  , $SIDEBAR_FOOTER = $(".sidebar-footer")
  , $LEFT_COL = $(".left_col")
  , $RIGHT_COL = $(".right_col")
  , $NAV_MENU = $(".nav_menu")
  , $FOOTER = $("footer");
$(document).ready(function () {
    var e = function () {
        $RIGHT_COL.css("min-height", $(window).height());
        var e = $BODY.outerHeight()
          , t = $BODY.hasClass("footer_fixed") ? 0 : $FOOTER.height()
          , n = $LEFT_COL.eq(1).height() + $SIDEBAR_FOOTER.height()
          , i = n > e ? n : e;
        i -= $NAV_MENU.height() + t,
        $RIGHT_COL.css("min-height", i)
    }
    ;
    $SIDEBAR_MENU.find("a").on("click", function (t) {
        var n = $(this).parent();
        n.is(".active") ? (n.removeClass("active active-sm"),
        $("ul:first", n).slideUp(function () {
            e()
        })) : (n.parent().is(".child_menu") || ($SIDEBAR_MENU.find("li").removeClass("active active-sm"),
        $SIDEBAR_MENU.find("li ul").slideUp()),
        n.addClass("active"),
        $("ul:first", n).slideDown(function () {
            e()
        }))
    }),
    $MENU_TOGGLE.on("click", function () {
        $BODY.hasClass("nav-md") ? ($SIDEBAR_MENU.find("li.active ul").hide(),
        $SIDEBAR_MENU.find("li.active").addClass("active-sm").removeClass("active")) : ($SIDEBAR_MENU.find("li.active-sm ul").show(),
        $SIDEBAR_MENU.find("li.active-sm").addClass("active").removeClass("active-sm")),
        $BODY.toggleClass("nav-md nav-sm"),
        e()
    }),
    $SIDEBAR_MENU.find('a[href="' + CURRENT_URL + '"]').parent("li").addClass("current-page"),
    $SIDEBAR_MENU.find("a").filter(function () {
        return this.href == CURRENT_URL
    }).parent("li").addClass("current-page").parents("ul").slideDown(function () {
        e()
    }).parent().addClass("active"),
    $(window).smartresize(function () {
        e()
    }),
    e(),
    $.fn.mCustomScrollbar && $(".menu_fixed").mCustomScrollbar({
        autoHideScrollbar: !0,
        theme: "minimal",
        mouseWheel: {
            preventDefault: !0
        }
    })
}),
$(document).ready(function () {
    $(".collapse-link").on("click", function () {
        var e = $(this).closest(".x_panel")
          , t = $(this).find("i")
          , n = e.find(".x_content");
        e.attr("style") ? n.slideToggle(200, function () {
            e.removeAttr("style")
        }) : (n.slideToggle(200),
        e.css("height", "auto")),
        t.toggleClass("fa-chevron-up fa-chevron-down")
    }),
    $(".close-link").click(function () {
        var e = $(this).closest(".x_panel");
        e.remove()
    })
}),
$(document).ready(function () {
    $('[data-toggle="tooltip"]').tooltip({
        container: "body"
    })
}),
$(".progress .progress-bar")[0] && $(".progress .progress-bar").progressbar(),
$(document).ready(function () {
    if ($(".js-switch")[0]) {
        var e = Array.prototype.slice.call(document.querySelectorAll(".js-switch"));
        e.forEach(function (e) {
            new Switchery(e, {
                color: "#26B99A"
            })
        })
    }
}),
$(document).ready(function () {
    $("input.flat")[0] && $(document).ready(function () {
        $("input.flat").iCheck({
            checkboxClass: "icheckbox_flat-green",
            radioClass: "iradio_flat-green"
        })
    })
}),
$("table input").on("ifChecked", function () {
    checkState = "",
    $(this).parent().parent().parent().addClass("selected"),
    countChecked()
}),
$("table input").on("ifUnchecked", function () {
    checkState = "",
    $(this).parent().parent().parent().removeClass("selected"),
    countChecked()
});
var checkState = "";
$(".bulk_action input").on("ifChecked", function () {
    checkState = "",
    $(this).parent().parent().parent().addClass("selected"),
    countChecked()
}),
$(".bulk_action input").on("ifUnchecked", function () {
    checkState = "",
    $(this).parent().parent().parent().removeClass("selected"),
    countChecked()
}),
$(".bulk_action input#check-all").on("ifChecked", function () {
    checkState = "all",
    countChecked()
}),
$(".bulk_action input#check-all").on("ifUnchecked", function () {
    checkState = "none",
    countChecked()
}),
$(document).ready(function () {
    $(".expand").on("click", function () {
        $(this).next().slideToggle(200),
        $expand = $(this).find(">:first-child"),
        "+" == $expand.text() ? $expand.text("-") : $expand.text("+")
    })
}),
"undefined" != typeof NProgress && ($(document).ready(function () {
    NProgress.start()
}),
$(window).load(function () {
    NProgress.done()
})),
function (e, t) {
    var n = function (e, t, n) {
        var i;
        return function () {
            function c() {
                n || e.apply(a, o),
                i = null
            }
            var a = this
              , o = arguments;
            i ? clearTimeout(i) : n && e.apply(a, o),
            i = setTimeout(c, t || 100)
        }
    }
    ;
    jQuery.fn[t] = function (e) {
        return e ? this.bind("resize", n(e)) : this.trigger(t)
    }
}(jQuery, "smartresize");
function InitSelect2(values, selector, url, language) {
    var pageSize = 20;
    $(selector).select2({
        ajax: {
            url: url,
            type: "GET",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    searchString: params.term,
                    // search term
                    pageSize: pageSize,
                    pageNum: params.page
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                var more = (params.page * pageSize) < data.Total;
                return {
                    results: data.Results,
                    more: more
                };
            },
            cache: true
        },
        minimumInputLength: 1,
        language: language
    });
    if (values) {

    }
    $.each($.parseJSON(values), function () {
        $(selector).append('<option value="' + this.id + '" selected="selected">' + this.text + '</option>');
    })
};

function ShowAlertDialog(message) {
    $("#alertMessage").text(message);
    $('#alertModal').modal('show');
}

//full screen
function toggleFullScreen(elem) {
    // ## The below if statement seems to work better ## if ((document.fullScreenElement && document.fullScreenElement !== null) || (document.msfullscreenElement && document.msfullscreenElement !== null) || (!document.mozFullScreen && !document.webkitIsFullScreen)) {
    if ((document.fullScreenElement !== undefined && document.fullScreenElement === null) || (document.msFullscreenElement !== undefined && document.msFullscreenElement === null) || (document.mozFullScreen !== undefined && !document.mozFullScreen) || (document.webkitIsFullScreen !== undefined && !document.webkitIsFullScreen)) {
        if (elem.requestFullScreen) {
            elem.requestFullScreen();
        } else if (elem.mozRequestFullScreen) {
            elem.mozRequestFullScreen();
        } else if (elem.webkitRequestFullScreen) {
            elem.webkitRequestFullScreen(Element.ALLOW_KEYBOARD_INPUT);
        } else if (elem.msRequestFullscreen) {
            elem.msRequestFullscreen();
        }
    } else {
        if (document.cancelFullScreen) {
            document.cancelFullScreen();
        } else if (document.mozCancelFullScreen) {
            document.mozCancelFullScreen();
        } else if (document.webkitCancelFullScreen) {
            document.webkitCancelFullScreen();
        } else if (document.msExitFullscreen) {
            document.msExitFullscreen();
        }
    }
}

window.ElfInput = {
    elfNode: undefined, elfInsrance: '', dialogName: '', elfUrl: '', elfLang: '', currentId: '', elfDirHashMap: { // Dialog name / elFinder holder hash Map
        image: '', // example for 1st LocalFileVolume "/images"
        flash: '',
        files: '',
        link: ''
    },
}

function initCkEl(url, lang) {
    var elfNode, elfInsrance, dialogName,
        elfUrl = url, // Your connector's URL
        elfDirHashMap = { // Dialog name / elFinder holder hash Map
            image: '', // example for 1st LocalFileVolume "/images"
            flash: '',
            files: '',
            link: ''
        };
    var getLang = function () {
        return lang;
    }

    CKEDITOR.on('dialogDefinition', function (event) { // connection manager
        var editor = event.editor;
        var dialogDefinition = event.data.definition;
        var tabCount = dialogDefinition.contents.length;
        for (var i = 0; i < tabCount; i++) { // cycle to replace the click of button "View on the server"
            var content = dialogDefinition.contents[i];
            if (content != null) {
                var browseButton = content.get('browse');

                if (browseButton !== null) {
                    browseButton.hidden = false;
                    browseButton.onClick = function (dialog, i) {

                        dialogName = CKEDITOR.dialog.getCurrent()._.name;
                        if (elfNode) {
                            if (elfDirHashMap[dialogName] && elfDirHashMap[dialogName] != elfInsrance.cwd().hash) {
                                elfInsrance.request({
                                    data: { cmd: 'open', target: elfDirHashMap[dialogName] },
                                    notify: { type: 'open', cnt: 1, hideCnt: true },
                                    syncOnFail: true
                                });
                            }
                            elfNode.dialog('open');
                        } else {
                            elfNode = $('<div \>');
                            elfNode.dialog({
                                modal: true,
                                width: '80%',
                                title: 'Server File Manager',
                                create: function (event, ui) {
                                    var startPathHash = (elfDirHashMap[dialogName] && elfDirHashMap[dialogName]) ? elfDirHashMap[dialogName] : '';
                                    // elFinder configure
                                    elfInsrance = $(this).elfinder({
                                        startPathHash: startPathHash,
                                        useBrowserHistory: false,
                                        resizable: false,
                                        width: '100%',
                                        url: elfUrl,
                                        lang: getLang(),
                                        getFileCallback: function (file) {
                                            var url = file;
                                            var dialog = CKEDITOR.dialog.getCurrent();
                                            if (dialogName == 'image') {
                                                var urlObj = 'txtUrl'
                                            } else if (dialogName == 'flash') {
                                                var urlObj = 'src'
                                            } else if (dialogName == 'files' || dialogName == 'link') {
                                                var urlObj = 'url'
                                            } else {
                                                return;
                                            }
                                            dialog.setValueOf(dialog._.currentTabId, urlObj, url);
                                            elfNode.dialog('close');
                                            elfInsrance.disable();
                                        }
                                    }).elfinder('instance');
                                },
                                open: function () {
                                    elfNode.find('div.elfinder-toolbar input').blur();
                                    setTimeout(function () {
                                        elfInsrance.enable();
                                    }, 100);
                                },
                                resizeStop: function () {
                                    elfNode.trigger('resize');
                                }
                            }).parent().css({ 'zIndex': '11000' });
                        }

                    }
                }
            }
        }
    });
};

function InitElf(url, lang) {
    ElfInput.elfUrl = url;
    ElfInput.elfLang = lang;

};

function OpenElf(id, url) {
    ElfInput.currentId = id;
    if (url != undefined) {
        ElfInput.elfUrl = url;
    }
    if (ElfInput.elfNode == undefined) {
        ElfInput.elfNode = $('<div \>');
        ElfInput.elfNode.dialog({
            modal: true,
            width: '80%',
            title: 'Server File Manager',
            create: function (event, ui) {
                var startPathHash = (ElfInput.elfDirHashMap[ElfInput.dialogName] && ElfInput.elfDirHashMap[ElfInput.dialogName]) ? ElfInput.elfDirHashMap[ElfInput.dialogName] : '';
                // elFinder configure
                ElfInput.elfInsrance = $(this).elfinder({
                    startPathHash: startPathHash,
                    useBrowserHistory: false,
                    resizable: false,
                    width: '100%',
                    url: ElfInput.elfUrl,
                    lang: ElfInput.elfLang,
                    getFileCallback: function (file) {
                        var url = file;
                        $(ElfInput.currentId).val(url).change();
                        ElfInput.elfNode.dialog('close');
                        ElfInput.elfInsrance.disable();

                    }
                }).elfinder('instance');
            },
            open: function () {
                ElfInput.elfNode.find('div.elfinder-toolbar input').blur();
                setTimeout(function () {
                    ElfInput.elfInsrance.enable();
                }, 100);
            },
            resizeStop: function () {
                ElfInput.elfNode.trigger('resize');
            }
        }).parent().css({ 'zIndex': '11000' });
    }
    else {
        if (ElfInput.elfDirHashMap[ElfInput.dialogName] && ElfInput.elfDirHashMap[ElfInput.dialogName] != ElfInput.elfInsrance.cwd().hash) {
            ElfInput.elfInsrance.request({
                data: { cmd: 'open', target: ElfInput.elfDirHashMap[dialogName] },
                notify: { type: 'open', cnt: 1, hideCnt: true },
                syncOnFail: true
            });
        }
        ElfInput.elfNode.dialog('open');
    }
};

function ReplaceCKEditor(selector) {
    if (typeof selector === 'undefined') {
        var ckSelector = '.ck_editor';
    }
    else {
        var ckSelector = selector + ' .ck_editor';
    }
    $(ckSelector).each(function () {
        var editor = CKEDITOR.instances[$(this).attr('id')];
        if (!editor) {
            CKEDITOR.replace($(this).attr('id'), { filebrowserBrowseUrl: '#' });
        }
    });
};

function DestroyCKEditor(selector) {
    if (typeof selector === 'undefined') {
        var ckSelector = '.ck_editor';
    }
    else {
        var ckSelector = selector + ' .ck_editor';
    }
    $(ckSelector).each(function () {
        var editor = CKEDITOR.instances[$(this).attr('id')];
        if (editor) {
            editor.destroy();
        }
    });
}

function UpdateCKEditor(selector) {
    if (typeof selector === 'undefined') {
        var ckSelector = '.ck_editor';
    }
    else {
        var ckSelector = selector + ' .ck_editor';
    }
    $(ckSelector).each(function () {
        var editor = CKEDITOR.instances[$(this).attr('id')];
        if (editor) {
            editor.updateElement();
        }
    });
}

function ReplaceSelect2(lang, selector) {
    if (typeof selector === 'undefined') {
        $('.ddl_select2').select2({
            theme: "bootstrap",
            language: lang
        });

        $(".color_select2").select2({
            theme: "bootstrap",
            language: lang,
            templateResult: formatColor,
            templateSelection: formatColor
        });
    }
    else {
        var select2Selector = selector + ' .ddl_select2';
        $(select2Selector).select2({
            theme: "bootstrap",
            language: lang
        });

        $(select2Selector = selector + ' .color_select2').select2({
            theme: "bootstrap",
            language: lang,
            templateResult: formatColor,
            templateSelection: formatColor
        });
    }

};


function InitAjaxForm(selector, lang) {
    var form = $(selector)
        .removeData("validator") /* added by the raw jquery.validate plugin */
        .removeData("unobtrusiveValidation");  /* added by the jquery unobtrusive plugin */

    $.validator.unobtrusive.parse(form);

    ReplaceCKEditor(selector);
    if (typeof lang !== 'undefined') {
        ReplaceSelect2(lang, selector);
    }
};

function ShowPopup(selector, lang) {
    InitAjaxForm(selector, lang);
    $(selector).modal();
}

function HideModal(selector) {
    $(selector).modal('hide');
}

function CloneTableRow(tableSelector) {
    var rowCount = $(tableSelector + " > tbody > tr").length;
    var idPlaceHolder = '_0_';
    var namePlaceHolder = '[0]';
    var newId = "_" + rowCount + "_";
    var newName = "[" + rowCount + "]";
    var childTableExist = $(tableSelector + " table").length > 0;
    var hasParentTable = $(tableSelector).parents('table').length > 0;

    var row = $(tableSelector + " > tbody > tr:first").clone().show().find("input,table,div,select").each(function () {
        $(this).attr({
            'id': function (_, id) {
                return ChangeRowAttribute(id, idPlaceHolder, newId, hasParentTable);
            },
            'name': function (_, name) {
                return ChangeRowAttribute(name, namePlaceHolder, newName, hasParentTable);
            },
            'onchange': function (_, onChange) {
                return ChangeRowAttribute(onChange, idPlaceHolder, newId, hasParentTable);
            },
        });
    }).val('').end();

    row = row.find("button,span").each(function () {
        $(this).attr({
            'onClick': function (_, onClick) {
                return ChangeRowAttribute(onClick, idPlaceHolder, newId, hasParentTable);
            },
            'data-target': function (_, dataTarget) {
                return ChangeRowAttribute(dataTarget, idPlaceHolder, newId, hasParentTable);
            },
            'data-valmsg-for': function (_, dataValmsgFor) {
                return ChangeRowAttribute(dataValmsgFor, namePlaceHolder, newName, hasParentTable);
            },
            'for': function (_, forAttr) {
                return ChangeRowAttribute(forAttr, idPlaceHolder, newId, hasParentTable);
            },
        });
    }).end();

    row = row.find("tbody tr:gt(0)").remove().end().appendTo(tableSelector);

    InitAjaxForm(tableSelector + '  > tbody > tr:eq(' + rowCount + ')');
}

function ChangeRowAttribute(currentValue, placeHolder, newReplaceValue, hasParentTable) {
    if (typeof currentValue != 'undefined') {
        var n = currentValue.lastIndexOf(placeHolder);
        if (n != -1) {
            if (hasParentTable) {
                return currentValue.slice(0, n) + currentValue.slice(n).replace(placeHolder, newReplaceValue);
            }
            return currentValue.replace(placeHolder, newReplaceValue);
        }
        return currentValue;
    }
    return '';
};

function DeleteTableRow(_this) {
    var row = $(_this).closest('tr');
    row.hide();
    row.find("input").each(function () {
        $(this).attr({
            'value': ''
        });
    });
};

function SetFileName(_this, inputNameSelector) {
    var path = decodeURI($(_this).val());
    $(_this).val(path);
    var fileName = path.substr(path.lastIndexOf("/") + 1);
    $(inputNameSelector).val(fileName)
};

function formatColor(color) {
    if (!color.id) { return color.text; }
    var colorValue = color.element.text.split('/');
    var $color = $(
        '<span><i style="background-color:' + colorValue[1] + ';" class="color-square"></i> ' + colorValue[0] + '</span>'
    );
    return $color;
};

$.fn.extend({
    trackChanges: function () {
        $(":input", this).change(function () {
            $(this.form).data("changed", true);
        });
    }
    ,
    isChanged: function () {
        return this.data("changed");
    }
});

function ConfirmChangeVersion() {
    if (isDirty) {
        var result = confirm("Bạn sẽ mất dữ liệu của các phiên bản cũ nếu thay đổi. Bạn có chắc muốn tiếp tục thực hiện?");
        return result;
    }
    else {
        HideModal('#divModal');
        return false;
    }
}

function ChangeUrl(object) {
    if (typeof (history.pushState) != "undefined") {
        var url, index = document.URL.indexOf('?');
        if (index > 0) {
            url = document.URL.substr(0, index);
        }
        else
            url = document.URL;
        var isFirstAtt = true;
        for (var i in object) {
            if (object.hasOwnProperty(i)) {
                if (isFirstAtt) {
                    if (object[i] != '') {
                        url += "?" + i + "=" + object[i];
                        isFirstAtt = false;
                    }
                }
                else {
                    url += object[i] != '' ? "&" + i + "=" + object[i] : "";
                }
            }
        }

        history.pushState({}, null, url);
    }
}

function UpdateQueryStringParameter(key, value) {
    var url = window.location.href;
    var re = new RegExp("([?&])" + key + "=.*?(&|$)", "i");
    var separator = url.indexOf('?') !== -1 ? "&" : "?";
    if (url.match(re)) {
        url = url.replace(re, '$1' + key + "=" + value + '$2');
    }
    else {
        url = url + separator + key + "=" + value;
    }
    history.pushState({}, null, url);
}

function UpdatePagingAjaxUrl(_this) {
    var currentUrl = window.location.href;
    currentUrl = currentUrl.substr(0, currentUrl.indexOf('?'));
    var target = _this.getAttribute("href");
    var url = currentUrl + target.substr(target.indexOf('?'));
    history.pushState({}, null, url);
}

function GetLayoutImages() {

}