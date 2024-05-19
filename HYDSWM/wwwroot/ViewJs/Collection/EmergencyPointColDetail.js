$(document).ready(function () {

    $('#txtFromDate').datetimepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });
    $('#txtToDate').datetimepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });

    var date = new Date();
    document.getElementById("txtFromDate").value = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + '00:00';
    document.getElementById("txtToDate").value = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + date.getHours() + ':' + date.getMinutes();
    AllAssignedVehicleLst('ddlVehicleNumber', 0, 'All VehicleNumber')
    GetDataTableData('Load');
   // GetPointsName();
    //SetValues('Load');
});

var IsClick = 0;
var dt;
function GetDataTableData(Type) {



    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var VehicleUid = document.getElementById('ddlVehicleNumber').value;
  
    // var VehicleUid = $('#ddlVehicleNumber').val();

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
            url: "/Collection/EmerGetAllPointCollectionDetail_Paging/",
            type: 'POST',
            data: function (d) {
                d.FromDate = FDate;
                d.ToDate = TDate;
                d.VehicleUid = VehicleUid
               
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

            { data: "VehicleNo", sortable: true },
            
            
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    if (row.PointName == 'NEW POINT')
                        return '<span >' + row.PointName + '</span>';
                    else
                        return '<span >' + row.PointName + '</span>';
                }
            },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    if (row.Status == 'COLLECTED')
                        return '<span class="badge badge-success">' + row.Status + '</span>';
                    else
                        return '<span class="badge badge-danger">' + row.Status + '</span>';
                }
            },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a data-Location='" + row.Address +
                        "'data-PickDTime='" + row.PickDTime +
                        "'data-Status='" + row.Status +
                        "'data-Lat='" + row.Lat +
                        "'data-Lng='" + row.Lng +
                        "'href=javascript:void(0)  onclick=ShowMapPopup(this);  >" + row.Address + "</a>";
                }
            },
            { data: "PickDTime", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.PClId + '" href="' + row.Img1Url + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Url + '" alt="" class="img-preview rounded"></a>';
                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.PClId + '" href="' + row.Img2Url + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img2Url + '" alt="" class="img-preview rounded"></a>';
                }
            }
        ]
    });


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


function DownloadFile(FType) {


    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
   
   // var Status = '-1';
    var VehicleUid = '0';
   
    //var VisitType = '';
    if (FType == 'Excel') {
       
        VehicleUid = $('#ddlVehicleNumber').val();
        IsClick = 1;
        Status = $('#ddlStatus').val();
       
    }



    var TName = "Emergency GeoPoint Visit Collection Report";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Collection/ExportAllEmerGeoPointsVisitSummary",
        data: { FromDate: FDate, ToDate: TDate, VehicleUid: VehicleUid,  FName: TName },
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