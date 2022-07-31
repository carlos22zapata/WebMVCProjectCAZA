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

    var valor = $("#mesMaquina").dxSelectBox("instance").option('value');
    var variable = ""; //valor['Variable'];
    var maquina = ""; //valor['Cod_activo'];
    var tiempo = $("#mesVer").dxSelectBox("instance").option('value') === null ? "Minutos" : $("#mesVer").dxSelectBox("instance").option('value');

    try {
        variable = valor['Variable'];
        maquina = valor['Cod_activo'];

        var fini_ = moment(document.getElementById('mes-ini').value + '-01', 'YYYY-MM-DD').format('YYYY-MM-DD');
        var ffin_ = moment(document.getElementById('mes-fin').value + '-01').add(1, 'month').subtract(1, 'day').format('YYYY-MM-DD');

        var verTiempo = document.getElementById("mesVer").innerHTML;
        verTiempo = verTiempo.substring(verTiempo.search("value") + 7, verTiempo.search("value") + 10);

        if ($('#txt_tb0').text() === "0") {
            animacionWW('A', 1)
            animacionWW('A', 2)
            animacionWW('A', 3)
            animacionWW('A', 4)

            Historicos_variables2(idEmpresa, fini_, ffin_, "mes", 0, maquina, 0, verTiempo, variable);
        }
        else {

            animacionWW('X', 1)
            animacionWW('X', 2)
            animacionWW('X', 3)
            animacionWW('X', 4)

            animacionWW('A', 5);
            dgConsolidado(fini_, ffin_, null, "mes", tiempo, verTiempo);
        }

        document.getElementById('alertaInicial').style.display = "none";
    }
    catch {
        Swal.fire({
            title: 'Verifique los datos seleccionados',
            html: 'Debe escoger una maquina valida',
            icon: 'error'
        });

        animacionWW('X', 1)
        animacionWW('X', 2)
        animacionWW('X', 3)
        animacionWW('X', 4)
    }

    

}

function obtenerSemana(idEmpresa) {

    var valor = $("#semanaMaquina").dxSelectBox("instance").option('value');
    var variable = ""; //valor['Variable'];
    var maquina = ""; //valor['Cod_activo'];
    var tiempo = $("#semanaVer").dxSelectBox("instance").option('value') === null ? "Minutos" : $("#semanaVer").dxSelectBox("instance").option('value');

    try {
        variable = valor['Variable'];
        maquina = valor['Cod_activo'];

        //var prueba = document.getElementById('semana-ini').value;

        var s1 = document.getElementById('semana-ini').value.substring(6, 8);
        var s2 = document.getElementById('semana-fin').value.substring(6, 8);
                
        var fini_ = moment(document.getElementById('semana-ini').value.substring(0, 4) + "-" + moment(moment().isoWeek(s1).startOf("isoWeek")).format().substring(5, 7) + "-" + moment(moment().isoWeek(s1).startOf("isoWeek")).format().substring(8, 10), "YYYY-MM-DD").add(-6, 'days').format();
        var ffin_ = moment(document.getElementById('semana-fin').value.substring(0, 4) + "-" + moment(moment().isoWeek(s2).startOf("isoWeek")).format().substring(5, 7) + "-" + moment(moment().isoWeek(s2).startOf("isoWeek")).format().substring(8, 10)).format();

        //var fini_ = moment(moment().isoWeek(s1).startOf("isoWeek")).format();
        //var ffin_ = moment(moment().isoWeek(s2).endOf("isoWeek")).format();

        var tur = document.getElementById('semana-ini').value + "|" + document.getElementById('semana-fin').value;

        var verTiempo = document.getElementById("semanaVer").innerHTML;
        verTiempo = verTiempo.substring(verTiempo.search("value") + 7, verTiempo.search("value") + 10);

        if ($('#txt_tb0').text() === "0") {
            animacionWW('A', 1)
            animacionWW('A', 2)
            animacionWW('A', 3)
            animacionWW('A', 4)

            Historicos_variables2(idEmpresa, fini_, ffin_, "semana", tur, maquina, 0, verTiempo, variable);
        }
        else {

            animacionWW('X', 1)
            animacionWW('X', 2)
            animacionWW('X', 3)
            animacionWW('X', 4)

            animacionWW('A', 5);
            dgConsolidado(fini_, ffin_, tur, "semana", tiempo, verTiempo);
        } 

        document.getElementById('alertaInicial').style.display = "none";

    }
    catch (error) {

        console.log('########## error ##########');
        console.log(error);

        Swal.fire({
            title: 'Verifique los datos seleccionados',
            html: 'Debe escoger una maquina valida',
            icon: 'error'
        });

        animacionWW('X', 1)
        animacionWW('X', 2)
        animacionWW('X', 3)
        animacionWW('X', 4)
    }

    

}

function obtenerDia(idEmpresa) {

    animacionWW('A', 1)
    animacionWW('A', 2)
    animacionWW('A', 3)
    animacionWW('A', 4)

    var valor = $("#diaMaquina").dxSelectBox("instance").option('value');
    var variable = ""; //valor['Variable'];
    var maquina = ""; //valor['Cod_activo'];
    var tiempo = $("#diaVer").dxSelectBox("instance").option('value') === null ? "Minutos" : $("#diaVer").dxSelectBox("instance").option('value');

    try {
        variable = valor['Variable'];
        maquina = valor['Cod_activo'];

        var fini_ = document.getElementById("diaDesde").innerHTML;
        fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 17);
        //alert(fini_);
        var ffin_ = document.getElementById("diaHasta").innerHTML;
        ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 17);
        //alert(ffin_);
        var turno = $('#diaTurno').dxSelectBox("option", "value");

        var verTiempo = document.getElementById("diaVer").innerHTML;
        verTiempo = verTiempo.substring(verTiempo.search("value") + 7, verTiempo.search("value") + 10);

        if ($('#txt_tb0').text() === "0") {
            animacionWW('A', 1)
            animacionWW('A', 2)
            animacionWW('A', 3)
            animacionWW('A', 4)

            Historicos_variables2(idEmpresa, fini_, ffin_, "dia", turno, maquina, 0, verTiempo, variable);
        }
        else {

            animacionWW('X', 1)
            animacionWW('X', 2)
            animacionWW('X', 3)
            animacionWW('X', 4)

            animacionWW('A', 5);
            dgConsolidado(fini_, ffin_, turno, "dia", tiempo, verTiempo);
        }

        document.getElementById('alertaInicial').style.display = "none";
    }
    catch {
        Swal.fire({
            title: 'Verifique los datos seleccionados',
            html: 'Debe escoger una maquina valida',
            icon: 'error'
        });

        animacionWW('X', 1)
        animacionWW('X', 2)
        animacionWW('X', 3)
        animacionWW('X', 4)
    }
}

