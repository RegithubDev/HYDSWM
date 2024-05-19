var tablpointarr = [];
var today = new Date().toLocaleDateString();
function goBack() {
    window.history.back()
}
function AddStops() {
    var IsValid = 1;
    var stopid = $('#ddlStop').val();
    var stopname = $("#ddlStop option:selected").text();
    var Tripname = $("#ddlTrip option:selected").text();
    var TrInfo = $('#ddlTrip').val();
    var TripId = TrInfo.split(',')[0];
    var BufferMin = TrInfo.split(',')[1];
    var VUId = TrInfo.split(',')[2];
    var PickupTime = $('#txtPickupTime').val();
    //var SArrvlTime = $('#txtSArrvlTime').val();
    //var SDeptTime = $('#txtSDeptTime').val();
    if (stopid == '' || PickupTime == '' || TripId == '')
        IsValid = 0

    if (IsValid == 1) {
        var IsExist = false;
        $('#tblStop > tbody > tr').each(function (i, el) {
            var $tds = $(this).find('td');
            var Istopid = $(this).attr('data-StopId'); 

            if (Istopid == stopid)
                IsExist = true;

        });
        if (IsExist == false) {

            var data = {};
            data.BufferMin = BufferMin;
            data.StopId = stopid;
            data.PickupTime = PickupTime;
            data.TripId = TripId;
            data.TripName = Tripname;
            data.StopName = stopname;
            data.VehicleUId = VUId;
            //data.DeptTime = SDeptTime;
            tablpointarr.push(data);

            const sorted = tablpointarr.sort((a, b) => {
                
                const aDate = new Date(today + ' ' + a.PickupTime)
                const bDate =  new Date(today + ' ' + b.PickupTime)
              
                return bDate.getTime() - aDate.getTime();
            })
            
            $("#tblStop > tbody").html("");
            for (var i = 0; i < sorted.length; i++) {
                var tr = '<tr data-StopId=' + sorted[i].StopId + ' data-tripid=' + sorted[i].TripId + '><td >' + sorted[i].StopName +
                    '</td><td>' + sorted[i].PickupTime +
                    '</td><td >' + sorted[i].TripName +
                    '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveRow(this);"> <i class="icon-close2"></i> Remove</button>'
                '</td></tr>';
                $('#tblStop > tbody').append(tr);
            }
        }
        else
            ShowCustomMessage('0', 'This geo point is already exists', '');
    }
    else
        ShowCustomMessage('0', 'Please Fill All Required Details', '');

}
function AddTrips() {
    var IsValid = 1;
    var Tripname = $("#ddlMTrip option:selected").text();
    var TripId = $('#ddlMTrip').val();
    var VehicleNo = $("#ddlVehicleNo option:selected").text();
    var VUId = $('#ddlVehicleNo').val();
    var BufferMin = $('#txtBufferMin').val();
    
    if (Tripname == '' || BufferMin == '')
        IsValid = 0

    if (IsValid == 1) {
        var IsExist = false;
        $('#tblTrip > tbody > tr').each(function (i, el) {
            var ITripId = $(this).attr('data-tripid');
            
            if (TripId == ITripId)
                IsExist = true;

        });

        if (IsExist == false) {
            var tr = '<tr  data-tripid="' + TripId + '"><td>' + Tripname +
                '</td><td>' + VehicleNo +
                '</td><td>' + BufferMin +
                '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveTripRow(this);"> <i class="icon-cancel-circle2"></i> Remove</button>'
            '</td></tr>';
            $('#tblTrip > tbody').append(tr);


            // $('#txtBufferMin').val('0');

            var Tinfo = TripId + "," + BufferMin + "," + VUId;
            $('#ddlTrip')
                .append($('<option>', { value: Tinfo })
                    .text(Tripname));
            // $('#txtSDeptTime').val('');
            //$('#ddlStop').val('');
        }
        else
            ShowCustomMessage('0', 'This Trip is already exists', '');
    }
    else
        ShowCustomMessage('0', 'Please Fill All Required Details', '');

}
function RemoveRow(objthis) {
    var stopid = $(objthis).closest('tr').attr('data-StopId');

    tablpointarr = tablpointarr.filter(function (obj) {
        return obj.StopId !== parseInt(stopid);
    });
    $(objthis).closest('tr').remove();
}
function RemoveTripRow(objthis) {
    var tripid = $(objthis).closest('tr').attr('data-tripid');
    $("#ddlTrip option[value='" + tripid + "']").remove();
    if (tablpointarr.length > 0) {
        
        tablpointarr = tablpointarr.filter(function (obj) {
            return obj.TripId !== parseInt(tripid);
        });
        const sorted = tablpointarr.sort((a, b) => {

            const aDate = new Date(today + ' ' + a.PickupTime)
            const bDate = new Date(today + ' ' + b.PickupTime)

            return bDate.getTime() - aDate.getTime();
        })
        
        $("#tblStop > tbody").html("");
        for (var i = 0; i < sorted.length; i++) {
            var tr = '<tr data-StopId=' + sorted[i].StopId + ' data-tripid=' + sorted[i].TripId + '><td >' + sorted[i].StopName +
                '</td><td>' + sorted[i].PickupTime +
                '</td><td >' + sorted[i].TripName +
                '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveRow(this);"> <i class="icon-close2"></i> Remove</button>'
            '</td></tr>';
            $('#tblStop > tbody').append(tr);
        }
    }
    $(objthis).closest('tr').remove();
}

