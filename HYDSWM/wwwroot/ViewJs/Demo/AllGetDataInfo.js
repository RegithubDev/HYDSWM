$(document).ready(function () {


    AllMZoneLst('ddlSZone', 0, 'All Zone');
    AllDVehicleTypeLst('ddlSVehicleType', 0, 'All Vehicle Type');
    AllAssetStatusLst('ddlSStatus', 0, 'All');

    //CallAllFunc1();
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
    //GetDataTableData(Type);

}

var chartdata;
function CallAllFunc1() {

    SetDataBind();

}


function SetDataBind(Type) {
    ShowLoading($('#example1'));
    var VehicleTypeId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';


    if (Type == 'Click') {

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
        url: '/Demo/GetVMasterSummary',
        dataType: "json",
        data: { Jobjval: JSON.stringify(objval) },
        success: function (result) {

            var myJSON = result;//JSON.parse(result);
            chartdata = result.ZoneData;
            setTimeout(function () {
                EchartsPieDonutDark.init();
            }, 2000);

            bindtable(myJSON);
            HideLoading($('#example1'));

        },
        error: function (result) {
            HideLoading($('#example1'));
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

            var filename = "Vehicle Summary Report";//
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

    var vtypeid = $(objthis).attr('vtypeid');
    var zid = $(objthis).attr('zid');
    var cirid = $(objthis).attr('cirid');
    var wardid = $(objthis).attr('wardid');
    var stypeid = $(objthis).attr('stypeid');


    var url = "/Asset/VehicleSummary?vtypeid=" + vtypeid + "&zid=" + zid + "&cirid=" + cirid + "&wardid=" + wardid + "&stypeid=" + stypeid;
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
    $('#exampleS tbody').empty();

    for (var j = 0; j < Zonedata.length; j++) {

        var totalActiveInActive = Zonedata[j].Active + Zonedata[j].InActive;

        rows = "<tr>" + "<td style='border: 1px solid black; text-align: left;'>" + Zonedata[j].ZoneNo +
            "</td>" + "<td style='border: 1px solid black; text-align: left;'>" + Zonedata[j].Active +

            "</td>" + "<td style='border: 1px solid black; text-align: left;'>" + Zonedata[j].InActive +
            "</td>" + "<td style='border: 1px solid black; text-align: left;'>" + totalActiveInActive +

            //+ "<td style='border: 1px solid black; text - align: center;' vtypeid='" + vehicletypedata[i].VehicleTypeId + "' zid='" + vehicletypedata[i].ZId + "' cirid='" + vehicletypedata[i].CircleId + "' wardid='" + vehicletypedata[i].WardId + "' stypeid='7' onclick='SummaryClick(this);'>" + vehicletypedata[i].TotalAsset +


            "</td> </tr>";

        $('#exampleS tbody').append(rows);


    }



    if ($('#hfTotalrows1').val() > 0) {
        $('#example1').DataTable().clear().destroy();

        // $('#example1 tbody > tr').remove();
    }
    if ($('#hfTotalrows2').val() > 0) {
        $('#exampleS').DataTable().clear().destroy();

        // $('#example1 tbody > tr').remove();
    }
    //var Zcount = 0;
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
                    var rows = '';
                    rows += "<tr>";
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
                    rows += "<td style='border: 1px solid black; text - align: center;'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleType +
                        "</td>" + "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;'  vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='2'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Active +
                        "</td>" + "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;'  vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='3'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].InActive +
                        "</td>" + "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;'  vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='4'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].InRepair +
                        "</td>" + "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;'  vtypeid='" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].VehicleTypeId + "' zid='" + Zonedata[i].ZId + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + Zonedata[i].Circlelst[j].Wardlst[k].WardId + "' stypeid='8'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].Condemed +
                        "</td>" + "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;'  vtypeid='" + 0 + "' zid='" + 0 + "' cirid='" + Zonedata[i].Circlelst[j].CircleId + "' wardid='" + 0 + "' stypeid='7'>" + Zonedata[i].Circlelst[j].Wardlst[k].VehicleData[l].TotalAsset +

                        //+ "<td style='border: 1px solid black; text - align: center;' vtypeid='" + vehicletypedata[i].VehicleTypeId + "' zid='" + vehicletypedata[i].ZId + "' cirid='" + vehicletypedata[i].CircleId + "' wardid='" + vehicletypedata[i].WardId + "' stypeid='7' onclick='SummaryClick(this);'>" + vehicletypedata[i].TotalAsset +


                        "</td> </tr>";


                    $('#example1 tbody').append(rows);
                }

            }
        }





    }


    var rowcount = Zonedata.length;
    var IsNew = 0
    //for (i = 0; i < vehicletypedata.length; i++) {

    //    var count = i + 1;

    //    var rows = '';
    //    var Zcount = 0;//Zonedata[i].TotalZRowCount;//vehicletypedata.length;
    //    var circleinfo = '';

    //    //for (i = 0; i < item.Circlelst.length; i++) {

    //    //    circleinfo += "<td rowspan='" + item.Circlelst[i].TotalCircleRowCount + "' style='border: 1px solid black; text - align: center;'>";
    //    //}

    //    rows = "<tr>" + "<td style='border: 1px solid black; text - align: center;' class='build-name'>" + vehicletypedata[i].Zonecode +
    //        "</td>" + "<td style='border: 1px solid black; text - align: center;' class='cbuild-name'>" + vehicletypedata[i].CircleCode +
    //        "</td>" + "<td style='border: 1px solid black; text - align: center;' class='wbuild-name'>" + vehicletypedata[i].WardName +
    //        "</td>" + "<td style='border: 1px solid black; text - align: center;'>" + vehicletypedata[i].VehicleType +
    //        "<td  onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;' vtypeid='" + vehicletypedata[i].VehicleTypeId + "' zid='" + vehicletypedata[i].ZId + "' cirid='" + vehicletypedata[i].CircleId + "' wardid='" + vehicletypedata[i].WardId + "' stypeid='2'>" + vehicletypedata[i].Active + "</td>" +
    //        "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;' vtypeid='" + vehicletypedata[i].VehicleTypeId + "' zid='" + vehicletypedata[i].ZId + "' cirid='" + vehicletypedata[i].CircleId + "' wardid='" + vehicletypedata[i].WardId + "' stypeid='3'>" + vehicletypedata[i].InActive + "</td>" +
    //        "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;' vtypeid='" + vehicletypedata[i].VehicleTypeId + "' zid='" + vehicletypedata[i].ZId + "' cirid='" + vehicletypedata[i].CircleId + "' wardid='" + vehicletypedata[i].WardId + "' stypeid='4'>" + vehicletypedata[i].InRepair + "</td>" +
    //        "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;' vtypeid='" + vehicletypedata[i].VehicleTypeId + "' zid='" + vehicletypedata[i].ZId + "' cirid='" + vehicletypedata[i].CircleId + "' wardid='" + vehicletypedata[i].WardId + "' stypeid='8'>" + vehicletypedata[i].Condemed + "</td>" +
    //        "<td onclick='ClickForRedirection(this);' style='border: 1px solid black; text-align: center;' vtypeid='" + vehicletypedata[i].VehicleTypeId + "' zid='" + vehicletypedata[i].ZId + "' cirid='" + vehicletypedata[i].CircleId + "' wardid='" + vehicletypedata[i].WardId + "' stypeid='7'>" + vehicletypedata[i].TotalAsset + "</td>" +

    //        //+ "<td style='border: 1px solid black; text - align: center;' vtypeid='" + vehicletypedata[i].VehicleTypeId + "' zid='" + vehicletypedata[i].ZId + "' cirid='" + vehicletypedata[i].CircleId + "' wardid='" + vehicletypedata[i].WardId + "' stypeid='7' onclick='SummaryClick(this);'>" + vehicletypedata[i].TotalAsset +


    //        "</td> </tr>";

    //    $('#example1 tbody').append(rows);


    //}

    //var tabid = $('#example1');
    //$('#hfTotalrows1').val(rowcount);
    //if ($('#hfTotalrows1').val() > 0)
    //    setdatatable();


}

