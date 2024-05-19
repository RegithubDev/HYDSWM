var map1;
var circle;
var marker;
var geocoder;
var SData;
var tablarr = [];
var currentlatlng = new google.maps.LatLng(17.4940964, 78.4000115);
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
    AllMZoneLst('ddlSZone', 0, 'Select');
    AllCategoryLst();
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
    
    loadMap();
    $("#txtRadius").on("propertychange change keyup paste input", function () {
        drawCircle();
    });
});
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
function Formsubmit1() {

    SaveAndUpdateGeoPointInfo();
    return false;
}
function SaveAndUpdateGeoPointInfo() {
    
    var isvalid = 1, IsRemarks = 1;
    var input = {
        GeoPointId: $("#hfGPId").val(),
        GeoPointName: $("#txtPointName").val(),
        CategoryId: $("#ddlCatgory").val(),
        Remarks: $("#txtRemarksSSS").val(),
        Radius: $("#txtRadius").val(),
        Lat: $("#txtLat").val(),
        Lng: $("#txtLng").val(),
        Location: $("#txtLocation").val(),
        IsActive: $('#ckbIsActive').is(':checked'),
        CreatedBy: $("#hfLoginId").val(),
        ZoneId: $("#ddlSZone").val(),
        CircleId: $("#ddlSCircle").val(),
        WardId: $("#ddlSWard").val(),
    };


    if (input.GeoPointName == '' || input.CategoryId == '' || input.Remarks == '' || input.Radius == '' || input.Lat == '' || input.Lng == '' || input.Location == '')
        isvalid = 0;
    var formData = new FormData();
    formData.append('file', $('#files')[0].files[0]);
    formData.append('input', JSON.stringify(input));
    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Collection/AddGeoLocationSurvey',
            // dataType: "json",
            data: formData,
            contentType: false,
            processData: false,
            success: function (response) {
                
                var myJSON = JSON.parse(response);
                if (myJSON.Result == 1 || myJSON.Result == 2) {
                    $('#smartwizard').smartWizard('next');
                    //$("#step1").attr("style", "display:none");
                    //$("#step2").attr("style", "display:block");
                    //document.getElementById("step1").style.display = "none";
                    //document.getElementById("step2").style.display = "block";

                   // $('#modal_form_vertical').modal('toggle');
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
function CallAllRouteByZoneAndWard() {
    $('#ddlRoute').val('0');
    AllMActiveRouteLst('ddlRoute', 0, 'All Route', $('#ddlZone').find(":selected").attr('value'), $('#ddlCircle').find(":selected").attr('value'));
}
function CallSCircleByZone() {
    $('#ddlSCircle').val('0');
    AllMCircleLst('ddlSCircle', 0, 'All Circle', $('#ddlSZone').find(":selected").attr('value'));
}
function CallSWardByCircle() {
    AllMWardLst('ddlSWard', 0, 'All Ward', $('#ddlSCircle').find(":selected").attr('value'));
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

function GetDataTableData(Type) {

    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var RouteId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var TripId = '0';
    if (Type == 'Click') {
        RouteId = $('#ddlRoute').val();
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        TripId = $('#ddlTrip').val();
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
            url: "/Collection/GetAllRouteWiseCollectionSummary_Paging/",
            type: 'POST',
            data: function (d) {
                d.FromDate = FDate;
                d.ToDate = TDate;
                d.Route = RouteId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.NotiId = TripId;
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
            { data: "SDate", sortable: true },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    if (row.Status == 'Completed')
                        return '<span class="badge badge-success">' + row.RouteBInfo + '</span>';
                    else if (row.Status == 'Not Started')
                        return '<span class="badge badge-danger">' + row.RouteBInfo + '</span>';
                    else if (row.Status == 'Started')
                        return '<span class="badge badge-warning">' + row.RouteBInfo + '</span>';
                    else if (row.Status == 'In Progress')
                        return '<span class="badge badge-info">' + row.RouteBInfo + '</span>';


                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.Status == 'Completed')
                        return '<span class="badge badge-success">' + row.Status + '</span>';
                    else if (row.Status == 'Not Started')
                        return '<span class="badge badge-danger">' + row.Status + '</span>';
                    else if (row.Status == 'Started')
                        return '<span class="badge badge-warning">' + row.Status + '</span>';
                    else if (row.Status == 'In Progress')
                        return '<span class="badge badge-info">' + row.Status + '</span>';


                }
            },

            { data: "RSTDate", sortable: false },
            { data: "TotalPoints", sortable: false },
            { data: "TotalCollectedPoints", sortable: false },
            { data: "NotCollectedPoints", sortable: false },
            { data: "TotalTimeTaken", sortable: false },
            { data: "TotalWeight", sortable: false },
            { data: "LastSyncOn", sortable: false }
        ]
    });


}
//$(document).on("click", "#btnSave", function (e) {
//    e.preventDefault();
//    // your code here
    
   
//})
function ShowCustomMessage1(typemsg, msg, url) {

    if (typemsg == '1') {
        swal({
            title: 'Good job!',
            text: msg,
            type: 'success'
        },
            function () {
                //window.location.href = url;
            });
    }
    else {
        swal({
            title: 'Oops...',
            text: msg,
            type: 'error'
        });
    }
}
document.querySelector('#btnSave').addEventListener('click', function (event) {
    event.preventDefault();
    event.stopPropagation();
    var pointid = $("#ddlGreyPoint").val();
    if (pointid > 0) {
        $.ajax({
            type: "post",
            url: "/Collection/SaveActualGreyPoint",
            data: { PointId: $("#ddlGreyPoint").val(), PointName: $("#ddlGreyPoint option:selected").text(), PClId: PClId },
            datatype: "json",
            traditional: true,
            success: function (data) {
                
                ShowCustomMessage1('1', "Grey Point added successfully", '');
                //ShowCustomMessage('1', "Grey Point added successfully", '/Collection/AllRouteWiseCollectDetail');

                //$('#modal_form_vertical').modal('toggle');
                CloseModal();
                CallAllFunction('Click');
                return false;
            }
        });
    }
    else {
        ShowCustomMessage1('0', 'Please Enter All Required Details', '');
    }
});
$(document).on("click", "#btnRemove", function () {
    
    $.ajax({
        type: "post",
        url: "/Collection/DeletegreypointintoActual",
        data: { PClId: PClId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            
            ShowCustomMessage('1', "Gray Point Remove Sucessfully", '/Collection/AllRouteWiseCollectDetail');

            $('#modal_form_vertical').modal('toggle');

        }
    });

})
var PClId = 0;
var SLat = '';
var SLng = '';
var SRouteId = '';
function ShowTextOnHoverGrayPoint(objthis) {
    
    var SPickupTime = $(objthis).attr('data-SPickupTime');
    var SchedulePickupTime = $(objthis).attr('data-SchedulePickupTime');
    var TripName = $(objthis).attr('data-TripName');
    var GeoPointName = $(objthis).attr('data-SGeoPointName');
    var Remarks1 = $(objthis).attr('data-SRemarks');
    var BeforeImage = $(objthis).attr('data-SBeforeImage');
    var AfterImage = $(objthis).attr('data-SAfterImage');
    var PointCatIdS = $(objthis).attr('data-PointCatId');
    var ColorIdS = $(objthis).attr('data-ColorId');
    var SDate = $(objthis).attr('data-STDate');//Add Parameter
    var TripName = $(objthis).attr('data-TripName');//Add Parameter
    var SAfterDate = $(objthis).attr('data-SAfterDate');//Add Parameter
     SLat = $(objthis).attr('data-SLat');//Add Parameter
     SLng = $(objthis).attr('data-SLng');//Add Parameter
     SRouteId = $(objthis).attr('data-SRouteId');//Add Parameter

    

    PClId = $(objthis).attr('data-PClId');//Add Parameter
     arr = [];
    if (SDate != undefined) {
        
        arr = SDate.split('T')
    }
   

    if (SPickupTime == 'N/A' || SPickupTime == '0')
        SPickupTime = 'Not Visited';

    $('#modal_form_Option').modal('toggle');

   // $('#modal_form_Greypoint').modal('toggle');

    $("#mainheadingtag1").text(GeoPointName);
    $("#txtSdlPickup1").text(SchedulePickupTime);
    $("#txtActpickup1").text(SPickupTime);
    $("#txtRouteCode1").text(TripName);
    $("#txtPointName1").text(GeoPointName);
    $("#txtRemarks1").text(Remarks1);
    $('#ImgAfter1').append('');
    $('#ImgBefore1').append('');
    $("#dvBeforeDate1").text(SAfterDate);
    $("#dvAfterDate1").text(arr[0] + ' ' + SPickupTime);


    if ($(objthis).attr('data-SBeforeImage') != undefined) {
        $("#ImgAfter1").attr("src", AfterImage);
        $("#ImgBefore1").attr("src", BeforeImage);
        $("#hrfBeforeImage1").attr("href", BeforeImage)
        $("#hrfAfterImage1").attr("href", AfterImage)

    }
    else {
        //   $('<img />', { src: 'test.png', width: '200px', height: '100px' }).appendTo($('#ImgAfter').empty())
        $('#ImgAfter1').empty().append('<img id="imageresource" src="' + AfterImage + '" alt="" class="img-preview rounded">');
        $('#ImgBefore1').empty().append('<img id="imageresource" src="' + BeforeImage + '" alt="" class="img-preview rounded">');
    }

    //$("#ddlGreyPoint").select2({
    //    dropdownParent: $("#frmGPoint")
    //});
    var IsRequired = 0;
    var Category = "Select Actual Point";
    $.ajax({
        type: "post",
        url: "/Master/GetAllGreyPoint",
        data: { RouteTripCode: TripName, SDate: document.getElementById('txtFromDate').value },
        datatype: "json",
        traditional: true,
        success: function (data) {

            var Myjson = JSON.parse(data);
            var Resource = "<select id='ddlGreyPoint' class='form-control'>";
            //$("#ddlGreyPoint").select2({

            //})
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select Actual Point</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            //var Myjson = JSON.parse(data);
            //var Resource = "<select id='ddlGreyPoint' class='form-control'>";
            //Resource = Resource + '<option value="">Select Actual Point</option>';

            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].StopId + '>' + Myjson[i].GeoPointName + '</option>';
            }
            Resource = Resource + '</select>';
            $("#ddlGreyPoint").html(Resource);

        }
    });
}

