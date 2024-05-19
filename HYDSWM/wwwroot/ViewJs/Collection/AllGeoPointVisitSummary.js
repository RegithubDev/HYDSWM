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

    //AllMZoneLst('ddlSLACategoryType',1,'Select');
    AllMZoneLst('ddlZone', 0, 'All Zone');
    // AllMCircleLst('ddlCircle', 0, 'All Circle');
    //AllMWardLst('ddlWard', 0, 'All Ward');
    //AllMTripLst('ddlTrip', 0, 'All Trip');
    AllMActiveRouteLst('ddlRoute', 0, 'All Route');
    AllAssignedVehicleLst('ddlVehicleNumber', 0, 'All VehicleNumber')
    //$('#example').DataTable().clear().destroy();
    //loadMap();
    // GetDataTableData(Type);
    // CallAllFunc('Load');
    AllCategoryLst();
    //AllMZoneLst('ddlZone', 0, 'All Zone');
    GetDataTableData('Load');
    //CallAllFunction('Load');
    //SetValues('Load');
    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function () {
        
        var tr = $(this).closest('tr');
        var row = $('#example').DataTable().row(tr);
        var TDate = tr.find('.gTDate').attr('cTDate');
        var PointId = tr.find('.gticket').attr('cticketid');


        var FDate = document.getElementById('txtFromDate').value; //
        var TDate = document.getElementById('txtToDate').value;//

        var RouteId = $('#ddlRoute').val();
        var ZoneId = $('#ddlZone').val();
        var CircleId = $('#ddlCircle').val();
        var WardId = $('#ddlWard').val();
        var TripId = $('#ddlTrip').val();
        var VisitingType = $('#ddlVisitingType').val();
        var dataS = {
            Pointid: PointId,
            ZoneId: ZoneId,
            CircleId: CircleId,
            WardId: WardId,
            RouteId: RouteId,
            TripId: TripId,
            Visitingtype: VisitingType,
            FromDate: FDate,
            ToDate: TDate
        }

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var iccData;
            $.ajax({
                url: "/Collection/GetAllGeoPointVisitByPointId",
                type: "POST",
                dataType: "json",
                //data: dataS,
                data: { Pointid: PointId, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, RouteId: RouteId, TripId: TripId, Visitingtype: VisitingType, FromDate: FDate, ToDate: TDate },
                success: function (data) {

                    var myJSON = JSON.parse(data);
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });

});
//function CallAllRouteByZoneAndWard() {
//    $('#ddlRoute').val('0');
//    AllMActiveRouteLst('ddlRoute', 0, 'All Route', $('#ddlZone').find(":selected").attr('value'), $('#ddlCircle').find(":selected").attr('value'));
//}
//function CallAllTripByRoute() {
//    $('#ddlTrip').val('0');
//    AllMActiveRouteTrip('ddlTrip', 0, 'All Trip', $('#ddlRoute').find(":selected").attr('value'));
//}
function AllCategoryLst() {
    $("#ddlCatgory").html();
    $.ajax({
        type: "post",
        url: "/Master/GetAllGeoPointCategory",
        data: { IsAll: 'No', GPCId: 0 },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='ddlCatgory' class='form-control'>";
            Resource = Resource + '<option value="">Select</option>';
            for (var i = 0; i < Myjson.length; i++) {
                if (Myjson[i].GPCId != 5 && Myjson[i].GPCId != 6) {
                    Resource = Resource + '<option value=' + Myjson[i].GPCId + '>' + Myjson[i].GeoPointCategory + '</option>';
                }

            }
            Resource = Resource + '</select>';
            $('#ddlCatgory').html(Resource);
        }
    });
}
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}
function format(item) {
    // `d` is the original data object for the row
    //alert(JSON.stringify(d.CASO));
    var InnerGrid = '<div class="table-scrollable"><table class="table display product-overview mb-30"><thead><tr><th>Sr. No.</th> <th> Zone </th><th>Circle </th><th> Ward</th><th> Route Code </th><th> Route Trip Code </th><th>Point Name </th><th>Category </th><th>Visit Date </th><th>Visit Time </th><th>Vehicle Number </th><th>Shift Name </th><th>Shift Date </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;
        var status = ''
        // var dateS = info.PickupTime.split('T');


        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td>' + info.ZoneNo + '</td>' +
            '<td>' + info.CircleName + '</td>' +
            '<td>' + info.WardNo + '</td>' +
            '<td>' + info.RouteCode + '</td>' +
            '<td>' + info.RouteName + '</td>' +
            '<td>' + info.GeoPointName + '</td>' +
            '<td>' + info.GeoPointCategory + '</td>' +
            '<td>' + info.PDate + '</td>' +
            '<td>' + info.PTime + '</td>' +
            '<td>' + info.VehicleNo + '</td>' +
            '<td>' + info.ShiftName + '</td>' +
            '<td>' + info.ShiftDate + '</td>' +
            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}
var IsClick = 0;
var dt;

