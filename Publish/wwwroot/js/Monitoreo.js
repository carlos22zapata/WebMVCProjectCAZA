
var TipoParo = "";
var HoraParo = "";
var idIncidencia = "";

cuantaDiv = 0;

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

function mostrarTablaActivos() {
    var t = document.getElementById("tb_activos");

    if (t.style.display === "none") {
        //t.style.display = "inline-table";
        $('#tb_activos').show(1000);
        document.getElementById('btnOM').textContent = "Ocultar tabla";
    }
    else {
        //t.style.display = "none";
        $('#tb_activos').hide(1000);
        document.getElementById('btnOM').textContent = "Mostrar tabla";
    }
}

function prueba() {
    return 'ppp';
}

//function selection_changed(selectedItems)
//{
//    return selectedItems.selectedRowsData[0].Cod_activo;
//}

//$('input[type=checkbox]').click(function () {

//    var contenedor = document.getElementById('dv_tablas');
//    var html;

//    contenedor.innerHTML = "";

//    $("#tb_activos tr").each(function (i, row) {
//        var ch = $("input[type=checkbox]:checked");

//        if (i > 0) {
//            var dato0 = $(this).find('td:first').html();
//            var dato2 = $(this).find('input[type=checkbox]:checked').length;

//            if (dato2 === 1) {
//                //html = "Activo: " + dato0 + " - Valor: " + dato2 + "  ";
//                //contenedor.append(html);

//                $.ajax({
//                    type: "GET",
//                    url: FactoryX.Urls.graficoTR,
//                    data: { dato0 },
//                    success: function (Result) {
//                        var Data = $.parseJSON(Result);

//                        //alert(Data[0].Nombre_tabla);
//                        agrega_grafico(Data[0].Nombre_tabla);
//                    },
//                    error: (response) => {
//                        alert("No se tiene acceso al controlador.");
//                        location.reload(true);
//                    }
//                });
//            }
//        }        
//    });
//});

function formatNumber(n) {
    n = String(n).replace(/\D/g, "");
    return n === '' ? n : Number(n).toLocaleString();
}

function format(input) {
    var num = input.value.replace(/\./g, '');
    if (!isNaN(num)) {
        num = num.toString().split('').reverse().join('').replace(/(?=\d*\.?)(\d{3})/g, '$1.');
        num = num.split('').reverse().join('').replace(/^[\.]/, '');
        input.value = num;
    }

    else {
        alert('Solo se permiten numeros');
        input.value = input.value.replace(/[^\d\.]*/g, '');
    }
}

function agrega_grafico(tabla, nombreDiv, nombreEspera) {

    $.ajax({
        type: "GET",
        url: FactoryX.Urls.graficoTR2,
        data: { tabla },
        success: function (Result) {
            //var Data = $.parseJSON(Result);

            //alert(Data[0].Nombre_tabla);
            //agrega_grafico(Data[0].Nombre_tabla);
            //LineChart(Result[0].name, Result[0].data, Result[0].id, tabla, Result[0].activo, nombreDiv, Result[0].umbralMinimo, Result[0].umbralMaximo);
            //contenedor.insertBefore(newItem, tabla);//.append(tabla);

            if (Result.length === 0) {
                console.log("No se encontraron datos");

                document.getElementById('lab_' + nombreDiv).style.backgroundColor = "#F9E48E";
                document.getElementById('lab_' + nombreDiv).style.border = "2px solid yellow";
                document.getElementById('lab_' + nombreDiv).style.borderRadius = "5px";

                document.getElementById('lab_' + nombreDiv).innerHTML = 'No se encontraron datos que mostrar para el activo: ' + tabla.substring(4, tabla.length);
                document.getElementById(nombreEspera).style.display = 'none';
            } else {
                LineChart(Result[0].name, Result[0].data, Result[0].id, tabla, Result[0].activo, Result[0].activo1, nombreDiv, nombreEspera, Result[0].umbralMinimo, Result[0].umbralMaximo, Result[0].variable, Result[0].unidad, Result[0].sku, Result[0].cod_plan);
            }
        },
        error: (Result) => {
            alert("No se encuentran tablas o registros disponibles para el activo.");
            $(this).find('input[type=checkbox]:checked').lengt = 0;
        }
    });
}

var evento1;

