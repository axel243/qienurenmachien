document.addEventListener('DOMContentLoaded', function () {

	jQuery(function ($) {
		$('.sortableTable').footable({
			"sorting": {
				"enabled": true
			}
		});
	});
}, false);