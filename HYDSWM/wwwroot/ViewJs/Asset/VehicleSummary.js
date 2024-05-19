$(document).ready(function () {




    GetDataTableData('Load')

});


function urlParams(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};

var dt;
function GetDataTableData() {
    
    var VehicleTypeId = urlParams('vtypeid');
    var ZoneId = urlParams('zid');
    var CircleId = urlParams('cirid');
    var WardId = urlParams('wardid');
    var StatusId = urlParams('stypeid');

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
            url: "/Asset/GetAllVehicleInfo/",
            type: 'POST',
            data: function (d) {
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.WardId = WardId;
                d.Status = StatusId;
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
        'createdRow': function (row, data, dataIndex) {
            $(row).attr('UId', data.UId);
            $(row).attr('OperationTypeId', data.OperationTypeId);
        },
        columns: [

            {
                sortable: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },


            {
                sortable: true,
                "render": function (data, type, row, meta) {
                    return '<span class="' + row.LabelClass + '">' + row.AssetStatus + '</span>';
                }
            },
           
            { data: "UId", sortable: true },

            { data: "VehicleNo", sortable: true },
            { data: "ChassisNo", sortable: true },
            { data: "VehicleType", sortable: true },
            { data: "OwnerType", sortable: true },
            { data: "OperationType", sortable: true },
            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "WardNo", sortable: true },
            { data: "GrossWt", sortable: true },
            { data: "TareWt", sortable: true },
            { data: "NetWt", sortable: true },

            //{
            //    sortable: false,
            //    "render": function (data, type, row, meta) {

            //        return '<a class="gticket" cticketid="' + row.VehicleId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
            //    }
            //},
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.VehicleId + '" href="' + row.ImgUrl + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.ImgUrl + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "DriverName", sortable: false },
            { data: "ContactNo", sortable: false },
            { data: "InchargeName", sortable: false },
            { data: "InchargeContactNo", sortable: false },
            { data: "LandMark", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    var link = 'http://maps.google.co.in/maps?q=' + row.DLCordinates;
                    var linkHTML = '<a href="' + link + '" target="_blank">' + row.DLCordinates + '</a>';
                    return linkHTML;
                }
            },
            { data: "DLRadius", sortable: false },
            { data: "DTStationName", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    var link = 'http://maps.google.co.in/maps?q=' + row.DTCordinates;
                    var linkHTML = '<a href="' + link + '" target="_blank">' + row.DTCordinates + '</a>';
                    return linkHTML;
                }
            },
            { data: "DTRadius", sortable: false }

        ]
    });

}










function DownloadFile(FType) {



    var VehicleTypeId = urlParams('vtypeid');
    var ZoneId = urlParams('zid');
    var CircleId = urlParams('cirid');
    var WardId = urlParams('wardid');
    var Status = urlParams('stypeid');


    var TName = "Vehicle Summary Details";

    ShowLoading($('#example'));

    $.ajax({
        type: "POST",
        url: "/Asset/ExportAllVehicleSummaryDetails",
        data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleTypeId: VehicleTypeId, FName: TName, Status: Status, },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#example'));
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
            HideLoading($('#example'));
        }
    });
}