function obtenerHora(idEmpresa) {

    var valor = $("#horaMaquina").dxSelectBox("instance").option('value');
    var variable = ""; //valor['Variable'];
    var maquina = ""; //valor['Cod_activo'];
    var tiempo = $("#horaVer").dxSelectBox("instance").option('value') === null ? "Minutos" : $("#horaVer").dxSelectBox("instance").option('value');

    try {
        variable = valor['Variable'];
        maquina = valor['Cod_activo'];

        var fini_ = document.getElementById("horaDesde").innerHTML;
        fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 26);
        //alert(fini_);
        var ffin_ = document.getElementById("horaHasta").innerHTML;
        ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 26);

        var verTiempo = document.getElementById("horaVer").innerHTML;
        verTiempo = verTiempo.substring(verTiempo.search("value") + 7, verTiempo.search("value") + 10);

        if ($('#txt_tb0').text() === "0") {
            animacionWW('A', 1)
            animacionWW('A', 2)
            animacionWW('A', 3)
            animacionWW('A', 4)

            Historicos_variables2(idEmpresa, fini_, ffin_, "hora", 0, maquina, 0, verTiempo, variable);
        }
        else {

            animacionWW('X', 1)
            animacionWW('X', 2)
            animacionWW('X', 3)
            animacionWW('X', 4)

            animacionWW('A', 5);
            dgConsolidado(fini_, ffin_, null, "hora", tiempo);
        }

        document.getElementById('alertaInicial').style.display = "none";
    }
    catch {
        Swal.fire({
            title: 'Verifique los datos seleccionados',
            html: 'Debe escoger una maquina valida',
            icon: 'error'
        });

        animacionWW('X', 1)
        animacionWW('X', 2)
        animacionWW('X', 3)
        animacionWW('X', 4)
    }
}

function IndicadorAnalisisTiempos(idEmpresa, fini_, ffin_, filtro, turno, cod_activo, variable, verTiempo) {

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.IndicadorAnalisisTiempos,
        //url: FactoryX.Urls.GestionKPI_tiemposXX,
        data: { idEmpresa, fini_, ffin_, filtro, turno, cod_activo, variable, verTiempo, sw01 }
    }).done(function (data) {
        //console.log(data);
        if (data.length === 0) {
            animacionWW('C', 1)
            animacionWW('C', 2)
            animacionWW('C', 3)
            animacionWW('C', 4)

            Swal.fire({
                title: 'No se encontraron datos en este periodo de tiempo seleccionado',
                html: 'Escoja otro rango de tiempo',
                icon: 'info'
            });
        }
        else {
            GraficoBarras1(data);
            GraficoPie1(data);
            Tabla1(data);
            Tabla2(data, verTiempo);
        }        
    });

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.IndicadorTipoAnalisisTiempos,
        //url: FactoryX.Urls.GestionKPI_tiemposXX,
        data: { idEmpresa, fini_, ffin_, filtro, turno, cod_activo, variable, verTiempo, sw01 }
    }).done(function (data) {
        //console.log(data);
        if (data.length === 0) {
            animacionWW('C', 1);
            animacionWW('C', 2);
            animacionWW('C', 3);
            animacionWW('C', 4);
            $('#container2').hide();
            $('#container3').hide();
            //$('#noHayDatos').show();
            document.getElementById('noHayDatos').style.display = 'inline-table';
            document.getElementById('tablaRegistros3').style.display = 'none';
        }
        else {
            GraficoBarras2(data);
            $('#container2').show();
            $('#container3').show();
            //$('#noHayDatos').hide();
            document.getElementById('noHayDatos').style.display = 'none';
            document.getElementById('tablaRegistros3').style.display = 'inline-table';
        }
        
    });
} 

function Historicos_variables2(idEmpresa, fini_, ffin_, filtro, turno, cod_activo, flag, verTiempo, variable) {

    IndicadorAnalisisTiempos(idEmpresa, fini_, ffin_, filtro, turno, cod_activo, variable, verTiempo);

    //$.ajax({
    //    type: "POST",
    //    url: FactoryX.Urls.Historicos_variables2,
    //    //url: FactoryX.Urls.GestionKPI_tiemposXX,
    //    data: { idEmpresa, fini_, ffin_, filtro, turno, cod_activo, variable }
    //}).done(function (data) {

    //    Grafico_disponibilidad(data, fini_, ffin_, flag, verTiempo);        
    //});


}

