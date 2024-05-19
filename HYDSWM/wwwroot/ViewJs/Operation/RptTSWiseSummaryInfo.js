$(document).ready(function () {
    //$('#ddlVehicleType').multiselect({
    //    selectAllValue: 'multiselect-all',
    //    maxHeight: '300',
    //    buttonWidth: '235',
    //    numberDisplayed: 2,
    //    // nonSelectedText: 'Select Vehicle',
    //    //includeSelectAllOption: true,
    //    enableFiltering: true,
    //    enableCaseInsensitiveFiltering: true
    //});

    $('#example').DataTable().clear().destroy();
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
    AllMZoneLst('ddlZone', 0, 'All Zone');
    //GetAllVehicleType();
   AllVehicleTypeLst('ddlVehicleType', 0, 'All Vehicle Type');
    GetAllTransferStation();
    SetValues('Load');

});
function GetAllVehicleType() {
    $("#ddlVehicleType").empty();
    $('#ddlVehicleType').multiselect('destroy');

    $.ajax({
        type: "post",
        url: "/Master/GetAllVehicleType",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            
            var Myjson = JSON.parse(data);
            
            //$("#ddlVehicleType").empty();
            //var Resource = "<select id='ddlVehicleType' class='form-control'>";
            //for (var i = 0; i < Myjson.length; i++) {
            //    Resource = Resource + '<option value=' + Myjson[i].VehicleTypeId + '>' + Myjson[i].VehicleType + '</option>';
            //}
            //Resource = Resource + '</select>';
            $('#ddlVehicleType').html(Resource);
            $.each(Myjson, function (index, item) {
                $("#ddlVehicleType").append($("<option></option>").val(item.VehicleTypeId).html(item.VehicleType));
            });
            $('#ddlVehicleType').multiselect({
                selectAllValue: 'multiselect-all',
                enableCaseInsensitiveFiltering: true,
                enableFiltering: true,
                maxHeight: '300',
                buttonWidth: '235',
                numberDisplayed: 2,
                nonSelectedText: 'Select Vehicle Type',
            });
        }
    });
}
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}

var IsClick = 0;
function DownloadFile(FType) {

    var zid = "0";
    var cid = "0";
    var VId = "0";
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var TSId = "0";

    if (IsClick == 1) {
        zid = $('#ddlZone').val();//$('#ddlDesignation').find(":selected").attr('value') == undefined ? "0" : $('#ddlDesignation').find(":selected").attr('value');
        cid = $('#ddlCircle').val();
        VId = $('#ddlVehicleType').val();
        TSId = $('#ddlTransferStation').val();
    }

    var TName = "Operation Summary";

    ShowLoading($('#dvNotification'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportEmployeeAttendanceData",
        data: { zid: zid, cid: cid, VehicleTypeId: VId, TSId: TSId, FromDate: FDate, ToDate: TDate, FName: TName },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#dvNotification'));
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
            HideLoading($('#dvNotification'));
        }
    });
}

function GetAllTransferStation() {
    $('#ddlTransferStation').html('');

    $.ajax({
        type: "post",
        url: "/Operation/GetAllTransferStationByUser",
        data: '{}',
        success: function (data) {
            var myJSON = JSON.parse(data);
            var Resource = "<select id='ddlTransferStation'>";
            Resource = Resource + '<option value="0">All Transfer Station</option>';
            // Resource = Resource + '<option value="0">N/A</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource = Resource + '<option value=' + myJSON[i].TSId + '>' + myJSON[i].TStationName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlTransferStation').html(Resource);
        }

    });
}

function SetValues(Type) {
    var zid = "0";
    var cid = "0";
    var VId = "0";
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;
    var TSId = "0";

    if (Type == 'Click') {
        IsClick = 1;
        zid = $('#ddlZone').val();//$('#ddlDesignation').find(":selected").attr('value') == undefined ? "0" : $('#ddlDesignation').find(":selected").attr('value');
        cid = $('#ddlCircle').val();
        VId = $('#ddlVehicleType').val();
        TSId = $('#ddlTransferStation').val();
    }
    ShowLoading($('#dvContent'));
    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: '/Operation/GetMDashboardLst',
        dataType: "json",
        data: { zid: zid, cid: cid, VehicleTypeId: VId, TSId: TSId, FromDate: FDate, ToDate: TDate },
        success: function (data) {
            var myJSON = JSON.parse(data);
            bindtable(myJSON);
            HideLoading($('#dvContent'));
        },
        error: function (result) {
            HideLoading($('#dvContent'));
        }
    });
}
function bindtable(data) {

    $('#example tbody').empty();

    if ($('#hfTotalrows').val() > 0)
        $('#example').DataTable().clear().destroy();

    var rowcount = data.length;
    $.each(data, function (i, item) {
        var count = i + 1;
        var variance = ''
        var edit = "<td><a cid='" + item.TSId + "' href='javascript: void (0); ' title='Export To Excel' onclick='CallFunc(this); '><i class='ti-download'></i></a></td>";
        if (item.WTVariance > 0)
            variance = "<td><span class='badge badge-success'><i class='icon-arrow-up8'></i>" + item.WTVarVal + "</span></td>";
        else if (item.WTVariance < 0)
            variance = "<td><span class='badge badge-danger'><i class='icon-arrow-down8'></i>" + item.WTVarVal + "</span></td>";
        else
            variance = "<td><span class='badge badge-info'>" + item.WTVarVal + "</span></td>";

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<td>" + item.TSName +
            "</td>" + "<td>" + item.TotalPrimaryVehicle +

            "</td>" + "<td>" + item.TotalContainer +
            "</td>" + "<td>" + item.TotalHookLoaderTrip +
            "</td>" + "<td>" + item.TotalOpenVehicleTrip +
            "</td>" + "<td>" + item.TotalRCVVehicle +
            "</td>" + "<td>" + item.SecondaryVehicleWT +
            "</td>" + "<td>" + item.TotalWBWeight +
            "</td>" + variance +
            "</td>" + edit +
            "</tr>";
        $('#example tbody').append(rows);


    });

    //var tabid = $('#example');
    $('#hfTotalrows').val(rowcount);
    if ($('#hfTotalrows').val() > 0)
        setdatatableoncontrol('example');

}

function CallFunc(obj) {
    var TSId = $(obj).attr('cid');
    var zid = "0";
    var cid = "0";
    var VId = "0";
    var FDate = document.getElementById('txtFromDate').value;
    var TDate = document.getElementById('txtToDate').value;

    if (IsClick == 1) {
        zid = $('#ddlZone').val();//$('#ddlDesignation').find(":selected").attr('value') == undefined ? "0" : $('#ddlDesignation').find(":selected").attr('value');
        cid = $('#ddlCircle').val();
        VId = $('#ddlVehicleType').val();
    }
    var TName = "Transfer Station Summary";

    ShowLoading($('#dvNotification'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportDataLogByTS",
        data: { zid: zid, cid: cid, VehicleTypeId: VId, TSId: TSId, FromDate: FDate, ToDate: TDate, FName: TName },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#dvNotification'));
            var ctype = '';
            FType = 'Excel';
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
            HideLoading($('#dvNotification'));
        }
    });
}
