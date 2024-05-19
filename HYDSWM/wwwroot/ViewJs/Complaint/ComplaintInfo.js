var map;
var Cmap;
var circle;
var marker;
var geocoder;
var Cgeocoder;
var infowindow;
var currentlatlng = new google.maps.LatLng(17.4940964, 78.4000115);
var IsMax = 0;
var Cmarker;
function Formsubmit() {

    SaveAndUpdateComplaintInfo();
    return false;
}
function SaveAndUpdateComplaintInfo() {
    var isvalid = 1;
    var StatusId = $("#ddlStatus").val();

    var IFormData = {

        complaint_num: $("#complaint_num").val(),
        StatusId: StatusId,
        complaint_cat: $("#complaint_cat").val(),
        complaint_add: $("#complaint_add").val(),
        complaint_descrip: $("#complaint_descrip").val(),
        Transfer_station: $("#Transfer_station").val(),
        ckbIsActive: $("#ckbIsActive").val(),
    };

    var formData = new FormData();
 
    formData.append('input', JSON.stringify(IFormData));

    if (IFormData.CCId == '' || IFormData.StatusId == '' || IFormData.ActionRemark == '')
        isvalid = 0;
    if (isvalid == 1) {
        ShowLoading($('#dvUpdComp'));
        $.ajax({
            type: "POST",
            url: '/Complaint/UpdateStaffComplaint',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                if (data.data.Result == 1 || data.data.Result == 2) {
                    HideLoading($('#dvUpdComp'));
                    ShowMessage('1', data.Msg);

                    $('#modal_form_vertical').modal('toggle');
                }
                else {
                    HideLoading($('#dvUpdComp'));
                    ShowMessage('0', data.Msg);
                }
            },
            error: function (result) {
               HideLoading($('#dvUpdComp'));
                ShowMessage('0', 'Something Went Wrong!');
            }
        });
    }
    else
        ShowMessage('0', 'Please Enter All Required Details');

}


function CallFunc1(obj) {
    
    //var ddId = $(obj).attr('cid');
    $('#model_content').load("/Complaint/AddComplaint");
    $('#modal_form_AddDetail').modal('toggle');

    //if (ddId > 0) {

        //SetDataOnControls(ddId);

    //}
}





function ShowMessage(typemsg, msg) {

    if (typemsg == '1') {
        swal({
            title: 'Good job!',
            text: msg,
            type: 'success'
        },
            function () {
                //window.location.href = '/Complaint/AllComplaint';
            });
    }
    else {
        swal({
            title: 'Oops...',
            text: msg,
            type: 'error'
        });
    }
    //$(".sweet-alert").css('background-color', '##3a445b');
}
function CallFunc(obj) {
    
    var Address = $(obj).attr('Address');
    var Lat = $(obj).attr('Lat');
    var Lng = $(obj).attr('Lng');
    var ddId = $(obj).attr('cid');
    var C2Id = $(obj).attr('C2Id');
    //$('#user_content').load("/Citizen/UpdateComplaint?ComplaintNo=" + ddId + "&CCId=" + C2Id);
    $('#modal_form_vertical').modal('toggle');

    if (C2Id > 0)
        SetDataOnControl(C2Id, ddId, Lat, Lng, Address);
}
function SetDataOnControl(C2Id, ddId, Lat, Lng, Address) {
   // GetAllTicketStatus();

    $("#txtActionRemarks").val('');

    $("#hfCCId").val(C2Id);
    $("#txtComplaintNo").val(ddId);

    $("#hfCLat").val(Lat);
    $("#hfCLng").val(Lng);
    loadCMap();
    setTimeout(() => {
        $("#txtCAddress").val(Address);

    }, 2000);
    //$("#txtCAddress").val(Address);
}
function loadMap() {
    var lat;
    var lng;
    if ($('#hfLat').val() != '' && !isNaN($('#hfLat').val()) && parseInt($('#hfLat').val()) > 0) {
        lat = $('#hfLat').val();
        lng = $('#hfLng').val();
        currentlatlng = new google.maps.LatLng(lat, lng);
    }
    setLatLongValue();
    var mapOptions = {
        zoom: 15,
        center: currentlatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById('dvIMap'), mapOptions);
    var input = document.getElementById('searchInput');
    map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    geocoder = new google.maps.Geocoder();
    var autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.bindTo('bounds', map);



    autocomplete.addListener('place_changed', function () {

        var place = autocomplete.getPlace();
        currentlatlng = new google.maps.LatLng(place.geometry.location.lat(), place.geometry.location.lng());//e.latLng;
        if (place.geometry.viewport) {
            map.fitBounds(place.geometry.viewport);
        } else {
            map.setCenter(place.geometry.location);
            map.setZoom(17);
        }
        marker.setPosition(place.geometry.location);
        marker.setVisible(true);
        setLatLongValue();

    });

    google.maps.event.addDomListener(map, 'click', function (e) {

        currentlatlng = e.latLng;
        if (currentlatlng) {
            map.panTo(currentlatlng);
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
        map: map
    });

    if (marker) {
        google.maps.event.addDomListener(marker, "drag", function () {
            currentlatlng = marker.getPosition();
            setLatLongValue();
        });
    }


}

function setLatLongValue() {

    $('#hfLat').val(currentlatlng.lat());
    $('#hfLng').val(currentlatlng.lng());
    GetAddress();
}

function GetAddress() {

    var geocoder = geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': currentlatlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                $('#txtAddress').val(results[1].formatted_address);
                //alert("Location: " + results[1].formatted_address);
            }
        }
    });
}