function GraficoBarras1(data) {

    let to = [];
    let tpp = [];
    let tpnp = [];
    let tiempoConjunto = [];
    let noRegistrado = [];
    var Ylabel = data[0].Filtro;

    for (var i = 0; i < data.length; i++) {
        to[i] = data[i].To;
        tpp[i] = data[i].Tpp;
        tpnp[i] = data[i].Tpnp;
        noRegistrado[i] = data[i].NoRegistrados;
        tiempoConjunto[i] = data[i].Leyenda;
    }

    //grafico Barras
    series_char = [];
    series_char[0] = { type: 'column', name: "TO - Tiempo de ejecución", data: to };
    series_char[1] = { type: 'column', name: "TPP - Tiempo de paros planificados", data: tpp };
    series_char[2] = { type: 'column', name: "TPNP - Tiempo de paros no planificados", data: tpnp };
    series_char[3] = { type: 'column', name: "No registrado", data: noRegistrado };

    Highcharts.chart('container', {
        title: {
            text: 'Análisis de tiempos'
        },
        xAxis: {
            categories: tiempoConjunto
        },
        yAxis: {
            title: {
                text: Ylabel
            },
        },

        credits: {
            enabled: false
        },

        legend: {
            layout: 'vertical',
            align: 'center',
            verticalAlign: 'bottom'
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
        series: series_char,

        //{
        plotOptions: {
            series: {
                borderWidth: 0,
                dataLabels: {
                    enabled: true
                    //format: '{point.y:.2f}%'
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

    animacionWW('C', 1)
}

function GraficoPie1(data) {

    var to = 0;
    var tpp = 0;
    var tpnp = 0;
    var noRegistrado = 0;

    for (var i = 0; i < data.length; i++) {
        to = to + data[i].To;
        tpp = tpp + data[i].Tpp;
        tpnp = tpnp + data[i].Tpnp;
        noRegistrado = noRegistrado + data[i].NoRegistrados;
    }

    series_char_tiempo = [];
    series_char_tiempo[0] = { name: "TO - Tiempo de ejecución", y: to };
    series_char_tiempo[1] = { name: "TPP - Tiempo de paros planificados", y: tpp };
    series_char_tiempo[2] = { name: "TPNP - Tiempo de paros no planificados", y: tpnp };
    series_char_tiempo[3] = { name: "No registrado", y: noRegistrado };

    Highcharts.chart('container1', { //Grafico de PIE
        //chart: {
        //    //plotBackgroundColor: null,
        //    //plotBorderWidth: null,
        //    //plotShadow: false,
        //    type: 'pie'
        //},
        title: {
            text: 'Análisis de tiempos (porcentaje)'
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
            data: series_char_tiempo
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

    animacionWW('C', 2)
}

function Tabla1(data) {

    var filtro_ = data[0].Filtro;
    var datos = [];
    var fecha_;
    var seVe = false;

    for (var i = 0; i < data.length; i++) {
               

        if (data[i].Filtro.substring(0,3) === "Día") {
            fecha_ = moment(data[i].Leyenda, 'DD/MM/YYYY').format('DD/MM/YYYY');

            tipoDato = 'date';
            formato = 'dd/MM/yyyy';
        }
        else if (data[i].Filtro === "Fecha y hora") {
            fecha_ = moment(data[i].Leyenda, 'DD/MM/YYYY').format('DD/MM/YYYY');

            tipoDato = 'date';
            formato = 'dd/MM/yyyy';

            seVe = true;

            Hora = data[i].Hora;

            //filtro_ = "Fecha_";
        }
        else {
            fecha_ = data[y].tiempo[i];

            tipoDato = 'text';
            formato = '';
        }

        datos.push({
            "Filtro": filtro_,
            "Fecha": fecha_, //data[i].Leyenda,
            "TO": data[i].To,
            "TPP": data[i].Tpp,
            "TPNP": data[i].Tpnp,
            "No registrado": data[i].NoRegistrados,
            "Unidades": data[i].Unidades,
            "Turno": data[0].Cod_turno,
            "Activo": data[0].Activo,
            "Hora": data[i].Hora,
        });
        total_suma = 0;
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
        //customizeColumns: function (columns) {
        //    columns[2].width = widthColTurno;
        //},
        pager: {
            showPageSizeSelector: true,
            allowedPageSizes: [5, 10, 20],
            showInfo: true
        },
        columns:
            [
                "Filtro",
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
                    caption: "TO",
                    dataField: "TO",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                },
                {
                    caption: "TPP",
                    dataField: "TPP",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                },
                {
                    caption: "TPNP",
                    dataField: "TPNP",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                },
                {
                    caption: "No registrado",
                    dataField: "No registrado",
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

function Tabla2(data) {

    var to = 0;
    var tpp = 0;
    var tpnp = 0;
    var noRegistrado = 0;
    let datos2 = [];

    for (var i = 0; i < data.length; i++) {
        to = to + data[i].To;
        tpp = tpp + data[i].Tpp;
        tpnp = tpnp + data[i].Tpnp;
        noRegistrado = noRegistrado + data[i].NoRegistrados;
    }

    series_char_tiempo = [];
    series_char_tiempo[0] = { name: "TO - Tiempo de ejecución", y: to };
    series_char_tiempo[1] = { name: "TPP - Tiempo de paros planificados", y: tpp };
    series_char_tiempo[2] = { name: "TPNP - Tiempo de paros no planificados", y: tpnp };
    series_char_tiempo[3] = { name: "No registrado", y: noRegistrado };

    var total_tiempos = 0;

    for (var y = 0; y < series_char_tiempo.length; y++) {
        total_tiempos = total_tiempos + series_char_tiempo[y].y;
        //objeto2 = series_char_sku;
    }

    for (var y = 0; y < series_char_tiempo.length; y++) {        
        var aux = (series_char_tiempo[y].y).toFixed(2);
        
        datos2.push({
            "Tipo tiempo": series_char_tiempo[y].name,
            "Tiempo": aux,
            "Porcentaje": ((series_char_tiempo[y].y * 100) / total_tiempos).toFixed(2)
        });
    }

    $("#tablaRegistros2").dxDataGrid({
        dataSource: datos2,
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
        columns: ["Tipo tiempo", "Tiempo", "Porcentaje"]
    });
}

function GraficoBarras2(data) { //Graficos de barras de la esquina inferior izquierda y derecha, contiene los dos gráficos
    let series_char3 = [];
    let datos = [];
    var Ylabel = "Minutos";
    let tiempoConjunto = data[0].Leyenda;

    for (var i = 0; i < data.length; i++) {
        //series_char3[i] = { name: data[i].Des_tipo, data: datos, stack: data[i].Tipo };
        series_char3[i] = { name: data[i].Des_tipo, data: data[i].Valor, stack: data[i].Tipo }
    }

    //Grafico de la esquina inferior derecha

    Highcharts.chart('container2', {
        credits: {
            enabled: false
        },
        title: {
            text: 'Análisis de tiempos de paro'
        },

        xAxis: {
            categories: tiempoConjunto
        },

        yAxis: {
            allowDecimals: false,
            min: 0,
            title: {
                text: Ylabel
            }
        },

        tooltip: {
            formatter: function () {
                return '<b>' + this.series.userOptions.stack + '</b><br/>' +
                    this.series.name + ': ' + this.y + '<br/>' +
                    'Total: ' + this.point.stackTotal;
            },
        },

        plotOptions: {
            column: {
                stacking: 'normal'
            }
            //,series: {
            //    dataLabels: {
            //        enabled: true
            //    }
            //}
        },

        series: series_char3,
        colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
            '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
        chart: {
            type: 'column',
            backgroundColor: null,
            style: {
                fontFamily: 'Dosis, sans-serif'
            }
        }
    });

    animacionWW('C', 3)

    //Grafico de la esquina inferior izquierda

    Highcharts.chart('container3', {
        credits: {
            enabled: false
        },
        title: {
            text: 'Análisis de tiempos de paro (porcentaje)'
        },

        xAxis: {
            categories: tiempoConjunto
        },

        yAxis: {
            allowDecimals: false,
            min: 0,
            title: {
                text: 'Porcentaje'
            }
        },

        tooltip: {
            formatter: function () {
                return '<b>' + this.series.userOptions.stack + '</b><br/>' +
                    this.series.name + ': ' + this.point.percentage.toFixed(2) + '%';
            }

        },

        plotOptions: {
            column: {
                stacking: 'percent'
            }
        },

        series: series_char3,
        colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
            '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
        chart: {
            type: 'column',
            backgroundColor: null,
            style: {
                fontFamily: 'Dosis, sans-serif'
            }
        }
    });

    animacionWW('C', 4)

    Tabla3(data);
}

function Tabla3(data) { //Analisis de paros, ultima tabla
    //Tabla Análisis de tiempos de paro

    let datos = [];
    var filtro_ = data[0].Filtro;
    var seVe = false;

    for (var i = 0; i < data.length; i++) {

        var recorrido = data[i].Leyenda.length;

        var fecha_;
        var hora_

        for (var y = 0; y < recorrido; y++) {
            //var fecha = data[i].Leyenda[y];
            var tipo = data[i].Tipo;
            var descripcion = data[i].Des_tipo;
            var tiempo = data[i].Valor[y];
            var porcentaje = data[i].Valor[y];

            if (data[i].Filtro.substring(0,3) === "Día") {

                fecha_ = moment(data[i].Leyenda[y], 'DD/MM/YYYY').format('MM/DD/YYYY');

                tipoDato = 'datetime';
                formato = 'dd/MM/yyyy';
            }
            else if (data[i].Filtro === "Fecha y hora") {
                fecha_ = moment(data[i].Leyenda[y], 'DD/MM/YYYY').format('MM/DD/YYYY');

                tipoDato = 'datetime';
                formato = 'dd/MM/yyyy';

                seVe = true;

                hora_ = data[i].Hora[y]

                filtro_ = "Fecha_";
            }
            else {
                fecha_ = data[i].Leyenda[y];

                tipoDato = 'text';
                formato = '';

                
            }

            datos.push({
                "Filtro": data[i].Filtro,
                "Fecha": fecha_,
                "Tipo tiempo": tipo,
                "Descripción": descripcion,
                "Tiempo": tiempo,
                "Porcentaje": porcentaje,
                "Hora": data[i].Hora[y]
            });
        }        
    }

    $("#tablaRegistros3").dxDataGrid({
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
            "Filtro",
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
            "Tipo tiempo",
            "Descripción",
            {
                caption: "Tiempo",
                dataField: "Tiempo",
                format: {
                    type: "fixedPoint",
                    precision: 2
                },
            }
            //"Tiempo"//,
            ////"Porcentaje"
        ]
    });
}

//#region Graficos antiguos

//function Grafico_disponibilidad(data, fini_, ffin_, flag, verTiempo) {

//    if (data.length === 0) {
//        //alert("No se encontraron registros que mostrar"); //CAZA
//        //fin_carga();
//        Swal.fire({
//            title: 'No se encontraron registros que mostrar',
//            html: 'Valide la selección de los datos usados.',
//            icon: 'info'
//        });

//        animacionWW('X', 1)
//        animacionWW('X', 2)
//        animacionWW('X', 3)
//        animacionWW('X', 4)
//    }

//    var filtro_ = "";

//    if (data[0].filtro === "dia") {
//        if (data[0].sku[0] === "" || data[0].sku[0] === null || data[0].sku[0] === undefined) {
//            filtro_ = "Día";
//        }
//        else {
//            filtro_ = "Día - Turno: " + data[0].sku[0];
//        }

//    }  
//    if (data[0].filtro === "mes")
//        filtro_ = "Mes";
//    if (data[0].filtro === "hora")
//        filtro_ = "Fecha y hora";
//    if (data[0].filtro === "semana")
//        filtro_ = "Semana";

//    var factorX = data[0].nombreActivo;
//    document.getElementById('Etiqueta_maquina').textContent = "Análisis de tiempos - " + factorX;
//    document.getElementById('alertaInicial').textContent = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + fini_ + ', hasta: ' + ffin_;
//    if (flag == 0) {
//        //document.getElementById('alertaInicial').style.display == "none";
//        $('#alertaInicial').hide(1000);
//    }
     
//    tiempo1 = [];
//    tiempo2 = [];
//    tiempo3 = [];
//    tiempoConjunto = [];
//    tiempo_horario = [];
//    tiempo1 = data[0].tiempo;
//    tiempo2 = data[0].tiempo2;
//    tiempo3 = data[0].tiempo3;
//    tiempo_horario = data[0].sku_conteo_total;
//    tiempoConjunto = tiempo1.concat(tiempo2);
//    tiempoConjunto = tiempoConjunto.concat(tiempo3);
//    tiempoConjunto = tiempoConjunto.filter((a, b) => tiempoConjunto.indexOf(a) === b);
//    //tiempoConjunto = tiempoConjunto.sort();

//    tiempoTotal = [];
//    series = [];

//    to = [];
//    tpp = [];
//    tpnp = [];
//    no_registrado = [];
//    var Ylabel = "";

//    if (verTiempo === "Hor") {
//        Ylabel = "Horas";
//        for (var j = 0; j < tiempoConjunto.length; j++) {
//            if (tiempo1.indexOf(tiempoConjunto[j]) == -1) {
//                to[j] = parseFloat(0.00);
//                //to[j] = 0;
//            }
//            else {
//                to[j] = parseFloat((parseFloat(data[0].data[tiempo1.indexOf(tiempoConjunto[j])]) / 60).toFixed(2));
//                //to[j] = data[0].data[tiempo1.indexOf(tiempoConjunto[j])];
//            }
//            if (tiempo2.indexOf(tiempoConjunto[j]) == -1) {
//                tpp[j] = parseFloat(0.00);
//                //tpp[j] = 0;
//            }
//            else {
//                tpp[j] = parseFloat((parseFloat(data[0].data2[tiempo2.indexOf(tiempoConjunto[j])]) / 60).toFixed(2));

//                //tpp[j] = data[0].data2[tiempo2.indexOf(tiempoConjunto[j])];
//            }
//            if (tiempo3.indexOf(tiempoConjunto[j]) == -1) {
//                tpnp[j] = parseFloat(0.00);
//                //tpnp[j] = 0;
//            }
//            else {
//                tpnp[j] = parseFloat((parseFloat(data[0].data3[tiempo3.indexOf(tiempoConjunto[j])]) / 60).toFixed(2));
//                //tpnp[j] = data[0].data3[tiempo3.indexOf(tiempoConjunto[j])]
//            }
//        }

//        for (var j = 0; j < tiempoConjunto.length; j++) {
//            if (data[0].filtro == "hora") {
//                no_registrado[j] = parseFloat((parseFloat(1.00) - to[j] - tpp[j] - tpnp[j]).toFixed(2));
//                //no_registrado[j] = 60 - to[j] - tpp[j] - tpnp[j];
//            }
//            else {
//                no_registrado[j] = parseFloat(((parseFloat(tiempo_horario[j]) / 60) - to[j] - tpp[j] - tpnp[j]).toFixed(2));
//                //no_registrado[j] = tiempo_horario[j] - to[j] - tpp[j] - tpnp[j];
//            }
//        }
//    }
//    else {
//        Ylabel = "Minutos";
//        for (var j = 0; j < tiempoConjunto.length; j++) {
//            if (tiempo1.indexOf(tiempoConjunto[j]) == -1) {
//                //to[j] = parseFloat(0.00);
//                to[j] = 0;
//            }
//            else {
//                //to[j] = parseFloat((data[0].data[tiempo1.indexOf(tiempoConjunto[j])] / 60).toFixed(2));
//                to[j] = data[0].data[tiempo1.indexOf(tiempoConjunto[j])];
//            }
//            if (tiempo2.indexOf(tiempoConjunto[j]) == -1) {
//                //tpp[j] = parseFloat(0.00);
//                tpp[j] = 0;
//            }
//            else {
//                //tpp[j] = parseFloat((data[0].data2[tiempo2.indexOf(tiempoConjunto[j])] / 60).toFixed(2));
//                tpp[j] = data[0].data2[tiempo2.indexOf(tiempoConjunto[j])];
//            }
//            if (tiempo3.indexOf(tiempoConjunto[j]) == -1) {
//                //tpnp[j] = parseFloat(0.00);
//                tpnp[j] = 0;
//            }
//            else {
//                //tpnp[j] = parseFloat((data[0].data3[tiempo3.indexOf(tiempoConjunto[j])] / 60).toFixed(2));
//                tpnp[j] = data[0].data3[tiempo3.indexOf(tiempoConjunto[j])]
//            }
//        }

//        for (var j = 0; j < tiempoConjunto.length; j++) {
//            if (data[0].filtro == "hora") {
//                //no_registrado[j] = parseFloat(1.00) - to[j] - tpp[j] - tpnp[j];
//                no_registrado[j] = 60 - to[j] - tpp[j] - tpnp[j];
//            }
//            else {
//                //no_registrado[j] = parseFloat((tiempo_horario[j] / 60).toFixed(2)) - to[j] - tpp[j] - tpnp[j];

//                //CAZA aqui esta el problema detectado en tiempo_horario
//                no_registrado[j] = tiempo_horario[j] - to[j] - tpp[j] - tpnp[j];
//            }
//        }
//    }

//    ////grafico Barras
//    series_char = [];
//    series_char[0] = { type: 'column', name: "TO - Tiempo de ejecución", data: to };
//    series_char[1] = { type: 'column', name: "TPP - Tiempo de paros planificados", data: tpp };
//    series_char[2] = { type: 'column', name: "TPNP - Tiempo de paros no planificados", data: tpnp };
//    series_char[3] = { type: 'column', name: "No registrado", data: no_registrado };

//    //#region Gráfico 1 superior izquierdo Barras
    

//    //Highcharts.chart('container', {
//    //    title: {
//    //        text: 'Análisis de tiempos'
//    //    },
//    //    xAxis: {
//    //        categories: tiempoConjunto
//    //    },
//    //    yAxis: {
//    //        title: {
//    //            text: Ylabel
//    //        },
//    //        //plotLines: [{
//    //        //    id: 'limit-min',
//    //        //    color: '#FF0000',
//    //        //    dashStyle: 'ShortDash',
//    //        //    width: 2,
//    //        //    value: 100,
//    //        //    zIndex: 0,
//    //        //    label: {
//    //        //        text: 'Máximo teórico'
//    //        //    }
//    //        //}]

//    //    },

//    //    credits: {
//    //        enabled: false
//    //    },

//    //    legend: {
//    //        layout: 'vertical',
//    //        align: 'center',
//    //        verticalAlign: 'bottom'
//    //    },

//    //    labels: {
//    //        items: [{
//    //            //html: 'Disponibilidad de los activos',
//    //            style: {
//    //                left: '50px',
//    //                top: '18px',
//    //                color: ( // theme
//    //                    Highcharts.defaultOptions.title.style &&
//    //                    Highcharts.defaultOptions.title.style.color
//    //                ) || 'black'
//    //            }
//    //        }]
//    //    },
//    //    series: series_char,

//    //    //{
//    //    plotOptions: {
//    //        series: {
//    //            borderWidth: 0,
//    //            dataLabels: {
//    //                enabled: true
//    //                //format: '{point.y:.2f}%'
//    //            }
//    //        }
//    //    },

//    //    type: 'spline',
//    //    //name: 'Average',
//    //    //data: [3, 2.67, 3, 6.33, 3.33],
//    //    marker: {
//    //        lineWidth: 2,
//    //        lineColor: Highcharts.getOptions().colors[3],
//    //        fillColor: 'white'

//    //    },
//    //    colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
//    //        '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
//    //    chart: {
//    //        backgroundColor: null,
//    //        style: {
//    //            fontFamily: 'Dosis, sans-serif'
//    //        }
//    //    }
//    //});

//    //#endregion

//    //animacionWW('C', 1)

//    //Grafico pastel
//    series_char_tiempo = [];
 
//    add = function (arr) {
//        return arr.reduce((a, b) => a + b, 0);
//    };
//    var j = 0;
//    for (i = 0; i < series_char.length; i++) {
//        if (add(series_char[i].data) != 0) {
//            series_char_tiempo[j] = { name: series_char[i].name, y: add(series_char[i].data) };
//            j = j + 1;
//        }
//    }

//    //#region Grafico 2 superior derecho Pastel

//    //Highcharts.chart('container1', { //Grafico de PIE
//    //    //chart: {
//    //    //    //plotBackgroundColor: null,
//    //    //    //plotBorderWidth: null,
//    //    //    //plotShadow: false,
//    //    //    type: 'pie'
//    //    //},
//    //    title: {
//    //        text: 'Análisis de tiempos (porcentaje)'
//    //    },
//    //    tooltip: {
//    //        pointFormat: '{series.name}: <b>{point.percentage:.2f}%</b>'
//    //    },
//    //    accessibility: {
//    //        point: {
//    //            valueSuffix: '%'
//    //        }
//    //    },
//    //    credits: {
//    //        enabled: false
//    //    },
//    //    plotOptions: {
//    //        pie: {
//    //            allowPointSelect: true,
//    //            cursor: 'pointer',
//    //            dataLabels: {
//    //                enabled: true,
//    //                format: '<b>{point.name}</b>: {point.percentage:.2f} %'
//    //            }
//    //        }
//    //    },
//    //    series: [{
//    //        name: 'Valor',
//    //        colorByPoint: true,
//    //        data: series_char_tiempo
//    //    }],
//    //    //series: series_char,
//    //    colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
//    //        '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
//    //    chart: {
//    //        backgroundColor: null,
//    //        style: {
//    //            fontFamily: 'Dosis, sans-serif'
//    //        },
//    //        type: 'pie'
//    //    }
//    //});

//    //animacionWW('C', 2)

//    //#endregion

//    //division tiempos de paros 
//    series_char3 = [];
//    tpp_nombres = [];
//    tpp_valor = [];
//    tpp_fecha = [];
//    tpnp_nombres = [];
//    tpnp_valor = [];
//    tpnp_fecha = [];

//    tpp_nombres = data[0].tpp_nombres;
//    tpp_fecha = data[0].tpp_fecha;
//    tpp_valor = data[0].tpp_valor;
//    tpnp_nombres = data[0].tpnp_nombres;
//    tpnp_fecha = data[0].tpnp_fecha;
//    tpnp_valor = data[0].tpnp_valor;


//    if (verTiempo == "Hor") {
//        aux_externo = 0;
//        aux = 0;
//        for (i = 0; i < tpp_nombres.length; i++) {
//            valor_temporal = [];
//            for (j = 0; j < tiempoConjunto.length; j++) {
//                for (k = 0; k < tpp_fecha[i].length; k++) {

//                    if (tiempoConjunto[j] == tpp_fecha[i][j]) {
//                        valor_temporal[j] = parseFloat((parseFloat(tpp_valor[i][j]) / 60).toFixed(2));
//                        break;
//                    }
//                    else {
//                        valor_temporal[j] = parseFloat(0.00);
//                    }

//                }
//            }
//            series_char3[aux_externo] = { name: tpp_nombres[aux], data: valor_temporal, stack: 'TPP' };
//            aux = aux + 1;
//            aux_externo = aux_externo + 1;
//        }
//        aux = 0;
//        for (i = 0; i < tpnp_nombres.length; i++) {
//            valor_temporal = [];
//            for (j = 0; j < tiempoConjunto.length; j++) {
//                for (k = 0; k < tpnp_fecha[i].length; k++) {

//                    if (tiempoConjunto[j] == tpnp_fecha[i][j]) {
//                        valor_temporal[j] = parseFloat((parseFloat(tpnp_valor[i][j]) / 60).toFixed(2));
//                        break;
//                    }
//                    else {
//                        valor_temporal[j] = parseFloat(0.00);
//                    }

//                }
//            }
//            series_char3[aux_externo] = { name: tpnp_nombres[aux], data: valor_temporal, stack: 'TPNP' };
//            aux = aux + 1;
//            aux_externo = aux_externo + 1;
//        }
//    }
//    else {
//        aux_externo = 0;
//        aux = 0;
//        for (i = 0; i < tpp_nombres.length; i++) {
//            valor_temporal = [];
//            for (j = 0; j < tiempoConjunto.length; j++) {
//                for (k = 0; k < tpp_fecha[i].length; k++) {

//                    if (tiempoConjunto[j] == tpp_fecha[i][j]) {
//                        valor_temporal[j] = parseInt(tpp_valor[i][j]);
//                        break;
//                    }
//                    else {
//                        valor_temporal[j] = 0;
//                    }

//                }
//            }
//            series_char3[aux_externo] = { name: tpp_nombres[aux], data: valor_temporal, stack: 'TPP' };
//            aux = aux + 1;
//            aux_externo = aux_externo + 1;
//        }
//        aux = 0;
//        for (i = 0; i < tpnp_nombres.length; i++) {
//            valor_temporal = [];
//            for (j = 0; j < tiempoConjunto.length; j++) {
//                for (k = 0; k < tpnp_fecha[i].length; k++) {

//                    if (tiempoConjunto[j] == tpnp_fecha[i][j]) {
//                        valor_temporal[j] = parseInt(tpnp_valor[i][j]);
//                        break;
//                    }
//                    else {
//                        valor_temporal[j] = 0;
//                    }

//                }
//            }
//            series_char3[aux_externo] = { name: tpnp_nombres[aux], data: valor_temporal, stack: 'TPNP' };
//            aux = aux + 1;
//            aux_externo = aux_externo + 1;
//        }
//    }

//    if (series_char3.length == 0) {
//        series_char3[0] = { name: 'No existen registros de paradas en el periodo', data: 0, stack: 'No hay datos' };
//    }

//    //#region Grafico 3 de barras esquina inferior izquierda

//    //Highcharts.chart('container2', {
//    //    credits: {
//    //        enabled: false
//    //    },
//    //    title: {
//    //        text: 'Análisis de tiempos de paro'
//    //    },

//    //    xAxis: {
//    //        categories: tiempoConjunto
//    //    },

//    //    yAxis: {
//    //        allowDecimals: false,
//    //        min: 0,
//    //        title: {
//    //            text: Ylabel
//    //        }
//    //    },

//    //    tooltip: {
//    //        formatter: function () {
//    //            return '<b>' + this.series.userOptions.stack + '</b><br/>' +
//    //                this.series.name + ': ' + this.y + '<br/>' +
//    //                'Total: ' + this.point.stackTotal;
//    //        },
//    //    },

//    //    plotOptions: {
//    //        column: {
//    //            stacking: 'normal'
//    //        }
//    //    },

//    //    series: series_char3,
//    //    colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
//    //        '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
//    //    chart: {
//    //        type: 'column',
//    //        backgroundColor: null,
//    //        style: {
//    //            fontFamily: 'Dosis, sans-serif'
//    //        }
//    //    }
//    //});

//    //animacionWW('C', 3)

//    //#endregion

//    //#region Grafico 4 esquina inferior derecha

//    //Highcharts.chart('container3', {
//    //    credits: {
//    //        enabled: false
//    //    },
//    //    title: {
//    //        text: 'Análisis de tiempos de paro (porcentaje)'
//    //    },

//    //    xAxis: {
//    //        categories: tiempoConjunto
//    //    },

//    //    yAxis: {
//    //        allowDecimals: false,
//    //        min: 0,
//    //        title: {
//    //            text: 'Porcentaje'
//    //        }
//    //    },

//    //    tooltip: {
//    //        formatter: function () {
//    //            return '<b>' + this.series.userOptions.stack + '</b><br/>' +
//    //                this.series.name + ': ' + this.point.percentage.toFixed(2) + '%';
//    //        }

//    //    },

//    //    plotOptions: {
//    //        column: {
//    //            stacking: 'percent'
//    //        }
//    //    },

//    //    series: series_char3,
//    //    colors: ['#7cb5ec', '#f7a35c', '#90ee7e', '#7798BF', '#aaeeee', '#ff0066',
//    //        '#eeaaee', '#55BF3B', '#DF5353', '#7798BF', '#aaeeee'],
//    //    chart: {
//    //        type: 'column',
//    //        backgroundColor: null,
//    //        style: {
//    //            fontFamily: 'Dosis, sans-serif'
//    //        }
//    //    }
//    //});

//    //animacionWW('C', 4)

//    //#endregion

//    //para Tabla 
//    //var ppp = data.length;


//    var datos = [];
//    var datos2 = [];
//    var datos3 = [];
//    var objeto = {};
//    var objeto2 = {};
//    var objeto3 = {};
        
//    for (var y = 0; y < tiempoConjunto.length; y++) {

//        datos.push({
//            "Filtro": filtro_,
//            "Fecha": tiempoConjunto[y],
//            "TO": to[y],
//            "TPP": tpp[y],
//            "TPNP": tpnp[y],
//            "No registrado": no_registrado[y],
//            "Unidades": Ylabel,
//            "Turno": data[0].sku[y],
//            "Activo": data[0].nombreActivo
//        });
//        total_suma = 0;
//    }

//    objeto = datos;

//    //#region Tabla Registros 1

//    //var widthColTurno = (data[0].filtro === 'dia' ? 'auto' : '0');

//    //$("#tablaRegistros1").dxDataGrid({
//    //    dataSource: objeto,
//    //    allowColumnReordering: true,
//    //    showBorders: true,
//    //    paging: {
//    //        pageSize: 10
//    //    },
//    //    groupPanel: {
//    //        visible: true
//    //    },
//    //    export: {
//    //        enabled: true
//    //    },
//    //    onExporting: function (e) {
//    //        var workbook = new ExcelJS.Workbook();
//    //        var worksheet = workbook.addWorksheet('Main sheet');
//    //        DevExpress.excelExporter.exportDataGrid({
//    //            worksheet: worksheet,
//    //            component: e.component,
//    //            customizeCell: function (options) {
//    //                var excelCell = options;
//    //                excelCell.font = { name: 'Arial', size: 12 };
//    //                excelCell.alignment = { horizontal: 'left' };
//    //            }
//    //        }).then(function () {
//    //            workbook.xlsx.writeBuffer().then(function (buffer) {
//    //                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
//    //            });
//    //        });
//    //        e.cancel = true;
//    //    },
//    //    //customizeColumns: function (columns) {
//    //    //    columns[2].width = widthColTurno;
//    //    //},
//    //    pager: {
//    //        showPageSizeSelector: true,
//    //        allowedPageSizes: [5, 10, 20],
//    //        showInfo: true
//    //    },
//    //    columns:
//    //        [
//    //            "Filtro",
//    //            {
//    //                caption: filtro_,
//    //                dataField: "Fecha"
//    //            },
//    //            //"Turno",
//    //            "TO",
//    //            "TPP",
//    //            "TPNP",
//    //            "No registrado",
//    //            "Unidades",
//    //            "Activo"
//    //        ]
//    //});

//    //#endregion

//    //Tabla SKUS
//    var total_tiempos = 0;
//    for (var y = 0; y < series_char_tiempo.length; y++) {
//        total_tiempos = total_tiempos + series_char_tiempo[y].y;
//        //objeto2 = series_char_sku;
//    }

//    for (var y = 0; y < series_char_tiempo.length; y++) {
//        var aux = 0;
//        if (verTiempo == "Hor") {
//            aux = (series_char_tiempo[y].y).toFixed(2);
//        }
//        else {
//            aux = series_char_tiempo[y].y;
//        }

//        datos2.push({
//            //"Fecha": data[y].fecha[i],
//            "Tipo tiempo": series_char_tiempo[y].name,
//            "Tiempo": aux,
//            "Porcentaje": ((series_char_tiempo[y].y * 100) / total_tiempos).toFixed(2)
//            //"Total": inTurno[y] + outTurno[y]
//            //"Sku": data[y].sku[i]
//        });

//        //objeto2 = series_char_sku;
//    }

//    objeto2 = datos2;

//    //#region Tabla de Analisis de Tiempos

//    //Tabla de Análisis de tiempos (porcentaje)

//    //$("#tablaRegistros2").dxDataGrid({
//    //    dataSource: objeto2,
//    //    allowColumnReordering: true,
//    //    showBorders: true,
//    //    paging: {
//    //        pageSize: 10
//    //    },
//    //    groupPanel: {
//    //        visible: true
//    //    },
//    //    export: {
//    //        enabled: true
//    //    },
//    //    onExporting: function (e) {
//    //        var workbook = new ExcelJS.Workbook();
//    //        var worksheet = workbook.addWorksheet('Main sheet');
//    //        DevExpress.excelExporter.exportDataGrid({
//    //            worksheet: worksheet,
//    //            component: e.component,
//    //            customizeCell: function (options) {
//    //                var excelCell = options;
//    //                excelCell.font = { name: 'Arial', size: 12 };
//    //                excelCell.alignment = { horizontal: 'left' };
//    //            }
//    //        }).then(function () {
//    //            workbook.xlsx.writeBuffer().then(function (buffer) {
//    //                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
//    //            });
//    //        });
//    //        e.cancel = true;
//    //    },
//    //    pager: {
//    //        showPageSizeSelector: true,
//    //        allowedPageSizes: [5, 10, 20],
//    //        showInfo: true
//    //    },
//    //    columns: ["Tipo tiempo", "Tiempo", "Porcentaje"]
//    //});

//    //#endregion

//    //Tabla tiempos de paro

//    if (verTiempo == "Hor") {
//        for (var y = 0; y < tiempoConjunto.length; y++) {
//            for (var j = 0; j < tpp_nombres.length; j++) {
//                if (tpp_valor[j][y] != 0) {
//                    datos3.push({
//                        //"Fecha": data[y].fecha[i],
//                        "Fecha": tiempoConjunto[y],
//                        "Tipo tiempo": "TPP",
//                        "Descripción": tpp_nombres[j],
//                        "Tiempo": parseFloat((parseFloat(tpp_valor[j][y]) / 60).toFixed(2)),
//                        "Porcentaje": (((parseFloat(tpp_valor[j][y]) / 60).toFixed(2) * 100) / tpp[y]).toFixed(2),
//                        "Filtro": filtro_
//                        //"Total": inTurno[y] + outTurno[y]
//                        //"Sku": data[y].sku[i]
//                    });
//                }
//            }
//            for (var j = 0; j < tpnp_nombres.length; j++) {
//                if (tpnp_valor[j][y] != 0) {
//                    datos3.push({
//                        //"Fecha": data[y].fecha[i],
//                        "Fecha": tiempoConjunto[y],
//                        "Tipo tiempo": "TPNP",
//                        "Descripción": tpnp_nombres[j],
//                        "Tiempo": parseFloat((parseFloat(tpnp_valor[j][y]) / 60).toFixed(2)),
//                        "Porcentaje": (((parseFloat(tpnp_valor[j][y]) / 60).toFixed(2) * 100) / tpnp[y]).toFixed(2),
//                        "Filtro": filtro_
//                        //"Total": inTurno[y] + outTurno[y]
//                        //"Sku": data[y].sku[i]
//                    });
//                }
//            }
//            //objeto2 = series_char_sku;
//        }
//    }
//    else {
//        for (var y = 0; y < tiempoConjunto.length; y++) {
//            for (var j = 0; j < tpp_nombres.length; j++) {
//                if (tpp_valor[j][y] != 0) {
//                    datos3.push({
//                        //"Fecha": data[y].fecha[i],
//                        "Fecha": tiempoConjunto[y],
//                        "Tipo tiempo": "TPP",
//                        "Descripción": tpp_nombres[j],
//                        "Tiempo": tpp_valor[j][y],
//                        "Porcentaje": ((tpp_valor[j][y] * 100) / tpp[y]).toFixed(2),
//                        "Filtro": filtro_
//                        //"Total": inTurno[y] + outTurno[y]
//                        //"Sku": data[y].sku[i]
//                    });
//                }
//            }
//            for (var j = 0; j < tpnp_nombres.length; j++) {
//                if (tpnp_valor[j][y] != 0) {
//                    datos3.push({
//                        //"Fecha": data[y].fecha[i],
//                        "Fecha": tiempoConjunto[y],
//                        "Tipo tiempo": "TPNP",
//                        "Descripción": tpnp_nombres[j],
//                        "Tiempo": tpnp_valor[j][y],
//                        "Porcentaje": ((tpnp_valor[j][y] * 100) / tpnp[y]).toFixed(2),
//                        "Filtro": filtro_
//                        //"Total": inTurno[y] + outTurno[y]
//                        //"Sku": data[y].sku[i]
//                    });
//                }
//            }
//            //objeto2 = series_char_sku;
//        }
//    }

//    objeto3 = datos3;

//    ////Tabla Análisis de tiempos de paro
//    //$("#tablaRegistros3").dxDataGrid({
//    //    dataSource: objeto3,
//    //    allowColumnReordering: true,
//    //    showBorders: true,
//    //    paging: {
//    //        pageSize: 10
//    //    },
//    //    groupPanel: {
//    //        visible: true
//    //    },
//    //    export: {
//    //        enabled: true
//    //    },
//    //    onExporting: function (e) {
//    //        var workbook = new ExcelJS.Workbook();
//    //        var worksheet = workbook.addWorksheet('Main sheet');
//    //        DevExpress.excelExporter.exportDataGrid({
//    //            worksheet: worksheet,
//    //            component: e.component,
//    //            customizeCell: function (options) {
//    //                var excelCell = options;
//    //                excelCell.font = { name: 'Arial', size: 12 };
//    //                excelCell.alignment = { horizontal: 'left' };
//    //            }
//    //        }).then(function () {
//    //            workbook.xlsx.writeBuffer().then(function (buffer) {
//    //                saveAs(new Blob([buffer], { type: 'application/octet-stream' }), 'DataGrid.xlsx');
//    //            });
//    //        });
//    //        e.cancel = true;
//    //    },
//    //    pager: {
//    //        showPageSizeSelector: true,
//    //        allowedPageSizes: [5, 10, 20],
//    //        showInfo: true
//    //    },
//    //    columns: [
//    //        "Filtro",
//    //        {
//    //            caption: filtro_,
//    //            dataField: "Fecha"
//    //        },
//    //        "Tipo tiempo",
//    //        "Descripción",
//    //        "Tiempo",
//    //        "Porcentaje"
//    //    ]
//    //});

//    //fin_carga();

//}

//#endregion

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

function animacionWW(valor, num) {
    //var t1 = document.getElementById("progresBar1");
    //var t2 = document.getElementById("progresBar2");
    //var t3 = document.getElementById("progresBar3");
    //var t4 = document.getElementById("progresBar4");

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
            case 4:
                $('#progresBar4').show();
                break;
            case 5:
                $('#progresBar5').show();
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
            case 4:
                $('#progresBar4').hide();
                break;
            case 5:
                $('#progresBar5').hide();
                break;

            default:
        }
    }
}

$(document).ready(function () {

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
    Activo_al_cargar(idEmpresa, fini_, ffin_, filtro, turno);
});

function Activo_al_cargar(idEmpresa, fini_, ffin_, filtro, turno) {

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.Lista_activos2,
        data: idEmpresa
    }).done(function (data) {
        //console.log(data.tiempo);
        try {
            //alert.log(data.data.length);
        }
        catch{
            //alert('Error');
        }

        document.getElementById('alertaInicial').innerHTML = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + fini_ + ' hasta: ' + ffin_ + ', en el activo: ' + data[0].Cod_activo;

        //console.log(data.activo);
        //console.log(data.cod_plan);
        //console.log(data.sku);
        animacionWW('A', 1)
        animacionWW('A', 2)
        animacionWW('A', 3)
        animacionWW('A', 4)
        Historicos_variables2(idEmpresa, fini_, ffin_, filtro, turno, data[0].Cod_activo, 1, "Min", data[0].Variable)
        //aler(data[0].id);
    });
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

function dgConsolidado(fini_, ffin_, turno, filtro, tiempo, verTiempo) {

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.ConsolidadoKpiTiempos,
        data: { fini_, ffin_, turno, filtro, tiempo, verTiempo, sw01 }
    }).done(function (data) {

        if (data.length == 0) {
            Swal.fire({
                title: 'No se encontraron registros que mostrar',
                html: 'Valide la selección de los datos usados.',
                icon: 'info'
            });

            $('#progresBar5').hide();

        }

        var datos = [];
        var unidades = 'Unidades';
        var filtro_;
        var fecha_ = [];
        var seVe = false;

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
                "En turno": data[i].enTurnox,
                "Extra turno": data[i].sinTurno,
                "Total": data[i].total,
                "Unidades": tiempo,
                "Turno": data[i].turno,
                "Activo": data[i].nombreActivo,
                "TO": data[i].to,
                "TPNP": data[i].tpnp,
                "TPP": data[i].tpp,
                "NoRegistrados": data[i].noRegistrado,
                "Hora": data[i].hora
            });

        }

        if (filtro === "dia") {
            if (data[0].turno === "" || data[0].turno === null) {
                filtro_ = "Día";
            }
            else {
                filtro_ = "Día - Turno: " + data[0].turno;
            }
            
        }            
        if (filtro === "mes")
            filtro_ = "Mes";
        if (filtro === "hora")
            filtro_ = "Fecha_";
        if (filtro === "semana")
            filtro_ = "Semana";

        //var widthColTurno = 0;//filtro === 'dia' ? 'auto' : '0';

        console.log('Datos de respuesta:')
        console.log(data);

        $("#gridConsolidado").dxDataGrid({
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
            //customizeColumns: function (columns) {
            //    columns[3].width = widthColTurno;
            //},
            pager: {
                showPageSizeSelector: true,
                allowedPageSizes: [5, 10, 20],
                showInfo: true
            },
            //option: {
            //    timeZone: "America/Guayaquil",
            //},
            columns: [

                {
                    caption: "Activo",
                    dataField: "Activo",
                },     
                {
                    caption: "Filtro",
                    dataField: "Filtro",
                },
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
                    caption: "Unidades",
                    dataField: "Unidades",
                },
                {
                    caption: "TO",
                    dataField: "TO",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                },
                {
                    caption: "TPP",
                    dataField: "TPP",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                },
                {
                    caption: "TPNP",
                    dataField: "TPNP",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                },
                {
                    caption: "No Registrado",
                    dataField: "NoRegistrados",
                    format: {
                        type: "fixedPoint",
                        precision: 2
                    },
                }
            ]
        });

        //$("#gridConsolidado").option("timeZone", "Europe/Berlin");
        //$("#gridConsolidado").option("timeZone", "America/Buenos_Aires");

        animacionWW('X', 5);

    });


}