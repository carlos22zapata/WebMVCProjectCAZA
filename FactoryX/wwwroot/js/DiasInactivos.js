
$(document).ready(function () {
    fa = new Date();

    var fecha = (fa.getFullYear() + "-" +
        (((fa.getMonth() + 1)) < 10 ? "0" + (fa.getMonth() + 1) : (fa.getMonth() + 1)) + "-" +
        (fa.getDate() < 10 ? "0" + fa.getDate() : fa.getDate()));

    document.getElementById('fini').value = fecha;
    document.getElementById('ffin').value = fecha;
});

function validaDias(accion) {
    var Lunes = document.getElementById('chLu').checked;
    var Martes = document.getElementById('chMa').checked;
    var Miercoles = document.getElementById('chMi').checked;
    var Jueves = document.getElementById('chJu').checked;
    var Viernes = document.getElementById('chVi').checked;
    var Sabado = document.getElementById('chSa').checked;
    var Domingo = document.getElementById('chDo').checked;
    
    var fini_ = new Date($('#fini').val());
    var ffin_ = new Date($('#ffin').val());

    if (Lunes === false && Martes === false && Miercoles === false && Jueves === false && Viernes === false && Sabado === false && Domingo === false) {
        Swal.fire({
            title: 'Seleccione al menos un día',
            html: 'Debe seleccionar un día para procesar los registros',
            icon: 'info'
        });
    } else if (document.getElementById('CB_sel_tipo').value === "") {
        Swal.fire({
            title: 'Seleccione un tipo',
            html: 'Seleccione un tipo y escriba un motivo de la inactividad',
            icon: 'info'
        });
    } else {
        Swal.fire({
            title: 'Confirme su solicitud',
            html: 'Esta opción tardara un tiempo, de acuerdo a la <b> cantidad de registros a insertar.</b>' + ' Se insertarán tandos datos como esten seleccionados, esta seguro que desea continuar?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            cancelButtonText: "Cancelar",
            confirmButtonText: 'Continuar'
        }).then((result) => {
            if (result.value) {
                var fini = document.getElementById('fini').value.split('-');
                var ffin = document.getElementById('ffin').value.split('-');

                var fini0 = Date.UTC(fini[0], fini[1] - 1, fini[2]);
                var ffin0 = Date.UTC(ffin[0], ffin[1] - 1, ffin[2]);

                var dif = ffin0 - fini0;
                var dias = Math.floor(dif / (1000 * 60 * 60 * 24)) + 1;

                var dia, mes, ano, mesf, diaf, anof;

                for (var i = 1; i <= dias; i++) {
                    fini_.setDate(fini_.getDate() + 1);
                    ffin_.setDate(ffin_.getDate() + 1);

                    dia = fini_.getDate() < 10 ? "0" + fini_.getDate().toString() : fini_.getDate().toString();
                    mes = (fini_.getMonth() + 1) < 10 ? "0" + (fini_.getMonth() + 1).toString() : (fini_.getMonth() + 1).toString();
                    ano = fini_.getFullYear().toString();

                    diaf = ffin_.getDate() < 10 ? "0" + ffin_.getDate().toString() : ffin_.getDate().toString();
                    mesf = (ffin_.getMonth() + 1) < 10 ? "0" + (ffin_.getMonth() + 1).toString() : (ffin_.getMonth() + 1).toString();
                    anof = ffin_.getFullYear().toString();

                    var hora1  = $('#hora1').dxDateBox('instance').option('value').getHours()  + ':' + $('#hora1').dxDateBox('instance').option('value').getMinutes()  + ':' + $('#hora1').dxDateBox('instance').option('value').getSeconds();
                    var hora11 = $('#hora11').dxDateBox('instance').option('value').getHours() + ':' + $('#hora11').dxDateBox('instance').option('value').getMinutes() + ':' + $('#hora11').dxDateBox('instance').option('value').getSeconds();
                    var hora2  = $('#hora2').dxDateBox('instance').option('value').getHours()  + ':' + $('#hora2').dxDateBox('instance').option('value').getMinutes()  + ':' + $('#hora2').dxDateBox('instance').option('value').getSeconds();

                    var tipo = document.getElementById('CB_sel_tipo').value;
                    var activo = document.getElementById('CB_sel_activo').value;
                    var observacion = document.getElementById('txt_observa').value;
                    var ffecha = mes + '/' + dia + '/' + ano + " " + hora1;
                    //var ffechaf = mesf + '/' + diaf + '/' + anof + " " + hora2; //Lo cambie por que la fecha hasta qye esta allí es solo para saber hasta donde va a tomar en cuenta los días pero no para marcar un fin de cada día que se agregue, de la fecha hasta solo me impoorta la hora
                    var ffechaf = mes + '/' + dia + '/' + ano + " " + hora11;

                    if (Lunes === true && fini_.getDay() === 1) {
                        if (accion === "i") {
                            guardaFechas(ffecha, tipo, activo, observacion, ffechaf);
                        } else {
                            eliminaFechas(ffecha, ffechaf, tipo, activo);
                        }                        
                    } else if (Martes === true && fini_.getDay() === 2) {
                        if (accion === "i") {
                            guardaFechas(ffecha, tipo, activo, observacion, ffechaf);
                        } else {
                            eliminaFechas(ffecha, ffechaf, tipo, activo);
                        }  
                    } else if (Miercoles === true && fini_.getDay() === 3) {
                        if (accion === "i") {
                            guardaFechas(ffecha, tipo, activo, observacion, ffechaf);
                        } else {
                            eliminaFechas(ffecha, ffechaf, tipo, activo);
                        }  
                    } else if (Jueves === true && fini_.getDay() === 4) {
                        if (accion === "i") {
                            guardaFechas(ffecha, tipo, activo, observacion, ffechaf);
                        } else {
                            eliminaFechas(ffecha, ffechaf, tipo, activo);
                        }  
                    } else if (Viernes === true && fini_.getDay() === 5) {
                        if (accion === "i") {
                            guardaFechas(ffecha, tipo, activo, observacion, ffechaf);
                        } else {
                            eliminaFechas(ffecha, ffechaf, tipo, activo);
                        }  
                    } else if (Sabado === true && fini_.getDay() === 6) {
                        if (accion === "i") {
                            guardaFechas(ffecha, tipo, activo, observacion, ffechaf);
                        } else {
                            eliminaFechas(ffecha, ffechaf, tipo, activo);
                        }  
                    } else if (Domingo === true && fini_.getDay() === 0) {
                        if (accion === "i") {
                            guardaFechas(ffecha, tipo, activo, observacion, ffechaf);
                        } else {
                            eliminaFechas(ffecha, ffechaf, tipo, activo);
                        }  
                    }
                }
            }            
            //$('#dg_tia').dxDataGrid("instance").refresh();
        });
    }
}

