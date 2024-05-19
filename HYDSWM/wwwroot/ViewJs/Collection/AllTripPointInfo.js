$(document).ready(function () {

    GetDataTableData('Load');

    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#example').DataTable().row(tr);

        // var RouteId = tr.attr("data-RouteId");//tr.find('td:eq(3)').text();
        var RTDId = tr.find('.gticket').attr('cticketid');
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var iccData;
            $.ajax({
                url: "/Collection/GetPointsInfoByTripId",
                type: "POST",
                dataType: "json",
                data: { RTDId: RTDId },
                success: function (data) {
                    debugger
                    var myJSONData = JSON.parse(data);
                    // var myJSON = myJSONData.Table[0];
                    var myJSON = myJSONData.Table1;
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });
});

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
            url: "/Collection/GetAllTripPoint_Paging/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {
                d.NotiId = '1';
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
            {
                sortable: true,
                "render": function (data, type, row, meta) {


                    return '<span class="gticket" cticketid="' + row.RTDId + '">' + row.RouteCode + '</span>';

                }
            },
            { data: "TId", sortable: true },
            {
                sortable: true,
                "render": function (data, type, row, meta) {
                    if (row.VehicleNo=='')
                        return '<span class="badge badge-danger">No Vehicle Assigned</span>';
                    else
                        return '<span class="badge badge-success">' + row.VehicleNo+'</span>';

                }
            },
           /* { data: "TId", sortable: true },*/
            { data: "BufferMin", sortable: true },
            { data: "TotalStop", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.Status == 1)
                        return '<span class="badge badge-success">ACTIVE</span>';
                    else
                        return '<span class="badge badge-danger">DE-ACTIVE</span>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    
                    return '<a data-RTDId=' + row.RTDId +
                            ' href=javascript:void(0)  onclick=ShowMapPopup(this);  >View Route On Map</a>';
                   

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    if (row.TotalStop > 0)
                        return "<a cid='" + row.RTDId + "' href='javascript:void(0);' title='edit' onclick='RedirectToPage(this);'><i class='ti-pencil'></i></a><br><a data-SStatus='" + row.Status + "' cid='" + row.RTDId + "'  href='javascript:void(0);' title='Vehicle Allocation' onclick='RedirectToVehiclePage(this);'><i class='icon-truck'></i></a>";
                        else
                    return "<a cid='" + row.RTDId + "' href='javascript:void(0);' title='edit' onclick='RedirectToPage(this);'><i class='ti-pencil'></i></a>";

                }
            }
        ]
    });
}
function RedirectToPage(obj) {
    var ddId = $(obj).attr('cid');
    window.location = "/Collection/AddSTripPoint?cid=" + ddId;
}
function RedirectToVehiclePage(obj) {
    var CheckStatus = $(obj).attr('data-SStatus');
    if (CheckStatus == "false") {
        ShowCustomMessage('0', 'This Route is De-Active ,Please active this route then you are able to assign vehicle', '');
    }
    else {
        var ddId = $(obj).attr('cid');
        window.location = "/Collection/AddVehicleToTrip?cid=" + ddId;
    }
  
}
function format(item) {
    // `d` is the original data object for the row
    //alert(JSON.stringify(d.CASO));
    var InnerGrid = '<div class="table-scrollable"><table class="table display product-overview mb-30"><thead><tr><th>Sr. No.</th> <th> Geo Point </th><th> Point Category </th><th>Shift Name</th><th> Pickup Time </th><th> Zone </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;

        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td>' + info.GeoPointName + '</td>' +
            '<td>' + info.GeoPointCategory + '</td>' +
            '<td>' + info.ShiftName + '</td>' +
            '<td>' + info.SPickupTime + '</td>' +
            '<td>' + info.ZoneNo + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}

function ShowMapPopup(objthis) {

    var RTDId = $(objthis).attr('data-RTDId');

    $.ajax({
        url: "/Collection/GetAllNStopByTripId",
        type: "POST",
        dataType: "json",
        data: { RTDId: RTDId },
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
        if (locations[i].PointCatId==5) {
            contentString = "<div style='float:right; padding: 10px;font-size: 18px;font-weight:bold;background-color: #1ab394;color: white;'>Geo Point -" + locations[i].StopName +
                "<br/>Pickup Time -" + locations[i].SPickupTime +
                "<br/>Route Trip Code -" + locations[i].TId +
                "<br/>Vehicle No -" + locations[i].VehicleNo +
                "<br/>Start Point" +
                "</div>";

            marker = new google.maps.Marker({
                position: myLatlng,//new google.maps.LatLng(locations[i].Lat, locations[i].Lng),
                map: map,
                icon: iconBase + 'tYellow.png',
                content: contentString
            });
        }
        else if (locations[i].PointCatId == 6) {
            contentString = "<div style='float:right; padding: 10px;font-size: 18px;font-weight:bold;background-color: #1ab394;color: white;'>Geo Point -" + locations[i].StopName +
                "<br/>Pickup Time -" + locations[i].SPickupTime +
                "<br/>Route Trip Code -" + locations[i].TId +
                "<br/>Vehicle No -" + locations[i].VehicleNo +
                "<br/>End Point" +
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
                "<br/>Route Trip Code -" + locations[i].TId +
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
function DownloadFile(FType) {


    var IsReport = 0;
    //var VisitType = '';
    if (FType == 'Excel') {

       // VehicleUid = $('#ddlVehicleNumber').val();
        IsClick = 1;
       // PointId = document.getElementById('ddlPointName').value;

    }


    var TName = "All Trip Point  Report";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Collection/ExportAllTripPoint",
        data: { FName: TName, IsReport: IsReport},
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