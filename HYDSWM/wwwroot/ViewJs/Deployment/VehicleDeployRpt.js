var isTrip = 0;
$(document).ready(function () {


    $('#txtFromDate').datepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });

    var date = new Date();
    document.getElementById("txtFromDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();



    $('#txtFromDate').datepicker({
        changeMonth: true,
        changeYear: true,

        autoclose: true,
        endDate: document.getElementById("txtFromDate").value,
    });

    AllDVehicleTypeLst('ddlVehicleType', 0, 'All Vehicle Type')

    AllMZoneLst('ddlZone', 0, 'All Zone');
    isTrip = 0;
    CallAllFunc('Load', isTrip)
    CallFunc()



});


var IsClick = 0;
var dt;







function GetDataTableData(Type, TripCount) {


    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';
    var FromDate = $("#txtFromDate").val();
    var TSId = "0";




    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        VehicleTypeId = $('#ddlVehicleType').val();
        FromDate: $("#txtFromDate").val(),
            TSId = "0",
            isTrip = TripCount
        IsClick = 1;

    }




    dt = $('#example2').DataTable({
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
            url: "/Deployment/GetAllAttendencePaging/",
            type: 'POST',
            data: function (d) {
                d.FromDate = FromDate;

                d.WardId = WardId;
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.VehicleTypeId = VehicleTypeId;
                d.TSId = TSId;
                d.TripCount = TripCount;
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
            { data: "VehicleType", sortable: true },
            { data: "OwnerType", sortable: true },
            { data: "DeployedOn", sortable: true },
            { data: "ConcatDeployed", sortable: true },
            { data: "AllotedTSName", sortable: true },
            { data: "RTSName", sortable: true },
            { data: "ReportedOn", sortable: true },
            { data: "TripNo", sortable: true },
            { data: "ZWC", sortable: true },

        ]
    });


}


function CallFunc() {
    SetDataBind('Click');
    EchartsColumnsStackedLight.init();
    EchartsColumnsStackedLight1.init();
    GetDataTableData('Click', isTrip)

}

function CallAllFunc(Type, TripCount) {

    isTrip = TripCount;

    $('#example').DataTable().clear().destroy();
    SetDataBind();
    GetDataTableData('Load', TripCount)


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


function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'Select', $('#ddlCircle').find(":selected").attr('value'));
}
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}
var IsClick = 0;
var dt;

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


