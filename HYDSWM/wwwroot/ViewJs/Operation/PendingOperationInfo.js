$(document).ready(function () {

    $('#txtFromDate').datetimepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });
    $('#txtToDate').datetimepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });

    var date = new Date();
    document.getElementById("txtFromDate").value = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + '00:00';
    document.getElementById("txtToDate").value = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + date.getHours() + ':' + date.getMinutes();
    $('#ddlHKL').select2({
        dropdownParent: $('#modal_form_vertical')
    });
    GetAllUser();
    GetAllTransferStation();
    GetDataTableData('Load');
    //SetValues('Load');
});
var IsClick = 0;
var TData;
function SetValues(Type) {
    ShowLoading($('#dvContent'));
    var TSId = "0";
    if (Type == 'Click') {
        IsClick = 1;
        TSId = $('#ddlTransferStation').val();
    }
    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: '/Operation/GetAllPendingOperation',
        dataType: "json",
        data: { TsId: TSId },
        success: function (data) {
            var myJSON = JSON.parse(data);
            if (TData != null)
                TData = null;
            TData = data;
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

        var arrivalofcontainer, containeropen, pcstatus, containerclose, arrivalofhl, linkagetohl, movetopf, wbstatus, status;


        if (item.DHLTId > 0)
            arrivalofcontainer = "<span class='badge badge-success'><i class='icon-checkmark-circle'></i></span><br><span class='badge badge-success'>" + item.ContainerCode + "</span><br><span class='badge badge-success'>" + item.DCreatedDate + "</span>";
        else
            arrivalofcontainer = "<span class='badge badge-danger'><i class='icon-cancel-circle2'></i></span></span>";

        if (item.COPrimaryCreatedDate != '')
            containeropen = "<span class='badge badge-success'><i class='icon-checkmark-circle'></i></span><br><span class='badge badge-success'>" + item.ContainerCode + "</span><br><span class='badge badge-success'>" + item.COPrimaryCreatedDate + "</span>";
        else
            containeropen = "<span class='badge badge-danger'><i class='icon-cancel-circle2'></i></span>";

        if (item.TotalPV > 0)
            pcstatus = "<span class='badge badge-success'><i class='icon-checkmark-circle'></i></span><br><span class='badge badge-success'>PV-" + item.TotalPV + "</span>";
        else
            pcstatus = "<span class='badge badge-danger'><i class='icon-cancel-circle2'></i></span>";


        if (item.CCPrimaryCreatedDate != '')
            containerclose = "<span class='badge badge-success'><i class='icon-checkmark-circle'></i></span><br><span class='badge badge-success'>" + item.ContainerCode + "</span><br><span class='badge badge-success'>" + item.CCPrimaryCreatedDate + "</span>";
        else
            containerclose = "<span class='badge badge-danger'><i class='icon-cancel-circle2'></i></span>";

        if (item.LHARCreatedDate != '')
            arrivalofhl = "<span class='badge badge-success'><i class='icon-checkmark-circle'></i></span><br><span class='badge badge-success'>" + item.LHVehicleNo + "</span><br><span class='badge badge-success'>" + item.LHARCreatedDate + "</span>";
        else
            arrivalofhl = "<span class='badge badge-danger'><i class='icon-cancel-circle2'></i></span>";

        if (item.LHCreatedDate != '')
            linkagetohl = "<span class='badge badge-success'><i class='icon-checkmark-circle'></i></span><br><span class='badge badge-success'>" + item.LHVehicleNo + "</span><br><span class='badge badge-success'>" + item.LHCreatedDate + "</span>";
        else
            linkagetohl = "<span class='badge badge-danger'><i class='icon-cancel-circle2'></i></span>";

        if (item.SyncedOnWB != '')
            movetopf = "<span class='badge badge-success'><i class='icon-checkmark-circle'></i></span><br><span class='badge badge-success'>" + item.SyncedOnWB + "</span>";
        else
            movetopf = "<span class='badge badge-danger'><i class='icon-cancel-circle2'></i></span>";

        if (item.WBCreatedDate != '')
            wbstatus = "<span class='badge badge-success'><i class='icon-checkmark-circle'></i></span><br><span class='badge badge-success'>" + item.WBCreatedDate + "</span>";
        else
            wbstatus = "<span class='badge badge-danger'><i class='icon-cancel-circle2'></i></span>";

        status = "<span class='badge badge-danger'>Open</span>";

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<td>" + item.TSName +
            "</td>" + "<td >" + arrivalofcontainer +
            "</td>" + "<td>" + containeropen +
            "</td>" + "<td>" + pcstatus +
            "</td>" + "<td class='col-sm-2 Column-VerticalLine'>" + containerclose +
            "</td>" + "<td>" + arrivalofhl +
            "</td>" + "<td>" + linkagetohl +
            "</td>" + "<td>" + movetopf +
            "</td>" + "<td>" + wbstatus +
            "</td>" + "<td>" + status +
            "</td>" + "<td>" + item.DCreatedBy +
            "</td>" + "<td>" + item.LastActivityOn +
            "</td></tr>";
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

function GetAllTransferStation() {
    $('#ddlTransferStation').html('');

    $.ajax({
        type: "post",
        url: "/Operation/GetAllTransferStationByUser",
        data: '{}',
        success: function (data) {
            var myJSON = JSON.parse(data);
            var Resource = "<select id='ddlTransferStation'>";
            Resource = Resource + '<option value="0">All Transfer Station</option>';
            // Resource = Resource + '<option value="0">N/A</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource = Resource + '<option value=' + myJSON[i].TSId + '>' + myJSON[i].TStationName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlTransferStation').html(Resource);
        }

    });
}

function GetAllUser() {
    $('#ddlUser').html('');

    $.ajax({
        type: "post",
        url: "/User/GetAllActiveUser",
        data: '{}',
        success: function (data) {
            var myJSON = JSON.parse(data);
            var Resource = "<select id='ddlUser'>";
            Resource = Resource + '<option value="0">All User</option>';
            // Resource = Resource + '<option value="0">N/A</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource = Resource + '<option value=' + myJSON[i].EmailId + '>' + myJSON[i].EmailId + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlUser').html(Resource);
        }

    });
}

function DownloadFile(FType) {
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var TSId = "0";
    var Status = "-1";
    var UserId = '0';

    if (IsClick == 1) {
        TSId = $('#ddlTransferStation').val();
        Status = $('#ddlStatus').val();
        UserId = $('#ddlUser').val();
    }

    var TName = "Operation Monitoring From Date-" + FDate + " To Date-" + TDate;

    ShowLoading($('#dvContent'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportOpenOperationSummaryData",
        data: { TSId: TSId, Status: Status, FromDate: FDate, ToDate: TDate, VehicleTypeId: UserId, FName: TName },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#dvContent'));
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
            HideLoading($('#dvContent'));
        }
    });
}

var dt;
function GetDataTableData(Type) {
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var TSId = "0";
    var Status = "-1";
    var UserId = '0';

    if (Type == 'Click') {
        IsClick = 1;
        TSId = $('#ddlTransferStation').val();
        Status = $('#ddlStatus').val();
        UserId = $('#ddlUser').val();
    }
    dt = $('#example').DataTable({
        processing: true,
        destroy: true,
        responsive: true,
        serverSide: true,
        searchable: true,
        lengthMenu: [[10, 20, 50, 100, 500, 10000, -1], [10, 20, 50, 100, 500, 10000, "All"]],
        language: {
            infoEmpty: "No records available",
            searchPlaceholder: "Search Records"
        },
        dom: 'Blfrtip',
        buttons: {
            buttons: [
                {
                    extend: 'copyHtml5',
                    className: 'btn btn-light',
                    text: '<i class="icon-copy3 mr-2"></i> Copy'
                },
                //{
                //    extend: 'csvHtml5',
                //    className: 'btn btn-light',
                //    text: '<i class="icon-file-spreadsheet mr-2"></i> CSV',
                //    extension: '.csv'
                //},
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
            url: "/Operation/GetAllPendingOperation/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {
                d.FromDate = FDate;
                d.ToDate = TDate;
                d.Status = TSId;
                d.NotiId = Status;
                d.VehicleTypeId = UserId;
                return { requestModel: d };
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

            { data: "DCreatedDate", sortable: false },
            { data: "TSName", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.DHLTId > 0 )
                        return '<span class="badge badge-success"><i class="icon-checkmark-circle"></i></span><br><span class="badge badge-success">' + row.ContainerCode + '</span><br><span class="badge badge-success">' + row.DCreatedDate + '</span>';
                   
                    else
                        return '<span class="badge badge-danger"><i class="icon-cancel-circle2"></i></span><br><span class="badge badge-danger">NO</span>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.TotalPV > 0)
                        return '<span class="badge badge-success"><i class="icon-checkmark-circle"></i></span><br><span class="badge badge-success">PV-' + row.TotalPV + '</span>';
                    else
                        return '<span class="badge badge-danger"><i class="icon-cancel-circle2"></i></span><br><span class="badge badge-danger">PV-0</span>';

                }
            },
            {
                sortable: false,
                "className": "col-sm-2 Column-VerticalLine",
                "render": function (data, type, row, meta) {

                    if (row.CCPrimaryCreatedDate != '')
                        return '<span class="badge badge-success"><i class="icon-checkmark-circle"></i></span><br><span class="badge badge-success">' + row.CCPrimaryCreatedDate + '</span>';
                    else
                        return '<span class="badge badge-danger"><i class="icon-cancel-circle2"></i></span><br><span class="badge badge-danger">NO</span>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.LHARCreatedDate != '')
                        return '<span class="badge badge-success"><i class="icon-checkmark-circle"></i></span><br><span class="badge badge-success">' + row.LHARVehicleNo + '</span><br><span class="badge badge-success">' + row.LHARCreatedDate + '</span>';
                    else
                        return '<span class="badge badge-danger"><i class="icon-cancel-circle2"></i></span><br><span class="badge badge-danger">NO</span>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.LHCreatedDate != '') {
                        var TType = ''
                        if (row.HKLTType == 'Edited')
                            TType = '<span class="badge badge-danger">' + row.HKLTType + '</span>';
                        else
                            TType = '<span class="badge badge-success">' + row.HKLTType + '</span>';

                        return '<span class="badge badge-success"><i class="icon-checkmark-circle"></i></span><br><span class="badge badge-success">' + row.LHVehicleNo + '</span><br><span class="badge badge-success">' + row.LKHContainerNo + '</span><br><span class="badge badge-success">' + row.LHCreatedDate + '</span><br>' + TType;
                    }
                    else
                        return '<span class="badge badge-danger"><i class="icon-cancel-circle2"></i></span><br><span class="badge badge-danger">NO</span>';

                }
            },
            {
                sortable: false,

                "render": function (data, type, row, meta) {

                    if (row.SyncedOnWB != '')
                        return '<span class="badge badge-success"><i class="icon-checkmark-circle"></i></span><br><span class="badge badge-success">YES</span>';
                    else
                        return '<span class="badge badge-danger"><i class="icon-cancel-circle2"></i></span><br><span class="badge badge-danger">NO</span>';

                }
            },
            {
                sortable: false,
                "className": "col-sm-2 Column-VerticalLine",
                "render": function (data, type, row, meta) {

                    if (row.WBCreatedDate != '') {
                        var TType = ''
                        if (row.WBTType == 'Edited')
                            TType = '<span class="badge badge-danger">' + row.WBTType + '</span>';
                        else
                            TType = '<span class="badge badge-success">' + row.WBTType + '</span>';

                        return '<span class="badge badge-success"><i class="icon-checkmark-circle"></i></span><br><span class="badge badge-success">' + row.WBHKLNo + '</span><br><span class="badge badge-success">' + row.NetWt + '</span><br><span class="badge badge-success">' + row.WBCreatedDate + '</span>' + TType;
                    }
                        else
                        return '<span class="badge badge-danger"><i class="icon-cancel-circle2"></i></span><br><span class="badge badge-danger">NO</span>';

                }
            },
            { data: "TotalWTPV", sortable: false },
            { data: "NetWt", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.Status == 'CLOSED')
                        return '<span class="badge badge-success">CLOSED</span>';
                    else
                        return '<span class="badge badge-danger">OPEN</span>';

                }
            },
            { data: "DCreatedBy", sortable: false },
            { data: "LastActivityOn", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    if (row.DHLTId > 0 && row.TotalPV == 0)
                        return '<a dhltid=' + row.DHLTId + ' href="javascript:void(0);" title="Delete Container" onclick="RemoveContainer(this);"><i class="ti-close"></i></a>';
                   else if (row.LHCreatedDate == '' && row.TotalPV > 0)
                        return '<a DUId=' + row.DUId + ' TSId=' + row.TSId + ' PCId=' + row.COPrimaryId + ' DHLTId=' + row.DHLTId + ' LastTDate="' + row.LastTDate + '" href="javascript:void(0);" title="Add Hook Loader" onclick="CallFunc(this);"><i class="ti-pencil"></i></a>';
                    else if (row.LHCreatedDate != '' && row.WBCreatedDate=='')
                        return '<a OperationTypeId=' + row.OperationTypeId + ' TSId=' + row.TSId + ' TSName="' + row.TSName + '" PCId=' + row.COPrimaryId + ' DHLTId=' + row.DHLTId + ' VHLTId=' + row.LHDHLId + ' VUID=' + row.VUID + ' LHVehicleNo="' + row.LHVehicleNo + '" LHTDate="' + row.LHTDate + '" href="javascript:void(0);" title="Add WB Data" onclick="CallWBFunc(this);"><i class="ti-trello"></i></a>';
                    else
                        return '';
                }
            }
        ]
    });

}

