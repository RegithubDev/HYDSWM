var map1;
var circle;
var marker;
var geocoder;
var currentlatlng = new google.maps.LatLng(17.4940964, 78.4000115);
var SData;
$(document).ready(function () {
    $('#txtFromDate').datetimepicker({
        changeMonth: true,
        changeYear: true,
        //maxDate: '0',
        yearRange: "-100:+20",
        minDate: '0'
    });
    //$('#txtToDate').datetimepicker({
    //    changeMonth: true,
    //    changeYear: true,
    //    //maxDate: '0',
    //    //yearRange: "-100:+20",
    //   // minDate: '0'
    //});
    var date = new Date();
    document.getElementById("txtFromDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear() + ' ' + date.getHours() + ':' + date.getMinutes();
   
    GetPointsNameByWardId();
    GetDataTableData();
    //$("#dvPoint").hide();
    //$("#dvPointName").hide();
    //$("#dvtxtPointName").hide();
    $("#dvtxtPointName").show();
    $("#dvPoint").show();
    $("#divcategory").hide();
    var DDLIdA = 0;
    var splitUrl = window.location.href;
    var cids = splitUrl.split('=');
    DDLIdA = cids[1];
    if (DDLIdA != 0) {
        GetvehicleDataById(DDLIdA)
    }

    //  GetPointsName('ddlPointName', 0, 'All PointName');
    AllMZoneLst('ddlZone', 0, 'All Zone');
    AllAssignedVehicleLst('ddlVehicleNumber', 0, 'All VehicleNumber');
    loadMap();
    $("#txtRadius").on("propertychange change keyup paste input", function () {
        drawCircle();
    });
    AllCategoryLst();
    
    
});

function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}

function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'Select', $('#ddlCircle').find(":selected").attr('value'));
}

var dt;
function GetDataTableData() {
    
    var CId = getUrlParameterInfo('cid');
    var requestModel = {
        VehicleUid: CId
    };
    var countrow = 0;
    $.ajax({
        type: "post",
        
        url: "/Collection/GetEmergencyPointsInfo",
        data: requestModel,
       
        datatype: "json", //GeoPointId, GeoPointName
        traditional: true,
        success: function (data) {
            
            var arr = data.data;
            var categry = "";
            $('#example tbody').html("");
            for (var i = 0; i <= arr.length; i++) {
                countrow++;
                if (arr.length > 0) {
                    if (arr[i].GeoPointCategory == 1) {
                        categry = "BIN";
                    }
                    else if (arr[i].GeoPointCategory == 2) {
                        categry = "GVP";
                    }
                    else {
                        categry = "HOTSPOT";
                    }


                    $('#example tbody').append('<tr><td>' + countrow +

                        '</td><td >' + arr[i].PointName +
                        '</td><td  style="display:none;">' + arr[i].Lat +
                        '</td><td style="display:none;">' + arr[i].Lng +
                        '</td><td  style="display:none;">' + arr[i].TypeId +
                        '</td><td style="display:none;">' + arr[i].Radius +
                        '</td><td style="display:none;">' + arr[i].ZoneNo +
                        '</td><td style="display:none;">' + arr[i].CircleName +
                        '</td><td style="display:none;">' + arr[i].WardNo +
                        '</td><td style="display:none;">' + arr[i].GeoPointCategory +
                        '</td><td>' + categry +
                        '</td><td>' + arr[i].PickupTime +
                        '</td><td  style="display:none;">' + arr[i].PointId +

                        "</td><td><a href = 'javascript: void (0); '   title='Remove' onclick = RemoveRow(this); <i class='icon-close2'></i></a>"

                        // return "<a href = 'javascript: void (0);'   title='Remove' onclick = RemoveRow(this); <i class='icon-close2'></i></a>";
                    );
                }
                    
            }
        }
    });
   //dt.columns([5,6, 7,8]).visible(false);
}