var dt11;
var dt12;
function setdatatable() {
    dt11 = $('#example1').DataTable({
        "destroy": true,
        "responsive": true,
        "pageLength": 200000,
        "order": [[0, "asc"]],
        "paging": false,
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
            var monTotal = 0;
            var tueTotal = 0;
            var wedTotal = 0;
            var thuTotal = 0;
            var friTotal = 0;
            var friTotal1 = 0;
            var friTotal2 = 0;
            var friTotal3 = 0;

            // Iterate through the data and calculate the totals for non-empty columns
            for (var i = 0; i < data.length; i++) {
                if (data[i][0] != '') {
                    monTotal += intVal(data[i][1]);
                    tueTotal += intVal(data[i][2]);
                    wedTotal += intVal(data[i][3]);
                    thuTotal += intVal(data[i][4]);
                    friTotal += intVal(data[i][5]);
                    friTotal1 += intVal(data[i][6]);
                    friTotal2 += intVal(data[i][7]);
                    friTotal3 += intVal(data[i][8]);
                }


            }

            // Update footer by showing the total with the reference of the column index 
            $(api.column(0).footer()).html('Grand Total');
            $(api.column(1).footer()).html(monTotal);
            $(api.column(2).footer()).html(tueTotal);
            $(api.column(3).footer()).html(wedTotal);
            $(api.column(4).footer()).html(thuTotal);
            $(api.column(5).footer()).html(friTotal);
            $(api.column(6).footer()).html(friTotal1);
            $(api.column(7).footer()).html(friTotal2);
            $(api.column(8).footer()).html(friTotal3);
        },


    });

}

