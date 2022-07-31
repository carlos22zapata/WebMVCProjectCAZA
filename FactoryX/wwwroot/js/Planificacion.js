let registrosSeleccionadosGrid = [];
var noPermitidos = 0;

$(document).ready(function () {
    //document.getElementById('cancelarOrden').style.display = "none";
    //document.getElementById('nuevaOrden').style.display = "none";
    //document.getElementById('eliminarOrden').style.display = "none";
});

var cerrado = false;

function reset1() {
    document.getElementById("ie1").value = "";
}

$('#ie1').change(function (e) {

    try {
        var reader = new FileReader();
        reader.readAsArrayBuffer(e.target.files[0]);

        if (reader === null) {
            alert('Error 1');
        }

        reader.onload = function (e) {
            var data = new Uint8Array(reader.result);

            var wb = XLSX.read(data, { type: 'array', cellDates: true, dateNF: 'mm/dd/yyyy;@' });

            var first_sheet_name = wb.SheetNames[0];
            var worksheet = wb.Sheets[first_sheet_name];
            var resultado = XLSX.utils.sheet_to_json(worksheet);

            //var Cod_plan = "";
            //var Plan_anterior = "";
            //var Cod_centro = "";
            //var Estado = "";
            //var Cod_producto = "";
            //var Cantidad = 0;
            //var FechaDespacho = "01/01/2020";
            //var Unidad = "";

            console.log(resultado);

            var result1 = JSON.stringify(resultado);                                  

            var result0 = [
                { id: 1, color: 'yellow' },
                { id: 2, color: 'blue' },
                { id: 3, color: 'red' }
            ]; 

            //var result1 = JSON.stringify(result0);   

            $.ajax({
                    type: "POST",
                    url: Factory.Urls.InsertPedidosT,
                    data: { resultado }, //, Estado, Cod_producto, Cantidad, FechaDespacho, Unidad },
                    dataType: 'text',
                    success: (response) => {
                            console.log('id del plan asignado correctamenete.');
                            
                        },
                        error: (response) => {
                            alert("No se pudo asignar el registro.");
                        }
                    });                      

            $('#dg_PedidosGeneral').dxDataGrid("instance").refresh();
            $('#ie1').modal('hide');
            location.reload(true);
        }
    }
    catch {
        alert('Error: no hubo comunicación con el controlador');
    }

   //location.reload(true);

    //document.getElementById("ie1").value = "";    
    
});

function insertaRenglonPedido(Cod_plan, Cod_centro, Cod_producto, Cantidad, FechaDespacho, Unidad) {
        
    $.ajax({
        type: "GET",
        url: Factory.Urls.InsertPedidosT_Reng,
        data: { Cod_plan, Cod_centro, Cod_producto, Cantidad, FechaDespacho, Unidad },
        success: (response) => {
            console.log('id del plan asignado correctamenete.');
            //$('#dg_PedidosGeneral').dxDataGrid("instance").refresh();
        },
        error: (response) => {
            alert("No se pudo asignar el registro.");
        }
    });
      
}

function asignaCod_plan(cod_plan) {
    $.ajax({
        type: "GET",
        url: Factory.Urls.AsignaCod_plan,
        data: { cod_plan },
        success: (response) => {
            console.log('id del plan asignado correctamenete.');
        },
        error: (response) => {
            alert("No se pudo asignar el id del plan.");
        }
    });
}

function asignaId() {
    //asignaCod_plan(document.getElementById('CB_sel_plan').value);

    $.ajax({
        type: "GET",
        url: Factory.Urls.AsignaEmpresa,
        data: {},
        success: (response) => {
            console.log('id de la empresa asignado correctamenete.');
        },
        error: (response) => {
            alert("No se pudo asignar el id de la empresa.");
        }
    });
}

function actuclizaActivos(actionName) {
    //$("#dg_Activos").dxDataGrid("instance"); //.getDataSource().reload(); 
}

function ver_plantas() {
    var t = document.getElementById("grid_centro");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#grid_centro').show(1000);
    }
    else {
        //t.style.display = "none";
        $('#grid_centro').hide(1000);
    }
}

$("#dg_centro").dxDataGrid({
    onRowInserting: function (info) {
        $("#dg_Pedidos").dxDataGrid("instance").refresh();
    }
});

$("#dg_Pedidos").dxDataGrid({
    onRowInserting: function (info) {
        $("#dg_Pedidos").dxDataGrid("instance").refresh();
    },

    onEditorPreparing(e) {
        if (e.parentType === "dataRow" && e.dataField === "Cod_plan") {
            e.setValue(document.getElementById('CB_sel_plan').value);
        }
    }
});

