var sw01 = false;

var tipoDato = 'text';
var formato = '';

function dateDiff(secondDate) {
    var diffInDay = Math.floor(Math.abs((new Date() - secondDate) / (24 * 60 * 60 * 1000)));
    return $("#age").text(diffInDay + " days");
}

function switch_valueChanged(data) {
    sw01 = $("#swich01").dxSwitch("instance").option("value");
}

function dateBox_valueChanged(data) {
    dateDiff(new Date(data.value));
}

$(function () {
    dateDiff(new Date(1981, 3, 27));
});

function mostrarFiltroMes() {

    var t = document.getElementById("mesFilter");
    //mostrarFiltroSemana();
    //mostrarFiltroDia();
    //mostrarFiltroHora();

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#mesFilter').show(1000);
        document.getElementById("btnHora").disabled = true;
        document.getElementById("btnSemana").disabled = true;
        document.getElementById("btnDia").disabled = true;
        document.getElementById('btnMes').textContent = "Ocultar";
    }
    else {
        //t.style.display = "none";
        $('#mesFilter').hide(1000);
        document.getElementById("btnHora").disabled = false;
        document.getElementById("btnSemana").disabled = false;
        document.getElementById("btnDia").disabled = false;
        document.getElementById('btnMes').textContent = "Mes";
    }
}

function mostrarFiltroSemana() {
    var t = document.getElementById("semanaFilter");
    //mostrarFiltroMes();
    //mostrarFiltroDia();
    //mostrarFiltroHora();

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#semanaFilter').show(1000);
        document.getElementById("btnHora").disabled = true;
        document.getElementById("btnMes").disabled = true;
        document.getElementById("btnDia").disabled = true;
        document.getElementById('btnSemana').textContent = "Ocultar";
    }
    else {
        //t.style.display = "none";
        $('#semanaFilter').hide(1000);
        document.getElementById("btnHora").disabled = false;
        document.getElementById("btnMes").disabled = false;
        document.getElementById("btnDia").disabled = false;
        document.getElementById('btnSemana').textContent = "Semana";
    }
}

function mostrarFiltroDia() {
    var t = document.getElementById("diaFilter");

    //mostrarFiltroSemana();
    //mostrarFiltroMes();
    //mostrarFiltroHora();

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#diaFilter').show(1000);
        document.getElementById("btnHora").disabled = true;
        document.getElementById("btnSemana").disabled = true;
        document.getElementById("btnMes").disabled = true;
        document.getElementById('btnDia').textContent = "Ocultar";
    }
    else {
        //t.style.display = "none";
        $('#diaFilter').hide(1000);
        document.getElementById("btnHora").disabled = false;
        document.getElementById("btnSemana").disabled = false;
        document.getElementById("btnMes").disabled = false;
        document.getElementById('btnDia').textContent = "Día";
    }
}

function mostrarFiltroHora() {
    var t = document.getElementById("horaFilter");
    //mostrarFiltroSemana();
    //mostrarFiltroDia();
    //mostrarFiltroMes();

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#horaFilter').show(1000);
        document.getElementById("btnSemana").disabled = true;
        document.getElementById("btnMes").disabled = true;
        document.getElementById("btnDia").disabled = true;
        document.getElementById('btnHora').textContent = "Ocultar";
    }
    else {
        //t.style.display = "none";
        $('#horaFilter').hide(1000);
        document.getElementById("btnSemana").disabled = false;
        document.getElementById("btnMes").disabled = false;
        document.getElementById("btnDia").disabled = false;
        document.getElementById('btnHora').textContent = "Hora";
    }
}

function obtenerMes(idEmpresa) {

    try {
        var valor = $("#mesMaquina").dxSelectBox("instance").option('value');
        var variable = valor['Variable'];
        var maquina  = valor['Cod_activo'];
        var unidad   = valor['Unidad'];

        //var fini_ = moment(document.getElementById('mes-ini').value + '-01', 'YYYY-MM-DD').format('YYYY-MM-DD');
        //var ffin_ = moment(document.getElementById('mes-fin').value + '-01').add(1, 'month').subtract(1, 'day').format('YYYY-MM-DD');

        var finiM = moment($("#mes-ini").dxDateBox("instance").option('value'), 'YYYY-MM-DD').format('YYYY-MM-DD').substring(0, 8) + '01';
        var ffinM = moment($("#mes-fin").dxDateBox("instance").option('value'), 'YYYY-MM-DD').format('YYYY-MM-DD').substring(0, 8) + '01';
        ffinM = moment(ffinM).add(1, 'month').subtract(1, 'day');
        ffinM = moment(ffinM, 'YYYY-MM-DD').format('YYYY-MM-DD');

        if ($('#txt_tb0').text() === "0") {
            //Historicos_variables(idEmpresa, fini_, ffin_, "mes", 0, maquina, 0, variable, unidad);
            IndividualKpiUnidadesProducidas(idEmpresa, finiM, ffinM, "mes", null, maquina, variable, unidad);
        }
        else {
            animacionWW('A', 3);
            dgConsolidado(finiM, ffinM, null, "mes");
        } 

        document.getElementById('alertaInicial').style.display = "none";
    }
    catch {
        Swal.fire({
            title: 'Verifique los datos seleccionados',
            html: 'Debe escoger una maquina valida',
            icon: 'error'
        });

        animacionWW('X', 1);
        animacionWW('X', 2);   
        animacionWW('X', 3);   
    }
}

