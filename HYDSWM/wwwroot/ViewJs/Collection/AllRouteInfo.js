

function DeleteEmpFunc(objthis) {
    if (confirm("Are you sure you want to delete this?")) {
        var cid = $(objthis).attr("cid");
        $.ajax({
            type: "POST",
            url: '/DeleteLog/DeleteRouteMasterInfoById',
            data: { Id: cid },
            success: function (Result) {
                var data = JSON.parse(Result);
                if (data.Result == 1 || data.Result == 2) {
                    ShowMessage('1', data.Msg);
                }
                else
                    ShowMessage('0', data.Msg);
            },
            error: function (result) {
                ShowMessage('0', 'Something Went Wrong!');
            }
        });
    }
}

function Formsubmit() {

    SaveAndUpdateNouteInfo();
    return false;

}
function RedirectToPage(obj) {
    var ddId = $(obj).attr('cid');
    window.location = "/Collection/AddRoute?cid=" + ddId;
}
function CallFunc(obj) {

    var ddId = $(obj).attr('cid');
    $('#user_content').load("/Collection/AddNRoute");
    $('#modal_form_vertical').modal('toggle');
    setTimeout(() => {
        $("#ddlVehicleNo").select2({
            dropdownParent: $("#modal_form_vertical")
        });
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
        url: "/Collection/GetNRouteInfoById",
        data: { RouteId: ddId },
        success: function (data) {


            var myJSONData = JSON.parse(data);
            var myJSON = myJSONData.Table[0];
            var nstopdata = myJSONData.Table1;
            $("#hfRouteId").val(myJSON.RouteId);
            $("#txtRouteName").val(myJSON.RouteName);
            $("#txtRouteCode").val(myJSON.RouteCode);
            $("#txtBufferMin").val(myJSON.BufferMin);
            setTimeout(() => {
                $("#ddlVehicleNo").val(myJSON.VehicleUId).trigger('change');
            }, 3000);
            // $("#ddlVehicleNo > [value=" + myJSON.VehicleUId + "]").attr("selected", "true");
            //$("#txtArrvlTime").val(myJSON.Arvltime);
            //$("#txtDeptTime").val(myJSON.DeptTime);
            if (myJSON.IsActive == true)
                $("#ckbIsActive").prop("checked", true);
            else
                $("#ckbIsActive").prop("checked", false);

            if (nstopdata != '')
                GetStopsByRouteCode(nstopdata);
        }
    });
}
function SaveAndUpdateNouteInfo() {

    var isvalid = 1;
    var FormData = {
        RouteId: $("#hfRouteId").val(),
        RouteName: $("#txtRouteName").val(),
        RouteCode: $("#txtRouteCode").val(),
        VehicleUId: $('#ddlVehicleNo').val(),
       // ArrvlTime: $("#txtArrvlTime").val(),
       // DeptTime: $("#txtDeptTime").val(),
        BufferMin: $("#txtBufferMin").val(),
        IsActive: $('#ckbIsActive').is(':checked')
    };
    var tablarr = [];
    $('#tblStop > tbody > tr').each(function (i, el) {
        
        var $tds = $(this).find('td');
        var stopid = $(this).attr('data-StopId');
        var PickupTime = $tds.eq(1).text();
        var TripId = $(this).attr('data-tripid');
       // var SDeptTime = $tds.eq(2).text();
        var data = {};
        data.StopId = stopid;
        data.PickupTime = PickupTime;
        data.TripId = TripId;
        //data.DeptTime = SDeptTime;
        tablarr.push(data);
    });

    if (FormData.RouteName == '' || FormData.VehicleUId == '')
        isvalid = 0;
    if (tablarr.length == 0)
        isvalid = 0

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Collection/AddNRouteInfo',
            data: { jobj: JSON.stringify(FormData), JArrayval: JSON.stringify(tablarr) },
            success: function (data) {
                var myJSON = JSON.parse(data);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg, '/Collection/AllNRoute');

                    $('#modal_form_vertical').modal('toggle');
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
function format(item) {
    // `d` is the original data object for the row
    //alert(JSON.stringify(d.CASO));
    var InnerGrid = '<div class="table-scrollable"><table class="table display product-overview mb-30"><thead><tr><th>Sr. No.</th> <th> Point Name </th><th> Pickup Time </th><th> Trip </th><th> Vehicle No </th><th> Buffer Min </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;

        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td>' + info.StopName + '</td>' +
            '<td>' + info.SPickupTime + '</td>' +
            '<td>' + info.TripName + '</td>' +
            '<td>' + info.VehicleNo + '</td>' +
            '<td>' + info.BufferMin + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}
$(document).ready(function () {

    GetDataTableData('Load');
    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#example').DataTable().row(tr);
        var routecode = tr.find('td:eq(3)').text();

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var iccData;
            $.ajax({
                url: "/Collection/GetAllNStopByRouteCode",
                type: "POST",
                dataType: "json",
                data: { RouteCode: routecode },
                success: function (data) {
                    var myJSON = JSON.parse(data);
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });
});
function GetStopsByRouteCode(myJSON) {


    for (var i = 0; i < myJSON.length; i++) {
        var tr = '<tr data-StopId=' + myJSON[i].StopId + ' data-tripid=' + myJSON[i].TripId + '><td >' + myJSON[i].StopName +
            '</td><td>' + myJSON[i].PickupTime +
            '</td><td >' + myJSON[i].TripName +
            '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveRow(this);"> <i class="icon-close2"></i> Remove</button>'
        '</td></tr>';
        $('#tblStop > tbody').append(tr);
    }


}

var dt;
function GetDataTableData(Type) {

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
            url: "/Collection/AllNRouteInfo/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {
                d.NotiId = '';
                return { requestModel: d };
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

            //{ data: "VehicleNo", sortable: true },
            //{ data: "VehicleType", sortable: true },
            { data: "RouteName", sortable: true },
            { data: "RouteCode", sortable: true },
            { data: "SourceStop", sortable: true },
            { data: "DestinationStop", sortable: true },
            { data: "SArrvlTime", sortable: true },
            { data: "SDeptTime", sortable: true },
            { data: "TotalStop", sortable: true },
            { data: "TotalTrips", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.IsActive == 1)
                        return '<span class="badge badge-success">ACTIVE</span><br/><a data-RouteCode=' + row.RouteCode +
                            ' href=javascript:void(0)  onclick=ShowMapPopup(this);  >View Route On Map</a>';
                    else
                        return '<span class="badge badge-danger">DE-ACTIVE</span><br/><a data-RouteCode=' + row.RouteCode +
                            ' href=javascript:void(0)  onclick=ShowMapPopup(this);  >View Route On Map</a>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return "<a cid='" + row.RouteId + "' href='javascript:void(0);' title='edit' onclick='RedirectToPage(this);'><i class='ti-pencil'></i></a>";

                }
            }
        ]
    });
}


