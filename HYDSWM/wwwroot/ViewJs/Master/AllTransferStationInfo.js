var map1;
var circle;
var marker;
var geocoder;
var SData;

var currentlatlng = new google.maps.LatLng(17.4940964, 78.4000115);
function Formsubmit() {

    SaveAndUpdateTStationInfo();
    return false;
}
function CallFunc(obj,x) {
    // loadMap();
    if(x == 0)
        document.getElementById("add_upd").innerHTML = "Add Transfer Station";
    else
    document.getElementById('add_upd').innerHTML = "Update Transfer Station";

    document.getElementById('txtTSName').value = "";

    document.getElementById('ddlTSType').value = "";

    document.getElementById('txtNoOfContainer').value = "";


    document.getElementById('ddlZone').value = "";


    document.getElementById('ddlCircle').value = "";


    document.getElementById('ddlWard').value = "";

    document.getElementById('ddlCity').value = "";

    document.getElementById('txtLocation').value = "";

    document.getElementById('txtRadius').value = "";

    document.getElementById("ckbIsActive").checked = false;
    var ddId = $(obj).attr('cid');
    $('#modal_form_vertical').modal('toggle');


    setTimeout(function () {
        AllMCityLst('ddlCity', 1, 'Select');
    }, 1000);




    if (ddId > 0)
        SetDataOncontrols(ddId,x);



}
function CallCircleByZone() {
    $('#ddlSCircle').val('0');
    $('#ddlSWard').val('0');
    $('#ddlSWard').trigger("change.select2");
    AllMCircleLst('ddlSCircle', 0, 'All Circle', $('#ddlSZone').find(":selected").attr('value'));
}
function CallWardByCircle() {
    AllMWardLst('ddlSWard', 0, 'All Ward', $('#ddlSCircle').find(":selected").attr('value'));
}
function SetDataOncontrols(ddId,x) {
    $.ajax({
        type: "post",
        url: "/Master/GetTStationInfoById",
        data: { TSId: ddId },
        success: function (data) {
            var myJSON = JSON.parse(data);
            $("#hfTSId").val(myJSON.TSId);
            $("#txtTSName").val(myJSON.TStationName);
            $("#ddlTSType").val(myJSON.TSType);
            $("#txtNoOfContainer").val(myJSON.NoOfContainer);
            $("#ddlZone").val(myJSON.ZoneId).change();
            setTimeout(() => {
                $("#ddlCircle").val(myJSON.CircleId).change();

                setTimeout(() => {
                    $("#ddlWard").val(myJSON.WardId);
                }, 1000);

                setTimeout(() => {
                    $("#ddlCity").val(myJSON.CityId);
                }, 1000);

            }, 1000);

            $("#txtLat").val(myJSON.Lat);
            $("#txtLng").val(myJSON.Lng);
            $("#txtLocation").val(myJSON.Location);
            $("#txtRadius").val(myJSON.Radius);
            if (myJSON.IsActive == 'True')
                $("#ckbIsActive").prop("checked", true);
            else
                $("#ckbIsActive").prop("checked", false);

            if (myJSON.ImgUrl != '') {
                $('#downloadLink').remove();
                $("#files").after("<a id='downloadLink' href='" + myJSON.ImgUrl + "' download>Download File</a>");
            }
            currentlatlng = new google.maps.LatLng(myJSON.Lat, myJSON.Lng);
            setMarker();
            loadMap();
            //loadMap();
        }
    });
}
function SaveAndUpdateTStationInfo() {
    var isvalid = 1, IsRemarks = 1;
    var input = {
        TSId: $("#hfTSId").val(),
        TStationName: $("#txtTSName").val(),
        TSType: $("#ddlTSType").val(),
        NoOfContainer: $("#txtNoOfContainer").val(),
        ZoneId: $("#ddlZone").val(),
        WardId: $("#ddlWard").val(),
        CircleId: $("#ddlCircle").val(),
        Radius: $("#txtRadius").val(),
        Lat: $("#txtLat").val(),
        Lng: $("#txtLng").val(),
        Location: $("#txtLocation").val(),
        IsActive: $('#ckbIsActive').is(':checked'),
        UserId: $("#hfLoginId").val(),
        CityId: $("#ddlCity").val()

    };


    if (input.TStationName == '' || input.TSType == '' || input.NoOfContainer == '' || input.Radius == '' || input.Lat == '' || input.Lng == '' || input.Location == '' || input.ZoneId == '' || input.CircleId == '' || input.WardId == '' || input.CityId == '')
        isvalid = 0;
    var formData = new FormData();
    formData.append('file', $('#files')[0].files[0]);
    formData.append('input', JSON.stringify(input));
    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Master/SaveAndUpdateTStationInfo',
            // dataType: "json",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                var myJSON = JSON.parse(response);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg, '/Master/AllTransaferStation');

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
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 1, 'Select', $('#ddlZone').find(":selected").attr('value'));
}
function CallFWardByCircle() {
    AllMWardLst('ddlWard', 1, 'Select', $('#ddlCircle').find(":selected").attr('value'));
}
$(document).ready(function () {
    GetDataTableData('Load');
    loadMap();
    $("#txtRadius").on("propertychange change keyup paste input", function () {
        drawCircle();
    });
    AllMZoneLst('ddlSZone', 0, 'All Zone');
    AllMZoneLst('ddlZone', 1, 'Select');
});

