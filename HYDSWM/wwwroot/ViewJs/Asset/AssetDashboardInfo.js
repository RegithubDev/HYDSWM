var EchartsColumnsBasicDark = function () {



    //
    // Setup module components
    //

    // Basic column chart
    var _columnsBasicDarkExample = function () {

        if (typeof echarts == 'undefined') {
            console.warn('Warning - echarts.min.js is not loaded.');
            return;
        }


        // Define element
        var columns_basic_element = document.getElementById('columns_basic');


        //
        // Charts configuration
        //

        if (columns_basic_element) {
            // Initialize chart
            var columns_basic = echarts.init(columns_basic_element);
            ShowLoading($('#columns_basic'));
            $.ajax({
                type: "POST",
                // contentType: "application/json; charset=utf-8",
                url: '/Asset/GetZoneWiseVehicle',
                data: { ZoneId: $('#ddlZone').find(":selected").attr('value') },
                //  dataType: "json",
                success: function (data) {
                    var myJSON = JSON.parse(data);

                    var TotalAssetarr = [];
                    var AssetNamearr = [];
                    for (i = 0; i < myJSON.length; i++) {
                        TotalAssetarr.push(myJSON[i].TotalVehicle);

                        AssetNamearr.push(myJSON[i].ZoneNo);
                    }

                    // Options
                    columns_basic.setOption({

                        // Define colors
                        color: ['#b6a2de', '#2ec7c9', '#d87a80', '#ffb980', '#d87a80'],

                        // Global text styles
                        textStyle: {
                            fontFamily: 'Roboto, Arial, Verdana, sans-serif',
                            fontSize: 13
                        },

                        // Chart animation duration
                        animationDuration: 750,

                        // Setup grid
                        grid: {
                            left: 0,
                            right: 40,
                            top: 35,
                            bottom: 0,
                            containLabel: true
                        },

                        // Add legend
                        legend: {
                            data: ['Total Vehicle'],
                            itemHeight: 8,
                            itemGap: 20,
                            textStyle: {
                                padding: [0, 5]

                            }
                        },

                        // Add tooltip
                        tooltip: {
                            trigger: 'axis',
                            backgroundColor: 'rgba(0,0,0,0.75)',
                            padding: [10, 15],
                            textStyle: {
                                fontSize: 13,
                                fontFamily: 'Roboto, sans-serif'
                            }
                        },

                        // Horizontal axis
                        xAxis: [{
                            type: 'category',
                            data: AssetNamearr,//['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                            axisLabel: {
                                color: '#333',
                                rotate: 70,
                            },
                            axisLine: {
                                lineStyle: {
                                    color: '#999'
                                }
                            },
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    color: '#eee',
                                    type: 'dashed'
                                }
                            }
                        }],

                        // Vertical axis
                        yAxis: [{
                            type: 'value',
                            axisLabel: {
                                color: '#333'
                            },
                            axisLine: {
                                lineStyle: {
                                    color: '#999'
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: ['#eee']
                                }
                            },
                            splitArea: {
                                show: true,
                                areaStyle: {
                                    color: ['rgba(250,250,250,0.1)', 'rgba(0,0,0,0.01)']
                                }
                            }
                        }],



                        // Add series
                        series: [
                            {
                                name: 'Total Vehicle',
                                type: 'bar',
                                data: TotalAssetarr,//[2.0, 4.9, 7.0, 23.2, 25.6, 76.7, 135.6, 162.2, 32.6, 20.0, 6.4, 3.3],
                                itemStyle: {
                                    normal: {
                                        label: {
                                            show: true,
                                            position: 'top',
                                            textStyle: {
                                                fontWeight: 500
                                            }
                                        }
                                    }
                                }
                            }
                        ]
                    });
                    HideLoading($('#columns_basic'));
                },
                error: function (result) {
                    HideLoading($('#columns_basic'));
                    ShowCustomMessage('0', 'Something Went Wrong!', '');
                }
            });


        }


        //
        // Resize charts
        //

        // Resize function
        var triggerChartResize = function () {
            columns_basic_element && columns_basic.resize();
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
        window.addEventListener('load', function () {
            clearTimeout(resizeCharts);
            resizeCharts = setTimeout(function () {
                triggerChartResize();
            }, 200);
        })
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function () {

            _columnsBasicDarkExample();
        }
    }
}();


document.addEventListener('DOMContentLoaded', function () {
    AllMZoneLst('ddlZone', 0, 'All Zone');
    AllVehicleTypeLst('ddlVehicleType', 0, 'All Vehicle Type');
    CallAllClickFunc();
    CallAllFunc();


});

