$(document).ready(function () {
    $("#filter").on('change keyup paste', function (event) {
        $('#tabela_kandidat tr').each(function () {
            if ($(this).attr("id").indexOf($("#filter").val()) == -1) $(this).hide();
            else $(this).show();
        });
    });
});
