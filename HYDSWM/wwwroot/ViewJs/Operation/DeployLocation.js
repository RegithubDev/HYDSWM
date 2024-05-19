var map1;
var circle;
var marker;
var geocoder;
var SData;

var currentlatlng = new google.maps.LatLng(17.4940964, 78.4000115);
function Formsubmit() {

    SaveAndUpdateGeoPointInfo();
    return false;
}
function CallFunc(obj) {

    var ddId = $(obj).attr('cid');
    $('#modal_form_vertical').modal('toggle');

    if (ddId > 0)
        SetDataOncontrols(ddId);
}
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
                Resource = Resource + '<option value=' + Myjson[i].GPCId + '>' + Myjson[i].GeoPointCategory + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlCatgory').html(Resource);
        }
    });
}
function SetDataOncontrols(ddId) {
    $.ajax({
        type: "post",
        url: "/Operation/GetDeployLocationInfoById",
        data: { DLId: ddId },
        success: function (data) {
            var myJSON = JSON.parse(data);
            $("#hfGPId").val(myJSON.DLId);
            $("#txtLandmark").val(myJSON.LandMark);
            setTimeout(() => {
                $("#ddlCatgory").val(myJSON.PointCategoryId);
            }, 1000);
            $("#ddlZone").val(myJSON.ZoneId).change();
            setTimeout(() => {
                $("#ddlCircle").val(myJSON.CircleId).change();

                setTimeout(() => {
                    $("#ddlWard").val(myJSON.WardId);
                }, 1000);
            }, 1000);

            $("#txtLat").val(myJSON.Lat);
            $("#txtLng").val(myJSON.Lng);
           
            $("#txtRadius").val(myJSON.Radius);
          
            if (myJSON.IsActive == 'True')
                $("#ckbIsActive").prop("checked", true);
            else
                $("#ckbIsActive").prop("checked", false);

       
            //loadMap();
            currentlatlng = new google.maps.LatLng(myJSON.Lat, myJSON.Lng);
            setMarker();
            loadMap();
        }
    });
}
function SaveAndUpdateGeoPointInfo() {
    
    var isvalid = 1
    var DLId = 0;
    if ($("#hfGPId").val() == "") {
        DLId = 0;
    }
    else {
        DLId = $("#hfGPId").val();
    }
    var fromdata = {
        DLId: DLId,
        DeployLocation: $("#txtLandmark").val(),
        Radius: $("#txtRadius").val(),
        Lat: $("#txtLat").val(),
        Lng: $("#txtLng").val(),
        IsActive: $('#ckbIsActive').is(':checked'),
       // CreatedBy: $("#hfLoginId").val(),
        ZoneId: $("#ddlZone").val(),
        CircleId: $("#ddlCircle").val(),
        WardId: $("#ddlWard").val(),
    };


    if (fromdata.DeployLocation == '' || fromdata.Radius == '' || fromdata.Lat == '' || fromdata.Lng == '' )
        isvalid = 0;
    
    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Operation/AddDeployLocation',
            // dataType: "json",
            data: { jobj: JSON.stringify(fromdata) },
            
            success: function (response) {
                var myJSON = JSON.parse(response);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg, '/Operation/AllDeployLocation');

                    $('#modal_form_vertical').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJSON.Msg, '');

            },
            error: function (result) {
                ShowCustomMessage('0', 'Something Went Wrong!', '');
            }
        });
    else {

        ShowCustomMessage('0', 'Please Enter All Required Details', '');
    }

}
function loadMap() {
    var lat;
    var lng;
    if ($('#txtLat').val() != '' && !isNaN($('#txtLat').val()) && parseInt($('#txtLat').val()) > 0) {
        lat = $('#txtLat').val();
        lng = $('#txtLng').val();
        currentlatlng = new google.maps.LatLng(lat, lng);
    }
    setLatLongValue();
    var mapOptions = {
        zoom: 15,
        center: currentlatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map1 = new google.maps.Map(document.getElementById('dvIMap1'), mapOptions);
    var input = document.getElementById('searchInput');
    map1.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    geocoder = new google.maps.Geocoder();
    var autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.bindTo('bounds', map1);



    autocomplete.addListener('place_changed', function () {

        var place = autocomplete.getPlace();
        currentlatlng = new google.maps.LatLng(place.geometry.location.lat(), place.geometry.location.lng());//e.latLng;
        if (place.geometry.viewport) {
            map1.fitBounds(place.geometry.viewport);
        } else {
            map1.setCenter(place.geometry.location);
            map1.setZoom(17);
        }
        marker.setPosition(place.geometry.location);
        marker.setVisible(true);
        setLatLongValue();
        drawCircle();
    });

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
            //setLatLongValue();
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


$(document).ready(function () {
    AllMZoneLst('ddlZone', 0, 'Select');
    AllMZoneLst('ddlSZone', 0, 'All Zone');
    //loadMap();
    CallAllFunc('Load');
    loadMap();
    $("#txtRadius").on("propertychange change keyup paste input", function () {
        drawCircle();
    });
});
var dt;
var IsClick = 0;
function GetDataTableData(Type) {
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    var Status = '-1';

    if (Type == 'Click') {
        IsClick = 1;
        ZoneId = $('#ddlSZone').val();
        CircleId = $('#ddlSCircle').val();
        WardId = $('#ddlSWard').val();
        Status = $('#ddlStatus').val();
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
                    extend: 'csvHtml5',
                    className: 'btn btn-light',
                    text: '<i class="icon-file-spreadsheet mr-2"></i> Excel',
                    extension: '.csv'
                }
            ]
        },
        initComplete: function () {
            $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
        },
        ajax: {
            url: "/Operation/GetAllDeployLocation_Paging/",
            type: 'POST',
            data: function (d) {
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.WardId = WardId;
                d.Status = Status;
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
           
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return "<a cid='" + row.DLId + "'   href='javascript:void(0);' title='edit' onclick='CallFunc(this);'><i class='ti-pencil'></i></a>";

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.IsActive == 1)
                        return '<span class="badge badge-success">ACTIVE</span>';
                    else
                        return '<span class="badge badge-danger">DE-ACTIVE</span>';

                }
            },


            { data: "LandMark", sortable: true },
            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "WardNo", sortable: true },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a data-GeoPointName='" + row.LandMark +
                      
                        "'data-Lat='" + row.Lat +
                        "'data-Lng='" + row.Lng +
                        "'data-GeoPointId='" + row.DLId +
                        "'href=javascript:void(0)  onclick=ShowMapPopup(this);  >" + row.Lat + " | " + row.Lng + "</a>";


                }
            },
            { data: "Radius", sortable: true },
            
           
            { data: "CreatedBy", sortable: true },
            { data: "ModifiedOn", sortable: true },
            

        ]
    });

}

