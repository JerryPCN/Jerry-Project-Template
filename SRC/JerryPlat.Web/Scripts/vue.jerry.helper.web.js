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
        Area: "/Admin",
        BaseUrl: "",
        SideWidth: "200px",
        SideHideWidth: "0px",
        FormLabelWidth: "150px",
        CloseOnClickModal: false,
        CloseOnPressEscape: false,
        DialogWidth: "60%",
        FormLabelSuffix: "：",
        LoadingText: "正在努力加载中....",
        MessageTitle: "消息",
        ConfirmTitle: "提示",
        OK: "确定",
        Cancel: "取消",
        AddTitle: "添加",
        EditTitle: "编辑",
        SelectRecordFirst: "请先选择要操作的记录。",
        Delete: "确实要删除记录吗?",
        InValidModel: "请先填写有效的数据.",
        Callback: function () { },
        Vue: {
            AppElement: "#JerryPlatApp",
            DefaultDataList: "getDataList",
            DefaultTable: "List",
            DefaultForm: "List.ModelDialog"
        },
        AllPageParam: {
            PageIndex: 1,
            PageSize: 100
        }
    }, defaultValue);

    //#region Loading Dialog
    var loading = null;
    var _openLoadingCount = 0;
    function _openLoading() {
        if (_openLoadingCount == 0) {
            vueApp && (loading = vueApp.$loading({
                lock: true,
                text: _default.LoadingText,
                spinner: 'el-icon-loading',
                background: 'rgba(0, 0, 0, 0.7)'
            }));
        }
        _openLoadingCount++;
    }

    function _closeLoading() {
        if (_openLoadingCount == 1) {
            vueApp && loading && loading.close();
            loading = null;
        }
        if (_openLoadingCount > 0) {
            _openLoadingCount--;
        }
    }
    //#endregion

    //#region Alert & Confirm Dialog
    function _message(message, type, okCallback) {
        vueApp && (vueApp.$alert(message, _default.MessageTitle, {
            type: type,
            closeOnClickModal: true,
            closeOnPressEscape: true,
            confirmButtonText: _default.OK,
            callback: okCallback || _default.Callback
        }));
    }

    function _alert(message, okCallback) {
        _message(message, "info", okCallback);
    }

    function _error(message, okCallback) {
        _message(message, "error", okCallback);
    }

    function _confirm(message, okCallback, cacelCallback) {
        vueApp && (vueApp.$confirm(message, _default.ConfirmTitle, {
            confirmButtonText: _default.OK,
            cancelButtonText: _default.Cancel,
            type: 'warning'
        }).then(okCallback || _default.Callback).catch(cacelCallback || _default.Callback));
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
        if(array == null){
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
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)","i");
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

    //strDate : "/Date(1516172795107)/"
    //return 1516172795107
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
                        _error(error);
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

    function _getInitList(model, options) {
        model = model || {};
        return $.extend(true, {
            PageParam: {
                PageIndex: 1,
                PageSize: 20
            },
            PageModel: {
                TotalItem: 0,
                TotalPage: 1
            },
            Action: null,
            Sort: "",
            Data: [],
            MultipleSelection: [],
            ModelDialog: {
                Model: model,
                Visible: false,
                Progress: [0]
            }
        }, options);
    }

    function _getVueModelOptions(options) {
        options = $.extend(true, {
            el: _default.Vue.AppElement,
            data: {
                User: {
                    ModelDialog: {
                        Visible: false,
                        Model: {
                            Original: "",
                            Password: "",
                            Confirm: ""
                        }
                    }
                },
                Common: {
                    MenuVisible: true,
                    CloseOnClickModal: _default.CloseOnClickModal,
                    CloseOnPressEscape: _default.CloseOnPressEscape,
                    DialogWidth: _default.DialogWidth,
                    SideWidth: _default.SideWidth,
                    FormLabelWidth: _default.FormLabelWidth,
                    FormLabelSuffix: _default.FormLabelSuffix
                }
            },
            created: function () {
                this.initPage();
            },
            methods: {
                getSex: function (sex) {
                    return sex == 1 ? "男" : "女";
                },
                getDate: function (date, format) {
                    return date;
                    //format = format || "yyyy-MM-dd hh:mm:ss";
                    //return _format(new Date(_getDate(date)), format);
                },
                getItemValue: function (array, id, key) {
                    return helper.getItemValue(array, o=>o.Id == id, key);
                },
                getUploadData: function (folder) {
                    folder = folder || "File";
                    return {
                        folder: folder,
                        __RequestVerificationToken: $("input[name=__RequestVerificationToken]").val()
                    };
                },
                checkPassword: function (rule, value, callback) {
                    if (value === '') {
                        callback(new Error('请再次输入密码'));
                    } else if (value !== this.User.ModelDialog.Model.Password) {
                        callback(new Error('两次输入密码不一致!'));
                    } else {
                        callback();
                    }
                },
                initPage: function () {
                },
                handleSelect: function (url) {
                    _redirect(url);
                },
                clearData: function(){
                    _confirm("确定要清空站点数据吗？", function () {
                        _post("/Admin/System/ClearData", null, function (data) {
                            _alert("清空站点数据成功，即将重新登陆。", function () {
                                _redirect("/Admin");
                            });
                        });
                    })
                },
                restartSite: function(){
                    _confirm("确定要重启站点吗？", function () {
                        _post("/Admin/System/Restart", null, function (data) {
                            _alert("重启站点成功，即将重新登陆。", function () {
                                _redirect("/Admin");
                            });
                        });
                    })
                },
                handleMenuCommand: function (command) {
                    switch (command) {
                        case "change-password":
                            this.User.ModelDialog.Visible = true;
                            break;
                        case "clear-data":
                            this.clearData();
                            break;
                        case "restart-site":
                            this.restartSite();
                            break;
                    }
                },
                getProgress: function(context, index){
                    var progress = context && context.Progress;
                    if (!progress || progress.length - 1 < index) {
                        return 0;
                    }
                    return progress[index];
                },
                submitFormBefore: function () {
                    return true;
                },
                submitForm: function (formName, callback, action, aftersubmitcallback, table) {
                    var _this = this;

                    _this.$refs[formName].validate(function (valid) {
                        if (!valid || !_this.submitFormBefore(formName)) {
                            _alert(_default.InValidModel);
                            return false;
                        }

                        callback = callback || _this["handleSave"];
                        aftersubmitcallback = aftersubmitcallback || _this[_default.Vue.DefaultDataList];
                        table = table || _default.Vue.DefaultTable;
                        callback(formName, action, aftersubmitcallback, table);
                    });
                },
                resetForm: function (formName) {
                    this.$refs[formName].resetFields();
                },
                setMenu: function () {
                    this.Common.MenuVisible = !this.Common.MenuVisible;
                    this.Common.SideWidth = (this.Common.MenuVisible ? _default.SideWidth : _default.SideHideWidth);
                }
            }
        }, options);
        return options;
    }

    function _getVueListOptions(options, action, pageSize) {
        options = $.extend(true, {
            data: {
                List: _getInitList(null, { Action: action, PageParam: { PageSize: pageSize } })
            },
            methods: {
                /*
                * title: String --> Popup Name
                * modelDialog: Object -->  List.ModelDialog
                */
                getTitle: function (title, modelDialog) {
                    modelDialog = modelDialog || _getContext(this, _default.Vue.DefaultForm);
                    return (modelDialog.Model.Id == 0 ? _default.AddTitle : _default.EditTitle) + " " + title;
                },
                getFileName: function (fileName) {
                    return _getFileName(fileName);
                },
                existFile: function (fileName) {
                    return !_isNullOrEmpty(fileName);
                },
                /*
                * form: String --> "List.ModelDialog"
                * model: Object
                */
                openDialog: function (form, model) {
                    form = form || _default.Vue.DefaultForm;
                    var modelDialog = _getContext(this, form);
                    modelDialog.Visible = true;
                    modelDialog.Model = model;
                },
                closeDialogBefore: function () {
                    return true;
                },
                /*
                * form: String --> "List.ModelDialog"
                */
                closeDialog: function (form) {
                    if (!this.closeDialogBefore()) {
                        return;
                    }
                    var _this = this;
                    form = form || _default.Vue.DefaultForm;
                    var modelDialog = _getContext(this, form);
                    _this.resetForm(form);

                    modelDialog.Visible = false;
                    if (modelDialog.Progress != null) {
                        $.each(modelDialog.Progress, function (index, item) {
                            Vue.set(modelDialog.Progress, index, 0);//https://cn.vuejs.org/v2/api/#Vue-set
                        });
                    }
                }
            }
        }, options);

        //var _data = options.data;

        //options.data = function () {
        //    return _data;
        //};

        return _getVueModelOptions(options);
    }

    /*
    * controller：String --> "ControllerName"
    * initModel: Object --> {List:_getInitList({Id:0,Name:""}), OtherList:_getInitList({Id:0,Name:""})}
                        --> {List:{ModelDialog:{Model:{Id:0,Name:""}}}, OtherList:{ModelDialog:{Model:{Id:0,Name:""}}}}
    *         OR Function --> function(){return {Id:0,Name:""}}
    */
    function _getPageVueOptions(controller, initModel, options, action, pagesize) {
        if (typeof (initModel) == "function") {
            initModel = { List: { ModelDialog: { Model: initModel } } };
        } else {
            initModel = $.extend(true, { List: { ModelDialog: { Model: function () { return {} } } } }, initModel);
        }
        options = $.extend(true, {
            data: {
                List:{
                    ModelDialog: {
                        Model: _getContext(initModel,_default.Vue.DefaultForm).Model()
                    }
                }
            },
            methods: {
                initPage: function () {
                    this.getDataList();
                },
                getSearchModel: function (table) {
                    table = table || _default.Vue.DefaultTable;
                    return {
                        PageParam: _getContext(this, table).PageParam,
                        Sort: this[table].Sort
                    };
                },
                /*
                * index
                */
                getCharIndex: function (index) {
                    return "ABCDEFGHIJKLMN"[index];
                },
                /*
                * table: String --> "List"
                */
                getDataList: function (table) {
                    table = table || _default.Vue.DefaultTable;
                    var _this = this;
                    var action = _this[table].Action || _default.Area + "/" + controller + "/Get" + table;
                    helper.post(action, this.getSearchModel(table), function (data) {
                        _setPageModel(data, _this[table]);
                    });
                },
                /*
                * table: Object --> List
                */
                handleSelectionChange: function(val, table) {
                    table = table || this[_default.Vue.DefaultTable];
                    table.MultipleSelection = val;
                },
                /*
                * table: String --> List
                */
                handleSortChange: function (sort, table) {
                    table = table || _default.Vue.DefaultTable;
                    if (_isNullOrEmpty(sort) || _isNullOrEmpty(sort.prop)) {
                        this[table].Sort = "";
                    } else {
                        this[table].Sort = sort.prop + " " + (sort.order == "ascending" ? "ASC" : "DESC");
                    }
                    this.getDataList(table);
                },
                getItemId: function (item) {
                    return item.Id
                },
                /*
                * table: Object --> List
                */
                getSelectedIdList: function (table) {
                    var _this = this;
                    table = table || _this[_default.Vue.DefaultTable];
                    var idList = [];
                    $.each(table.MultipleSelection, function (index, item) {
                        idList.push(_this.getItemId(item));
                    });
                    return { idList: idList };
                },
                handleAddBefore: function (model) {
                    return model;
                },
                /*
                * form: String --> "List.ModelDialog"
                */
                handleAdd: function (form) {
                    form = form || _default.Vue.DefaultForm;
                    var model = this.handleAddBefore(_getContext(initModel, form).Model());
                    this.openDialog(form, model);
                },
                handleEditBefore: function(model){
                    return model;
                },
                /*
                * form: String --> "List.ModelDialog"
                * action: String --> null, '', '/Admin/Banner/GetDetail'
                */
                handleEdit: function (index, row, form, action, action_data) {
                    var _this = this;
                    form = form || _default.Vue.DefaultForm;
                    if (action == null) {
                        var model = _this.handleEditBefore(JSON.parse(JSON.stringify(row)));
                        _this.openDialog(form, model);//https://cn.vuejs.org/v2/api/#data
                        return;
                    }
                    action = action || _default.Area + "/" + controller + "/GetDetail";
                    action_data = action_data || { Id: row.Id };
                    _post(action, action_data, function (data) {
                        var model = _this.handleEditBefore(data);
                        _this.openDialog(form, model);
                    });
                },
                handleAddRowBefore: function (index, collection) {
                    return true;
                },
                handleAddRow: function (item, collection) {
                    if (!this.handleAddRowBefore(item, collection)) {
                        return;
                    }
                    collection.push(item);
                },
                handleDeleteRowBefore: function (index, collection, info) {
                    return true;
                },
                handleDeleteRow: function (index, collection, info) {
                    if (!this.handleDeleteRowBefore(index, collection, info)) {
                        return;
                    }
                    collection.splice(index, 1);
                },
                /*
                * table: String --> "List"
                */
                handleDelete: function (index, row, action, callback, table) {
                    var _this = this;
                    if (typeof (action) === "function") {
                        table = callback;
                        callback = action;
                        action = null;
                    }
                    action = action || _default.Area + "/" + controller + "/Delete/";
                    callback = callback || this[_default.Vue.DefaultDataList];
                    table = table || _default.Vue.DefaultTable;
                    _delete(function () {
                        _post(action + row.Id, null, function (data) {
                            _resetPage(_this[table]);
                            callback(table);
                        });
                    });
                },
                /*
                * table: String --> "List"
                */
                handleDeleteList: function (action, callback, table) {
                    var _this = this;
                    if (typeof (action) === "function") {
                        table = callback;
                        callback = action;
                        action = null;
                    }

                    table = table || _default.Vue.DefaultTable;

                    if (_this[table].MultipleSelection.length == 0) {
                        _alert(_default.SelectRecordFirst);
                        return;
                    }

                    action = action || "/Admin/" + controller + "/DeleteList";
                    callback = callback || this[_default.Vue.DefaultDataList];

                    _delete(function () {
                        _post(action, _this.getSelectedIdList(_this[table]), function (data) {
                            _resetPage(_this[table]);
                            callback(table);
                        });
                    });
                },
                /*
                * form： String --> "List.ModelDialog"
                * table: String --> "List"
                */
                handleSave: function (form, action, callback, table) {
                    var _this = this;
                    if (typeof (action) === "function") {
                        table = callback;
                        callback = action;
                        action = null;
                    }
                    action = action || _default.Area + "/" + controller + "/Save";
                    callback = callback || this[_default.Vue.DefaultDataList];
                    table = table || _default.Vue.DefaultTable;
                    var modelDialog = _getContext(this, form);
                    helper.post(action, modelDialog.Model, function (data) {
                        helper.resetPage(_this[table]);
                        _this.closeDialog(form, null, modelDialog);
                        callback(table);
                    });
                },
                /*
                * modelDialog: Object --> List.ModelDialog
                */
                handleBeforeUpload: function (field, modelDialog, file) {
                    modelDialog = modelDialog || _getContext(this, _default.Vue.DefaultForm);
                    Vue.set(modelDialog.Progress, field, 0);//https://cn.vuejs.org/v2/api/#Vue-set
                    return true;
                },
                /*
                * modelDialog: Object --> List.ModelDialog
                */
                handleUploadProgress: function (field, modelDialog, event, file, fileList) {
                    modelDialog = modelDialog || _getContext(this, _default.Vue.DefaultForm);
                    Vue.set(modelDialog.Progress, field, event.percent);//https://cn.vuejs.org/v2/api/#Vue-set
                },
                /*
                * modelDialog: Object --> List.ModelDialog
                */
                handleUploadSuccess: function (field, modelDialog, response, file, fileList) {
                    if (response.Status == constantHelper.Ok) {
                        modelDialog = modelDialog || _getContext(this, _default.Vue.DefaultForm);
                        var cmd = "modelDialog.Model";
                        $.each(field.split('.'), function (index, item) {
                            cmd += "['" + item + "']";
                        });
                        cmd += "=response.Data;";
                        eval(cmd);
                    }
                },
                /*
                * table: String --> List
                */
                handleSizeChange: function (val, table, callback) {
                    this.handleCurrentChange(val, table, callback);
                },
                /*
                * table: String --> List
                */
                handleCurrentChange: function (val, table, callback) {
                    table = table || _default.Vue.DefaultTable;
                    callback = callback || this[_default.Vue.DefaultDataList];
                    this[table].PageParam.PageIndex = val;
                    callback(table);
                }
            }
        }, options);
        return _getVueListOptions(options, action, pagesize);
    }

    function _resetPage(table, pageIndex) {
        table = table || (vueApp && (vueApp[_default.Vue.DefaultTable]));
        pageIndex = pageIndex || 1;
        table && (table.PageParam.PageIndex = pageIndex);
    }

    function _setPageModel(data, table) {
        table = table || (vueApp && (vueApp[_default.Vue.DefaultTable]));
        if (table == null) {
            return;
        }

        table.Data = data.Data;
        table.PageParam = Object.assign({}, table.PageParam, data.PageParam);
        table.PageModel = Object.assign({}, table.PageModel, data.PageModel);
    }

    function _setModel(model, modelDialog) {
        modelDialog = vueApp && (_getContext(vueApp,_default.Vue.DefaultForm));
        modelDialog && (modelDialog.Model = Object.assign({}, modelDialog.Model, model));
    }

    return {
        "default": _default,

        alert: _alert,
        error: _error,
        confirm : _confirm,
        "delete" : _delete,

        isNullOrEmpty : _isNullOrEmpty,

        redirect : _redirect,

        getCookie : _getCookie,
        getQueryString : _getQueryString,
        getFileName: _getFileName,
        addDays:_addDays,
        getDate: _getDate,
        getItemValue: _getItemValue,
        format: _format,

        ajax : _ajax,
        get : _get,
        post : _post,
        postFile: _postFile,

        on: _onBus,
        emit: _emitBus,

        getInitList: _getInitList,
        getVueModelOptions: _getVueModelOptions,
        getVueListOptions: _getVueListOptions,
        getPageVueOptions: _getPageVueOptions,

        getContext: _getContext,

        setPageModel: _setPageModel,
        setModel: _setModel,
        resetPage: _resetPage
    };
}));

function getVueOptions() {
    return helper.getVueModelOptions();
}