var EchartsPieDonutDark = function () {
    ShowLoading($('#pie_donut'));


    // Basic donut chart
    var _scatterPieDonutDarkExample = function () {
        if (typeof echarts == 'undefined') {
            console.warn('Warning - echarts.min.js is not loaded.');
            return;
        }

        // Define element
        var pie_donut_element = document.getElementById('pie_donut');

        //
        // Charts configuration
        //

        if (pie_donut_element) {


            // Initialize chart
            var pie_donut = echarts.init(pie_donut_element);



            //var myJSON = chartdata;//result;
            var Zonedata1 = chartdata//myJSON.ZoneData;

            var pieData = [];

            for (i = 0; i < Zonedata1.length; i++) {
                pieData.push({
                    value: Zonedata1[i].TotalAsset,
                    name: Zonedata1[i].ZoneNo
                });
            }

            // Options
            pie_donut.setOption({
                //color: [
                //    '#2ec7c9', '#d87a80', '#FF9800', '#ffb980', '#b6a2de',
                //    '#8d98b3', '#e5cf0d', '#97b552', '#95706d', '#dc69aa',
                //    '#07a2a4', '#9a7fd1', '#588dd5', '#f5994e', '#c05050',
                //    '#59678c', '#c9ab00', '#7eb00a', '#6f5553', '#c14089'
                //],
                textStyle: {
                    fontFamily: 'Roboto, Arial, Verdana, sans-serif',
                    fontSize: 13
                },
                title: {

                },
                tooltip: {

                },

                series: [{
                    name: 'Asset Summary Info',
                    type: 'pie',

                    center: ['50%', '57.5%'],
                    itemStyle: {
                        normal: {
                            borderWidth: 2,
                            borderColor: '#fff',
                            label: {
                                position: "inside",
                                show: true,
                                formatter: function (params) {
                                    return params.value
                                },
                            }
                        }
                    },
                    data: pieData
                }]
            });
            HideLoading($('#pie_donut'));



        }

        // Resize function
        var triggerChartResize = function () {
            pie_donut_element && pie_donut.resize();
        };

        // On sidebar width change
        var sidebarToggle = document.querySelector('.sidebar-control');
        sidebarToggle && sidebarToggle.addEventListener('click', triggerChartResize);

        // On window resize
        var resizeCharts;
        window.addEventListener('resize', function () {
            clearTimeout(resizeCharts);
            resizeCharts = setTimeout(function () {
                triggerChartResize();
            }, 200);
        });
    };

    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _scatterPieDonutDarkExample();
        }
    };
}();