function AddStops() {
    var IsValid = 1;
    var stopid = $('#ddlStop').val();
    var stopname = $("#ddlStop option:selected").text();
    var Tripname = $("#ddlTrip option:selected").text();
    var TripId = $('#ddlTrip').val();
    var PickupTime = $('#txtPickupTime').val();
    //var SArrvlTime = $('#txtSArrvlTime').val();
    //var SDeptTime = $('#txtSDeptTime').val();
    if (stopid == '' || PickupTime == ''  )
        IsValid = 0

    if (IsValid == 1) {
        var IsExist = false;
        $('#tblStop > tbody > tr').each(function (i, el) {
            var $tds = $(this).find('td');
            var Istopid = $tds.attr('data-StopId');

            if (Istopid == stopid)
                IsExist = true;

        });

        if (IsExist == false) {
            var tr = '<tr data-StopId="' + stopid + '" data-tripid="' + TripId + '"><td>' + stopname +
                '</td><td>' + PickupTime +
                '</td><td >' + Tripname +
                '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveRow(this);"> <i class="icon-cancel-circle2"></i> Remove</button>'
            '</td></tr>';
            $('#tblStop > tbody').append(tr);


            $('#txtPickupTime').val('');
           // $('#txtSDeptTime').val('');
            //$('#ddlStop').val('');
        }
        else
            ShowCustomMessage('0', 'This geo point is already exists', '');
    }
    else
        ShowCustomMessage('0', 'Please Fill All Required Details', '');

}
function RemoveRow(objthis) {
    $(objthis).closest('tr').remove();
}
function cp() {
    var tablarr = [];
    $('#tblStop > tbody > tr').each(function (i, el) {
        var $tds = $(this).find('td');
        var stopid = $tds.attr('data-StopId');
        var PickupTime = $tds.eq(1).text();
        var TripId = $tds.attr('data-TripId');
        //var SDeptTime = $tds.eq(2).text();
        var data = {};
        data.stopid = stopid;
        data.PickupTime = PickupTime;
        data.TripId = TripId;
        tablarr.push(data);
    });

}


