$(document).ready(function () {
    var hoy = new Date();
    var dd = hoy.getDate();
    var mm = hoy.getMonth() + 1;
    var yyyy = hoy.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }

    if (mm < 10) {
        mm = '0' + mm;
    } 

    var actual = yyyy + "-"+ mm + "-" + dd; //dd + "/" + mm + "/" + yyyy;

    document.getElementById('fini').value = actual;
    document.getElementById('ffin').value = actual;
});

//function asignaId() {
//    $.ajax({
//        type: "GET",
//        url: FactoryX.Urls.AsignaEmpresa,
//        data: {},
//        success: (response) => {
//            console.log('id de la empresa asignado corectamenete.');
//        },
//        error: (response) => {
//            alert("No se pudo asignar el id de la empresa.");
//        }
//    });
//}