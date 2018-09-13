
function Setpath(p) {
    document.getElementById('ImagePoster').src = p.value.replace('C:\\fakepath\\', '\\image\\poster\\');
}

function OnlyNumber(p) {
    if (p.value.length == 1)
        p.value = p.value.replace(/[^1-9]/g, '')
    else
        p.value=p.value.replace(/\D/g,'')
}

function add_screening() {
    var table = document.getElementById('TableScreenings');
    if (document.getElementById('TBAddScreeningID').value == "" ||
        document.getElementById('TBAddLocation').value == "" ||
        document.getElementById('TBAddTime').value == "" ||
        document.getElementById('TBAddPrice').value==""||
        document.getElementById('TBAddSeat').value=="") {
        alert("请填写场次的全部信息");
        return;
    }
    var table_count = table.rows.length;
    for (var i = 0; i < table_count; i++) {
        if (table.rows[i].cells[0].innerHTML == document.getElementById('TBAddScreeningID').value) {
            alert("已存在该场次");
            return;
        }
    }

    var row = table.insertRow(table.rows.length);
    var cell_screeningid = row.insertCell(0);
    cell_screeningid.innerHTML = document.getElementById('TBAddScreeningID').value;
    var cell_location = row.insertCell(1);
    cell_location.innerHTML = document.getElementById('TBAddLocation').value;
    var cell_time = row.insertCell(2);
    cell_time.innerHTML = document.getElementById('TBAddTime').value;
    var cell_price = row.insertCell(3);
    cell_price.innerHTML = document.getElementById('TBAddPrice').value;
    var cell_seat = row.insertCell(4);
    cell_seat.innerHTML = document.getElementById('TBAddSeat').value;
    var cell_del = row.insertCell(5);
    cell_del.innerHTML = '<input type="button" value="删除" onclick="return DelRow()"/>';
    var textbox_per = document.getElementById('TBPer');
    textbox_per.value += document.getElementById('TBAddScreeningID').value + "#" + document.getElementById('TBAddLocation').value + "#" + document.getElementById('TBAddTime').value + "#" + document.getElementById('TBAddPrice').value + "#" + document.getElementById('TBAddSeat').value + "\n";
}

function DelRow() {
    document.getElementById('TableScreenings').deleteRow(event.srcElement.parentNode.parentNode.rowIndex);
}