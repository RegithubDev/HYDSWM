$(document).ready(function () {
    GetDataTableData('Load');

});

function GetDataTableData(Type) {
    var date = new Date();

    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var TSId = getUrlParameterInfo('TSId');
    var IsCompleted = getUrlParameterInfo('IsCompleted');
    var UIDType = getUrlParameterInfo('UIDType');
    var Status = getUrlParameterInfo('Status');
    var FDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + '00:00';
    var TDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + date.getHours() + ':' + date.getMinutes();

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
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

            { data: "UId", sortable: true },
            { data: "EntityType", sortable: true },
            { data: "EntityCode", sortable: true },
            { data: "TStationName", sortable: true },
            { data: "Status", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.DeviationStatus == "INSIDE")
                        return '<span class="badge badge-success">' + row.DeviationStatus + '</span>';
                    else
                        return '<span class="badge badge-danger">' + row.DeviationStatus + '</span>';

                }
            },
            /*{ data: "Step2CreatedOn", sortable: true },*/
            { data: "CreatedOn", sortable: true },
            { data: "TotalHourTaken", sortable: false }
        ]
    });

    if (Status == "0")
        dt.column(8).visible(false);
}


function DownloadFile(FType) {

    var date = new Date();

    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var TSId = getUrlParameterInfo('TSId');
    var IsCompleted = getUrlParameterInfo('IsCompleted');
    var UIDType = getUrlParameterInfo('UIDType');
    var Status = getUrlParameterInfo('Status');
    var FDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + '00:00';
    var TDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + date.getHours() + ':' + date.getMinutes();

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
    }

    var TName = "All Container";
  
    ShowLoading($('#dvNotification'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportAllArvlOfEntityOpt1Noti",
        data: { Status: TSId, Route: IsCompleted, NotiId: UIDType, VehicleTypeId: Status, FromDate: FDate, ToDate: TDate, FName: TName },
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