function RemoveContainer(obj) {

    var ddId = $(obj).attr('dhltid');

    $.ajax({
        type: "POST",
        url: '/Deployment/RemoveArvlContainerById',
        data: { DHLTId: ddId },
        success: function (data) {
            var myJson = JSON.parse(data);
            if (myJson.Result == 1 || myJson.Result == 2) {

                ShowCustomMessage('1', myJson.Msg, '/Operation/AllPendingOperation');

            }
            else
                ShowCustomMessage('0', myJson.Msg, '');

        },
        error: function (result) {
            ShowCustomMessage('0', 'Something Went Wrong!', '');
        }
    });
}


function CallFunc(obj) {
    $("#hfPCId").val('0');
    $("#hfTSId").val('0');
    $("#hfLDate").val('');
    $("#hfDUId").val('');
    $("#hfDHLTId").val('0');


    var DUId = $(obj).attr('DUId');
    var DHLTId = $(obj).attr('DHLTId');
    var TSId = $(obj).attr('TSId');
    var PCId = $(obj).attr('PCId');
    var LastTDate = $(obj).attr('LastTDate');
    var kd = LastTDate.split(" ")[0];
    var ktime = LastTDate.split(" ")[1];

    $("#hfPCId").val(PCId);
    $("#hfTSId").val(TSId);
    $("#hfLDate").val(LastTDate);
    $("#hfDUId").val(DUId);
    $("#hfDHLTId").val(DHLTId);
    //$('#txtTDate').prop({
    //    min: LastTDate
    //});

    $('#modal_form_vertical').modal('toggle');


    setTimeout(function () {

        $('#txtTDate').datetimepicker({
            changeMonth: true,
            changeYear: true,
            minDate: kd,
            maxDate: '0'
        });
        document.getElementById("txtTDate").value = LastTDate;
        GetAllHKL(TSId);
    }, 1000);
}
function Formsubmit() {

    AddHKL();
    return false;
}
function AddHKL() {
    var input = {
        VUId: $("#ddlHKL").val(),
        CUId: $("#hfDUId").val(),
        CDHLTId: $("#hfDHLTId").val(),
        CTSId: $("#hfTSId").val(),
        PCId: $("#hfPCId").val(),
        LastTDate: $("#hfLDate").val(),
        TDate: $("#txtTDate").val()
    }


    if (input.CUId != '' && input.VUId != '' && input.PCId > 0 && input.LastTDate != '' && input.TDate != '') {
        $.ajax({
            type: "POST",
            url: '/Deployment/AddHKL',
            data: { jobj: JSON.stringify(input) },
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Operation/AllPendingOperation');

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
        ShowCustomMessage('0', 'Please Select HKL And Transaction Date!', '');
}
var checkPastTime = function (inputDateTime) {

    if (typeof (inputDateTime) != "undefined" && inputDateTime !== null) {
        var current = new Date();

        //check past year and month
        if (inputDateTime.getFullYear() < current.getFullYear()) {
            $('#datetimepicker7').datetimepicker('reset');
            alert("Sorry! Past date time not allow.");
        } else if ((inputDateTime.getFullYear() == current.getFullYear()) && (inputDateTime.getMonth() < current.getMonth())) {
            $('#datetimepicker7').datetimepicker('reset');
            alert("Sorry! Past date time not allow.");
        }

        // 'this' is jquery object datetimepicker
        // check input date equal to todate date
        if (inputDateTime.getDate() == current.getDate()) {
            if (inputDateTime.getHours() < current.getHours()) {
                $('#datetimepicker7').datetimepicker('reset');
            }
            this.setOptions({
                minTime: current.getHours() + ':00' //here pass current time hour
            });
        } else {
            this.setOptions({
                minTime: false
            });
        }
    }
};

function GetAllHKL(TSId) {
    $('#ddlHKL').html('');

    $.ajax({
        type: "post",
        url: "/Deployment/GetAllHKLForOperation",
        data: { TSId: TSId },
        success: function (data) {
            var myJSON = JSON.parse(data);
            var Resource = "<select id='ddlHKL'>";
            Resource = Resource + '<option value="">Select  Hookloader</option>';
            // Resource = Resource + '<option value="0">N/A</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource = Resource + '<option value=' + myJSON[i].CUId + '>' + myJSON[i].VehicleNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlHKL').html(Resource);
        }

    });
}

function CallWBFunc(obj) {
    $("#hfWBPCId").val('0');
    $("#hfWBTSId").val('0');
    $("#hfWBLDate").val('');
    $("#hfWBVUId").val('');
    $("#hfWBDHLTId").val('0');
    $("#hfWBVDHLTId").val('0');
    $("#hfWBTSName").val('');
    $("#hfWBOperationTypeId").val('0');
    $("#hfWBVNo").val('');


    var OperationTypeId = $(obj).attr('OperationTypeId');
    var TSId = $(obj).attr('TSId');
    var TSName = $(obj).attr('TSName');
    var PCId = $(obj).attr('PCId');
    var DHLTId = $(obj).attr('DHLTId');
    var VHLTId = $(obj).attr('VHLTId');
    var VUID = $(obj).attr('VUID');
    var LHVehicleNo = $(obj).attr('LHVehicleNo');
    var LHTDate = $(obj).attr('LHTDate');
    var kd = LHTDate.split(" ")[0];
    var ktime = LHTDate.split(" ")[1];

    $("#hfWBPCId").val(PCId);
    $("#hfWBTSId").val(TSId);
    $("#hfWBLDate").val(LHTDate);
    $("#hfWBVUId").val(VUID);
    $("#hfWBDHLTId").val(DHLTId);
    $("#hfWBOperationTypeId").val(OperationTypeId);
    $("#hfWBTSName").val(TSName);
    $("#hfWBVDHLTId").val(VHLTId);
    $("#hfWBVNo").val(LHVehicleNo);
    

    $('#modal_form_vertical_WB').modal('toggle');


   

    $('#txtWBTDate').datetimepicker({
            changeMonth: true,
            changeYear: true,
            minDate: kd,
            maxDate: '0'
        });
    document.getElementById("txtWBTDate").value = LHTDate;
        
   
}

function WBFormsubmit() {

    AddManualWBRelease();
    return false;
}

function AddManualWBRelease() {
   
    var input = {
        PCId: $("#hfWBPCId").val(),
        TSId: $("#hfWBTSId").val(),
        VUID: $("#hfWBVUId").val(),
        CDHLTId: $("#hfWBDHLTId").val(),
        VDHLTId: $("#hfWBVDHLTId").val(),
        TSName: $("#hfWBTSName").val(),
        OperationTypeId: $("#hfWBOperationTypeId").val(),
        VehicleNo: $("#hfWBVNo").val(),
        GrossWt: $("#txtGrossWt").val(),
        TareWt: $("#txtTareWt").val(),
        NetWt: $("#txtNetWt").val(),
        TDate: $("#txtWBTDate").val()
    }
    

    if (input.VUID != '' && input.TSName != '' && input.PCId > 0 && input.VehicleNo != '' && input.TDate != '') {
        $.ajax({
            type: "POST",
            url: '/Deployment/AddManualWBRelease',
            data: { jobj: JSON.stringify(input) },
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Operation/AllPendingOperation');

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
        ShowCustomMessage('0', 'Please Add Required Fields!', '');
}