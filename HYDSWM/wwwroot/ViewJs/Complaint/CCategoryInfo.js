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
        url: '/Master/GetAllComplaintCategory',
        dataType: "json",
        data: { IsAll: "ALL", ComplaintTypeId: 0 },
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

    $('#example tbody').empty();

    if ($('#hfTotalrows').val() > 0)
        $('#example').DataTable().clear().destroy();

    var rowcount = data.length;
    $.each(data, function (i, item) {
        var count = i + 1;
        var edit;
        edit = "<td><a cid='" + item.ComplaintTypeId + "' href='javascript: void (0); ' title='edit' onclick='CallFunc(this); '><i class='ti-pencil'></i></a></td>";


        var status;
        if (item.IsActive == true)
            status = "<span class='badge badge-success'>ACTIVE</span>";
        else
            status = "<span class='badge badge-danger'>DE-ACTIVE</span>"

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<td>" + item.ComplaintType +
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

function CallFunc(obj) {
    $("#hfZId").val('0');
    $("#txtCCategory").val('');
    var ddId = $(obj).attr('cid');
    $('#user_content').load("/Complaint/AddCCategory");
    $('#modal_form_AddDetail').modal('toggle');
    if (ddId > 0) {
        setTimeout(function () {
            SetDataOnControls(ddId);
        }, 2000);
    }
}

function SetDataOnControls(ddId) {
    $.ajax({
        type: "post",
        url: '/Master/GetAllComplaintCategory',
        data: { IsAll: "ALL", ComplaintTypeId: ddId },
        success: function (data) {

            var myJSON = JSON.parse(data);
            $("#hfZId").val(myJSON.ComplaintTypeId);
            $("#txtCCategory").val(myJSON.ComplaintType);
            if (myJSON.IsActive == 'True')
                $("#ckbIsActive").prop("checked", true);
            else
                $("#ckbIsActive").prop("checked", false);

        }
    });
}

function Formsubmit() {
    SaveAndUpdateCCategory();
    return false;
}
function SaveAndUpdateCCategory() {
    var isvalid = 1;
    var FormData = {
        ComplaintTypeId: $("#hfZId").val(),
        ComplaintType: $("#txtCCategory").val(),
        IsActive: $('#ckbIsActive').is(':checked')
    };
    if (FormData.ComplaintType == '')
        isvalid = 0;

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Complaint/SaveAndUpdateCCategory',
            data: { jobj: JSON.stringify(FormData) },
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Complaint/AllComplaintCategory');

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