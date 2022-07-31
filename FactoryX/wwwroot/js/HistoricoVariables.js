
Highcharts.setOptions({
    colors: ["#DDDF0D", "#7798BF", "#55BF3B", "#DF5353", "#aaeeee", "#ff0066", "#eeaaee",
        "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
    chart: {
        backgroundColor: {
            linearGradient: [255, 255, 255],
            stops: [
                [0, 'rgb(255, 255, 255)'],
                [1, 'rgb(183, 193, 202)']
            ]
        },
        borderWidth: 0,
        borderRadius: 15,
        plotBackgroundColor: null,
        plotShadow: false,
        plotBorderWidth: 0
    },
    lang: {
        rangeSelectorZoom: ""
    },
    title: {
        style: {
            color: '#FFF',
            font: '16px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
        }
    },
    subtitle: {
        style: {
            color: '#DDD',
            font: '12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
        }
    },
    xAxis: {
        gridLineWidth: 1,
        gridLineColor: 'rgba(255, 255, 255, .1)',
        lineColor: '#999',
        tickColor: '#999',
        labels: {
            style: {
                color: '#999',
                fontWeight: 'bold'
            }
        },
        title: {
            style: {
                color: '#AAA',
                font: 'bold 12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
            }
        }
    },
    yAxis: {
        gridLineWidth: 1,
        alternateGridColor: null,
        minorTickInterval: null,
        gridLineColor: 'rgba(200, 200, 200, .5)',
        lineWidth: 0,
        tickWidth: 0,
        labels: {
            style: {
                color: '#999',
                fontWeight: 'bold'
            }
        },
        title: {
            style: {
                color: '#AAA',
                font: 'bold 12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
            }
        }
    },
    legend: {
        itemStyle: {
            color: '#CCC'
        },
        itemHoverStyle: {
            color: '#FFF'
        },
        itemHiddenStyle: {
            color: '#333'
        }
    },
    credits: {
        style: {
            right: '50px'
        }
    },
    labels: {
        style: {
            color: '#CCC'
        }
    },
    tooltip: {
        backgroundColor: {
            linearGradient: [0, 0, 0, 50],
            stops: [
                [0, 'rgba(96, 96, 96, .8)'],
                [1, 'rgba(16, 16, 16, .8)']
            ]
        },
        borderWidth: 0,
        style: {
            color: '#FFF'
        }
    },
    plotOptions: {
        line: {
            dataLabels: {
                color: '#CCC'
            },
            marker: {
                lineColor: '#5CC0D6'
            }
        },
        spline: {
            marker: {
                lineColor: '#5CC0D6'
            }
        },
        scatter: {
            marker: {
                lineColor: '#5CC0D6'
            }
        }
    },

    toolbar: {
        itemStyle: {
            color: '#CCC'
        }
    }
});

//Highcharts.setOptions({
//    colors: ["#DDDF0D", "#7798BF", "#55BF3B", "#DF5353", "#aaeeee", "#ff0066", "#eeaaee",
//        "#55BF3B", "#DF5353", "#7798BF", "#aaeeee"],
//    chart: {
//        backgroundColor: {
//            linearGradient: [255, 255, 255],
//            stops: [
//                [0, 'rgb(255, 255, 255)'],
//                [1, 'rgb(183, 193, 202)']
//            ]
//        },
//        borderWidth: 0,
//        borderRadius: 15,
//        plotBackgroundColor: null,
//        plotShadow: false,
//        plotBorderWidth: 0
//    },
//    lang: {
//        rangeSelectorZoom: ""
//    },
//    title: {
//        style: {
//            color: '#FFF',
//            font: '16px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
//        }
//    },
//    subtitle: {
//        style: {
//            color: '#DDD',
//            font: '12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
//        }
//    },
//    xAxis: {
//        gridLineWidth: 1,
//        gridLineColor: 'rgba(255, 255, 255, .1)',
//        lineColor: '#999',
//        tickColor: '#999',
//        labels: {
//            style: {
//                color: '#999',
//                fontWeight: 'bold'
//            }
//        },
//        title: {
//            style: {
//                color: '#AAA',
//                font: 'bold 12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
//            }
//        }
//    },
//    yAxis: {
//        gridLineWidth: 1,
//        alternateGridColor: null,
//        minorTickInterval: null,
//        gridLineColor: 'rgba(200, 200, 200, .5)',
//        lineWidth: 0,
//        tickWidth: 0,
//        labels: {
//            style: {
//                color: '#999',
//                fontWeight: 'bold'
//            }
//        },
//        title: {
//            style: {
//                color: '#AAA',
//                font: 'bold 12px Lucida Grande, Lucida Sans Unicode, Verdana, Arial, Helvetica, sans-serif'
//            }
//        }
//    },
//    legend: {
//        itemStyle: {
//            color: '#CCC'
//        },
//        itemHoverStyle: {
//            color: '#FFF'
//        },
//        itemHiddenStyle: {
//            color: '#333'
//        }
//    },
//    credits: {
//        style: {
//            right: '50px'
//        }
//    },
//    labels: {
//        style: {
//            color: '#CCC'
//        }
//    },
//    tooltip: {
//        backgroundColor: {
//            linearGradient: [0, 0, 0, 50],
//            stops: [
//                [0, 'rgba(96, 96, 96, .8)'],
//                [1, 'rgba(16, 16, 16, .8)']
//            ]
//        },
//        borderWidth: 0,
//        style: {
//            color: '#FFF'
//        }
//    },
//    plotOptions: {
//        line: {
//            dataLabels: {
//                color: '#CCC'
//            },
//            marker: {
//                lineColor: '#5CC0D6'
//            }
//        },
//        spline: {
//            marker: {
//                lineColor: '#5CC0D6'
//            }
//        },
//        scatter: {
//            marker: {
//                lineColor: '#5CC0D6'
//            }
//        }
//    },

//    toolbar: {
//        itemStyle: {
//            color: '#CCC'
//        }
//    }
//});


function mostrarFiltroDia() {
    var t = document.getElementById("diaFilter");

    //mostrarFiltroSemana();
    //mostrarFiltroMes();
    //mostrarFiltroHora();    

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#diaFilter').show(1000);
        document.getElementById("btnHora").disabled = true;
    }
    else {
        //t.style.display = "none";
        $('#diaFilter').hide(1000);
        document.getElementById("btnHora").disabled = false;
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
        document.getElementById("btnDia").disabled = true;
        //document.getElementById('btnOM').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#horaFilter').hide(1000);
        document.getElementById("btnDia").disabled = false;
        //document.getElementById('btnOM').textContent = "Mostrar tabla";
    }
}