// Función para calcular los días transcurridos entre dos fechas
restaFechas = function (f1, f2) {
    var aFecha1 = f1.split('/');
    var aFecha2 = f2.split('/');

    var p0 = aFecha1[0];
    var p1 = aFecha1[1];
    var p2 = aFecha1[2];

    var fFecha1 = Date.UTC(aFecha1[2], aFecha1[1] - 1, aFecha1[0]);
    var fFecha2 = Date.UTC(aFecha2[2], aFecha2[1] - 1, aFecha2[0]);
    var dif = fFecha2 - fFecha1;
    var dias = Math.floor(dif / (1000 * 60 * 60 * 24));
    return dias;
}

function guardaFechas(fecha, tipo, activo, observacion, fechafin) {
    
    $.ajax({
        type: "GET",
        url: FactoryX.Urls.guardaFechas,
        data: { fecha, tipo, activo, observacion, fechafin },
        success: (response) => {
            console.log('id del plan asignado correctamenete.');
            $('#dg_tia').dxDataGrid("instance").refresh();
        },
        error: (response) => {
            Swal.fire({
                title: 'No se insertarón registros',
                html: 'Revise el controlador.',
                icon: 'error'
            });
        }
    });
}


//$('#input-excel').change(function (e) {
//    //var prueba = document.getElementById('input-excel').value;
//    //alert(prueba);

//    var reader = new FileReader();
//    reader.readAsArrayBuffer(e.target.files[0]);
    
//    reader.onload = function (e) {
//        var data = new Uint8Array(reader.result);

//        var wb = XLSX.read(data, { type: 'array' });

//        var first_sheet_name = wb.SheetNames[0];
//        var worksheet = wb.Sheets[first_sheet_name];
//        var resultado = XLSX.utils.sheet_to_json(worksheet);
               
//        console.log(resultado);       
//    };
//});

function importaEx() {

    alert('Prueba....');

}

function reset1() {
    document.getElementById("input-excel").value = "";
}