function GetDataTableData(Type) {
    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var RouteId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var TripId = '0';
    var IsReport = 0;
    var WardId = 0;
    var VisitingType = '0';
    var Status = '-1';
    var CategoryId = '0';
    var VehicleUid = '0';
    //var VisitType = '';
    if (Type == 'Click') {
        RouteId = $('#ddlRoute').val();
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        TripId = $('#ddlTrip').val();
        VisitingType = $('#ddlVisitingType').val();
        IsClick = 1;
        Status = $('#ddlStatus').val();
        CategoryId = $('#ddlCatgory').val();
        VehicleUid = $('#ddlVehicleNumber').val();
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
            url: "/Collection/GetAllGeoPointsVisitSummary/",
            type: 'POST',
            data: function (d) {

                d.FromDate = FDate;
                d.ToDate = TDate;
                d.Route = RouteId;
                d.WardId = WardId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.NotiId = TripId;
                d.IsReport = IsReport;
                d.VisitingType = VisitingType;
                d.Status = Status;
                d.CategoryId = CategoryId;
                d.VehicleUid = VehicleUid
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

            //{
            //    data: "GeoPointId", "render": function (data, type, row, meta) {

            //        return '<span class="gticket" cticketid="' + row.GeoPointId + '" TDate="' + row.Dated + '">' + row.readerid + '</span>';
            //    }
            //},

            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "WardNo", sortable: true },
            {
                data: "GeoPointId", "render": function (data, type, row, meta) {

                    return '<span class="gticket" cticketid="' + row.GeoPointId + '" >' + row.GeoPointName + '</span>';
                }
            },
            { data: "GeoPointCategory", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.GeoPointStatus == 1)
                        return '<span>ACTIVE</span>';
                    else
                        return '<span>DE-ACTIVE</span>';

                }
            },

            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.TotalVisit == 0)
                        return '<span class="badge badge-danger">' + row.TotalVisit + '</span>';
                    else if (row.TotalVisit == 1)
                        return '<span class="badge badge-success">' + row.TotalVisit + '</span>';
                    else if (row.TotalVisit == 2)
                        return '<span class="badge badge-warning" style="background-color:#ffd800">' + row.TotalVisit + '</span>';
                    else if (row.TotalVisit == 3)
                        return '<span class="badge badge-primary">' + row.TotalVisit + '</span>';
                    else if (row.TotalVisit == 4)
                        return '<span class="badge badge-warning">' + row.TotalVisit + '</span>';
                    else
                        return '<span class="badge badge-primary" style="background-color:#6352ca">' + row.TotalVisit + '</span>';

                }
            },

            { data: "PDate", sortable: false },
            { data: "GeoPointLifecycle", sortable: false },
            //{ data: "GeoCordinate", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    // Assuming you have a property called "Link" in your data
                    var link = 'http://maps.google.co.in/maps?q=' + row.GeoCordinate;

                    // Create the link HTML with target="_blank"
                    var linkHTML = '<a href="' + link + '" target="_blank">' + row.GeoCordinate+'</a>';

                    // Return the HTML for the column
                    return linkHTML;
                }
            },


        ]
    });


}
function ShowTextOnHover(objthis) {
    var SPickupTime = $(objthis).attr('data-SPickupTime');
    var SchedulePickupTime = $(objthis).attr('data-SchedulePickupTime');
    var TripName = $(objthis).attr('data-TripName');
    var Msg = 'Schedule Pickup Time-' + SchedulePickupTime + '\n' + ' Actual Pickup Time-' + SPickupTime + '\n' + ' Route Trip Code-' + TripName;
    ShowCustomMessage('0', Msg, '');
}

function DownloadFile(FType) {


    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var RouteId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var TripId = '0';
    var IsReport = 0;
    var WardId = 0;
    var VisitingType = '0';
    var Status = '-1';
    var CategoryId = '0';
    //var VisitType = '';
    if (FType == 'Excel') {
        RouteId = $('#ddlRoute').val();
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        TripId = $('#ddlTrip').val();
        VisitingType = $('#ddlVisitingType').val();
        IsClick = 1;
        Status = $('#ddlStatus').val();
        CategoryId = $('#ddlCatgory').val();;
    }



    var TName = "GeoPoint Visit Collection Report";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Collection/ExportAllGeoPointsVisitSummary",
        data: { FromDate: FDate, ToDate: TDate, Route: RouteId, WardId: WardId, ZoneId: ZoneId, CircleId: CircleId, NotiId: TripId, IsReport: IsReport, VisitingType: VisitingType, Status: Status, CategoryId: CategoryId, FName: TName },
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
function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'Select', $('#ddlCircle').find(":selected").attr('value'));
}
function CallAllTripByRoute() {
    $('#ddlTrip').val('0');
    $('#ddlVehicleNumber').val('0');
    AllMActiveRouteTrip('ddlTrip', 0, 'All Trip', $('#ddlRoute').find(":selected").attr('value'));
    AllAssignedVehicleLst1('ddlVehicleNumber', 0, 'All VehicleNumber', $('#ddlRoute').find(":selected").attr('value'), $('#ddlTrip').find(":selected").attr('value'));

}

function CallAllTripId() {
    $('#ddlVehicleNumber').val('0');
    AllAssignedVehicleLst1('ddlVehicleNumber', 0, 'All VehicleNumber', $('#ddlRoute').find(":selected").attr('value'), $('#ddlTrip').find(":selected").attr('value'));

}

function CSVData(FType) {
    
    // Assuming you have jQuery loaded
    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var RouteId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var TripId = '0';
    var IsReport = 0;
    var WardId = 0;
    var VisitingType = '0';
    var Status = '-1';
    var CategoryId = '0';
    //var VisitType = '';
    if (FType == 'Excel') {
        RouteId = $('#ddlRoute').val();
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        TripId = $('#ddlTrip').val();
        VisitingType = $('#ddlVisitingType').val();
        IsClick = 1;
        Status = $('#ddlStatus').val();
        CategoryId = $('#ddlCatgory').val();;
    }



    var TName = "GeoPoint Visit Collection Report";


    $.ajax({
        url: '/Collection/ExportAllGeoPointsVisitCSV', // URL of your controller action
        method: 'POST',
        data: { FromDate: FDate, ToDate: TDate, Route: RouteId, WardId: WardId, ZoneId: ZoneId, CircleId: CircleId, NotiId: TripId, IsReport: IsReport, VisitingType: VisitingType, Status: Status, CategoryId: CategoryId, FName: TName },
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