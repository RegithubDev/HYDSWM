var tablpointarr = [];
var today = new Date().toLocaleDateString();
var StartPointTime = '';
var islastpoint = 0;
var ismidpoint = 0;
var isclickedonedit = 0;
var clickedstopid = 0;
function AddStops() {
    var Errmsg = 'Please Fill All Required Details';
    var stopinfo = $('#ddlStop').val();
    if (stopinfo != null) {
        var IsValid = 1;

        var stopid = stopinfo.split('-')[0];
        var stopname = $("#ddlStop option:selected").text();
        // var Tripname = $("#txtTrip").val();
        var RouteTripCode = $("#txtTripUId").val();
        var TrInfo = $('#hfTripId').val();
        var TripId = $('#hfTripId').val();
        var BufferMin = $('#hfBufferMinId').val();
        var VUId = $('#hfVehicleUIdId').val();
        var PickupTime = $('#txtPickupTime').val();
        var PCatId = $('#ddlCatgory').val();
        var PCatName = $("#ddlCatgory option:selected").text();
        var Lat = stopinfo.split('-')[1];
        var Lng = stopinfo.split('-')[2];
        var WardId = $('#ddlWard').val();
        //var SArrvlTime = $('#txtSArrvlTime').val();
        //var SDeptTime = $('#txtSDeptTime').val();

        if (stopid == '' || PickupTime == '' || TripId == '' || PCatId == '')
            IsValid = 0

        if (IsValid == 1) {
            if (PCatId != 5) {

                const aDate = new Date(today + ' ' + StartPointTime)
                const bDate = new Date(today + ' ' + PickupTime)
                if (bDate <= aDate) {
                    IsValid = 0
                    Errmsg = 'Points Pickuptime can not be less than start point pickup time';
                }
                $("#ddlWard").prop("disabled", false);
                $("#ddlCatgory").prop("disabled", false);
                $("#ddlStop").prop("disabled", false);
            }


            var IsExist = false;
            if (isclickedonedit == 1) {
                tablpointarr = tablpointarr.filter(function (obj) {
                    return parseInt(obj.StopId) !== parseInt(clickedstopid);
                });
                $("#tblStop > tbody").html("");
            }

            else {
                $('#tblStop > tbody > tr').each(function (i, el) {
                    var $tds = $(this).find('td');
                    var Istopid = $(this).attr('data-StopId');

                    if (Istopid == stopid)
                        IsExist = true;

                });
            }
            if (IsExist == false) {

                var data = {};
                data.BufferMin = BufferMin;
                data.StopId = stopid;
                data.PickupTime = PickupTime;
                data.TripId = TripId;
                data.TripName = RouteTripCode;
                data.StopName = stopname;
                data.VehicleUId = VUId;
                data.PointCatId = PCatId;
                data.PointCatName = PCatName;
                data.Lat = Lat;
                data.Lng = Lng;
                data.WardId = WardId;
                //data.DeptTime = SDeptTime;
                tablpointarr.push(data);

                if (PCatId == 5)
                    StartPointTime = PickupTime;

                const sorted = tablpointarr.sort((a, b) => {

                    const aDate = new Date(today + ' ' + a.PickupTime)
                    const bDate = new Date(today + ' ' + b.PickupTime)

                    return bDate.getTime() - aDate.getTime();
                })
                SetTableData(sorted, 0);
                SetMapData(sorted);
                //$("#tblStop > tbody").html("");
                //var IICount = sorted.length;
                //for (var i = 0; i < sorted.length; i++) {
                //    var tr = '<tr data-StopId=' + sorted[i].StopId + ' data-tripid=' + sorted[i].TripId + ' data-pointcatid=' + sorted[i].PointCatId + '><td >' + IICount +
                //        '</td><td>' + sorted[i].StopName +
                //        '</td><td>' + sorted[i].PointCatName +
                //        '</td><td>' + sorted[i].PickupTime +
                //        '</td><td >' + sorted[i].TripName +
                //        '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveRow(this);"> <i class="icon-close2"></i> Remove</button>'
                //    '</td></tr>';
                //    $('#tblStop > tbody').append(tr);
                //    IICount--;
                //}
                isclickedonedit = 0;
                clickedstopid = 0;
                AllCategoryLst(0);
                $('#txtPickupTime').val('');
                $("#ddlWard").prop("disabled", false);
                $("#ddlCatgory").prop("disabled", false);
                $("#ddlStop").prop("disabled", false);
            }
            else
                ShowCustomMessage('0', 'This geo point is already exists', '');
        }
        else
            ShowCustomMessage('0', Errmsg, '');
    }
    else
        ShowCustomMessage('0', Errmsg, '');


}
function RemoveRow(objthis) {

    $("#ddlWard").prop("disabled", false);
    $("#ddlCatgory").prop("disabled", false);
    $("#ddlStop").prop("disabled", false);
    var stopid = $(objthis).closest('tr').attr('data-StopId');
    var pointcatid = $(objthis).closest('tr').attr('data-pointcatid');

    if (pointcatid == 5) {
        isclickedonedit = 0;
        clickedstopid = 0;
        var answer = window.confirm("Do You Want To Remove Start Point. After Yes It Will Remove All Points.");
        if (answer) {
            $("#tblStop > tbody").html("");
            tablpointarr = [];
            StartPointTime = '';
        }

    }
    else {

        tablpointarr = tablpointarr.filter(function (obj) {
            return parseInt(obj.StopId) !== parseInt(stopid);
        });

        const sorted = tablpointarr.sort((a, b) => {

            const aDate = new Date(today + ' ' + a.PickupTime)
            const bDate = new Date(today + ' ' + b.PickupTime)

            return bDate.getTime() - aDate.getTime();
        })
        SetTableData(sorted, 0);
        SetMapData(sorted);
        //$("#tblStop > tbody").html("");

        //var IICount = sorted.length;
        //for (var i = 0; i < sorted.length; i++) {
        //    var tr = '<tr data-StopId=' + sorted[i].StopId + ' data-tripid=' + sorted[i].TripId + ' data-pointcatid=' + sorted[i].PointCatId + '><td >' + IICount +
        //        '</td><td>' + sorted[i].StopName +
        //        '</td><td>' + sorted[i].PointCatName +
        //        '</td><td>' + sorted[i].PickupTime +
        //        '</td><td >' + sorted[i].TripName +
        //        '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveRow(this);"> <i class="icon-close2"></i> Remove</button>'
        //    '</td></tr>';
        //    $('#tblStop > tbody').append(tr);
        //    IICount--;
        //}
        isclickedonedit = 0;
        clickedstopid = 0;
        $(objthis).closest('tr').remove();
    }
    AllCategoryLst(0);
}