var EchartsColumnsStackedLight = function () {


    //
    // Setup module components
    //

    // Stacked column chart
    var _columnsStackedLightExample = function () {
        if (typeof echarts == 'undefined') {
            console.warn('Warning - echarts.min.js is not loaded.');
            return;
        }

        // Define element
        var columns_stacked_element = document.getElementById('columns_stacked2');


        //
        // Charts configuration
        //

        if (columns_stacked_element) {

            // Initialize chart
            var columns_stacked = echarts.init(columns_stacked_element);
            ShowLoading($('#columns_stacked2'));


            //
            // Chart config
            //



            $.ajax({


                type: "POST",
                // contentType: "application/json; charset=utf-8",
                url: '/Deployment/GetDeploymentTsReport',
                data: {

                    ZoneId: $('#ddlZone').val(),
                    CircleId: $('#ddlCircle').val(),
                    WardId: $('#ddlWard').val(),
                    VehicleTypeId: $('#ddlVehicleType').val(),
                    FromDate: $("#txtFromDate").val()


                },



                success: function (data) {

                    var myJSON = JSON.parse(data);

                    var Result1 = myJSON.Table;

                    var collectedarr = [];
                    var totalhousedarr = [];
                    var circlearr = [];
                    var circlearr1 = [];
                    var circlearr2 = [];
                    var circlearr3 = [];


                    for (i = 0; i < Result1.length; i++) {
                        collectedarr.push(Result1[i].Deployed);
                        totalhousedarr.push(Result1[i].NotDeployed);
                        circlearr.push(Result1[i].VehicleType);
                        circlearr1.push(Result1[i].ReportedTs);
                        circlearr2.push(Result1[i].NotReportedTs);
                        circlearr3.push(Result1[i].NotDeployReportedTs);


                    }

                    // Options
                    columns_stacked.setOption({

                        // Define colors
                        color: ['#2ec7c9', '#b6a2de', '#5ab1ef', '#ffb980', '#d87a80'],

                        // Global text styles
                        textStyle: {
                            fontFamily: 'var(--body-font-family)',
                            color: 'var(--body-color)',
                            fontSize: 14,
                            lineHeight: 22,
                            textBorderColor: 'transparent'
                        },

                        // Chart animation duration
                        animationDuration: 750,

                        // Setup grid
                        grid: {
                            left: 10,
                            right: 10,
                            top: 35,
                            bottom: 0,
                            containLabel: true
                        },

                        // Add legend
                        legend: {
                            data: ['Deployed', 'Not Deployed', 'Reported at TS/SCTP', 'Not Reported at TS/SCTP', 'Not Dep But Report at TS/SCTP'],
                            itemHeight: 5,
                            itemGap: 3,
                            textStyle: {
                                padding: [0, 2],
                                color: '#000000'
                            },
                            orient: 'horizontal', // set orientation to horizontal
                            top: 'top', // adjust top position
                            width: '95%', // set a width to fit all items in a single row
                            // adjust left position
                        },

                        // Add tooltip
                        tooltip: {
                            trigger: 'axis',
                            backgroundColor: 'rgba(0,0,0,0.9)',
                            padding: [10, 15],
                            textStyle: {
                                fontSize: 13,
                                fontFamily: 'Roboto, sans-serif',
                                color: '#fff'
                            }
                        },

                        // Horizontal axis
                        xAxis: [{
                            type: 'category',
                            data: circlearr,//['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                            axisLabel: {
                                color: '#000000',
                                rotate: 70,
                            },
                            axisLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.25)'
                                }
                            },
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.1)',
                                    type: 'dashed'
                                }
                            }
                        }],

                        // Vertical axis
                        yAxis: [{
                            type: 'value',
                            axisLabel: {
                                color: '#000000'
                            },
                            axisLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.25)'
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.1)'
                                }
                            },
                            splitArea: {
                                show: true,
                                areaStyle: {
                                    color: ['rgba(255,255,255,0.01)', 'rgba(0,0,0,0.01)']
                                }
                            }
                        }],

                        // Add series
                        series: [





                            {
                                name: 'Deployed',
                                type: 'bar',
                                data: collectedarr,
                                stack: 'Advertising',

                            },
                            {
                                name: 'Not Deployed',
                                data: totalhousedarr,
                                type: 'bar',
                                stack: 'Advertising',

                            },
                            {
                                name: 'Reported at TS/SCTP',
                                type: 'bar',
                                data: circlearr1,
                                stack: 'Advertising',

                            },

                            {
                                name: 'Not Reported at TS/SCTP',
                                data: circlearr2,
                                type: 'bar',
                                stack: 'Advertising',

                            },

                            {
                                name: 'Not Dep But Report at TS/SCTP',
                                data: circlearr3,
                                type: 'bar',
                                stack: 'Advertising',

                            },


                        ]
                    });
                    HideLoading($('#columns_stacked2'));
                },





            });






        }


        //
        // Resize charts
        //

        // Resize function
        var triggerChartResize = function () {
            columns_stacked_element && columns_stacked.resize();
        };

        // On sidebar width change
        var sidebarToggle = document.querySelectorAll('.sidebar-control');
        if (sidebarToggle) {
            sidebarToggle.forEach(function (togglers) {
                togglers.addEventListener('click', triggerChartResize);
            });
        }

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
            _columnsStackedLightExample();
        }
    }


}();



