var activosSeleccionados = []; //Arreglo que uso para poder aplicar los filtros
var sw01 = false;

function dateDiff(secondDate) {
    var diffInDay = Math.floor(Math.abs((new Date() - secondDate) / (24 * 60 * 60 * 1000)));
    return $("#age").text(diffInDay + " days");
}

//Establece valor del swiche
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

    var sku = $("#SkuMes").dxSelectBox("instance").option('value'); //var sku = document.getElementById('SkuMes').value;
    //var finiM = moment(document.getElementById('mes-ini').value + '-01', 'YYYY-MM-DD').format('YYYY-MM-DD');
    //var ffinM = moment(document.getElementById('mes-fin').value + '-01').add(1, 'month').subtract(1, 'day').format('YYYY-MM-DD');

    var finiM = moment($("#mes-ini").dxDateBox("instance").option('value'), 'YYYY-MM-DD').format('YYYY-MM-DD').substring(0, 8) + '01';
    var ffinM = moment($("#mes-fin").dxDateBox("instance").option('value'), 'YYYY-MM-DD').format('YYYY-MM-DD').substring(0, 8) + '01';
    ffinM = moment(ffinM).add(1, 'month').subtract(1, 'day');
    ffinM = moment(ffinM, 'YYYY-MM-DD').format('YYYY-MM-DD');

    var activos = document.getElementById('alertaFiltrosMes').innerHTML.trim();
    var act = activos.substr(17, activos.length).split(",");

    //Agregue esto por que se estaba incluyendo como primer elemento un caracter en blanco en el array y esto me chocaba con el if (act[0].trim() !== "")
    if (act[0].trim() === "")
        act.splice(0, 1);

    if (act.length > 0) {
        if (act[0].trim() !== "activos:") {
            if (act[0].trim() !== "") {
                act = activos.substr(17, activos.length).split(",");
            }
            else {
                act = [];
            }
        }
        else {
            act = [];
        }
    }
        

    GetOEE(idEmpresa, finiM, ffinM, "mes", "", sku, act, sw01);

}

function obtenerSemana(idEmpresa) {

    var annoI = 0;
    var semanaI = 0;
    var annoF = 0;
    var semanaF = 0;
    var tur = '';

    var appName = navigator.userAgent.indexOf('Firefox');

    var s1 = document.getElementById('semana-ini').value.substring(6, 8);
    var s2 = document.getElementById('semana-fin').value.substring(6, 8);

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


    
    var sku = $("#SkuSemana").dxSelectBox("instance").option('value'); //sku = document.getElementById('SkuSemana').value;

    var activos = document.getElementById('alertaFiltrosSemana').innerHTML.trim();
    var act = activos.substr(17, activos.length).split(",");

    //Agregue esto por que se estaba incluyendo como primer elemento un caracter en blanco en el array y esto me chocaba con el if (act[0].trim() !== "")
    if (act[0].trim() === "")
        act.splice(0, 1);

    if (act.length > 0) {
        if (act[0].trim() !== "activos:") {
            if (act[0].trim() !== "") {
                act = activos.substr(17, activos.length).split(",");
            }
            else {
                act = [];
            }
        }
        else {
            act = [];
        }
    }

    var fini_;
    var ffin_;

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.FirstDateOfWeek,
        data: { annoI, semanaI, annoF, semanaF }
    }).done(function (data) {

        fini_ = moment(data[0].fecha1).format();
        ffin_ = moment(data[0].fecha2).format();

        GetOEE(idEmpresa, fini_, ffin_, "semana", tur, sku, act, sw01); 

    }).error(function (dat) {
        alert("Error");
    });

    //var fini_ = moment(document.getElementById('semana-ini').value.substring(0, 4) + "-" + moment(moment().isoWeek(s1).startOf("isoWeek")).format().substring(5, 7) + "-" + moment(moment().isoWeek(s1).startOf("isoWeek")).format().substring(8, 10), "YYYY-MM-DD").add(-6, 'days').format();
    //var ffin_ = moment(document.getElementById('semana-fin').value.substring(0, 4) + "-" + moment(moment().isoWeek(s2).startOf("isoWeek")).format().substring(5, 7) + "-" + moment(moment().isoWeek(s2).startOf("isoWeek")).format().substring(8, 10)).format();

    
       
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

    var activos = document.getElementById('alertaFiltrosDia').innerHTML.trim();
    var act = activos.substr(17, activos.length).split(",");

    //Agregue esto por que se estaba incluyendo como primer elemento un caracter en blanco en el array y esto me chocaba con el if (act[0].trim() !== "")
    if (act[0].trim() === "")
        act.splice(0, 1);

    if (act.length > 0) {
        if (act[0].trim() !== "activos:") {
            if (act[0].trim() !== "") {
                act = activos.substr(17, activos.length).split(",");
            }
            else {
                act = [];
            }   
        }
        else {
            act = [];
        }    
    }

    var sku = $("#SkuDia").dxSelectBox("instance").option('value'); //sku = document.getElementById('SkuDia').value;

    GetOEE(idEmpresa, fini_, ffin_, "dia", turno, sku, act, sw01);
}

