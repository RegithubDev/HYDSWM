$(document).ready(function () {
    GetDataTableData3('Load');
    // Add event listener for opening and closing details
    $('#example3 tbody').on('click', 'td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#example3').DataTable().row(tr);
        var CTDId = tr.find('.gticket').attr('cticketid');
        var TDate = tr.find('.gticket').attr('TDate');

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var iccData;
            $.ajax({
                url: "/Operation/GetAllMPrimaryVehicleTransactionByContainer",
                type: "POST",
                dataType: "json",
                data: { CTDId: CTDId, TDate: TDate },
                success: function (data) {
                    var myJSON = JSON.parse(data);
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });
});

var dt3;
function GetDataTableData3(Type) {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var Status = getUrlParameterInfo('Status');
    var TSId = getUrlParameterInfo('TSId');

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
    }

    dt3 = $('#example3').DataTable({
        processing: true,
        destroy: true,
        responsive: true,
        serverSide: true,
        searchable: true,
        lengthMenu: [[10, 20, 50, 100, 500, 10000, -1], [10, 20, 50, 100, 500, 10000, "All"]],
        language: {
            //infoEmpty: "No records available",
            searchPlaceholder: "Search Records",
            search: ""
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
                    text: '<i class="icon-file-spreadsheet mr-2"></i> Excel',
                    extension: '.csv'
                },
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i>',
                    className: 'btn bg-blue btn-icon dropdown-toggle'
                }
            ]
        },
        initComplete: function () {
            $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
        },
        ajax: {
            url: "/Operation/GetAllForceTransactionInfo/",
            type: 'POST',
            data: function (d) {
                d.NotiId = TId;
                d.Route = Status;
                d.Status = TSId;
                return {

                    requestModel: d
                };
            },
            dataType: "json",
            dataSrc: function (json) {

                json.draw = json.draw;
                json.recordsTotal = json.recordsTotal;
                json.recordsFiltered = json.recordsFiltered;
                json.data = json.data;
                var return_data = json;
                return return_data.data;
            }
        },
        columns: [

            {
                sortable: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { data: "OperationType", sortable: true },
            { data: "AssetType", sortable: true },
            { data: "EnType", sortable: true },
            { data: "UId", sortable: true },
            { data: "TStationName", sortable: true },
            { data: "CreatedOn", sortable: true },
            { data: "CreatedBy", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.ActionStatus == 'FORCE TRANSACTION' && TId != '0')
                        return "<a cid='" + row.CTDId + "' OPTId='" + row.OperationTypeId + "' href='javascript:void(0);' title='edit' onclick='CallFuncModal(this);'>" + row.ActionStatus + "</a>";
                    else
                        return row.ActionStatus;

                }
            }
        ]
    });

    //if (TId =="")
    //    dt3.column(8).visible(false);
}

function DownloadFile(FType) {

    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var Status = getUrlParameterInfo('Status');
    var TSId = getUrlParameterInfo('TSId');

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
    }

    var TName = "Transaction Report";

    ShowLoading($('#dvNotification'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportAllForceTransactionOpt1Noti",
        data: { NotiId: TId, Route: Status, Status: TSId, FName: TName },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#dvNotification'));
            var ctype = '';
            if (FType == 'Excel')
                ctype = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
            else
                ctype = 'application/pdf';

            var filename = TName;//
            var blob = new Blob([response], { type: ctype });
            var DownloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = DownloadUrl;
            if (FType == 'Excel')
                a.download = filename + ".xlsx";
            else
                a.download = filename + ".pdf";
            document.body.appendChild(a);
            a.click();
        },
        error: function (result) {
            HideLoading($('#dvNotification'));
        }
    });
}


function CallFuncModal(obj) {
    // loadMap();
    var ddId = $(obj).attr('cid');
    var OperationTypeId = $(obj).attr('OPTId');
    $("#hfFCTDId").val('0');
    $("#hfOPTId").val('0');
    $("#hfFCTDId").val(ddId);
    $("#hfOPTId").val(OperationTypeId);
    $('#modal_form_vertical').modal('toggle');

}

function Formsubmit() {

    ChangeStatusOfTransaction();
    return false;
}
function ChangeStatusOfTransaction() {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var Status = getUrlParameterInfo('Status');
    var TSId = getUrlParameterInfo('TSId');

    var isvalid = 1;
    var FormData = {
        CTDId: $("#hfFCTDId").val(),
        OperationTypeId: $("#hfOPTId").val(),
        TId: TId,
        StatusId: $("#ddlStatus").val(),
    };
    if (FormData.CTDId == '' || FormData.StatusId == '' || FormData.OperationTypeId == '')
        isvalid = 0;

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Operation/ChangeStatusOfTransaction',
            data: { jobj: JSON.stringify(FormData) },
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Operation/AllForceTransactionOpt1Noti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId);

                    $('#modal_form_vertical').modal('toggle');
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