function LineChart() {

    var filtro = ''
    var mensaje = false;

    var t1 = document.getElementById("diaFilter");
    var t2 = document.getElementById("horaFilter");
    
    if (t1.style.display !== "none") {
        filtro = 'ts'
    }
    else if (t2.style.display !== "none") {
        filtro = 'th'
    } else {
        Swal.fire({
            title: "Active un filtro para poder aplicar una selección válida",
            text: "seleccione un filtro por dia o por horas",
            icon: "info",
            confirmButtonText: 'Cerrar'
        }); 

        mensaje = true;
    }

    var ds = $("#dg_Activos").dxDataGrid("getDataSource")._items;    

    var contenedor = document.getElementById('dv_tablas');
    contenedor.innerHTML = "";

    var desde, hasta;

    if (filtro === 'ts') {

        desde = document.getElementById("diaDesde").innerHTML;
        desde = desde.substring(desde.search("value") + 7, desde.search("value") + 17);
        hasta = document.getElementById("diaHasta").innerHTML;
        hasta = hasta.substring(hasta.search("value") + 7, hasta.search("value") + 17);

    } else if (filtro === 'th') { //Filtro por hora

        desde = document.getElementById("horaDesde").innerHTML;
        desde = desde.substring(desde.search("value") + 7, desde.search("value") + 26);
        //alert(fini_);
        hasta = document.getElementById("horaHasta").innerHTML;
        hasta = hasta.substring(hasta.search("value") + 7, hasta.search("value") + 26);
    }

    for (var i = 0; i < ds.length; i++) {
        var chk = ds[i].Check;        

        if (chk === true) {
            var l = document.createElement('label');
            l.setAttribute("id", "lab_nuevoDiv" + i);

            contenedor.appendChild(l);
            contenedor.insertBefore(l, contenedor.childNodes[0]);

            //Creo el div interno
            var interno = document.createElement('div');
            interno.setAttribute("id", "internoDiv" + i);
            interno.classList.add('spinner-border', 'text-secondary');
            contenedor.appendChild(interno);
            contenedor.insertBefore(interno, contenedor.childNodes[0]);
            contenedor.insertBefore(document.createElement("br"), contenedor.childNodes[0]);
            document.getElementById("internoDiv" + i).style.marginRight = "10px";

            var g = document.createElement('div');
            g.setAttribute("id", "nuevoDiv" + i);
            //g.style.backgroundColor = i === 0 ? "yellow" : "red";
            //g.style.height = "45px";

            contenedor.appendChild(g);
            contenedor.insertBefore(g, contenedor.childNodes[0]);
            contenedor.insertBefore(document.createElement("br"), contenedor.childNodes[0]);

            var lab = document.getElementById("lab_nuevoDiv" + i);

            lab.innerHTML = ". Cargando valores, por favor espere...";
            lab.style.fontWeight = "bold";
            lab.style.backgroundColor = "#D4EDDA";
            lab.style.border = "2px solid green";
            lab.style.borderRadius = "5px";

            var tabla = ds[i].Tabla;
            var divv = "nuevoDiv" + i;
            var cod_activo = ds[i].Cod_activo;
            var labDiv = "lab_nuevoDiv" + i;
            var nombreEspera = "internoDiv" + i;

            if (mensaje === false) {
                agrega_grafico(tabla, cod_activo, divv, labDiv, filtro, desde, hasta, nombreEspera);
            }
        }
    }

    
}

