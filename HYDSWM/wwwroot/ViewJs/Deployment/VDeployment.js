$(document).ready(function () {
    var date = new Date();
    document.getElementById("txtFromDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    document.getElementById("txtToDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();

    $('#txtFromDate').datepicker({
        changeMonth: true,
        changeYear: true,
        //maxDate: '0',
        autoclose: true,
        endDate: document.getElementById("txtFromDate").value,
    });
    $('#txtToDate').datepicker({
        changeMonth: true,
        changeYear: true,
        // maxDate: '0'
        autoclose: true,
        endDate: document.getElementById("txtToDate").value,
    });
    AllDVehicleTypeLst('ddlVehicleType', 0, 'All Vehicle Type')

    AllMZoneLst('ddlZone', 0, 'All Zone');
    //AllEntityLst();
    GetDataTableData('Load');
    GetDataTableDataSAT('Load');
    //$('#example tbody').on('click', 'td.details-control', function () {

    //    var tr = $(this).closest('tr');
    //    var row = $('#example').DataTable().row(tr);
    //    var TDate = tr.find('.gticket').attr('tdate');
    //    var UId = tr.find('.gticket').attr('uid');
    //    var EntityType = tr.find('.gticket').attr('entitytype');
    //    var VehicleTypeId = tr.find('.gticket').attr('vtypeid');


    //    if (row.child.isShown()) {
    //        // This row is already open - close it
    //        row.child.hide();
    //        tr.removeClass('shown');
    //    }
    //    else {
    //        // Open this row
    //        var iccData;
    //        $.ajax({
    //            url: "/Deployment/GetEntityTrans",
    //            type: "POST",
    //            dataType: "json",
    //            //data: dataS,
    //            data: { TDate: TDate, UId: UId, EntityType: EntityType, VehicleTypeId: VehicleTypeId },
    //            success: function (data) {

    //                var myJSON = JSON.parse(data);
    //                row.child(format(myJSON)).show();
    //                tr.addClass('shown');
    //            }
    //        });

    //    }


    //});
    $('#profile-tab').on('click', function (e) {

        $("#btnExcelSAT").show();
        $("#btnExcel").hide();

    });
    $('#home-tab').on('click', function (e) {

        $("#btnExcelSAT").hide();
        $("#btnExcel").show();
    });

});

function CSVData(FType) {
    
    // Assuming you have jQuery loaded
    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';

    var VehicleUid = '0';
    var etype = '';
    if (IsClick == 1) {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        IsClick = 1;
        VehicleUid = $('#ddlEntity').val();
        VehicleTypeId = $('#ddlVehicleType').val();
        //VehicleUid = info != '' ? info.split('-')[0] : '';
        //etype = info != '' ? info.split('-')[1] : '';
    }
    var TName = "All Vehicle Deployment  Report";
    $.ajax({
        url: '/Deployment/ExportCSV', // URL of your controller action
        method: 'POST',
        data: { FromDate: FDate, ToDate: TDate, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleUid: VehicleUid, NotiId: 0, FName: TName, VehicleTypeId: VehicleTypeId },
        success: function (csvData) {
           
            // For downloading the CSV as a file
            var blob = new Blob([csvData], { type: 'text/csv' });
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = TName+ '.csv';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });


}
function SATCSVTrips(FType) {
    
    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';

    var VehicleUid = '0';
    var etype = '';
    if (IsClick == 1) {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        IsClick = 1;
        VehicleUid = $('#ddlEntity').val();
        VehicleTypeId = $('#ddlVehicleType').val();
        //VehicleUid = info != '' ? info.split('-')[0] : '';
        //etype = info != '' ? info.split('-')[1] : '';
    }
    var TName = "SAT Trips  Report";
    $.ajax({
        url: '/Deployment/ExportSATCSV', // URL of your controller action
        method: 'POST',
        data: { FromDate: FDate, ToDate: TDate, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleUid: VehicleUid, NotiId: 0, FName: TName, VehicleTypeId: VehicleTypeId },
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
function AllDVehicleTypeLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Deployment/GetAllVehicleTypeByLogin",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            $("#ddlVehicleType").select2({

            });
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].VehicleTypeId + '>' + Myjson[i].VehicleType + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllVehicleLst() {
    $("#ddlEntity").html();
    var vehicletypeid = $("#ddlVehicleType").val();
    $.ajax({
        type: "post",
        url: "/Master/GetAllVehicleNumber",
        data: { VehicleTypeId: vehicletypeid },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            $("#ddlEntity").select2({

            });
            var Resource = "<select id='ddlEntity' class='form-control'>";

            Resource = Resource + '<option value="0">All Vehicle</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].UId + '>' + Myjson[i].VehicleNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlEntity').html(Resource);
        }
    });
}
function format(item) {
    // `d` is the original data object for the row
    //alert(JSON.stringify(d.CASO));
    var InnerGrid = '<div class="table-scrollable"><table class="table display product-overview mb-30"><thead><tr><th>Sr. No.</th> <th> Transfer Station Name </th><th>Operation Type </th><th> Entity Type</th><th> Entity No</th><th> Date & Time</th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;
        var status = ''
        // var dateS = info.PickupTime.split('T');


        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td>' + info.TStationName + '</td>' +
            '<td>' + info.OperationType + '</td>' +

            '<td>' + info.EntityType + '</td>' +
            '<td>' + info.EntityNo + '</td>' +
            '<td>' + info.CreatedOn + '</td>' +
            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}