function obtenerSemana(idEmpresa) {

    try {

        var valor = $("#semanaMaquina").dxSelectBox("instance").option('value');
        var variable = valor['Variable'];
        var maquina = valor['Cod_activo'];
        var unidad = valor['Unidad'];

        var s1 = document.getElementById('semana-ini').value.substring(6, 8);
        var s2 = document.getElementById('semana-fin').value.substring(6, 8);

        var annoI = 0;
        var semanaI = 0;
        var annoF = 0;
        var semanaF = 0;
        var tur = '';

        var appName = navigator.userAgent.indexOf('Firefox');

        if (appName > -1) {

            annoI = $('#BoxIni_ano').dxNumberBox("instance").option('value');
            semanaI = $('#BoxIni').dxNumberBox("instance").option('value');
            annoF = $('#BoxFin_ano').dxNumberBox("instance").option('value');
            semanaF = $('#BoxFin').dxNumberBox("instance").option('value');
            tur = annoI + '-W' + semanaI + "|" + annoF + '-W' + semanaF;
        }
        else {
            //var s1 = document.getElementById('semana-ini').value.substring(6, 8);
            //var s2 = document.getElementById('semana-fin').value.substring(6, 8);

            annoI = parseInt(document.getElementById('semana-ini').value.substring(0, 4));
            semanaI = parseInt(document.getElementById('semana-ini').value.substring(6, 8));
            annoF = parseInt(document.getElementById('semana-fin').value.substring(0, 4));
            semanaF = parseInt(document.getElementById('semana-fin').value.substring(6, 8));
            tur = document.getElementById('semana-ini').value + "|" + document.getElementById('semana-fin').value;
        }

        var fini_ = moment().year(annoI).week(semanaI).day("Monday").startOf("day").format("DD-MM-YYYY");
        var ffin_ = moment(moment().year(annoF).week(semanaF).day("Monday").startOf("day"), "DD-MM-YYYY")
        ffin_ = ffin_.add(6, 'days').format("DD-MM-YYYY");

        //var fini_ = moment(document.getElementById('semana-ini').value.substring(0, 4) + "-" + moment(moment().isoWeek(s1).startOf("isoWeek")).format().substring(5, 7) + "-" + moment(moment().isoWeek(s1).startOf("isoWeek")).format().substring(8, 10), "YYYY-MM-DD").add(-6, 'days').format();
        //var ffin_ = moment(document.getElementById('semana-fin').value.substring(0, 4) + "-" + moment(moment().isoWeek(s2).startOf("isoWeek")).format().substring(5, 7) + "-" + moment(moment().isoWeek(s2).startOf("isoWeek")).format().substring(8, 10)).format();

        //var tur = document.getElementById('semana-ini').value + "|" + document.getElementById('semana-fin').value;

        if ($('#txt_tb0').text() === "0") {
            //Historicos_variables(idEmpresa, fini_, ffin_, "semana", tur, maquina, 0, variable, unidad);
            IndividualKpiUnidadesProducidas(idEmpresa, fini_, ffin_, "semana", tur, maquina, variable, unidad);
        }
        else {
            animacionWW('A', 3); 
            dgConsolidado(fini_, ffin_, tur, "semana");
        } 

        document.getElementById('alertaInicial').style.display = "none";
    }
    catch {
        Swal.fire({
            title: 'Verifique los datos seleccionados',
            html: 'Debe escoger una maquina valida',
            icon: 'error'
        });

        animacionWW('X', 1);
        animacionWW('X', 2);
        animacionWW('X', 3);
    }
    
}

function obtenerDia(idEmpresa) {

    try {
        var valor = $("#diaMaquina").dxSelectBox("instance").option('value');
        var variable = valor['Variable'];
        var maquina = valor['Cod_activo'];
        var unidad = valor['Unidad'];

        var fini_ = document.getElementById("diaDesde").innerHTML;
        fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 17);
        //alert(fini_);
        var ffin_ = document.getElementById("diaHasta").innerHTML;
        ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 17);
        //alert(ffin_);
        var turno = $('#diaTurno').dxSelectBox("option", "value");

        if ($('#txt_tb0').text() === "0") {
            //Historicos_variables(idEmpresa, fini_, ffin_, "dia", turno, maquina, 0, variable, unidad);
            IndividualKpiUnidadesProducidas(idEmpresa, fini_, ffin_, "dia", turno, maquina, variable, unidad);
            //data: { idEmpresa, fini_, ffin_, filtro, turno, cod_activo, variable, unidad }
        }
        else {
            animacionWW('A', 3); 
            dgConsolidado(fini_, ffin_, turno, "dia");
        }

        document.getElementById('alertaInicial').style.display = "none";
    }
    catch {
        Swal.fire({
            title: 'Verifique los datos seleccionados',
            html: 'Debe escoger una maquina valida',
            icon: 'error'
        });

        animacionWW('X', 1);
        animacionWW('X', 2);
        animacionWW('X', 3);
    }
}

