
$(document).ready(function () {



    var date = new Date();
    document.getElementById("txtFromDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    var currentYear = (new Date).getFullYear();
    var currentMonth = (new Date).getMonth() + 1;
    var currentDay = (new Date).getDate();
    $('#txtFromDate').datepicker({
        changeMonth: true,
        changeYear: true,
        format: 'm/d/yyyy',
        //endDate: '+0d',
        autoclose: true,
        endDate: document.getElementById("txtFromDate").value,
        //minDate: new Date((currentYear - 1), 12, 1),
        //dateFormat: 'dd/mm/yyyy',
        // maxDate: document.getElementById("txtFromDate").value

    });
    $("#rdbGVP").prop("checked", true);
    //AllMZoneLst('ddlSLACategoryType',1,'Select');
    AllMZoneLst('ddlZone', 0, 'All Zone');
    //AllMCircleLst('ddlCircle', 0, 'All Circle');
    // AllMWardLst('ddlWard', 0, 'All Ward');
    //AllMTripLst('ddlTrip', 0, 'All Trip');
    AllMActiveRouteLst('ddlRoute', 0, 'All Route');
    $('#example').DataTable().clear().destroy();
    // refreshNotification();
    CallAllFunc('Load');
    CallAllClickFunc();
    // loadMap();
    //$("#txtRadius").on("propertychange change keyup paste input", function () {
    //    drawCircle();
    //});

});

function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}

function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'Select', $('#ddlCircle').find(":selected").attr('value'));
}
function CallAllTripByRoute() {
    $('#ddlTrip').val('0');
    AllMActiveRouteTrip('ddlTrip', 0, 'All Trip', $('#ddlRoute').find(":selected").attr('value'));
}

function CallAllFunc(Type) {
    
    SetRoutesOperationData(Type);
    SetMasterAndOperationData(Type);
    SetEmerGencyPointData(Type);
    GetGeopointDataForMap(Type, '-2');
}

