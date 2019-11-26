//tinyMCEPopup.requireLangPack();

var uploader = {
	init : function() {
		
	},

	insert : function(data) {
	    // Insert the contents from the input into the document
		tinyMCEPopup.editor.execCommand('mceInsertContent', false, data);
		tinyMCEPopup.close();
	}
};

tinyMCEPopup.onInit.add(uploader.init, uploader);
