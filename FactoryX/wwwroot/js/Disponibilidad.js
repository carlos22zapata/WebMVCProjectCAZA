var sw01 = false;

var tipoDato = 'text';
var formato = '';

//Establece valor del swiche
function switch_valueChanged(data) {
    sw01 = $("#swich01").dxSwitch("instance").option("value");
}

function dateDiff(secondDate) {
    var diffInDay = Math.floor(Math.abs((new Date() - secondDate) / (24 * 60 * 60 * 1000)));
    return $("#age").text(diffInDay + " days");
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
        //document.getElementById('btnOM').textContent = "Ocultar tabla";

    }
    else {
        //t.style.display = "none";
        $('#mesFilter').hide(1000);
        document.getElementById("btnHora").disabled = false;
        document.getElementById("btnSemana").disabled = false;
        document.getElementById("btnDia").disabled = false;
        //document.getElementById('btnOM').textContent = "Mostrar tabla";
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
        //document.getElementById('btnOM').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#semanaFilter').hide(1000);
        document.getElementById("btnHora").disabled = false;
        document.getElementById("btnMes").disabled = false;
        document.getElementById("btnDia").disabled = false;
        //document.getElementById('btnOM').textContent = "Mostrar tabla";
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
        //document.getElementById('btnOM').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#diaFilter').hide(1000);
        document.getElementById("btnHora").disabled = false;
        document.getElementById("btnSemana").disabled = false;
        document.getElementById("btnMes").disabled = false;
        //document.getElementById('btnOM').textContent = "Mostrar tabla";
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
        //document.getElementById('btnOM').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#horaFilter').hide(1000);
        document.getElementById("btnSemana").disabled = false;
        document.getElementById("btnMes").disabled = false;
        document.getElementById("btnDia").disabled = false;
        //document.getElementById('btnOM').textContent = "Mostrar tabla";
    }
}

function obtenerMes(idEmpresa) {

    tipoDato = 'text';

    var sku = $("#SkuMes").dxSelectBox("instance").option('value'); 

    var finiM = moment($("#mes-ini").dxDateBox("instance").option('value'), 'YYYY-MM-DD').format('YYYY-MM-DD').substring(0, 8) + '01';
    var ffinM = moment($("#mes-fin").dxDateBox("instance").option('value'), 'YYYY-MM-DD').format('YYYY-MM-DD').substring(0, 8) + '01';
    ffinM = moment(ffinM).add(1, 'month').subtract(1, 'day');
    ffinM = moment(ffinM, 'YYYY-MM-DD').format('YYYY-MM-DD');

    //var finiM = moment(document.getElementById('mes-ini').value + '-01', 'YYYY-MM-DD').format('YYYY-MM-DD');
    //var ffinM = moment(document.getElementById('mes-fin').value + '-01').add(1, 'month').subtract(1, 'day').format('YYYY-MM-DD');
    
    animacionWW('A');
    //alert(ffin_);
    GetDisponibilidad(idEmpresa, finiM, ffinM, "mes", "", sku);

}

function obtenerSemana(idEmpresa) {

    tipoDato = 'text';

    var annoI = 0;
    var semanaI = 0;
    var annoF = 0;
    var semanaF = 0;

    var appName = navigator.userAgent.indexOf('Firefox');

    if (appName > -1) {

        annoI = $('#BoxIni_ano').dxNumberBox("instance").option('value');
        semanaI = $('#BoxIni').dxNumberBox("instance").option('value');
        annoF = $('#BoxFin_ano').dxNumberBox("instance").option('value');
        semanaF = $('#BoxFin').dxNumberBox("instance").option('value');
    }
    else {
        //var s1 = document.getElementById('semana-ini').value.substring(6, 8);
        //var s2 = document.getElementById('semana-fin').value.substring(6, 8);

        annoI = parseInt(document.getElementById('semana-ini').value.substring(0, 4));
        semanaI = parseInt(document.getElementById('semana-ini').value.substring(6, 8));
        annoF = parseInt(document.getElementById('semana-fin').value.substring(0, 4));
        semanaF = parseInt(document.getElementById('semana-fin').value.substring(6, 8));
    }

    var fini_ = moment().year(annoI).week(semanaI).day("Monday").startOf("day").format("DD-MM-YYYY");
    var ffin_ = moment(moment().year(annoF).week(semanaF).day("Monday").startOf("day"), "DD-MM-YYYY")
        ffin_ = ffin_.add(6, 'days').format("DD-MM-YYYY");

    var tur = document.getElementById('semana-ini').value + "|" + document.getElementById('semana-fin').value;

    sku = $("#SkuSemana").dxSelectBox("instance").option('value'); //document.getElementById('SkuSemana').value;

    animacionWW('A');
    GetDisponibilidad(idEmpresa, fini_, ffin_, "semana", tur, sku);

}