function obtenerHora(idEmpresa) {

    try {
        var valor = $("#horaMaquina").dxSelectBox("instance").option('value');
        var variable = valor['Variable'];
        var maquina = valor['Cod_activo'];
        var unidad = valor['Unidad'];

        var fini_ = document.getElementById("horaDesde").innerHTML;
        fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 26);

        var ffin_ = document.getElementById("horaHasta").innerHTML;
        ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 26);

        //var maquina = document.getElementById("horaMaquina").innerHTML;
        //maquina = maquina.substring(maquina.search("value") + 7, maquina.search("value") + 12);

        if ($('#txt_tb0').text() === "0") {
            //Historicos_variables(idEmpresa, fini_, ffin_, "hora", 0, maquina, 0, variable);
            IndividualKpiUnidadesProducidas(idEmpresa, fini_, ffin_, "hora", null, maquina, variable, unidad);
        }
        else {
            animacionWW('A', 3); 
            dgConsolidado(fini_, ffin_, null, "hora");
        }

        document.getElementById('alertaInicial').style.display = "none";

    }
    catch {
        Swal.fire({
            title: 'Verifique los datos seleccionados',
            html: 'Debe escoger una maquina valida',
            icon: 'error'
        });

        animacionWW('X', 1);
        animacionWW('X', 2);
        animacionWW('X', 3);
    }
}

function IndividualKpiUnidadesProducidas(idEmpresa, fini_, ffin_, filtro, turno, cod_activo, variable, unidad) {

    //alert("Prueba");

    $.ajax({
        type: "POST", //Con esto genero el grafico de barras de unidades producidas en un dia en turno y fuera de turno
        url: FactoryX.Urls.KpiUnidadesProducidasX, //Historicos_variables,
        data: { idEmpresa, fini_, ffin_, filtro, turno, unidad, cod_activo, porSku:false, sw01 }
    }).done(function (data) {
        grafico1(data);
        tabla1(data);
    })

    $.ajax({
        type: "POST", //Con esto genero el gráfico de pastel que tiene el porcentaje por productos creados 
        url: FactoryX.Urls.KpiUnidadesProducidasX, //Historicos_variables,
        data: { idEmpresa, fini_, ffin_, filtro, turno, unidad, cod_activo, porSku:true, sw01 }
    }).done(function (data) {
        grafico2(data);
        tabla2(data);
    })
}

function grafico1(data) {

    series_char = [];

    let enTurno = [];
    let fueraTurno = [];
    let fecha = [];
    let unidades = [];
    let tiempoConjunto = [];
    unidades[0] = "Unidades";

    for (var i = 0; i < data.length; i++) {
        enTurno[i] = data[i].enTurnox;
        fueraTurno[i] = data[i].sinTurno;
        fecha[i] = data[i].fecha;
        tiempoConjunto[i] = data[i].fecha + (data[i].unidades === 'Fecha y hora' ? ' ' + data[i].hora + ':00' : '');
    }
    
    series_char[0] = { type: 'column', name: "En turno ", data: enTurno };
    series_char[1] = { type: 'column', name: "Extra turno ", data: fueraTurno };

    Highcharts.chart('container', {
        chart: {
            type: 'column'
        },
        title: {
            text: 'Unidades producidas'
        },
        xAxis: {
            categories: tiempoConjunto
        },
        yAxis: {
            title: {
                text: unidades
            },

        },
        credits: {
            enabled: false
        },
        legend: {
            layout: 'vertical',
            align: 'right',
            verticalAlign: 'middle'
        },
        labels: {
            items: [{
                //html: 'Disponibilidad de los activos',
                style: {
                    left: '50px',
                    top: '18px',
                    color: ( // theme
                        Highcharts.defaultOptions.title.style &&
                        Highcharts.defaultOptions.title.style.color
                    ) || 'black'
                }
            }]
        },
        tooltip: {
            headerFormat: '<b>{point.x}</b><br/>',
            pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
        },
        plotOptions: {
            column: {
                stacking: 'normal',
                dataLabels: {
                    enabled: true
                }
            }
        },
        series: series_char,
        colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
            '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
        chart: {
            backgroundColor: null,
            style: {
                fontFamily: 'Dosis, sans-serif'
            }
        }
    });

    animacionWW('C', 1);
}

function grafico2(data) {

    series_char_sku = [];

    for (var i = 0; i < data.length; i++) {
        series_char_sku[i] = { name: data[i].cod_producto, y: data[i].total };
    }

    Highcharts.chart('container1', {

        title: {
            text: 'Porcentaje de SKUs producidos'
        },
        tooltip: {
            pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>'
        },
        accessibility: {
            point: {
                valueSuffix: '%'
            }
        },
        credits: {
            enabled: false
        },
        plotOptions: {
            pie: {
                allowPointSelect: true,
                cursor: 'pointer',
                dataLabels: {
                    enabled: true,
                    format: '<b>{point.name}</b>: {point.percentage:.2f} %'
                }
            }
        },
        series: [{
            name: 'Valor',
            colorByPoint: true,
            data: series_char_sku
        }],
        //series: series_char,
        colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
            '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
        chart: {
            backgroundColor: null,
            style: {
                fontFamily: 'Dosis, sans-serif'
            },
            type: 'pie'
        }
    });

    animacionWW('C', 2);
}

