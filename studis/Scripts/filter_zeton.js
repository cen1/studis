$(document).ready(function () {
    $("#filter").on('change keyup paste', function (event) {
        console.log("asd");
        $('#izpis_zetonov tr').each(function () {
            if ($(this).attr("id").indexOf($("#filter").val()) == -1) $(this).hide();
            else $(this).show();
        });
    });
});