function abrirNuevaOrden() {
    var cod_plan = document.getElementById('txtCod_plan').value;
    var cod_centro = document.getElementById('CB_sel_planta').value;

    if (document.getElementById('agregarOrden').innerHTML.trim() === "Agregar nueva orden") {
        document.getElementById('cancelarOrden').style.display = "inline";
        document.getElementById('nuevaOrden').style.display = "inline";
        document.getElementById('eliminarOrden').style.display = "inline";
        document.getElementById('CB_sel_plan').style.display = "none";
        document.getElementById('agregarOrden').innerHTML = "Guardar orden";
        document.getElementById('txtCod_plan').value = "";
        document.getElementById('CB_sel_planta').value = "";
    }
    else {


        if (cod_plan === "" || cod_plan === undefined) {
            Swal.fire({
                title: "debe colocar un código para el plan",
                text: "Vuelva a intentarlo",
                icon: "warning",
                confirmButtonText: 'Cerrar'
            });
        } else if (cod_centro === "" || cod_centro === undefined) {
            Swal.fire({
                title: "debe seleccionar un código de planta",
                text: "Vuelva a intentarlo",
                icon: "warning",
                confirmButtonText: 'Cerrar'
            });
        } else {
            document.getElementById('cancelarOrden').style.display = "none";
            document.getElementById('nuevaOrden').style.display = "none";
            document.getElementById('eliminarOrden').style.display = "none";
            document.getElementById('CB_sel_plan').style.display = "inline";
            document.getElementById('agregarOrden').innerHTML = "Agregar nueva orden";


            //Aqui va el ajax para guardar el registro
            $.ajax({
                type: "GET",
                url: Factory.Urls.GuardaPlan,
                data: { cod_plan, cod_centro },
                success: (response) => {
                    if (response[0].Code === "Error al guardar") {
                        Swal.fire({
                            title: "El registro que intenta guardar ya existe.",
                            text: "Vuelva a intentarlo",
                            icon: "warning",
                            confirmButtonText: 'Cerrar'
                        });
                    } else {
                        console.log('id de la empresa asignado correctamenete.');
                        location.reload(true);
                    }
                },
                error: (response) => {
                    alert("No se pudo asignar el id de la empresa.");
                }
            });
        }
    }
}

function cancelarNuevaOrden() {
    document.getElementById('cancelarOrden').style.display = "none";
    document.getElementById('nuevaOrden').style.display = "none";
    document.getElementById('eliminarOrden').style.display = "none";
    document.getElementById('CB_sel_plan').style.display = "inline";
    document.getElementById('agregarOrden').innerHTML = "Agregar nueva orden";
}

function eliminarOrden() {

    cod_plan = document.getElementById('txtCod_plan').value;

    if (cod_plan === "" || cod_plan === undefined) {
        Swal.fire({
            title: "debe colocar un codigo de plan valido",
            text: "Vuelva a intentarlo",
            icon: "warning",
            confirmButtonText: 'Cerrar'
        });
    } else {
        $.ajax({
            type: "GET",
            url: Factory.Urls.EliminaPlan,
            data: { cod_plan },
            success: (response) => {
                Swal.fire({
                    title: "Orden de producción eliminada correctamente",
                    text: "Vuelva a intentarlo",
                    icon: "success",
                    confirmButtonText: 'Cerrar'
                });
                location.reload(true);
            },
            error: (response) => {
                Swal.fire({
                    title: "No se pudo eliminar la orden.",
                    text: "Posiblemente sea por que el número de orden no existe o tiene renglones asociados. Verifique el código insertado o revise que haya eliminado todos sus renglones ",
                    icon: "error",
                    confirmButtonText: 'Cerrar'
                });
            }
        });
    }


}

function itemsCheck(selectedItems) {
    var data2 = selectedItems.selectedRowsData;
    registrosSeleccionadosGrid = [];
    noPermitidos = 0;

    if (data2.length > 0) {

        var datos00 = data2.length;

        for (var i = 0; i < datos00; i++) {
            try {
                registrosSeleccionadosGrid.push({ "Cod_plan": data2[i].Cod_plan });

                if (data2[i].Des_pedido !== "Iniciado") {
                    noPermitidos = noPermitidos + 1;
                }
            }
            catch (error) {
                alert("Error: " + error)
            }
            
        }
    }
    else {
        registrosSeleccionadosGrid = [];
    }
}