function obtenerHora(idEmpresa) {
    var fini_ = document.getElementById("horaDesde").innerHTML;
    fini_ = fini_.substring(fini_.search("value") + 7, fini_.search("value") + 26);
    //alert(fini_);
    var ffin_ = document.getElementById("horaHasta").innerHTML;
    ffin_ = ffin_.substring(ffin_.search("value") + 7, ffin_.search("value") + 26);
    //alert(ffin_);

    var sku = $("#SkuHora").dxSelectBox("instance").option('value'); //sku = document.getElementById('SkuHora').value;

    var activos = document.getElementById('alertaFiltrosHora').innerHTML.trim();
    var act = activos.substr(17, activos.length).split(",");

    //Agregue esto por que se estaba incluyendo como primer elemento un caracter en blanco en el array y esto me chocaba con el if (act[0].trim() !== "")
    if (act[0].trim() === "")
        act.splice(0, 1);

    if (act.length > 0) {
        if (act[0].trim() !== "activos:") {
            if (act[0].trim() !== "") {
                act = activos.substr(17, activos.length).split(",");
            }
            else {
                act = [];
            }
        }
        else {
            act = [];
        }
    }

    GetOEE(idEmpresa, fini_, ffin_, "hora", "", sku, act, sw01);

}

function GetOEE(idEmpresa, fini_, ffin_, filtro, turno, sku, act, sw01) {

    var inicio = false;
    var Dfini = fini_;
    var Dffin = ffin_;
    document.getElementById('alertaInicial').style.display = "none";

    animacionWW('A');
    animacionWW2('A');

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.IndicadorInicio,
        data: { Dfini, Dffin, inicio, turno, sku, filtro, act, sw01 }
    }).done(function (data) {

        animacionWW('C');
        document.getElementById('porcDisponibilidad').textContent = data[0].Disp + ' %';
        document.getElementById('porcRendimiento').textContent = data[0].Rend + ' %';
        document.getElementById('porcOee').textContent = data[0].Oee + ' %';

        GraficoGauge(data[0].Oee);

    }).fail(function () {
        Swal.fire({
            title: 'No se encontraron registros que mostrar',
            html: 'Valide la selección de los datos usados.',
            icon: 'info'
        });

        animacionWW('C');
        animacionWW2('C');
    });

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.IndicadorAgrupadoOEE,
        data: { Dfini, Dffin, inicio, turno, sku, filtro, activosSeleccionados, act }
    }).done(function (data) {
        animacionWW2('C');
        //document.getElementById('porcDisponibilidad').textContent = data[0].Disp + ' %';
        //document.getElementById('porcRendimiento').textContent = data[0].Rend + ' %';
        //document.getElementById('porcOee').textContent = data[0].Oee + ' %';
        //document.getElementById('alertaInicial').textContent = 'Datos obtenidos del último día culminado de trabajo: ' + moment(data[0].Fecha).format('DD-MM-YYYY');

        //GraficoGauge(data[0].Oee);

        GraficoBarrasLineas(data);

    }).fail(function () {
        //Swal.fire({
        //    title: 'No se encontraron registros que mostrar',
        //    html: 'Valide la selección de los datos usados.',
        //    icon: 'info'
        //});

        animacionWW2('C');
    });
}

