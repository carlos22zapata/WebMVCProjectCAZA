$(document).ready(function () {

    var datos = getParameterByName('datos').split("|");
    var cantidad = datos[0];
    var producto = datos[1];

    //Creo el div para guadar las etiquetas
    var contenedor = document.getElementById('contenedor');
    contenedor.innerHtml = "";

    for (var i = 0; i < cantidad; i++) {
        //alert("Van: " + i);

        var nombreDiv = "qrResult" + i;
        var g = document.createElement('div');
        g.setAttribute("id", nombreDiv);

        contenedor.appendChild(g);
        contenedor.insertBefore(g, contenedor.childNodes[0]);
        contenedor.insertBefore(document.createElement("br"), contenedor.childNodes[0]);
        var divX = document.getElementById(nombreDiv);

        divX.innerHTML = producto;
        divX.style.fontSize = "10px";
        divX.style.maxWidth = "200px";
        divX.style.margin = "1px";
        divX.style.border = "black 1px solid";
        divX.style.borderRadius = "10px";
        divX.style.display = "flex";
        divX.style.padding = "3px";

        //border - top: 2px solid black;
        //border - left: 2px solid black;
        //border - right: 2px solid black;
        //border - bottom: 2px solid black;

        var qrcode = new QRCode(document.getElementById(nombreDiv), {
            width: 80,
            height: 80,

        });        

        qrcode.makeCode(producto);
    }

    var nroDiv = 1;    

    //var qrcode = new QRCode(document.getElementById('qrResult0'), {
    //    width: 80,
    //    height: 80
    //});

    //qrcode.makeCode(producto);

    //qrcode = new QRCode(document.getElementById('qrResult1'), {
    //    width: 80,
    //    height: 80
    //});

    ////document.getElementById('etiqueta1').innerHTML = producto;
    
    //qrcode.makeCode(producto);
});

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}