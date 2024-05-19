$(document).ready(function () {
    //AllMZoneLst('ddlSLACategoryType',1,'Select');
    $('#example').DataTable().clear().destroy();
    SetValues();
});

function SetValues() {
    ShowLoading($('#dvContent'));
    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: '/Master/GetAllZoneInfo',
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
            edit = "<td><a cid='" + item.ZId + "' href='javascript: void (0); ' title='edit' onclick='CallFunc(this,1); '><i class='ti-pencil'></i></a></td>";
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
            "</td>" + "<td>" + item.ZoneNo +
            "</td>" + "<td>" + item.Zonecode +
            "</td>" + "<td>" + item.CityNo +
            "</td>" + "<td>" + item.Citycode +
            "</td>" + "<td>" + status +
            "</td>" + edit +
            "</tr>";
        $('#example tbody').append(rows);


    });

    var tabid = $('#example');
    $('#hfTotalrows').val(rowcount);
    if ($('#hfTotalrows').val() > 0)
        setdatatable();

}
function setdatatable() {
    $('#example').DataTable({
        destroy: true,
        "responsive": true,
        "order": [[0, "asc"]],
        lengthMenu: [
            [10, 25, 50, 500, 1000, 5000],
            ['10 rows', '25 rows', '50 rows', '500 rows', '1000 rows', '5000 rows']
        ],
        language: {
            infoEmpty: "No records available",
        },
        dom: 'Blfrtip',
        buttons: {
            buttons: [
                {
                    extend: 'copyHtml5',
                    className: 'btn btn-light',
                    text: '<i class="icon-copy3 mr-2"></i> Copy'
                },
                {
                    extend: 'csvHtml5',
                    className: 'btn btn-light',
                    text: '<i class="icon-file-spreadsheet mr-2"></i> CSV',
                    extension: '.csv'
                },
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i>',
                    className: 'btn bg-blue btn-icon dropdown-toggle'
                }
            ]
        }

    });
}

function CallFunc(obj,x) {
    $("#hfZId").val('0');
    $("#txtZone").val('');
    var ddId = $(obj).attr('cid');
    $('#user_content').load("/Master/AddZone?add_upd="+ x);
    $('#modal_form_AddDetail').modal('toggle');


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
        url: "/Master/GetZoneInfoById",
        data: { ZId: ddId },
        success: function (data) {

            var myJSON = JSON.parse(data);
            $("#hfZId").val(myJSON.ZId);
            $("#txtZone").val(myJSON.ZoneNo);
            $("#txtZoneCode").val(myJSON.Zonecode);

            //var city_zone = myJSON.CityNo;

            if (myJSON.IsActive == 'True')
                $("#ckbIsActive").prop("checked", true);
            else
                $("#ckbIsActive").prop("checked", false);

            
            document.getElementById("ddlCity").value = myJSON.CId;

            

        }
    });
}

function Formsubmit() {
    SaveAndUpdateZone();
    return false;
}
function SaveAndUpdateZone() {
    var isvalid = 1;
    var FormData = {
        ZId: $("#hfZId").val(),
        ZoneNo: $("#txtZone").val(),
        Zonecode: $("#txtZoneCode").val(),
        IsActive: $('#ckbIsActive').is(':checked'),
        CId: $('#ddlCity').val()
    };
    if (FormData.ZoneNo == '' || FormData.Zonecode == '' )
        isvalid = 0;

    if (isvalid == 1) { 
        $.ajax({
            type: "POST",
            url: '/Master/SaveAndUpdateZone',
            data: { jobj: JSON.stringify(FormData) },
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Master/AllZone');

                    $('#modal_form_AddDetail').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJson.Msg, '');

            },
            error: function (result) {
                ShowCustomMessage('0', 'Something Went Wrong!', '');
            }
        });
    }
    else
        ShowCustomMessage('0', 'Please Enter All Required Details', '');

}