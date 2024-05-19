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
    GetPointsName();

    AllMZoneLst('ddlZone', 0, 'All Zone');
    // AllMCircleLst('ddlCircle', 0, 'All Circle');
    //AllMWardLst('ddlWard', 0, 'All Ward');
    //AllMTripLst('ddlTrip', 0, 'All Trip');
    AllMActiveRouteLst('ddlRoute', 0, 'All Route');
    //SetValues('Load');
});
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}
function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'Select', $('#ddlCircle').find(":selected").attr('value'));
}
var IsClick = 0;
var dt;
function GetDataTableData(Type) {
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var VehicleUid = document.getElementById('ddlVehicleNumber').value;
    var PointId = document.getElementById('ddlPointName').value;
    var  RouteId = $('#ddlRoute').val();
    var  ZoneId = $('#ddlZone').val();
    var  CircleId = $('#ddlCircle').val();
    var WardId = $('#ddlWard').val();
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
            url: "/Collection/GetAllPointCollectionDetail_Paging/",
            type: 'POST',
            data: function (d) {
                d.FromDate = FDate;
                d.ToDate = TDate;
                d.VehicleUid = VehicleUid
                d.NotiId = PointId
                d.Route = RouteId;
                d.WardId = WardId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
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
            { data: "RouteCode", sortable: true },
            { data: "TripName", sortable: true },
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
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    // Assuming you have a property called "Link" in your data
                    var link = 'http://maps.google.co.in/maps?q=' + row.GeoCordinate;

                    // Create the link HTML with target="_blank"
                    var linkHTML = '<a href="' + link + '" target="_blank">' + row.GeoCordinate + '</a>';

                    // Return the HTML for the column
                    return linkHTML;
                }
            },
            { data: "ShiftName", sortable: true },
            { data: "ShiftDate", sortable: true },
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

function GetPointsName() {
    var IsRequired = 0;
    var Category = "Select Actual Point";

    $.ajax({
        type: "post",
        url: "/Collection/GetAllDPointsName",
        // data: { RouteTripCode: TripName, SDate: document.getElementById('txtFromDate').value },
        data: '{}',
        datatype: "json", //GeoPointId, GeoPointName
        traditional: true,
        success: function (data) {



            //var Myjson = JSON.parse(data);
            MyjsonMain = data.data;
            var Resource = "<select id='ddlPointName' class='form-control'>";
            $("#ddlPointName").select2({

            });
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select Actual Point</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            // var Myjson = JSON.parse(data);
            var Resource = "<select id='ddlPointName' class='form-control'>";
            Resource = Resource + '<option value="">Select Actual Point</option>';

            for (var i = 0; i < MyjsonMain.length; i++) {
                Resource = Resource + '<option value=' + MyjsonMain[i].GeoPointId + '>' + MyjsonMain[i].GeoPointName + '</option>';
            }
            Resource = Resource + '</select>';
            $("#ddlPointName").html(Resource);

        }
    });
}

function DownloadFile(FType) {


    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var PointId = '0';
    // var Status = '-1';
    var VehicleUid = '0';

    //var VisitType = '';
    if (FType == 'Excel') {

        VehicleUid = $('#ddlVehicleNumber').val();
        IsClick = 1;
        PointId = document.getElementById('ddlPointName').value;

    }


    var TName = "All Collection Point  Report";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Collection/ExportAllCollectionPoints",
        data: { FromDate: FDate, ToDate: TDate, VehicleUid: VehicleUid, PointId: PointId, FName: TName },
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

function CSVData(FType) {
    
    // Assuming you have jQuery loaded
    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var PointId = '0';
    // var Status = '-1';
    var VehicleUid = '0';

    //var VisitType = '';
    if (FType == 'Excel') {

        VehicleUid = $('#ddlVehicleNumber').val();
        IsClick = 1;
        PointId = document.getElementById('ddlPointName').value;

    }


    var TName = "All Collection Point  Report";


    $.ajax({
        url: '/Collection/ExportAllCollectionCSV', // URL of your controller action
        method: 'POST',
        data: { FromDate: FDate, ToDate: TDate, VehicleUid: VehicleUid, PointId: PointId, FName: TName },
        success: function (csvData) {

            // For downloading the CSV as a file
            var blob = new Blob([csvData], { type: 'text/csv' });
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = TName + '.csv';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });


}

//$(document).on("click", "#btnExcel", function () {
//    
//    var htmltable = document.getElementById('example');
//    var html = htmltable.outerHTML;
//    window.open('data:application/vnd.ms-excel,' + encodeURIComponent(html));

//    //TableToExcel.convert(document.getElementById("example"), {
//    //    name: "Traceability.xlsx",
//    //    sheet: {
//    //        name: "Sheet1"
//    //    }
//    //});
//   // fnExcelReport();
//})

//function fnExcelReport() {
//    var tab_text = '<table border="1px" style="font-size:20px" ">';
//    var textRange;
//    var j = 0;
//    var tab = document.getElementById('example'); // id of table
//    var lines = tab.rows.length;

//    // the first headline of the table
//    if (lines > 0) {
//        tab_text = tab_text + '<tr bgcolor="#DFDFDF">' + tab.rows[0].innerHTML + '</tr>';
//    }

//    // table data lines, loop starting from 1
//    for (j = 1; j < lines; j++) {
//        tab_text = tab_text + "<tr>" + tab.rows[j].innerHTML + "</tr>";
//    }

//    tab_text = tab_text + "</table>";
//    tab_text = tab_text.replace(/<A[^>]*>|<\/A>/g, "");             //remove if u want links in your table
//    tab_text = tab_text.replace(/<img[^>]*>/gi, "");                 // remove if u want images in your table
//    tab_text = tab_text.replace(/<input[^>]*>|<\/input>/gi, "");    // reomves input params
//    // console.log(tab_text); // aktivate so see the result (press F12 in browser)

//    var ua = window.navigator.userAgent;
//    var msie = ua.indexOf("MSIE ");

//    // if Internet Explorer
//    if (msie > 0 || !!navigator.userAgent.match(/Trident.*rv\:11\./)) {
//        txtArea1.document.open("txt/html", "replace");
//        txtArea1.document.write(tab_text);
//        txtArea1.document.close();
//        txtArea1.focus();
//        sa = txtArea1.document.execCommand("SaveAs", true, "DataTableExport.xls");
//    }
//    else // other browser not tested on IE 11
//        sa = window.open('data:application/vnd.ms-excel,' + encodeURIComponent(tab_text));

//    return (sa);
//}