function selection_changed(selectedItems) {

    itemsCheck(selectedItems);

    var data = selectedItems.selectedRowsData[0];
    var cod_plan = data.Cod_plan;    

    if (data) {
        $("#planSel").text("Sku asociados: " + cod_plan);

        asignaId();
        asignaCod_plan(cod_plan);
        $('#dg_Pedidos').dxDataGrid("instance").refresh();

        $.ajax({
            type: "GET",
            url: Factory.Urls.PlanCerrado, //Revisa si el plan esta cerrado y en caso de que si este cerrado desactiva los comtroles para editar, borrar o agregar nuevo registro
            data: { cod_plan },
            success: (response) => {
                console.log('id de la empresa asignado correctamenete.');

                if (response === true) {
                    $("#dg_Pedidos").dxDataGrid({
                        editing: {
                            //mode: "form",
                            allowUpdating: false,
                            allowDeleting: false,
                            allowAdding: false
                        }
                    });
                }
                else {
                    $("#dg_Pedidos").dxDataGrid({
                        editing: {
                            //mode: "form",
                            allowUpdating: true,
                            allowDeleting: true,
                            allowAdding: true
                        }
                    });
                }


            },
            error: (response) => {
                alert("No se pudo asignar el id de la empresa. Plan cerrado");
            }
        });


        if (cod_plan === "" || cod_plan === undefined) {
            //document.getElementById('comentario1').style.display = "inline";
            document.getElementById('tb_pedidos').style.display = "none";
        } else {
            //document.getElementById('comentario1').style.display = "none";
            document.getElementById('tb_pedidos').style.display = "inline";
        }
    }
}

function abrirModal() {
    //verWarningSku('n');
    $("#modalPlanificacion").modal();

}

function abrirModalFiltro() {
    //verWarningSku('n');
    $("#modalPlanificacionFiltro").modal();

}

function FechaExportacion() {
    var fini_ = document.getElementById("diaDesdeEx").innerHTML;
    fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 17);
    var ffin_ = document.getElementById("diaHastaEx").innerHTML;
    ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 17);

    DescargarExcelFiltro(fini_, ffin_)

}

function DescargarExcelFiltro(fini_, ffin_) {

    //alert('gro');

    $.ajax({
        type: "POST",
        url: Factory.Urls.DescargarExcelFiltro,
        data: { fini_, ffin_ },
        success: function (data) {

            //var response = JSON.parse(data); 

            //console.log(data.result);
            //$.unblockUI();

            //get the file name for download
            //if (data.fileName != "") {
            //    //use window.location.href for redirect to download action for download the file
            //    window.location.href = "@Url.RouteUrl(new { Controller = 'Planificacion', Action = 'Download' }) /? file = " + data.fileName;
            //}

            var file = data.fileName;

           //descarga(file)
        }
    });
}

function descarga(file) {
    $.ajax({
        type: "POST",
        url: Factory.Urls.Download,
        data: { file },
    });
}

$("#btnExportar").click(function () {
    
    $.ajax({
        url: root + '/Sigeri/ExportarExcel',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(array),
        success: function (data) {
            //console.log(data);
        }
    });
});

function cierrePalnes() {

    if (registrosSeleccionadosGrid.length == 0) {
        Swal.fire({
            title: "No hay registros seleccionados",
            text: "Seleccione al menos un registro con estado Iniciado para poder usar esta opción",
            icon: "error",
            confirmButtonText: 'Cerrar'
        });
    }
    else if (noPermitidos > 0) {
        Swal.fire({
            title: "Registros seleccionados no validos",
            text: "Solo se pueden cerrar registros en estado de Iniciado, quite la selección de los que no tengan este estado.",
            icon: "warning",
            confirmButtonText: 'Cerrar'
        });
    }    
    else {
        Swal.fire({
            title: 'Cierre de plan',
            text: 'Esta seguro de cerrar los registros seleccionados?',
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: 'Aceptar',
            cancelButtonText: 'Cancelar',
        }).then((result) => {
            if (result.value === true) {
                $.ajax({
                    url: Factory.Urls.CierrePlanes,
                    type: 'GET',
                    data: { valores: JSON.stringify(registrosSeleccionadosGrid) },
                    contentType: 'application/json',
                    success: function (data) {
                        //console.log(data);
                        //$("#dg_PedidosGeneral").dxDataGrid("instance").refresh();
                        window.location.reload();

                        Swal.fire({
                            title: "Registros actualizados con exito!",
                            text: "Las ordenes de producción han sido cerradas correctamente.",
                            icon: "success",
                            confirmButtonText: 'Aceptar'
                        });
                    }
                });
            }
        });
    }
}

//function selection_changed_select(selectedItems) {
//    var data = selectedItems.selectedRowsData;
//    registrosSeleccionadosGrid = [];

//    if (data.length > 0) {
//        for (var i = 0; i < data[0].Cod_plan.length; i++) {
//            registrosSeleccionadosGrid.push({ "Cod_plan": data[i].Cod_plan });
//        }
//    }
//    else {
//        registrosSeleccionadosGrid = [];  
//    }
          
//}