function tabla1(data) {

    var datos = [];
    var unidades = 'Unidades';
    var filtro_;
    var seVe = false;

    try {
        filtro_ = data[0].unidades;
    }
    catch{
        filtro_ = null;
    }

    var fecha_;

    for (var i = 0; i < data.length; i++) {
        
        if (data[i].unidades.substring(0,3) === "Día") {            

            fecha_ = moment(data[i].fecha, 'DD/MM/YYYY').format('MM/DD/YYYY'); 

            tipoDato = 'datetime';
            formato = 'dd/MM/yyyy';
        }
        else if (data[i].unidades === "Fecha y hora") {
            fecha_ = moment(data[i].fecha, 'DD/MM/YYYY').format('MM/DD/YYYY');

            tipoDato = 'datetime';
            formato = 'dd/MM/yyyy';
            seVe = true;
            filtro_ = "Fecha_";
        }
        else {
            fecha_ = data[i].fecha;

            tipoDato = 'text';
            formato = '';
        }
        
        datos.push({
            "Filtro": data[i].unidades,
            "Fecha": fecha_,
            "Hora": data[i].hora,
            "En turno": data[i].enTurnox,
            "Extra turno": data[i].sinTurno,
            "Total": data[i].total,
            "Unidades": unidades,
            "Turno": data[i].turno,
            "Activo": data[i].cod_activo
        });
    }

    $("#tablaRegistros1").dxDataGrid({
        dataSource: datos,
        allowColumnReordering: true,
        showBorders: true,
        paging: {
            pageSize: 10
        },
        groupPanel: {
            visible: true
        },
        export: {
            enabled: true
        },
        onExporting: function (e) {
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('Main sheet');
            DevExpress.excelExporter.exportDataGrid({
                worksheet: worksheet,
                component: e.component,
                customizeCell: function (options) {
                    var excelCell = options;
                    excelCell.font = { name: 'Arial', size: 12 };
                    excelCell.alignment = { horizontal: 'left' };
                }
            }).then(function () {
                workbook.xlsx.writeBuffer().then(function (buffer) {
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
                });
            });
            e.cancel = true;
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },
        allowColumnResizing: true,
        columns: [

            "Filtro"
            ,
            {
                caption: filtro_,
                dataField: "Fecha",
                dataType: tipoDato,
                format: formato,
            },
            {
                caption: "Hora",
                dataField: "Hora",
                format: {
                    type: "fixedPoint",
                    precision: 0
                },
                minWidth: 60,
                visible: seVe,
            },
            {
                caption: "En Turno",
                dataField: "En turno",
                format: {
                    type: "fixedPoint",
                    precision: 2
                },
            },
            {
                caption: "Extra turno",
                dataField: "Extra turno",
                format: {
                    type: "fixedPoint",
                    precision: 2
                },
            },
            {
                caption: "Total",
                dataField: "Total",
                format: {
                    type: "fixedPoint",
                    precision: 2
                },
            },
            "Unidades",
            "Activo"
        ]
    });
}

function tabla2(data) {

    var total_SKU = 0;    
    var datos2 = [];

    for (var i = 0; i < data.length; i++) {
        total_SKU = total_SKU + data[i].total;
    }

    for (var i = 0; i < data.length; i++) {
        datos2.push({
            "SKU": data[i].cod_producto,
            "Unidades totales": data[i].total,
            "Porcentaje": ((data[i].total * 100) / total_SKU).toFixed(2)
        });
    }

    $("#tablaRegistros2").dxDataGrid({
        dataSource: datos2,
        allowColumnReordering: true,
        showBorders: true,
        allowColumnResizing: true,
        paging: {
            pageSize: 10
        },
        groupPanel: {
            visible: true
        },
        export: {
            enabled: true
        },
        onExporting: function (e) {
            var workbook = new ExcelJS.Workbook();
            var worksheet = workbook.addWorksheet('Main sheet');
            DevExpress.excelExporter.exportDataGrid({
                worksheet: worksheet,
                component: e.component,
                customizeCell: function (options) {
                    var excelCell = options;
                    excelCell.font = { name: 'Arial', size: 12 };
                    excelCell.alignment = { horizontal: 'left' };
                }
            }).then(function () {
                workbook.xlsx.writeBuffer().then(function (buffer) {
                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
                });
            });
            e.cancel = true;
        },
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },
        columns: [
            "SKU",
            {
                caption: "Unidades totales",
                dataField: "Unidades totales",
                format: {
                    type: "fixedPoint",
                    precision: 2
                },
            },
            "Porcentaje"
        ]
    });
}

//function grafico1_(data) {

//    Highcharts.chart('container', {
//        chart: {
//            type: 'column'
//        },
//        title: {
//            text: 'Unidades producidas'
//        },
//        xAxis: {
//            categories: tiempoConjunto
//        },
//        yAxis: {
//            title: {
//                text: unidades
//            },

