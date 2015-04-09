$(document).ready(function () {
    $('#SearchString').keyup(function (e) {
        //var url = "@(Url.Action('StudentSearchPartial', 'Student'))";
        var url = "/Student/StudentSearchPartial";
        var data = $('#SearchString').val();

        $.ajax({
            url: url,
            type: "POST",
            data: { 'searchString': data },
            success: function (resp) {
                $('#searchDiv').html(resp);
                $("#searchDiv  p:nth-child(1)").remove();
            },
            error: function (resp) {
                alert("err with ajax post");
            }
        });
    });
    $('#SearchString1').keyup(function (e) {
        var url = "/Student/StudentSearchPDFPartial";
        var data = $('#SearchString1').val();

        $.ajax({
            url: url,
            type: "POST",
            data: { 'searchString1': data },
            success: function (resp) {
                $('#searchDiv').html(resp);
                $("#searchDiv  p:nth-child(1)").remove();
            },
            error: function (resp) {
                alert("err with ajax post");
            }
        });
    });
});