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

    AllMZoneLst('ddlZone', 0, 'All Zone');
    // AllGreyPointLst('ddlGreyPoint', 0, 'All GreyPoint')
    // GetDataTableData('Load');
    CallAllFunction('Load');
    //SetValues('Load');
    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#example').DataTable().row(tr);
        var TDate = tr.find('.gTDate').attr('cTDate');
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
                url: "/Collection/GetAllColPointByRouteDate",
                type: "POST",
                dataType: "json",
                data: { RouteId: RouteId, TDate: TDate },
                success: function (data) {
                    var myJSON = JSON.parse(data);
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });

});
function CallAllRouteByZoneAndWard() {
    $('#ddlRoute').val('0');
    AllMActiveRouteLst('ddlRoute', 0, 'All Route', $('#ddlZone').find(":selected").attr('value'), $('#ddlCircle').find(":selected").attr('value'));
}
function CallAllTripByRoute() {
    $('#ddlTrip').val('0');
    AllMActiveRouteTrip('ddlTrip', 0, 'All Trip', $('#ddlRoute').find(":selected").attr('value'));
}
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}
function format(item) {
    // `d` is the original data object for the row
    //alert(JSON.stringify(d.CASO));
    var InnerGrid = '<div class="table-scrollable"><table class="table display product-overview mb-30"><thead><tr><th>Sr. No.</th> <th> Vehicle No </th><th>Route Trip Code </th><th> Point Name </th><th> Status </th><th> Address </th><th> Collection Date & Time </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;
        var status = ''

        if (info.Status == 'COLLECTED')
            status = '<span class="badge badge-success">' + info.Status + '</span>';
        else {
            if (info.TypeId == 0)
                status = '<span class="badge badge-secondary">NOT COLLECTED</span>';
            else if (info.TypeId == 1)
                status = '<span class="badge badge-danger">' + info.Status + '</span>';
        }

        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td>' + info.VehicleNo + '</td>' +
            '<td>' + info.TId + '</td>' +
            '<td>' + info.PointName + '</td>' +
            '<td>' + status + '</td>' +
            '<td>' + info.Address + '</td>' +
            '<td>' + info.PickDTime + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}
var IsClick = 0;
var dt;

