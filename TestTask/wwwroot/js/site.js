function ConvertDate(date) {
    let dateConvert = new Date(date);

    let dd = dateConvert.getDate();
    if (dd < 10) dd = '0' + dd;

    let mm = dateConvert.getMonth() + 1;
    if (mm < 10) mm = '0' + mm;

    let dateResult = dd + "/" + mm + "/" + dateConvert.getFullYear();
    return dateResult;
}
function RequestListValuts(date) {
    let stringDateRequestListValuts = ConvertDate(date);
    let url = "/Home/index/?date=" + stringDateRequestListValuts;
    $(document).ready(function () {
        $("#list-valuts").load(url);
    });
};
function RequestDynamicOfValuts(ID) {
    let dateEnd;
    let dateBegin;
    let dateBeginRequest;
    if (ID == "none") {
        dateEnd = ConvertDate(document.getElementById("select-dateEnd-dynamic").value);
        dateBeginRequest = ConvertDate(document.getElementById("select-dateBegin-dynamic").value);
        ID = document.getElementById("id-dynamic").value;
    } else {
        dateEnd = ConvertDate(document.getElementById("select-date-list-valuts").value);
        dateBegin = new Date(document.getElementById("select-date-list-valuts").value);
        dateBegin.setDate(dateBegin.getDate() - 3);
        dateBeginRequest = ConvertDate(dateBegin);
    }
    let url = "/Home/index/?dateBegin=" + dateBeginRequest + "&dateEnd=" + dateEnd + "&ID=" + ID;
    console.log(url);
    document.getElementById("list-valuts").hidden = true;
    $(document).ready(function () {
        $("#dynamic").load(url);
    });
}
function BackToListValuts() {
    document.getElementById("list-valuts").hidden = false;
    let elem = document.getElementById("dynamic-info");
    elem.parentNode.removeChild(elem);
}