function EditRow(objthis) {
    var stopid = $(objthis).closest('tr').attr('data-StopId');
    var lat = $(objthis).closest('tr').attr('data-lat');
    var lng = $(objthis).closest('tr').attr('data-lng');
    var pointcatid = $(objthis).closest('tr').attr('data-pointcatid');
    var wardid = $(objthis).closest('tr').attr('data-wardid');
    var PickupTime = $(objthis).closest('tr').find("td:eq(3)").text();

    if (pointcatid == 5) {
        var answer = window.confirm("Do You Want To Remove Start Point. After Yes It Will Remove All Points.");
        if (answer) {
            $("#tblStop > tbody").html("");
            tablpointarr = [];
            StartPointTime = '';
        }

    }
    else {
        $('#ddlWard').val(wardid);
        $('#ddlCatgory').val(pointcatid).change();;

        // AllCategoryLst(1);
        $("#ddlWard").prop("disabled", true);
        $("#ddlCatgory").prop("disabled", true);
        $("#ddlStop").prop("disabled", true);
        $('#txtPickupTime').val(PickupTime);
        setTimeout(() => {

            //$('#ddlStop').select2().select2('val', stopid);;
            $('#ddlStop').val(stopid + '-' + lat + '-' + lng).trigger('change');

        }, 1000);
        isclickedonedit = 1;
        clickedstopid = stopid;
        //tablpointarr = tablpointarr.filter(function (obj) {
        //    return parseInt(obj.StopId) !== parseInt(stopid);
        //});

        //const sorted = tablpointarr.sort((a, b) => {

        //    const aDate = new Date(today + ' ' + a.PickupTime)
        //    const bDate = new Date(today + ' ' + b.PickupTime)

        //    return bDate.getTime() - aDate.getTime();
        //})
        //SetTableData(sorted, 0);
        //SetMapData(sorted);


        //$(objthis).closest('tr').remove();
    }

}

