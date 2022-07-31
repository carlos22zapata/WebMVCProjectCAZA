
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
    var variable = valor['Variable'];
    var maquina = valor['Cod_activo'];

    //var fini_ = moment(document.getElementById('mes-ini').value + '-01', 'YYYY-MM-DD').format('YYYY-MM-DD');
    //var ffin_ = moment(document.getElementById('mes-fin').value + '-01').add(1, 'month').subtract(1, 'day').format('YYYY-MM-DD');

    var finiM = moment($("#mes-ini").dxDateBox("instance").option('value'), 'YYYY-MM-DD').format('YYYY-MM-DD').substring(0, 8) + '01';
    var ffinM = moment($("#mes-fin").dxDateBox("instance").option('value'), 'YYYY-MM-DD').format('YYYY-MM-DD').substring(0, 8) + '01';
    ffinM = moment(ffinM).add(1, 'month').subtract(1, 'day');
    ffinM = moment(ffinM, 'YYYY-MM-DD').format('YYYY-MM-DD');

    var verTiempo = document.getElementById("mesVer").innerHTML;
    verTiempo = verTiempo.substring(verTiempo.search("value") + 7, verTiempo.search("value") + 23);

    Historicos_variables3(idEmpresa, finiM, ffinM, "mes", 0, maquina, 0, verTiempo, variable);
    document.getElementById('alertaInicial').style.display = "none";
}

function obtenerSemana(idEmpresa) {

    var valor = $("#semanaMaquina").dxSelectBox("instance").option('value');
    var variable = valor['Variable'];
    var maquina = valor['Cod_activo'];

    var annoI = 0;
    var semanaI = 0;
    var annoF = 0;
    var semanaF = 0;
    var tur = '';

    var appName = navigator.userAgent.indexOf('Firefox');

    var s1 = document.getElementById('semana-ini').value; //.substring(6, 8);
    var s2 = document.getElementById('semana-fin').value; //.substring(6, 8);

    if (appName > -1) {

        annoI = $('#BoxIni_ano').dxNumberBox("instance").option('value');
        semanaI = $('#BoxIni').dxNumberBox("instance").option('value');
        annoF = $('#BoxFin_ano').dxNumberBox("instance").option('value');
        semanaF = $('#BoxFin').dxNumberBox("instance").option('value');
        tur = annoI + '-W' + semanaI + "|" + annoF + '-W' + semanaF;
    }
    else {

        annoI = parseInt(document.getElementById('semana-ini').value.substring(0, 4));
        semanaI = parseInt(document.getElementById('semana-ini').value.substring(6, 8));
        annoF = parseInt(document.getElementById('semana-fin').value.substring(0, 4));
        semanaF = parseInt(document.getElementById('semana-fin').value.substring(6, 8));
        tur = document.getElementById('semana-ini').value + "|" + document.getElementById('semana-fin').value;
    }   
        
    //var fini_ = moment().year(annoI).week(semanaI).day("Monday").startOf("day").format("DD-MM-YYYY");
    //var ffin_ = moment(moment().year(annoF).week(semanaF).day("Monday").startOf("day"), "DD-MM-YYYY").add(6, 'days').format("DD-MM-YYYY");

    var fini_ = moment().year(annoI).week(semanaI).day("Monday").startOf("day").format("DD-MM-YYYY");
    var ffin_ = moment(moment().year(annoF).week(semanaF).day("Monday").startOf("day"), "DD-MM-YYYY")
    ffin_ = ffin_.add(6, 'days').format("DD-MM-YYYY");

    var verTiempo = document.getElementById("semanaVer").innerHTML;
    verTiempo = verTiempo.substring(verTiempo.search("value") + 7, verTiempo.search("value") + 23);

    Historicos_variables3(idEmpresa, fini_, ffin_, "semana", tur, maquina, 0, verTiempo, variable);
    document.getElementById('alertaInicial').style.display = "none"; 
}

function obtenerDia(idEmpresa) {

    var valor = $("#diaMaquina").dxSelectBox("instance").option('value');
    var variable = valor['Variable'];
    var maquina = valor['Cod_activo'];

    var fini_ = document.getElementById("diaDesde").innerHTML;
    fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 17);
    //alert(fini_);
    var ffin_ = document.getElementById("diaHasta").innerHTML;
    ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 17);
    //alert(ffin_);
    var turno = $('#diaTurno').dxSelectBox("option", "value");

    var verTiempo = document.getElementById("diaVer").innerHTML;
    verTiempo = verTiempo.substring(verTiempo.search("value") + 7, verTiempo.search("value") + 23);

    Historicos_variables3(idEmpresa, fini_, ffin_, "dia", turno, maquina, 0, verTiempo, variable);
    document.getElementById('alertaInicial').style.display = "none";
}