function GetStopsByRouteCode(myJSON) {


    for (var i = 0; i < myJSON.length; i++) {
        var tr = '<tr data-StopId=' + myJSON[i].StopId + ' data-tripid=' + myJSON[i].TripId + '><td >' + myJSON[i].StopName +
            '</td><td>' + myJSON[i].PickupTime +
            '</td><td >' + myJSON[i].TripName +
            '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveRow(this);"> <i class="icon-close2"></i> Remove</button>'
        '</td></tr>';
        $('#tblStop > tbody').append(tr);
    }


}

function SetTripsByRouteCode(myJSON) {


    for (var i = 0; i < myJSON.length; i++) {

        $('#ddlTrip')
            .append($('<option>', { value: myJSON[i].TInfo })
                .text(myJSON[i].TripName));

        var tr = '<tr  data-tripid=' + myJSON[i].TripId + '><td >' + myJSON[i].TripName +
            '</td><td>' + myJSON[i].VehicleNo +
            '</td><td>' + myJSON[i].BufferMin +
            '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveTripRow(this);"> <i class="icon-close2"></i> Remove</button>'
        '</td></tr>';
        $('#tblTrip > tbody').append(tr);
    }


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

    AllActiveVehicleLst('ddlVehicleNo', 0, 'Select');
    AllActiveGeoPointLst('ddlStop', 0, 'Select');
    AllMTripLst('ddlMTrip', 0, 'Select');

    if (ddId > 0)
        SetDataOncontrols(ddId);

});


function SetDataOncontrols(ddId) {
    $.ajax({
        type: "post",
        url: "/Collection/GetNRouteInfoById",
        data: { RouteId: ddId },
        success: function (data) {


            var myJSONData = JSON.parse(data);
            var myJSON = myJSONData.Table[0];
            var nstopdata = myJSONData.Table1;
            var ntripdata = myJSONData.Table2;
            $("#hfRouteId").val(myJSON.RouteId);
            $("#txtRouteName").val(myJSON.RouteName);
            $("#txtRouteCode").val(myJSON.RouteCode);
           // $("#txtBufferMin").val(myJSON.BufferMin);
            //setTimeout(() => {
            //    $("#ddlVehicleNo").val(myJSON.VehicleUId).trigger('change');
            //}, 3000);
            // $("#ddlVehicleNo > [value=" + myJSON.VehicleUId + "]").attr("selected", "true");
            //$("#txtArrvlTime").val(myJSON.Arvltime);
            //$("#txtDeptTime").val(myJSON.DeptTime);
            if (myJSON.IsActive == true)
                $("#ckbIsActive").prop("checked", true);
            else
                $("#ckbIsActive").prop("checked", false);

            if (nstopdata != '') {
                GetStopsByRouteCode(nstopdata);
                tablpointarr = nstopdata;
            }

            if (ntripdata != '')
                SetTripsByRouteCode(ntripdata);
        }
    });
}
function SaveAndUpdateNouteInfo() {

    var isvalid = 1;
    var FormData = {
        RouteId: $("#hfRouteId").val(),
        RouteName: $("#txtRouteName").val(),
        RouteCode: $("#txtRouteCode").val(),
        //VehicleUId: $('#ddlVehicleNo').val(),
        // ArrvlTime: $("#txtArrvlTime").val(),
        // DeptTime: $("#txtDeptTime").val(),
       // BufferMin: $("#txtBufferMin").val(),
        IsActive: $('#ckbIsActive').is(':checked')
    };
    var tablarr = tablpointarr;// [];
    //$('#tblStop > tbody > tr').each(function (i, el) {

    //    var $tds = $(this).find('td');
    //    var stopid = $(this).attr('data-StopId');
    //    var PickupTime = $tds.eq(1).text();
    //    var TripId = $(this).attr('data-tripid');
    //    // var SDeptTime = $tds.eq(2).text();
    //    var data = {};
    //    data.StopId = stopid;
    //    data.PickupTime = PickupTime;
    //    data.TripId = TripId;
    //    //data.DeptTime = SDeptTime;
    //    tablarr.push(data);
    //});

    if (FormData.RouteName == '' )
        isvalid = 0;
    if (tablarr.length == 0)
        isvalid = 0

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Collection/AddNRouteInfo',
            data: { jobj: JSON.stringify(FormData), JArrayval: JSON.stringify(tablarr) },
            success: function (data) {
                var myJSON = JSON.parse(data);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg, '/Collection/AllNRoute');

                    $('#modal_form_vertical').modal('toggle');
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