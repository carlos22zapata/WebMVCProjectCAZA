//$('#clock').fitText(1.3);
$(document).ready(function () {
    $('#date').html(moment().locale('es').format('L'));
});

function update() {
    $('#clock').html(moment().locale('es').format('LTS'));   
    //$('#clock').html(moment().locale('es').format('H:mm:ss'));
}
setInterval(update, 1000);