function ShowTextOnHover(objthis) {
    
    var SPickupTime = $(objthis).attr('data-SPickupTime');
    var SchedulePickupTime = $(objthis).attr('data-SchedulePickupTime');
    var TripName = $(objthis).attr('data-TripName');
    var GeoPointName = $(objthis).attr('data-SGeoPointName');
    var Remarks1 = $(objthis).attr('data-SRemarks');
    var BeforeImage = $(objthis).attr('data-SBeforeImage');
    var AfterImage = $(objthis).attr('data-SAfterImage');
    var PointCatIdS = $(objthis).attr('data-PointCatId');
    var ColorIdS = $(objthis).attr('data-ColorId');
    var SDate = $(objthis).attr('data-SDate');//Add Parameter
    var TripName = $(objthis).attr('data-TripName');//Add Parameter
    var SAfterDate = $(objthis).attr('data-SAfterDate');//Add Parameter

    arr = [];
    if (SDate != undefined) {

        arr = SDate.split('T')
    }

    if (SPickupTime == 'N/A' || SPickupTime == '0')
        SPickupTime = 'Not Visited';
    var Msg = ' Route Trip Code-' + TripName + '\n' + 'Schedule Pickup Time-' + SchedulePickupTime + '\n' + ' Actual Pickup Time-' + SPickupTime;
    $('#modal_form_vertical').modal('toggle');

    if (PointCatIdS == 5) {
        $("#mainheadingtag").text(GeoPointName);
        $("#divbeforeimage").hide();
        $("#divafterimage").hide();
        $("#divremarks").hide();
    }
    else if (PointCatIdS == 6) {
        $("#mainheadingtag").text(GeoPointName);
        $("#divbeforeimage").hide();
        $("#divafterimage").hide();
        $("#divremarks").hide();
    }
    else {
        if (ColorIdS == 2) {
            $("#mainheadingtag").text(GeoPointName);
            $("#divbeforeimage").show();
            $("#divafterimage").show();
            $("#divremarks").show();

            $("#dvBeforeDate").text(SAfterDate);
            $("#dvAfterDate").text(arr[0] + ' ' + SPickupTime);

        }
        else if (ColorIdS == 1) {
            $("#mainheadingtag").text(GeoPointName);
            $("#divbeforeimage").show();
            $("#divafterimage").show();
            $("#divremarks").show();

            $("#dvBeforeDate").text(SAfterDate);
            $("#dvAfterDate").text(arr[0] + ' ' + SPickupTime);

        }
        else {
            $("#mainheadingtag").text(GeoPointName);
            $("#divbeforeimage").hide();
            $("#divafterimage").hide();
            $("#divremarks").hide();
        }
    }

    $("#txtSdlPickup").text(SchedulePickupTime);
    $("#txtActpickup").text(SPickupTime);
    $("#txtRouteCode").text(TripName);
    $("#txtPointName").text(GeoPointName);
    $("#txtRemarks").text(Remarks1);
    $('#ImgAfter').append('');
    $('#ImgBefore').append('');
    //'<img id="imageresource" src="' + row.Img2Url + '" alt="" class="img-preview rounded">'
    if ($(objthis).attr('data-SBeforeImage') != undefined) {
        $("#ImgAfter").attr("src", AfterImage);
        $("#ImgBefore").attr("src", BeforeImage);
        $("#hrfBeforeImage").attr("href", BeforeImage)
        $("#hrfAfterImage").attr("href", AfterImage)

    }
    else {
        //   $('<img />', { src: 'test.png', width: '200px', height: '100px' }).appendTo($('#ImgAfter').empty())
        $('#ImgAfter').empty().append('<img id="imageresource" src="' + AfterImage + '" alt="" class="img-preview rounded">');
        $('#ImgBefore').empty().append('<img id="imageresource" src="' + BeforeImage + '" alt="" class="img-preview rounded">');
    }




    // ShowGreenMessage('1', Msg, '');
}