function LineChart(Series, datos, ultimoID, tabla, activo, activo1, nombreDiv, nombreEspera, umbralMin, umbralMax, variable, unidad, sku, cod_plan) {

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

        //creaDiv = (tabla === 'IOT_ACT_1_CA' ? 'dv_tablas' : 'dv_tablas1');

        //suma = addCommas(suma.toFixed(2));

        var contadorPF = 0;
        fa = new Date();
        var fecha1 = ((fa.getDate() < 10 ? "0" + fa.getDate() : fa.getDate()) + "-" +
            (((fa.getMonth() + 1)) < 10 ? "0" + (fa.getMonth() + 1) : (fa.getMonth() + 1)) + "-" +
            (fa.getFullYear()));
        //var f = new Date();
        //fecha = f.getDate() + "/" + (f.getMonth() + 1) + "/" + f.getFullYear();
        Highcharts.chart(nombreDiv, {
            //$('#container1').highcharts({
            chart: {
                //type: 'line',
                //animation: Highcharts.svg, // don't animate in old IE
                //marginRight: 10,
                zoomType: 'x',
                panning: true,
                panKey: 'meta',
                type: 'area',
                events: {
                    load: function () {

                        fa = new Date();
                        //console.log("Pasa....");                        

                        var fecha = ((fa.getDate() < 10 ? "0" + fa.getDate() : fa.getDate()) + "-" +
                            (((fa.getMonth() + 1)) < 10 ? "0" + (fa.getMonth() + 1) : (fa.getMonth() + 1)) + "-" +
                            (fa.getFullYear()));

                        if (variable === "CA" || variable === "CI") {
                            document.getElementById('lab_' + nombreDiv).innerHTML = "- Total de " + nombreUnidad + ": " + addCommas(suma.toFixed(2)) + " " + unidad + "<br/>" +
                                " - Primer evento registrado: " + fecha + " " + evento1 + //"<br/>" +
                                " -  Último evento registrado: " + fecha + " " + Series[Series.length - 1] + " -";
                        }
                        else {
                            var aux = datos[datos.length - 1];
                            document.getElementById('lab_' + nombreDiv).innerHTML = "- Último dato de " + nombreUnidad + ": " + addCommas(aux) + " " + unidad + "<br/>" +
                                " - Primer evento registrado: " + fecha + " " + evento1 + //"<br/>" +
                                " -  Último evento registrado: " + fecha + " " + Series[Series.length - 1] + " -";
                        }
                        //document.getElementById('tup1').innerHTML = suma;
                        //document.getElementById('tup2').innerHTML = fecha + " " + Series[Series.length - 1];

                        // set up the updating of the chart each second
                        var series = this.series[0];

                        setInterval(function () {

                            //GraficarDatos();
                            if (contadorPF === 0) {

                                $.ajax({
                                    type: "GET",
                                    url: FactoryX.Urls.graficoTR2,
                                    data: { idEmpresa: 0, actualizado: ultimoID, tabla }
                                }).done(function (data) {
                                    //console.log("Cantidad de registros: " + data[0].data[i]);

                                    //var prueba = data[0].name.length;

                                    for (var i = 0; i < data[0].name.length; i++) {

                                        //console.log("Valor X:" + data[0].name[i] + ", Valor Y:" + data[0].data);
                                        var x = data[0].name[i];
                                        var y = parseFloat(data[0].data[i].toFixed(2));
                                        //Series.addPoint(x, y);
                                        //series.addPoint([x, y], true, x);
                                        series.xAxis.categories.push(x);
                                        series.addPoint([x, y], true, true);

                                        ultimoID = data[0].id;
                                        suma = (suma += y);

                                        //<div id="progresBar" style="display:none;">
                                        //    <div class="spinner-border text-secondary" role="status">
                                        //        <span class="sr-only">Calculando valores...</span>
                                        //    </div>
                                        //    <a>&nbsp; Calculando valores...</a>
                                        //    <br />
                                        //</div>

                                        if (variable === "CA" || variable === "CI") {
                                            document.getElementById('lab_' + nombreDiv).innerHTML = "- Total de " + nombreUnidad + ": " + addCommas(suma.toFixed(2)) + " " + unidad + "<br/>" +
                                                " - Primer evento registrado: " + fecha + " " + evento1 + //"<br/>" +
                                                " -  Último evento registrado: " + fecha + " " + Series[Series.length - 1] + " -";
                                        }
                                        else {
                                            var aux = datos[datos.length - 1];
                                            document.getElementById('lab_' + nombreDiv).innerHTML = "- Último dato de " + nombreUnidad + ": " + addCommas(aux[1]) + " " + unidad + "<br/>" +
                                                " - Primer evento registrado: " + fecha + " " + evento1 + //"<br/>" +
                                                " -  Último evento registrado: " + fecha + " " + Series[Series.length - 1] + " -";
                                        }
                                        //document.getElementById("tup1").innerHTML = suma += y;
                                        //document.getElementById("tup2").innerHTML = fecha + " " + x;
                                    }

                                    setTimeout(LineChart, 1000);

                                }).fail(
                                    console.log("Consulta datos No."));


                            }
                        }, 30000);
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
                text: activo1 + "/" + variable + "/" + unidad + "/" + fecha1
            },
            xAxis: {
                type: 'datetime',
                categories: Series,
                title: {
                    text: 'Tiempo transcurrido (hh:mm)'
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

function prueba(newData, value, currentRowData) {
    console.log('Cambiado');
}

//Función que arma el gráfigo de tiempo real
function selection_changed(selectedItems) {
    var data = selectedItems.selectedRowsData;
    var contenedor = document.getElementById('dv_tablas');
    var contenedor1 = document.getElementById('dv_tablas1');

    contenedor.innerHTML = "";
    contenedor1.innerHTML = "";

    repetido = "";

    $.map(data, function (value) {

        if (repetido !== (value.Cod_activo + value.Variable)) {
            var cod_activo = value.Cod_activo;
            var variable = value.Variable;

            contador = 0;

            $.ajax({
                type: "GET",
                url: FactoryX.Urls.graficoTR,
                data: { cod_activo, variable },
                success: function (Result) {
                    var Data = $.parseJSON(Result);

                    var l = document.createElement('label');
                    l.setAttribute("id", "lab_nuevoDiv" + cuantaDiv);

                    contenedor.appendChild(l);
                    contenedor.insertBefore(l, contenedor.childNodes[0]);

                    //Creo el div interno
                    var interno = document.createElement('div');
                    interno.setAttribute("id", "internoDiv" + cuantaDiv);
                    interno.classList.add('spinner-border', 'text-secondary');
                    contenedor.appendChild(interno);
                    contenedor.insertBefore(interno, contenedor.childNodes[0]);
                    contenedor.insertBefore(document.createElement("br"), contenedor.childNodes[0]);
                    document.getElementById("internoDiv" + cuantaDiv).style.marginRight = "10px";


                    //contenedor.insertBefore(interno, contenedor.childNodes[0]);
                    //contenedor.insertBefore(document.createElement("br"), contenedor.childNodes[0]);

                    //    <div class="spinner-border text-secondary" role="status">
                    //        <span class="sr-only">Calculando valores...</span>
                    //    </div>
                    //    <a>&nbsp; Calculando valores...</a>
                    //    <br />


                    //Creo los div necesarios con su id correspondiente cada uno
                    var g = document.createElement('div');
                    g.setAttribute("id", "nuevoDiv" + cuantaDiv);

                    contenedor.appendChild(g);
                    contenedor.insertBefore(g, contenedor.childNodes[0]);
                    contenedor.insertBefore(document.createElement("br"), contenedor.childNodes[0]);

                    document.getElementById("lab_nuevoDiv" + cuantaDiv).innerHTML = ". Cargando valores, por favor espere...";
                    document.getElementById("lab_nuevoDiv" + cuantaDiv).style.fontWeight = "bold";
                    document.getElementById("lab_nuevoDiv" + cuantaDiv).style.backgroundColor = "#D4EDDA";
                    document.getElementById("lab_nuevoDiv" + cuantaDiv).style.border = "2px solid green";
                    document.getElementById("lab_nuevoDiv" + cuantaDiv).style.borderRadius = "5px";

                    //alert(Data[0].Nombre_tabla);
                    var nombreDiv = "nuevoDiv" + cuantaDiv;
                    var nombreEspera = "internoDiv" + cuantaDiv;

                    agrega_grafico(Data[0].Nombre_tabla, nombreDiv, nombreEspera);

                    document.getElementById(nombreDiv).style.minWidth = "600px";

                    cuantaDiv++;
                },
                error: (response) => {
                    alert("No se tiene acceso al controlador.");
                    location.reload(true);
                }
            });
        }

        repetido = value.Cod_activo + value.Variable;
    });
}

//$(document).ready(function () {
//    $("#myBtn").click(function () {
//        $("#myModal").modal();
//    });
//});


//function abrirModalPrueba(){
//    $("#myModal").modal();
//}

function abrirModalSKU(selectedItems) {
    //verWarningSku('n');
    var ca = selectedItems.row.data;

    document.getElementById('btn_aprobacion').style.display = 'inline';

    $("#modalSKU").modal();
    //document.getElementById('modalSKU').style.display = "inline";
    document.getElementById('titulo').innerHTML = "Activo seleccionado: " + ca.Des_activo;

    cod_activo = document.getElementById('Lab_activo').value = ca.Cod_activo;
    variable = document.getElementById('Lab_variable').value = ca.Variable;
    document.getElementById('Lab_id').value = ca.Id;
    document.getElementById('Lab_tabla').value = ca.Tabla;

    //document.getElementById('btn_aceptar1').innerHTML = 'Obtener código';
    //document.getElementById('aprobacion').style.display = "none";

    llena_CB_HoraParo(document.getElementById('Lab_tabla').value, cod_activo = document.getElementById('Lab_activo').value);

    var posicion = selectedItems.row.cells[0].rowIndex;
    var grid = selectedItems.component;

    TipoParo = "";
    HoraParo = "";

    document.getElementById('txt_observacion').value = "";
    document.getElementById('CB_TipoParo').value = "";
    document.getElementById('CB_HoraParo').value = "";

    //Busco en base de datos si tengo un registro de ese activo en la tabla de insidencias con la fecha de hasta en nulo
    $.ajax({
        type: "POST",
        url: FactoryX.Urls.IncidenciasActivas,
        data: { cod_activo }, //Pendiente por crear
        success: (response) => {

            var hayR = response.length;

            if (hayR > 0) {
                document.getElementById('CB_TipoParo').value = response[0].Cod_tipo;
                document.getElementById('CB_HoraParo').value = response[0].Desde;
                document.getElementById('txt_observacion').value = response[0].Observacion;

                document.getElementById('CB_TipoParo').setAttribute('disabled', true);
                document.getElementById('CB_HoraParo').setAttribute('disabled', true);
                document.getElementById('txt_observacion').setAttribute('disabled', true);
                document.getElementById('Lab_idIncidencia').value = response[0].Cod_incidencia;
                document.getElementById('btn_guardaParo').innerHTML = 'Finalizar paro del activo';

            } else {

                document.getElementById('CB_TipoParo').disabled = false;
                document.getElementById('CB_HoraParo').disabled = false;
                document.getElementById('txt_observacion').disabled = false;
                document.getElementById('btn_guardaParo').innerHTML = 'Guardar registro de paro';

            }
        },
        error: (response) => {
            alert("No se pudo consultar la tabla de incidencias.");
        }
    });
}

function muestraCerrarOP(selectedItems) {
    var ca = selectedItems.row.data;
    //document.getElementById('modalCerrarOP').style.display = "inline";
    $("#modalCerrarOP").modal();
    document.getElementById('Lab_id3').value = ca.Iid;
    var nreng = ca.Id;



    $.ajax({
        type: "POST",
        url: FactoryX.Urls.InformacionOP,
        data: { nreng }, //Pendiente por crear
        success: (response) => {
            //location.reload(true);

            console.log(response);

            document.getElementById('Lab_orden').innerHTML = 'Código del plan: ' + response.Cod_plan;
            document.getElementById('Lab_produ').innerHTML = 'Código del sku:  ' + response.Cod_producto;
        },
        error: (response) => {
            Swal.fire({
                title: "No se pudieron consultar los registros.",
                text: "Posiblemente se deba a que no hay conexión con la base de datos.",
                icon: "error",
                confirmButtonText: 'Cerrar'
            });
            //alert("No se pudieron actualizar los registros.");
        }
    });
}

function muestraCambiaSKU(selectedItems) {
    var ca = selectedItems.row.data;

    $("#modalPlan_SKU").modal();
    document.getElementById('idSKU').value = ca.Id;
    document.getElementById('Lab_cod_activoSKU').value = ca.Cod_activo;
    document.getElementById('Lab_cod_plan').value = ca.Cod_plan;
    document.getElementById('Lab_SKUX').value = ca.SKU;
    document.getElementById('lab_planes_activos').innerHTML = "Activo: " + ca.Cod_activo + " - Variable: " + ca.Variable;

    planesActivos(ca.Cod_activo);
}

function cerrarOP() {

    var iid = document.getElementById('Lab_id3').value;

    $.ajax({
        type: "POST",
        url: FactoryX.Urls.CerrarOP,
        data: { iid }, //Pendiente por crear
        success: (response) => {
            $("#dg_activos").dxDataGrid("instance").refresh();
            $("#modalCerrarOP").modal('hide');
        },
        error: (response) => {
            Swal.fire({
                title: "No se pudieron actualizar los registros.",
                text: "Posiblemente se deba a que no hay seleccionado una orden válida.",
                icon: "info",
                confirmButtonText: 'Cerrar'
            });
            //alert("No se pudieron actualizar los registros.");
        }
    });
}

function abrirModalHistoricosSKU(selectedItems) {
    var ca = selectedItems.row.data;
    document.getElementById('Lab_tabla2').value = ca.Tabla;
    document.getElementById('Lab_id2').value = ca.Id;

    $("#modalSKU_Historico").modal();
    //document.getElementById('modalSKU_Historico').style.display = "inline";
    //document.getElementById('contenido2').style.display = 'none';

    document.getElementById('btn_aprobacion').style.display = 'inline';
    document.getElementById('btn_aceptar1').style.display = "none";
    document.getElementById('aprobacion').style.display = "none";

    document.getElementById('titulo2').innerHTML = "Activo seleccionado: " + ca.Des_activo;
    document.getElementById('Lab_activo3').value = ca.Cod_activo;
    //document.getElementById('contenido1').style.display = 'inline';    
    //$('#CB_HoraCambio_D').val("");    

    document.getElementById('CB_HoraCambio_D').value = "";
    document.getElementById('CB_HoraCambio_H').value = "";
    document.getElementById('CB_SKU_').value = "";
    document.getElementById('CB_OP_').value = "";

    document.getElementById('CB_HoraCambio_D').removeAttribute("disabled");
    document.getElementById('CB_HoraCambio_H').removeAttribute("disabled");
    document.getElementById('CB_SKU_').removeAttribute("disabled");
    document.getElementById('CB_OP_').removeAttribute("disabled");

    llena_CB_HoraParo(ca.Tabla, ca.Cod_activo);
}

function guardaHistoricos() {
    var idActivos_tablas = document.getElementById('Lab_id2').value;
    var codigo = document.getElementById('aprobacion').value;

    $.ajax({
        type: 'GET',
        url: FactoryX.Urls.ValidaCodigo,
        data: { idActivos_tablas, codigo },
        success: (response) => {

            if (response === false) {
                Swal.fire({
                    title: "El código que se introdujo no es válido",
                    text: "Vuelva a probar nuevamente con un código diferente",
                    icon: "error",
                    confirmButtonText: 'Cerrar'
                });
            } else {

                var cod_producto = document.getElementById('CB_SKU_').value;
                var cod_plan = document.getElementById('CB_OP_').value;
                var fini = document.getElementById('CB_HoraCambio_D').value;
                var ffin = document.getElementById('CB_HoraCambio_H').value;
                var tabla = document.getElementById('Lab_tabla2').value;

                $.ajax({
                    type: "GET",
                    url: FactoryX.Urls.ActualizaHistoricos,
                    data: { cod_producto, cod_plan, fini, ffin, tabla },
                    success: (response) => {
                        Swal.fire({
                            title: "Registros actualizados correctamente!",
                            text: "Cambios guardados. Puede cambiar otro rango de registros si así lo desea, una vez terminado solo cierre la ventana.",
                            icon: "success",
                            confirmButtonText: 'Cerrar'
                        });

                        activaDesactivaControles1(0);
                    },
                    error: (response) => {
                    }
                });
            }
        },
        error: (response) => {
            alert('Error al validar el códogo en base de datos');
        }
    });
}

function verAprobacion() {
    var acep = document.getElementById('btn_aceptar1');
    var tabla = document.getElementById('Lab_tabla2').value;
    //var activo = document.getElementById('Lab_activo3').value;

    //llena_CB_HoraParo(tabla, activo);

    if (acep.innerHTML === 'Obtener código') {
        document.getElementById('btn_aceptar1').innerHTML = 'Aceptar';
        document.getElementById('aprobacion').value = '';
        document.getElementById('aprobacion').style.display = "inline";

        //Reviso en base de datos si el correo esta vacio  
        $.ajax({
            type: "POST",
            url: FactoryX.Urls.ValidaCorreo,
            data: { tabla },
            success: (response) => {
                if (response === undefined) {
                    Swal.fire({
                        title: "Llene la información de los correos",
                        text: "No existen correos registrados en base de datos.",
                        icon: 'error',
                        confirmButtonText: 'Cerrar'
                    });
                } else {
                    Swal.fire({
                        title: "Se envio un correo con el código a la direccion: " + "\r\n" + response,
                        text: "Coloque el código en la casilla correspondiente y presione aceptar.",
                        icon: 'info',
                        confirmButtonText: 'Cerrar'
                    });
                }
            },
            error: (response) => {
                alert("Error al consultar correo en base de datos. Error: " + response);
            }
        });
    }

    else { //Si el botón dice Aceptar valido que el código coincida con el que esta en base de datos
        var idActivos_tablas = document.getElementById('Lab_id2').value;
        var codigo = document.getElementById('aprobacion').value;

        $.ajax({
            type: 'GET',
            url: FactoryX.Urls.ValidaCodigo,
            data: { idActivos_tablas, codigo },
            success: (response) => {

                if (response === false) {
                    Swal.fire({
                        title: "El código que se introdujo no es válido",
                        text: "Vuelva a probar nuevamente con un código diferente",
                        icon: "error",
                        confirmButtonText: 'Cerrar'
                    });
                } else {
                    Swal.fire({
                        title: "Código aprobado",
                        text: "Puede acceder a cambiar los registros",
                        icon: "success",
                        confirmButtonText: 'Cerrar'
                    });

                    document.getElementById('contenido1').style.display = 'none';
                    document.getElementById('contenido2').style.display = 'inline';

                }
            },
            error: (response) => {
                alert('Error al validar el códogo en base de datos');
            }
        });
    }
}

function abrirModalIncidencias(selectedItems) {
    var ca = selectedItems.row.data.Cod_activo;
    alert(ca);
}

//function cerrarModal(nro) {
//    if (nro === 1) {
//        document.getElementById('modalSKU').style.display = "none";
//        //alert(TipoParo);
//    }
//    else if (nro === 2) {
//        document.getElementById('modalSKU_Historico').style.display = "none";
//    }
//    else if (nro === 3) {
//        document.getElementById('modalCerrarOP').style.display = "none";
//    }

//}

function selection_sku(selectedItems) {
    var data = selectedItems.selectedRowsData;
    var skuSeleccionado = "";
    var cod_activo = "";
    var variable = "";

    document.getElementById('modalSKU').style.display = "none";

    $.map(data, function (value) {
        skuSeleccionado = value.Cod_producto;
        cod_activo = document.getElementById('Lab_activo').value;
        variable = document.getElementById('Lab_variable').value;
        //alert(value.Cod_producto);        
    });

    cambia_SKU(skuSeleccionado);
    verWarningSku('n');
}

//function cambia_SKU(skuSeleccionado) {

//    var key = document.getElementById('Lab_id').value;

//    $.ajax({
//        type: "PUT",
//        url: FactoryX.Urls.UpdateSKU,
//        data: { key, skuSeleccionado},
//        success: (response) => {
//            //location.reload(true);
//            //$("#dg_activos").dxGrid("reload");//.dxDataGrid("getDataSource").reload();
//            $("#dg_activos").dxDataGrid("instance").refresh();
//        },
//        error: (response) => {
//            alert("Error al actualizar SKU");
//        }
//    });
//}

function reloadData() {
    var ds = $("#dg_activos");
    ds.dxDataGrid.reload();
}

function enviaCorreo() {
    $.ajax({
        type: "POST",
        url: FactoryX.Urls.enviaCorreo,
        data: {},
        success: (response) => {
            alert(response);
        },
        error: (response) => {
            alert("Error");
        }
    });
}

//function verWarningSku(sn) {

//    var wn = document.getElementById('warning_sku');

//    if (sn === 's') {
//        wn.style.display = 'inline';
//    }
//    else {
//        wn.style.display = 'none';     
//    }
//}

function enviaTelegram() {
    $.ajax({
        type: "POST",
        url: FactoryX.Urls.enviaTelegram,
        data: {},
        success: (response) => {
            alert("Telegram enviado.");
        },
        error: (response) => {
            alert("Error de envío.");
        }
    });
}

function getDetailGridDataSource(key) {
    return {
        store: ordersStore,
        filter: ["Id", "=", key],
        reshapeOnPush: true
    };
}

function asignaId() {
    $.ajax({
        type: "GET",
        url: FactoryX.Urls.AsignaEmpresa,
        data: {},
        success: (response) => {
            console.log('id de la empresa asignado corectamenete.');
        },
        error: (response) => {
            alert("No se pudo asignar el id de la empresa.");
        }
    });
}

function llena_CB_HoraParo(tabla, cod_activo) {

    $("[id*='CB_HoraParo']").empty();
    $("[id*='CB_HoraCambio_D']").empty();
    $("[id*='CB_HoraCambio_H']").empty();

    $.ajax({
        type: "GET",
        url: FactoryX.Urls.CargaHoras,
        data: { tabla, cod_activo },
        success: (response) => {
            $.each(response, function (i, response) {
                $("#CB_HoraParo").append('<option value="' + response.timestamp + '">' + response.timestamp + '</option>');
                $("#CB_HoraCambio_D").append('<option value="' + response.timestamp + '">' + response.timestamp + '</option>');
                $("#CB_HoraCambio_H").append('<option value="' + response.timestamp + '">' + response.timestamp + '</option>');
            });

            console.log('Horas asignadas correctamenete.');
        },
        error: (response) => {
            alert("No se pudo asignar la lista de horas.");
        }
    });
}

function guardaParo() {
    var cod_activo = document.getElementById('Lab_activo').value;
    var cod_tipo = document.getElementById('CB_TipoParo').value;
    var observacion = document.getElementById('txt_observacion').value;
    var fecha = document.getElementById('CB_HoraParo').value;
    var idIncidencia = document.getElementById('Lab_idIncidencia').value;

    if (document.getElementById('btn_guardaParo').innerHTML === 'Guardar registro de paro') {

        if (cod_tipo === "") {
            Swal.fire({
                title: "Complete los campos para guardar el registro",
                text: "Debe seleccionar un tipo de paro",
                type: "warning",
                icon: "warning",
                confirmButtonText: 'Cerrar'
            });
        } else if (fecha === "") {
            Swal.fire({
                title: "Complete los campos para guardar el registro",
                text: "Debe seleccionar una fecha y hora",
                type: "warning",
                icon: "warning",
                confirmButtonText: 'Cerrar'
            });
        } else if (observacion === "") {
            Swal.fire({
                title: "Complete los campos para guardar el registro",
                text: "Debe escribir una observación en referencia al paro",
                type: "warning",
                icon: "warning",
                confirmButtonText: 'Cerrar'
            });
        } else {
            $.ajax({
                type: "GET",
                url: FactoryX.Urls.GuardaParo,
                data: { cod_activo, cod_tipo, observacion, fecha },
                success: (response) => {
                    Swal.fire({
                        title: "Registro guardado correctamente",
                        text: "Se incluyó el registro con la información del paro.",
                        type: "warning",
                        icon: "success",
                        confirmButtonText: 'Cerrar'
                    });

                    //cerrarModal(1);
                    console.log("Registros guardados exitosamente.");
                    $("#dg_activos").dxDataGrid("instance").refresh(); //Actualizo el grid
                },
                error: (response) => {
                    alert("No se pudo guardar el registro de los paros.");
                }
            });
        }

    } else {
        $.ajax({
            type: "GET",
            url: FactoryX.Urls.CierreParo,
            data: { idIncidencia },
            success: (response) => {
                Swal.fire({
                    title: "Registro actualizado correctamente",
                    text: "Se estableció la fecha fin del paro del activo",
                    type: "warning",
                    icon: "success",
                    confirmButtonText: 'Cerrar'
                });

                //cerrarModal(1);
                console.log("Registros guardados exitosamente.");
                $("#dg_activos").dxDataGrid("instance").refresh(); //Actualizo el grid
            },
            error: (response) => {
                alert("No se pudo guardar el registro de los paros.");
            }
        });
    }


}

function apruebaHistoricos() {

    var sku = document.getElementById('CB_SKU_').value;
    var cod_plan = document.getElementById('CB_OP_').value;
    var fini = document.getElementById('CB_HoraCambio_D').value;
    var ffin = document.getElementById('CB_HoraCambio_H').value;
    var tabla = document.getElementById('Lab_tabla2').value;

    if (fini === undefined || fini === "") {
        Swal.fire({
            title: "Debe selecionar una fecha y hora de inicio ",
            text: "Seleccione y vuelva a intentar procesar su solicitud",
            icon: "error",
            confirmButtonText: 'Cerrar'
        });
        return false;
    } else if (ffin === undefined || ffin === "") {
        Swal.fire({
            title: "Debe selecionar una fecha y hora fin ",
            text: "Seleccione y vuelva a intentar procesar su solicitud",
            icon: "error",
            confirmButtonText: 'Cerrar'
        });
        return false;
    } else if (ffin <= fini) {
        Swal.fire({
            title: "La fecha final no puede ser menor ni igual a la fecha de inicio ",
            text: "Seleccione y vuelva a intentar procesar su solicitud",
            icon: "error",
            confirmButtonText: 'Cerrar'
        });
        return false;
    } else {

        //Reviso en base de datos si el correo esta vacio  
        $.ajax({
            type: "POST",
            url: FactoryX.Urls.ValidaCorreo,
            data: { tabla },
            success: (response) => {
                if (response === undefined) {
                    Swal.fire({
                        title: "Llene la información de los correos",
                        text: "No existen correos registrados en base de datos.",
                        icon: 'error',
                        confirmButtonText: 'Cerrar'
                    });
                    return false;
                } else {
                    Swal.fire({
                        title: 'Se solicitará un código de aprobación',
                        html: 'Esta opción requiere un código que se enviará por correo electronico a la siguiente dirección: <b>' + response + '</b>, si esta de acuerdo presione el botón continuar, de lo contrario presione el botón cancelar.',
                        icon: 'warning',
                        showCancelButton: true,
                        confirmButtonColor: '#3085d6',
                        cancelButtonColor: '#d33',
                        cancelButtonText: "Cancelar",
                        confirmButtonText: 'Continuar'
                    }).then((result) => {
                        if (result.value) {

                            var correo = response;

                            //Aquí se evia el correo electronico
                            $.ajax({
                                type: "POST",
                                url: FactoryX.Urls.CreaCodigo,
                                data: { correo, fini, ffin, cod_plan, sku, tabla },
                                success: (response) => {

                                },
                                error: (response) => {
                                    Swal.fire({
                                        title: 'Error al enviar código',
                                        text: 'Problema al consultar controlador Monitoreo, método CreaCodigo'
                                    });
                                    return false;
                                }

                            });

                            activaDesactivaControles1(1);

                            //document.getElementById('btn_aprobacion').style.display = 'none';
                            //document.getElementById('btn_aceptar1').style.display = "inline";
                            //document.getElementById('aprobacion').style.display = "inline";
                        }
                    });

                    return false;

                    //swal({
                    //    title: "Se envio un correo con el código a la direccion: " + "\r\n" + response,
                    //    text: "Coloque el código en la casilla correspondiente y presione aceptar.",
                    //    icon: 'info',
                    //    confirmButtonText: 'Cerrar'
                    //});
                }
            },
            error: (response) => {
                alert("Error al consultar correo en base de datos. Error: " + response);
            }
        });

        //Swal.fire({
        //title: 'Se solicitará un código de aprobación',
        //text: "Esta opción requiere un código que se enviará por correo electronico a la siguiente dirección: " + "Correo, " + "Si esta de acuerdo presione el botón continuar, de lo contrario presione el botón cancelar.",
        //icon: 'warning',
        //showCancelButton: true,
        //confirmButtonColor: '#3085d6',
        //cancelButtonColor: '#d33',
        //cancelButtonText: "Cancelar",
        //confirmButtonText: 'Continuar'
        //}).then((result) => {
        //    if (result.value) {
        //        document.getElementById('btn_aprobacion').style.display = 'none';
        //        document.getElementById('btn_aceptar1').style.display = "inline";
        //        document.getElementById('aprobacion').style.display = "inline";
        //    }
        //});

        //$.ajax({
        //    type: "GET",
        //    url: FactoryX.Urls.ActualizaHistoricos,
        //    data: { cod_producto, cod_plan, fini, ffin, tabla },
        //    success: (response) => {
        //        swal({
        //            title: "Registros actualizados correctamente!",
        //            text: "Cambios guardados. Puede cambiar otro rango de registros si así lo desea, una vez terminado solo cierre la ventana.",
        //            icon: "success",
        //            confirmButtonText: 'Cerrar'
        //        });
        //    },
        //    error: (response) => {
        //    }
        //});
    }
}

function activaDesactivaControles1(ad) {

    if (ad === 0) {
        document.getElementById('btn_aprobacion').style.display = 'inline';
        document.getElementById('CB_HoraCambio_D').removeAttribute("disabled");
        document.getElementById('CB_HoraCambio_H').removeAttribute("disabled");
        document.getElementById('CB_SKU_').removeAttribute("disabled");
        document.getElementById('CB_OP_').removeAttribute("disabled");
        document.getElementById('aprobacion').style.display = 'none';
        document.getElementById('btn_aceptar1').style.display = 'none';
    } else {
        document.getElementById('btn_aprobacion').style.display = 'none';
        document.getElementById('CB_HoraCambio_D').setAttribute('disabled', true);
        document.getElementById('CB_HoraCambio_H').setAttribute('disabled', true);
        document.getElementById('CB_SKU_').setAttribute('disabled', true);
        document.getElementById('CB_OP_').setAttribute('disabled', true);
        document.getElementById('aprobacion').style.display = 'inline';
        document.getElementById('btn_aceptar1').style.display = 'inline';
    }

}

function cellPrepared(e) {
    if (e.rowType === 'data') {
        if (e.data.Cod_incidencia > 0) {
            e.cellElement.css({ color: 'red' });

            //e.gridLineColor = 'red';
        }
    }
}


//function PruebaRefresh() {
//    $("#dg_activos").dxDataGrid("instance").refresh();    
//}
//var locale = getLocale();
//Globalize.locale(locale);

$(document).ready(function () {

});

function GetUserIP() {
    var ret_ip;
    $.ajaxSetup({ async: false });
    $.get('http://jsonip.com/', function (r) {
        ret_ip = r.ip;
    });
    return ret_ip;
}

function findIP(onNewIP) { //  onNewIp - your listener function for new IPs
    var myPeerConnection = window.RTCPeerConnection || window.mozRTCPeerConnection || window.webkitRTCPeerConnection; //compatibility for firefox and chrome
    var pc = new myPeerConnection({ iceServers: [] }),
        noop = function () { },
        localIPs = {},
        ipRegex = /([0-9]{1,3}(\.[0-9]{1,3}){3}|[a-f0-9]{1,4}(:[a-f0-9]{1,4}){7})/g,
        key;

    function ipIterate(ip) {
        if (!localIPs[ip]) onNewIP(ip);
        localIPs[ip] = true;
    }
    pc.createDataChannel(""); //create a bogus data channel
    pc.createOffer(function (sdp) {
        sdp.sdp.split('\n').forEach(function (line) {
            if (line.indexOf('candidate') < 0) return;
            line.match(ipRegex).forEach(ipIterate);
        });
        pc.setLocalDescription(sdp, noop, noop);
    }, noop); // create offer and set local description
    pc.onicecandidate = function (ice) { //listen for candidate events
        if (!ice || !ice.candidate || !ice.candidate.candidate || !ice.candidate.candidate.match(ipRegex)) return;
        ice.candidate.candidate.match(ipRegex).forEach(ipIterate);
    };
}

var ul = document.createElement('ul');
ul.textContent = 'Your IPs are: '
document.body.appendChild(ul);

function addIP(ip) {
    console.log('got ip: ', ip);
    var li = document.createElement('li');
    li.textContent = ip;
    ul.appendChild(li);
}

findIP(addIP);

$(function () {

});

function planesActivos(cod_activo) {

    var result = [];

    var $loadIndicator = $("<div>").dxLoadIndicator({ visible: false }),
        $dropDownButtonImage = $("<img>", {
            src: "images/icons/custom-dropbutton-icon.svg",
            class: "custom-icon"
        });

    let resultados = [];

    var iid = document.getElementById('idSKU').value;
    var coa = document.getElementById('Lab_cod_activoSKU').value;
    var cop = document.getElementById('Lab_cod_plan').value;
    var sku = document.getElementById('Lab_SKUX').value;

    $.ajax({
        type: "GET",
        url: FactoryX.Urls.CargaPlan,
        data: { cod_activo },
        dataType: 'json',
        contentType: "application/json",
        success: (response) => {

            for (var i = 0; i < response.length; i++) {
                resultados.push(response[i]['Cod_centro']);
            }

            $("#planesActivos").dxSelectBox({
                placeholder: "Seleccione un valor...",
                items: resultados,
                value: 'OP: ' + cop + ", SKU: " + sku,
                searchEnabled: true
            });
        }

    });
}

function desacoplaOrden() {

    var cod_activo = document.getElementById('Lab_cod_activoSKU').value

    $.ajax({
        type: "GET",
        url: FactoryX.Urls.DesacoplaOrden,
        data: { cod_activo },
        success: function (Result) {
            $("#dg_activos").dxDataGrid("instance").refresh();
        }
    });
}

function establecePlanSku() {
    var valor = $("#planesActivos").dxSelectBox('instance').option('value')

    var coma = valor.indexOf(",");
    var tama = valor.length;

    var cod_activo = document.getElementById('Lab_cod_activoSKU').value;
    var cod_plan = valor.substring(4, coma);
    var sku_activo = valor.substring(coma + 7, tama);

    $.ajax({
        type: "GET",
        url: FactoryX.Urls.ActualizaSKU,
        data: { cod_activo, cod_plan, sku_activo },
        success: function (Result) {
            $("#dg_activos").dxDataGrid("instance").refresh();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            alert("Error: " + xhr.responseText);
        }
    });
}

//var simpleProducts = [
//    "HD Video Player",
//    "SuperHD Video Player",
//    "SuperPlasma 50",
//    "SuperLED 50",
//    "SuperLED 42"
//];