function obtenerDia(idEmpresa) {

    var fini_ = document.getElementById("diaDesde").innerHTML;
    fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 17);
    //alert(fini_);
    var ffin_ = document.getElementById("diaHasta").innerHTML;
    ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 17);
    //alert(ffin_);
    var turno = $('#diaTurno').dxSelectBox("option", "value");
        //document.getElementById("diaTurno").innerHTML;
    //turno = turno.substring(turno.search("value") + 7, turno.search("value") + 14);
    //alert(turno);  

    sku = $("#SkuDia").dxSelectBox("instance").option('value');

    animacionWW('A');
    GetDisponibilidad(idEmpresa, fini_, ffin_, "dia", turno, sku);
}

function obtenerHora(idEmpresa) {

    var fini_ = document.getElementById("horaDesde").innerHTML;
    fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 26);
    //alert(fini_);
    var ffin_ = document.getElementById("horaHasta").innerHTML;
    ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 26);
    //alert(ffin_);

    sku = $("#SkuHora").dxSelectBox("instance").option('value');

    animacionWW('A');
    GetDisponibilidad(idEmpresa, fini_, ffin_, "hora", "", sku);

}


function GetDisponibilidad(idEmpresa, fini_, ffin_, filtro, turno, sku, inicio) {

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.IndicadorDisponibilidad, //IndicadorDisponibilidad, //GetDisponibilidad
        data: { idEmpresa, fini_, ffin_, filtro, turno, sku, inicio, sw01}
    }).done(function (data) {
        //console.log(data.tiempo);

        if (data[0].activo.length === 0) {
            Swal.fire({
                title: 'No se encontraron registros que mostrar',
                html: 'Valide la selección de los datos usados.',
                icon: 'info'
            });
        }

        try {
            alert.log(data.data.length);
        }
        catch {
            //alert('Error');
        }

        if (inicio === false || inicio === undefined) {
            document.getElementById('alertaInicial').style.display = "none";
        }

        try {
            var fechaXX = moment((data[0].fecha[0]), 'DD/MM/YYYY').format('DD-MM-YYYY');
            //document.getElementById('alertaInicial').textContent = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + moment(data[0].fecha[0], 'DD/MM/YYYY').format('DD-MM-YYYY') + ', hasta: ' + moment(data[0].fecha[0], 'DD/MM/YYYY').add('days', 6).format('DD-MM-YYYY');
            Grafico_disponibilidad(data);
        }
        catch {
            Swal.fire({
                title: 'No se encontraron registros que mostrar',
                html: 'Valide la selección de los datos usados.',
                icon: 'info'
            });

            animacionWW('C');
        }
              
    });
}

function creaTabla(data) {

    var ppp = data.length;
    var fecha_ = [];
    var titulo = data[0].filtro;
    var seVe = false;

    if (titulo === "Fecha y hora") {
        titulo = "Fecha_";
    }

        
    var datos = [];
    var objeto = {};

    for (var y = 0; y < data.length; y++) {
        for (var i = 0; i < data[y].tiempo.length; i++) { //supuesto fallo
            var tiempo = data[y].tiempo[i];


            if (data[y].filtro.substring(0,3) === "Día") {
                fecha_ = moment(data[y].tiempo[i], 'DD/MM/YYYY').format('MM/DD/YYYY');

                tipoDato = 'date';
                formato = 'dd/MM/yyyy';
            }
            else if (data[y].filtro === "Fecha y hora") {
                //fecha_ = moment(data[y].tiempo[i], 'DD/MM/YYYY HH:mm').format('MM/DD/YYYY HH:mm');
                fecha_ = moment(data[y].tiempo[i], 'DD/MM/YYYY').format('MM/DD/YYYY');

                tipoDato = 'date';
                formato = 'dd/MM/yyyy';

                seVe = true;
            }
            else {
                fecha_ = data[y].tiempo[i];
            }

            datos.push({
                "Fecha": data[y].fecha[i],
                "Tiempo": fecha_,
                "Valor": data[y].data[i],
                "Activo": data[y].activo[i] + " - " + data[y].nombreActivo,
                "Cod_plan": data[y].cod_plan[i],
                "Sku": data[y].sku[i],
                "Filtro": data[y].filtro,
                "Hora": data[y].hora[i],
            });
        }
    }

    
    $("#tablaRegistros").dxDataGrid({
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

            columns: [
                //{
                //    caption: "Fecha",
                //    dataField: "Fecha",
                //},
                {
                    caption: "Filtro",
                    dataField: "Filtro",
                },
                {
                    caption: titulo,
                    dataField: "Tiempo",
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
                    caption: "Disponibilidad(%)",
                    dataField: "Valor",
                },
                {
                    caption: "Activo",
                    dataField: "Activo",
                }                
            ] 
    });

};