function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'Select', $('#ddlCircle').find(":selected").attr('value'));
}
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}
var IsClick = 0;
var dt;

function GetDataTableData(Type) {
    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';
    var info = '0';
    var VehicleUid = '';
    var etype = '';
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        IsClick = 1;
        info = $('#ddlEntity').val();
        VehicleTypeId = $('#ddlVehicleType').val();
        //VehicleUid = info != '' ? info.split('-')[0] : '';
        //etype = info != '' ? info.split('-')[1] : '';
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
            url: "/Deployment/GetAllVDeployment_Paging/",
            type: 'POST',
            data: function (d) {

                d.FromDate = FDate;
                d.ToDate = TDate;
                d.WardId = WardId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.VehicleUid = info;
                d.VehicleTypeId = VehicleTypeId;
                d.NotiId = 0;
                d.ContratorId = "0";
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

                    return '<a  href="' + row.Img1Url + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Url + '" alt="" class="img-preview rounded"></a>';
                }
            },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    return '<span class="gticket" vtypeid="' + row.VehicleTypeId + '" uid="' + row.UId + '"  entitytype="' + row.EntityType + '" tdate="' + row.CreatedDate + '" >' + row.EntityNo + '</span>';
                }
            },
            { data: "UId", sortable: true },
            { data: "VEntityNo", sortable: true },
           
            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "WardNo", sortable: true },
            { data: "LandMark", sortable: true },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a data-EntityNo='" + row.EntityNo +
                        "'data-VEntityNo='" + row.VEntityNo +
                        "'data-Address='" + row.Address +
                        "'data-ZoneNo='" + row.ZoneNo +
                        "'data-WardNo='" + row.WardNo +
                        "'data-CircleName='" + row.CircleName +
                        "'data-ShiftName='" + row.ShiftName +
                        "'data-Lat='" + row.Lat +
                        "'data-Lng='" + row.Lng +
                        "'href=javascript:void(0)  onclick=ShowMapPopup(this);  >" + row.Address + "</a>";


                }
            },
            /* { data: "Remarks", sortable: true },*/
            { data: "OwnerType", sortable: true },
            { data: "CreatedOn", sortable: true },
            //{ data: "ShiftName", sortable: true },
            { data: "CreatedBy", sortable: true },

        ]
    });


}
function GetDataTableDataSAT(Type) {
    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';
    var info = '0';
    var VehicleUid = '';
    var etype = '';
    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        IsClick = 1;
        info = $('#ddlEntity').val();
        VehicleTypeId = $('#ddlVehicleType').val();

    }
    $('#example1').DataTable().clear().destroy();
    dt = $('#example1').DataTable({
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
            url: "/Deployment/GetAllSATTrips_Paging/",
            type: 'POST',
            data: function (d) {

                d.FromDate = FDate;
                d.ToDate = TDate;
                d.WardId = WardId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.VehicleUid = info;
                d.VehicleTypeId = VehicleTypeId;
                d.NotiId = 0;
                d.ContratorId = "0";
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

            { data: "TStationName", sortable: true },
            { data: "VehicleType", sortable: true },
            { data: "VehicleNo", sortable: true },
            { data: "ReportedOn", sortable: true },

        ]
    });


}
function ShowMapPopup(objthis) {

    var lat = $(objthis).attr('data-Lat');
    var lng = $(objthis).attr('data-Lng');
    var EntityNo = $(objthis).attr('data-EntityNo');
    var Location = $(objthis).attr('data-Address');
    var WardNo = $(objthis).attr('data-WardNo');
    var ZoneNo = $(objthis).attr('data-ZoneNo');
    var CircleName = $(objthis).attr('data-CircleName');
    var EntityType = $(objthis).attr('data-VEntityNo');
    var ShiftName = $(objthis).attr('data-ShiftName');
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


    contentString = "<div style='float:right; padding: 10px;font-size: 14px;background-color: #33414E;color: white;'>Entity No-" + EntityNo +
        "<br/>Entity Type-" + EntityType +
        "<br/>Location-" + Location +
        "<br/>Zone-" + ZoneNo +
        "<br/>Circle-" + CircleName +
        "<br/>Ward-" + WardNo +
        "<br/>Actual Shift-" + ShiftName +
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

function AllEntityLst() {
    $("#ddlEntity").html();
    $.ajax({
        type: "post",
        url: "/Deployment/GetAllEntityInfo",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='ddlEntity' class='form-control'>";
            Resource = Resource + '<option value="">All Entity</option>';
            for (var i = 0; i < Myjson.length; i++) {
                var ival = Myjson[i].UId + "_" + Myjson[i].EntityType;
                Resource = Resource + '<option value=' + ival + '>' + Myjson[i].EntityNo + '</option>';

            }
            Resource = Resource + '</select>';
            $('#ddlEntity').html(Resource);
        }
    });
}