function CallAllClickFunc() {
    $('#dvTotalAsset').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');

        var TId = '0';
        var TName = "All Vehicle";
        var VehicleTypeId = $('#ddlVehicleType').find(":selected").attr('value');
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId, "_blank");
    });
    $('#dvRegister').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var TName = "All Register Vehicle";
        var VehicleTypeId = $('#ddlVehicleType').find(":selected").attr('value');
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId, "_blank");
    });
    $('#dvInUse').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TName = "In Use Vehicle";
        var TId = "2";
        var VehicleTypeId = $('#ddlVehicleType').find(":selected").attr('value');
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId, "_blank");
    });
    $('#dvInStock').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '3';
        var TName = "In Stock Vehicle";
        var VehicleTypeId = $('#ddlVehicleType').find(":selected").attr('value');
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId, "_blank");
    });
    $('#dvInRepair').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '4';
        var TName = "In Repair Vehicle";
        var VehicleTypeId = $('#ddlVehicleType').find(":selected").attr('value');
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId, "_blank");
    });
    $('#dvInDead').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '5';
        var TName = "In Dead Vehicle";
        var VehicleTypeId = $('#ddlVehicleType').find(":selected").attr('value');
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId, "_blank");
    });
    $('#dvInScrapped').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '6';
        var TName = "In Scrapped Vehicle";
        var VehicleTypeId = $('#ddlVehicleType').find(":selected").attr('value');
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId, "_blank");
    });


    $('#dvTotalCNTAsset').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '0';
        var TName = "All Container";
        window.open('/Asset/AllContainerNoti?TId=' + TId + '&TName=' + TName, "_blank");
    });

    $('#dvCNTInUse').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TName = "In Use Container";
        var TId = '2';
        window.open('/Asset/AllContainerNoti?TId=' + TId + '&TName=' + TName, "_blank");
    });
    $('#dvCNTInStock').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '3';
        var TName = "In Stock Container";
        window.open('/Asset/AllContainerNoti?TId=' + TId + '&TName=' + TName, "_blank");
    });
    $('#dvCNTInRepair').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '4';
        var TName = "In Repair Container";
        window.open('/Asset/AllContainerNoti?TId=' + TId + '&TName=' + TName, "_blank");
    });
    $('#dvCNTInDead').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '5';
        var TName = "In Dead Container";
        window.open('/Asset/AllContainerNoti?TId=' + TId + '&TName=' + TName, "_blank");
    });
    $('#dvCNTInScrapped').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '6';
        var TName = "In Scrapped Container";
        window.open('/Asset/AllContainerNoti?TId=' + TId + '&TName=' + TName, "_blank");
    });
}

function CallAllFunc() {
    EchartsColumnsBasicDark.init();
    ShowLoading($('#dvNotification'));
    ShowLoading($('#dvNotification1'));
    $.ajax({
        type: "POST",
        url: '/Asset/GetAllAssetNotification',
        data: { ZoneId: $('#ddlZone').find(":selected").attr('value'), CircleId: $('#ddlCircle').find(":selected").attr('value'), WardId: $('#ddlWard').find(":selected").attr('value'), VehicleTypeId: $('#ddlVehicleType').find(":selected").attr('value') },
        success: function (data) {
            var myJSON = JSON.parse(data);
            $("#h2TotalAsset").html(myJSON.TotalAsset);
            $("#h2InUse").html(myJSON.TotalInUse);
            $("#h2InStock").html(myJSON.TotalInStock);
            $("#h2InRepair").html(myJSON.TotalInRepair);
            $("#h2InDead").html(myJSON.TotalInDead);
            $("#h2InScrapped").html(myJSON.TotalInScrapped);

            $("#h2TotalCNTAsset").html(myJSON.TotalCNTAsset);
            $("#h2CNTInUse").html(myJSON.TotalCNTInUse);
            $("#h2CNTInStock").html(myJSON.TotalCNTInStock);
            $("#h2CNTInRepair").html(myJSON.TotalCNTInRepair);
            $("#h2CNTInDead").html(myJSON.TotalCNTInDead);
            $("#h2CNTInScrapped").html(myJSON.TotalCNTInScrapped);

            HideLoading($('#dvNotification'));
            HideLoading($('#dvNotification1'));
        },
        error: function (result) {
            HideLoading($('#dvNotification'));
            HideLoading($('#dvNotification1'));
        }
    });

}
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    $('#ddlWard').val('0');
    $('#ddlWard').trigger("change.select2");
    AllMCircleLst('ddlCircle', 0, 'All Circle', $('#ddlZone').find(":selected").attr('value'));
}
function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'All Ward', $('#ddlCircle').find(":selected").attr('value'));
}