function SetRoutesOperationData(Type) {
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    var TripId = '0';
    var RouteId = '0';
    var FDate = document.getElementById('txtFromDate').value;//param
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        WardId = $('#ddlWard').val();
        TripId = $('#ddlTrip').val();
        RouteId = $('#ddlRoute').val();
        FDate = document.getElementById('txtFromDate').value;//param
    }

    $.ajax({
        type: "POST",
        url: '/Collection/GetAllZoneWiseCltnNoti',
        data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, RouteId: RouteId, TripId: TripId, fromDate: FDate },
        success: function (data) {

            var myJSONData = JSON.parse(data);
            var Mdata = myJSONData.Table[0];
            var Odata = myJSONData.Table1[0];


            $("#bZ1TotalPoint").html(Mdata.Z1TotalPoint);
            $("#bZ1CollectedPoint").html(Mdata.Z1CollectedPoint);
            $("#bZ1CollectedPointPerc").html("[" + Mdata.Z1CollectedPerc + "%" + "]");
            //$("#bZ1NotCollectedPoint").html(Mdata.Z1TotalGreyPoint + " [" + Mdata.Z1NotCollectedPoint + "]");
            $("#bZ1NotCollectedPoint").html(Mdata.Z1NotCollectedPoint);
            $("#bZp1NotCollectedPoint").html("[G- " + Mdata.Z1TotalGreyPoint + "]");
            $("#bZ1UniquePoint").html(Mdata.Z1UniquePoint);
            $("#bZ1MoreThanOncePoint").html(Mdata.Z1MoreThanOncePoint);
            $("#bZ1HotspotProgress").html(Mdata.Z1HotSpotCollectedPoint + " [" + Mdata.Z1HotSpotCollectedPerc + "%" + "]");//HotSpot Progress
            $("#bZ1Vehicle").html(Mdata.Z1TotalVehicleAssigned + " [" + Mdata.Z1TotalVehicleAssignedPerc + "%" + "]");//VehicleDeplyed Progress
            $("#bZ2Vehicle").html(Mdata.Z2TotalVehicleAssigned + " [" + Mdata.Z2TotalVehicleAssignedPerc + "%" + "]");//VehicleDeplyed Progress
            $("#bZ3Vehicle").html(Mdata.Z3TotalVehicleAssigned + " [" + Mdata.Z3TotalVehicleAssignedPerc + "%" + "]");//VehicleDeplyed Progress
            $("#bZ4Vehicle").html(Mdata.Z4TotalVehicleAssigned + " [" + Mdata.Z4TotalVehicleAssignedPerc + "%" + "]");//VehicleDeplyed Progress
            $("#bZ5Vehicle").html(Mdata.Z5TotalVehicleAssigned + " [" + Mdata.Z5TotalVehicleAssignedPerc + "%" + "]");//VehicleDeplyed Progress
            $("#bZ6Vehicle").html(Mdata.Z6TotalVehicleAssigned + " [" + Mdata.Z6TotalVehicleAssignedPerc + "%" + "]");//VehicleDeplyed Progress
            $("#bZAllVehicle").html(Mdata.ZAllTotalVehicleAssigned + " [" + Mdata.ZAllTotalVehicleAssignedPerc + "%" + "]");//VehicleDeplyed Progress


            $("#bZ2TotalPoint").html(Mdata.Z2TotalPoint);
            $("#bZ2CollectedPoint").html(Mdata.Z2CollectedPoint);
            $("#bZ2CollectedPointPerc").html("[" + Mdata.Z2CollectedPerc + "%" + "]");
            //$("#bZ2NotCollectedPoint").html(Mdata.Z2TotalGreyPoint + " [" + Mdata.Z2NotCollectedPoint + "]");
            $("#bZ2NotCollectedPoint").html(Mdata.Z2NotCollectedPoint);
            $("#bZp2NotCollectedPoint").html("[G- " + Mdata.Z2TotalGreyPoint + "]");
            $("#bZ2UniquePoint").html(Mdata.Z2UniquePoint);
            $("#bZ2MoreThanOncePoint").html(Mdata.Z2MoreThanOncePoint);
            $("#bZ2HotspotProgress").html(Mdata.Z2HotSpotCollectedPoint + " [" + Mdata.Z2HotSpotCollectedPerc + "%" + "]");//HotSpot Progress

            $("#bZ3TotalPoint").html(Mdata.Z3TotalPoint);
            $("#bZ3CollectedPoint").html(Mdata.Z3CollectedPoint);
            $("#bZ3CollectedPointPerc").html("[" + Mdata.Z3CollectedPerc + "%" + "]");
            // $("#bZ3NotCollectedPoint").html(Mdata.Z3TotalGreyPoint + " [" + Mdata.Z3NotCollectedPoint + "]");
            $("#bZ3NotCollectedPoint").html(Mdata.Z3NotCollectedPoint);
            $("#bZp3NotCollectedPoint").html("[G- " + Mdata.Z3TotalGreyPoint + "]");
            $("#bZ3UniquePoint").html(Mdata.Z3UniquePoint);
            $("#bZ3MoreThanOncePoint").html(Mdata.Z3MoreThanOncePoint);
            $("#bZ3HotspotProgress").html(Mdata.Z3HotSpotCollectedPoint + " [" + Mdata.Z3HotSpotCollectedPerc + "%" + "]");//HotSpot Progress

            $("#bZ4TotalPoint").html(Mdata.Z4TotalPoint);
            $("#bZ4CollectedPoint").html(Mdata.Z4CollectedPoint);
            $("#bZ4CollectedPointPerc").html("[" + Mdata.Z4CollectedPerc + "%" + "]");
            //$("#bZ4NotCollectedPoint").html(Mdata.Z4TotalGreyPoint + " [" + Mdata.Z4NotCollectedPoint + "]");
            $("#bZ4NotCollectedPoint").html(Mdata.Z4NotCollectedPoint);
            $("#bZp4NotCollectedPoint").html("[G- " + Mdata.Z4TotalGreyPoint + "]");
            $("#bZ4UniquePoint").html(Mdata.Z4UniquePoint);
            $("#bZ4MoreThanOncePoint").html(Mdata.Z4MoreThanOncePoint);
            $("#bZ4HotspotProgress").html(Mdata.Z4HotSpotCollectedPoint + " [" + Mdata.Z4HotSpotCollectedPerc + "%" + "]");//HotSpot Progress


            $("#bZ5TotalPoint").html(Mdata.Z5TotalPoint);
            $("#bZ5CollectedPoint").html(Mdata.Z5CollectedPoint);
            $("#bZ5CollectedPointPerc").html("[" + Mdata.Z5CollectedPerc + "%" + "]");
            //$("#bZ5NotCollectedPoint").html(Mdata.Z5TotalGreyPoint + " [" + Mdata.Z5NotCollectedPoint + "]");
            $("#bZ5NotCollectedPoint").html(Mdata.Z5NotCollectedPoint);
            $("#bZp5NotCollectedPoint").html("[G- " + Mdata.Z5TotalGreyPoint + "]");
            $("#bZ5UniquePoint").html(Mdata.Z5UniquePoint);
            $("#bZ5MoreThanOncePoint").html(Mdata.Z5MoreThanOncePoint);
            $("#bZ5HotspotProgress").html(Mdata.Z5HotSpotCollectedPoint + " [" + Mdata.Z5HotSpotCollectedPerc + "%" + "]");//HotSpot Progress

            $("#bZ6TotalPoint").html(Mdata.Z6TotalPoint);
            $("#bZ6CollectedPoint").html(Mdata.Z6CollectedPoint);
            $("#bZ6CollectedPointPerc").html("[" + Mdata.Z6CollectedPerc + "%" + "]");
            //  $("#bZ6NotCollectedPoint").html(Mdata.Z5TotalGreyPoint + " [" + Mdata.Z6NotCollectedPoint + "]");
            $("#bZ6NotCollectedPoint").html(Mdata.Z6NotCollectedPoint);
            $("#bZp6NotCollectedPoint").html("[G- " + Mdata.Z6TotalGreyPoint + "]");
            $("#bZ6UniquePoint").html(Mdata.Z6UniquePoint);
            $("#bZ6MoreThanOncePoint").html(Mdata.Z6MoreThanOncePoint);
            $("#bZ6HotspotProgress").html(Mdata.Z6HotSpotCollectedPoint + " [" + Mdata.Z6HotSpotCollectedPerc + "%" + "]");//HotSpot Progress

            $("#bZAllTotalPoint").html(Mdata.ZAllTotalPoint);
            $("#bZAllCollectedPoint").html(Mdata.ZAllCollectedPoint);
            $("#bZAllCollectedPointPerc").html("[" + Mdata.ZAllCollectedPerc + "%" + "]");
            //  $("#bZ6NotCollectedPoint").html(Mdata.Z5TotalGreyPoint + " [" + Mdata.Z6NotCollectedPoint + "]");
            $("#bZAllNotCollectedPoint").html(Mdata.ZAllNotCollectedPoint + " [G- " + Mdata.ZAllTotalGreyPoint + "]");
            $("#bZAllUniquePoint").html(Mdata.ZAllUniquePoint);
            $("#bZAllMoreThanOncePoint").html(Mdata.ZAllMoreThanOncePoint);
            $("#bZAllHotspotProgress").html(Mdata.ZAllHotSpotCollectedPoint + " [" + Mdata.ZAllHotSpotCollectedPerc + "%" + "]");//HotSpot Progress



            //Route Section
            $("#bZAllTotalRoutes").html(Odata.ZAllTotalRoute);
            $("#bZAllTotalTrips").html(Odata.ZAllTotalTrip);
            // $("#bZAllRouteProgress").html("-");
            $("#bZAllRouteProgress").html(Odata.ZAllRoutePerc + "%");
            $("#bZAllRouteEarlyCompl").html(Odata.ZAllRouteEarlyComp);
            $("#bZAllRouteLateCompl").html(Odata.ZAllRouteLatComp);
            $("#bZAllRouteNotCompl").html(Odata.ZAllRouteNotComp);


            //Route Section
            $("#bZ1TotalRoutes").html(Odata.Z1TotalRoute);
            $("#bZ1TotalTrips").html(Odata.Z1TotalTrip);
            // $("#bZ1RouteProgress").html("-");
            $("#bZ1RouteProgress").html(Odata.Z1RoutePerc + "%");
            $("#bZ1RouteEarlyCompl").html(Odata.Z1RouteEarlyComp);
            $("#bZ1RouteLateCompl").html(Odata.Z1RouteLatComp);
            $("#bZ1RouteNotCompl").html(Odata.Z1RouteNotComp);

            $("#bZ2TotalRoutes").html(Odata.Z2TotalRoute);
            $("#bZ2TotalTrips").html(Odata.Z2TotalTrip);
            $("#bZ2RouteProgress").html(Odata.Z2RoutePerc + "%");
            //$("#bZ2RouteProgress").html("-");
            $("#bZ2RouteEarlyCompl").html(Odata.Z2RouteEarlyComp);
            $("#bZ2RouteLateCompl").html(Odata.Z2RouteLatComp);
            $("#bZ2RouteNotCompl").html(Odata.Z2RouteNotComp);

            $("#bZ3TotalRoutes").html(Odata.Z3TotalRoute);
            $("#bZ3TotalTrips").html(Odata.Z3TotalTrip);
            $("#bZ3RouteProgress").html(Odata.Z3RoutePerc + "%");
            // $("#bZ3RouteProgress").html("-");
            $("#bZ3RouteEarlyCompl").html(Odata.Z3RouteEarlyComp);
            $("#bZ3RouteLateCompl").html(Odata.Z3RouteLatComp);
            $("#bZ3RouteNotCompl").html(Odata.Z3RouteNotComp);

            $("#bZ4TotalRoutes").html(Odata.Z4TotalRoute);
            $("#bZ4TotalTrips").html(Odata.Z4TotalTrip);
            // $("#bZ4RouteProgress").html("-");
            $("#bZ4RouteProgress").html(Odata.Z4RoutePerc + "%");
            $("#bZ4RouteEarlyCompl").html(Odata.Z4RouteEarlyComp);
            $("#bZ4RouteLateCompl").html(Odata.Z4RouteLatComp);
            $("#bZ4RouteNotCompl").html(Odata.Z4RouteNotComp);

            $("#bZ5TotalRoutes").html(Odata.Z5TotalRoute);
            $("#bZ5TotalTrips").html(Odata.Z5TotalTrip);
            //$("#bZ5RouteProgress").html("-");
            $("#bZ5RouteProgress").html(Odata.Z5RoutePerc + "%");
            $("#bZ5RouteEarlyCompl").html(Odata.Z5RouteEarlyComp);
            $("#bZ5RouteLateCompl").html(Odata.Z5RouteLatComp);
            $("#bZ5RouteNotCompl").html(Odata.Z5RouteNotComp);

            $("#bZ6TotalRoutes").html(Odata.Z6TotalRoute);
            $("#bZ6TotalTrips").html(Odata.Z6TotalTrip);
            // $("#bZ6RouteProgress").html("-");
            $("#bZ6RouteProgress").html(Odata.Z6RoutePerc + "%");
            $("#bZ6RouteEarlyCompl").html(Odata.Z6RouteEarlyComp);
            $("#bZ6RouteLateCompl").html(Odata.Z6RouteLatComp);
            $("#bZ6RouteNotCompl").html(Odata.Z6RouteNotComp);

            //Route Section for OnTime completion
            $("#bZAllRouteOnTimeCompl").html(Odata.ZAllRouteOnTimeComp);
            $("#bZ1RouteOnTimeCompl").html(Odata.Z1RouteOnTimeComp);
            $("#bZ2RouteOnTimeCompl").html(Odata.Z2RouteOnTimeComp);
            $("#bZ3RouteOnTimeCompl").html(Odata.Z3RouteOnTimeComp);
            $("#bZ4RouteOnTimeCompl").html(Odata.Z4RouteOnTimeComp);
            $("#bZ5RouteOnTimeCompl").html(Odata.Z5RouteOnTimeComp);
            $("#bZ6RouteOnTimeCompl").html(Odata.Z6RouteOnTimeComp);

        },
        error: function (result) {
        }
    });

}