function DownloadFile(FType) {

    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';

    var VehicleUid = '0';
    var etype = '';
    if (IsClick == 1) {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        IsClick = 1;
        VehicleUid = $('#ddlEntity').val();
        VehicleTypeId = $('#ddlVehicleType').val();
        //VehicleUid = info != '' ? info.split('-')[0] : '';
        //etype = info != '' ? info.split('-')[1] : '';
    }
    var TName = "All Vehicle Deployment  Report";

    ShowLoading($('#myTab'));
    $.ajax({
        type: "POST",
        url: "/Deployment/ExportAllVDeployment",
        data: { FromDate: FDate, ToDate: TDate, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleUid: VehicleUid, NotiId: 0, FName: TName, VehicleTypeId: VehicleTypeId },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#myTab'));
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
            HideLoading($('#myTab'));
        }
    });
}
function DownloadFileSAT(FType) {

    var FDate = document.getElementById('txtFromDate').value; //
    var TDate = document.getElementById('txtToDate').value;//
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';

    var VehicleUid = '0';
    var etype = '';
    if (IsClick == 1) {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        IsClick = 1;
        VehicleUid = $('#ddlEntity').val();
        VehicleTypeId = $('#ddlVehicleType').val();
        //VehicleUid = info != '' ? info.split('-')[0] : '';
        //etype = info != '' ? info.split('-')[1] : '';
    }
    var TName = "All SAT Trips  Report";

    ShowLoading($('#myTab'));
    $.ajax({
        type: "POST",
        url: "/Deployment/ExportAllSATTrips",
        data: { FromDate: FDate, ToDate: TDate, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleUid: VehicleUid, NotiId: 0, FName: TName, VehicleTypeId: VehicleTypeId },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#myTab'));
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
                a.download = filename + ".csv";
            document.body.appendChild(a);
            a.click();
        },
        error: function (result) {
            HideLoading($('#myTab'));
        }
    });
}