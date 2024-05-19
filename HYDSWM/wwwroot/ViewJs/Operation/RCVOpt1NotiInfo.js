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
            url: "/Operation/GetAllOpt1RCVNoti/",
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
                "className": 'details-control',
                "orderable": false,
                "data": null,

                "defaultContent": ''
            },
            {
                sortable: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },

            { data: "OperationType", sortable: true },
            { data: "ContainerCode", sortable: true },
            { data: "TStationName", sortable: true },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    return '<span class="gticket" cticketid="' + row.CTDId + '" TDate="' + row.CreatedDate + '">' + row.Step1UId + '</span>';

                }
            },
            { data: "CreatedOn", sortable: false },
            { data: "LastActivityOn", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.Status == 'IN PROGRESS')
                        return '<span class="badge badge-danger">' + row.Status + '</span>';
                    else
                        return '<span class="badge badge-success">' + row.Status + '</span>';

                }
            },
            { data: "TotalVehicleScanned", sortable: false },
            { data: "CreatedBy", sortable: false },
            { data: "ContactNo", sortable: false },
        ]
    });


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

    var TName = "RCV Operation";

    ShowLoading($('#dvNotification'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportAllOpt1RCVNoti",
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


function format(item) {
    var InnerGrid = '<div class="table-scrollable"><table class="table table-bordered table-striped"><thead><tr><th>Sr. No.</th><th>Image</th> <th> Operation Type </th><th> Vehicle No </th><th> Vehicle Type </th><th> Vehicle UId </th><th> Transfer Station </th><th> Is Deviated </th><th> Remarks </th><th> Scanned On </th><th> Scanned By </th><th> Status </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;
        var Ftransaction = '';
        var isdeviated = ''
        if (info.IsDeviated == false)
            isdeviated = 'INSIDE';
        else
            isdeviated = 'OUTSIDE';

        if (info.ActionStatus == 'FORCE TRANSACTION')
            Ftransaction = '<span class="badge badge-danger">' + info.ActionStatus + '</span>';
        else if (info.ActionStatus == 'REJECTED TRANSACTION')
            Ftransaction = '<span class="badge badge-warning">' + info.ActionStatus + '</span>';
        else
            Ftransaction = '<span class="badge badge-success">' + info.ActionStatus + '</span>';

        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td><a class="gticket" cticketid = "' + info.CTDId + '" href = "' + info.ImgUrl + '" data-fancybox="gallery" > <img id="imageresource" src="' + info.ImgUrl + '" alt="" class="img-preview rounded"></a></td>' +
            '<td>' + info.OperationType + '</td>' +
            '<td>' + info.VehicleNo + '</td>' +
            '<td>' + info.VehicleType + '</td>' +
            '<td>' + info.UId + '</td>' +
            '<td>' + info.TStationName + '</td>' +
            '<td>' + isdeviated + '</td>' +
            '<td>' + info.Remarks + '</td>' +
            '<td>' + info.CreatedOn + '</td>' +
            // '<td><span class="' + info.LabelClass + '">' + info.AssetStatus + '</span></td>' +

            '<td>' + info.CreatedBy + '</td>' +
            '<td>' + Ftransaction + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}