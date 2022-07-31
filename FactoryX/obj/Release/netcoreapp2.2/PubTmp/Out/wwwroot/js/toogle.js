function Toggle() {
    var temp = document.getElementById("reveal");
    var itemp = document.getElementById("eye");
    if (temp.type === "password") {
        temp.type = "text";
        itemp.className = "fa fa-eye-slash";
    }
    else {
        temp.type = "password";
        itemp.className = "fa fa-eye";
    }
}

function ToggleI() {
    var temp = document.getElementById("revealI");
    var itemp = document.getElementById("eyeI");
    if (temp.type === "password") {
        temp.type = "text";
        itemp.className = "fa fa-eye-slash";
    }
    else {
        temp.type = "password";
        itemp.className = "fa fa-eye";
    }
} 