var dt;
function GetDataTableData(Type) {
    var ZoneId = getUrlParameterInfo('ZId');
    var CircleId = getUrlParameterInfo('CId');
    var WardId = getUrlParameterInfo('WId');
    var CityId = getUrlParameterInfo('CityId');

    if (ZoneId != '') {
        document.getElementById("btnAdd").style.display = "none";
    }
    else {
        document.getElementById("btnAdd").style.display = "block";
    }

    if (Type == 'Click') {
        ZoneId = $('#ddlSZone').find(":selected").attr('value');
        WardId = $('#ddlSWard').find(":selected").attr('value');
        CircleId = $('#ddlSCircle').find(":selected").attr('value');
        CityId = $('#ddlSCity').find(":selected").attr('value');
    }


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
            url: "/Master/GetAllTransferStation/",
            type: 'POST',
            data: function (d) {
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.WardId = WardId;
                d.CityId = CityId;
            
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

                    return '<a class="gticket" cticketid="' + row.TSId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a data-TStationName='" + row.TStationName +
                        "'data-CircleName='" + row.CircleName +
                        "'data-Location='" + row.Location +
                        "'data-ZoneNo='" + row.ZoneNo +
                        "'data-WardNo='" + row.WardNo +
                        "'data-CityNo='" + row.CityNo +
                        "'data-CircleName='" + row.CircleName +
                        "'data-Radius='" + row.Radius +
                        "'data-Lat='" + row.Lat +
                        "'data-Lng='" + row.Lng +
                        "'href=javascript:void(0)  onclick=ShowMapPopup(this);  >" + row.Location + "</a>";


                }
            },
            { data: "TStationName", sortable: true },
            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "WardNo", sortable: true },
            { data: "CityNo", sortable: true },
            { data: "Radius", sortable: true },
            { data: "TSType", sortable: true },
            { data: "NoOfContainer", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.IsActive == 1)
                        return '<span class="badge badge-success">ACTIVE</span>';
                    else
                        return '<span class="badge badge-danger">DE-ACTIVE</span>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    // if (ZoneId == "") {
                    return "<a cid='" + row.TSId + "' href='javascript:void(0);' title='edit' onclick='CallFunc(this,1);'><i class='ti-pencil'></i></a>";
                    //}
                    //else {

                    //    return '';
                    //}
                }
            }
        ]
    });

}
function ShowMapPopup(objthis) {

    var lat = $(objthis).attr('data-Lat');
    var lng = $(objthis).attr('data-Lng');
    var TStationName = $(objthis).attr('data-TStationName');
    var Location = $(objthis).attr('data-Location');
    var WardNo = $(objthis).attr('data-WardNo');
    var ZoneNo = $(objthis).attr('data-ZoneNo');
    var CircleName = $(objthis).attr('data-CircleName');
    var Radius = $(objthis).attr('data-Radius');
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


    contentString = "<div style='float:right; padding: 10px;font-size: 14px;background-color: #33414E;color: white;'>Transfer Station Name-" + TStationName +
        "<br/>Location-" + Location +
        "<br/>Zone-" + ZoneNo +
        "<br/>Circle-" + CircleName +
        "<br/>Ward-" + WardNo +
        "<br/>Radius(In Meter)-" + Radius +
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
            // setLatLongValue();
            drawCircle();
        });
        drawCircle();
    }


}
function drawCircle() {

    if (circle != undefined)
        circle.setMap(null);

    var radius = 500;


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
