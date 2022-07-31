$(document).ready(function () {

    var sPaginaURL = window.location.search.substring(1);

    if (sPaginaURL == "Lon=1") {
        Swal.fire({
            title: 'Sesión bloqueada',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            },
            html: 'Su sesión ha sido bloqueada por que otro usuario inicio sesión con sus credenciales, si desconoce esta acción considere cambiar su clave para aumentar la seguridad de su acceso.',
            icon: 'warning'
        });
    }        
});