function OldShowTextOnHover(objthis) {
    var SPickupTime = $(objthis).attr('data-SPickupTime');
    var SchedulePickupTime = $(objthis).attr('data-SchedulePickupTime');
    var TripName = $(objthis).attr('data-TripName');
    if (SPickupTime == 'N/A')
        SPickupTime = 'Not Visited';
    var Msg = ' Route Trip Code-' + TripName + '\n' + 'Schedule Pickup Time-' + SchedulePickupTime + '\n' + ' Actual Pickup Time-' + SPickupTime;
    ShowGreenMessage('1', Msg, '');
}
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
    var RouteId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var TripId = '0';
    var PageSize = 10;
    if (Type == 'Click') {
        RouteId = $('#ddlRoute').val();
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        TripId = $('#ddlTrip').val();
        PageSize = $('#ddlMap').val();
    }
    ShowLoading($('#dvTimeline'));
    $.ajax({
        type: "post",
        url: "/Collection/GetAllPointForTimeline",
        data: { FromDate: FDate, ToDate: TDate, RouteId: RouteId, ZoneId: ZoneId, CircleId: CircleId, TripId: TripId, PageSize: PageSize },
        datatype: "json",
        success: function (data) {
            var locations = data;
            
            HideLoading($('#dvTimeline'));
            SetMapData(data);
            var content = '', i, j;

            for (i = 0; i < locations.length; i++) {

                if (locations[i].SRouteTripInfo != null) {
                    var inid = "flush-heading" + i;
                    var iflushid = "#flush-collapse" + i;
                    var iflushcolid = "flush-collapse" + i;
                    var icontent = ''
                    content += '<div class="accordion-item">';
                    content += '<h2 class="accordion-header" id=' + inid + '><button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target=' + iflushid + ' aria-expanded="true" aria-controls=' + iflushcolid + ' > [Date]: ' + locations[i].STDate + '&nbsp;&nbsp;&nbsp;Route Trip Code # ' + locations[i].TripName + '&nbsp;&nbsp;&nbsp;Vehicle No: ' + locations[i].RouteName + '&nbsp;&nbsp;&nbsp;Points: ' + locations[i].TotalCollectedPoints + '/' + locations[i].TotalPoints + '</button ></h2 >';
                    // content += '<h5>Route-' + locations[i].RouteCode + '     Route Trip Code-' + locations[i].TripName + '       Points-' + locations[i].TotalCollectedPoints + '/' + locations[i].TotalPoints + '    Dated On-' + locations[i].STDate + '</h5>';
                    content += '<div id=' + iflushcolid + ' class="accordion-collapse show" aria-labelledby=' + inid + ' data-bs-parent="#dvTimeline">';
                    content += '<div class="accordion-body" style="background-color: white;">';
                    content += '<div class="path-container">';
                    content += '<div class="path-content" style="background-color: white;">';

                    for (j = 0; j < locations[i].SRouteTripInfo.length; j++) {
                        content += '<div class="path">';

                        if (locations[i].SRouteTripInfo[j].PointCatId == 5) {
                            content += '<span class="up-text">' + locations[i].SRouteTripInfo[j].SchPickupTime + '</span>';//locations[i].SRouteTripInfo[j].PointCatId
                            if (locations[i].SRouteTripInfo[j].SPickupTime != 'N/A') {
                                content += ' <div class="start-point" style="background-color:#4caf50;" data-SRemarks="' + locations[i].SRouteTripInfo[j].Remarks + '" data-SAfterImage="' + locations[i].SRouteTripInfo[j].AfterImage + '" data-SBeforeImage="' + locations[i].SRouteTripInfo[j].BeforeImage + '"  data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + '  data-PointCatId=' + locations[i].SRouteTripInfo[j].PointCatId + '  data-ColorId=' + locations[i].SRouteTripInfo[j].ColorId + ' data-SGeoPointName="' + locations[i].SRouteTripInfo[j].PointName + '"  onclick="ShowTextOnHover(this); " title="Click To View Detail">S</div>';
                                content += '   <span class="down-text">' + locations[i].SRouteTripInfo[j].SPickupTime + '</span>';
                            }
                            else
                                content += ' <div class="start-point" style="background-color: black;" data-SRemarks="' + locations[i].SRouteTripInfo[j].Remarks + '" data-SAfterImage="' + locations[i].SRouteTripInfo[j].AfterImage + '" data-SBeforeImage="' + locations[i].SRouteTripInfo[j].BeforeImage + '" data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' data-PointCatId=' + locations[i].SRouteTripInfo[j].PointCatId + '  data-ColorId=' + locations[i].SRouteTripInfo[j].ColorId + '  data-SGeoPointName=' + locations[i].SRouteTripInfo[j].PointName + '  onclick="ShowTextOnHover(this); " title="Click To View Detail">S</div>';

                            //  content += '<li data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' class="st" onclick="ShowTextOnHover(this); " title="Click To View Detail" style="background:black;">S</li>';
                        }
                        else if (locations[i].SRouteTripInfo[j].PointCatId == 6) {
                            content += '<span class="up-text">' + locations[i].SRouteTripInfo[j].SchPickupTime + '</span>';
                            if (locations[i].SRouteTripInfo[j].SPickupTime != 'N/A') {
                                content += ' <div class="start-point" style="background-color:#4caf50;" data-SRemarks="' + locations[i].SRouteTripInfo[j].Remarks + '" data-SAfterImage="' + locations[i].SRouteTripInfo[j].AfterImage + '" data-SBeforeImage="' + locations[i].SRouteTripInfo[j].BeforeImage + '" data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + '  data-PointCatId=' + locations[i].SRouteTripInfo[j].PointCatId + '  data-ColorId=' + locations[i].SRouteTripInfo[j].ColorId + '  data-SGeoPointName="' + locations[i].SRouteTripInfo[j].PointName + '"  onclick="ShowTextOnHover(this); " title="Click To View Detail">E</div>';
                                content += '   <span class="down-text">' + locations[i].SRouteTripInfo[j].SPickupTime + '</span>';
                            }
                            else
                                content += ' <div class="end-point" style="background-color: black;" data-SRemarks=' + locations[i].SRouteTripInfo[j].Remarks + ' data-SAfterImage=' + locations[i].SRouteTripInfo[j].AfterImage + ' data-SBeforeImage=' + locations[i].SRouteTripInfo[j].BeforeImage + ' data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + '  data-PointCatId=' + locations[i].SRouteTripInfo[j].PointCatId + '  data-ColorId=' + locations[i].SRouteTripInfo[j].ColorId + '   data-SGeoPointName=' + locations[i].SRouteTripInfo[j].PointName + '  onclick="ShowTextOnHover(this); " title="Click To View Detail">E</div>';

                            //content += '<li data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' class="st" onclick="ShowTextOnHover(this); " title="Click To View Detail" style="background:black;">E</li>';
                        }
                        else {
                            
                            //locations[i].SRouteTripInfo[j].AfterImage
                            if (locations[i].SRouteTripInfo[j].ColorId == 1) {
                                content += '<span class="up-text">' + locations[i].SRouteTripInfo[j].SchPickupTime + '</span>';
                                content += '<div class="path-item" style="--clr-bg:#777"  data-SGeoPointName="' + locations[i].SRouteTripInfo[j].PointName + '" data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' data-SAfterImage="' + locations[i].SRouteTripInfo[j].AfterImage + '" data-SBeforeImage="' + locations[i].SRouteTripInfo[j].BeforeImage + '"  data-PointCatId=' + locations[i].SRouteTripInfo[j].PointCatId + ' data-ColorId=' + locations[i].SRouteTripInfo[j].ColorId + '  data-SRemarks="' + locations[i].SRouteTripInfo[j].Remarks + '"   data-STDate="' + locations[i].TDate + '"  data-TripName="' + locations[i].TripName + '"  data-PClId="' + locations[i].SRouteTripInfo[j].PClIdSS + '" data-SAfterDate="' + locations[i].SRouteTripInfo[j].SAfterDate + '" data-SLat="' + locations[i].SRouteTripInfo[j].Lat + '" data-SLng="' + locations[i].SRouteTripInfo[j].Lng + '"  data-SRouteId="' + locations[i].SRouteTripInfo[j].RouteId + '" onclick="ShowTextOnHoverGrayPoint(this); " title="Click To View Detail">' + locations[i].SRouteTripInfo[j].RowNumber + '</div>';
                                content += '<span class="down-text">' + locations[i].SRouteTripInfo[j].StopId + '</span>';
                                if (locations[i].SRouteTripInfo[j].SPickupTime != 'N/A')
                                    content += '<span class="down-text other-down-text">' + locations[i].SRouteTripInfo[j].SPickupTime + '</span>';

                                //content += '<li data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' class="st" onclick="ShowTextOnHover(this); " title="Click To View Detail" style="background:#777 !important;">' + locations[i].SRouteTripInfo[j].RowNumber + '</li>';
                            }
                            else if (locations[i].SRouteTripInfo[j].ColorId == 2) {
                                content += '<span class="up-text">' + locations[i].SRouteTripInfo[j].SchPickupTime + '</span>';
                                content += '<div class="path-item" style="--clr-bg:#4caf50"   data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' data-SAfterImage="' + locations[i].SRouteTripInfo[j].AfterImage + '" data-SBeforeImage="' + locations[i].SRouteTripInfo[j].BeforeImage + '" data-PointCatId=' + locations[i].SRouteTripInfo[j].PointCatId + ' data-ColorId=' + locations[i].SRouteTripInfo[j].ColorId + ' data-SGeoPointName="' + locations[i].SRouteTripInfo[j].PointName + '"  data-SRemarks="' + locations[i].SRouteTripInfo[j].Remarks + '" data-SDate= "' + locations[i].TDate + '"  data-SAfterDate="' + locations[i].SRouteTripInfo[j].SAfterDate + '"  onclick="ShowTextOnHover(this); " title="Click To View Detail">' + locations[i].SRouteTripInfo[j].RowNumber + '</div>';
                                content += '<span class="down-text">' + locations[i].SRouteTripInfo[j].StopId + '</span>';
                                if (locations[i].SRouteTripInfo[j].SPickupTime != 'N/A')
                                    content += '<span class="down-text other-down-text">' + locations[i].SRouteTripInfo[j].SPickupTime + '</span>';

                                //content += '<li data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' class="st" onclick="ShowTextOnHover(this);" title="Click To View Detail" style="background:#4caf50 !important;">' + locations[i].SRouteTripInfo[j].RowNumber + '</li>';
                            }
                            else if (locations[i].SRouteTripInfo[j].ColorId == 3) {
                                content += '<span class="up-text">' + locations[i].SRouteTripInfo[j].SchPickupTime + '</span>';
                                content += '<div class="path-item" style="--clr-bg:#ee1809" data-SGeoPointName="' + locations[i].SRouteTripInfo[j].PointName + '" data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + '  data-SRemarks="' + locations[i].SRouteTripInfo[j].Remarks + '" data-SAfterImage="' + locations[i].SRouteTripInfo[j].AfterImage + '" data-SBeforeImage="' + locations[i].SRouteTripInfo[j].BeforeImage + '" data-PointCatId=' + locations[i].SRouteTripInfo[j].PointCatId + ' data-ColorId=' + locations[i].SRouteTripInfo[j].ColorId + ' onclick="ShowTextOnHover(this); " title="Click To View Detail">' + locations[i].SRouteTripInfo[j].RowNumber + '</div>';
                                content += '<span class="down-text">' + locations[i].SRouteTripInfo[j].StopId + '</span>';
                                if (locations[i].SRouteTripInfo[j].SPickupTime != 'N/A')
                                    content += '<span class="down-text other-down-text">' + locations[i].SRouteTripInfo[j].SPickupTime + '</span>';

                                //content += '<li data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' class="st" onclick="ShowTextOnHover(this);" title="Click To View Detail" style="background:#f44336 !important;">' + locations[i].SRouteTripInfo[j].RowNumber + '</li>';
                            }
                            else if (locations[i].SRouteTripInfo[j].ColorId == 4) {
                                content += '<span class="up-text">' + locations[i].SRouteTripInfo[j].SchPickupTime + '</span>';
                                content += '<div class="path-item" style="--clr-bg:#ee1809" data-SGeoPointName="' + locations[i].SRouteTripInfo[j].PointName + '" data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + '  data-SRemarks="' + locations[i].SRouteTripInfo[j].Remarks + '" data-SAfterImage="' + locations[i].SRouteTripInfo[j].AfterImage + '" data-SBeforeImage="' + locations[i].SRouteTripInfo[j].BeforeImage + '" data-PointCatId=' + locations[i].SRouteTripInfo[j].PointCatId + ' data-ColorId=' + locations[i].SRouteTripInfo[j].ColorId + ' onclick="ShowTextOnHover(this); " title="Click To View Detail">' + locations[i].SRouteTripInfo[j].RowNumber + '</div>';
                                content += '<span class="down-text">' + locations[i].SRouteTripInfo[j].StopId + '</span>';
                                if (locations[i].SRouteTripInfo[j].SPickupTime != 'N/A')
                                    content += '<span class="down-text other-down-text">' + locations[i].SRouteTripInfo[j].SPickupTime + '</span>';

                                //content += '<li data-SPickupTime=' + locations[i].SRouteTripInfo[j].SPickupTime + ' data-SchedulePickupTime=' + locations[i].SRouteTripInfo[j].SchPickupTime + ' data-TripName=' + locations[i].SRouteTripInfo[j].TripName + ' class="st" onclick="ShowTextOnHover(this);" title="Click To View Detail" style="background:#f44336 !important;">' + locations[i].SRouteTripInfo[j].RowNumber + '</li>';
                            }
                        }
                        content += '</div>';
                    }


                    content += '</div>';
                    content += '</div>';
                    content += '</div>';
                    content += '</div>';
                    content += '</div>';
                    content += '</div>';

                }

            }
            $("#dvTimeline").html(content);

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

                contentString += '<tr><td>Route Code:</td><td style="align:right;">' + locations[i].RouteCode + '</td></tr>';
                contentString += '<tr><td>Vehicle No:</td><td style="align:right;">' + locations[i].SRouteTripInfo[j].VehicleNo + '</td></tr>';
                contentString += '<tr><td>Route Trip Code:</td><td style="align:right;">' + locations[i].TripName + '</td></tr>';
                contentString += '<tr><td>Point Name:</td><td style="align:right;">' + locations[i].SRouteTripInfo[j].GeoPointName + '</td></tr>';
                contentString += '<tr><td>Schedule Pickup Time:</td><td style="align:right;">' + locations[i].SRouteTripInfo[j].SchPickupTime + '</td></tr>';
                contentString += '<tr><td>Actual Pickup Time:</td><td style="align:right;">' + cstatus + '</td></tr>';
                contentString += '<tr><td>Status:</td><td style="align:right;">' + locations[i].SRouteTripInfo[j].Status + '</td></tr>';

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
    GetDataTableData(Type);
    AllPointTimelineInfo(Type);
}

$(document).on("click", "#btngrayPoint", function () {
    CloseModal();
    $('#modal_form_Greypoint').modal('toggle');
})
$(document).on("click", "#btnWizard", function () {
    
    CloseModal();
    //$(".wizard-form steps-validation").steps({
    //    bodyTag: "fieldset",
    //});
    $('#smartwizard').smartWizard({
        selected: 0, // Initial selected step, 0 = first step
        theme: 'basic', // theme for the wizard, related css need to include for other than basic theme
        justified: true, // Nav menu justification. true/false
        autoAdjustHeight: true, // Automatically adjust content height
        backButtonSupport: true, // Enable the back button support
        enableUrlHash: true, // Enable selection of the step based on url hash
        transition: {
            animation: 'none', // Animation effect on navigation, none|fade|slideHorizontal|slideVertical|slideSwing|css(Animation CSS class also need to specify)
            speed: '400', // Animation speed. Not used if animation is 'css'
            easing: '', // Animation easing. Not supported without a jQuery easing plugin. Not used if animation is 'css'
            prefixCss: '', // Only used if animation is 'css'. Animation CSS prefix
            fwdShowCss: '', // Only used if animation is 'css'. Step show Animation CSS on forward direction
            fwdHideCss: '', // Only used if animation is 'css'. Step hide Animation CSS on forward direction
            bckShowCss: '', // Only used if animation is 'css'. Step show Animation CSS on backward direction
            bckHideCss: '', // Only used if animation is 'css'. Step hide Animation CSS on backward direction
        },
        toolbar: {
            position: 'bottom', // none|top|bottom|both
            showNextButton: true, // show/hide a Next button
            showPreviousButton: true, // show/hide a Previous button
            extraHtml: '' // Extra html to show on toolbar
        },
        anchor: {
            enableNavigation: true, // Enable/Disable anchor navigation 
            enableNavigationAlways: false, // Activates all anchors clickable always
            enableDoneState: true, // Add done state on visited steps
            markPreviousStepsAsDone: true, // When a step selected by url hash, all previous steps are marked done
            unDoneOnBackNavigation: false, // While navigate back, done state will be cleared
            enableDoneStateNavigation: true // Enable/Disable the done state navigation
        },
        keyboard: {
            keyNavigation: true, // Enable/Disable keyboard navigation(left and right keys are used if enabled)
            keyLeft: [37], // Left key code
            keyRight: [39] // Right key code
        },
        lang: { // Language variables for button
            next: 'Next',
            previous: 'Previous'
        },
        disabledSteps: [], // Array Steps disabled
        errorSteps: [], // Array Steps error
        warningSteps: [], // Array Steps warning
        hiddenSteps: [], // Hidden steps
        getContent: null // Callback function for content loading
    });
    $('#txtLat').val(SLat);
    $('#txtLng').val(SLng);
    FillRouteTripData();
})
function FillRouteTripData() {
    
    $.ajax({
        type: "post",
        url: "/Collection/GetAllRouteTripByRoute",
        data: { RouteId: SRouteId },
        success: function (data) {

            var myJSONData = JSON.parse(data);
            var nstopdata = myJSONData.Table[0];
            var myJSON = myJSONData.Table1;
            //  var myJSON = JSON.parse(data);
            $("#hfRouteId").val(SRouteId);
            //setTimeout(() => {
            //    $("#txtRouteCode").val(nstopdata.RouteCode).trigger('change');

            //}, 2000);

            $("#txtRouteCodeSSS").val(nstopdata.RouteCode);
            $("#txtZone").val(nstopdata.ZoneNo);
            $("#txtCircle").val(nstopdata.CircleName);


            for (var i = 0; i < myJSON.length; i++) {

                var data = {};
                data.TripId = myJSON[i].TripId;
                data.VehicleUId = myJSON[i].VehicleUId;
                data.TId = myJSON[i].TId;
                data.BufferMin = myJSON[i].BufferMin;
                tablarr.push(data);

                var tr = '<tr  data-tripid=' + myJSON[i].TripId + ' data-vuid="' + myJSON[i].VehicleUId + '"><td >' + myJSON[i].TId +
                    /* '</td><td>' + myJSON[i].VehicleNo +*/
                    '</td><td>' + myJSON[i].BufferMin +
                    '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveTripRow(this);"> <i class="icon-close2"></i> Remove</button>'
                '</td></tr>';
                $('#tblTrip > tbody').append(tr);
            }
            ChangeIdText();

        }
    });
}
function ChangeIdText() {
    //var Tripname = $("#ddlMTrip option:selected").text();

    var RouteCode = $('#txtRouteCodeSSS').val();
    var IICount = tablarr.length + 1;

    var Id = RouteCode + "-T" + IICount;
    $('#txtTId').val(Id);

}

function RemoveTripRow(objthis) {
    var tripid = $(objthis).closest('tr').attr('data-tripid');

    tablarr = tablarr.filter(function (obj) {
        return parseInt(obj.TripId) !== parseInt(tripid);
    });

    $("#tblTrip > tbody").html("");

    var IICount = 1;
    var Itablarr = [];
    for (var i = 0; i < tablarr.length; i++) {

        var RouteCode = $('#txtRouteCodeSSS').val();
        var Id = RouteCode + "-T" + IICount;

        var data = {};
        data.TripId = IICount;
        data.VehicleUId = tablarr[i].VehicleUId;//Vehicle Number
        data.TId = Id;
        data.BufferMin = tablarr[i].BufferMin;
        Itablarr.push(data);


        var tr = '<tr  data-tripid="' + IICount + '" data-vuid="' + tablarr[i].VehicleUId + '"><td>' + Id +
            /* '</td><td>' + TId +*/
            /*  '</td><td>' + VehicleNo +*/
            '</td><td>' + tablarr[i].BufferMin +
            '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveTripRow(this);"> <i class="icon-cancel-circle2"></i> Remove</button>'
        '</td></tr>';
        $('#tblTrip > tbody').append(tr);
        IICount++;
    }
    tablarr = [];
    tablarr = Itablarr;
    $(objthis).closest('tr').remove();
    ChangeIdText();
}
function AddTrips() {
    var IsValid = 1;
    // var Tripname = $("#ddlMTrip option:selected").text();
    //var TripId = $('#ddlMTrip').val();
    // var VehicleNo = $("#ddlVehicleNo option:selected").text();
    var VUId = "";//$('#ddlVehicleNo').val();
    var BufferMin = $('#txtBufferMin').val();
    var TId = $('#txtTId').val();
    var TripId = TId.split('-')[3].replace('T', '');;


    if (BufferMin == '' || TId == '')
        IsValid = 0

    if (IsValid == 1) {
        var IsExist = false;
        $('#tblTrip > tbody > tr').each(function (i, el) {
            var $tds = $(this).find('td');
            // var ITripId = $(this).attr('data-tripid');
            var ITId = $tds.eq(1).text();
            if (ITId == TId)
                IsExist = true;

        });

        if (IsExist == false) {

            var data = {};
            data.TripId = TripId;
            data.VehicleUId = VUId;
            data.TId = TId;
            data.BufferMin = BufferMin;
            tablarr.push(data);

            var tr = '<tr  data-tripid="' + TripId + '" data-vuid="' + VUId + '"><td>' + TId +
                /* '</td><td>' + TId +*/
                /*  '</td><td>' + VehicleNo +*/
                '</td><td>' + BufferMin +
                '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveTripRow(this);"> <i class="icon-cancel-circle2"></i> Remove</button>'
            '</td></tr>';
            $('#tblTrip > tbody').append(tr);

            ChangeIdText();
        }
        else
            ShowCustomMessage('0', 'This Route Trip Code is already exists', '');
    }
    else
        ShowCustomMessage('0', 'Please Fill All Required Details', '');

}