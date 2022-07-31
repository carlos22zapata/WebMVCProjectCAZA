let eventos = [];
var solapado = true;

//###################### Función para la selección de los activos #################

//Agregar
let activosSel = [];

function selection_changed(selectedItems) {
    activosSel = [];
    activosSel = selectedItems.selectedRowsData;
    //console.log(activosSel);   
}

function arregloActivos() { 
    return activosSel;
}

//Eliminar - Borrar
let activosSelB = [];

function selection_changedB(selectedItems) {
    activosSelB = [];
    activosSelB = selectedItems.selectedRowsData;
    //console.log(activosSel);   
}

function arregloActivosB() {
    return activosSelB;
}

//###################################################################################


//###################### Función para la selección de los turnos #################

//Agregar - insertar
let turnosSel = [];



function selection_changedT(selectedItems) {
    
    var valores = JSON.stringify(selectedItems.selectedRowsData);

    selectedItems.component.byKey(selectedItems.currentSelectedRowKeys[0]).done(regX => {        
        //alert(`Turno seleccionado: ${regX.Key}`);  
        //selectionChangedRaised = false;   

        var key = regX.Cod_turno;
        //regX.deselectRows(key);
        //$("#dg_Turnos2").dxDataGrid.deselectRows(key);  
    });
       

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.ValidaTurnos,
        data: { valores },
        dataType: 'text',
        success: (response) => {

            if (response === 'false') {
                Swal.fire({
                    title: "Revise los datos selecionados",
                    text: "No puede seleccionar turnos que se sobrepongan uno con otro",
                    icon: "error",
                    confirmButtonText: 'Cerrar'
                });

                solapado = false;
            } else {
                turnosSel = [];
                turnosSel = selectedItems.selectedRowsData;
                //console.log(activosSel);

                solapado = true;
            }
            
        },
        error: (response) => {
            console.log("No se pudo asignar el registro.");
        }
    });    
}

function arregloTurnos() {
    return turnosSel;
}

//Borrar - eliminar
let turnosSelBT = [];

function selection_changedBT(selectedItems) {
    turnosSelBT = [];
    turnosSelBT = selectedItems.selectedRowsData;
    //console.log(activosSel);   
}

function arregloTurnosBT() {
    return turnosSelBT;
}

//function selection_click_T(e) {
//    var dataGrid = e.component;
//    var keys = dataGrid.getSelectedRowKeys();

//    if (!selectionChangedRaised) {
        
//        dataGrid.deselectRows(keys);
//    }
//    //selectionChangedRaised = false;   

//    alert("Prueba selección " + keys);
//}

//#################################################################################

function insertaRegistro(parametros) {
     
    var valores = JSON.stringify(parametros);
    $('#btn_guardaActividad').hide();

    if (solapado === false) {
        Swal.fire({
            title: "Revise los datos selecionados",
            text: "No puede seleccionar turnos que se sobrepongan uno con otro",
            icon: "error",
            confirmButtonText: 'Cerrar'
        });
    } else {

        animacionWW('A');

        activaDiv('act');

        $('#ModalInsertaCalendario').modal('toggle');

        $.ajax({
            type: "POST",
            url: FactoryX.Urls.InsertCalendario,
            data: { valores },
            dataType: 'text',
            success: (response) => {
                console.log('id del plan asignado correctamenete.');
                animacionWW('C');
                
                $('#btn_guardaActividad').show();
                //alert("Se guardara la información con los datos seleccionados...***");
                
                if (response === '0') {

                    //$('#dg_calendario').dxDataGrid("instance").refresh();

                    Swal.fire({
                        title: "Datos guardados con exito",
                        text: "Se guardaron los datos con exito...",
                        icon: "success",
                        confirmButtonText: 'Cerrar'
                    }).then((result) => {
                        if (result.value) {
                            activaDiv('des');
                            window.location.reload();
                        };
                    });

                    

                    //$('#calendar').fullCalendar('refetchEvents');
                    
                    
                }
                else {

                    Swal.fire({
                        title: "Se detectaron algunos errores al guardar los registros",
                        text: "Se detecto un error en algunos registros, total de registros que no pudieron guardarse: " + response,
                        icon: "info",
                        confirmButtonText: 'Cerrar'
                    }).then((result) => {
                        if (result.value) {
                            activaDiv('des');
                            window.location.reload();
                        };
                    });
                }

                
            },
            error: (response) => {
                console.log("No se pudo asignar el registro.");
                animacionWW('C');
                $('#btn_guardaActividad').show();

                $('#ModalInsertaCalendario').modal('toggle');

                $('#dg_calendario').dxDataGrid("instance").refresh();

                Swal.fire({
                    title: "Datos guardados con exito",
                    text: "Se guardaron los datos con exito...",
                    icon: "success",
                    confirmButtonText: 'Cerrar'
                });

                $('#dg_calendario').dxDataGrid("instance").refresh();
            }
        });

    }

    //}).done(function (data) {
    //    alert(data);
    //}).fail(function (xhr, textStatus, error) {
    //    alert('Error - ');
    //});
        
}