function goBack() {
    window.history.back()
}
$(document).ready(function () {
    var ddId = getUrlParameterInfo('cid');
    document.getElementById("dvSubmit").style.display = 'none';
    // AllActiveGeoPointLst('ddlStop', 0, 'Select');

    if (ddId > 0)
        SetDataOncontrols(ddId);
    else
        AllCategoryLst(0);
});



function Formsubmit() {

    SaveAndUpdateTripPointInfo();
    return false;

}
function CallFunc(obj) {

    var ddId = $(obj).attr('cid');
    $('#user_content').load("/Collection/AddNRoute");
    $('#modal_form_vertical').modal('toggle');
    setTimeout(() => {
        //$("#ddlVehicleNo").select2({
        //    dropdownParent: $("#modal_form_vertical")
        //});
        $("#ddlStop").select2({
            dropdownParent: $("#modal_form_vertical")
        });
        AllActiveVehicleLst('ddlVehicleNo', 0, 'Select');
        AllActiveGeoPointLst('ddlStop', 0, 'Select');
        AllMTripLst('ddlTrip', 0, 'Select');
    }, 2000);

    if (ddId > 0)
        setTimeout(() => {
            SetDataOncontrols(ddId);
        }, 3000);

}
function SetDataOncontrols(ddId) {
    $.ajax({
        type: "post",
        url: "/Collection/GetPointsInfoByTripId",
        data: { RTDId: ddId },
        success: function (data) {

            var myJSONData = JSON.parse(data);
            var nstopdata = myJSONData.Table[0];
            var myJSON = myJSONData.Table1;
            //  var myJSON = JSON.parse(data);
            $("#hfRouteId").val(nstopdata.RouteId);
            $("#hfCircleId").val(nstopdata.CircleId);
            $("#hfZoneId").val(nstopdata.ZoneId);
            $("#txtZone").val(nstopdata.ZoneNo);
            $("#txtCircle").val(nstopdata.CircleName);
            $("#txtRouteCode").val(nstopdata.RouteCode);
            $("#hfTripId").val(nstopdata.TripId);
            $("#hfVehicleUIdId").val(nstopdata.VehicleUId);
            $("#hfBufferMinId").val(nstopdata.BufferMin);
            $("#txtTrip").val(nstopdata.TripName);
            $("#txtTripUId").val(nstopdata.TId);
            $("#hfShiftId").val(nstopdata.ShiftId);
            $("#txtShiftName").val(nstopdata.ShiftName);

            CallFZoneByCircle();

            for (var i = 0; i < myJSON.length; i++) {

                var data = {};
                data.BufferMin = myJSON[i].BufferMin;
                data.StopId = myJSON[i].StopId;
                data.PickupTime = myJSON[i].PickupTime;
                data.TripId = myJSON[i].TripId;
                data.TripName = myJSON[i].TId;
                data.StopName = myJSON[i].StopName;
                data.VehicleUId = myJSON[i].VehicleUId;
                data.PointCatId = myJSON[i].PointCategoryId;
                data.PointCatName = myJSON[i].GeoPointCategory;
                data.Lat = myJSON[i].Lat;
                data.Lng = myJSON[i].Lng;
                data.WardId = myJSON[i].WardId;
                //data.DeptTime = SDeptTime;
                tablpointarr.push(data);


            }
            const sorted = tablpointarr.sort((a, b) => {

                const aDate = new Date(today + ' ' + a.PickupTime)
                const bDate = new Date(today + ' ' + b.PickupTime)

                return bDate.getTime() - aDate.getTime();
            })
            SetTableData(sorted, 0);
            AllCategoryLst(0);
            SetMapData(sorted);
        }
    });
}
function SetTableData(sorted, Order) {

    var islastexist = 0;
    for (var i = 0; i < sorted.length; i++) {
        if (sorted[i].PointCatId == 6) {
            islastexist = 1;
        }
    }

    ismidpoint = 0;
    islastpoint = 0;

    document.getElementById("dvAddTrip").style.display = 'block';
    var IICount = sorted.length;
    $("#tblStop > tbody").html("");
    for (var i = 0; i < sorted.length; i++) {
        var edit = '<a href="javascript:void(0);" title="Edit" onclick="EditRow(this);"><i class="ti-pencil"></i></a>';
        if (sorted[i].PointCatId == 5) {
            StartPointTime = sorted[i].PickupTime;
            edit = '';
        }
        else if (sorted[i].PointCatId == 6) {
            document.getElementById("dvAddTrip").style.display = 'none';
            islastpoint = 1;
            document.getElementById("dvSubmit").style.display = 'block';
            edit = '';
        }
        else {
            if (islastexist == 1)
                edit = '';
            ismidpoint = 1;
        }

        var tr = '<tr data-StopId=' + sorted[i].StopId + ' data-tripid=' + sorted[i].TripId + ' data-pointcatid=' + sorted[i].PointCatId + ' data-wardid=' + sorted[i].WardId + ' data-lat=' + sorted[i].Lat + ' data-lng=' + sorted[i].Lng + '><td >' + IICount +
            '</td><td>[' + sorted[i].StopId + ' ] - ' + sorted[i].StopName +
            '</td><td>' + sorted[i].PointCatName +
            '</td><td>' + sorted[i].PickupTime +
            '</td><td >' + sorted[i].TripName +
            '</td><td><a href="javascript:void(0);" title="Remove" onclick="RemoveRow(this);" style="color:red;"><i class="icon-close2"></i></a>' +
            /*            '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveRow(this);"> <i class="icon-close2"></i> Remove</button>'+*/
            '</td><td>' + edit +
            '</td></tr>';
        $('#tblStop > tbody').append(tr);
        if (Order == 1)
            IICount++;
        else
            IICount--;
    }
    if (islastpoint == 0 || ismidpoint == 0)
        document.getElementById("dvSubmit").style.display = 'none';
}
function SaveAndUpdateTripPointInfo() {

    var isvalid = 1;
    var FormData = {
        RouteId: $("#hfRouteId").val(),
        TripId: $("#hfTripId").val(),
        ShiftId: $("#hfShiftId").val(),
        RouteCode: $("#txtRouteCode").val(),
        //VehicleUId: $('#ddlVehicleNo').val(),

    };
    var tablarr = tablpointarr;

    if (FormData.RouteCode == '' || FormData.TripId == '')
        isvalid = 0;

    if (tablarr.length == 0)
        isvalid = 0
    if (islastpoint == 0 || ismidpoint == 0)
        isvalid = 0;

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Collection/AddSTripPointInfo',
            data: { jobj: JSON.stringify(FormData), JArrayval: JSON.stringify(tablarr) },
            success: function (data) {
                var myJSON = JSON.parse(data);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg, '/Collection/AllTripPoint');

                    //$('#modal_form_vertical').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJSON.Msg, '');

            },
            error: function (result) {
                ShowMessage('0', 'Something Went Wrong!');
            }
        });
    else {

        ShowCustomMessage('0', 'Please Enter All Required Details', '');
    }
}
function CallFZoneByCircle() {

    AllMWardLst('ddlWard', 1, 'Select', $('#hfCircleId').val());
}
function CallAllGeoPointByZoneLst() {

    AllGeoPointByZoneLst('ddlStop', 1, 'Select', $('#ddlWard').val(), $('#ddlCatgory').val());
}
function AllCategoryLst(IsEdit) {
    var IICount = tablpointarr.length;
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

                if (IsEdit == 1) {
                    Resource = Resource + '<option value=' + Myjson[i].GPCId + '>' + Myjson[i].GeoPointCategory + '</option>';
                }
                else {
                    if (IICount == 0) {
                        if (Myjson[i].GPCId == 5)
                            Resource = Resource + '<option value=' + Myjson[i].GPCId + '>' + Myjson[i].GeoPointCategory + '</option>';
                    }
                    else if (IICount == 1) {
                        if (Myjson[i].GPCId != 5 && Myjson[i].GPCId != 6)
                            Resource = Resource + '<option value=' + Myjson[i].GPCId + '>' + Myjson[i].GeoPointCategory + '</option>';
                    }
                    else {
                        if (Myjson[i].GPCId != 5)
                            Resource = Resource + '<option value=' + Myjson[i].GPCId + '>' + Myjson[i].GeoPointCategory + '</option>';
                    }
                }
            }
            Resource = Resource + '</select>';
            $('#ddlCatgory').html(Resource);
        }
    });
}