function getLatLongValue() {

    if ($('#hfLat').val() != '' && !isNaN($('#hfLat').val()) && parseInt($('#hfLat').val()) > 0) {
        if ($('#hfLng').val() != '' && !isNaN($('#hfLng').val()) && parseInt($('#hfLng').val()) > 0) {
            currentlatlng = new google.maps.LatLng($('#hfLat').val(), $('#hfLng').val());
            map.panTo(currentlatlng);
            setMarker();
        }
    }
}

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
    document.getElementById("txtFromDate").value =  (date.getMonth() + 1) + '/' + date.getDate()+'/'+date.getFullYear()  ;
    document.getElementById("txtToDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear() ;
    loadMap();
    GetDataTableData('Load');
});
function GetAllTicketStatus() {

    $.ajax({
        type: "post",
        url: "/HelpDesk/GetAllTicketStatus",
        data: '{}',
        success: function (data) {
            var myjson = JSON.parse(data);

            // start category binding
            $('#ddlStatus').html('');
            var Resource = "<select id='ddlStatus' class='form-control'>";
            Resource = Resource + '<option value="">Select</option>';
            for (var i = 0; i < myjson.length; i++) {
                if (IsMax == 1) {
                    if (myjson[i].TStatusId == 6)
                        Resource = Resource + '<option value=' + myjson[i].TStatusId + '>' + myjson[i].StatusName + '</option>';
                }
                else
                    Resource = Resource + '<option value=' + myjson[i].TStatusId + '>' + myjson[i].StatusName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlStatus').html(Resource);

            //End region
        }
    });
}
var dt;
function GetDataTableData(Type) {
    var FromDate = document.getElementById('txtFromDate').value;
    var ToDate = document.getElementById('txtToDate').value;
    var IsClick = '0';
    var NotiId = "";
    if (Type != 'Load') {
        IsClick = '1';
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
            url: "/Complaint/GetAllStaffComplaint_Paging/",
            type: 'POST',
            data: function (d) {
                d.ToDate = ToDate;
                d.FromDate = FromDate;
                d.ContratorId = IsClick;
                d.NotiId = NotiId;
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

                    return '<a class="gticket" cticketid="' + row.SComplaintId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "Complaintcode", sortable: true },
            { data: "ComplaintType", sortable: true },
            { data: "Location", sortable: true },
            { data: "FAddress", sortable: true },
            { data: "Remarks", sortable: true },
            { data: "CreatedBy", sortable: true },
            { data: "ComplaintOn", sortable: true },
            { data: "ClosedOn", sortable: true },
            { data: "TotalTimeTaken", sortable: true },
            {
                sortable: true,
                "render": function (data, type, row, meta) {
                    return '<span class="' + row.LabelClass + '">' + row.Status + '</span>';
                }
            },

            { data: "CAddress", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.SComplaintId + '" href="' + row.Img2Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img2Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "CRemarks", sortable: true },
            { data: "ClosedBy", sortable: true },
            { data: "TStationName", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.IsClosed == false) {
                        return "<a cid='" + row.Complaintcode + "' CId='" + row.SComplaintId + "'Lat='" + row.FLat + "'Lng='" + row.FLng + "'Address='" + row.FAddress + "'  href='javascript:void(0);' title='edit' onclick='CallFunc(this);'><i class='ti-pencil'></i></a>";
                    }
                    else
                        return '';

                }
            }
        ]
    });

}