//        },
//        credits: {
//            enabled: false
//        },
//        legend: {
//            layout: 'vertical',
//            align: 'right',
//            verticalAlign: 'middle'
//        },
//        labels: {
//            items: [{
//                //html: 'Disponibilidad de los activos',
//                style: {
//                    left: '50px',
//                    top: '18px',
//                    color: ( // theme
//                        Highcharts.defaultOptions.title.style &&
//                        Highcharts.defaultOptions.title.style.color
//                    ) || 'black'
//                }
//            }]
//        },
//        tooltip: {
//            headerFormat: '<b>{point.x}</b><br/>',
//            pointFormat: '{series.name}: {point.y}<br/>Total: {point.stackTotal}'
//        },
//        plotOptions: {
//            column: {
//                stacking: 'normal',
//                dataLabels: {
//                    enabled: true
//                }
//            }
//        },
//        series: series_char,
//        colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
//            '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
//        chart: {
//            backgroundColor: null,
//            style: {
//                fontFamily: 'Dosis, sans-serif'
//            }
//        }
//    });

//    animacionWW('C', 1); 
//}

//function grafico2_(data) {
//    Highcharts.chart('container1', {
        
//        title: {
//            text: 'Porcentaje de SKUs producidos'
//        },
//        tooltip: {
//            pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>'
//        },
//        accessibility: {
//            point: {
//                valueSuffix: '%'
//            }
//        },
//        credits: {
//            enabled: false
//        },
//        plotOptions: {
//            pie: {
//                allowPointSelect: true,
//                cursor: 'pointer',
//                dataLabels: {
//                    enabled: true,
//                    format: '<b>{point.name}</b>: {point.percentage:.2f} %'
//                }
//            }
//        },
//        series: [{
//            name: 'Valor',
//            colorByPoint: true,
//            data: series_char_sku
//        }],
//        //series: series_char,
//        colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
//            '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
//        chart: {
//            backgroundColor: null,
//            style: {
//                fontFamily: 'Dosis, sans-serif'
//            },
//            type: 'pie'
//        }
//    });

//    animacionWW('C', 2);
//}

//function Historicos_variables(idEmpresa, fini_, ffin_, filtro, turno, cod_activo, flag, variable, unidad) {

//    $.ajax({
//        type: "POST",
//        url: FactoryX.Urls.Historicos_variables,
//        data: { idEmpresa, fini_, ffin_, filtro, turno, cod_activo, variable, unidad}
//    }).done(function (data) {

//        try {
//            alert.log(data.data.length);
//        }
//        catch{
//        }

//        Grafico_disponibilidad(data, fini_, ffin_, flag);
//    });
//}

//function Grafico_disponibilidad(data, fini_, ffin_, flag) {

//    if (data.length === 0) {
//        //alert("No se encontraron registros que mostrar"); //CAZA
//        fin_carga();
//        Swal.fire({
//            title: 'No se encontraron registros que mostrar',
//            html: 'Valide la selección de los datos usados.',
//            icon: 'info'
//        });
//    }

//    var prueba = data[0];

//    var factorX = "100040 - Termoformadora MULTIVAC"; //data[0].nombreActivo;

//    document.getElementById('Etiqueta_maquina').textContent = "Unidades producidas - " + factorX;
//    document.getElementById('alertaInicial').textContent = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + fini_ + ', hasta: ' + ffin_;
//    if (flag == 0) {
//        $('#alertaInicial').hide(1000);
//    }

//    tiempo1 = [];
//    tiempo2 = [];
//    tiempoConjunto = [];

//    tiempo1 = data[0].tiempo;
//    tiempo2 = data[0].tiempo2;
//    tiempoConjunto = tiempo1.concat(tiempo2);
//    tiempoConjunto = tiempoConjunto.filter((a, b) => tiempoConjunto.indexOf(a) === b);
//    tiempoConjunto = tiempoConjunto.sort();

//    unidades = "";
//    unidades = data[0].unidades[0];


//    tiempoTotal = [];
//    series = [];

//    inTurno = [];
//    outTurno = [];

//    for (var j = 0; j < tiempoConjunto.length; j++) {
//        if (tiempo1.indexOf(tiempoConjunto[j]) == -1) {
//            inTurno[j] = 0;
//        }
//        else {
//            inTurno[j] = data[0].data[tiempo1.indexOf(tiempoConjunto[j])];
//        }
//        if (tiempo2.indexOf(tiempoConjunto[j]) == -1) {
//            outTurno[j] = 0;
//        }
//        else {
//            outTurno[j] = data[0].data2[tiempo2.indexOf(tiempoConjunto[j])];
//        }
//    }

//    //inTurno[0] = data[0].data[0];
//    //outTurno[0] = data[0].data2[0];
//    //grafico Barras
//    series_char = [];
//    for (i = 0; i < series.length; i++) {
//        series_char[i] = { type: 'column', name: data[i].nombreActivo, data: series[i] };
//    }

//    series_char[0] = { type: 'column', name: "En turno", data: inTurno };
//    series_char[1] = { type: 'column', name: "Extra turno ", data: outTurno };

