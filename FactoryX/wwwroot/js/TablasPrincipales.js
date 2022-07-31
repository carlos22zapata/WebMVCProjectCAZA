//Aquí abro el modal de los activos
function abrirmodalHorariosActivos(selectedItems) {
    var ca = selectedItems.row.data;
    document.getElementById('Lab_Cod_activo').innerHTML = ca.Cod_activo;
    document.getElementById('Lab_Des_activo').innerHTML = ca.Des_activo;

    //document.getElementById('modalHorariosActivos').style.display = "inline";
    $("#modalHorariosActivos").modal();    
    $('#Dia').val('1');

    try {
        asignaCod_activo(ca.Cod_activo);
        $('#dg_TurnosActivos').dxDataGrid("instance").refresh();
    } catch(error){
        console.error(error);
    }
    

    //Busco los valores de los días Lunes
    //$('#Lu1d').dxDateBox({ value: '1900/01/01 00:00' });

    var cod_activo = document.getElementById('Lab_Cod_activo').innerHTML;
    asignaHora(1, cod_activo);
}


//Aquí abro el modal de los grupos
function abrirmodalHorariosGrupos(selectedItems) {
    var ca = selectedItems.row.data;
    //document.getElementById('Lab_Cod_activo').innerHTML = ca.Cod_activo;
    

    //document.getElementById('modalHorariosActivos').style.display = "inline";
    $("#modalHorariosGrupos").modal();
    $('#Dia').val('1');

    try {
        document.getElementById('Lab_Des_grupo').innerHTML = ca.Des_grupo;
    } catch (error) {
        console.error(error);
    }


    //Busco los valores de los días Lunes
    //$('#Lu1d').dxDateBox({ value: '1900/01/01 00:00' });

    var cod_activo = document.getElementById('Lab_Cod_activo').innerHTML;
    asignaHora(1, cod_activo);
}

function selectValor() {
    asignaHora(document.getElementById('Dia').value, document.getElementById('Lab_Cod_activo').innerHTML);
}

function asignaHora(dia, cod_activo) {
    $.ajax({
        type: "GET",
        url: FactoryX.Urls.AsignaHora,
        data: { dia, cod_activo},
        success: (response) => {

            var hora1 = response.length === 0 ? "00:00" : response[0].hi1;
            var hora2 = response.length === 0 ? "00:00" : response[0].hf1;

            $('#Lu1d').dxDateBox({ value: '1900/01/01 ' + hora1 });
            $('#Lu1h').dxDateBox({ value: '1900/01/01 ' + hora2 });
        },
        error: (response) => {
            alert("No se pudo leer la hora establecida para el activo: " + cod_activo + ", el día: " + dia);
        }
    });
}

function asignaCod_activo(cod_activo) {
    $.ajax({
        type: "GET",
        url: FactoryX.Urls.AsignaCod_activo,
        data: { cod_activo },
        success: (response) => {
            console.log('código asignado correctamenete.');
        },
        error: (response) => {
            alert("No se pudo asignar el código del activo.");
        }
    });
}


function ver_gruposProductos() {
    var t = document.getElementById("grid_grupo");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#grid_grupo').show(1000);
    }
    else {
        //t.style.display = "none";
        $('#grid_grupo').hide(1000);
    }
}

function ver_gruposActivos() {
    var t = document.getElementById("grid_grupoActivos");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#grid_grupoActivos').show(1000);
    }
    else {
        //t.style.display = "none";
        $('#grid_grupoActivos').hide(1000);
    }
}

function asignaId() { //No lleva el idEmpresa por que es asignado directamente de la vista
    $.ajax({
        type: "GET",
        url: FactoryX.Urls.AsignaEmpresa,
        data: {  },
        success: (response) => {
            console.log('id de la empresa asignado corectamenete.');
        },
        error: (response) => {
            alert("No se pudo asignar el id de la empresa.");            
        }
    });
}

function actuclizaActivos(actionName) {
    //$("#dg_Activos").dxDataGrid("instance"); //.getDataSource().reload(); 
}

function guardaHorasActivos() {

    var dia = document.getElementById('Dia').value;
    var cod_activo = document.getElementById('Lab_Cod_activo').innerHTML;

    var hi1x = $('#Lu1d').dxDateBox("instance");
    var hf1x = $('#Lu1h').dxDateBox("instance");

    var hi1 = hi1x.option('value');//.substring(11,19);    
    var hf1 = hf1x.option('value');//.substring(11, 19);

    if (hi1 > hf1) {
        Swal.fire({
            title: "La hora desde no puede ser mayor a la hora hasta  ",
            text: "Vuelva a seleccionar sus registros",
            icon: "error",
            confirmButtonText: 'Cerrar'
        });
    }
    else {
        $.ajax({
            type: "GET",
            url: FactoryX.Urls.GuardaHorasActivos,
            data: { dia, cod_activo, hi1, hf1 },
            success: function (Result) {
                Swal.fire({
                    title: "Registro guardado ",
                    text: "Registro guardado con exito",
                    icon: "success",
                    confirmButtonText: 'Cerrar'
                });
            },
            error: (Result) => {
                alert("No se pudo entrar al controlador para signar las horas correspondientes");
            }
        });
    }    
}

$("#dg_Activos2").dxDataGrid({
    onRowInserting: function () {
        asignaId();
        $("#dg_Activos2").dxDataGrid("instance").refresh();
    },
    onRowUpdating: function () {
        asignaId();
    },
    onRowRemoved: function () {
        asignaId();
    }
});

$("#dg_grupoA").dxDataGrid({
    onRowInserting: function () {
        asignaId();
        $("#dg_Activos2").dxDataGrid("instance").refresh();
    },
    onRowUpdating: function () {
        asignaId();
    },
    onRowRemoved: function () {
        asignaId();
    }
});

$("#dg_productos").dxDataGrid({
    onRowInserting: function () {
        asignaId();
        $("#dg_productos").dxDataGrid("instance").refresh();
    },
    onRowUpdating: function () {
        asignaId();
    },
    onRowRemoved: function () {
        asignaId();
    }
});

$("#dg_grupo_productos").dxDataGrid({
    onRowInserting: function () {
        asignaId();
        $("#dg_productos").dxDataGrid("instance").refresh();
    },
    onRowUpdating: function () {
        asignaId();
    },
    onRowRemoved: function () {
        asignaId();
    }
});

$("#dg_turnos").dxDataGrid({
    onRowInserting: function () {
        asignaId();
    },
    onRowUpdating: function () {
        asignaId();
    },
    onRowRemoved: function () {
        asignaId();
    }
});

$("#dg_tipo_tia").dxDataGrid({
    onRowInserting: function () {
        location.reload(true);
    },
    onRowUpdating: function () {
        location.reload(true);
    },
    onRowRemoved: function () {
        location.reload(true);
    }
});