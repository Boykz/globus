/**
 * editor_plugin_src.js
 *
 * Copyright 2009, Moxiecode Systems AB
 * Released under LGPL License.
 *
 * License: http://tinymce.moxiecode.com/license
 * Contributing: http://tinymce.moxiecode.com/contributing
 */


(function () {

    tinymce.create('tinymce.plugins.ssc', {
        init: function (ed, url) {
            if (!window.jQuery) {
                cm.setDisabled('sscInsertValue', true);
                cm.setDisabled('sscInsertPageBreak', true);
                cm.setDisabled('sscConditions', true);
                cm.setDisabled('sscAddTable', true);
                return;
            }

            if (!tinymce.dom.DOMUtils.prototype.inConditions) { // Добавление дополнителных методов в class: tinymce.dom.DOMUtils
                // проверка нахождения узла в объекте "Условия"
                tinymce.dom.DOMUtils.prototype.inConditions = function (node) {
                    var parent = tinyMCE.DOM.getParent(node, function (p) {
                        return tinyMCE.DOM.hasClass(p, 'conditions');
                    });
                    return !!parent;
                };

                // проверка нахождения узла в объекте "Список"
                tinymce.dom.DOMUtils.prototype.inList = function (node) {
                    var parent = tinyMCE.DOM.getParent(node, function (p) {
                        return p.nodeName === 'TABLE' && tinyMCE.DOM.hasClass(p, 'sscList');
                    });
                    return !!parent;
                };

                // проверка нахождения узла в объекте "Заголовок Списка"
                tinymce.dom.DOMUtils.prototype.inListTitle = function (node) {
                    var parent = tinyMCE.DOM.getParent(node, function (p) {
                        return p.nodeName === 'TD' && tinyMCE.DOM.hasClass(p, 'sscListTitle');
                    });
                    return !!parent;
                };

                tinymce.dom.DOMUtils.prototype.inListItem = function (node) {
                    var parent = tinyMCE.DOM.getParent(node, function (p) {
                        return p.nodeName === 'TD' && tinyMCE.DOM.hasClass(p, 'sscListItem');
                    });
                    return !!parent;
                };

                // jquery методы
                $.extend({
                    removeDialogs: function () {
                        $(".dialog").remove();
                    },
                    showMessage: function (message) {
                        $('.message').remove();
                        $('html').append("<div class='message'>" + message + "</div>");
                    },
                    getValueInfosFor: function (params) {
                        var _params = {
                            fieldTypes: null, // типы полей для выбора, null все поля
                            searchInLists: true, // поиск полей в списках
                            node: null, // в шаблоне в зависимости от места нахождения элемента определяются доступные поля к этому элементу.
                            isAccessibleField: function (field) { return true; },
                            isValidType: function (fieldType) {
                                return this.fieldTypes == null || $.inArray(fieldType, this.fieldTypes) != -1;
                            }
                        };
                        $.extend(_params, params);
                        if (!!_params.node) {
                            var inListTitle = tinyMCE.DOM.inListTitle(_params.node);
                            var inListItem = tinyMCE.DOM.inListItem(_params.node);
                            var parentList = inListTitle || inListItem ? tinyMCE.DOM.getParent(_params.node, function (p) {
                                return p.nodeName === 'TABLE' && tinyMCE.DOM.hasClass(p, 'sscList');
                            }) : null;
                            var listName = !!parentList && parentList.getAttribute('listName') ? parentList.getAttribute('listName') : '';
                            var groupValues = !!parentList && parentList.getAttribute('groupValues') ? parentList.getAttribute('groupValues').split('/') : [];
                            _params.isAccessibleField = function (field) {
                                if (!field.valueOfList) return true;
                                if (inListTitle && $.inArray(field.fieldName, groupValues) > -1 && field.fieldName.lastIndexOf(listName + '|', 0) == 0) return true;
                                else if (inListItem && field.fieldName.lastIndexOf(listName + '|', 0) == 0) return true;
                                else return false;
                            };
                        }
                        var ret = {};
                        $('#valueInfos .valueInfo').each(function () {
                            var $this = $(this);
                            var field = {
                                fieldType: $this.attr('valueType'),
                                fieldName: $this.attr('valueName'),
                                title: $this.text(),
                                valueOfList: $this.attr('valueName').indexOf('|') != -1
                            };
                            if (field.valueOfList && !_params.searchInLists) return;
                            if (_params.isValidType(field.fieldType) && _params.isAccessibleField(field)) ret[field.fieldName] = field;
                        });
                        return ret;
                    }
                });

                $("html").click(function () {
                    $.removeDialogs();
                });

                $.fn.turnToButton = function (withBorder) {
                    if (withBorder === true) return this.addClass('button2');
                    return this.addClass('button');
                };

                $.fn.turnToOption = function () {
                    return this.addClass('option');
                };

                $.fn.sscOn = function (event, func) {
                    if (event == 'click') return this.click(func);
                    if (event == 'dblclick') return this.dblclick(func);
                };

                $.fn.dialogShower = function (onEvent, content, addStyle, afterShow) {
                    return this.unbind(onEvent).sscOn(onEvent, function (e1) {
                        e1.stopPropagation();
                        $.removeDialogs();
                        var str = (typeof content == 'function') ? content.call(this) : content;
                        if (!str) return;
                        var sTop = $(window).scrollTop(), sLeft = $(window).scrollLeft();
                        var offset = $(this).offset();
                        var iframeOffset = $("#" + tinyMCE.activeEditor.id + "_tbl .mceIframeContainer").offset();
                        var top = iframeOffset.top + offset.top - sTop + this.offsetHeight;
                        var left = iframeOffset.left + offset.left - sLeft;
                        var dialog = $('body').append("<div class='dialog' style='top:" + top + "px; left:" + left + "px; " + addStyle + "'>" + str + "</div>")
                            .find('.dialog:last').click(function (e2) { e2.stopPropagation(); });
                        if (afterShow) afterShow.call(this, dialog);
                        return false;
                    });
                };

                $.fn.fieldValueEvents = function () {
                    return this.dialogShower(
                        'click'
                        , function () {
                            var str = '';
                            var valueInfos = $.getValueInfosFor({node: this});
                            $.each(valueInfos, function (ind, valueInfo) {
                                str += "<div class='valueInfo' valueName='" + valueInfo.fieldName + "'>" + htmlEncode(valueInfo.title) + "</div>";
                            });
                            str += "<div class='deleteFieldValue'>Удалить</div>";
                            return str;
                        }
                        , 'min-width:300px; max-width:400px; max-height:300px;'
                        , function (dialog) {
                            var thisValue = $(this);
                            dialog.find('.deleteFieldValue').turnToOption().click(function () {
                                $.removeDialogs();
                                thisValue.remove();
                            });
                            dialog.find('.valueInfo').turnToOption().click(function () {
                                var $this = $(this);
                                var name = $this.attr('valueName');
                                var title = $this.text();
                                thisValue.attr('name', name).val(title).removeClass('errorField');
                                $.removeDialogs();
                            });
                        });
                };

                $.fn.pageBreakEvents = function () {
                    return this.dialogShower('dblclick', "<div style='text-align:center; font-weight:bold; color:red;'>удалить</div>", '', function ($dialog) {
                        var $pageBreakInput = $(this);
                        $dialog.find('div').turnToOption().click(function () {
                            $pageBreakInput.remove();
                            $.removeDialogs();
                        });
                    });
                };

                $.fn.conditionsEvents = function () {
                    showCoditionsFor(this, 'dblclick', function (values) { // метод getsetter(), объязанности метода - если values==null возврать данных, в другом случае получить данные
                        if (values === undefined) { // возврать данных: serializedCondition, additionalContent(дополнительный контент(в данном случае "Вид") для диалога)
                            var selected1 = this.nodeName == 'SPAN' ? 'selected' : '';
                            var selected2 = this.nodeName == 'DIV' ? 'selected' : '';
                            var addContent = "<b>Вид:</b> <select><option value='SPAN' " + selected1 + ">Текст</option><option value='DIV' " + selected2 + ">Блок</option></select>";
                            return { serializedConditions: $(this).attr('conditions'), additionalContent: addContent };
                        }
                        // получение данных с диалога: serializedCondition, additionalContent
                        var $this = $(this);
                        var newNodeName = values.additionalContent.find('select').val(); // получение значения справочника "Вид"
                        if (newNodeName === 'SPAN' && $(this).find('p, table, ul, br, div').length > 0) newNodeName = 'DIV';
                        if (this.nodeName != newNodeName) { // измена типа
                            var $newElement = $("<" + newNodeName + " class='conditions' conditions='" + values.serializedConditions + "'/>").append($this.contents());
                            tinyMCE.activeEditor.selection.select(this); // перенос курсора на старый элемент(this)
                            $this.remove(); // удалить старый элемент
                            if (newNodeName == 'DIV') {
                                tinyMCE.activeEditor.forceBlocks.insertPara();
                                var parentBlock = tinyMCE.activeEditor.forceBlocks.getParentBlock(tinyMCE.activeEditor.selection.getStart());
                                if (parentBlock) parentBlock.parentNode.insertBefore($newElement[0], parentBlock);
                                else tinyMCE.activeEditor.selection.getRng().insertNode($newElement[0]);
                            } else {
                                var $newBlock = $('<p/>'); // так как блок был удален, нужно создать новый блок
                                $newBlock.append($newElement);
                                tinyMCE.activeEditor.selection.getRng().insertNode($newBlock[0]);
                            }
                            $newElement.conditionsEvents();
                        } else $(this).attr('conditions', values.serializedConditions); // установка serializedConditions
                    });
                    return this.hover(function () {
                        var conditions = $(this).attr('conditions');
                        var message = getConditionsInfosFor(conditions);
                        if (message != '') $.showMessage(message);
                    }, function () {
                        $('.message').remove();
                    });
                };
                $.fn.sscListEvents = function () {
                    return this.dialogShower(
                        'dblclick'
                    // метод для получения контента диалога
                        , function () {
                            var $list = $(this); // table.sscList
                            var selectedListName = this.getAttribute('listName');
                            var str = "<div class='listProperties'><b>Список:</b><br/><select>";
                            str += "<option value=''>--</option>";
                            var listsCount = $('#hiddenInfos .listInfo').each(function () {
                                var listName = this.getAttribute('listName');
                                var selected = listName == selectedListName ? "selected" : "";
                                str += "<option value='" + listName + "' " + selected + ">" + htmlEncode($(this).find('.listTitle').text()) + "</option>";
                            }).length;
                            str += "</select><br/><b>Сгруппировать по следующим полям:</b><div class='groupableValues'></div>";
                            str += "<div class='buttons'><b class='okButton'>ОК</b> <b class='removeButton'>Удалить список</b> <b class='cancelButton'>Отмена</b></div></div>";
                            return listsCount < 1 ? null : str;
                        }
                    // стили диалога
                        , 'max-height:300px; min-width:300px; overflow-x:hidden; background:#D9E8FB; padding:5px;'
                    // после создания диалога
                        , function ($dialog) { // $dialog - контент диалога
                            var list = this;
                            $dialog.find('select').change(function () {
                                var selectedListName = $(this).val();
                                var groupableValues = $dialog.find('.groupableValues').empty();
                                if (!selectedListName) return;
                                var checkedValues = list.getAttribute('groupValues') ? list.getAttribute('groupValues').split('/') : [];
                                var str = '';
                                $("#hiddenInfos .listInfo[listName='" + selectedListName + "'] .groupableValue").each(function () {
                                    var valueName = this.getAttribute('valueName');
                                    var checked = $.inArray(valueName, checkedValues) > -1 ? "checked" : "";
                                    str += "<input type='checkbox' " + checked + " value='" + valueName + "'/> " + htmlEncode(this.innerText) + "<br/>";
                                });
                                groupableValues.html(str);
                            }).change();
                            // при нажатии "ОК"
                            $dialog.find('.okButton').turnToButton().click(function () {
                                var listName = $dialog.find('select').val();
                                if (!listName) {
                                    list.removeAttribute('listName');
                                    list.removeAttribute('groupValues');
                                    $.removeDialogs();
                                    return;
                                }
                                var checkedValues = [];
                                $dialog.find(':checked').each(function () {
                                    checkedValues.push(this.getAttribute('value'));
                                });
                                list.setAttribute('listName', listName);
                                list.setAttribute('groupValues', joinArray(checkedValues, '/')); // joinArray() в ConditionDetails.aspx
                                $.removeDialogs();
                            });
                            // удаление списка
                            $dialog.find('.removeButton').turnToButton().click(function () {
                                if (confirm('Хотите удалить?')) list.parentNode.removeChild(list);
                            });
                            // при нажатии "Отмена"
                            $dialog.find('.cancelButton').turnToButton().click(function () {
                                $.removeDialogs();
                            });
                        });
                };
            }
            ed.settings.sscModeOn = true; // используется в файлах - ~/Content/tiny_mce/tiny_mce.js, ~/Content/tiny_mce/plugins/table/editor_plugin.js

            // добавление команды "Добавить поле"
            ed.addCommand('sscInsertValue', function () {
                var id = 'fieldValue' + new Date().getTime();
                tinyMCE.execCommand('mceInsertContent', false, "<input type='button' id='" + id + "' name='notSelected' class='fieldValue errorField' value='Не выбран'/>");
                $(tinyMCE.activeEditor.getDoc()).find("#" + id).fieldValueEvents();
            });

            ed.addCommand('sscInsertPageBreak', function () {
                var id = 'pageBreak' + new Date().getTime();
                tinyMCE.execCommand('mceInsertContent', false, "<input type='button' id='" + id + "' class='pageBreakInput' value='Разрыв страниц'/>");
                $(tinyMCE.activeEditor.getDoc()).find("#" + id).pageBreakEvents();
            });

            ed.addCommand('sscInsertConditions', function () {
                var id = 'conditions' + new Date().getTime();
                if (ed.selection.isCollapsed())
                    tinyMCE.execCommand('mceInsertContent', false, "<span id='" + id + "' class='conditions' conditions=''>условия</span>");
                else {
                    var selection = tinyMCE.activeEditor.selection.getContent();
                    tinyMCE.activeEditor.selection.setContent("<div id='" + id + "' class='conditions' conditions=''>" + selection + '</div>');
                    $(tinyMCE.activeEditor.getDoc()).find("#" + id + " .fieldValue").fieldValueEvents();
                }
                $(tinyMCE.activeEditor.getDoc()).find("#" + id).conditionsEvents();
            });

            ed.addCommand('sscInsertTable', function () {
                var id = 'sscList' + new Date().getTime();
                tinyMCE.execCommand('mceInsertContent', false, "<table id='" + id + "' class='sscList'><tr><td class='sscListTitle'>Заголовок</td></tr><tr><td class='sscListItem'>Данные каждого цикла</td></tr></table>");
                $(tinyMCE.activeEditor.getDoc()).find("#" + id).sscListEvents();
            });

            // Регистрация кнопок
            ed.addButton('sscInsertValue', {
                title: 'Добавить поле',
                cmd: 'sscInsertValue',
                image: '/content/tiny_mce/plugins/ssc/add.png'
            });
            ed.addButton('sscInsertPageBreak', {
                title: 'Добавить разрыв страницы',
                cmd: 'sscInsertPageBreak',
                image: '/content/tiny_mce/plugins/ssc/page_break.gif'
            });
            ed.addButton('sscConditions', {
                title: 'Добавить условия',
                cmd: 'sscInsertConditions',
                image: '/content/tiny_mce/plugins/ssc/condition.png'
            });
            ed.addButton('sscInsertTable', {
                title: 'Добавить цикл',
                cmd: 'sscInsertTable',
                image: '/content/tiny_mce/plugins/ssc/list.png'
            });

            // при перемещении курсора выполняется, проверка место нахождения курсора и активация/деактивация кнопок "sscConditions" и "sscAddTable"
            ed.onNodeChange.add(function (ed, cm, n) {
                n = ed.selection.getStart();
                var inConditions = ed.dom.inConditions(n);
                var inList = ed.dom.inList(n);
                cm.setDisabled('sscConditions', inConditions || inList);
                cm.setDisabled('sscInsertTable', inList);
            });

            ed.onSetContent.add(function (ed, o) {
                $(ed.getDoc()).find(".fieldValue").fieldValueEvents();
                $(ed.getDoc()).find(".conditions").conditionsEvents();
                $(ed.getDoc()).find(".sscList").sscListEvents();
            });

            ed.onClick.add(function (ed, e) {
                $.removeDialogs();
            });
        },

        getInfo: function () {
            return {
                longname: 'Ssc Plugin',
                author: 'Улугбек',
                authorurl: 'none',
                infourl: 'none',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('ssc', tinymce.plugins.ssc);

})();

