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
    AllMZoneLst('ddlZone', 0, 'All Zone');
    // AllVehicleTypeLst('ddlVehicleType', 0, 'All Vehicle Type');
    GetAllTransferStation();
    GetDataTableData('Load');

});

function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}

//biding table and get data function
var dt;
var IsClick = 0;
function GetDataTableData(Type) {
   
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var TSId = "0";

    if (Type == 'Click') {
        IsClick = 1;
        //zid = $('#ddlZone').val();//$('#ddlDesignation').find(":selected").attr('value') == undefined ? "0" : $('#ddlDesignation').find(":selected").attr('value');
        // cid = $('#ddlCircle').val();
        // VId = $('#ddlVehicleType').val();
        TSId = $('#ddlTransferStation').val();
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
            url: "/Operation/GetAllRptContainerforHKL/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {
                d.FromDate = FDate;
                d.ToDate = TDate;
                //d.ZoneId = zid;
                //d.Shift = TId.value;
                //d.CircleId = cid;
                //d.VehicleTypeId = VId;
                d.Status = TSId;
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
            { data: "CreatedDate", sortable: true },
            { data: "ZoneNo", sortable: true },
            { data: "TStationName", sortable: true },
            { data: "ContainerCode", sortable: true },
            { data: "VehicleNo", sortable: true },

            //{
            //    sortable: false,
            //    "render": function (data, type, row, meta) {

            //        if (row.WBTDate != '')
            //            return '<span class="badge badge-success">' + row.WBTDate + '</span>';
            //        else
            //            return '<span class="badge badge-danger">Not Reached At WB</span>';

            //    }
            //},
            //{ data: "Distance", sortable: false },
            //{ data: "TotalTrip", sortable: false }
        ]
    });

}

//function for Download excel file
function DownloadFile(FType) {

    
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var TSId = "0";

    if (IsClick == 1) {
        zid = $('#ddlZone').val();
        TSId = $('#ddlTransferStation').val();
    }

    var TName = "Linkage Report";

    ShowLoading($('#dvNotification'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportAllRptContainerforHKL",
        data: { Status: TSId, FromDate: FDate, ToDate: TDate, FName: TName },
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