function RemoveRow(ele) {
    
    console.log($(ele).closest("tr"));
    $(ele).closest("tr").remove();

    var countrow = 0;
    var Lat = "";
    var Lng = "";
    var TypeId = "";
    var Radius = "";
    var Pointname = "";
    var CategoryId = "";
    var PickupTime = "";
    var PointID = "";
    var arr = [];
    //$('#example tbody').html("");
    $('#example > tbody > tr').each(function (i, el) {

        var $tds = $(this).find('td');
        // var ITripId = $(this).attr('data-tripid');
        Pointname = $tds.eq(1).text(),
            Lat = $tds.eq(2).text(),
            Lng = $tds.eq(3).text(),
            TypeId = $tds.eq(4).text(),
            Radius = $tds.eq(5).text()

        arr.push({
            Pointname: $tds.eq(1).text(),
            Lat: $tds.eq(2).text(),
            Lng: $tds.eq(3).text(),
            TypeId: $tds.eq(4).text(),
            Radius: $tds.eq(5).text(),
            Zone: $tds.eq(6).text(),
            Circle: $tds.eq(7).text(),
            Ward: $tds.eq(8).text(),
            CategoryId: $tds.eq(9).text(),
            PickupTime: $tds.eq(11).text(),
            PointID: $tds.eq(12).text()
        });


    });
    
    $('#example tbody').html("");
    var categry = "";
    for (var i = 0; i <= arr.length; i++) {
        countrow++;
        
        if (arr[i].CategoryId == 1) {
                categry = "BIN";
            }
        else if (arr[i].CategoryId == 2) {
                categry = "GVP";
            }
            else {
                categry = "HOTSPOT";
            }
        $('#example tbody').append('<tr><td>' + countrow +

            '</td><td >' + arr[i].Pointname +
            '</td><td  style="display:none;">' + arr[i].Lat +
            '</td><td  style="display:none;">' + arr[i].Lng +
            '</td><td  style="display:none;">' + arr[i].TypeId +
            '</td><td style="display:none;">' + arr[i].Radius +
            '</td><td style="display:none;">' + arr[i].Zone +
            '</td><td style="display:none;">' + arr[i].Circle +
            '</td><td style="display:none;">' + arr[i].Ward +
            '</td><td style="display:none;">' + arr[i].CategoryId +
            '</td><td>' + categry +
            '</td><td>' + arr[i].PickupTime +
            '</td><td style="display:none;">' + arr[i].PointID +

            "</td><td><a href = 'javascript: void (0); '   title='Remove' onclick = RemoveRow(this); <i class='icon-close2'></i></a>"

            // return "<a href = 'javascript: void (0);'   title='Remove' onclick = RemoveRow(this); <i class='icon-close2'></i></a>";
        );
    }

    // $(this).closest("tr").remove();
}

