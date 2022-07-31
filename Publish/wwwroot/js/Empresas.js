//function ListaEmpresas() {
       
//    $.ajax({
//        type: "GET",
//        url: Seqtor.Urls.ListaEmpresas,
//        data: {},
//        success: function (Result) {
//            if (Result.length !== 0) {
//                $.each(Result, function (key, value) {
//                    var tr = $("<option />");
//                    $.each(value, function (k, v) {
//                        if (k === "Des_institucion") {
//                            tr.append(v);
//                            //$("#products-simple").options.append(v);
//                        }
//                    });
//                    $("#products-simple").options("datasource",tr);
//                });
//            }

//        }
//    });
//} 


//setTimeout(function () {
//    viewModel.settings.dataSource(new DevExpress.data.DataSource({
//        store: getItems(),
//        paginate: true
//    }));
//}, 1000);



function EmpresaSeleccionada() {
    sel = document.getElementById('SelecEmpresa').value;
    alert(sel);
}

function EmrpesaSeleccionada() {

    var emp = document.getElementById('SelecEmpresa').value;

    if (emp === "") {
        Swal.fire({
            title: "Seleccione una empresa",
            text: "No se seleccionó una empresa valida",
            icon: "warning",
            confirmButtonText: 'Cerrar'
        });
    } else {
        window.location = "/Home/VistaPrincipal?idEmpresa=" + emp;
    }    
}

function RedireccionaInicio() {
    window.location = "/Identity/Account/Login";
}

function ActualizaIp() {
    $.ajax({
        type: "POST",
        url: FactoryX.Urls.ActualizaIp,
        data: {  },
        success: (response) => {
            location.reload(true);
            console.log('id del plan asignado correctamenete.');            
        },
        error: (response) => {
            location.reload(true);
            console.log("No se pudo asignar el registro.");
        }
    });        
}

$(document).ready(function () {
    //const connection = new signalR.HubConnectionBuilder().withUrl("/Hubs/usuariosConectados").build();

    //alert(connection);

    //connection.on("Prueba", (user) => {
    //    alert("Conexión.....");
    //})
})