$('#input-excel').change(function (e) {

    try {
        var reader = new FileReader();
        reader.readAsArrayBuffer(e.target.files[0]);

        if (reader === null) {
            alert('Error 1');
        }

        reader.onload = function (e) {
            var data = new Uint8Array(reader.result);

            var wb = XLSX.read(data, { type: 'array' });

            var first_sheet_name = wb.SheetNames[0];
            var worksheet = wb.Sheets[first_sheet_name];
            var resultado = XLSX.utils.sheet_to_json(worksheet);

            console.log(resultado);

            for (var i = 0; i < resultado.length; i++) {

                var registro = resultado[i];

                //alert(JSON.stringify(registro);
                //alert(registro.CeldaA0);

                var fd = (registro.Fecha_desde.toString()).split('-');
                var fh = (registro.Fecha_hasta.toString()).split('-');

                var fecha = fd[1] + '/' + fd[0] + '/' + fd[2];
                var tipo = registro.id_Tipo;
                var observacion = registro.Observacion;
                var fechafin = fh[1] + '/' + fh[0] + '/' + fh[2];

                $.ajax({
                    type: "GET",
                    url: FactoryX.Urls.guardaFechas,
                    data: { fecha, tipo, observacion, fechafin },
                    success: (response) => {
                        console.log('id del plan asignado correctamenete.');
                        $('#dg_tia').dxDataGrid("instance").refresh();
                    },
                    error: (response) => {
                        alert("No se pudo asignar el id del plan.");
                    }
                });

            }
            document.getElementById("modalExcel").value = "";
            $('#modalExcel').modal('hide');
        };
    } catch {
        alert('Error: no hubo comunicación con el controlador');
    }

    
});

function mostrarDias() {
    var t = document.getElementById("grid_grupoActivos");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#grid_grupoActivos').show(1000);
        //document.getElementById('btnOM').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#grid_grupoActivos').hide(1000);
        //document.getElementById('btnOM').textContent = "Mostrar tabla";
    }
}

function mostrarTipoDias() {
    var t = document.getElementById("tipo_inactividad");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#tipo_inactividad').show(1000);
        //document.getElementById('btnOM').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#tipo_inactividad').hide(1000);
        //document.getElementById('btnOM').textContent = "Mostrar tabla";
    }
}

function descargaExcel() {
    $.ajax({
        type: "GET",
        url: FactoryX.Urls.ConsultaCapacidadesActivos,
        data: { },
        success: (response) => {

            if (typeof XLSX === 'undefined') XLSX = require('xlsx');
            var ws = XLSX.utils.json_to_sheet(response);
            var wb = XLSX.utils.book_new();
            XLSX.utils.book_append_sheet(wb, ws, "Hoja1");
            XLSX.writeFile(wb, "Inactividades.xlsx");
                       
            //for (var i = 0; i < response.length; i++) {
            //    var resultado = response[i];
            //    alert(resultado.Fecha_desde);
            //}
        },
        error: (response) => {
            alert("No se pudo asignar el id del plan.");
        }
    });
}

function eliminaFechas(fecha, fecha_hasta, tipo, activo) {
    $.ajax({
        type: "GET",
        url: FactoryX.Urls.eliminaFechas,
        data: { fecha, fecha_hasta, tipo, activo },
        success: (response) => {
            console.log('registros eliminados correctamenete.');    
            $('#dg_tia').dxDataGrid("instance").refresh();
        },
        error: (response) => {
            Swal.fire({
                title: 'No se insertarón registros',
                html: 'Revise el controlador.',
                icon: 'error'
            });
        }
    });
}

//$('#input-excel').change(function (e) {
//    var X = XLSX;
//    var fileUpload = document.getElementById('input-excel');
    
//    var reader = new FileReader();
//    reader.readAsArrayBuffer(e.target.files[0]);

//    reader.onload = function (e) {
//        var data = e.target.result;
//        var workbook = XLSX.read(data, { type: 'binary' });
//        var result = {};
//        workbook.SheetNames.forEach(function (sheetName) {
//            var roa = X.utils.sheet_to_row_object_array(workbook.Sheets[sheetName]);
//            if (roa.length > 0) {
//                result[sheetName] = roa;
//            }
//        });
//        var output = JSON.stringify(result, 2, 2);
//        jQuery.ajax({
//            type: "POST",
//            url: FactoryX.Urls.LeeExcel,
//            dataType: "json",
//            data: {
//                dataToUpload: output
//            },
//            success: function (successMsg) {
//                alert(successMsg.records);
//            }
//        });
//});