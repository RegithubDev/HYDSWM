var dt;
var IsClick = 0;
function GetDataTableData(Type) {
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;


    if (Type == 'Click') {
        IsClick = 1;
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
            url: "/RptOperation/GetAllWBData_Paging/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {
                d.FromDate = FDate;
                d.ToDate = TDate;
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

            { data: "Vehicleno", sortable: true },
            { data: "GrossWt", sortable: true },
            { data: "TareWt", sortable: true },
            { data: "Netwt", sortable: true },
            { data: "SiteID", sortable: true },

            {
                sortable: false,
                "render": function (data, type, row, meta) {


                    return row.TDate.replace('T', ' ');


                }
            },
            { data: "Material", sortable: false },
            { data: "Party", sortable: false },
            { data: "Transporter", sortable: false },
            { data: "BillDCno", sortable: false },
            { data: "Billweight", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {


                    return row.InDate.replace('T', ' ');


                }
            },
            { data: "User1", sortable: false },
            { data: "User2", sortable: false },
            { data: "Status", sortable: false },
            { data: "SW_SiteID", sortable: false },
            { data: "TripNo", sortable: false },
            { data: "ShiftNo", sortable: false },
            { data: "TransferWasteIE", sortable: false },
            { data: "TransferWaste", sortable: false },
            { data: "Remarks", sortable: false },
            { data: "ManifestNumber", sortable: false },
            { data: "ManifestWeight", sortable: false },
            { data: "MembershipCode", sortable: false },
            { data: "InGatePassNo", sortable: false },
            { data: "InMeterReading", sortable: false },
            { data: "OutGatePassNo", sortable: false },
            { data: "OutMeterReading", sortable: false },
            { data: "TransferID", sortable: false },
            { data: "TypeOfWaste", sortable: false },
            { data: "TotalKMSTravelled", sortable: false },
            { data: "BillableWeight", sortable: false },
            { data: "TotalTransportCharges", sortable: false }
        ]
    });

}

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
    GetDataTableData('Load');
    //SetValues('Load');
});

function DownloadFile(FType) {
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;


    var TName = "Master WB Report From Date-" + FDate + " To Date-" + TDate;

    ShowLoading($('#dvContent'));
    $.ajax({
        type: "POST",
        url: "/RptOperation/ExportMasterWBData",
        data: { FromDate: FDate, ToDate: TDate, FName: TName },
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
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;


    var TName = "Master WB Report From Date-" + FDate + " To Date-" + TDate;

    $.ajax({
        url: '/RptOperation/ExportMasterWBCSV', // URL of your controller action
        method: 'POST',
        data: { FromDate: FDate, ToDate: TDate, FName: TName },
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