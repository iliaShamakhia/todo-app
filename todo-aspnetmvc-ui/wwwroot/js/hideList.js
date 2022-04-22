$(document).ready(function () {
    $('.hide').click(function () {
        var list = "#list" + this.id
        $(list).attr("style", "display:none !important");
    });
});