function agrega_grafico(tabla, cod_activo, divv, labDiv, filtro, desde, hasta, nombreEspera) {
    //alert("Tabla: " + tabla + " - Div: " + divv);



    $.ajax({
        type: "POST",
        url: Factory.Urls.ValoresGrafico,
        data: { cod_activo, tabla, filtro, desde, hasta }, //Pendiente por crear
        success: function (Result) {

            if (Result.length == 0) {
                Swal.fire({
                    title: "No hay registros que mostrar.",
                    text: "Posiblemente se deba a que no hay seleccionado un filtro válido.",
                    icon: "info",
                    confirmButtonText: 'Cerrar'
                });
            }
            else {

                Chart(Result[0].name, Result[0].data, Result[0].id, tabla, Result[0].activo, Result[0].activo1, divv, nombreEspera, labDiv,
                    Result[0].umbralMinimo, Result[0].umbralMaximo, Result[0].variable, Result[0].unidad, Result[0].sku, Result[0].cod_plan, Result[0].desde_hasta);
            }
            
        },
        error: (response) => {

            Swal.fire({
                title: "No hay registros que mostrar.",
                text: "Posiblemente se deba a que no hay seleccionado un filtro válido.",
                icon: "info",
                confirmButtonText: 'Cerrar'
            });

        }
    });
}

var evento1;

