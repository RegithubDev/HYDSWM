document.addEventListener('DOMContentLoaded', function () {
    AllOperationTypeLst('ddlOperationType', 0, 'All Operation Type');
    GetAllTransferStation();
    CallAllClickFunc();
    CallAllFunc('Load');


});

function CallAllClickFunc() {
    $('#dvTotalAutoTipper').click(function (e) {
        var TId = '1,4';
        var IsVehicle = '1';
        var VehicleTypeId = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Scanned Primary Mini Tippers Vehicle";
        window.open('/Operation/GetAllOpt1VehicleNoti?TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId + '&TSId=' + TSId, "_blank");

    });
    $('#dvTotalSwachAuto').click(function (e) {
        var TId = '1,4';
        var IsVehicle = '1';
        var VehicleTypeId = '5';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Scanned Primary Ghmc Swatch Auto Vehicle";
        window.open('/Operation/GetAllOpt1VehicleNoti?TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId + '&TSId=' + TSId, "_blank");

    });
    $('#dvTotalPvtSwachAuto').click(function (e) {
        var TId = '1,4';
        var IsVehicle = '1';
        var VehicleTypeId = '6';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Scanned Primary Pvt Swatch Auto Vehicle";
        window.open('/Operation/GetAllOpt1VehicleNoti?TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId + '&TSId=' + TSId, "_blank");

    });
    $('#dvTotalHKLVehicle').click(function (e) {
        var TId = '2';
        var IsVehicle = '1';
        var VehicleTypeId = '3';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Scanned Hook Loaders Vehicle";
        window.open('/Operation/GetAllOpt1VehicleNoti?TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId + '&TSId=' + TSId, "_blank");

    });
    $('#dvTotalRCCompVehicle').click(function (e) {
        var TId = '2';
        var IsVehicle = '1';
        var VehicleTypeId = '2';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Scanned RC Compactors Vehicle";
        window.open('/Operation/GetAllOpt1VehicleNoti?TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId + '&TSId=' + TSId, "_blank");

    });
    $('#dvTotalTipperVehicle').click(function (e) {
        var TId = '3';
        var IsVehicle = '1';
        var VehicleTypeId = '4';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Scanned Tippers Vehicle";
        window.open('/Operation/GetAllOpt1VehicleNoti?TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId + '&TSId=' + TSId, "_blank");

    });
    $('#dvTotalVehicle').click(function (e) {
        var TId = '0';
        var IsVehicle = '1';
        var VehicleTypeId = '0';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Scanned Primary Vehicle";
        window.open('/Operation/GetAllOpt1VehicleNoti?TId=' + TId + '&TName=' + TName + '&VehicleTypeId=' + VehicleTypeId + '&TSId=' + TSId, "_blank");

    });

    $('#dvActiveContainer').click(function (e) {
        var TId = '1';
        var IsCompleted = '0';
        var Status = '0';
        var UIDType = 'CONTAINER';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Active Container";
        window.open('/Operation/GetAllOpt1ArvlEntityNoti?TId=' + TId + '&TName=' + TName + '&IsCompleted=' + IsCompleted + '&TSId=' + TSId + '&UIDType=' + UIDType + '&Status=' + Status, "_blank");

    });
    $('#dvContainerFilled').click(function (e) {
        var TId = '1';
        var Status = '0';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Container Filled";
        window.open('/Operation/AllOpt1ContainerNoti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
    $('#dvAwaitingHL').click(function (e) {
        var TId = '1';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Awaiting For Pickup By Hookloader";
        window.open('/Operation/AllOpt1ContainerNoti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
    $('#dvContainerAVGTime').click(function (e) {
        var TId = '1';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Container Average Waiting Time After Filled";
        window.open('/Operation/AllOpt1ContainerNoti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });

    $('#dvActiveHL').click(function (e) {
        var TId = '1';
        var IsCompleted = '0';
        var Status = '0';
        var UIDType = 'VEHICLE';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Active Hookloader";
        window.open('/Operation/GetAllOpt1ArvlEntityNoti?TId=' + TId + '&TName=' + TName + '&IsCompleted=' + IsCompleted + '&TSId=' + TSId + '&UIDType=' + UIDType + '&Status=' + Status, "_blank");

    });

    $('#dvLinkedHL').click(function (e) {
        var TId = '1';
        var Status = '0';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Hookloader Linked";
        window.open('/Operation/AllOpt1HKLNoti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });

    $('#dvAwaitingForCollHL').click(function (e) {
        var TId = '1';
        var IsCompleted = '0';
        var Status = '1';
        var UIDType = 'VEHICLE';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Hookloader Awaiting For Linkage With Container";
        window.open('/Operation/GetAllOpt1ArvlEntityNoti?TId=' + TId + '&TName=' + TName + '&IsCompleted=' + IsCompleted + '&TSId=' + TSId + '&UIDType=' + UIDType + '&Status=' + Status, "_blank");

    });
    $('#dvAVGTimeHL').click(function (e) {
        var TId = '1';
        var IsCompleted = '0';
        var Status = '1';
        var UIDType = 'VEHICLE';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Average Waiting Time Of Hookloader";
        window.open('/Operation/GetAllOpt1ArvlEntityNoti?TId=' + TId + '&TName=' + TName + '&IsCompleted=' + IsCompleted + '&TSId=' + TSId + '&UIDType=' + UIDType + '&Status=' + Status, "_blank");

    });
    $('#dvOpenRCV').click(function (e) {
        var TId = '1';
        var Status = '0';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Open RCV";
        window.open('/Operation/AllOpt1RCVNoti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
    $('#dvFilledRCV').click(function (e) {
        var TId = '1';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All RCV Filled";
        window.open('/Operation/AllOpt1RCVNoti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });

    $('#dvTotalUContainer').click(function (e) {
        var TId = '1';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Unique Container";
        window.open('/Operation/AllOpt1UNQContainerNoti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
    $('#dvTotalUHL').click(function (e) {
        var TId = '1';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Unique Hookloader";
        window.open('/Operation/AllOpt1UNQHKLNoti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
    $('#dvForceTrans').click(function (e) {
        var TId = '0';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Force Transaction";
        window.open('/Operation/AllForceTransactionOpt1Noti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
    $('#dvPVForceTrans').click(function (e) {
        var TId = '1';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Primary Vehicle Force Transaction";
        window.open('/Operation/AllForceTransactionOpt1Noti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
    $('#dvHKLForceTrans').click(function (e) {
        var TId = '2';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Hookloader Force Transaction";
        window.open('/Operation/AllForceTransactionOpt1Noti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
    $('#dvContainerForceTrans').click(function (e) {
        var TId = '3';
        var Status = '1';
        var TSId = $('#ddlTransferStation').find(":selected").attr('value');
        var TName = "All Container Force Transaction";
        window.open('/Operation/AllForceTransactionOpt1Noti?TId=' + TId + '&TName=' + TName + '&Status=' + Status + '&TSId=' + TSId, "_blank");

    });
}

function CallAllFunc(Type) {
    // document.getElementById("#btnDaily").className = "fc-agendaDay-button fc-button fc-state-default fc-corner-right  fc-state-active";
    SetAllNotification(Type);
    setTimeout(function () {
        SecondaryCollectionBarChart(Type, 'Daily');
    }, 1000);
    GetScTPMapData(Type);
}

function SetAllNotification(Type) {
    var TSId = '0';
    if (Type == 'Click') {
        TSId = $('#ddlTransferStation').val();
    }

    //ShowLoading($('#dvNotification'));
    //ShowLoading($('#dvNotification1'));
    //ShowLoading($('#dvNotification2'));
    $.ajax({
        type: "POST",
        url: '/Operation/GetAllOperation1Notification',
        data: { TsId: TSId },
        success: function (data) {
            var myJSON = JSON.parse(data);
            $("#h6ActiveContainer").html(myJSON.ActiveContainer);
            $("#h6ContainerFilled").html(myJSON.ContainerFilled);
            $("#h6AwaitingHL").html(myJSON.AwaitingForPickupHL);
            $("#h6ContainerAVGTime").html(myJSON.ContainerAVGWaitingHr);

            $("#h6TotalAutoTipper").html(myJSON.TotalAutoTipper);
            $("#h6TotalSwachAuto").html(myJSON.TotalGHMCAuto);
            $("#h6TotalPvtSwachAuto").html(myJSON.TotalPVTSwachAuto);
            $("#h6TotalHKLVehicle").html(myJSON.TotalHKL);
            $("#h6TotalRCCompVehicle").html(myJSON.TotalRCCompactor);
            $("#h6TotalTipperVehicle").html(myJSON.h6TotalTipperVehicle);
            $("#h6TotalVehicle").html(myJSON.TotalVehicle);

            $("#h6TotalUContainer").html(myJSON.UniqueContainer);
            $("#h6TotalUHL").html(myJSON.UniqueHL);
            $("#h6ForceTrans").html(myJSON.ForceTransaction);
            $("#h6PVForceTrans").html(myJSON.PVForceTransaction);
            $("#h6HKLForceTrans").html(myJSON.HKLForceTransaction);
            $("#h6ContainerForceTrans").html(myJSON.ContainerForceTransaction);


            $("#h6ActiveHL").html(myJSON.ActiveHookLoader);
            $("#h6LinkedHL").html(myJSON.LinkedHL);
            $("#h6AwaitingForCollHL").html(myJSON.AwaitingForContainer);
            $("#h6AVGTimeHL").html(myJSON.HLAVGWaitingHr);

            $("#h6OpenRCV").html(myJSON.OpenRCV);
            $("#h6FilledRCV").html(myJSON.ClosedRCV);

            //HideLoading($('#dvNotification'));
            //HideLoading($('#dvNotification1'));
            //HideLoading($('#dvNotification2'));
        },
        error: function (result) {
            //HideLoading($('#dvNotification'));
            //HideLoading($('#dvNotification1'));
            //HideLoading($('#dvNotification2'));
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

function SecondaryCollectionBarChart(Type, SearchType) {
    var TSId = '0';
    var OperationTypeId = '0';
    if (Type == 'Click') {
        TSId = $('#ddlTransferStation').val();
        OperationTypeId = $('#ddlOperationType').val();
    }
    "use strict";
    // based on prepared DOM, initialize echarts instance
    var myChart = echarts.init(document.getElementById("basic-bar"));
    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: '/Operation/GetAllScannedVehicleForBarChart',
        data: { TsId: TSId, SearchType: SearchType, OperationTypeId: OperationTypeId },
        //  dataType: "json",
        success: function (data) {

            var myJSON = JSON.parse(data);

            var TotalAssetarr = [];
            var AssetNamearr = [];
            for (i = 0; i < myJSON.length; i++) {
                TotalAssetarr.push(myJSON[i].TotalVehicle);

                AssetNamearr.push(myJSON[i].ShowDate);
            }

            // specify chart configuration item and data
            var option = {
                // Setup grid
                grid: {
                    left: "1%",
                    right: "2%",
                    bottom: "3%",
                    containLabel: true,
                },

                // Add Tooltip
                tooltip: {
                    trigger: "axis",
                },

                legend: {
                    data: ["Total Vehicle"],
                },
                toolbox: {
                    show: true,
                    feature: {
                        magicType: { show: true, type: ["line", "bar"] },
                        restore: { show: true },
                        saveAsImage: { show: true },
                    },
                },
                color: ["#009efb", "#7460ee"],
                calculable: true,
                xAxis: [
                    {
                        type: "category",
                        data: AssetNamearr,
                        axisLabel: {
                            color: '#333',
                            rotate: 70,
                        }
                    },
                ],
                yAxis: [
                    {
                        type: "value",
                    },
                ],
                series: [
                    {
                        name: "Total Vehicle",
                        type: "bar",
                        data: TotalAssetarr,
                        markPoint: {
                            data: [
                                { type: "max", name: "Max" },
                                { type: "min", name: "Min" },
                            ],
                        },
                        //markLine: {
                        //    // data: [{ type: "average", name: "Average" }],
                        //    data: [{

                        //        name: 'average line',
                        //        type: 'average'
                        //    }],
                        //},
                    },

                ],
            };
            // use configuration item and data specified to show chart
            myChart.setOption(option);

        },
        error: function (result) {
            HideLoading($('#columns_basic'));
            ShowCustomMessage('0', 'Something Went Wrong!', '');
        }
    });


    $(function () {
        // Resize chart on menu width change and window resize
        $(window).on("resize", resize);
        $(".sidebartoggler").on("click", resize);

        // Resize function
        function resize() {
            setTimeout(function () {
                // Resize chart
                myChart.resize();
            }, 200);
        }
    });

}

function GetScTPMapData(Type) {
    var date = new Date();



    var zid = "0";
    var cid = "0";
    var VId = "0";
    var FDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + '00:00';
    var TDate = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + date.getHours() + ':' + date.getMinutes();
    var TSId = "0";
    var TStatus = $("input[name='MStatus']:checked").val();

    if (Type == 'Click') {
        IsClick = 1;
        TSId = $('#ddlTransferStation').val();

    }
    $.ajax({
        type: "POST",
        // contentType: "application/json; charset=utf-8",
        url: '/Operation/GetOpMapDashboardLst',
        dataType: "json",
        data: { zid: zid, cid: cid, VehicleTypeId: VId, TSId: TSId, FromDate: FDate, ToDate: TDate, TStatus: TStatus },
        success: function (result) {

            var locations = JSON.parse(result);

            var infowindow = new google.maps.InfoWindow();

            var lat = locations.length ? locations[0].Lat : 17.4940964;
            var longt = locations.length ? locations[0].Lng : 78.4000115;
            var map = new google.maps.Map(document.getElementById('map_basic'), {
                zoom: 14,
                center: new google.maps.LatLng(lat, longt),
                mapTypeId: google.maps.MapTypeId.terrain

            });

            var contentString;
            var marker, i;
            var latlngbounds = new google.maps.LatLngBounds();
            var iconBase = '../otherfiles/global_assets/images/';
            for (i = 0; i < locations.length; i++) {

                var myLatlng = new google.maps.LatLng(locations[i].Lat, locations[i].Lng);
                var icon;
                // if (locations[i].state == 1 || locations[i].IsScannedToday == 1)
                if (locations[i].CStatus == 1)
                    icon = iconBase + "greenhome.png";
                else
                    icon = iconBase + "redhome.png";
                contentString = '';
                contentString += '<table class="table table-bordered border-success">';
                contentString += '<thead>';
                contentString += '<tbody>';

                contentString += '<tr><td>Transfer Station:</td><td style="align:right;">' + locations[i].TSName + '</td></tr>';
                contentString += '<tr><td>No. Of Primary Vehicle:</td><td style="align:right;">' + locations[i].TotalPrimaryVehicle + '</td></tr>';
                contentString += '<tr><td>Estimated Weight(MT):</td><td style="align:right;">' + locations[i].SecondaryVehicleWT + '</td></tr>';
                contentString += '<tr><td>No. Of Container Filled:</td><td style="align:right;">' + locations[i].TotalContainer + '</td></tr>';
                contentString += '<tr><td>No. Of Hookloader Trips:</td><td style="align:right;">' + locations[i].TotalHookLoaderTrip + '</td></tr>';
                contentString += '<tr><td>No. Of Open Vehicle Trips:</td><td style="align:right;">' + locations[i].TotalOpenVehicleTrip + '</td></tr>';
                contentString += '<tr><td>No. Of RCV Vehicle Trips:</td><td style="align:right;">' + locations[i].TotalRCVVehicle + '</td></tr>';
                contentString += '<tr><td>Total Weight As Per WB Data:</td><td style="align:right;">' + locations[i].TotalWBWeight + '</td></tr>';
                contentString += '<tr><td>Last Transaction On:</td><td style="align:right;">' + locations[i].LastTransOn + '</td></tr>';

                contentString += '</tbody>';
                contentString += '</table>';

                //contentString = "<div style='float:right; padding: 10px;font-size: 14px;background-color: #33414E;color: white;'>Transfer Station-" + locations[i].TSName +
                //    "<br/>No. Of Primary Vehicle-" + locations[i].TotalPrimaryVehicle +
                //    "<br/>Estimated Weight(MT)-" + locations[i].SecondaryVehicleWT +
                //    "<br/>No. Of Container Filled-" + locations[i].TotalContainer +
                //    "<br/>No Of Hookloader Trips-" + locations[i].TotalHookLoaderTrip +
                //    "<br/>No. Of Open Vehicle Trips-" + locations[i].TotalOpenVehicleTrip +
                //    "<br/>No. Of RCV Vehicle Trips-" + locations[i].TotalRCVVehicle +
                //    "<br/>Total Weight As Per WB Data-" + locations[i].TotalWBWeight +
                //    "<br/>Last Transaction On-" + locations[i].LastTransOn +

                //    "</div>";



                marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    icon: icon,
                    content: contentString
                });

                google.maps.event.addListener(marker, 'click', (function (marker, i) {
                    return function () {
                        infowindow.setContent(marker.content);
                        infowindow.open(map, marker);
                    }
                })(marker, i));
                //google.maps.event.addListener(marker, 'mouseout', (function (marker, i) {
                //    return function () {
                //        infowindow.close(map, marker);
                //    }
                //})(marker, i));
                //Extend each marker's position in LatLngBounds object.
                latlngbounds.extend(marker.position);
            }

            //Get the boundaries of the Map.
            var bounds = new google.maps.LatLngBounds();

            //Center map and adjust Zoom based on the position of all markers.
            map.setCenter(latlngbounds.getCenter());
            map.fitBounds(latlngbounds);
        },
        error: function (result) {
        }
    });
}

setInterval(function () {
    GetScTPMapData('Load') // this will run after every 5 seconds
}, 900000);