var EchartsColumnsStackedLight1 = function () {


    //
    // Setup module components
    //

    // Stacked column chart
    var _columnsStackedLightExample = function () {
        if (typeof echarts == 'undefined') {
            console.warn('Warning - echarts.min.js is not loaded.');
            return;
        }

        // Define element
        var columns_stacked_element = document.getElementById('columns_stacked');


        //
        // Charts configuration
        //

        if (columns_stacked_element) {

            // Initialize chart
            var columns_stacked = echarts.init(columns_stacked_element);
            ShowLoading($('#columns_stacked'));


            //
            // Chart config
            //



            $.ajax({


                type: "POST",
                // contentType: "application/json; charset=utf-8",
                url: '/Deployment/GetDeploymentTsReport',
                data: {

                    ZoneId: $('#ddlZone').val(),
                    CircleId: $('#ddlCircle').val(),
                    WardId: $('#ddlWard').val(),
                    VehicleTypeId: $('#ddlVehicleType').val(),
                    FromDate: $("#txtFromDate").val()


                },

                success: function (data) {

                    var myJSON = JSON.parse(data);

                    var Result1 = myJSON.Table1;

                    var collectedarr = [];
                    var totalhousedarr = [];
                    var circlearr = [];
                    var circlearr1 = [];
                    var circlearr2 = [];
                    var circlearr3 = [];
                    var circlearr4 = [];

                    for (i = 0; i < Result1.length; i++) {
                        collectedarr.push(Result1[i].Deployed);
                        totalhousedarr.push(Result1[i].OneTripCount);
                        circlearr.push(Result1[i].VehicleType);
                        circlearr1.push(Result1[i].TwoTripCount);
                        circlearr2.push(Result1[i].ThreeTripCount);
                        circlearr3.push(Result1[i].FourTripCount);
                        circlearr4.push(Result1[i].MorethanFourTripCount);



                    }

                    // Options
                    columns_stacked.setOption({

                        // Define colors
                        color: ['#2ec7c9', '#b6a2de', '#5ab1ef', '#ffb980', '#d87a80'],

                        // Global text styles
                        textStyle: {
                            fontFamily: 'var(--body-font-family)',
                            color: 'var(--body-color)',
                            fontSize: 14,
                            lineHeight: 22,
                            textBorderColor: 'transparent'
                        },

                        // Chart animation duration
                        animationDuration: 750,

                        // Setup grid
                        grid: {
                            left: 10,
                            right: 10,
                            top: 35,
                            bottom: 0,
                            containLabel: true
                        },

                        // Add legend
                        legend: {
                            data: ['Deployed', '1 Trip', '2 Trip', '3 Trip', '4 Trip', '>4 Trip'],
                            itemHeight: 5,
                            itemGap: 2,
                            textStyle: {
                                padding: [0, 2],
                                color: '#000000'
                            },
                            orient: 'horizontal', // set orientation to horizontal
                            top: 'top', // adjust top position
                            width: '95%', // set a width to fit all items in a single row
                            // adjust left position
                        },

                        // Add tooltip
                        tooltip: {
                            trigger: 'axis',
                            backgroundColor: 'rgba(0,0,0,0.9)',
                            padding: [10, 15],
                            textStyle: {
                                fontSize: 13,
                                fontFamily: 'Roboto, sans-serif',
                                color: '#fff'
                            }
                        },

                        // Horizontal axis
                        xAxis: [{
                            type: 'category',
                            data: circlearr,//['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                            axisLabel: {
                                color: '#000000',
                                rotate: 70,
                            },
                            axisLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.25)'
                                }
                            },
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.1)',
                                    type: 'dashed'
                                }
                            }
                        }],

                        // Vertical axis
                        yAxis: [{
                            type: 'value',
                            axisLabel: {
                                color: '#000000'
                            },
                            axisLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.25)'
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.1)'
                                }
                            },
                            splitArea: {
                                show: true,
                                areaStyle: {
                                    color: ['rgba(255,255,255,0.01)', 'rgba(0,0,0,0.01)']
                                }
                            }
                        }],

                        // Add series
                        series: [





                            {
                                name: 'Deployed',
                                type: 'bar',
                                data: collectedarr,
                                stack: 'Advertising',

                            },
                            {
                                name: '1 Trip',
                                data: totalhousedarr,
                                type: 'bar',
                                stack: 'Advertising',

                            },
                            {
                                name: '2 Trip',
                                type: 'bar',
                                data: circlearr2,
                                stack: 'Advertising',

                            },

                            {
                                name: '3 Trip',
                                data: circlearr3,
                                type: 'bar',
                                stack: 'Advertising',

                            },

                            {
                                name: '4 Trip',
                                data: circlearr1,
                                type: 'bar',
                                stack: 'Advertising',

                            },
                            {
                                name: '>4 Trip',
                                data: circlearr1,
                                type: 'bar',
                                stack: 'Advertising',

                            },


                        ]
                    });
                    HideLoading($('#columns_stacked'));
                },


            });



        }


        // Resize charts
        //

        // Resize function
        var triggerChartResize = function () {
            columns_stacked_element && columns_stacked.resize();
        };

        // On sidebar width change
        var sidebarToggle = document.querySelectorAll('.sidebar-control');
        if (sidebarToggle) {
            sidebarToggle.forEach(function (togglers) {
                togglers.addEventListener('click', triggerChartResize);
            });
        }

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
            _columnsStackedLightExample();
        }
    }


}();