function ShowMapPopup(objthis) {

    var RouteCode = $(objthis).attr('data-RouteCode');

    $.ajax({
        url: "/Collection/GetAllNStopByRouteCode",
        type: "POST",
        dataType: "json",
        data: { RouteCode: RouteCode },
        success: function (data) {
            var myJSON = JSON.parse(data);
            ShowDataOnMap(myJSON);
            $('#viewonmap').modal('toggle');
        }
    });
    return false;
}
function ShowDataOnMap(data) {
    geocoder = new google.maps.Geocoder();
    var markerarray = new Array();
    var locations = data;
    var count = locations.length - 1;
    var infowindow = new google.maps.InfoWindow();
    var contentString;
    var marker, i, mapOptions, map;
    var iconBase = '../otherfiles/global_assets/images/';

    mapOptions = {
        center: new google.maps.LatLng(locations[0].Lat, locations[0].Lng), zoom: 13,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };

    map = new google.maps.Map(document.getElementById("dvIMap"), mapOptions);

    for (i = 0; i < locations.length; i++) {

        var myLatlng = new google.maps.LatLng(locations[i].Lat, locations[i].Lng);

        var datainsert = {
            lat: parseFloat(locations[i].Lat),
            lng: parseFloat(locations[i].Lng)

        };
        markerarray.push(datainsert);
        if (i == 0) {
            contentString = "<div style='float:right; padding: 10px;font-size: 18px;font-weight:bold;background-color: #1ab394;color: white;'>Geo Point -" + locations[i].StopName +
                "<br/>Pickup Time -" + locations[i].SPickupTime +
                "<br/>Trip -" + locations[i].TripName +
                "<br/>Vehicle No -" + locations[i].VehicleNo +
                "<br/>Source" +
                "</div>";

            marker = new google.maps.Marker({
                position: myLatlng,//new google.maps.LatLng(locations[i].Lat, locations[i].Lng),
                map: map,
                icon: iconBase + 'tYellow.png',
                content: contentString
            });
        }
        else if (i == count) {
            contentString = "<div style='float:right; padding: 10px;font-size: 18px;font-weight:bold;background-color: #1ab394;color: white;'>Geo Point -" + locations[i].StopName +
                "<br/>Pickup Time -" + locations[i].SPickupTime +
                "<br/>Trip -" + locations[i].TripName +
                "<br/>Vehicle No -" + locations[i].VehicleNo +
                "<br/>Destination" +
                "</div>";

            marker = new google.maps.Marker({
                position: myLatlng,//new google.maps.LatLng(locations[i].Lat, locations[i].Lng),
                map: map,
                icon: iconBase + 'tred.png',
                content: contentString
            });
        }
        else {
            contentString = "<div style='float:right; padding: 10px;font-size: 18px;font-weight:bold;background-color: #1ab394;color: white;'>Geo Point -" + locations[i].StopName +
                "<br/>Pickup Time -" + locations[i].SPickupTime +
                "<br/>Trip -" + locations[i].TripName +
                "<br/>Vehicle No -" + locations[i].VehicleNo +
                "</div>";

            marker = new google.maps.Marker({
                position: myLatlng,//new google.maps.LatLng(locations[i].Lat, locations[i].Lng),
                map: map,
                icon: iconBase + 'green-dot.png',
                content: contentString
            });
        }


        google.maps.event.addListener(marker, 'mouseover', (function (marker, i) {
            return function () {

                infowindow.setContent(marker.content);
                infowindow.open(map, marker);
            }
        })(marker, i));
        google.maps.event.addListener(marker, 'mouseout', (function (marker, i) {
            return function () {
                infowindow.close(map, marker);
            }
        })(marker, i));
    }


    var polyline = new google.maps.Polyline({
        path: markerarray,
        strokeColor: '#FF0000',
        strokeOpacity: 1.0,
        strokeWeight: 2,
        geodesic: true,
        icons: [{
            icon: { path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW },
            offset: '100%',
            repeat: '20px'
        }]
    });
    polyline.setMap(map);
}