function CallAllFunc(Type) {
    GetDataTableData(Type);
    GetGeopointDataForMap(Type);
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


    contentString = "<div style='float:right; padding: 10px;font-size: 14px;background-color: #33414E;color: white;'>LandMark-" + GeoPointName +
        //"<br/>Category-" + GeoPointCategory +
        //"<br/>Location-" + Location +
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

function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'Select', $('#ddlZone').find(":selected").attr('value'));
}
function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'Select', $('#ddlCircle').find(":selected").attr('value'));
}

function CallSCircleByZone() {
    $('#ddlSCircle').val('0');
    AllMCircleLst('ddlSCircle', 0, 'All Circle', $('#ddlSZone').find(":selected").attr('value'));
}
function CallSWardByCircle() {
    AllMWardLst('ddlSWard', 0, 'All Ward', $('#ddlSCircle').find(":selected").attr('value'));
}

function GetGeopointDataForMap(Type) {

    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    var PointTypeId = '-1';
    var Status = '-1';

    if (Type == 'Click') {
        ZoneId = $('#ddlSZone').val();
        CircleId = $('#ddlSCircle').val();
        WardId = $('#ddlSWard').val();
        PointTypeId = $('#ddlPointToRoute').val();
        Status = $('#ddlStatus').val();
    }

    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: '/Collection/AllGeoPointsMapInfo',
        dataType: "json",
        data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, NotiId: PointTypeId, Status: Status },
        success: function (result) {

            var locations = JSON.parse(result);
            SetMapData(locations);
        },
        error: function (result) {
        }
    });
}
function SetMapData(locations) {

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
        var icon;

        //if (locations[i].CStatus == 1)
        icon = iconBase + "greenhome.png";


        contentString = '';
        contentString += '<table class="table table-bordered border-success">';
        contentString += '<thead>';
        contentString += '<tbody>';

        contentString += '<tr><td>Point Name:</td><td style="align:right;">' + locations[i].GeoPointName + '</td></tr>';
        contentString += '<tr><td>Category:</td><td style="align:right;">' + locations[i].GeoPointCategory + '</td></tr>';
        contentString += '<tr><td>Zone:</td><td style="align:right;">' + locations[i].ZoneNo + '</td></tr>';
        contentString += '<tr><td>Circle:</td><td style="align:right;">' + locations[i].CircleName + '</td></tr>';
        contentString += '<tr><td>Ward:</td><td style="align:right;">' + locations[i].WardNo + '</td></tr>';
        contentString += '<tr><td>Location:</td><td style="align:right;">' + locations[i].Location + '</td></tr>';

        contentString += '</tbody>';
        contentString += '</table>';



        marker = new google.maps.Marker({
            position: myLatlng,
            map: map,

            //icon: icon,
            content: contentString,

            icon: icon
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

function DownloadFile(FType) {
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    var PointTypeId = '-1';
    var Status = '-1';

    if (IsClick == 1) {
        ZoneId = $('#ddlSZone').val();
        CircleId = $('#ddlSCircle').val();
        WardId = $('#ddlSWard').val();
       /* PointTypeId = $('#ddlPointToRoute').val();*/
        Status = $('#ddlStatus').val();
    }

    var TName = "All Deploy Location Details";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportAllDeployLocation",
        data: { WardId: WardId, ZoneId: ZoneId, CircleId: CircleId, Status: Status, FName: TName },
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