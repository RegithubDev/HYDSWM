function goBack() {
    window.history.back()
}
$(document).ready(function () {
    var ddId = getUrlParameterInfo('cid');

    AllActiveVehicleLst('ddlVehicleNo', 0, 'Select');

    if (ddId > 0)
        SetDataOncontrols(ddId);

});


function Formsubmit() {

    SaveAndUpdateRouteTripInfo();
    return false;

}
function CallFunc(obj) {

    var ddId = $(obj).attr('cid');
    $('#user_content').load("/Collection/AddNRoute");
    $('#modal_form_vertical').modal('toggle');
    setTimeout(() => {
        //$("#ddlVehicleNo").select2({
        //    dropdownParent: $("#modal_form_vertical")
        //});
        $("#ddlStop").select2({
            dropdownParent: $("#modal_form_vertical")
        });
        AllActiveVehicleLst('ddlVehicleNo', 0, 'Select');
        AllActiveGeoPointLst('ddlStop', 0, 'Select');
        AllMTripLst('ddlTrip', 0, 'Select');
    }, 2000);

    if (ddId > 0)
        setTimeout(() => {
            SetDataOncontrols(ddId);
        }, 3000);

}
function SetDataOncontrols(ddId) {
    $.ajax({
        type: "post",
        url: "/Collection/GetPointsInfoByTripId",
        data: { RTDId: ddId },
        success: function (data) {

            var myJSONData = JSON.parse(data);
            var nstopdata = myJSONData.Table[0];
            //  var myJSON = JSON.parse(data);
            $("#hfRouteId").val(nstopdata.RouteId);
            $("#hfRTDId").val(nstopdata.RTDId);
            $("#txtRouteCode").val(nstopdata.RouteCode);
            $('#hfTripId').val(nstopdata.TripId);
            $('#txtTripUId').val(nstopdata.TId);
            $('#txtTrip').val(nstopdata.TripName);
           
            setTimeout(() => {
                $('#ddlVehicleNo').val(nstopdata.VehicleUId).trigger('change');
            }, 1000);

        }
    });
}
function SaveAndUpdateRouteTripInfo() {
    var isvalid = 1;

    var RTDId = $("#hfRTDId").val()
    var VUId = $("#ddlVehicleNo").val()

    if (VUId == '')
        isvalid = 0;

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Collection/AddVehicleToTrip',
            data: { RTDId: RTDId, VehicleUId: VUId },
            success: function (data) {
                var myJSON = JSON.parse(data);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg, '/Collection/AllTripPoint');

                    //$('#modal_form_vertical').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJSON.Msg, '');

            },
            error: function (result) {
                ShowMessage('0', 'Something Went Wrong!');
            }
        });
    else {

        ShowCustomMessage('0', 'Please Enter All Required Details', '');
    }
}