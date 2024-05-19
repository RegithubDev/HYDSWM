function goBack() {
    window.history.back()
}
function Formsubmit() {

    SaveAndUpdateNouteInfo();
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

$(document).ready(function () {
    var ddId = getUrlParameterInfo('cid');
    AllMZoneLst('ddlZone', 1, 'Select');
    AllMShiftLst('ddlShift', 1, 'Select');
    if (ddId > 0)
        SetDataOncontrols(ddId);

});

function CallFCircleByZone() {
    $('#ddlCircle').val('');
    AllMCircleLst('ddlCircle', 1, 'Select', $('#ddlZone').find(":selected").attr('value'));
}

function SetDataOncontrols(ddId) {
    $.ajax({
        type: "post",
        url: "/Collection/GetSRouteInfoById",
        data: { RouteId: ddId },
        success: function (data) {


            var myJSON = JSON.parse(data);
            //var myJSON = myJSONData.Table[0];

            $("#hfRouteId").val(myJSON.RouteId);
            //$("#txtRouteName").val(myJSON.RouteName);
            $("#txtRouteCode").val(myJSON.RouteCode);

            $("#ddlZone").val(myJSON.ZoneId).change();
            setTimeout(() => {
                $("#ddlCircle").val(myJSON.CircleId);
                $("#ddlShift").val(myJSON.ShiftId);
            }, 1000);
           
            
            if (myJSON.IsActive == "True")
                $("#ckbIsActive").prop("checked", true);
            else
                $("#ckbIsActive").prop("checked", false);

           
        }
    });
}
function Formsubmit() {

    SaveAndUpdateNouteInfo();
    return false;

}
function SaveAndUpdateNouteInfo() {

    var isvalid = 1;
    var FormData = {
        RouteId: $("#hfRouteId").val(),
       // RouteName: $("#txtRouteName").val(),
        RouteCode: $("#txtRouteCode").val(),
        ZoneId: $('#ddlZone').val(),
        CircleId: $("#ddlCircle").val(),
        ShiftId: $("#ddlShift").val(),
        // BufferMin: $("#txtBufferMin").val(),
        IsActive: $('#ckbIsActive').is(':checked')
    };

    if ( FormData.ZoneId == '' || FormData.CircleId == '' || FormData.ShiftId == '')
        isvalid = 0;
    
    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Collection/AddSRouteInfo',
            data: { jobj: JSON.stringify(FormData) },
            success: function (data) {
                var myJSON = JSON.parse(data);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg, '/Collection/AllSRoute');

                   // $('#modal_form_vertical').modal('toggle');
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

function GetNewRouteCode() {
    var ZoneId = $('#ddlZone').val();
    var CircleId = $("#ddlCircle").val();
    var RouteId = $("#hfRouteId").val();
    if (RouteId == '') {
        $.ajax({
            type: "post",
            url: "/Collection/GetNewRouteCode",
            data: { ZoneId: ZoneId, CircleId: CircleId },
            success: function (data) {


                var myJSON = JSON.parse(data);

                $("#txtRouteCode").val(myJSON.RouteCode);

            }
        });
    }
    
}