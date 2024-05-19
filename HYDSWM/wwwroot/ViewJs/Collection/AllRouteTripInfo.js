$(document).ready(function () {
    AllMZoneLst('ddlZone', 0, 'All Zone');
    GetDataTableData('Load');

    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function () {
        
        var tr = $(this).closest('tr');
        var row = $('#example').DataTable().row(tr);

        // var RouteId = tr.attr("data-RouteId");//tr.find('td:eq(3)').text();
        var RouteId = tr.find('.gticket').attr('cticketid');
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var iccData;
            $.ajax({
                url: "/Collection/GetAllRouteTripByRoute",
                type: "POST",
                dataType: "json",
                data: { RouteId: RouteId },
                success: function (data) {
                    var myJSONData = JSON.parse(data);
                    // var myJSON = myJSONData.Table[0];
                    var myJSON = myJSONData.Table1;
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });
});

var dt;
function GetDataTableData(Type) {
    var zid = '0';
    var cid = '0';

    if (Type = 'Click') {
        zid = $('#ddlZone').val();
        cid = $('#ddlCircle').val();
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
            url: "/Collection/AllSRouteInfo/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {
                d.NotiId = '1';
                d.ZoneId = zid;
                d.CircleId = cid;
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
            {
                sortable: true,
                "render": function (data, type, row, meta) {


                    return '<span class="gticket" cticketid="' + row.RouteId + '">' + row.RouteCode + '</span>';

                }
            },
            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "TotalStop", sortable: true },
            { data: "TotalTrips", sortable: true },

            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.IsActive == 1)
                        return '<span class="badge badge-success" data-RouteId=' + row.RouteId + '>ACTIVE</span>';
                    else
                        return '<span class="badge badge-danger" data-RouteId=' + row.RouteId + '>DE-ACTIVE</span>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return "<a cid='" + row.RouteId + "' href='javascript:void(0);' title='edit' onclick='RedirectToPage(this);'><i class='icon-add'></i></a>";

                }
            }
        ]
    });
}
function RedirectToPage(obj) {
    var ddId = $(obj).attr('cid');
    window.location = "/Collection/AddRouteTrip?cid=" + ddId;
}
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}
function format(item) {
    // `d` is the original data object for the row
    //alert(JSON.stringify(d.CASO));
    var InnerGrid = '<div class="table-scrollable"><table class="table display product-overview mb-30"><thead><tr><th>Sr. No.</th> <th> Route Trip Code </th><th> Vehicle No </th><th> Buffer Min </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;
        var Vehicleno = ''

        if (info.VehicleNo == '')
            Vehicleno = '<span class="badge badge-danger">No Vehicle Assigned</span>';
        else
            Vehicleno = '<span class="badge badge-success">' + info.VehicleNo + '</span>';

        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            /* '<td>' + info.TripName + '</td>' +*/
            '<td>' + info.TId + '</td>' +
            '<td>' + Vehicleno + '</td>' +
            '<td>' + info.BufferMin + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}

