/*!
 * Vue.jerry.helper.js v1
 * (c) 2018-2020 Jerry 15802775429
 * Released under the MIT License.
 */

var vueApp = null;

(function (global, factory) {
    global.helper = factory();
})(this, (function (defaultValue) {
    'use strict';

    var _default = $.extend(true, {
        FormLabelWidth: "150px",
        FormLabelSuffix: "：",
        LoadingText: "正在努力加载中....",
        MessageTitle: "",
        ConfirmTitle: "",
        OK: "确定",
        Cancel: "取消",
        AddTitle: "添加",
        EditTitle: "编辑",
        SelectRecordFirst: "请先选择要操作的记录。",
        Delete: "确实要删除记录吗?",
        InValidModel: "请先填写有效的数据.",
        Callback: function () { },
        Vue: {
            AppElement: "#JerryPlatApp"
        },
        AllPageParam: {
            PageIndex: 1,
            PageSize: 100
        }
    }, defaultValue);

    //#region Validation
    function _isIdCard(idCard) {
        return /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$/.test(idCard)//正则：身份证号码15位
        || /^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9Xx])$/.test(idCard);//正则：身份证号码18位
    }
    function _isTel(tel) {
        return /^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|(147))\d{8}$/.test(tel)
        || /^0\d{2,3}[- ]?\d{7,8}$/.test(tel)
        || /^[1]\d{10}$/.test(tel);
    }
    function _isEmail(email) {
        return /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(email);
    }
    function _isVerifyCode(code) {
        return /^[0-9a-zA-Z]{4}$/.test(code);
    }
    //#endregion

    //#region Loading Dialog
    var _openLoadingCount = 0;
    function _openLoading() {
        if (_openLoadingCount == 0) {
            vueApp && vueApp.$indicator && vueApp.$indicator.open({
                text: _default.LoadingText,
                spinnerType: 'fading-circle'
            });
        }
        _openLoadingCount++;
    }

    function _closeLoading() {
        if (_openLoadingCount == 1) {
            vueApp && vueApp.$indicator && vueApp.$indicator.close();
        }
        if (_openLoadingCount > 0) {
            _openLoadingCount--;
        }
    }
    //#endregion

    //#region Alert & Confirm Dialog
    function _message(message, callback) {
        vueApp && (vueApp.$messagebox.alert(message, _default.MessageTitle).then(callback || _default.Callback));
    }

    function _alert(message, callback) {
        _message(message, callback);
    }

    function _confirm(message, okCallback, cacelCallback) {
        vueApp && (vueApp.$messagebox.confirm(message, _default.ConfirmTitle)
            .then(okCallback || _default.Callback)
            .catch(cacelCallback || _default.Callback));
    }

    function _delete(okCallback, cacelCallback) {
        _confirm(_default.Delete, okCallback, cacelCallback);
    }

    //#endregion

    ///#region private functions
    function _redirect(url) {
        window.location.href = url;
    }

    function _getItemValue(array, where, key) {
        if (array == null) {
            return null;
        }
        where = where || function () { return false; };
        key = key || 'Name';
        for (var i = 0; i < array.length; i++) {
            if (where(array[i])) {
                return array[i][key];
            }
        }
        return null;
    }

    function _addDays(date, days) {
        return new Date(date.getTime() + 24 * 60 * 60 * 1000 * days);
    }

    /*
    * context: {A:{B:{C:1}}}
    * name: "A.B.C"
    */
    function _getContext(context, keyList, split) {
        split = split || ".";
        var keys = keyList.split(split);
        var value = context;
        $.each(keys, function (index, item) {
            value = value[item];
        });
        return value;
    }

    function _getCookie(key) {
        return $.cookie(key);
    }

    function _getQueryString(name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    function _getFileName(fileFullPath, split) {
        if (fileFullPath == null) {
            return "";
        }
        split = split || "/";
        return fileFullPath.substr(fileFullPath.lastIndexOf(split) + 1);
    }

    function _format(date, format) {
        var o = {
            "M+": date.getMonth() + 1, //month
            "d+": date.getDate(),    //day
            "h+": date.getHours(),   //hour
            "m+": date.getMinutes(), //minute
            "s+": date.getSeconds(), //second
            "q+": Math.floor((date.getMonth() + 3) / 3),  //quarter
            "S": date.getMilliseconds() //millisecond
        };
        if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
        (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o) if (new RegExp("(" + k + ")").test(format))
            format = format.replace(RegExp.$1,
            RegExp.$1.length == 1 ? o[k] :
            ("00" + o[k]).substr(("" + o[k]).length));
        return format;
    }

    //strDate : "/Date(1516172795107)/" OR "2018-03-23T23:02:38.703"
    //return 1516172795107  OR "2018-03-23T23:02:38.703"
    function _getDate(strDate) {
        if (strDate == "" || strDate == null) {
            return null;
        }
        if (strDate[0] == "/") {
            return eval(strDate.substring(6, strDate.length - 2));
        }
        return strDate;
    }

    function _getHttpOptions(url, method, data, isUploadFile, successcallback, failcallback) {
        var options = {
            url: url,
            type: method,
            success: successcallback || _callback,
            error: failcallback || _callback,
            data: data || {}
        };

        if (method == "POST") {
            options.data.__RequestVerificationToken = $("input[name=__RequestVerificationToken]").val();
        }

        if (isUploadFile) {
            options.data = _getFormData(options.data);
            options.contentType = "multipart/form-data";
        }

        return options;
    }

    function _getFormData(model) {
        var formData = new FormData();
        if (model != null) {
            $.each(model, function (key, value) {
                formData.append(key, value);
            });
        }
        return formData;
    }

    function _isNullOrEmpty(str) {
        return str == null || str == '';
    }

    function _ajax(method, url, data, successcallback, failcallback, isUploadFile) {
        successcallback = successcallback || _default.Callback;
        failcallback = failcallback || _default.Callback;

        _openLoading();
        $.ajax(_getHttpOptions(url, method, data, isUploadFile,
            function (data) {
                setTimeout(_closeLoading, 100);
                if (data.Status == constantHelper.Ok) {
                    successcallback(data.Data);
                    return;
                }

                if (data.Status == constantHelper.Logout) {
                    _redirect(data.Data);
                    return;
                }
                _alert(data.Message);
            }, function (xhr, error, ex) {
                setTimeout(_closeLoading, 100);
                switch (xhr.status) {
                    case -1://
                        _alert(error);
                        return;
                    case 401:
                        _redirect(xhr.responseText);
                        return;
                }
                failcallback(xhr);
            }));
    }

    function _get(url, data, successcallback, failcallback) {
        _ajax("GET", url, data, successcallback, failcallback, false);
    }

    function _post(url, data, successcallback, failcallback) {
        _ajax("POST", url, data, successcallback, failcallback, false);
    }

    function _postFile(url, data, successcallback, failcallback) {
        _ajax("POST", url, data, successcallback, failcallback, true);
    }
    ///#endregion

    //#endregion

    var bus = new Vue();
    function _onBus(name, callback) {
        bus.$on(name, callback);
    };
    function _emitBus(name, callback) {
        bus.$emit(name, callback);
    };

    function _getVueModelOptions(options) {
        options = $.extend(true, {
            el: _default.Vue.AppElement,
            data: {
                TabbarSelected: "t0",
                TabbarList: [
                    { Id: "t0", Url: "/Mob", Icon: "sy", Text: "首页" },
                    { Id: "t1", Url: "/Mob/Enroll", Icon: "bm", Text: "报名学车" },
                    { Id: "t2", Url: "/Mob/Question", Icon: "xt", Text: "习题考试" },
                    { Id: "t3", Url: "/Mob/User", Icon: "grzx", Text: "个人中心" }
                ],
                Common: {
                    FormLabelWidth: _default.FormLabelWidth,
                    FormLabelSuffix: _default.FormLabelSuffix
                }
            },
            watch: {
                "TabbarSelected": function (val, oldVal) {
                    var item = this.getTabbarItem(val);
                    if (item == null) {
                        return;
                    }
                    _redirect(item.Url);
                }
            },
            created: function () {
                this.initPage();
            },
            methods: {
                /*
                * index
                */
                getCharIndex: function (index) {
                    return "ABCDEFGHIJKLMN"[index];
                },
                getDate: function (date, format) {
                    return date;
                    //format = format || "yyyy-MM-dd hh:mm:ss";
                    //return _format(new Date(_getDate(date)), format);
                },
                getItemValue: function (array, id, key) {
                    return helper.getItemValue(array, o=>o.Id == id, key);
                },
                existFile: function (fileName) {
                    return !_isNullOrEmpty(fileName);
                },
                getTabbarItem: function (id) {
                    for (var i = 0; i < this.TabbarList.length; i++) {
                        if (this.TabbarList[i].Id == id) {
                            return this.TabbarList[i];
                        }
                    }
                    return null;
                },
                getIconSrc: function (item) {
                    var src = "/Content/Mob/"+item.Icon;
                    if (this.TabbarSelected == item.Id) {
                        src += ".active";
                    }
                    src += ".png";
                    return src;
                },
                initPage: function () {
                },
                handleSelect: function (url) {
                    _redirect(url);
                },
                submitFormBefore: function () {
                    return true;
                },
                goBack: function () {
                    window.history.back();
                },
                submitForm: function (formName, callback) {
                    var _this = this;
                    _this.$refs[formName].validate(function (valid) {
                        if (!valid || !_this.submitFormBefore(formName)) {
                            _alert(_default.InValidModel);
                            return false;
                        }
                        callback(formName);
                    });
                },
                resetForm: function (formName) {
                    this.$refs[formName].resetFields();
                }
            }
        }, options);
        return options;
    }

    function _qqGeocoder(position, callback) {
        try{
            var geocoder = new qq.maps.Geocoder({
                complete: function (result) {
                    callback(result);
                }
            });
            var coord = new qq.maps.LatLng(position.Latitude, position.Longitude);
            geocoder.getAddress(coord);
        } catch (e) {
            _alert(e.message);
        }
    }

    function _getLocation(position, callback) {
        _qqGeocoder(position, callback);
    }

    return {
        "default": _default,

        isIdCard: _isIdCard,
        isTel: _isTel,
        isEmail: _isEmail,
        isVerifyCode: _isVerifyCode,

        alert: _alert,
        confirm: _confirm,
        "delete": _delete,

        isNullOrEmpty: _isNullOrEmpty,

        redirect: _redirect,

        getCookie: _getCookie,
        getQueryString: _getQueryString,
        getFileName: _getFileName,
        addDays: _addDays,
        getDate: _getDate,
        getItemValue: _getItemValue,
        format: _format,

        ajax: _ajax,
        get: _get,
        post: _post,
        postFile: _postFile,

        on: _onBus,
        emit: _emitBus,

        getVueModelOptions: _getVueModelOptions,

        getContext: _getContext,

        getLocation: _getLocation
    };
}));

function getVueOptions() {
    return helper.getVueModelOptions();
}