//    //Grafico pastel
//    series_char_sku = [];
//    aux1 = 0;
//    for (i = 0; i < data[0].sku_conteo_total.length; i++) {
//        aux1 = aux1 + data[0].sku_conteo_total[i];
//    }
//    aux2 = 0;
//    for (i = 0; i < data[0].sku_conteo.length; i++) {
//        aux2 = aux2 + data[0].sku_conteo[i];
//    }


//    for (i = 0; i < data[0].sku_conteo.length; i++) {
//        series_char_sku[i] = { name: data[0].sku[i], y: data[0].sku_conteo[i] };
//    }

//    if (aux1 - aux2 != 0) {
//        series_char_sku[series_char_sku.length] = { name: "Sin registro", y: aux1 - aux2 };
//    }

//    //grafico1_();
//    //grafico2_();

//    var datos = [];
//    var datos2 = [];
//    var objeto = {};
//    var objeto2 = {};

//    var filtro_ = "";

//    if (data[0].filtro === "Día") {
//        if (data[0].tiempo3[0] === "" || data[0].tiempo3[0] === null || data[0].tiempo3[0] === undefined) {
//            filtro_ = "Día";
//        }
//        else {
//            filtro_ = "Día - Turno: " + data[0].tiempo3[0];
//        }
//    }
//    else if (data[0].filtro === "Hora"){
//        filtro_ = "Fecha y hora";
//    }
//    else {
//        filtro_ = data[0].filtro;
//    }

//    for (var y = 0; y < tiempoConjunto.length; y++) {

//        datos.push({
//            "Filtro": filtro_, //data[0].filtro,
//            "Fecha": data[0].filtro === "Día" ?
//                        tiempoConjunto[y].substring(4, tiempoConjunto[y].length) : tiempoConjunto[y],
//            "En turno": inTurno[y],
//            "Extra turno": outTurno[y],
//            "Total": inTurno[y] + outTurno[y],
//            "Unidades": unidades,
//            "Turno": data[0].tiempo3[y],
//            "Activo": data[0].nombreActivo
//        });
//        total_suma = 0;
//    }


//    objeto = datos;

//    $("#tablaRegistros1").dxDataGrid({
//        dataSource: objeto,
//        allowColumnReordering: true,
//        showBorders: true,
//        paging: {
//            pageSize: 10
//        },
//        groupPanel: {
//            visible: true
//        },
//        export: {
//            enabled: true
//        },
//        onExporting: function (e) {
//            var workbook = new ExcelJS.Workbook();
//            var worksheet = workbook.addWorksheet('Main sheet');
//            DevExpress.excelExporter.exportDataGrid({
//                worksheet: worksheet,
//                component: e.component,
//                customizeCell: function (options) {
//                    var excelCell = options;
//                    excelCell.font = { name: 'Arial', size: 12 };
//                    excelCell.alignment = { horizontal: 'left' };
//                }
//            }).then(function () {
//                workbook.xlsx.writeBuffer().then(function (buffer) {
//                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
//                });
//            });
//            e.cancel = true;
//        },
//        pager: {
//            showPageSizeSelector: true,
//            allowedPageSizes: [5, 10, 20],
//            showInfo: true
//        },
//        columns: [
            
//                "Filtro"
//            ,
//            {
//                caption: filtro_,
//                dataField: "Fecha"
//            },
//            //"Turno",
//            "En turno",
//            "Extra turno",
//            "Total",
//            "Unidades",
//            "Activo"
//        ]
//    });


//    //Tabla SKUS
//    var total_SKU = 0;
//    for (var y = 0; y < series_char_sku.length; y++) {
//        total_SKU = total_SKU + series_char_sku[y].y;
//        //objeto2 = series_char_sku;
//    }

//    for (var y = 0; y < series_char_sku.length; y++) {
//        datos2.push({
//            //"Fecha": data[y].fecha[i],
//            "SKU": series_char_sku[y].name,
//            "Unidades totales": series_char_sku[y].y,
//            "Porcentaje": ((series_char_sku[y].y * 100) / total_SKU).toFixed(2)
//            //"Total": inTurno[y] + outTurno[y]
//            //"Sku": data[y].sku[i]
//        });

//        //objeto2 = series_char_sku;
//    }

//    objeto2 = datos2;

//    $("#tablaRegistros2").dxDataGrid({
//        dataSource: objeto2,
//        allowColumnReordering: true,
//        showBorders: true,
//        paging: {
//            pageSize: 10
//        },
//        groupPanel: {
//            visible: true
//        },
//        export: {
//            enabled: true
//        },
//        onExporting: function (e) {
//            var workbook = new ExcelJS.Workbook();
//            var worksheet = workbook.addWorksheet('Main sheet');
//            DevExpress.excelExporter.exportDataGrid({
//                worksheet: worksheet,
//                component: e.component,
//                customizeCell: function (options) {
//                    var excelCell = options;
//                    excelCell.font = { name: 'Arial', size: 12 };
//                    excelCell.alignment = { horizontal: 'left' };
//                }
//            }).then(function () {
//                workbook.xlsx.writeBuffer().then(function (buffer) {
//                    saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
//                });
//            });
//            e.cancel = true;
//        },
//        pager: {
//            showPageSizeSelector: true,
//            allowedPageSizes: [5, 10, 20],
//            showInfo: true
//        },
//        columns: ["SKU", "Unidades totales", "Porcentaje"]
//    });