function Chart(Series, datos, ultimoID, tabla, activo, activo1, nombreDiv, nombreEspera, labDiv, umbralMin, umbralMax, variable, unidad, sku, cod_plan, desde_hasta) {

    var nombreUnidad = "";

    switch (variable) {
        case 'CA':
            nombreUnidad = 'Cantidad';
            break;
        case 'CI':
            nombreUnidad = "Ciclos";
            break;
        case 'TE':
            nombreUnidad = 'Temperatura';
            break;
        case 'PR':
            nombreUnidad = 'Presión';
            break;
        case 'HU':
            nombreUnidad = 'Humedad';
            break;
        case 'PE':
            nombreUnidad = 'Peso';
            break;
        default:
            break;
    }

    var suma = 0;

    if (datos !== undefined) {
        datos.forEach(function (numero) {
            suma += numero;
            ++this.count;
            evento1 = Series[0];
        }, this);

        var contadorPF = 0;
        fa = new Date();
        var fecha1 = ((fa.getDate() < 10 ? "0" + fa.getDate() : fa.getDate()) + "-" +
            (((fa.getMonth() + 1)) < 10 ? "0" + (fa.getMonth() + 1) : (fa.getMonth() + 1)) + "-" +
            (fa.getFullYear()));

        Highcharts.chart(nombreDiv, {
            chart: {
                zoomType: 'x',
                panning: true,
                panKey: 'meta',
                type: 'area',
                events: {
                    load: function () {

                        fa = new Date();
                        console.log("Pasa....");

                        var fecha = ((fa.getDate() < 10 ? "0" + fa.getDate() : fa.getDate()) + "-" +
                            (((fa.getMonth() + 1)) < 10 ? "0" + (fa.getMonth() + 1) : (fa.getMonth() + 1)) + "-" +
                            (fa.getFullYear()));

                        if (variable === "CA" || variable === "CI") {
                            document.getElementById(labDiv).innerHTML = "- Total de " + nombreUnidad + ": " + addCommas(suma.toFixed(2)) + " " + unidad +  "<br/>";
                        }
                        else {
                            document.getElementById(labDiv).innerHTML = "- Último dato de " + nombreUnidad + ": " + addCommas(datos[datos.length - 1]) + " " + unidad +  "<br/>";
                        }

                        var series = this.series[0];
                        document.getElementById('info_ini').style.display = "none";
                    }
                }
            },           
            rangeSelector: {
                enabled: true,
                buttons: [{
                    count: 30,
                    type: 'millisecond',
                    text: '30M'
                }, {
                    count: 60,
                    type: 'millisecond',
                    text: '1H'
                }, {
                    type: 'all',
                    width: 50,
                    text: 'ALL'
                }],
                inputEnabled: false,
                selected: 0
            },
            time: {
                useUTC: false
            },
            navigator: {
                enabled: true,
                height: 25,
                margin: 0,
                xAxis: {
                    labels: false
                }
            },
            boost: {
                useGPUTranslations: true
            },
            title: {
                text: activo1 + " || " + variable + " || " + unidad + " || " + desde_hasta
            },
            xAxis: {
                type: 'datetime',
                categories: Series,
                title: {
                    text: 'Tiempo transcurrido'
                },
                scrollbar: {
                    enabled: false
                },
                //tickInterval: 10,
                labels: {
                    formatter: function () {
                        //if (this.isFirst)
                        //    return 60 - this.value + ' seconds';
                        return this.value; //Date.now().toString(); //this.value;
                    }
                }
            },
            yAxis: {
                min: 0,
                title: {
                    text: nombreUnidad + " (" + unidad + ")"
                },
                plotLines: [{
                    id: 'limit-min',
                    color: '#FF0000',
                    dashStyle: 'ShortDash',
                    width: 2,
                    value: umbralMin,
                    zIndex: 0,
                    label: {
                        text: 'MIN'
                    }
                }, {
                    id: 'limit-max',
                    color: '#FF0000',
                    dashStyle: 'ShortDash',
                    width: 2,
                    value: umbralMax,
                    zIndex: 0,
                    label: {
                        text: 'MAX',
                        color: '#666666'
                    }
                }]
            },
            tooltip: {
                headerFormat: '<b>{series.name}</b><br/>',
                formatter: function () {
                    var pxi = this.y / 100 * this.x;
                    //console.log(pxi.toPrecision(4));


                    return '<b> ' + nombreUnidad + ' en el tiempo </b><br/>' +
                        '<b>' + nombreUnidad + ': </b>' + this.y + '<br/>' +
                        '<b>Tiempo: </b>' + this.x + '<br/>'; //+
                        //'<b>SKU: </b>' + this.sku + '<br/>' +
                        //'<b>OP: </b>' + this.cod_plan + '<br/>';


                    //<button type="button" class="btn btn-primary" onclick="highlight(this)">Ir al riesgo</button>';
                    //'<br/> <a data-toggle="modal" data-target="#modalRiesgo_M" class="btn btn-success">Editar riesgo</a>';
                }
            },
            plotOptions: {
                area: {
                    fillOpacity: 0.2,
                    allowPointSelect: true
                }
            },
            legend: {
                enabled: true
            },
            credits: {
                enabled: false
            },
            exporting: {
                enabled: true
            },
            series: [{
                name: activo,
                color: '#4CAF50',
                data: datos
            }]
        });  

        document.getElementById(nombreEspera).style.display = 'none';
    }
}

//Función para dar formato decimal a un número
function addCommas(nStr) {
    nStr += '';
    var x = nStr.split('.');
    var x1 = x[0];
    var x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
} 

function formatNumber(n) {
    n = String(n).replace(/\D/g, "");
    return n === '' ? n : Number(n).toLocaleString();
}

function selection_changed(selectedItems) {
    var ds = $("#dg_Activos").dxDataGrid("getDataSource")._items;

    for (var i = 0; i < ds.length; i++) {
        ds[i].Check = false;
    }

    var data = selectedItems.selectedRowsData;

    for (var i = 0; i < data.length; i++) {

        data[i].Check = true;
        var chk = data[i].Check;
    }
}