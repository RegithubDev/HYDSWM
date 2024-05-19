$(document).ready(function () {
    GetDataTableData('Load');

});

var dt;
function GetDataTableData(Type) {
    var ZoneId = getUrlParameterInfo('ZId');
    var CircleId = getUrlParameterInfo('CId');
    var WardId = getUrlParameterInfo('WId');
    var TName = getUrlParameterInfo('TName');
    var TId = getUrlParameterInfo('TId');
   
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
            url: "/Collection/AllEmergencyPointCltNoti_Paging/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {
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
                sortable: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { data: "PointName", sortable: true },
            { data: "GeoPointCategory", sortable: true },
            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "WardNo", sortable: true }
            //{
            //    sortable: false,
            //    "render": function (data, type, row, meta) {

            //        if (row.TotalVisit > 0)
            //            return '<span class="badge badge-success">' + row.TotalVisit + '</span>';
            //        else
            //            return '<span class="badge badge-danger">' + row.TotalVisit + '</span>';

            //    }
            //}
        ]
    });
}


