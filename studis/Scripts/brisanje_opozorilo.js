$(document).ready(function () {
    $("#izbrisi").click(function () {
        console.log("klik");
        var url = "/IzpitniRok/obstajajoPrijave/" + $("#id option:selected").val();
        console.log("URL: " + url);
        alert("asd");
        $.get(url, function (response) {
            console.log("noter");
            var obstajajoPrijave = $.parseJSON(response);
            console.log(obstajajoPrijave);
            if (obstajajoPrijave) confirm("Izpini rok že vsebuje prijave");
        });
        alert("asd");
    });
});