$(document).on("change", "#ddlPointType", function () {
    $("#dvPoint").show();
    
    if ($("#ddlPointType").val() == 2) {
        $("#dvtxtPointName").show();
        $("#dvPointName").hide();
        $("#txtLat").prop("disabled", false);
        $("#txtLng").prop("disabled", false);
        $("#txtRadius").prop("disabled", false);
        $("#divcategory").show();
        $("#divtodate").show();
        // $("#ddlCatgory").prop("disabled", false);
        ClearAllData();
    } else {
        $("#dvPointName").show();
        $("#dvtxtPointName").hide();
        $("#txtLat").prop("disabled", true);
        $("#txtLng").prop("disabled", true);
        $("#txtRadius").prop("disabled", true);
        $("#divcategory").show();
        $("#divtodate").show();
        GetPointsName();
    }
})
$(document).on("change", "#txtPointName", function () {


    GrepData();
})
function GrepData() {
    var dataS = $.grep(MyjsonMainSS, function (element, index) {
        return element.Id == $("#txtPointName").val();
        // console.log(data.items);
    });
    
    var data1 = dataS;
    $("#txtCategoryS").val(data1[0].GeoCategoryId);
    $("#txtPointtype").val(data1[0].PointType);
    //$("#txtLng").val(data1[0].Lng);
    //$("#txtRadius").val(data1[0].Radius);

}
var  MyjsonMainSS = "";
function GetPointsName() {
    var IsRequired = 0;
    var Category = "Select Actual Point";
    
    $.ajax({
        type: "post",
        url: "/Collection/GetAllDPointsName",
        // data: { RouteTripCode: TripName, SDate: document.getElementById('txtFromDate').value },
        data: {},
        datatype: "json", //GeoPointId, GeoPointName
        traditional: true,
        success: function (data) {



            //var Myjson = JSON.parse(data);
            MyjsonMain = data.data;
            // var Resource = "<select id='ddlPointName' class='form-control'>";
            $("#ddlPointName").select2({

            });
            //if (IsRequired == 1)
            //   Resource = Resource + '<option value="0">Select Actual Point</option>';
            //else
            //   Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            // var Myjson = JSON.parse(data);
            var Resource = "<select id='ddlPointName' class='form-control'>";
            Resource = Resource + '<option value="0">Select Actual Point</option>';

            for (var i = 0; i < MyjsonMain.length; i++) {
                Resource = Resource + '<option value=' + MyjsonMain[i].GeoPointId + '>' + MyjsonMain[i].GeoPointName + '</option>';
            }
            Resource = Resource + '</select>';
            $("#ddlPointName").html(Resource);

        }
    });
}
function GetPointsName1() {
    var IsRequired = 0;
    var Category = "Select Actual Point";
    
    $.ajax({
        type: "post",
        url: "/Collection/GetAllDPointsName1",
        // data: { RouteTripCode: TripName, SDate: document.getElementById('txtFromDate').value },
        data: { WardId: $("#ddlWard").val() },
        datatype: "json", //GeoPointId, GeoPointName
        traditional: true,
        success: function (data) {

            
            $("#ddlPointName").html("");
            //var Myjson = JSON.parse(data);
            var MyjsonMain1 = JSON.parse(data);
            // var Resource = "<select id='ddlPointName' class='form-control'>";
            $("#ddlPointName").select2({

            });
            //if (IsRequired == 1)
            //   Resource = Resource + '<option value="0">Select Actual Point</option>';
            //else
            //   Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            // var Myjson = JSON.parse(data);
            var Resource = "<select id='ddlPointName' class='form-control'>";
            Resource = Resource + '<option value="0">Select Actual Point</option>';

            for (var i = 0; i < MyjsonMain1.length; i++) {
                Resource = Resource + '<option value=' + MyjsonMain1[i].GeoPointId + '>' + MyjsonMain1[i].GeoPointName + '</option>';
            }
            Resource = Resource + '</select>';
            $("#ddlPointName").html(Resource);

        }
    });
}
$(document).on("click", "#btnAdd", function () {

    AdddatainGrid();
})

function ClearAllData() {
    $("#txtLat").val("")
    $("#txtLng").val("")
    $("#txtPointName").val("")
    $("#txtRadius").val("")
}
function AdddatainGrid() {
    
    $('.dataTables_empty').parent().remove();
    var isvalid = 0;
    var IsExist = 0;
    var pointnameS = "";
    //if ($("#ddlPointType").val() == 1) {
    //    pointnameS = $("#ddlPointName option:selected").text();
    //}
    //else {
    pointnameS = $("#txtPointName option:selected").text();
    // }

    $('#example > tbody > tr').each(function (i, el) {
        var $tds = $(this).find('td');
        // var ITripId = $(this).attr('data-tripid');
        var Pointname = $tds.eq(1).text();
        if (Pointname == pointnameS)
            IsExist = 1;

    });

    //if ($("#ddlPointType").val() == 1) {
    //    if ($("#ddlPointName").val() == 0) {
    //        isvalid = 1;
    //    }

    //}
    //else {
    if ($("#txtPointName").val() == "")
    {
        isvalid = 1;
    }
    // }

    //if ($("#txtLat").val() == '') {
    //    isvalid = 1;
    //}
    //if ($("#txtLng").val() == '') {
    //    isvalid = 1;
    //}
    //if ($("#txtRadius").val() == '') {
    //    isvalid = 1;
    //}

    var DDLId = 0;
    var splitUrl = window.location.href;
    var cids = splitUrl.split('=');
    DDLId = cids[1];

    if (DDLId == 0) {
        if ($("#ddlVehicleNumber").val() == 0) {
            isvalid = 1;
        }
        //  $("#example tbody").html("");
    }
    var PointName = "";
    // var PointId = $("#txtPointId").val();
    //if ($("#ddlPointType").val() == 1) {
    //    PointName = $("#ddlPointName option:selected").text();
    //}
    //else {
        PointName = $("#txtPointName option:selected").text();
    //}
    var PointID = $("#txtPointName").val();
    var Lat = $("#txtLat").val();
    var Lng = $("#txtLng").val();
   // var TypeId = $("#ddlPointType option:selected").text();
    var TypeId = $("#txtPointtype").val();
    var Radius = $("#txtRadius").val();
    var Zone = $("#ddlZone").val();
    var Circle = $("#ddlCircle").val();
    var Ward = $("#ddlWard").val();
    var CategoryId = $("#txtCategoryS").val();
    var CategoryName = "";
    var PickupTime = $("#txtToDate").val();

    var Tablelenght = $("#example tr").length;
    if (CategoryId == 1) {
        CategoryName = "BIN";
    }
    else if (CategoryId == 2) {
        CategoryName = "GVP";
    }
    else {
        CategoryName = "HOTSPOT";
    }
    //var StartDigit = $("#txtSdigit").val();
    //var EndDigit = $("#txtEdigit").val();
    var SrNo = 0;
    $("#example").show();
    // var count = StartDigit - 1;
    //$('#example tbody').html("");

    //for (var i = StartDigit; i <= EndDigit; i++) {
    //    count++;
    //    SrNo++;
    if (IsExist == 0) {
        if (isvalid == 0) {
            $('#example tbody').append('<tr><td>' + Tablelenght +

                '</td><td >' + PointName +
                '</td><td style="display:none;">' + Lat +
                '</td><td style="display:none;">' + Lng +
                '</td><td  style="display:none;">' + TypeId +
                '</td><td style="display:none;">' + Radius +
                '</td><td style="display:none;">' + Zone +
                '</td><td style="display:none;">' + Circle +
                '</td><td style="display:none;">' + Ward +
                '</td><td style="display:none;">' + CategoryId +
                '</td><td>' + CategoryName +
                '</td><td>' + PickupTime +
                '</td><td style="display:none;">' + PointID +

                "</td><td><a href = 'javascript: void (0); '   title='Remove' onclick = RemoveRow(this); <i class='icon-close2'></i></a>"

                // return "<a href = 'javascript: void (0);'   title='Remove' onclick = RemoveRow(this); <i class='icon-close2'></i></a>";
            );

        }
        else {
            alert("Please all filled are required")
        }
    }

    else {
        ShowCustomMessage('0', 'This Point Name is already exists', '');
    }


    //}

}

