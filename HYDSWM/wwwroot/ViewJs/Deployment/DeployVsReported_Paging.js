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
    var SDate = urlParams('SDate');
    var Typeid = urlParams('Typeid');

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
            url: "/Deployment/GetVehicleDeployedVsReportedPaging/",
            type: 'POST',
            data: function (d) {
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.WardId = WardId;
                d.Status = StatusId;
                d.VehicleTypeId = VehicleTypeId;
                d.FromDate = SDate;
                d.CategoryId = Typeid;
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

            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "WardNo", sortable: true },
            { data: "VehicleType", sortable: true },
            { data: "DriverName", sortable: false },
            { data: "ContactNo", sortable: false },
            { data: "VehicleNo", sortable: true },
            { data: "InchargeName", sortable: false },
            { data: "InchargeContactNo", sortable: false },

            { data: "LastDeployedDate", sortable: true },
            { data: "ReportedDate", sortable: true },
            { data: "Trip1Time", sortable: true }
            
        ]
    });

}


function DownloadFile(FType) {


    var VehicleTypeId = urlParams('vtypeid');
    var ZoneId = urlParams('zid');
    var CircleId = urlParams('cirid');
    var WardId = urlParams('wardid');
    var StatusId = urlParams('stypeid');
    var SDate = urlParams('SDate');
    var Typeid = urlParams('Typeid');

    var TName = "Deployed Vs Reported Summary";

    ShowLoading($('#example'));

    $.ajax({
        type: "POST",
        url: "/Asset/ExportDeployedVsReportedSummary",
        data: { SDate: SDate, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleTypeId: VehicleTypeId, FName: TName, Status: StatusId, Typeid: Typeid },
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

