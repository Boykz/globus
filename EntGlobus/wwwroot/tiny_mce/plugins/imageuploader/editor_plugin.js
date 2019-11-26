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
    tinymce.create('tinymce.plugins.imageuploader', {
        init: function (ed, url) {
            // Register commands
            ed.addCommand('mceImageUploaderPlugin', function () {
                // Internal image object like a flash placeholder
                if (ed.dom.getAttrib(ed.selection.getNode(), 'class').indexOf('mceItem') != -1)
                    return;
                ed.windowManager.open({
                    file: ed.settings.image_uploader_url,
                    width: 800,
                    height: 600,
                    inline: 1
                }, {
                    plugin_url: url
                });
            });

            // Register buttons
            ed.addButton('imageUpload', {
                title: 'Загрузка рисунка',
                cmd: 'mceImageUploaderPlugin',
                image: '/Content/tiny_mce/plugins/imageuploader/upload.png'
            });
        },

        getInfo: function () {
            return {
                longname: 'Image uploader plugin',
                author: 'Ulugbek',
                authorurl: 'http://tinymce.moxiecode.com',
                infourl: 'http://wiki.moxiecode.com/index.php/TinyMCE:Plugins/advimage',
                version: tinymce.majorVersion + "." + tinymce.minorVersion
            };
        }
    });

    // Register plugin
    tinymce.PluginManager.add('imageuploader', tinymce.plugins.imageuploader);

})();