function loadCMap() {

    var lat;
    var lng;
    if ($('#hfCLat').val() != '' && !isNaN($('#hfCLat').val()) && parseInt($('#hfCLat').val()) > 0) {
        lat = $('#hfCLat').val();
        lng = $('#hfCLng').val();
        currentlatlng = new google.maps.LatLng(lat, lng);
    }
    CsetLatLongValue();
    var mapOptions = {
        zoom: 15,
        center: currentlatlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    Cmap = new google.maps.Map(document.getElementById('dvICMap'), mapOptions);
    var input = document.getElementById('searchCInput');
    Cmap.controls[google.maps.ControlPosition.TOP_LEFT].push(input);

    Cgeocoder = new google.maps.Geocoder();
    var autocomplete = new google.maps.places.Autocomplete(input);
    autocomplete.bindTo('bounds', Cmap);



    autocomplete.addListener('place_changed', function () {

        var place = autocomplete.getPlace();
        currentlatlng = new google.maps.LatLng(place.geometry.location.lat(), place.geometry.location.lng());//e.latLng;
        if (place.geometry.viewport) {
            Cmap.fitBounds(place.geometry.viewport);
        } else {
            Cmap.setCenter(place.geometry.location);
            Cmap.setZoom(17);
        }
        Cmarker.setPosition(place.geometry.location);
        Cmarker.setVisible(true);
        CsetLatLongValue();

    });

    google.maps.event.addDomListener(map, 'click', function (e) {

        currentlatlng = e.latLng;
        if (currentlatlng) {
            Cmap.panTo(currentlatlng);
            CsetLatLongValue();
            CsetMarker();
        }
    });
    CgetLatLongValue();

}

function CsetMarker() {

    if (Cmarker != undefined)
        Cmarker.setMap(null);

    Cmarker = new google.maps.Marker({
        position: currentlatlng,
        draggable: true,
        map: Cmap
    });

    if (Cmarker) {
        google.maps.event.addDomListener(Cmarker, "drag", function () {
            currentlatlng = Cmarker.getPosition();
            CsetLatLongValue();
        });
    }


}

function CsetLatLongValue() {

    $('#hfCLat').val(currentlatlng.lat());
    $('#hfCLng').val(currentlatlng.lng());
    CGetAddress();
}

function CGetAddress() {

    var geocoder = geocoder = new google.maps.Geocoder();
    geocoder.geocode({ 'latLng': currentlatlng }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            if (results[1]) {
                $('#txtCAddress').val(results[1].formatted_address);
                //alert("Location: " + results[1].formatted_address);
            }
        }
    });
}

function CgetLatLongValue() {

    if ($('#hfCLat').val() != '' && !isNaN($('#hfCLat').val()) && parseInt($('#hfCLat').val()) > 0) {
        if ($('#hfCLng').val() != '' && !isNaN($('#hfCLng').val()) && parseInt($('#hfCLng').val()) > 0) {
            currentlatlng = new google.maps.LatLng($('#hfCLat').val(), $('#hfCLng').val());
            Cmap.panTo(currentlatlng);
            CsetMarker();
        }
    }
}
