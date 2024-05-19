$(document).ready(function () {

    GetDataTableData('Load');
    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#example').DataTable().row(tr);
        //var routecode = tr.find('td:eq(5)').text();
        var ticketid = tr.find('.gticket').attr('cticketid');
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var iccData;
            $.ajax({
                url: "/Asset/GetTStationInfoById",
                type: "POST",
                dataType: "json",
                data: { CMId: ticketid },
                success: function (data) {
                    var myJSON = JSON.parse(data);
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });
});

var dt;
function GetDataTableData(Type) {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var CollectionTypeId = "0";
    CollectionTypeId = getUrlParameterInfo('CollectionTypeId');
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
            url: "/Asset/GetAllContainerInfo/",
            type: 'POST',
            data: function (d) {
                d.Status = TId;
                d.NotiId = CollectionTypeId;
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
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.CMId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },
            {
                sortable: true,
                "render": function (data, type, row, meta) {
                    return '<span class="' + row.LabelClass + '">' + row.AssetStatus + '</span>';
                }
            },
            
            { data: "UId", sortable: true },
            { data: "ContainerCode", sortable: true },
            { data: "ContainerName", sortable: true },
            { data: "Capacity", sortable: true },
            { data: "ContainerType", sortable: true }
            
        ]
    });

    //if (TId != "")
    //    dt.column(8).visible(false);
}
function format(item) {
    var InnerGrid = '<div class="table-scrollable"><table class="table table-bordered table-striped"><thead><tr><th>Sr. No.</th> <th> Status </th><th> Remarks </th><th> Modified On </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;

        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td><span class="' + info.LabelClass + '">' + info.AssetStatus + '</span></td>' +
            '<td>' + info.Remarks + '</td>' +
            '<td>' + info.SCreatedOn + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}

function DownloadQrCode(objthis) {

    var UHouseId = $(objthis).attr('data-QUID');
    window.location = "/Asset/DownLoadQR?UId=" + UHouseId;
}