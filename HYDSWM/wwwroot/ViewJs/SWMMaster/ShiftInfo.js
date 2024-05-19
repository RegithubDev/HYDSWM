
$(document).ready(function () {

    $('#example').DataTable().clear().destroy();
    GetTableData();
});

function bindtable(data) {

    $('#example tbody').empty();

    if ($('#hfTotalrows').val() > 0)
        $('#example').DataTable().clear().destroy();

    var rowcount = data.length;
    $.each(data, function (i, item) {
        var count = i + 1;

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<<td>" + item.ShiftName +
            "</td>" + "<<td>" + item.ShiftSTime +
            "</td>" + "<<td>" + item.ShiftETime +
            "</td>" + "<<td>" + item.DedMin +
            "</td>" + "<<td>" + item.ExtMin +
            "</td>" + "<td><a cid='" + item.ShiftId + "' href='javascript: void (0); ' title='edit' onclick='CallFunc(this); '><i class='ti-pencil'></i></a>";
            "</td> </tr>";
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
                    fieldSeparator: '\t',
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
function GetTableData() {

    ShowLoading($('#dvContent'));
    
    $.ajax({
        type: "POST",
        url: '/SWMMaster/AllShiftInfo',
        dataType: "json",
        data: '{}',
        success: function (result) {
            var myJSON = JSON.parse(result);
            bindtable(myJSON);
            HideLoading($('#dvContent'));
        },
        error: function (xhr, status, error) {
            HideLoading($('#dvContent'));
            alert("Result: " + status + " " + error + " " + xhr.status + " " + xhr.statusText)
        }
    });
}

function CallFunc(obj) {

    var ddId = $(obj).attr('cid');
    $('#user_content').load("/SWMMaster/AddShift");
    $('#modal_form_vertical').modal('toggle');
    if (ddId > 0)
        SetDataOnControls(ddId);
}

function Formsubmit() {
    SaveAndUpdateShiftInfo();
    return false;
}

function SaveAndUpdateShiftInfo() {
   
    var isvalid = 1;
    var FormData = {
        ShiftId: $("#hfShiftId").val(),
        ShiftName: $("#txtShiftName").val(),
        ShiftSTime: $("#txtShiftSTime").val(),
        ShiftETime: $("#txtShiftETime").val(),
        BeforBMin: $("#txtBeforBMin").val(),
        AfterBMin: $("#txtAfterBMin").val()
    };


    if (FormData.ShiftName == '' || FormData.ShiftSTime == '' || FormData.ShiftETime == '' || FormData.BeforBMin == '' || FormData.AfterBMin == '')
        isvalid = 0;

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/SWMMaster/AddShiftInfo',
            data: { info: FormData },
            success: function (data) {
                var myJSON = JSON.parse(data);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg,'/SWMMaster/AllShift');

                    $('#modal_form_vertical').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJSON.Msg,'');

            },
            error: function (result) {
                ShowCustomMessage('0', 'Something Went Wrong!','');
            }
        });

}
function SetDataOnControls(SId) {
    $.ajax({
        type: "post",
        url: "/SWMMaster/ShiftInfoById",
        data: { ShiftId: SId },
        success: function (data) {
            var myJSON = JSON.parse(data);
            $("#hfShiftId").val(myJSON.ShiftId);
            $("#txtShiftName").val(myJSON.ShiftName);
            $("#txtShiftSTime").val(myJSON.ShiftSTime);
            $("#txtShiftETime").val(myJSON.ShiftETime);
            $("#txtBeforBMin").val(myJSON.DedMin);
            $("#txtAfterBMin").val(myJSON.ExtMin);
        }
    });
}