//    fin_carga();

//}

function mostrarTabla() {
    var t = document.getElementById("divTabla");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#divTabla').show(1000);
        document.getElementById('MOTabla').textContent = "Ocultar información en formato tabla";
        document.getElementById("MOTablaTodo").style.visibility = 'visible';
        $('#dash').hide(1000);
    }
    else {
        //t.style.display = "none";
        $('#divTabla').hide(1000);
        document.getElementById('MOTabla').textContent = "Mostrar información en formato tabla";
        document.getElementById("MOTablaTodo").style.visibility = 'hidden';
        $('#dash').show(1000);
    }
}

function mostrarTablaTodo() {
    var t = document.getElementById("divTabla");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        //$('#dash').show(1000);
        //$('#MOTablaTodo').hide(1000);
    }
    else {
        //t.style.display = "none";
        //$('#divTabla').hide(1000);
        $('#dash').show(1000);
        document.getElementById("MOTablaTodo").style.visibility = 'hidden';
        //document.getElementById('MOTablaTodo').textContent = "Mostrar históricos en formato tabla";
    }
}

//$(document).ready(function () {
$(".botoncito4").click(function () {
    //$(this).addClass("cargando"); //Animacion de espera
    animacionWW('A',1);
    animacionWW('A',2);
});
//});

function fin_carga() {
    $(".botoncito4").removeClass("cargando");
};

$(document).ready(function () {

    //Con esto detecto si el navegador es Firefox
    var appName = navigator.userAgent.indexOf('Firefox');

    if (appName > -1) {

        $('#divFirefoxSemanaIni').show();
        $('#divFirefoxSemanaFin').show();

        $('#divSemanaIni').hide();
        $('#divSemanaFin').hide();

        var today = new Date();

        $("#BoxIni").dxNumberBox({
            value: 1,
            min: 1,
            max: 53,
            showSpinButtons: true
        });

        $("#BoxFin").dxNumberBox({
            value: 1,
            min: 1,
            max: 53,
            showSpinButtons: true
        });

        $("#BoxIni_ano").dxNumberBox({
            value: today.getFullYear(),
            min: 2000,
            max: 2099,
            showSpinButtons: true
        });

        $("#BoxFin_ano").dxNumberBox({
            value: today.getFullYear(),
            min: 2000,
            max: 2099,
            showSpinButtons: true
        });
    }
    else {

        $('#divFirefoxSemanaIni').hide();
        $('#divFirefoxSemanaFin').hide();

        $('#divSemanaIni').show();
        $('#divSemanaFin').show();
    }

    var beforeOneWeek = new Date(new Date().getTime() - 60 * 60 * 24 * 7 * 1000)
    var day = beforeOneWeek.getDay()
    var diffToMonday = beforeOneWeek.getDate() - day + (day === 0 ? -6 : 1)
    var lastMonday = new Date(beforeOneWeek.setDate(diffToMonday))
    var lastSunday = new Date(beforeOneWeek.setDate(diffToMonday + 6));

    //var date = new Date();
    //var newdate = new Date(date);
    //newdate.setDate(newdate.getDate() - 7); // minus the date
    //var nd = new Date(newdate);
    var fini_ = lastMonday.getDate() + "/" + (lastMonday.getMonth() + 1) + "/" + lastMonday.getFullYear();
    var ffin_ = moment(fini_, "DD/MM/YYYY").add(6, 'days').format("DD/MM/YYYY");
    
    //var inicio = true;
    var turno = null;
    //var sku = null;
    var filtro = "dia";
    var idEmpresa = document.getElementById("idEmpresa").value;

    //alert("Flag 1");
    
    Activo_al_cargar(idEmpresa, fini_, ffin_, filtro, turno);
});

function Activo_al_cargar(idEmpresa, fini_, ffin_, filtro, turno) {

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.Lista_activos2,
        data: idEmpresa
    }).done(function (data) {

        document.getElementById('alertaInicial').innerHTML = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + fini_ + ' hasta: ' + ffin_ + ', en el activo: ' + data[0].Cod_activo;
        //alert("Flag 2");

        //Historicos_variables(idEmpresa, fini_, ffin_, filtro, turno, data[0].Cod_activo, 1, data[0].Variable, data[0].Unidad)
        IndividualKpiUnidadesProducidas(idEmpresa, fini_, ffin_, filtro, turno, data[0].Cod_activo, data[0].Variable, data[0].Unidad);
    });
}

function animacionWW(valor, num) {

    if (valor === 'A') {

        switch (num) {

            case 1:
                $('#progresBar1').show();
                break;
            case 2:
                $('#progresBar2').show();
                break; 
            case 3:
                $('#progresBar3').show();
                break; 

            default:
        }
    }
    else {
        //$('#progresBar').hide();
        switch (num) {

            case 1:
                $('#progresBar1').hide();
                break;
            case 2:
                $('#progresBar2').hide();
                break;
            case 3:
                $('#progresBar3').hide();
                break;
            
            default:
        }
    }
}