var dt;
function GetDataTableData(Type) {


    var VehicleTypeId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';


    if (Type == 'Click') {
        VehicleTypeId = $('#ddlSVehicleType').val();
        ZoneId = $('#ddlSZone').val();
        CircleId = $('#ddlSCircle').val();
        WardId = $('#ddlSWard').val();

    }
    $('#example2 tbody').empty();
    dt = $('#example2').DataTable({
        processing: true,
        destroy: true,
        responsive: true,
        serverSide: true,
        searchable: true,
        "pageLength": 200000,
        //fixedHeader: {
        //    header: true,
        //   // footer: true
        //},
        lengthMenu: [[10, 20, 50, 100, 500, 10000, -1], [10, 20, 50, 100, 500, 10000, "All"]],
        language: {
            //infoEmpty: "No records available",
            searchPlaceholder: "Search Records",
            search: ""
        },
        rowsGroup: [0, 1, 2],
        dom: 'Blfrtip',

        "drawCallback": function (settings) {

            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;
            var subTotal = new Array();
            var groupID = -1;
            var aData = new Array();
            var zData = new Array();
            var index = 0;
            var len = api.page.len();
            if (-1 == -1) {

                //
                //api.column(1, { page: 'current' }).data().each(function (group, i) {

                //    // console.log(group+">>>"+i);

                //    //var vals = api.row(api.row($(rows).eq(i)).index()).data();
                //    //var salary = vals[8] ? parseFloat(vals[8]) : 0;
                //    //var Condemed = vals[7] ? parseFloat(vals[7]) : 0;
                //    //var InRepair = vals[6] ? parseFloat(vals[6]) : 0;
                //    //var InActive = vals[5] ? parseFloat(vals[5]) : 0;
                //    //var Active = vals[4] ? parseFloat(vals[4]) : 0;

                //    var vals = api.row(api.row($(rows).eq(i)).index()).data();
                //    var salary = vals.TotalAsset;//vals[8] ? parseFloat(vals[8]) : 0;
                //    var Condemed = vals.Condemed;//vals[7] ? parseFloat(vals[7]) : 0;
                //    var InRepair = vals.InRepair; //vals[6] ? parseFloat(vals[6]) : 0;
                //    var InActive = vals.InActive; //vals[5] ? parseFloat(vals[5]) : 0;
                //    var Active = vals.Active;//vals[4] ? parseFloat(vals[4]) : 0;

                //    if (typeof aData[group] == 'undefined') {
                //        aData[group] = new Array();
                //        aData[group].rows = [];
                //        aData[group].Active = [];
                //        aData[group].InActive = [];
                //        aData[group].InRepair = [];
                //        aData[group].Condemed = [];
                //        aData[group].TotalAsset = [];
                //    }

                //    aData[group].rows.push(i);
                //    aData[group].Active.push(Active);
                //    aData[group].InActive.push(InActive);
                //    aData[group].InRepair.push(InRepair);
                //    aData[group].Condemed.push(Condemed);
                //    aData[group].TotalAsset.push(salary);

                //});



                //var grandTotal = {
                //    Active: 0,
                //    InActive: 0,
                //    InRepair: 0,
                //    Condemed: 0,
                //    TotalAsset: 0
                //};

                //for (var office in aData) {
                //    grandTotal.Active += sumArray(aData[office].Active);
                //    grandTotal.InActive += sumArray(aData[office].InActive);
                //    grandTotal.InRepair += sumArray(aData[office].InRepair);
                //    grandTotal.Condemed += sumArray(aData[office].Condemed);
                //    grandTotal.TotalAsset += sumArray(aData[office].TotalAsset);
                //}
                //
                //// Append grand total footer row
                //$(rows).last().after(
                //    '<tr><td style="border:1px solid black;" colspan="4">Grand Total</td>' +
                //    '<td style="border:1px solid black;">' + grandTotal.Active + '</td>' +
                //    '<td style="border:1px solid black;">' + grandTotal.InActive + '</td>' +
                //    '<td style="border:1px solid black;">' + grandTotal.InRepair + '</td>' +
                //    '<td style="border:1px solid black;">' + grandTotal.Condemed + '</td>' +
                //    '<td style="border:1px solid black;">' + grandTotal.TotalAsset + '</td></tr>'
                //);

                //// Helper function to sum an array
                //function sumArray(array) {
                //    return array.reduce(function (a, b) {
                //        return a + b;
                //    }, 0);
                //}


                api.column(0, { page: 'current' }).data().each(function (group, i) {

                    // console.log(group+">>>"+i);

                    var vals = api.row(api.row($(rows).eq(i)).index()).data();
                    var salary = vals.TotalAsset;//vals[8] ? parseFloat(vals[8]) : 0;
                    var Condemed = vals.Condemed;//vals[7] ? parseFloat(vals[7]) : 0;
                    var InRepair = vals.InRepair; //vals[6] ? parseFloat(vals[6]) : 0;
                    var InActive = vals.InActive; //vals[5] ? parseFloat(vals[5]) : 0;
                    var Active = vals.Active;//vals[4] ? parseFloat(vals[4]) : 0;

                    if (typeof zData[group] == 'undefined') {
                        zData[group] = new Array();
                        zData[group].rows = [];
                        zData[group].Active = [];
                        zData[group].InActive = [];
                        zData[group].InRepair = [];
                        zData[group].Condemed = [];
                        zData[group].TotalAsset = [];
                    }

                    zData[group].rows.push(i);
                    zData[group].Active.push(Active);
                    zData[group].InActive.push(InActive);
                    zData[group].InRepair.push(InRepair);
                    zData[group].Condemed.push(Condemed);
                    zData[group].TotalAsset.push(salary);

                });

                var zidx = 0;


                for (var office in zData) {

                    zidx = Math.max.apply(Math, zData[office].rows);
                    var sum = 0;
                    var Activesum = 0;
                    var InActivesum = 0;
                    var InRepairsum = 0;
                    var Condemedsum = 0;
                    $.each(zData[office].TotalAsset, function (k, v) {
                        sum = sum + v;
                    });
                    $.each(zData[office].Active, function (k, v) {
                        Activesum = Activesum + v;
                    });
                    $.each(zData[office].InActive, function (k, v) {
                        InActivesum = InActivesum + v;
                    });
                    $.each(zData[office].InRepair, function (k, v) {
                        InRepairsum = InRepairsum + v;
                    });
                    $.each(zData[office].Condemed, function (k, v) {
                        Condemedsum = Condemedsum + v;
                    });
                    office = office + " Total";
                    // console.log(aData[office].TotalAsset);
                    $(rows).eq(zidx).after(
                        '<tr><td style="border:1px solid black;" colspan="4">' + office + '</td>' +

                        '<td style="border:1px solid black;">' + Activesum + '</td>' +
                        '<td style="border:1px solid black;">' + InActivesum + '</td>' +
                        '<td style="border:1px solid black;">' + InRepairsum + '</td>' +
                        '<td style="border:1px solid black;">' + Condemedsum + '</td>' +
                        '<td style="border:1px solid black;">' + sum + '</td></tr>'
                    );

                };


                var idx = 0;


                //for (var office in aData) {

                //    idx = Math.max.apply(Math, aData[office].rows);
                //    var sum = 0;
                //    var Activesum = 0;
                //    var InActivesum = 0;
                //    var InRepairsum = 0;
                //    var Condemedsum = 0;
                //    $.each(aData[office].TotalAsset, function (k, v) {
                //        sum = sum + v;
                //    });
                //    $.each(aData[office].Active, function (k, v) {
                //        Activesum = Activesum + v;
                //    });
                //    $.each(aData[office].InActive, function (k, v) {
                //        InActivesum = InActivesum + v;
                //    });
                //    $.each(aData[office].InRepair, function (k, v) {
                //        InRepairsum = InRepairsum + v;
                //    });
                //    $.each(aData[office].Condemed, function (k, v) {
                //        Condemedsum = Condemedsum + v;
                //    });
                //    office = office + " Total";
                //    // console.log(aData[office].TotalAsset);
                //    //$(rows).eq(idx).after(
                //    //    '<tr><td style="border:1px solid black;" colspan="4">' + office + '</td>' +
                //    //    '<td style="border:1px solid black;">' + Activesum + '</td>' +
                //    //    '<td style="border:1px solid black;">' + InActivesum + '</td>' +
                //    //    '<td style="border:1px solid black;">' + InRepairsum + '</td>' +
                //    //    '<td style="border:1px solid black;">' + Condemedsum + '</td>' +
                //    //    '<td style="border:1px solid black;">' + sum + '</td></tr>'
                //    //);

                //};



            }
        },
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api(), data;

            // converting to interger to find total
            var intVal = function (i) {
                return typeof i === 'string' ?
                    i.replace(/[\$,]/g, '') * 1 :
                    typeof i === 'number' ?
                        i : 0;
            };

            // computing column Total of the complete result 
            var monTotal = api
                .column(1)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var tueTotal = api
                .column(2)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var wedTotal = api
                .column(3)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var thuTotal = api
                .column(4)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);

            var friTotal = api
                .column(5)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var friTotal1 = api
                .column(6)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var friTotal2 = api
                .column(7)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            var friTotal3 = api
                .column(8)
                .data()
                .reduce(function (a, b) {
                    return intVal(a) + intVal(b);
                }, 0);
            // Update footer by showing the total with the reference of the column index 
            $(api.column(0).footer()).html('Grand Total');
            $(api.column(1).footer()).html(monTotal);
            $(api.column(2).footer()).html(tueTotal);
            $(api.column(3).footer()).html(wedTotal);
            $(api.column(4).footer()).html(thuTotal);
            $(api.column(5).footer()).html(friTotal);
            $(api.column(6).footer()).html(friTotal1);
            $(api.column(7).footer()).html(friTotal2);
            $(api.column(8).footer()).html(friTotal3);
            //$('.dataTables_wrapper .dataTables_scrollFoot th').removeClass('tbl_brdr');
            /*  $('.dataTables_wrapper .dataTables_scrollFoot th').css('text-align', 'right');*/
            //$('.dataTables_wrapper .dataTables_scrollFoot th').removeClass('tbl_brdr');
        },

        buttons: {
            buttons: [

                {
                    extend: 'csvHtml5',
                    className: 'btn btn-light',
                    text: '<i class="icon-file-spreadsheet mr-2"></i> Excel',
                    extension: '.csv'
                }
            ]
        },
        initComplete: function () {
            $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
        },
        ajax: {
            url: "/Demo/GetAllVMasterSummary_Paging/",
            type: 'POST',
            data: function (d) {

                d.WardId = WardId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.VehicleTypeId = VehicleTypeId;
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


            { data: "Zonecode", sortable: true, "className": "tbl_brdr" },
            { data: "CircleCode", sortable: true, "className": "tbl_brdr" },
            { data: "WardName", sortable: true, "className": "tbl_brdr" },
            { data: "VehicleType", sortable: true, "className": "tbl_brdr" },
            //{ data: "Active", sortable: true, "className": "tbl_brdr" },
            {
                'className': 'tbl_brdr',
                data: "Active",
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a  href='/Asset/VehicleSummary?vtypeid=" + row.VehicleTypeId + "&zid=" + row.ZId + "&cirid=" + row.CircleId + "&wardid=" + row.WardId + "&stypeid=2'  target='_blank'>" + row.Active + "</a>";
                }
            },

            {
                'className': 'tbl_brdr',
                data: "InActive",
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a  href='/Asset/VehicleSummary?vtypeid=" + row.VehicleTypeId + "&zid=" + row.ZId + "&cirid=" + row.CircleId + "&wardid=" + row.WardId + "&stypeid=3'  target='_blank'>" + row.InActive + "</a>";
                }
            },
            {
                'className': 'tbl_brdr',
                data: "InRepair",
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a  href='/Asset/VehicleSummary?vtypeid=" + row.VehicleTypeId + "&zid=" + row.ZId + "&cirid=" + row.CircleId + "&wardid=" + row.WardId + "&stypeid=4'  target='_blank'>" + row.InRepair + "</a>";
                }
            },
            {
                'className': 'tbl_brdr',
                data: "Condemed",
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a  href='/Asset/VehicleSummary?vtypeid=" + row.VehicleTypeId + "&zid=" + row.ZId + "&cirid=" + row.CircleId + "&wardid=" + row.WardId + "&stypeid=8'  target='_blank'>" + row.Condemed + "</a>";
                }
            },
            {
                'className': 'tbl_brdr',
                data: "TotalAsset",
                sortable: true,
                "render": function (data, type, row, meta) {

                    return "<a  href='/Asset/VehicleSummary?vtypeid = " + row.VehicleTypeId + "&zid=" + row.ZId + "&cirid=" + row.CircleId + "&wardid=" + row.WardId + "&stypeid=7'  target='_blank'>" + row.TotalAsset + "</a>";
                }
            },



        ]
    });


}