function ShowMapPopup(objthis) {

    var lat = $(objthis).attr('data-Lat');
    var lng = $(objthis).attr('data-Lng');
    var Location = $(objthis).attr('data-Location');
    var PickDTime = $(objthis).attr('data-PickDTime');
    var Status = $(objthis).attr('data-Status');
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


    contentString = "<div style='float:right; padding: 10px;font-size: 14px;background-color: #33414E;color: white;'>Status-" + Status +
        "<br/>Location-" + Location +
        "<br/>Pickup Date & Time-" + PickDTime +
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

function AllPointTimelineInfo(Type) {
    $("#dvTimeline").html("");
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    //var RouteId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    //var TripId = '0';
    var PageSize = 10;
    if (Type == 'Click') {
        //RouteId = $('#ddlRoute').val();
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        //TripId = $('#ddlTrip').val();
        PageSize = $('#ddlMap').val();
    }
    ShowLoading($('#dvTimeline'));
    $.ajax({
        type: "post",
        url: "/Collection/GetAllEmerPointForTimeline",
        data: { FromDate: FDate, ToDate: TDate, ZoneId: ZoneId, CircleId: CircleId, PageSize: PageSize },
        datatype: "json",
        success: function (data) {
            var locations = data;

           // HideLoading($('#dvTimeline'));
            SetMapData(locations);

        },
        error: function (result) {
            HideLoading($('#dvTimeline'));
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
    var radius = 150;

    var iconBase = '../otherfiles/global_assets/images/';
    for (i = 0; i < locations.length; i++) {
        if (locations[i].SRouteTripInfo != null) {
            var markerarray1 = new Array();
            for (j = 0; j < locations[i].SRouteTripInfo.length; j++) {


                var datainsert = {
                    lat: parseFloat(locations[i].SRouteTripInfo[j].Lat),
                    lng: parseFloat(locations[i].SRouteTripInfo[j].Lng)

                };
                markerarray1.push(datainsert);
                var myLatlng = new google.maps.LatLng(locations[i].SRouteTripInfo[j].Lat, locations[i].SRouteTripInfo[j].Lng);
                var icon;
                var cstatus = locations[i].SRouteTripInfo[j].SPickupTime;

                if (cstatus == 'N/A')
                    cstatus = 'Not Visited';
                //    icon = iconBase + "greenhome.png";


                contentString = '';
                contentString += '<table class="table table-bordered border-success">';
                contentString += '<thead>';
                contentString += '<tbody>';

                //contentString += '<tr><td>Route Code:</td><td style="align:right;">' + locations[i].RouteCode + '</td></tr>';
                contentString += '<tr><td>Vehicle No:</td><td style="align:right;">' + locations[i].SRouteTripInfo[j].VehicleNo + '</td></tr>';
                /*contentString += '<tr><td>Route Trip Code:</td><td style="align:right;">' + locations[i].TripName + '</td></tr>';*/
                contentString += '<tr><td>Point Name:</td><td style="align:right;">' + locations[i].SRouteTripInfo[j].GeoPointName + '</td></tr>';
                contentString += '<tr><td>Schedule Pickup Time:</td><td style="align:right;">' + locations[i].SRouteTripInfo[j].SchPickupTime + '</td></tr>';
                contentString += '<tr><td>Actual Pickup Time:</td><td style="align:right;">' + cstatus + '</td></tr>';
                //contentString += '<tr><td>Status:</td><td style="align:right;">' + locations[i].SRouteTripInfo[j].Status + '</td></tr>';

                contentString += '</tbody>';
                contentString += '</table>';

                var marker_color = "009BEE";
                var marker_text_color = "FFFFFF";

                if (locations[i].SRouteTripInfo[j].ColorId == 2)
                    marker_color = '4caf50';
                else if (locations[i].SRouteTripInfo[j].ColorId == 3)
                    marker_color = 'ee1809';
                else if (locations[i].SRouteTripInfo[j].ColorId == 1) {
                    marker_color = '868e96';
                    var coptions = {
                        strokeColor: '#96b4c3',
                        strokeOpacity: 1.0,
                        strokeWeight: 5,
                        fillColor: '#96b4c3',
                        fillOpacity: 0.2,
                        map: map,
                        center: myLatlng,
                        radius: radius
                    };

                    var circle = new google.maps.Circle(coptions);
                }
                else if (locations[i].SRouteTripInfo[j].ColorId == 4)
                    marker_color = 'ee1809';
                // marker_color = '009efb'; blue
                var pinIcon = new google.maps.MarkerImage(
                    "http://chart.apis.google.com/chart?chst=d_map_pin_letter&chld=" + locations[i].SRouteTripInfo[j].RowNumber + "|" + marker_color + "|" + marker_text_color,
                    null, /* size is determined at runtime */
                    null, /* origin is 0,0 */
                    null, /* anchor is bottom center of the scaled image */
                    new google.maps.Size(30, 40)
                );

                var lbltext = locations[i].SRouteTripInfo[j].RowNumber;

                //else
                if (locations[i].SRouteTripInfo[j].PointCatId == 5) {
                    lbltext = "S";
                    marker_color = '000000';
                }
                else if (locations[i].SRouteTripInfo[j].PointCatId == 6) {
                    marker_color = '000000';
                    lbltext = "E";

                }
                iconSize = 0.35,
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
                    lat: locations[i].SRouteTripInfo[j].Lat,
                    lng: locations[i].SRouteTripInfo[j].Lng,
                    label: lbltext.toString(),
                    color: 'white'
                }

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

                //circle.setMap(null);







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
        }
    }
    //Get the boundaries of the Map.
    var bounds = new google.maps.LatLngBounds();

    //Center map and adjust Zoom based on the position of all markers.
    // map.setCenter(latlngbounds.getCenter());
    map.fitBounds(latlngbounds);
}

function CallAllFunction(Type) {
    //GetDataTableData(Type);
    AllPointTimelineInfo(Type);
}

