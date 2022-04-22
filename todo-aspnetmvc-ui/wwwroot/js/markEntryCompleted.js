$(document).ready(function () {
    $('.todo').change(function () {
        var self = $(this);
        var id = self.attr('id');
        var value = self.prop('checked');
        $.ajax({
            method: "POST",
            url: '/TodoEntry/MarkCompleted',
            data: {
                id: id,
                value: value
            }
        });
    });
});