function GetDisponibilidad(idEmpresa, fini_, ffin_, filtro, turno, sku) {

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.GetDisponibilidad,
        data: { idEmpresa, fini_, ffin_, filtro, turno, sku }
    }).done(function (data) {
        //console.log(data.tiempo);
        try {
            alert.log(data.data.length);
        }
        catch{
            //alert('Error');
        }

        Grafico_disponibilidad(data);
        
    });
}

function creaTabla(data) {

    var ppp = data.length;

    var datos = [];
    var objeto = {};

    for (var y = 0; y < data.length; y++) {
        for (var i = 0; i < data[y].Disp.length; i++) { //supuesto fallo
            var Oee = data[y].Disp[i];

            datos.push({
                "Fecha": data[y].Fecha[i],
                "Disponibilidad": data[y].Disp[i],
                "Rendimiento": data[y].Rend[i],
                "OEE": data[y].Oee[i]


                //"Fecha": data[y].fecha[i],
                //"Tiempo": data[y].tiempo[i],
                //"Valor": data[y].data[i],
                //"Activo": data[y].activo[i],
                //"Cod_plan": data[y].cod_plan[i],
                //"Sku": data[y].sku[i]
            });
        }
    }



    objeto = datos;

    $("#tablaRegistros").dxDataGrid({
        dataSource: objeto,
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
        columns: ["Fecha", "Disponibilidad", "Rendimiento", "OEE"]
    });

};

