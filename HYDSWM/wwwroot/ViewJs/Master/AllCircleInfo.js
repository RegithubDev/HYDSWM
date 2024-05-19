$(document).ready(function () {
   
    $('#example').DataTable().clear().destroy();
    SetValues();
});

function SetValues() {
    ShowLoading($('#dvContent'));
    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: '/Master/GetAllCircle',
        dataType: "json",
        data: { IsAll: "YES" },
        success: function (data) {
            var myJSON = JSON.parse(data);
            bindtable(myJSON);
            HideLoading($('#dvContent'));
        },
        error: function (result) {
            HideLoading($('#dvContent'));
        }
    });
}
function bindtable(data) {
    var CId = getUrlParameterInfo('cid');


    $('#example tbody').empty();

    if ($('#hfTotalrows').val() > 0)
        $('#example').DataTable().clear().destroy();

    var rowcount = data.length;
    $.each(data, function (i, item) {
        var count = i + 1;
        var edit;
        if (CId == '') {
            edit = "<td><a cid='" + item.CircleId + "' href='javascript: void (0); ' title='edit' onclick='CallFunc(this); '><i class='ti-pencil'></i></a></td>";
        }
        else {
            edit = "<td></td>";
            document.getElementById('btnAdd').style.display = "none";
        }

        var status;
        if (item.IsActive == true)
            status = "<span class='badge badge-success'>ACTIVE</span>";
        else
            status = "<span class='badge badge-danger'>DE-ACTIVE</span>"

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<td>" + item.CircleName +
            "</td>" + "<td>" + item.CircleCode +
            "</td>" + "<td>" + item.ZoneNo +
            "</td>" + "<td>" + item.Zonecode +
            "</td>" + "<td>" + item.CityNo +
            "</td>" + "<td>" + item.Citycode +
            "</td>" + "<td>" + status +
            "</td>" + edit +
            "</tr>";
        $('#example tbody').append(rows);


    });

    //var tabid = $('#example');
    $('#hfTotalrows').val(rowcount);
    if ($('#hfTotalrows').val() > 0)
        setdatatableoncontrol('example');

}


function CallFunc(obj,x) {
    $("#hfCircleId").val('0');
    $("#txtCircle").val('');
    var ddId = $(obj).attr('cid');
    $('#user_content').load("/Master/AddCircle?add_upd="+ x);
    $('#modal_form_AddDetail').modal('toggle');

    setTimeout(function () {
        AllMZoneLst('ddlZone', 1, 'Select');
    }, 1000);

    setTimeout(function () {
        AllMCityLst('ddlCity', 1, 'Select');
    }, 1000);

    

    if (ddId > 0) {
        setTimeout(function () {
            SetDataOnControls(ddId);
        }, 2000);
    }
}

function SetDataOnControls(ddId) {
    $.ajax({
        type: "post",
        url: "/Master/GetCircleInfoById",
        data: { CircleId: ddId },
        success: function (data) {

            var myJSON = JSON.parse(data);
            $("#hfCircleId").val(myJSON.CircleId);
            $("#txtCircle").val(myJSON.CircleName);
            $("#txtCircleCode").val(myJSON.CircleCode);
            document.getElementById("ddlZone").value = myJSON.ZoneId;

            if (myJSON.IsActive == 'True')
                $("#ckbIsActive").prop("checked", true);
            else
                $("#ckbIsActive").prop("checked", false);

            document.getElementById("ddlCity").value = myJSON.CId;
        }
            
       
    });
}

function Formsubmit() {

    SaveAndUpdateCircle();
    return false;
}
function SaveAndUpdateCircle() {
    var isvalid = 1;
    var FormData = {
        CircleId: $("#hfCircleId").val(),
        CircleName: $("#txtCircle").val(),
        CircleCode: $("#txtCircleCode").val(),
        ZoneId: $("#ddlZone").val(),
        IsActive: $('#ckbIsActive').is(':checked'),
        CId: $('#ddlCity').val()
    };
    if (FormData.CircleName == '' || FormData.ZoneId == '' || FormData.CircleCode == '' || FormData.CId == '')
        isvalid = 0;

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Master/SaveAndUpdateCircle',
            data: { jobj: JSON.stringify(FormData) },
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Master/AllCircle');

                    $('#modal_form_AddDetail').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJson.Msg, '');

            },
            error: function (result) {
                ShowCustomMessage('0', 'Something Went Wrong!', '');
            }
        });
    else
        ShowCustomMessage('0', 'Please Enter All Required Details', '');

}