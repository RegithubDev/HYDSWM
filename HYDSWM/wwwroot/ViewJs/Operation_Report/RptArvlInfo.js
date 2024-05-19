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
   
    GetAllTransferStation();
    GetDataTableData('Load');
    //SetValues('Load');
});
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
var IsClick = 0;
var dt;

function GetDataTableData(Type) {

   
    var TSId = '0';
    var IsCompleted = '-1';
    var UIDType = 'CONTAINER';
    var Status = '0';
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;

    if (Type == 'Click') {
        TSId = $('#ddlTransferStation').val();
        UIDType = $('#ddlEntityType').val();
    }

    dt = $('#example').DataTable({
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
            url: "/Operation/GetAllArvlOfEntityOpt1Noti/",
            type: 'POST',
            data: function (d) {
                d.Status = TSId;
                d.Route = IsCompleted;
                d.NotiId = UIDType;
                d.FromDate = FDate;
                d.ToDate = TDate;
                d.VehicleTypeId = Status;
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

            { data: "CreatedOn", sortable: true },
            { data: "ZoneNo", sortable: true },
            { data: "TStationName", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (UIDType =='CONTAINER')
                        return row.UIdType;
                    else
                        return row.EntityType

                }
            },
            { data: "EntityCode", sortable: true }
        ]
    });

 
}

function DownloadFile(FType) {
    

    var TSId = '0';
    var IsCompleted = '-1';
    var UIDType = 'CONTAINER';
    var Status = '0';
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;

    if (IsClick == 1) {
        TSId = $('#ddlTransferStation').val();
        UIDType = $('#ddlEntityType').val();
    }


    var TName = "Arrival Entity Report From Date-" + FDate + " To Date-" + TDate;

    ShowLoading($('#dvContent'));
    $.ajax({
        type: "POST",
        url: "/RptOperation/ExportArvlEntityData",
        data: { Status: TSId, Route: IsCompleted, NotiId: UIDType, VehicleTypeId: Status, FromDate: FDate, ToDate: TDate, FName: TName },
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


function CSVData(FType) {
    
    // Assuming you have jQuery loaded
    var TSId = '0';
    var IsCompleted = '-1';
    var UIDType = 'CONTAINER';
    var Status = '0';
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;

    if (IsClick == 1) {
        TSId = $('#ddlTransferStation').val();
        UIDType = $('#ddlEntityType').val();
    }


    var TName = "Arrival Entity Report From Date-" + FDate + " To Date-" + TDate;

    $.ajax({
        url: '/RptOperation/ExportArvlEntityCSV', // URL of your controller action
        method: 'POST',
        data: { Status: TSId, Route: IsCompleted, NotiId: UIDType, VehicleTypeId: Status, FromDate: FDate, ToDate: TDate, FName: TName },
        success: function (csvData) {

            // For downloading the CSV as a file
            var blob = new Blob([csvData], { type: 'text/csv' });
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = TName + '.csv';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });


}