//$(document).on("click", "#btnSave", function () {
//    AddPointType();

//});
function Formsubmit() {
    
    AddPointType();
    return false;
}
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode != 46 && charCode > 31
        && (charCode < 48 || charCode > 57))
        return false;

    return true;
}
function AddPointType() {
    
    /*if($("#Ispermanent").is("checked",true))*/
    var isactive = 0;
    if ($("#Ispermanent").is(":checked") == true) {
        isactive = 1;
    }
    var ZoneId = $('#ddlZone').val();
    var CircleId = $('#ddlCircle').val();
    var WardId = $('#ddlWard').val();
    var CategoryId = 1;
    var PickupTime = $('#txtToDate').val();
    if ($("#example  tbody tr").length > 0) {
        // var flag = 0;

        // checkValidation();
        // var dataS = JSON.parse(datamain)
        var DDLId = 0;
        //var PointId = 0;
        var splitUrl = window.location.href;
        var cids = splitUrl.split('=');
        DDLId = cids[1];
        // PointId = cids[1];
        var FormData = {
            VehicleUid: $("#ddlVehicleNumber").val(),
            SheuleTime: document.getElementById('txtFromDate').value,//parameter date
            Remarks: $("#txtRemarks").val(),
            // var VehicleUid = $("#ddlVehicleNumber").val();
            DDLId: DDLId,
            ZoneId: ZoneId,
            CircleId: CircleId,
            WardId: WardId,
            CategoryId: CategoryId,
            PickupTime: PickupTime,
            isactive: isactive
        };




        var total = 0;
        var count = 0;

        var jsonTable = $('#example tr:has(td)').map(function () {
            count++;

            var $td = $('td', this);
            //var countId = "".concat('E-', count);
            //var PointName = "".concat(countId, $td.eq(1).text());
            var PointName = $td.eq(1).text();
            var Lat = $td.eq(2).text();
            var Lng = $td.eq(3).text();
            var TypeId = 0;
            var PointIS = 0;
            if ($td.eq(4).text() == "Existing") {
                var TypeId = 1;
                PointIS = $("#ddlPointName").val();
            }
            else {
                var TypeId = 2;
                PointIS = count;
            }
            var Radius = $td.eq(5).text();
            var Zone   = $td.eq(6).text();
            var Circle = $td.eq(7).text();
            var Ward   = $td.eq(8).text();
            var OrderId = count;
            var PointId = $td.eq(12).text()
            var Category = $td.eq(9).text();
            var PickupTime = $td.eq(11).text();
           // var PickupTime = $td.eq(12).text();
            total += parseFloat($td.eq(2).text());
            return {
                //  PointId: PointId,

                PointName: PointName,
                Lat: Lat,
                Lng: Lng,
                TypeId: TypeId,
                RefEmrUid: DDLId,
                Radius: Radius,
                CreatedDate: '',
                CretedBy: '',
                ModifyDate: '',
                ModifyBy: '',
                OrderId: OrderId,
                PointId: PointId,
                Category: Category,
                PickupTime: PickupTime,
                Zone  :Zone  ,
                Circle:Circle,
                Ward: Ward
            }
        }).get();

        $('#example1 > tfoot > tr > td:nth-child(3)').html(total);


        var data = {};
        data = jsonTable;


        $.ajax({
            type: "POST",
            url: '/Collection/InsertEmrPointdetail',
            data: { jobjS: JSON.stringify(data), JArrayval: JSON.stringify(FormData) },
            async: false,
            success: function (data) {

                ShowCustomMessage('1', "Point SAVE SUCCESSFULLY ", '/Collection/EmergencyPointDetail');


                $('#modal_form_vertical').modal('toggle');
                return false;
            },
            error: function (result) {
            }
        });
    }
    else {
        ShowCustomMessage('0', 'No Data available in table', '');
    }

    // }
}

