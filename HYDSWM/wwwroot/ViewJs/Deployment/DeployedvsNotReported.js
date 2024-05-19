$(document).ready(function () {
    $('#txtFromDate').datepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });
    var date = new Date();
    document.getElementById("txtFromDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();

    AllMZoneLst('ddlSZone', 0, 'All Zone');
    AllDVehicleTypeLst('ddlSVehicleType', 0, 'All Vehicle Type');
    AllAssetStatusLst('ddlSStatus', 0, 'All');

    CallFunc1('Load');


});
function AllDVehicleTypeLst(ControlId, IsRequired, Category) {

    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Asset/GetAllVehicleTypeByLogin",
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
function CallSCircleByZone() {
    $('#ddlSCircle').val('0');
    AllMCircleLst('ddlSCircle', 0, 'All Circle', $('#ddlSZone').find(":selected").attr('value'));
}
function CallSWardByCircle() {
    AllMWardLst('ddlSWard', 0, 'All Ward', $('#ddlSCircle').find(":selected").attr('value'));
}
function CallFunc1(Type) {
    SetDataBind(Type);
}

var chartdata;
var GTotalVehicle = 0;
var GDeployedVehicle = 0;
var GNotReportedVehicle = 0;
function CallAllFunc1() {

    SetDataBind();

}


function SetDataBind(Type) {
    ShowLoading($('#example1'));
    var VehicleTypeId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    var SDate = $('#txtFromDate').val();


    if (Type == 'Click') {
        /*ShowLoading($('#example1'));*/
        VehicleTypeId = $('#ddlSVehicleType').val();
        ZoneId = $('#ddlSZone').val();
        CircleId = $('#ddlSCircle').val();
        WardId = $('#ddlSWard').val();
        SDate = $('#txtFromDate').val();

    }

    var objval = {
        ZoneId: ZoneId,
        CircleId: CircleId,
        WardId: WardId,
        VehicleTypeId: VehicleTypeId,
        SDate: SDate,

    };

    $.ajax({
        type: "POST",
        url: '/Deployment/GetDeployedVsNotReportedSummary',
        dataType: "json",
        data: { Jobjval: JSON.stringify(objval) },
        success: function (result) {

            var myJSON = result;//JSON.parse(result);
            //chartdata = result.ZoneData;

            bindtable(myJSON);
            HideLoading($('#example1'));
            var topMatchTd;
            var previousValue = "";
            var rowSpan = 1;

        }
    });
}
function GetFile(FType) {


    var VehicleTypeId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';


    if (FType == 'Click') {

        VehicleTypeId = $('#ddlSVehicleType').val();
        ZoneId = $('#ddlSZone').val();
        CircleId = $('#ddlSCircle').val();
        WardId = $('#ddlSWard').val();

    }

    var objval = {
        ZoneId: ZoneId,
        CircleId: CircleId,
        WardId: WardId,
        VehicleTypeId: VehicleTypeId,



    };

    $.ajax({
        type: "POST",
        url: '/Demo/ExportVehicleSummaryData',
        //dataType: "json",
        data: { Jobjval: JSON.stringify(objval) },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {

            var ctype = '';
            //if (FType == 'Excel')
            //    ctype = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
            //else
            ctype = 'application/pdf';

            var filename = "GNIDA Report";//
            var blob = new Blob([response], { type: ctype });
            var DownloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = DownloadUrl;
            //if (FType == 'Excel')
            //    a.download = filename + ".xlsx";
            //else
            a.download = filename + ".pdf";
            document.body.appendChild(a);
            a.click();
        },
        error: function (result) {
        }
    });
}
function ClickForRedirection(objthis) {

    var Typeid = $(objthis).attr('Typeid');
    var vtypeid = $(objthis).attr('vtypeid');
    var zid = $(objthis).attr('zid');
    var cirid = $(objthis).attr('cirid');
    var wardid = $(objthis).attr('wardid');
    var stypeid = $(objthis).attr('stypeid');
    var SDate = $('#txtFromDate').val();


    var url = "/Deployment/DeployedVsNotReported_Paging?vtypeid=" + vtypeid + "&zid=" + zid + "&Typeid=" + Typeid + "&cirid=" + cirid + "&wardid=" + wardid + "&stypeid=" + stypeid + "&SDate=" + SDate;
    window.open(url, "_blank");
}

function bindtable(data) {

    var Zonedata = data.ZoneData;
    var vehicletypedata = data.VehicleoutData;
    
    $('#example1 tbody').empty();
    $('#example1 tfoot').empty();


    if ($('#hfTotalrows1').val() > 0) {
        $('#example1').DataTable().clear().destroy();

        // $('#example1 tbody > tr').remove();
    }

    for (i = 0; i < Zonedata.length; i++) {

        var count = i + 1;
        var IsNewZ = 0;

        var Zcount = Zonedata[i].TotalZRowCount;//vehicletypedata.length;
        

        for (j = 0; j < Zonedata[i].Circlelst.length; j++) {
            var isnewCircle = 0;
            var CCount = Zonedata[i].Circlelst[j].TotalCircleRowCount;

            for (k = 0; k < Zonedata[i].Circlelst[j].Wardlst.length; k++) {
                var isnewWard = 0;
                var WCount = Zonedata[i].Circlelst[j].Wardlst[k].TotalWardRowCount;

                for (l = 0; l < Zonedata[i].Circlelst[j].Wardlst[k].VehicleData.length; l++) {
                    var zoneCodeCellContent = Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId;

                    // Add a class to the <td> element to style it using CSS
                    var zoneCodeCellClass = zoneCodeCellContent === 0 ? "bold-text" : "";

                    var rows = '';
                    rows += "<tr  class='" + zoneCodeCellClass + "'>";
                    if (IsNewZ == 0) {
                        rows += "<td rowspan='" + Zcount + "' style='border: 1px solid black; text - align: center;'>" + Zonedata[i].ZoneNo + "</td>";
                        IsNewZ = 1;
                    }
                    if (isnewCircle == 0) {
                        rows += "<td rowspan='" + CCount + "' style='border: 1px solid black; text - align: center;'>" + Zonedata[i].Circlelst[j].CircleCode + "</td>";
                        isnewCircle = 1;
                    }
                    if (isnewWard == 0) {
                        rows += "<td rowspan='" + WCount + "' style='border: 1px solid black; text - align: center;'>" + Zonedata[i].Circlelst[j].Wardlst[k].WardName + "</td>";
                        isnewWard = 1;
                    }
                   
                    if (Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleType == 0) {
                        rows += "<td style='border: 1px solid black; text-align: center;'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleType + "</td>";
                        rows += "<td onclick='ClickForRedirection(this);'  style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='1'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Deployed  + "</td>";
                        rows += "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='2'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].NotReported + "</td>";
                        

                        rows += "</tr>";

                    }
                    else {
                        
                        GDeployedVehicle = GDeployedVehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Deployed;
                        GNotReportedVehicle = GNotReportedVehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].NotReported;

                        rows += "<td style='border: 1px solid black; text - align: center;'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleType + "</td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='1'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Deployed + "</a></td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='2'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].NotReported + "</a></td>";
                        
                        rows += "</tr>";
                    }


                    $('#example1 tbody').append(rows);
                }


            }
        }





    }
    
    var footerRow = '<tfoot>' +
        '<tr class="bold-text">' +
        '<td colspan="4" style=" border: 1px solid black; text-align:left;">Grand Total:</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GDeployedVehicle + '</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GNotReportedVehicle + '</td>' +
        '</tr>' +
        '</tfoot>';

    $('#example1').append(footerRow);
    //$('#example1').DataTable();
    //var rowcount = Zonedata.length;
    //var IsNew = 0


}
//var dt12;
//var dt11;
//function setdatatable() {

