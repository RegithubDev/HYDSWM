$(document).ready(function () {

    GetDataTableData('Load');
    
});


var dt;
function GetDataTableData(Type) {
    var TId = '0';
    var TName = '0';
    var ZId = '0';
    var CId = '0';
    var WId = '0';
    var VehicleTypeId = '0';
    var CollectionTypeId = "0";
    

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
            url: "/Demo/GetAllVehicleInfo/",
            type: 'POST',
            data: function (d) {
                d.Status = TId;
                d.ZoneId = ZId;
                d.CircleId = CId;
                d.WardId = WId;
                d.NotiId = CollectionTypeId;
                d.VehicleTypeId = VehicleTypeId;
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
            { data: "VehicleNo", sortable: true },
            { data: "ChassisNo", sortable: true },
            { data: "VehicleType", sortable: true },
            { data: "OwnerType", sortable: true },
            { data: "OperationType", sortable: true }
        ]
    });

    //if (TId != "")
    //    dt.column(8).visible(false);
}



