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
    //$('#example').DataTable().clear().destroy();
    //loadMap();
    GetDataTableData('Load');
    // CallAllFunc('Load');
    AllCategoryLst();
    //AllMZoneLst('ddlZone', 0, 'All Zone');
    // GetDataTableData('Load');
    //CallAllFunction('Load');
    //SetValues('Load');
    // Add event listener for opening and closing details


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

var IsClick = 0;
var dt;

function GetDataTableData(Type) {

    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var PointId = '-1';
    var ZoneId = '0';
    var CircleId = '0';
    var Status = '-1';
    var CategoryId = '0';
    var WardId = 0;
    //var VisitType = '';
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        IsClick = 1;
        Status = $('#ddlStatus').val();
        CategoryId = $('#ddlCatgory').val();
        PointId = $('#ddlPointToRoute').val();;
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
            url: "/Collection/AllGeoPointNotCollected_Paging/",
            type: 'POST',
            data: function (d) {

                d.FromDate = FDate;
                d.ToDate = TDate;
                d.WardId = WardId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.NotiId = PointId;
                d.Status = Status;
                d.CategoryId = CategoryId;
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



            { data: "PDate", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    // Assuming you have a property called "Link" in your data
                    var link = 'http://maps.google.co.in/maps?q=' + row.GeoCordinate;

                    // Create the link HTML with target="_blank"
                    var linkHTML = '<a href="' + link + '" target="_blank">' + row.GeoCordinate + '</a>';

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



function CSVData(FType) {
    
    // Assuming you have jQuery loaded
    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var PointId = '-1';
    var ZoneId = '0';
    var CircleId = '0';
    var Status = '-1';
    var CategoryId = '0';
    var WardId = 0;
    //var VisitType = '';
    if (IsClick == 1) {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        IsClick = 1;
        Status = $('#ddlStatus').val();
        CategoryId = $('#ddlCatgory').val();
        PointId = $('#ddlPointToRoute').val();;
    }



    var TName = "GeoPoint Not Collected Report";


    $.ajax({
        url: '/Collection/ExportAllGeoPointNotCollected_PagingCSV', // URL of your controller action
        method: 'POST',
        data: { FromDate: FDate, ToDate: TDate, WardId: WardId, ZoneId: ZoneId, CircleId: CircleId, NotiId: PointId, Status: Status, CategoryId: CategoryId, FName: TName },
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




function CallAllFunction(Type) {
    GetDataTableData(Type);
    AllPointTimelineInfo(Type);
}
function DownloadFile(FType) {


    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var PointId = '-1';
    var ZoneId = '0';
    var CircleId = '0';
    var Status = '-1';
    var CategoryId = '0';
    var WardId = 0;
    //var VisitType = '';
    if (IsClick == 1) {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        IsClick = 1;
        Status = $('#ddlStatus').val();
        CategoryId = $('#ddlCatgory').val();
        PointId = $('#ddlPointToRoute').val();;
    }



    var TName = "GeoPoint Not Collected Report";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Collection/ExportAllGeoPointNotCollected_Paging",
        data: { FromDate: FDate, ToDate: TDate, WardId: WardId, ZoneId: ZoneId, CircleId: CircleId, NotiId: PointId, Status: Status, CategoryId: CategoryId, FName: TName },
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
    AllMActiveRouteTrip('ddlTrip', 0, 'All Trip', $('#ddlRoute').find(":selected").attr('value'));
}