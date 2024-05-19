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
    var dt11;

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
var GNotDeployedVehicle = 0;
var GTrips1Vehicle = 0;
var GTrips2Vehicle = 0;
var GTrips3Vehicle = 0;
var GTrips4Vehicle = 0;
var GMoreThan4TripsVehicle = 0;
var GReportedUniqueVehicle = 0;

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
        ShowLoading($('#example1'));
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
        url: '/Deployment/GetNotDeployedVsReportedSummary',
        dataType: "json",
        data: { Jobjval: JSON.stringify(objval) },
        success: function (result) {

            var myJSON = result;//JSON.parse(result);
            chartdata = result.ZoneData;

            bindtable(myJSON);
            HideLoading($('#example1'));
            var topMatchTd;
            var previousValue = "";
            var rowSpan = 1;

        }
    });
}
function GetFile(FType) {

    //var FDate = document.getElementById('txtFromDate').value;
    //var TDate = document.getElementById('txtToDate').value;

    ////var Fname = "GNIDA Report";

    //var Fname = " DATE - " + $("#txtFromDate").val();

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


    var url = "/Deployment/NotDeployedVsReportedPaging?vtypeid=" + vtypeid + "&zid=" + zid + "&Typeid=" + Typeid + "&cirid=" + cirid + "&wardid=" + wardid + "&stypeid=" + stypeid + "&SDate=" + SDate;
    window.open(url, "_blank");
}
function bindtable(data) {

    var Zonedata = data.ZoneData;
    var vehicletypedata = data.VehicleoutData;
    //var Result2 = data.Table1;
    //var Result3 = data.Table2;
    //var Result4 = data.Table3;
    //var Result5 = data.Table4;
    $('#example1 tbody').empty();
    $('#example1 tfoot').empty();


    if ($('#hfTotalrows1').val() > 0) {
        $('#example1').DataTable().clear().destroy();

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

                    //var totalVehicle = Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Deployed + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].NotDeployed;
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
                        rows += "<td onclick='ClickForRedirection(this);'  style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='1'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].NotDeployed + "</td>";
                        rows += "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='2'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].ReportedUnique + "</td>";
                        rows += "<td  onclick='ClickForRedirection(this);'  style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='3'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips1 + "</td>";
                        rows += "<td  onclick='ClickForRedirection(this);'  style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='4'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips2 + "</td>";
                        rows += "<td  onclick='ClickForRedirection(this);'  style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='5'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips3 + "</td>";
                        rows += "<td  onclick='ClickForRedirection(this);'  style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='6'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips4 + "</td>";
                        rows += "<td  onclick='ClickForRedirection(this);'  style='border: 1px solid black; text-align: center;' Typeid='1' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='7'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].MoreThan4Trips + "</td>";



                        rows += "</tr>";

                    }
                    else {
                        GNotDeployedVehicle = GNotDeployedVehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].NotDeployed;
                        GTrips1Vehicle = GTrips1Vehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips1;
                        GTrips2Vehicle = GTrips2Vehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips2;
                        GTrips3Vehicle = GTrips3Vehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips3;
                        GTrips4Vehicle = GTrips4Vehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips4;
                        GMoreThan4TripsVehicle = GMoreThan4TripsVehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].MoreThan4Trips;
                        GReportedUniqueVehicle = GReportedUniqueVehicle + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].ReportedUnique;

                        rows += "<td style='border: 1px solid black; text - align: center;'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleType + "</td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='1'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].NotDeployed + "</a></td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='2'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].ReportedUnique + "</a></td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='3'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips1 + "</a></td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='4'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips2 + "</a></td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='5'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips3 + "</a></td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='6'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Trips4 + "</a></td>";
                        rows += "<td style='border: 1px solid black; text-align: center;'><a href='' onclick='ClickForRedirection(this);' Typeid='0' vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='7'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].MoreThan4Trips + "</a></td>";


                        rows += "</tr>";
                    }


                    $('#example1 tbody').append(rows);
                }


            }
        }





    }
    /*var deppercentage = (GDeployedVehicle / GTotalVehicle) * 100*/
    var footerRow = '<tfoot>' +
        '<tr class="bold-text">' +
        '<td colspan="4" style=" border: 1px solid black; text-align:left;">Grand Total:</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GNotDeployedVehicle + '</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GReportedUniqueVehicle + '</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GTrips1Vehicle + '</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GTrips2Vehicle + '</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GTrips3Vehicle + '</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GTrips4Vehicle + '</td>' +
        '<td style="border: 1px solid black; text-align:center;">' + GMoreThan4TripsVehicle + '</td>' +
        '</tr>' +
        '</tfoot>';

    $('#example1').append(footerRow);
    //$('#example1').DataTable();
    //var rowcount = Zonedata.length;
    //var IsNew = 0


}


var dt12;
function setdatatable() {

    dt11 = $('#example1').DataTable({
        "destroy": true,
        "responsive": true,
        "pageLength": 200000,
        //"order": [[1, "asc"]],
        //"ordering":false,
        "paging": false,
        fixedHeader: true,
        fixedColumns: true,
        //scrollX: true,
        //processing: true,
        rowsGroup: [1, 2],
        lengthMenu: [
            [10, 25, 50, 500, 1000, 5000, -1],
            ['10 rows', '25 rows', '50 rows', '500 rows', '1000 rows', '5000 rows', "All"]
        ],

        language: {
            infoEmpty: "No records available",
        },

        dom: 'Blfrtip',
        buttons: {
            buttons: [


            ]
        },
        "footerCallback": function (row, data, start, end, display) {

            var api = this.api();

            // Function to convert a value to an integer, accounting for commas and dollar signs
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // Initialize variables to store the totals for each day of the week
            //var monTotal = 0;
            //var tueTotal = 0;
            //var wedTotal = 0;
            //var thuTotal = 0;
            //var friTotal = 0;
            //var friTotal1 = 0;
            //var friTotal2 = 0;
            //var friTotal3 = 0;

            //// Iterate through the data and calculate the totals for non-empty columns
            //for (var i = 0; i < data.length; i++) {
            //    if (data[i][0] != '') {
            //        monTotal += intVal(data[i][1]);
            //        tueTotal += intVal(data[i][2]);
            //        wedTotal += intVal(data[i][3]);
            //        thuTotal += intVal(data[i][4]);
            //        friTotal += intVal(data[i][5]);
            //        friTotal1 += intVal(data[i][6]);
            //        friTotal2 += intVal(data[i][7]);
            //    }


            //}

            //  var Deppercentage = (GDeployedVehicle / GTotalVehicle) * 100

            // Update footer by showing the total with the reference of the column index 
            $(api.column(0).footer()).html('Grand Total');
            $(api.column(1).footer()).html('');
            $(api.column(2).footer()).html('');
            $(api.column(3).footer()).html('');
            $(api.column(4).footer()).html(GNotDeployedVehicle);
            $(api.column(5).footer()).html(GReportedUniqueVehicle);
            $(api.column(6).footer()).html(GTrips1Vehicle);
            $(api.column(7).footer()).html(GTrips2Vehicle);
            $(api.column(8).footer()).html(GTrips3Vehicle);
            $(api.column(9).footer()).html(GTrips4Vehicle);
            $(api.column(10).footer()).html(GMoreThan4TripsVehicle);



        },


    });
    //  $('#example1 thead').addClass('fixed-header');
    // new $.fn.dataTable.FixedHeader(dt11);

}


var dt;

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



    var TName = "Not Deployed Vs Reported Summary";

    ShowLoading($('#example'));

    $.ajax({
        type: "POST",
        url: "/Deployment/ExportNotDeployedVsReportedSummary",
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