function obtenerHora(idEmpresa) {

    var valor = $("#horaMaquina").dxSelectBox("instance").option('value');
    var variable = valor['Variable'];
    var maquina = valor['Cod_activo'];

    var fini_ = document.getElementById("horaDesde").innerHTML;
    fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 26);
    //alert(fini_);
    var ffin_ = document.getElementById("horaHasta").innerHTML;
    ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 26);

    var verTiempo = document.getElementById("horaVer").innerHTML;
    verTiempo = verTiempo.substring(verTiempo.search("value") + 7, verTiempo.search("value") + 23);

    Historicos_variables3(idEmpresa, fini_, ffin_, "hora", 0, maquina, 0, verTiempo, variable);
    document.getElementById('alertaInicial').style.display = "none";
}

function Historicos_variables3(idEmpresa, fini_, ffin_, filtro, turno, cod_activo, flag, verTiempo, variable) {

    $.ajax({
        type: "POST",
        //url: FactoryX.Urls.Historicos_variables3,
        url: FactoryX.Urls.IndicadorProductividadVelocidad,
        data: { idEmpresa, fini_, ffin_, filtro, turno, variable, cod_activo, verTiempo, sw01 }
    }).done(function (data) {

        if (data.length === 0) {

            animacionWW('C');

            Swal.fire({
                title: 'No se encontraron datos en este periodo de tiempo seleccionado',
                html: 'Escoja otro rango de tiempo',
                icon: 'info'
            });
        }
        else {
            IndicadorProductividadVelocidad(data);
        }

         //console.log(data);
        //Grafico_disponibilidad(data, fini_, ffin_, flag, verTiempo);
        
    }).fail(function () {
        Swal.fire({
            title: 'Error',
            html: 'Se ha encontrado un error en la consulta de los datos',
            icon: 'error'
        });
    });
}

function IndicadorProductividadVelocidad(data) {

    let productividad = [];
    let velocidad = [];
    let tiempoConjunto = [];
    var Ylabel = data[0].VerTiempo;
    let datos = [];
    var filtro_ = data[0].Filtro;
    var seVe = false;

    var fecha_;

    for (var i = 0; i < data.length; i++) {
        productividad[i] = data[i].Productividad;
        velocidad[i] = data[i].Velocidad;
        tiempoConjunto[i] = data[i].Leyenda;

        if (data[i].Filtro.substring(0,3) === "Día") {

            fecha_ = moment(data[i].Leyenda, 'DD/MM/YYYY').format('MM/DD/YYYY');

            tipoDato = 'datetime';
            formato = 'dd/MM/yyyy';
        }
        else if (data[i].Filtro === "Fecha y hora") {
            fecha_ = moment(data[i].Leyenda, 'DD/MM/YYYY').format('MM/DD/YYYY');

            tipoDato = 'datetime';
            formato = 'dd/MM/yyyy';

            seVe = true;
            filtro_ = "Fecha_"
        }
        else {
            fecha_ = data[i].Leyenda;

            tipoDato = 'text';
            formato = '';
        }

        datos.push({
            "Filtro": data[0].Filtro,
            "Fecha": fecha_,
            "Productividad": data[i].Productividad,
            "Velocidad": data[i].Velocidad,
            "Unidades": "Unidades",
            "Turno": data[i].Turno,
            "Activo": data[0].Activo,
            "Hora": data[i].Hora,
        });
        total_suma = 0;
    }

    series_char = [];
    series_char[0] = { type: 'column', name: "Productividad", data: productividad };
    series_char[1] = { type: 'column', name: "Velocidad", data: velocidad };

    Highcharts.chart('container', {
        title: {
            text: 'Análisis de productividad'
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

    animacionWW('C');

    //Tabla de datos

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
            //"Turno",
            "Productividad", //"Productividad real", 
            "Velocidad",
            "Unidades",
            "Activo"
        ]
    });
}