function GetvehicleDataById(ddId) {

    $.ajax({
        type: "post",
        url: "/Collection/GetEmergencyPointsInfoById",
        data: { EmrUid: ddId },
        async: false,
        success: function (data) {
            
            var myJSONData = JSON.parse(data);


            $("#txtRemarks").val(myJSONData[0].Remarks);
            setTimeout(() => {
                $("#ddlVehicleNumber").val(myJSONData[0].VehicleUid).trigger('change');

            }, 2000);

            $("#txtFromDate").val(myJSONData[0].SheduleTime);

        }
    });
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
function getLatLongValue() {

    if ($('#txtLat').val() != '' && !isNaN($('#txtLat').val()) && parseInt($('#txtLat').val()) > 0) {
        if ($('#txtLng').val() != '' && !isNaN($('#txtLng').val()) && parseInt($('#txtLng').val()) > 0) {
            currentlatlng = new google.maps.LatLng($('#txtLat').val(), $('#txtLng').val());
            map1.panTo(currentlatlng);
            setMarker();
        }
    }
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
                if (Myjson[i].GPCId != 5 && Myjson[i].GPCId != 6) {
                    Resource = Resource + '<option value=' + Myjson[i].GPCId + '>' + Myjson[i].GeoPointCategory + '</option>';
                }

            }
            Resource = Resource + '</select>';
            $('#ddlCatgory').html(Resource);
        }
    });
}


function GetPointsNameByWardId() {
    var IsRequired = 0;
    var Category = "Select Actual Point";
    
    $.ajax({
        type: "post",
        url: "/Collection/GetAllDPointsNameByZoneCircle",
        // data: { RouteTripCode: TripName, SDate: document.getElementById('txtFromDate').value },
        data: { WardId: $("#ddlWard").val() },
        datatype: "json", //GeoPointId, GeoPointName
        traditional: true,
        success: function (data) {

            
            $("#txtPointName").html("");
            //var Myjson = JSON.parse(data);
             MyjsonMainSS = JSON.parse(data);
            // var Resource = "<select id='ddlPointName' class='form-control'>";
            $("#txtPointName").select2({

            });
            //if (IsRequired == 1)
            //   Resource = Resource + '<option value="0">Select Actual Point</option>';
            //else
            //   Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            // var Myjson = JSON.parse(data);
            var Resource = "<select id='txtPointName' class='form-control'>";
            Resource = Resource + '<option value="0">Select Actual Point</option>';

            for (var i = 0; i < MyjsonMainSS.length; i++) {
                Resource = Resource + '<option value=' + MyjsonMainSS[i].Id + '>' + MyjsonMainSS[i].PointName + '</option>';
            }
            Resource = Resource + '</select>';
            $("#txtPointName").html(Resource);

        }
    });
}