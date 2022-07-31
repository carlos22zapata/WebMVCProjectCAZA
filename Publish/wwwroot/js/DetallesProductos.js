function abrirModalQR(selectedItems) {
    var ca = selectedItems.row.data;

    document.getElementById('prd').innerHTML = ca.Cod_producto + " - " + ca.Des_producto;

    if (document.getElementById('divQr').style.display === 'none') {
        document.getElementById('divQr').style.display = 'inline';
    }

    qrcode.makeCode(ca.Cod_producto + " - " + ca.Des_producto);
}

function abrirEtiquetas(idEmpresa, ) {

    // la idea es abrir un controlador con ajax y desde alli abrir una nueva ventana con el reporte

    var cod_producto = document.getElementById('prd').innerHTML;
    var cantidad = document.getElementById('NroEtiquetas').value;

    if (cod_producto.trim() === "") {
        Swal.fire({
            title: 'Debe seleccionar al menos un producto para generar el reporte',
            html: 'Seleccione un producto',
            icon: 'info'
        });
    }
    else {
        window.location.href = "ReporteEtiquetasProductos?datos=" + cantidad + "|" + cod_producto;
    }    
}