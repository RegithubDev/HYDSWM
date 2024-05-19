$(document).ready(function () {
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

    CallAllFunc()
    CallFunc()




});

var cresult1;
var cresult2;
var cresult3;
function CallFunc() {
    SetDataBind('Click');
  
   
  
}

function CallAllFunc() {



    $('#example').DataTable().clear().destroy();
    SetDataBind();


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

// Setup module
// ------------------------------


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
        var columns_stacked_element = document.getElementById('columns_clustered');


        //
        // Charts configuration
        //

        if (columns_stacked_element) {

            // Initialize chart
            var columns_stacked = echarts.init(columns_stacked_element);
           // ShowLoading($('#columns_clustered'));


            //
            // Chart config
            //


            var Result1 = cresult1; //myJSON.Table;

            var collectedarr = [];
            var totalhousedarr = [];
            var circlearr = [];
            var circlearr1 = [];
            var circlearr2 = [];



            for (i = 0; i < Result1.length; i++) {
                collectedarr.push(Result1[i].TotalActive);
                totalhousedarr.push(Result1[i].TotalInActive);
                circlearr.push(Result1[i].VehicleType);
                circlearr1.push(Result1[i].TotalRepair);
                circlearr2.push(Result1[i].TotalCondemed);


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
                    data: ['Active', 'InActive', 'Repair', 'Condemned'],
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
                        name: 'Active',
                        type: 'bar',
                        data: collectedarr,
                        stack: 'Advertising',

                    },
                    {
                        name: 'InActive',

                        data: totalhousedarr,
                        type: 'bar',
                        stack: 'Advertising',



                    },
                    {
                        name: 'Repair',
                        type: 'bar',
                        data: circlearr1,
                        stack: 'Advertising',

                    },
                    {
                        name: 'Condemned',
                        type: 'bar',
                        data: circlearr2,
                        stack: 'Advertising',

                    },

                ]
            });
           // HideLoading($('#columns_clustered'));






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
        var columns_stacked_element = document.getElementById('columns_stacked1');


        //
        // Charts configuration
        //

        if (columns_stacked_element) {

            // Initialize chart
            var columns_stacked = echarts.init(columns_stacked_element);
          //  ShowLoading($('#columns_stacked1'));


            //
            // Chart config
            //



            var Result2 = cresult2;//myJSON.Table1;

            var collectedarr = [];
            var totalhousedarr = [];
            var circlearr = [];

            for (i = 0; i < Result2.length; i++) {
                collectedarr.push(Result2[i].Deployed);
                totalhousedarr.push(Result2[i].NotDeployed);
                circlearr.push(Result2[i].VehicleType);

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
                    data: ['Deployed', 'Not Deployed'],
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


                ]
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
var EchartsColumnsStackedLight2 = function () {


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
           // ShowLoading($('#columns_stacked2'));


            //
            // Chart config
            //



            var Result3 = cresult3;//myJSON.Table2;

            var collectedarr = [];
            var totalhousedarr = [];
            var circlearr = [];
            var circlearr1 = [];
            var circlearr2 = [];
            var circlearr3 = [];

            for (i = 0; i < Result3.length; i++) {
                collectedarr.push(Result3[i].TotalActiveMaster);
                totalhousedarr.push(Result3[i].Deployed);
                circlearr.push(Result3[i].VehicleType);
                circlearr1.push(Result3[i].NotDeployed);
                circlearr2.push(Result3[i].DeployedReported);
                circlearr3.push(Result3[i].NotDeployedReported);


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
                    data: ['Active', 'Deployed', 'Deployed & Reported', 'Not Deployed & Reported', 'Not Deployed'],
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
                        name: 'Active',
                        type: 'bar',
                        data: collectedarr,
                        stack: 'Advertising',

                    },
                    {
                        name: 'Deployed',
                        data: totalhousedarr,
                        type: 'bar',
                        stack: 'Advertising',

                    },
                    {
                        name: 'Deployed & Reported',
                        type: 'bar',
                        data: circlearr2,
                        stack: 'Advertising',

                    },

                    {
                        name: 'Not Deployed & Reported',
                        data: circlearr3,
                        type: 'bar',
                        stack: 'Advertising',

                    },

                    {
                        name: 'Not Deployed',
                        data: circlearr1,
                        type: 'bar',
                        stack: 'Advertising',

                    },


                ]
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





function DownloadFile(FType) {

    var FromDate = document.getElementById('txtFromDate').value; //

    var ZoneId = '0';
    var CircleId = '0';
    var WardId = 0;
    var VehicleTypeId = '0';

    if (IsClick == 1) {
        ZoneId = $('#ddlZone').val();
        CircleId = $('#ddlCircle').val();
        WardId = $('#ddlWard').val();
        IsClick = 1;

        VehicleTypeId = $('#ddlVehicleType').val();

    }
    var TName = "Deployment Summary Report";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Deployment/ExportAllDeployment",
        data: { FromDate: FromDate, ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleTypeId: VehicleTypeId, FName: TName },
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
            "</td>" + "<td>" + item.TotalActive +
            "</td>" + "<td>" + item.TotalInActive +
            "</td>" + "<td>" + item.TotalRepair +
            "</td>" + "<td>" + item.TotalCondemed +
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
        url: '/Deployment/GetVehicleDepoyment',
        dataType: "json",
        data: requestModel,
        success: function (result) {
            var myJSON = JSON.parse(result);
            var Result1 = myJSON.Table;
            var Result2 = myJSON.Table1;
            var Result3 = myJSON.Table2;
            cresult1 = Result1;
            cresult2 = Result2;
            cresult3 = Result3;
            EchartsColumnsStackedLight.init();
            bindtable(Result1);
            EchartsColumnsStackedLight1.init();
            bindtable1(Result2);
            EchartsColumnsStackedLight2.init();
            bindtable2(Result3);

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
            "</td>" + "<td>" + item.NotDeployed +
            "</td>" + "<td>" + item.TotalDeployed +



            "</td> </tr>";
        $('#example1 tbody').append(rows);

    });

    var tabid = $('#example1');
    $('#hfTotalrows1').val(rowcount);
    if ($('#hfTotalrows1').val() > 0)
        setdatatable();





}


function bindtable2(data) {



    $('#example2 tbody').empty();

    if ($('#hfTotalrows2').val() > 0)
        $('#example2').DataTable().clear().destroy();

    var rowcount = data.length;


    $.each(data, function (i, item) {
        var count = i + 1;

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<td>" + item.VehicleType +
            "</td>" + "<td>" + item.TotalActiveMaster +
            "</td>" + "<td>" + item.Deployed +
            "</td>" + "<td>" + item.DeployedReported +
            "</td>" + "<td>" + item.NotDeployed +
            "</td>" + "<td>" + item.NotDeployedReported +
            /* "</td>" + "<td>" + item.Total +*/



            "</td> </tr>";
        $('#example2 tbody').append(rows);

    });

    var tabid = $('#example2');
    $('#hfTotalrows2').val(rowcount);
    if ($('#hfTotalrows2').val() > 0)
        setdatatable();




}


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
    $('#example2').DataTable({
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