function Grafico_disponibilidad(data, fini_, ffin_, flag, verTiempo) {

    if (data.length === 0) {
        //alert("No se encontraron registros que mostrar"); //CAZA
        fin_carga();
        Swal.fire({
            title: 'No se encontraron registros que mostrar',
            html: 'Valide la selección de los datos usados.',
            icon: 'info'
        });

        animacionWW('X');
    }

    var factorX = data[0].nombreActivo;
    document.getElementById('Etiqueta_maquina').textContent = "Productividad - " + factorX;
    //document.getElementById('alertaInicial').textContent = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + fini_ + ', hasta: ' + ffin_;
    if (flag == 0) {
        //document.getElementById('alertaInicial').style.display == "none";
        $('#alertaInicial').hide(1000);
    }
    //creaTabla(data);

    //filtro = 
    tiempo1 = [];
    tiempo2 = [];
    tiempo3 = [];
    tiempo4 = [];
    tiempo5 = [];
    tiempoConjunto = [];
    tiempo_horario = [];
    tiempo1 = data[0].tiempo;
    tiempo2 = data[0].tiempo2;
    if (data[0].filtro !== "hora") {
        tiempo3 = data[0].tiempo3;
    }
    tiempo4 = data[0].tiempo4;
    tiempo5 = data[0].tiempo5;
    tiempo_horario = data[0].sku_conteo_total;
    tiempoConjunto = tiempo1.concat(tiempo2);

    if (data[0].filtro !== "hora") {
        tiempoConjunto = tiempoConjunto.concat(tiempo3);
    }

    tiempoConjunto = tiempoConjunto.concat(tiempo4);
    tiempoConjunto = tiempoConjunto.concat(tiempo5);
    tiempoConjunto = tiempoConjunto.filter((a, b) => tiempoConjunto.indexOf(a) === b);
   
    tiempoTotal = [];
    series = [];

    unidades = [];
    tpp = [];
    horario = [];
    to = [];
    horario_te = [];
    p_teorica = [];
    p_real = [];
    velocidad = [];
    var Ylabel = "";

    if (verTiempo === "Unidades por día") {
        Ylabel = "Unidades por día";
        //for (var j = 0; j < tiempoConjunto.length; j++) {
        for (var j = 0; j < tiempo5.length; j++) {
            if (tiempo1.indexOf(tiempoConjunto[j]) === -1) {
                unidades[j] = 0;
            }
            else {
                unidades[j] = data[0].data[tiempo1.indexOf(tiempoConjunto[j])];
            }
            if (tiempo2.indexOf(tiempoConjunto[j]) === -1) {
                tpp[j] = parseFloat(0.00);
            }
            else {
                tpp[j] = parseFloat((parseFloat(data[0].data2[tiempo2.indexOf(tiempoConjunto[j])]) / (60 * 24)));
            }
            if (data[0].filtro !== "hora") {
                if (tiempo3.indexOf(tiempoConjunto[j]) === -1) {
                    horario[j] = parseFloat(0.00);
                }
                else {
                    horario[j] = parseFloat((parseFloat(data[0].data3[tiempo3.indexOf(tiempoConjunto[j])]) / (60 * 24)));
                }
            }
            if (tiempo4.indexOf(tiempoConjunto[j]) === -1) {
                to[j] = parseFloat(0.00);
            }
            else {
                to[j] = parseFloat((parseFloat(data[0].data4[tiempo4.indexOf(tiempoConjunto[j])]) / (60 * 24)));
            }
            if (tiempo5.indexOf(tiempoConjunto[j]) === -1) {
                horario_te[j] = parseFloat(0.00);
            }
            else {
                horario_te[j] = parseFloat((parseFloat(data[0].data5[tiempo5.indexOf(tiempoConjunto[j])]) / (60 * 24)));
            }
        }
        if (data[0].filtro == "hora") {
            for (var j = 0; j < unidades.length; j++) {
                horario[j] = parseFloat((parseFloat(1.00) / 24));
            }
        }
    }
    else {
        Ylabel = "Unidades por hora";
        for (var j = 0; j < tiempo5.length; j++) {
            if (tiempo1.indexOf(tiempoConjunto[j]) == -1) {
                unidades[j] = 0;
            }
            else {
                unidades[j] = data[0].data[tiempo1.indexOf(tiempoConjunto[j])];
            }
            if (tiempo2.indexOf(tiempoConjunto[j]) == -1) {
                tpp[j] = parseFloat(0.00);
            }
            else {
                tpp[j] = parseFloat((parseFloat(data[0].data2[tiempo2.indexOf(tiempoConjunto[j])]) / 60));
            }
            if (data[0].filtro !== "hora") {
                if (tiempo3.indexOf(tiempoConjunto[j]) == -1) {
                    horario[j] = parseFloat(0.00);
                }
                else {
                    horario[j] = parseFloat((parseFloat(data[0].data3[tiempo3.indexOf(tiempoConjunto[j])]) / 60));
                }
            }
            if (tiempo4.indexOf(tiempoConjunto[j]) == -1) {
                to[j] = parseFloat(0.00);
            }
            else {
                to[j] = parseFloat((parseFloat(data[0].data4[tiempo4.indexOf(tiempoConjunto[j])]) / 60));
            }
            if (tiempo5.indexOf(tiempoConjunto[j]) == -1) {
                horario_te[j] = parseFloat(0.00);
            }
            else {
                horario_te[j] = parseFloat((parseFloat(data[0].data5[tiempo5.indexOf(tiempoConjunto[j])]) / 60));
            }
        }
        if (data[0].filtro == "hora") {
            for (var j = 0; j < unidades.length; j++) {
                horario[j] = parseFloat(1.00);
            }
        }
    }



    for (var j = 0; j < tiempo5.length; j++) {

        p_teorica[j] = parseFloat((unidades[j] / (horario_te[j] - tpp[j])).toFixed(2));    //esta se modificará
        p_real[j] = parseFloat((unidades[j] / (horario[j] - tpp[j])).toFixed(2));
        velocidad[j] = parseFloat((unidades[j] / to[j]).toFixed(2));
        if (isNaN(velocidad[j])) {
            velocidad[j] = parseFloat(0.00);
        }
        if (p_teorica[j] == 0 && p_real[j] == 0 && velocidad[j] == 0) {
            p_teorica.splice(p_teorica.indexOf(j), 1);
            p_real.splice(p_real.indexOf(j), 1);
            velocidad.splice(velocidad.indexOf(j), 1);
        }

    }

    //inTurno[0] = data[0].data[0];
    //outTurno[0] = data[0].data2[0];

    //grafico Barras
    series_char = [];
    //for (i = 0; i < series.length; i++) {        
    //    series_char[i] = { type: 'column', name: data[i].nombreActivo, data: series[i] };
    //}
    series_char[0] = { type: 'column', name: "Productividad", data: p_teorica };    
    series_char[1] = { type: 'column', name: "Velocidad", data: velocidad };
    //series_char[2] = { type: 'column', name: "Productividad real", data: p_real };


    Highcharts.chart('container', {
        title: {
            text: 'Análisis de productividad'
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

    animacionWW('C');

    var datos = [];
    //var objeto = {};

    var filtro_ = "";

    if (data[0].filtro === "dia") {
        if (data[0].sku[0] === "" || data[0].sku[0] === null || data[0].sku[0] === undefined) {
            filtro_ = "Día";
        }
        else {
            filtro_ = "Día - Turno: " + data[0].sku[0];
        }

    }  
    if (data[0].filtro === "mes")
        filtro_ = "Mes";
    if (data[0].filtro === "hora")
        filtro_ = "Fecha y hora";
    if (data[0].filtro === "semana")
        filtro_ = "Semana";


    for (var y = 0; y < tiempoConjunto.length; y++) {

        if (!isNaN(p_teorica[y])) {
            datos.push({
                "Filtro": filtro_,
                "Fecha": data[0].filtro === "dia" ? tiempoConjunto[y].substring(4, tiempoConjunto[y].length) : tiempoConjunto[y],
                "Productividad": p_teorica[y],
                //"Productividad real": p_real[y],
                "Velocidad": velocidad[y],
                "Unidades": Ylabel,
                "Turno": data[0].sku[y],
                "Activo": data[0].nombreActivo
            });
            total_suma = 0;
        }
        
    }


    //objeto = datos;

    var widthColTurno = (data[0].filtro === 'dia' ? 'auto' : '0');

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
        columns: [
            "Filtro",
            {
                caption: filtro_,
                dataField: "Fecha" 
            }
            ,
            //"Turno",
            "Productividad", //"Productividad real", 
            "Velocidad",
            "Unidades",
            "Activo"
        ]
    });


    fin_carga();

}

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

$(".botoncito4").click(function () {
    //$(this).addClass("cargando");
    animacionWW('A');
});

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
        animacionWW('A');

        //console.log(data.activo);
        //console.log(data.cod_plan);
        //console.log(data.sku);
        document.getElementById('alertaInicial').innerHTML = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + fini_ + ' hasta: ' + ffin_ + ', en el activo: ' + data[0].Cod_activo;
        Historicos_variables3(idEmpresa, fini_, ffin_, filtro, turno, data[0].Cod_activo, 1, "Min", data[0].Variable)
        //aler(data[0].id);
    });
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