function tbl_consolidado(btn) {

    if ($('#txt_tb0').text() === "0") {
        $('#txt_tb0').text('1');
        $('#btn_tbl01').text('Ocultar tabla consolidado de maquinas');
        $('#btn_tbl02').text('Ocultar tabla consolidado de maquinas');
        $('#btn_tbl03').text('Ocultar tabla consolidado de maquinas');
        $('#btn_tbl04').text('Ocultar tabla consolidado de maquinas');
        $('#div_general01').hide(1000);
        $('#div_general02').show(1000);
    } else {
        $('#txt_tb0').text('0');
        $('#btn_tbl01').text('Ver tabla consolidado de maquinas');
        $('#btn_tbl02').text('Ver tabla consolidado de maquinas');
        $('#btn_tbl03').text('Ver tabla consolidado de maquinas');
        $('#btn_tbl04').text('Ver tabla consolidado de maquinas');
        $('#div_general01').show(1000);
        $('#div_general02').hide(1000);
    }    
}

function dgConsolidado(fini_, ffin_, turno, filtro) {

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.KpiUnidadesProducidasX,
        data: { fini_, ffin_, turno, filtro, sw01 }
    }).done(function (data) {

        var datos = [];
        var unidades = 'Unidades';
        var filtro_;
        var fecha_ = [];
        var seVe = false;

        for (var i = 0; i < data.length; i++) {

            if (data[i].unidades.substring(0,3) === "Día") {

                fecha_ = moment(data[i].fecha, 'DD/MM/YYYY').format('MM/DD/YYYY');

                tipoDato = 'date';
                formato = 'dd/MM/yyyy';
            }
            else if (data[i].unidades === "Fecha y hora") {
                fecha_ = moment(data[i].fecha, 'DD/MM/YYYY').format('MM/DD/YYYY');

                tipoDato = 'date';
                formato = 'dd/MM/yyyy';
                seVe = true;
                filtro_ = "Fecha_"
            }
            else {
                fecha_ = data[i].fecha;

                tipoDato = 'text';
                formato = '';
            }

            datos.push({
                "Filtro": data[i].unidades,
                "Fecha": fecha_,
                "Hora": data[i].hora,
                "En turno": data[i].enTurnox,
                "Extra turno": data[i].sinTurno,
                "Total": data[i].total,
                "Unidades": unidades,
                "Turno": data[i].turno,
                "Activo": data[i].cod_activo
            });
        }

        if (data[0].unidades === "Día") {
            if (data[0].turno === "" || data[0].turno === null) {
                filtro_ = "Día";
            }            
            else {
                filtro_ = "Día - Turno: " + data[0].turno;
            }
        }
        else if (data[0].unidades === "Fecha y hora") {
            filtro_ = "Fecha_";
        }
        else {
            filtro_ = data[0].unidades;
        }
         

        //console.log('Datos de respuesta:')
        //console.log(data);

         $("#gridConsolidado").dxDataGrid({
            dataSource: datos,
            allowColumnReordering: true,
             showBorders: true,
             allowColumnResizing: true,
            paging: {
                pageSize: 20
            },
            groupPanel: {
                visible: true
            },
            export: {
                enabled: true
            },
            onExporting: function (e) {
                var workbook = new ExcelJS.Workbook();
                var worksheet = workbook.addWorksheet('Main sheet');
                DevExpress.excelExporter.exportDataGrid({
                    worksheet: worksheet,
                    component: e.component,
                    customizeCell: function (options) {
                        var excelCell = options;
                        excelCell.font = { name: 'Arial', size: 12 };
                        excelCell.alignment = { horizontal: 'left' };
                    }
                }).then(function () {
                    workbook.xlsx.writeBuffer().then(function (buffer) {
                        saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
                    });
                });
                e.cancel = true;
            },
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [5, 10, 20],
                showInfo: true
            },
            columns: [
                
                {
                    caption: "Activo",
                    dataField: "Activo",
                },
                "Filtro",
                {
                    caption: "Unidades",
                    dataField: "Unidades",
                },
                {
                    caption: filtro_,
                    dataField: "Fecha", //"fecha",
                    dataType: tipoDato,
                    format: formato,
                }, 
                {
                    caption: "Hora",
                    dataField: "Hora",
                    format: {
                        type: "fixedPoint",
                        precision: 0
                    },
                    minWidth: 60,
                    visible: seVe,
                },
                {
                    caption: "En Turno",
                    dataField: "En turno",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                },
                {
                    caption: "Fuera de Turno",
                    dataField: "Extra turno", //"sinTurno",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                },
                {
                    caption: "Totales",
                    dataField: "Total",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                }
            ]

        });

        animacionWW('X', 3);

    });

    
}

$(function () {
    var now = new Date();

    $("#mes-ini").dxDateBox({
        type: "date",
        value: now, displayFormat: 'monthAndYear',
        calendarOptions: {
            maxZoomLevel: 'year',
            minZoomLevel: 'century',
        }
    });

    $("#mes-fin").dxDateBox({
        type: "date",
        value: now, displayFormat: 'monthAndYear',
        calendarOptions: {
            maxZoomLevel: 'year',
            minZoomLevel: 'century',
        }
    });

});