function DownloadFile(FType) {



    var FromDate = document.getElementById('txtFromDate').value;

    var TripCount = isTrip;

    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';

    var TSId = "0";

    if (IsClick == 1) {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        IsClick = 1;
        Trip: TripCount;
        TSId: TSId;

        VehicleTypeId = $('#ddlVehicleType').val();

    }
    var TName = "All Deployment Summary Report";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Deployment/ExportAllATDSummary",
        data: { FromDate: FromDate, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleTypeId: VehicleTypeId, FName: TName, Trip: TripCount, TSId: TSId },
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




function bindtable(data) {



    $('#example tbody').empty();

    if ($('#hfTotalrows').val() > 0)
        $('#example').DataTable().clear().destroy();


    var rowcount = data.length;

    $.each(data, function (i, item) {
        var count = i + 1;

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<td>" + item.VehicleType +
            "</td>" + "<td>" + item.Deployed +
            "</td>" + "<td>" + item.TotalDeployed +
            "</td>" + "<td>" + item.ReportedTs +
            "</td>" + "<td>" + item.NotReportedTs +
            "</td>" + "<td>" + item.NotDeployReportedTs +
            "</td>" + "<td>" + item.Total +



            "</td> </tr>";
        $('#example tbody').append(rows);

    });

    var tabid = $('#example');
    $('#hfTotalrows').val(rowcount);
    if ($('#hfTotalrows').val() > 0)
        setdatatable();


}

function SetDataBind(Type) {


    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';
    var FromDate = $("#txtFromDate").val();


    if (Type == 'Click') {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        VehicleTypeId = $('#ddlVehicleType').val();
        FromDate: $("#txtFromDate").val(),
            IsClick = 1;

    }

    var requestModel = {
        ZoneId: ZoneId,
        CircleId: CircleId,
        WardId: WardId,
        VehicleTypeId: VehicleTypeId,
        FromDate: FromDate


    };

    $.ajax({
        type: "POST",
        url: '/Deployment/GetDeploymentTsReport',
        dataType: "json",
        data: requestModel,
        success: function (result) {
            var myJSON = JSON.parse(result);
            var Result1 = myJSON.Table;
            var Result2 = myJSON.Table1;
            bindtable(Result1);
            bindtable1(Result2);


        }
    });
}


function bindtable1(data) {



    $('#example1 tbody').empty();

    if ($('#hfTotalrows1').val() > 0)
        $('#example1').DataTable().clear().destroy();
    var rowcount = data.length;


    $.each(data, function (i, item) {
        var count = i + 1;

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<td>" + item.VehicleType +
            "</td>" + "<td>" + item.Deployed +
            "</td>" + "<td>" + item.OneTripCount +
            "</td>" + "<td>" + item.TwoTripCount +
            "</td>" + "<td>" + item.ThreeTripCount +
            "</td>" + "<td>" + item.FourTripCount +
            "</td>" + "<td>" + item.MorethanFourTripCount +
            "</td>" + "<td>" + item.AllTripCount +


            "</td> </tr>";
        $('#example1 tbody').append(rows);

    });

    var tabid = $('#example1');
    $('#hfTotalrows1').val(rowcount);
    if ($('#hfTotalrows1').val() > 0)
        setdatatable();





}


//function bindtable2(data) {

//    ;

//    $('#example2 tbody').empty();

//    if ($('#hfTotalrows2').val() > 0)
//        $('#example2').DataTable().clear().destroy();

//    var rowcount = data.length;


//    $.each(data, function (i, item) {
//        var count = i + 1;




//            "</td> </tr>";
//        $('#example2 tbody').append(rows);

//    });

//    var tabid = $('#example2');
//    $('#hfTotalrows2').val(rowcount);
//    if ($('#hfTotalrows2').val() > 0)
//        setdatatable();




//}


function setdatatable() {
    $('#example').DataTable({
        destroy: true,
        "responsive": true,
        "order": [[0, "asc"]],
        lengthMenu: [
            [10, 25, 50, 500, 1000, 5000],
            ['10 rows', '25 rows', '50 rows', '500 rows', '1000 rows', '5000 rows']
        ],
        language: {
            infoEmpty: "No records available",
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
                    text: '<i class="icon-file-spreadsheet mr-2"></i> CSV',
                    fieldSeparator: '\t',
                    extension: '.csv'
                },
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i>',
                    className: 'btn bg-blue btn-icon dropdown-toggle'
                }
            ]
        }

    });
    $('#example1').DataTable({
        destroy: true,
        "responsive": true,
        "order": [[0, "asc"]],
        lengthMenu: [
            [10, 25, 50, 500, 1000, 5000],
            ['10 rows', '25 rows', '50 rows', '500 rows', '1000 rows', '5000 rows']
        ],
        language: {
            infoEmpty: "No records available",
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
                    text: '<i class="icon-file-spreadsheet mr-2"></i> CSV',
                    fieldSeparator: '\t',
                    extension: '.csv'
                },
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i>',
                    className: 'btn bg-blue btn-icon dropdown-toggle'
                }


            ]
        }

    });

}