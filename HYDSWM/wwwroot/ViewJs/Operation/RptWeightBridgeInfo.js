﻿
$(document).ready(function () {
    $('#txtFromDate').datepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });
    $('#txtToDate').datepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });
    var date = new Date();
    document.getElementById("txtFromDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    document.getElementById("txtToDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    GetDataTableData('Load');
});
var dt;
var IsClick = '0';
function GetDataTableData(Type) {
    var FromDate = document.getElementById('txtFromDate').value;
    var ToDate = document.getElementById('txtToDate').value;

    if (Type != 'Load') {
        IsClick = '1';
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
            url: "/Operation/GetAllWeightBridgeInfo/",
            type: 'POST',
            data: function (d) {
                d.ToDate = ToDate;
                d.FromDate = FromDate;
                d.NotiId = IsClick;
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
            { data: "WBId", sortable: true },
            { data: "WTCode", sortable: true },
            { data: "TDate", sortable: true },
            { data: "Status", sortable: true },
            { data: "TCode", sortable: true },
            { data: "EntityNo", sortable: true },
            { data: "ContainerCode", sortable: false },
            { data: "GrossWt", sortable: true },
            { data: "TareWt", sortable: true },
            { data: "NetWt", sortable: true },
            { data: "TotalWTPV", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.WTVariance > 0)
                        return '<span class="badge badge-success"><i class="icon-arrow-up8"></i>' + row.WTVarVal + '</span>';
                    else if (row.WTVariance < 0)
                        return '<span class="badge badge-danger"><i class="icon-arrow-down8"></i>' + row.WTVarVal + '</span>';
                    else
                        return '<span class="badge badge-info ">' + row.WTVarVal + '</span>';

                }
            },
            { data: "CreatedDate", sortable: true }

        ]
    });

}

function DownloadFile(FType) {
    var FromDate = document.getElementById('txtFromDate').value;
    var ToDate = document.getElementById('txtToDate').value;

    var TName = "Weight Bridge Report From Date-" + FromDate + " To Date-" + ToDate;

    ShowLoading($('#dvContent'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportWeightBridgeSummaryData",
        data: { NotiId: IsClick, FromDate: FromDate, ToDate: ToDate, FName: TName },
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
    var FromDate = document.getElementById('txtFromDate').value;
    var ToDate = document.getElementById('txtToDate').value;

    var TName = "Weight Bridge Report From Date-" + FromDate + " To Date-" + ToDate;

    $.ajax({
        url: '/Operation/ExportWeightBridgeSummaryCSV', // URL of your controller action
        method: 'POST',
        data: { NotiId: IsClick, FromDate: FromDate, ToDate: ToDate, FName: TName },
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