function Grafico_disponibilidad(data) {

    if (data.length === 0) {
        //alert("No se encontraron registros que mostrar"); //CAZA

        Swal.fire({
            title: 'No se encontraron registros que mostrar',
            html: 'Valide la selección de los datos usados.',
            icon: 'info'
        });
    }

    creaTabla(data);

    tiempoTotal = [];
    series = [];


    //activo1 = data[0].activo;
    //activo2 = data[1].activo;
    for (var i = 0; i < data.length; i++) {
        tiempoTotal = tiempoTotal.concat(data[i].tiempo);
    }
    tiempoTotal = tiempoTotal.filter((a, b) => tiempoTotal.indexOf(a) === b);
    //tiempoTotal = tiempoTotal.sort();

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

    series_char = [];
    for (i = 0; i < series.length; i++) {
        series_char[i] = { type: 'column', name: data[i].nombreActivo, data: series[i] };
    }

    Highcharts.chart('container', {
        title: {
            text: 'Disponibilidad'
        },
        xAxis: {
            categories: tiempoTotal
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
        series: series_char,

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

//function setDivHeight() {
//    var div = $('#container-OEE');
//    div.height(div.width() * 0.75);    
//}

function GraficoGauge(porc) {

    var color = '#DF5353';

    //Asigno el color
    if (porc < 65) {
        color = '#E2482D';
    } else if (porc >= 65 && porc < 75) {
        color = '#DDDF0D';
    } else {
        color = '#009D71';
    }

    //setDivHeight();

    //[0.1, '#DF5353'], // red
    //    //[0.8, '#DDDF0D'], // yellow
    //    [0.999, '#55BF3B']  // green

    var gaugeOptions = {
        chart: {
            type: 'solidgauge'
        },

        title: null,

        pane: {
            center: ['45%', '85%'],
            size: '140%',
            startAngle: -90,
            endAngle: 90,
            background: {
                backgroundColor:
                    Highcharts.defaultOptions.legend.backgroundColor || '#EEE',
                innerRadius: '60%',
                outerRadius: '100%',
                shape: 'arc'
            }
        },

        exporting: {
            enabled: false
        },

        tooltip: {
            enabled: false
        },

        // the value axis
        yAxis: {
            stops: [
                [0.1, color]
            ],
            lineWidth: 0,
            tickWidth: 0,
            minorTickInterval: null,
            tickAmount: 2,
            title: {
                y: -70
            },
            labels: {
                y: 16
            }
        },

        plotOptions: {
            solidgauge: {
                dataLabels: {
                    y: 5,
                    borderWidth: 0,
                    useHTML: true
                }
            }
        }
    };

    // The speed gauge
    var chartSpeed = Highcharts.chart('container-OEE', Highcharts.merge(gaugeOptions, {
        yAxis: {
            min: 0,
            max: 100,
            title: {
                text: ''
            }
        },

        credits: {
            enabled: false
        },

        series: [{
            name: 'Speed',
            data: [porc],
            dataLabels: {
                format:
                    '<div style="text-align:center">' +
                    '<span style="font-size:25px">{y} %</span><br/>' +
                    '<span style="font-size:16px;opacity:0.4">% OEE</span>' +
                    '</div>'
            },
            tooltip: {
                valueSuffix: 'Cálculo OEE'
            }
        }]
    }));

}

function GraficoBarrasLineas(data) {

    //########################### A evaluar

    creaTabla(data);

    if (data.length === 0) {
        //alert("No se encontraron registros que mostrar"); //CAZA

        Swal.fire({
            title: 'No se encontraron registros que mostrar',
            html: 'Valide la selección de los datos usados.',
            icon: 'info'
        });
    }

    serieD = [];
    serieR = [];
    serieO = [];
    agrupacion = [];

    for (var i = 0; i < data.length; i++) {
        try {
            serieD.push(data[i].Disp);
            serieR.push(data[i].Rend);
            serieO.push(data[i].Oee);
            agrupacion.push(data[i].Agrupacion);
        }
        catch (error){
            console.error(error);
            alert(error);
        }
        
    }

    Highcharts.chart('containerBL', {
		chart: {
			zoomType: 'xy'
		},
		title: {
			text: 'Tendencia OEE',
			align: 'center'
		},
		credits: {
			enabled: false //Quita el link de Highcharts
		},
		
		xAxis: [{
            categories: agrupacion
		}],
		
		//Esto representa el eje de las y inicio
		yAxis: [
		{ // Primary yAxis			
			title: {
				text: 'Porcentaje (%)'				
			}
		}
		,{title:null},{title:null}],
		//Esto representa el eje de las y fin
		
		
		//Esto representa el eje de las x inicio
		series: [		
			{
				name: 'Disponibilidad',
				type: 'column',
				yAxis: 1,
				data: serieD,				
				color: '#fbb040'
							
			}, 
			
			{
				name: 'Rendimiento',
				type: 'column',
				yAxis: 1,
				data: serieR,	
				color: '#7ebdb4'
			}, 
			
			{
				name: 'OEE',
				type: 'spline',
				yAxis: 1,
                data: serieO,
				color: '#de703c'				
			}		
		]
		//Esto representa el eje de las x fin
		
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

    animacionWW('A');
    animacionWW2('A');

    var f = new Date();
    var Dfini = f.getDate() + "-" + (f.getMonth() + 1) + "-" + f.getFullYear();
    var Dffin = '01-01-1900';
    var inicio = true;
    var turno = null;
    var sku = null;
    var filtro = "dia";

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.IndicadorInicio,
        data: { Dfini, Dffin, inicio, turno, sku, filtro, activosSeleccionados }
    }).done(function (data) {
        animacionWW('C');
        document.getElementById('porcDisponibilidad').textContent = data[0].Disp + ' %';
        document.getElementById('porcRendimiento').textContent = data[0].Rend + ' %';
        document.getElementById('porcOee').textContent = data[0].Oee + ' %';
        document.getElementById('alertaInicial').textContent = 'Datos obtenidos de la última semana completa de trabajo, desde: ' + moment(data[0].Fecha).format('DD-MM-YYYY') + ', hasta: ' + moment(data[0].Fecha).add('days', 6).format('DD-MM-YYYY');
                
        GraficoGauge(data[0].Oee);

    }).fail(function () {
        Swal.fire({
            title: 'No hay datos que mostrar',
            html: 'Valide la selección de los datos usados.',
            icon: 'info'
        });

        animacionWW2('C');
    });
    
    $.ajax({
        type: "POST",
        url: FactoryX.Urls.IndicadorAgrupadoOEE,
        data: { Dfini, Dffin, inicio, turno, sku, filtro, activosSeleccionados }
    }).done(function (data) {
        animacionWW2('C');
        GraficoBarrasLineas(data);
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

//function animacionWW() {
//    var t = document.getElementById("progresBar");

//    if (t.style.display === "none") {
//        //t.style.display = "inline-table";
//        $('#progresBar').show();
//        $('#indicador1').hide();
//        //document.getElementById('MOTabla').textContent = "Ocultar tabla";
//    }
//    else {
//        //t.style.display = "none";
//        $('#progresBar').hide();
//        $('#indicador1').show();
//        //document.getElementById('MOTabla').textContent = "Mostrar tabla";
//    }
//}

function animacionWW(valor) {
    var t = document.getElementById("progresBar");

    //if (t.style.display === "none") {
    if (valor === 'A') {
        //t.style.display = "inline-table";
        $('#progresBar').show();
        $('#indicador').hide();
        //document.getElementById('MOTabla').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#progresBar').hide();
        $('#indicador').show();
        //document.getElementById('MOTabla').textContent = "Mostrar tabla";
    }
}

function animacionWW2(valor) {
    var t = document.getElementById("progresBar2");

    //if (t.style.display === "none") {
    if (valor === 'A') {
        //t.style.display = "inline-table";
        $('#progresBar2').show();
        $('#indicador2').hide();
        //document.getElementById('MOTabla').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#progresBar2').hide();
        $('#indicador2').show();
        //document.getElementById('MOTabla').textContent = "Mostrar tabla";
    }
}

function recorrerTabla(filtro) {

    var tabla, etiqueta, modal

    switch (filtro) {

        case 'Mes':
            tabla = 'tb_ActivosMes';
            etiqueta = 'alertaFiltrosMes';
            modal = '#modalActivosMes'
            break;
        case 'Semana':
            tabla = 'tb_ActivosSemana';
            etiqueta = 'alertaFiltrosSemana';
            modal = '#modalActivosSemana'
            break;
        case 'Dia':
            tabla = 'tb_ActivosDia';
            etiqueta = 'alertaFiltrosDia';
            modal = '#modalActivosDia'
            break;
        case 'Hora':
            tabla = 'tb_ActivosHora';
            etiqueta = 'alertaFiltrosHora';
            modal = '#modalActivosHora'
            break;

        default:
    }

    var table = document.getElementById(tabla);
    var checkBoxes = table.getElementsByTagName("INPUT");
    var vueltas = 0;
    var co_activo = "";
    activosSeleccionados = [];

    for (var i = 0; i < checkBoxes.length; i++) {
        if (checkBoxes[i].checked === true) {
            var row = checkBoxes[i].parentNode.parentNode;
            var c = row.cells[0].innerHTML;
            activosSeleccionados.push(c);
            co_activo = co_activo + (i === 0 ? '' : ',') + row.cells[0].innerHTML;
            vueltas++;
        }        
    }

    document.getElementById(etiqueta).textContent = 'Filtros activos: ' + co_activo;

    if (vueltas === 0) {
        document.getElementById(etiqueta).style.display = "none";
    } else {
        document.getElementById(etiqueta).style.display = "inline";
    }

    $(modal).modal('hide');
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