//function cargaInicial(e) {
//    var grid = e.component;
//    var selectedKeys = grid.getSelectedRowKeys();
//    for (var i = 0; i < selectedKeys.length; i++) {
//        if (grid.getRowIndexByKey(selectedKeys[i]) < 0) {
//            grid.deselectRows([selectedKeys[i]]);
//        }
//    } 
//}

function validaInsercíon(parametros) {

    insertaRegistro(parametros);
}

function animacionWW(valor) {
    //var t = document.getElementById("clac1");
    if (valor === 'A') {
        $('#clac1').show();
    }
    else {
        $('#clac1').hide();
    }
}

function borraLote() {    

    if ($('#btn_borraActividadLL').text() === 'Mostrar eventos por lote') {
        $('#btn_borraActividadLL').text('Ocultar eventos por lote');
        document.getElementById('borrarDetalles').style.display = 'inline';
        $('#borraUno').hide();
    }
    else {
        $('#btn_borraActividadLL').text('Mostrar eventos por lote');
        $('#borrarDetalles').hide();
        $('#borraUno').show();
    }    
}

function eliminaRegistrosLote(parametros) {

    var valores = JSON.stringify(parametros);

    activaDiv('act');

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.DeleteCalendarioLote,
        data: { valores },
        dataType: 'text',
        success: (response) => {
            console.log('id del plan asignado correctamenete.');
            animacionWW('C');
            $('#btn_guardaActividad').show();

            $('#dg_calendario').dxDataGrid("instance").refresh();

            Swal.fire({
                title: "Datos borrados con exito",
                text: "Se borraron los datos con exito...",
                icon: "success",
                confirmButtonText: 'Cerrar'
            }).then((result) => {
                if (result.value) {
                    activaDiv('des');
                    window.location.reload();
                };
            });

            //activaDiv('des');
            //$('#dg_calendario').dxDataGrid("instance").refresh();
            //window.location.reload();
        },
        error: (response) => {
            console.log("No se pudo asignar el registro.");
            animacionWW('C');
            $('#btn_guardaActividad').show();

            $('#dg_calendario').dxDataGrid("instance").refresh();

            Swal.fire({
                title: "Datos borrados con exito",
                text: "Se borraron los datos con exito...",
                icon: "success",
                confirmButtonText: 'Cerrar'
            }).then((result) => {
                if (result.value) {
                    activaDiv('des');
                    window.location.reload();
                };
            });

            //activaDiv('des');
            //$('#dg_calendario').dxDataGrid("instance").refresh();
        }
    });   
}

function abreFormularios(v) {

    var hoy0 = Date.now();
    var hoy1 = moment(Date.now()).format('YYYY/MM/DD');

    if (v === 1) {

        $('#fechaIni').dxDateBox('instance').option({ value: hoy1 });
        $('#fechaFin').dxDateBox('instance').option({ value: hoy1 });

        $('#chLu').each(function () { this.checked = false; });
        $('#chMa').each(function () { this.checked = false; });
        $('#chMi').each(function () { this.checked = false; });
        $('#chJu').each(function () { this.checked = false; });
        $('#chVi').each(function () { this.checked = false; });
        $('#chSa').each(function () { this.checked = false; });
        $('#chDo').each(function () { this.checked = false; });

        $('#ModalInsertaCalendario').modal();
    } else {

        $('#btn_borraActividadLL').text('Mostrar eventos por lote').hide();
        $('#borrarDetalles').show();
        $('#borraUno').hide();

        $('#chLu_').each(function () { this.checked = false; });
        $('#chMa_').each(function () { this.checked = false; });
        $('#chMi_').each(function () { this.checked = false; });
        $('#chJu_').each(function () { this.checked = false; });
        $('#chVi_').each(function () { this.checked = false; });
        $('#chSa_').each(function () { this.checked = false; });
        $('#chDo_').each(function () { this.checked = false; });

        $('#fechaIniB').dxDateBox('instance').option({ value: hoy1 });
        $('#fechaFinB').dxDateBox('instance').option({ value: hoy1 });

        $('#ModalDetalle').modal();
    }
}

function muestraCalendario() {

    //document.getElementById('tablaCalendar').style.display = 'none';
    //document.getElementById('calendar').style.display = 'inLine';

    //$('#tablaCalendar').hide();
    $('#calendar0').show();
}

function activaDiv(a) {

    if (a === 'act') {
        console.log('activado bloqueo');
        document.getElementById('div_bloqueo').style.display = 'inline';
    }
    else {
        console.log('desactivado bloqueo');
        document.getElementById('div_bloqueo').style.display = 'none';
    }
    
    
}