function CallAllClickFunc() { //KHSB
    $('#dvMTotal').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date 
        var IsActive = "-1";
        var TName = "All Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvMActive').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "1";
        var TName = "All Active Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvMInActive').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "0";
        var TName = "All In-Active Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvMAssignedToRoute').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Assigned To A Route Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvMNotAssignedToRoute').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "0";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Not Assigned To A Route Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });


    $('#dvOTotal').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&FDate=' + FDate);
    });

    //left side widget
    $('#dvZ1TotalPoint').click(function (e) {
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "1";
        var TName = "All Kukatpally Zone Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&FDate=' + FDate);
    });
    $('#dvZ1CollectedPoint').click(function (e) {
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//param
        var IsActive = "-1";
        var TName = "All Kukatpally Zone Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ1HotspotProgress').click(function (e) {// For HotSpot Progress Click Event
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '9';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Kukatpally Zone HotSpot Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ1NotCollectedPoint').click(function (e) {

        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Kukatpally Zone Not Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvOCollected').click(function (e) {//Sonus
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '5';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "Operation Collected Avg. 7 Days";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvONotCollected').click(function (e) {//Sonus
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '6';
        var FDate = document.getElementById('txtFromDate').value;
        var PointTypeId = "-1";
        var IsActive = "-1";
        var TName = "Operation Not Collected Avg. 7 Days";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvOCollectedInRoute').click(function (e) {//Sonus
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '7';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "Operation Route Collected Avg. 7 Days";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvONotCollectedInRoute').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '8';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "Operation Route Not Collected Avg. 7 Days";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ1UniquePoint').click(function (e) {
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Kukatpally Zone Unique Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ1MoreThanOncePoint').click(function (e) {
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Kukatpally Zone More Than Once Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });


    $('#dvZ2TotalPoint').click(function (e) {
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "1";
        var TName = "All Serilingampally Zone Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&FDate=' + FDate);
    });
    $('#dvZ2CollectedPoint').click(function (e) {
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Serilingampally Zone Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ2NotCollectedPoint').click(function (e) {
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Serilingampally Zone Not Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + 'FDate' + FDate);
    });
    $('#dvZ2UniquePoint').click(function (e) {
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Serilingampally Zone Unique Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ2MoreThanOncePoint').click(function (e) {
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Serilingampally Zone More Than Once Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });

    $('#dvZ2HotspotProgress').click(function (e) {// For HotSpot Progress Click Event
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '9';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Serilingampally Zone HotSpot Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });

    $('#dvZ3TotalPoint').click(function (e) {
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "1";
        var TName = "All L B Nagar Zone Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&FDate=' + FDate);
    });
    $('#dvZ3CollectedPoint').click(function (e) {
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All L B Nagar Zone Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ3NotCollectedPoint').click(function (e) {
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All L B Nagar Zone Not Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ3HotspotProgress').click(function (e) {// For HotSpot Progress Click Event
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '9';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All L B Nagar Zone HotSpot Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ3UniquePoint').click(function (e) {
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All L B Nagar Zone Unique Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ3MoreThanOncePoint').click(function (e) {
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All L B Nagar Zone More Than Once Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });


    $('#dvZ4TotalPoint').click(function (e) {
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "1";
        var TName = "All Charminar Zone Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&FDate=' + FDate);
    });
    $('#dvZ4CollectedPoint').click(function (e) {
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Charminar Zone Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ4HotspotProgress').click(function (e) {// For HotSpot Progress Click Event
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '9';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Charminar Zone HotSpot Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    })
    $('#dvZ4NotCollectedPoint').click(function (e) {
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Charminar Zone Not Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ4UniquePoint').click(function (e) {
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Charminar Zone Unique Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ4MoreThanOncePoint').click(function (e) {
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Charminar Zone More Than Once Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });


    $('#dvZ5TotalPoint').click(function (e) {
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "1";
        var TName = "All Khairatabad Zone Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&FDate=' + FDate);
    });
    $('#dvZ5CollectedPoint').click(function (e) {
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Khairatabad Zone Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ5HotspotProgress').click(function (e) {// For HotSpot Progress Click Event
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '9';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Khairatabad Zone HotSpot Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    })
    $('#dvZ5NotCollectedPoint').click(function (e) {
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Khairatabad Zone Not Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ5UniquePoint').click(function (e) {
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Khairatabad Zone Unique Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ5MoreThanOncePoint').click(function (e) {
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Khairatabad Zone More Than Once Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });



    $('#dvZ6TotalPoint').click(function (e) {
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "1";
        var TName = "All Secunderabad Zone Geo Point";
        window.open('/Collection/AllGeoPointNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&FDate=' + FDate);
    });
    $('#dvZ6CollectedPoint').click(function (e) {
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Secunderabad Zone Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ6HotspotProgress').click(function (e) {// For HotSpot Progress Click Event
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '9';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Khairatabad Zone HotSpot Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    })
    $('#dvZ6NotCollectedPoint').click(function (e) {
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Secunderabad Zone Not Collected Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ6UniquePoint').click(function (e) {
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Secunderabad Zone Unique Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZ6MoreThanOncePoint').click(function (e) {
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All Secunderabad Zone More Than Once Geo Point";
        window.open('/Collection/AllPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });

    //right side widget
    $('#dvZ1TotalRoutes').click(function (e) {
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Kukatpally Zone Route";
        window.open('/Collection/AllRouteNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId);
    });
    $('#dvZ1RouteEarlyCompl').click(function (e) {  // For Early Completion KHSB
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Kukatpally Zone Route";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZAllRouteEarlyCompl').click(function (e) {  // For Early Completion
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Zone  Early Completion ";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ2RouteEarlyCompl').click(function (e) {  // For Early Completion
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Kukatpally Zone Early Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ3RouteEarlyCompl').click(function (e) {  // For Early Completion
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All L B Nagar Zone  Zone Early Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ4RouteEarlyCompl').click(function (e) {  // For Early Completion
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Charminar Zone Early Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ5RouteEarlyCompl').click(function (e) {  // For Early Completion
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Khairatabad  Zone Early Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ6RouteEarlyCompl').click(function (e) {  // For Early Completion
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Secunderabad   Zone Early Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ1RouteProgress').click(function (e) {  // For Early Completion
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Kukatpally   Zone Route Progress ";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvbZAllRouteProgress').click(function (e) {  // For Early Completion
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Kukatpally   Zone Route Progress ";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ2RouteProgress').click(function (e) {  // For Early Completion
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Serilingampally  Zone Route Progress ";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ3RouteProgress').click(function (e) {  // For Early Completion
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All L B Nagar  Zone Route Progress ";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ4RouteProgress').click(function (e) {  // For Early Completion
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Charminar Zone Route Progress ";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ5RouteProgress').click(function (e) {  // For Early Completion
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Khairatabad Zone Route Progress ";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ6RouteProgress').click(function (e) {  // For Early Completion
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Secunderabad Zone Route Progress ";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ1RouteLateCompl').click(function (e) {  // For Early Completion
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Kukatpally  Zone Late Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZAllRouteLateCompl').click(function (e) {  // For Early Completion
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Zone Late Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ2RouteLateCompl').click(function (e) {  // For Early Completion
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Serilingampally Zone Late Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ3RouteLateCompl').click(function (e) {  // For Early Completion
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All L B Nagar Zone Late Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ4RouteLateCompl').click(function (e) {  // For Early Completion
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Charminar Zone Late Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ5RouteLateCompl').click(function (e) {  // For Early Completion
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Khairatabad  Zone Late Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ6RouteLateCompl').click(function (e) {  // For Early Completion
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Secunderabad Zone Late Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ1RouteNotCompl').click(function (e) {  // For Ear
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Kukatpally Zone Not Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZAllRouteNotCompl').click(function (e) {  // For Ear
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Zone Not Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ2RouteNotCompl').click(function (e) {  // For Ear
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Serilingampally Zone Not Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ3RouteNotCompl').click(function (e) {  // For Ear

        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All L B Nagar Zone Not Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ4RouteNotCompl').click(function (e) {  // For Ear
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Charminar Zone Not Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ5RouteNotCompl').click(function (e) {  // For Ear
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Khairatabad Zone Not Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ6RouteNotCompl').click(function (e) {  // For Ear
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Secunderabad Zone Not Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ1TotalTrips').click(function (e) {
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Kukatpally Zone Route";
        window.open('/Collection/AllRouteTripNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });


    $('#dvZ2TotalRoutes').click(function (e) {
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Serilingampally Zone Route";
        window.open('/Collection/AllRouteNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId);
    });
    //$('#dvZ2TotalTrips').click(function (e) {
    //    var ZId = '2';
    //    var CId = $('#ddlCircle').find(":selected").attr('value');
    //    var WId = $('#ddlWard').find(":selected").attr('value');
    //    var TripId = $('#ddlTrip').find(":selected").attr('value');
    //    var TId = '1';
    //    var PointTypeId = "-1";
    //    var IsActive = "1";
    //    var TName = "All Serilingampally Zone Route";
    //    window.open('/Collection/AllRouteTripNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    //});
    $('#dvZ2TotalTrips').click(function (e) {
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Serilingampally Zone Route";
        window.open('/Collection/AllRouteTripNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });

    $('#dvZ3TotalRoutes').click(function (e) {
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All L B Nagar Zone Route";
        window.open('/Collection/AllRouteNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId);
    });
    $('#dvZ3TotalTrips').click(function (e) {
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All L B Nagar Zone Route";
        window.open('/Collection/AllRouteTripNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });


    $('#dvZ4TotalRoutes').click(function (e) {
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Charminar Zone Route";
        window.open('/Collection/AllRouteNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId);
    });
    $('#dvZ4TotalTrips').click(function (e) {
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Charminar Zone Route";
        window.open('/Collection/AllRouteTripNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });



    $('#dvZ5TotalRoutes').click(function (e) {
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Khairatabad Zone Route";
        window.open('/Collection/AllRouteNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId);
    });
    $('#dvZ5TotalTrips').click(function (e) {
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Khairatabad Zone Route";
        window.open('/Collection/AllRouteTripNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });


    $('#dvZ6TotalRoutes').click(function (e) {
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Secunderabad Zone Route";
        window.open('/Collection/AllRouteNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId);
    });
    $('#dvZ6TotalTrips').click(function (e) {
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Secunderabad Zone Route";
        window.open('/Collection/AllRouteTripNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });

    $('#dvZAllTotalRoutes').click(function (e) {
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Zone Route Summary";
        window.open('/Collection/AllRouteNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId);
    });
    $('#dvZAllTotalTrips').click(function (e) {
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Zone Trip Summary";
        window.open('/Collection/AllRouteTripNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });

    //Master Vehicle

    $('#dvMTotalVehicle').click(function (e) {
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Total Vehicle";
        window.open('/Collection/TotalAssignedVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });
    $('#dvMVehicleNotAssigned').click(function (e) {

        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Vehicle Not Assigned";
        window.open('/Collection/TotalAssignedVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&TripId=' + TripId);
    });

    //Total Vehicle 
    $('#dvZ1Vehicle').click(function (e) {

        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';

        var TName = "All Kukatpally Zone Total Vehicle";
        window.open('/Collection/TotalUniqueVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName);
    });
    $('#dvZAllVehicle').click(function (e) {

        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';

        var TName = "All  Zone Total Vehicle";
        window.open('/Collection/TotalUniqueVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName);
    });
    $('#dvZ2Vehicle').click(function (e) {
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "-1";
        var TName = "All Serilingampally Zone Total Vehicle";
        window.open('/Collection/TotalUniqueVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvZ3Vehicle').click(function (e) {
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "-1";
        var TName = "All L B Nagar Zone Total Vehicle";
        window.open('/Collection/TotalUniqueVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvZ4Vehicle').click(function (e) {
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "-1";
        var TName = "All Charminar Zone Total Vehicle";
        window.open('/Collection/TotalUniqueVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvZ5Vehicle').click(function (e) {
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "-1";
        var TName = "All Khairatabad Zone Total Vehicle";
        window.open('/Collection/TotalUniqueVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvZ6Vehicle').click(function (e) {
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "-1";
        var TName = "All Secunderabad Zone Total Vehicle";
        window.open('/Collection/TotalUniqueVehicle?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });


    //left Zone Wise Master side widget
    $('#dvZAllTotalPoint').click(function (e) {
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var IsActive = "1";
        var TName = "All Zone Geo Point";
        window.open('/Collection/AllZoneWiseSummary?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive);
    });
    $('#dvZAllCollectedPoint').click(function (e) {
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "All Zone Collected Geo Point";
        window.open('/Collection/AllPointSummaryNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZAllHotspotProgress').click(function (e) {// For HotSpot Progress Click Event
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '9';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "All Zone HotSpot Collected Geo Point";
        window.open('/Collection/AllPointSummaryNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZAllNotCollectedPoint').click(function (e) {
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "All Zone Not Collected Geo Point";
        window.open('/Collection/AllPointSummaryNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZAllUniquePoint').click(function (e) {
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "All Zone Unique Geo Point";
        window.open('/Collection/AllPointSummaryNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });
    $('#dvZAllMoreThanOncePoint').click(function (e) {
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '4';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "All Zone More Than Once Geo Point";
        window.open('/Collection/AllPointSummaryNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&TId=' + TId + '&RouteId=' + RouteId + '&TripId=' + TripId + '&FDate=' + FDate);
    });

    //on Time completion
    $('#dvZAllRouteOnTimeCompl').click(function (e) {  // For Early Completion
        var ZId = '0';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '5';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Zone OnTime Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });

    $('#dvZ1RouteOnTimeCompl').click(function (e) {  // For Early Completion
        var ZId = '1';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '5';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Kukatpally Zone OnTime Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });

    $('#dvZ2RouteOnTimeCompl').click(function (e) {  // For Early Completion
        var ZId = '2';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '5';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Serilingampally Zone OnTime Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ3RouteOnTimeCompl').click(function (e) {  // For Early Completion
        var ZId = '3';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '5';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All L B Nagar Zone OnTime Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });

    $('#dvZ4RouteOnTimeCompl').click(function (e) {  // For Early Completion
        var ZId = '4';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '5';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Charminar Zone OnTime Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
    $('#dvZ5RouteOnTimeCompl').click(function (e) {  // For Early Completion
        var ZId = '5';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '5';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Khairatabad Zone OnTime Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });

    $('#dvZ6RouteOnTimeCompl').click(function (e) {  // For Early Completion
        var ZId = '6';
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var RouteId = $('#ddlRoute').find(":selected").attr('value');
        var TripId = $('#ddlTrip').find(":selected").attr('value');
        var TId = '5';
        var PointTypeId = "-1";
        var IsActive = "1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var TName = "All Secunderabad Zone OnTime Completion";
        window.open('/Collection/EarlyCompletion?ZId=' + ZId + '&CId=' + CId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&IsActive=' + IsActive + '&RouteId=' + RouteId + '&FDate=' + FDate);
    });
//----------------For Emergency Click event

    $('#dvETotal').click(function (e) {//Total
        var ZId = $('#ddlZone').find(":selected").attr('value');//KHSB
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
       
        var TId = '4';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;//parameter date
        var IsActive = "-1";
        var TName = "All  Emergency Point";
        window.open('/Collection/AllEmergencyPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId  + '&FDate=' + FDate);
    });
    $('#dvECollected').click(function (e) {//Collected
        var ZId = $('#ddlZone').find(":selected").attr('value');//KHSB
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "All  Collected Emergency Point";
        window.open('/Collection/AllEmergencyPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&FDate=' + FDate);
    });
    $('#dENotCollected').click(function (e) {//Not Collected
        var ZId = $('#ddlZone').find(":selected").attr('value');//KHSB
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "All Not  Collected Emergency Point";
        window.open('/Collection/AllEmergencyPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&FDate=' + FDate);
    });
    $('#dvECollectedInRoute').click(function (e) {//Shedule Count
        var ZId = $('#ddlZone').find(":selected").attr('value');//KHSB
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '3';
        var PointTypeId = "-1";
        var FDate = document.getElementById('txtFromDate').value;
        var IsActive = "-1";
        var TName = "All  Sheduled Emergency Point";
        window.open('/Collection/AllEmergencyPointCltnNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&PointTypeId=' + PointTypeId + '&FDate=' + FDate);
    });
}


function SetMasterAndOperationData(Type) {
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    var TripId = '0';
    var RouteId = '0';
    var FDate = document.getElementById('txtFromDate').value;//param
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        WardId = $('#ddlWard').val();
        TripId = $('#ddlTrip').val();
        RouteId = $('#ddlRoute').val();
        FDate = document.getElementById('txtFromDate').value;//param
    }


    $.ajax({
        type: "POST",
        url: '/Collection/GetAllPointNoti',
        data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, RouteId: RouteId, TripId: TripId, fromDate: FDate },
        success: function (data) {
            var myJSONData = JSON.parse(data);
            var Mdata = myJSONData.Table[0];
            var Odata = myJSONData.Table1[0];


            $("#h6MTotal").html(Mdata.TotalCount);
            $("#h6MActiveCount").html(Mdata.Active);
            $("#h6AwaitingHL").html(Mdata.ActivePerc);
            $("#h6MActivePerc").html("[" + Mdata.ActivePerc + "%" + "]");
            $("#h6MInActiveCount").html(Mdata.InActive);
            $("#h6MInActivePerc").html("[" + Mdata.InActivePerc + "%" + "]");
            $("#h6MAssignedToRouteCount").html(Mdata.AssignedRouteCount);
            $("#h6MAssignedToRoutePerc").html("[" + Mdata.AssignedPerc + "%" + "]");
            $("#h6MNotAssignedToRouteCount").html(Mdata.NotAssignedCount);
            $("#h6MNotAssignedToRoutePerc").html("[" + Mdata.NotAssignedPerc + "%" + "]");


            $("#h6OTotalCount").html(Odata.TotalCount);
            $("#h6OCollectedCount").html(Odata.CollectedPointCount);
            $("#h6OCollectedPerc").html("[" + Odata.CollectedPointCountPerc + "%" + "]");
            $("#h6ONotCollectedCount").html(Odata.NotCollectedPointCount);
            $("#h6ONotCollectedPerc").html("[" + Odata.NotCollectedPointCountPerc + "%" + "]");
            $("#h6OCollectedInRouteCount").html(Odata.CollectedPointCountInRoute);
            $("#h6OCollectedInRoutePerc").html("[" + Odata.CollectedPointCountInRoutePerc + "%" + "]");
            $("#h6NotCollectedInRouteCount").html(Odata.NotCollectedPointCountInRoute);
            $("#h6NotCollectedInRoutePerc").html("[" + Odata.NotCollectedPointCountInRoutePerc + "%" + "]");
            $("#h7MTotalVehicle").html(Mdata.TotalVehicle);
            $("#h7MVehicleNotAssigned").html(Mdata.VehicleNotAssigned + " [" + Mdata.TotalVehiclePerc + "%" + "]");

        },
        error: function (result) {
        }
    });
}
function SetEmerGencyPointData(Type) {
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
   
    var FDate = document.getElementById('txtFromDate').value;
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        WardId = $('#ddlWard').val();
        
        FDate = document.getElementById('txtFromDate').value;//param
    }


    $.ajax({
        type: "POST",
        url: '/Collection/GetAllEmerGencyPointNoti',
        data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, fromDate: FDate },
        success: function (data) {
            
            var myJSONData = JSON.parse(data);
            var Mdata = myJSONData.Table[0];
        
            $("#hETotalCount").html(Mdata.TotalCount);
            $("#hECollectedCount").html(Mdata.CollectedPointCount);
            $("#hENotCollectedCount").html(Mdata.NotCollectedPointCount);
            $("#hESchedulepoints").html(Mdata.SheduleCollectedPointCount);
           

        },
        error: function (result) {
        }
    });
}
var map1;
var circle;
var marker;
var geocoder;
var SData;

var currentlatlng = new google.maps.LatLng(17.4940964, 78.4000115);
function loadMap() {
    var lat;
    var lng;

    setLatLongValue();
    var mapOptions = {
        zoom: 15,
        center: currentlatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map1 = new google.maps.Map(document.getElementById('dvIMap1'), mapOptions);
    //var input = document.getElementById('searchInput');
    //map1.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    //geocoder = new google.maps.Geocoder();
    //var autocomplete = new google.maps.places.Autocomplete(input);
    //autocomplete.bindTo('bounds', map1);



    //autocomplete.addListener('place_changed', function () {

    //    var place = autocomplete.getPlace();
    //    currentlatlng = new google.maps.LatLng(place.geometry.location.lat(), place.geometry.location.lng());//e.latLng;
    //    if (place.geometry.viewport) {
    //        map1.fitBounds(place.geometry.viewport);
    //    } else {
    //        map1.setCenter(place.geometry.location);
    //        map1.setZoom(17);
    //    }
    //    marker.setPosition(place.geometry.location);
    //    marker.setVisible(true);
    //    setLatLongValue();
    //    drawCircle();
    //});

    google.maps.event.addDomListener(map1, 'click', function (e) {

        currentlatlng = e.latLng;
        if (currentlatlng) {
            map1.panTo(currentlatlng);
            setLatLongValue();
            setMarker();
        }
    });
    getLatLongValue();

}


function setMarker() {
    if (marker != undefined)
        marker.setMap(null);

    marker = new google.maps.Marker({
        position: currentlatlng,
        draggable: true,
        map: map1
    });

    if (marker) {
        google.maps.event.addDomListener(marker, "drag", function () {
            currentlatlng = marker.getPosition();
            // setLatLongValue();
            drawCircle();
        });
        drawCircle();
    }


}
function drawCircle() {

    if (circle != undefined)
        circle.setMap(null);

    var radius = 50;


    if ($('#txtRadius').val() != '' && !isNaN($('#txtRadius').val()) && parseInt($('#txtRadius').val()) > 0) {
        radius = parseInt($('#txtRadius').val());
    }

    var options = {
        strokeColor: '#96b4c3',
        strokeOpacity: 1.0,
        strokeWeight: 5,
        fillColor: '#96b4c3',
        fillOpacity: 0.2,
        map: map1,
        center: currentlatlng,
        radius: radius
    };

    circle = new google.maps.Circle(options);
    setLatLongValue();
}
function setLatLongValue() {

    $('#txtLat').val(currentlatlng.lat());
    $('#txtLng').val(currentlatlng.lng());
    GetAddress();
}

function GetAddress() {

    var geocoder = geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': currentlatlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                $('#txtLocation').val(results[1].formatted_address);
                //alert("Location: " + results[1].formatted_address);
            }
        }
    });
}

function getLatLongValue() {

    if ($('#txtLat').val() != '' && !isNaN($('#txtLat').val()) && parseInt($('#txtLat').val()) > 0) {
        if ($('#txtLng').val() != '' && !isNaN($('#txtLng').val()) && parseInt($('#txtLng').val()) > 0) {
            currentlatlng = new google.maps.LatLng($('#txtLat').val(), $('#txtLng').val());
            map1.panTo(currentlatlng);
            setMarker();
        }
    }
}
function ShowMapPopup(objthis) {

    var lat = $(objthis).attr('data-Lat');
    var lng = $(objthis).attr('data-Lng');
    var GeoPointName = $(objthis).attr('data-GeoPointName');
    var GeoPointCategory = $(objthis).attr('data-GeoPointCategory');
    var Location = $(objthis).attr('data-Location');
    var Icon = '../otherfiles/global_assets/images/green-dot.png';

    var mapOptions = {
        center: new google.maps.LatLng(lat, lng),
        zoom: 14,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    var map = new google.maps.Map($("#dvIMap")[0], mapOptions);

    var infowindow = new google.maps.InfoWindow();
    var marker1, i;
    var iconBase = Icon;


    contentString = "<div style='float:right; padding: 10px;font-size: 14px;background-color: #33414E;color: white;'>Geo Point Name-" + GeoPointName +
        "<br/>Category-" + GeoPointCategory +
        "<br/>Location-" + Location +
        "</div>";
    marker1 = new google.maps.Marker({
        position: new google.maps.LatLng(lat, lng),
        map: map,
        icon: iconBase,
        content: contentString

    });

    google.maps.event.addListener(marker1, 'mouseover', (function (marker1, i) {
        return function () {
            infowindow.setContent(marker1.content);
            infowindow.open(map, marker1);
        }
    })(marker1, i));
    google.maps.event.addListener(marker1, 'mouseout', (function (marker1, i) {
        return function () {
            infowindow.close(map, marker1);
        }
    })(marker1, i));

    $('#viewonmap').modal('toggle');

    return false;
}

function GetGeopointDataForMap(Type, Count) {
    
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    //var PointTypeId = '-1';
    var Status = '-1';
    var RouteId = '0';
    var TripId = '0';
    var FDate = document.getElementById('txtFromDate').value;//param
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        RouteId = $('#ddlRoute').val();
        TripId = $('#ddlTrip').val();
        FDate = FDate;
        // PointTypeId = $('#ddlPointToRoute').val();
        //Status = $('#ddlStatus').val();
    }
    
    if ($("#rdbEmergency").prop("checked") == true) {
        $.ajax({
            type: "POST",
            // contentType: "application/json; charset=utf-8",
            url: '/Collection/GetAllVisitSummaryForEmergencyMap_Paging',
            dataType: "json",
            data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, Status: Status, VisitCount: Count, SDate: FDate },
            success: function (result) {
                
                var locations = JSON.parse(result);

                SetMapData(locations, Count);
            },
            error: function (result) {
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            // contentType: "application/json; charset=utf-8",
            url: '/Collection/GetAllVisitSummaryForMap_Paging',
            dataType: "json",
            data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, Status: Status, routeId: RouteId, TripId: TripId, VisitCount: Count, SDate: FDate },
            success: function (result) {

                var locations = JSON.parse(result);

                SetMapData(locations, Count);
            },
            error: function (result) {
            }
        });
    }
   
}
function GetGeopointDataForMapForEmergency(Type, Count) {

    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    //var PointTypeId = '-1';
    var Status = '-1';
   
    var FDate = document.getElementById('txtFromDate').value;//param
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
      
        FDate = FDate;
        // PointTypeId = $('#ddlPointToRoute').val();
        //Status = $('#ddlStatus').val();
    }

    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: '/Collection/GetAllVisitSummaryForEmergencyMap_Paging',
        dataType: "json",
        data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, Status: Status, VisitCount: Count, SDate: FDate },
        success: function (result) {
            
            var locations = JSON.parse(result);

            SetMapData(locations, Count);
        },
        error: function (result) {
        }
    });
}
var allCountnew = 0;
function GetAllCountDataforMap(Type)

{
    allCountnew = 0;
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    //var PointTypeId = '-1';
    var Status = '-1';
    var RouteId = '0';
    var TripId = '0';
    var FDate = document.getElementById('txtFromDate').value;//param
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        RouteId = $('#ddlRoute').val();
        TripId = $('#ddlTrip').val();
        FDate = FDate;
        // PointTypeId = $('#ddlPointToRoute').val();
        //Status = $('#ddlStatus').val();
    }
    if ($("#rdbEmergency").prop("checked") == true) {
        $.ajax({
            type: "POST",
            // contentType: "application/json; charset=utf-8",
            url: '/Collection/GetAllVisitSummaryForEmergencyMap_Paging',
            dataType: "json",
            data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, Status: Status, VisitCount: -2, SDate: FDate },
            success: function (result) {
                
                var locations = JSON.parse(result);

                SetMapData(locations, Count);
            },
            error: function (result) {
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            // contentType: "application/json; charset=utf-8",
            url: '/Collection/GetAllVisitSummaryForMap_Paging',
            dataType: "json",
            data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, Status: Status, routeId: RouteId, TripId: TripId, VisitCount: -2, SDate: FDate },
            async: false,
            success: function (result) {

                var locations = JSON.parse(result);
                for (i = 0; i < locations.length; i++) {
                    if (locations[i].GeoType == 1) {
                        allCountnew = allCountnew + 1;
                    }

                }
                //SetMapData(locations);
            }

        });
    }
    
}
var greycount = 0;
var redcount = 0;
var greencount = 0;
var yellowcount = 0;
var bluecount = 0;
var orangecount = 0;
var purplecount = 0;
var Allcount = 0;
function SetMapData(locations,geocount) {
    
    greycount = 0;
    redcount = 0;
    greencount = 0;
    yellowcount = 0;
    bluecount = 0;
    orangecount = 0;
    purplecount = 0;
    Allcount = 0;
    $("#htagidred").text(0);
    $("#htagidgreen").text(0);
    // $("#htagidgreen").text(greencount);
    $("#htagidyellow").text(0);
    $("#htagidblue").text(0);
    $("#htagidorange").text(0);
    $("#htagidpurple").text(0);
    $("#htagidgrey").text(0);
    $("#htagidAll").text(0);
    
    GetAllCountDataforMap();
    if (geocount == -1) {
        $("#htagidAll").text(0);
    }
    else {
        $("#htagidAll").text(allCountnew);
    }
  
    var infowindow = new google.maps.InfoWindow();

    var lat = locations.length ? locations[0].Lat : 17.4940964;
    var longt = locations.length ? locations[0].Lng : 78.4000115;
    var map = new google.maps.Map(document.getElementById('map_basic'), {
        zoom: 12,
        center: new google.maps.LatLng(lat, longt),
        mapTypeId: google.maps.MapTypeId.terrain

    });

    var contentString;
    var marker, i, j;
    var latlngbounds = new google.maps.LatLngBounds();

    var iconBase = '../otherfiles/global_assets/images/';

    var markerarray1 = new Array();
    // setTimeout(function () {
    
    for (i = 0; i < locations.length; i++) {


        var datainsert = {
            lat: parseFloat(locations[i].Lat),
            lng: parseFloat(locations[i].Lng)

        };
        markerarray1.push(datainsert);
        var myLatlng = new google.maps.LatLng(locations[i].Lat, locations[i].Lng);

       
        //for(v)

        var icon;
        if (locations[i].GeoType == 1) {
            Allcount = Allcount + 1;
            if (locations[i].TotalVisit == 0) {
                redcount=  redcount+1;
                //  icon = iconBase + "greenhome.png";
                icon = iconBase + "dot-red-low.png";

            }
            else if (locations[i].TotalVisit == 1) {
                icon = iconBase + "dot-green.png";
                greencount = greencount + 1;
            }
            else if (locations[i].TotalVisit == 2) {
                yellowcount = yellowcount + 1;
                icon = iconBase + "dot-yellow.png";
            }
            else if (locations[i].TotalVisit == 3) {
                bluecount = bluecount + 1;
                icon = iconBase + "dot-blue.png";
            }
            else if (locations[i].TotalVisit == 4) {
                orangecount = orangecount + 1;
                icon = iconBase + "dot-orange.png";
            }
            else {
                purplecount = purplecount + 1;
                icon = iconBase + "dot-purple.png";
            }
        }
        else {
            greycount=greycount +1;
            icon = iconBase + "dot-grey.png";
        }
        //if (locations[i].CStatus == 1)
        $("#htagidred").text(redcount);
        $("#htagidgreen").text(greencount);
       // $("#htagidgreen").text(greencount);
        $("#htagidyellow").text(yellowcount);
        $("#htagidblue").text(bluecount);
        $("#htagidorange").text(orangecount);
        $("#htagidpurple").text(purplecount);
        $("#htagidgrey").text(greycount);
        // $("#htagidAll").text(redcount + greencount + yellowcount + bluecount + orangecount + purplecount);
        

        const iconval = {
            url: icon, // url
            scaledSize: new google.maps.Size(12, 12), // scaled size
            origin: new google.maps.Point(0, 0), // origin
            anchor: new google.maps.Point(0, 0) // anchor
        };
        contentString = '';
        contentString += '<table class="table table-bordered border-success">';
        contentString += '<thead>';
        contentString += '<tbody>';

        contentString += '<tr><td>Point Name:</td><td style="align:right;">' + locations[i].GeoPointName + '</td></tr>';
        contentString += '<tr><td>Category:</td><td style="align:right;">' + locations[i].GeoPointCategory + '</td></tr>';
        contentString += '<tr><td>Zone:</td><td style="align:right;">' + locations[i].ZoneNo + '</td></tr>';
        contentString += '<tr><td>Circle:</td><td style="align:right;">' + locations[i].CircleName + '</td></tr>';
        contentString += '<tr><td>Ward:</td><td style="align:right;">' + locations[i].WardNo + '</td></tr>';
        //contentString += '<tr><td>Location:</td><td style="align:right;">' + locations[i].Location + '</td></tr>'; 
        contentString += '<tr><td>TotalVisit:</td><td style="align:right;">' + locations[i].TotalVisit + '</td></tr>';
        contentString += '<tr><td>Last Log:</td><td style="align:right;">' + locations[i].LastPickUpTime + '</td></tr>';
        contentString += '<tr><td>Radius:</td><td style="align:right;">' + locations[i].Radius + '</td></tr>';
        contentString += '<tr><td>Shift Name:</td><td style="align:right;">' + locations[i].ShiftName + '</td></tr>';
        contentString += '<tr><td>Shift Date:</td><td style="align:right;">' + locations[i].ShiftDate + '</td></tr>';
        contentString += '<tr><td>Lat,Lng:</td><td style="align:right;">' + parseFloat(locations[i].Lat).toFixed(4) + ', ' + parseFloat(locations[i].Lng).toFixed(4) + '</td></tr>';
        contentString += '</tbody>';
        contentString += '</table>';



        marker = new google.maps.Marker({
            position: myLatlng,
            map: map,

            //icon: icon,
            content: contentString,

            icon: iconval
        });

        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent(marker.content);
                infowindow.open(map, marker);
            }
        })(marker, i));
        //google.maps.event.addListener(marker, 'mouseout', (function (marker, i) {
        //    return function () {
        //        infowindow.close(map, marker);
        //    }
        //})(marker, i));
        //Extend each marker's position in LatLngBounds object.
        latlngbounds.extend(marker.position);
    }
    //}, 1000);



    //Get the boundaries of the Map.
    var bounds = new google.maps.LatLngBounds();

    //Center map and adjust Zoom based on the position of all markers.
    // map.setCenter(latlngbounds.getCenter());
    // map.fitBounds(latlngbounds);
}
$(document).on("click", "#rdbEmergency", function () {
    GetGeopointDataForMapForEmergency('Load', '-2');

})
$(document).on("click", "#rdbGVP", function () {
    GetGeopointDataForMap('Load', '-2');

})