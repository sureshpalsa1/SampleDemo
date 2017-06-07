var isShift = false;
var seperator = "/";

function IsValidDate(txt, keyCode, year, month, allowToday) {

    var len = txt.value.length;
    if (len > 0) {
        //if (keyCode == 16)
        //    isShift = false;
        var val = txt.value;
        //if (val.length >= 10) {

        if (val.length == 10) {
            var splits = val.split("/");
            var dt = new Date(splits[1] + "/" + splits[0] + "/" + splits[2]);

            //Validation for Dates
            if (dt.getDate() == splits[0] && dt.getMonth() + 1 == splits[1] && dt.getFullYear() == splits[2]) {
                //alert("Valid Date");
            }
            else {
                alert(val + " is invalid Date");
                txt.value = '';
                return;
            }

            //IsCorrectYear(txt, dt, year, month, allowToday)
        }
        else {
            alert(val + " is invalid Date");
            txt.value = '';
        }
        //}
        //else {
        //    alert(val + " is invalid Date");
        //    txt.value = '';
        //}
    }
}
function IsCorrectYear(txt, dt, year, month, allowToday) {
    var dtToday = new Date((new Date()).setHours(0, 0, 0, 0));

    var pastDate = new Date(Date.parse(parseInt(dtToday.getMonth() + 1 - month) + "/" + dtToday.getDate() + "/" + parseInt(dtToday.getFullYear() - year)));

    if (allowToday == 1) {
        if (dt <= pastDate || dt > dtToday) {

            if (dt > dtToday) {
                alert("You entered : " + txt.value + "\n Future dates are not allowed.");
                txt.value = '';
            }
            else {
                if (year > 0 || month > 0) {
                    alert("You entered : " + txt.value + "\n Past dates more than " + year + " year(s) and " + month + " month(s) are not allowed.");
                    txt.value = '';
                }
            }
        }
    }
    else if (dt <= pastDate || dt >= dtToday) {

        if (dt >= dtToday) {
            alert("You entered : " + txt.value + "\n Future dates are not allowed including today.");
            txt.value = '';
        }
        else {
            if (year > 0 || month > 0) {
                alert("You entered : " + txt.value + "\n Past dates more than " + year + " year(s) and " + month + " month(s) are not allowed.");
                txt.value = '';
            }
        }
    }
}

function IsValidFormat(txt, keyCode) {
    var len = txt.value.length;
    //if (len > 0) {
    //if (keyCode == 16)
    //    isShift = true;

    //Validate that its Numeric
    //if (((keyCode >= 48 && keyCode <= 57) || keyCode == 8 || keyCode == 9 || keyCode == 46 || keyCode <= 37 || keyCode <= 39 || (keyCode >= 96 && keyCode <= 105)) && isShift == false) {

    //    if ((txt.value.length == 2 || txt.value.length == 5) && keyCode != 8) {
    //        txt.value += seperator;
    //    }
    //    return true;
    //}
    //else {
    //    return false;
    //}
    //}
}

function IsAuditDate(txt, keyCode) {

    var len = txt.value.length;
    if (len > 0) {
        var val = txt.value;
        var splits = val.split("/");
        var dt = new Date(splits[1] + "/" + splits[0] + "/" + splits[2]);

        //Validation for Dates
        if ((dt.getDate() == splits[0]) && (dt.getMonth() + 1 == splits[1]) && (dt.getFullYear() == splits[2])) {
            //alert("Valid Date");
        }
        else {
            alert(val + " is invalid Date");
            txt.value = '';
            return;
        }
    }
}