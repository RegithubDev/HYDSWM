$(document).ready(function () {
    GetDataTableData('Load');
    $('#example tbody').on('click', 'td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#example').DataTable().row(tr);

        var TId = getUrlParameterInfo('TId');
        // var RouteId = tr.attr("data-RouteId");//tr.find('td:eq(3)').text();
        var RouteId = tr.find('.gticket').attr('cticketid');
        var TripId = tr.find('.gticket1').attr('cticketid1');
        var FDate = getUrlParameterInfo('FDate');
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var dataSS = {
                'RouteId': RouteId,
                'TypeId': TId,
                'TripId': TripId

            }
            var iccData;
            $.ajax({
                url: "/Collection/GetAllEarlyCompletionByRouteId",
                type: "POST",
                dataType: "json",
                data: { RouteId: RouteId, TypeId: TId, TripId: TripId, SDate: FDate },
                success: function (data) {

                    var myJSONData = JSON.parse(data);
                    // var myJSON = myJSONData.Table[0];
                    var myJSON = myJSONData.Table;
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });
});

var dt;
function GetDataTableData(Type) {

    var ZoneId = getUrlParameterInfo('ZId');
    var CircleId = getUrlParameterInfo('CId');
    var WardId = getUrlParameterInfo('WId');
    var TName = getUrlParameterInfo('TName');
    var TId = getUrlParameterInfo('TId');
    var IsActive = getUrlParameterInfo('IsActive');
    var RouteId = getUrlParameterInfo('RouteId');
    var FDate = getUrlParameterInfo('FDate');
    $("#spnHeader").html('');
    $("#spnHeader").html(TName);

    dt = $('#example').DataTable({
        processing: true,
        destroy: true,
        responsive: true,
        serverSide: true,
        searchable: true,
        lengthMenu: [[10, 20, 50, 100, 500, 10000, -1], [10, 20, 50, 100, 500, 10000, "All"]],
        language: {
            infoEmpty: "No records available",
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
            url: "/Collection/AllPointEarlyCompletion_Paging/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {

                d.Status = RouteId;
                d.ContratorId = TId;
                d.WardId = WardId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.FromDate = FDate;
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
            { data: "RouteName", sortable: true },//
            {
                data: "RouteCode", "render": function (data, type, row, meta) {

                    return '<span class="gticket" cticketid="' + row.RouteId + '" >' + row.RouteCode + '</span>';
                }
            },
            {
                data: "TId", "render": function (data, type, row, meta) {

                    return '<span class="gticket1" cticketid1="' + row.TripId + '" >' + row.TId + '</span>';
                }
            },
            //{ data: "TId", sortable: true },
            // { data: "RouteName", sortable: true },
            /*{ data: "RouteCode", sortable: true },*/
            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "CollectedPoint", sortable: false },
            { data: "TotalStop", sortable: true }

        ]
    });
}
function format(item) {

    // `d` is the original data object for the row
    //alert(JSON.stringify(d.CASO));
    var InnerGrid = '<div class="table-scrollable"><table class="table display product-overview mb-30"><thead><tr><th>Sr. No.</th> <th> Route Trip Code </th><th> zone No </th><th> Circle Name </th><th> Pickup Time </th><th> Shift Name </th><th> Shift Date </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;
        var Vehicleno = ''


        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            /* '<td>' + info.TripName + '</td>' +*/
            '<td>' + info.TId + '</td>' +
            '<td>' + info.ZoneNo + '</td>' +
            '<td>' + info.CircleName + '</td>' +
            '<td>' + info.PickupTime + '</td>' +
            '<td>' + info.ShiftName + '</td>' +
            '<td>' + info.ShiftDate + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}
function RedirectToPage(obj) {
    var ddId = $(obj).attr('cid');
    window.location = "/Collection/AddSRoute?cid=" + ddId;
}