//    dt11 = $('#example1').DataTable({

//        "responsive": true,
//        "pageLength": 1000000,
//        // "order": [[1, "asc"]],
//        //"ordering":false, 
//        rowsGroup: [1, 2],
//        "paging": false,
//        "destroy": true,

//        lengthMenu: [
//            [10, 25, 50, 500, 1000, 5000, -1],
//            ['10 rows', '25 rows', '50 rows', '500 rows', '1000 rows', '5000 rows', "All"]
//        ],

//        language: {
//            infoEmpty: "No records available",
//        },

//        dom: 'Blfrtip',
//        buttons: {
//            buttons: [


//            ]
//        },

//        "footerCallback": function (row, data, start, end, display) {

//            var api = this.api();

//            // Function to convert a value to an integer, accounting for commas and dollar signs
//            var intVal = function (i) {
//                return typeof i === 'string' ?
//                    i.replace(/[\$,]/g, '') * 1 :
//                    typeof i === 'number' ?
//                        i : 0;
//            };

//            // Initialize variables to store the totals for each day of the week
//            var monTotal = 0;
//            var tueTotal = 0;
//            var wedTotal = 0;
//            var thuTotal = 0;
//            var friTotal = 0;
//            var friTotal1 = 0;
//            var friTotal2 = 0;
//            var friTotal3 = 0;

         
//            // Update footer by showing the total with the reference of the column index 
//            $(api.column(0).footer()).html('Grand Total');
//            $(api.column(1).footer()).html('');
//            $(api.column(2).footer()).html('');
//            $(api.column(3).footer()).html('');
//            $(api.column(4).footer()).html(GDeployedVehicle);
//            $(api.column(5).footer()).html(GNotReportedVehicle);
//            //$(api.column(6).footer()).html(GNotDeployedVehicle);
//            //$(api.column(7).footer()).html(Deppercentage.toFixed(2) + " %");


//        },


//    });
//    //  $('#example1 thead').addClass('fixed-header');
//    // new $.fn.dataTable.FixedHeader(dt11);

//}

function DownloadFile(FType) {

    var FType = 'Excel';
    var VehicleTypeId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    var SDate = $('#txtFromDate').val();



    VehicleTypeId = $('#ddlSVehicleType').val();
    ZoneId = $('#ddlSZone').val();
    CircleId = $('#ddlSCircle').val();
    WardId = $('#ddlSWard').val();
    SDate = $('#txtFromDate').val();



    var TName = "Deployed Vs Not Reported";

    ShowLoading($('#example'));

    $.ajax({
        type: "POST",
        url: "/Deployment/ExportDeployedVsNotReported",
        data: { VehicleTypeId: VehicleTypeId, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, SDate: SDate, FName: TName },
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