function Grafico_disponibilidad(data) {

    var collator = new Intl.Collator(undefined, { numeric: true, sensitivity: 'base' });
    //var myArray = ['Semana 1', 'Semana 11', 'Semana 2'];
    //console.log(myArray.sort(collator.compare));


    if (data.length === 0) {
        //alert("No se encontraron registros que mostrar"); //CAZA

        Swal.fire({
            title: 'No se encontraron registros que mostrar',
            html: 'Valide la selección de los datos usados.',
            icon: 'info'
        });
    }

    animacionWW('C');
    creaTabla(data);

    //tiempoTotal0 = [];
    tiempoTotal = [];
    series = [];

    for (var i = 0; i < data.length; i++) {        
            tiempoTotal = tiempoTotal.concat(data[i].tiempo);                
    }

    tiempoTotal = tiempoTotal.filter((a, b) => tiempoTotal.indexOf(a) === b);
    //tiempoTotal = tiempoTotal.sort(collator.compare);

    for (i = 0; i < data.length; i++) {
        series[i] = [];
        for (var j = 0; j < tiempoTotal.length; j++) {
            cont = 0;
            for (var k = 0; k < data[i].data.length; k++) {
                if (tiempoTotal[j] === data[i].tiempo[k]) {
                    series[i][j] = data[i].data[k];
                    break;
                } else {
                    cont = cont + 1;
                }
            }
            if (cont === data[i].data.length) {
                series[i][j] = 0;
            }
        }
    }

    //document.getElementById('alertaInicial').textContent = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + moment(data[0].fecha[0], 'DD/MM/YYYY').format('DD-MM-YYYY') + ', hasta: ' + moment(data[0].fecha[0], 'DD/MM/YYYY').add('days', 6).format('DD-MM-YYYY');

    series_char = [];
    for (i = 0; i < series.length; i++) {
        series_char[i] = { type: 'column', name: data[i].nombreActivo, data: series[i] };
    }

    //series_char = series_char.sort(collator.compare);

    Highcharts.chart('container', {
        title: {
            text: 'Disponibilidad(%)'
        },
        xAxis: {
            categories: tiempoTotal //.sort(collator.compare)
        },
        yAxis: {
            title: {
                text: 'Porcentaje de disponibilidada (%)'
            },
            plotLines: [{
                id: 'limit-min',
                color: '#FF0000',
                dashStyle: 'ShortDash',
                width: 2,
                value: 100,
                zIndex: 0,
                label: {
                    text: 'Máximo teórico'
                }
            }]

        },

        credits: {
            enabled: false
        },

        legend: {
            align: 'center',
            verticalAlign: 'bottom',
            x: 0,
            y: 0
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
        series: series_char, //.sort(collator.compare),

        //{
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true,
                    format: '{point.y:.2f}%'
                }
            }
        },

        type: 'spline',
        //name: 'Average',
        //data: [3, 2.67, 3, 6.33, 3.33],
        marker: {
            lineWidth: 2,
            lineColor: Highcharts.getOptions().colors[3],
            fillColor: 'white'

        },
        colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
            '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
        chart: {
            backgroundColor: null,
            style: {
                fontFamily: 'Dosis, sans-serif'
            }
        }
    });
}

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

    

    //alert(appName);

    animacionWW('A');

    var f = new Date();
    var Dfini = f.getDate() + "-" + (f.getMonth() + 1) + "-" + f.getFullYear();
    var Dffin = '01-01-1900';
    var inicio = true;
    var turno = null;
    var sku = null;
    var filtro = "dia";
    var idEmpresa = document.getElementById('Empresa').value;

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.fechas_iniciales, //IndicadorDisponibilidad, //GetDisponibilidad
        data: { Dfini }
    }).done(function (data) {
        document.getElementById('alertaInicial').innerHTML = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + moment(data[0].fini).format("DD/MM/YYYY") + ' hasta: ' + moment(data[0].ffin).format("DD/MM/YYYY");
        GetDisponibilidad(idEmpresa, Dfini, Dffin, filtro, turno, sku, inicio);
    });

    
    
});

function mostrarTabla() {
    var t = document.getElementById("divTabla");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#divTabla').show(1000);
        document.getElementById('MOTabla').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#divTabla').hide(1000);
        document.getElementById('MOTabla').textContent = "Mostrar tabla";
    }
}

function animacionWW(valor) {
    var t = document.getElementById("progresBar");
        
    if (valor === 'A') {
        $('#progresBar').show();
    }
    else {
        $('#progresBar').hide();
    }
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