function SetMapData(locations) {

    var infowindow = new google.maps.InfoWindow();

    var lat = locations.length ? locations[0].Lat : 17.4940964;
    var longt = locations.length ? locations[0].Lng : 78.4000115;
    var map = new google.maps.Map(document.getElementById('map_basic'), {
        zoom: 14,
        center: new google.maps.LatLng(lat, longt),
        mapTypeId: google.maps.MapTypeId.terrain

    });

    var contentString;
    var marker, i, j;
    var latlngbounds = new google.maps.LatLngBounds();

    var iconBase = '../otherfiles/global_assets/images/';

    var ICount = locations.length - 2;
    var markerarray1 = new Array();
    for (i = 0; i < locations.length; i++) {


        var datainsert = {
            lat: parseFloat(locations[i].Lat),
            lng: parseFloat(locations[i].Lng)

        };
        markerarray1.push(datainsert);
        var myLatlng = new google.maps.LatLng(locations[i].Lat, locations[i].Lng);
        var icon;

        //if (locations[i].CStatus == 1)
        //    icon = iconBase + "greenhome.png";


        contentString = '';
        contentString += '<table class="table table-bordered border-success">';
        contentString += '<thead>';
        contentString += '<tbody>';

        contentString += '<tr><td>Point Name:</td><td style="align:right;">' + locations[i].StopName + '</td></tr>';
        contentString += '<tr><td>Point Category:</td><td style="align:right;">' + locations[i].PointCatName + '</td></tr>';
        contentString += '<tr><td>Pickup Time:</td><td style="align:right;">' + locations[i].PickupTime + '</td></tr>';

        contentString += '</tbody>';
        contentString += '</table>';

        var marker_color = "009BEE";
        var marker_text_color = "FFFFFF";
        var lbltext = ICount;

        if (locations[i].PointCatId == 5) {
            marker_color = '39c449';
            lbltext = "S";
        }
        else if (locations[i].PointCatId == 6) {
            marker_color = 'd02323';
            lbltext = "E";
        }
        else {
            marker_color = '009efb';
            ICount--;
        }
        var pinIcon = new google.maps.MarkerImage(
            "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + ICount + "|" + marker_color + "|" + marker_text_color,
            null, /* size is determined at runtime */
            null, /* origin is 0,0 */
            null, /* anchor is bottom center of the scaled image */
            new google.maps.Size(30, 40)
        );
        iconSize = 0.45,
            icon = {
                path: "M53.1,48.1c3.9-5.1,6.3-11.3,6.3-18.2C59.4,13.7,46.2,0.5,30,0.5C13.8,0.5,0.6,13.7,0.6,29.9 c0,6.9,2.5,13.1,6.3,18.2C12.8,55.8,30,77.5,30,77.5S47.2,55.8,53.1,48.1z",
                fillColor: '#' + marker_color,
                fillOpacity: 1,
                strokeWeight: 0,
                scale: iconSize,
                anchor: new google.maps.Point(32, 80),
                labelOrigin: new google.maps.Point(30, 30)
            };

        var markerConfig = {
            lat: locations[i].Lat,
            lng: locations[i].Lng,
            label: lbltext.toString(),
            color: 'white'
        }
        //else
        marker = new google.maps.Marker({
            position: myLatlng,
            map: map,

            //icon: icon,
            content: contentString,
            // icon: icon//pinIcon//  "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + locations[i].SRouteTripInfo[j].RowNumber + "|" + marker_color + "|" + marker_text_color
            label: {
                text: markerConfig.label,
                color: '#' + marker_text_color,
            },
            icon: icon
        });

        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                infowindow.setContent(marker.content);
                infowindow.open(map, marker);
            }
        })(marker, i));

        latlngbounds.extend(marker.position);
    }

    var polyline = new google.maps.Polyline({
        path: markerarray1,
        strokeColor: '#3dbedb',
        strokeOpacity: 1.0,
        strokeWeight: 2,
        geodesic: true
        //icons: [{
        //    icon: { path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW },
        //    offset: '100%',
        //    repeat: '20px'
        //}]
    });
    polyline.setMap(map);

    //Get the boundaries of the Map.
    var bounds = new google.maps.LatLngBounds();

    //Center map and adjust Zoom based on the position of all markers.
    // map.setCenter(latlngbounds.getCenter());
    map.fitBounds(latlngbounds);
}