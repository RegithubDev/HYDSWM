var tablarr = [];

function AddTrips() {
    var IsValid = 1;

    if ($("#ddlShift").val() == 0) {
        ShowCustomMessage('0', 'Please Select Shift', '');
    }
   // var Tripname = $("#ddlMTrip option:selected").text();
    //var TripId = $('#ddlMTrip').val();
   // var VehicleNo = $("#ddlVehicleNo option:selected").text();
    var ShiftName = $("#ddlShift option:selected").text();
    var ShiftId = $("#ddlShift option:selected").val();
    var VUId = "";//$('#ddlVehicleNo').val();
    var BufferMin = $('#txtBufferMin').val();
    var TId = $('#txtTId').val();
    var TripId = TId.split('-')[3].replace('T', '');;

    
    if ( BufferMin == '' || TId=='')
        IsValid = 0

    if (IsValid == 1) {
        var IsExist = false;
        $('#tblTrip > tbody > tr').each(function (i, el) {
            var $tds = $(this).find('td');
           // var ITripId = $(this).attr('data-tripid');
            var ITId = $tds.eq(1).text();
            if ( ITId == TId)
                IsExist = true;

        });

        if (IsExist == false) {

            var data = {};
            data.TripId = TripId;
            data.VehicleUId = VUId;
            data.TId = TId;
            data.BufferMin = BufferMin;
            data.ShiftId = ShiftId;
            data.ShiftName = ShiftName;
            tablarr.push(data);

            var tr = '<tr  data-tripid="' + TripId + '" data-vuid="' + VUId + '"><td>' + TId +
               /* '</td><td>' + TId +*/
              /*  '</td><td>' + VehicleNo +*/
                '</td><td>' + BufferMin +
                '</td><td>' + ShiftName +
                '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveTripRow(this);"> <i class="icon-cancel-circle2"></i> Remove</button>'
            '</td></tr>';
            $('#tblTrip > tbody').append(tr);

            ChangeIdText();
        }
        else
            ShowCustomMessage('0', 'This Route Trip Code is already exists', '');
    }
    else
        ShowCustomMessage('0', 'Please Fill All Required Details', '');

}

function RemoveTripRow(objthis) {
    var tripid = $(objthis).closest('tr').attr('data-tripid');

    tablarr = tablarr.filter(function (obj) {
        return parseInt(obj.TripId) !== parseInt(tripid);
    });
    
    $("#tblTrip > tbody").html("");

    var IICount = 1;
    var Itablarr = [];
    for (var i = 0; i < tablarr.length; i++) {

        var RouteCode = $('#txtRouteCode').val();
        var Id = RouteCode + "-T" + IICount;

        var data = {};
        data.TripId = IICount;
        data.VehicleUId = tablarr[i].VehicleUId;
        data.TId = Id;
        data.BufferMin = tablarr[i].BufferMin;
        data.ShiftId = tablarr[i].ShiftId;
        data.ShiftName = tablarr[i].ShiftName;
        Itablarr.push(data);

        
        var tr = '<tr  data-tripid="' + IICount + '" data-vuid="' + tablarr[i].VehicleUId + '"><td>' + Id +
            /* '</td><td>' + TId +*/
            /*  '</td><td>' + VehicleNo +*/
            '</td><td>' + tablarr[i].BufferMin +
            '</td><td>' + tablarr[i].ShiftName +
            '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveTripRow(this);"> <i class="icon-cancel-circle2"></i> Remove</button>'
        '</td></tr>';
        $('#tblTrip > tbody').append(tr);
        IICount++;
    }
    tablarr = [];
    tablarr = Itablarr;
    $(objthis).closest('tr').remove();
    ChangeIdText();
}

function goBack() {
    window.history.back()
}
$(document).ready(function () {
    var ddId = getUrlParameterInfo('cid');

   // AllActiveVehicleLst('ddlVehicleNo', 0, 'Select');
    AllMTripLst('ddlMTrip', 0, 'Select');
    AllShiftLst('ddlShift', 0, 'Select')

    if (ddId > 0)
        SetDataOncontrols(ddId);
    else
        ChangeIdText();

});
function ChangeIdText() {
    //var Tripname = $("#ddlMTrip option:selected").text();
    
    var RouteCode = $('#txtRouteCode').val();
    var IICount = tablarr.length+1;

    var Id = RouteCode + "-T" + IICount;
    $('#txtTId').val(Id);

}

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
        url: "/Collection/GetAllRouteTripByRoute",
        data: { RouteId: ddId },
        success: function (data) {

            var myJSONData = JSON.parse(data);
            var nstopdata = myJSONData.Table[0];
            var myJSON = myJSONData.Table1;
          //  var myJSON = JSON.parse(data);
            $("#hfRouteId").val(ddId);
            $("#txtRouteCode").val(nstopdata.RouteCode);
            $("#txtZone").val(nstopdata.ZoneNo);
            $("#txtCircle").val(nstopdata.CircleName);
            

            for (var i = 0; i < myJSON.length; i++) {

                var data = {};
                data.TripId = myJSON[i].TripId ;
                data.VehicleUId = myJSON[i].VehicleUId;
                data.TId = myJSON[i].TId;
                data.BufferMin = myJSON[i].BufferMin;
                data.ShiftId = myJSON[i].ShiftId;
                data.ShiftName = myJSON[i].ShiftName;
                tablarr.push(data);

                var tr = '<tr  data-tripid=' + myJSON[i].TripId + ' data-vuid="' + myJSON[i].VehicleUId + '"><td >' + myJSON[i].TId +
                   /* '</td><td>' + myJSON[i].VehicleNo +*/
                    '</td><td>' + myJSON[i].BufferMin +
                    '</td><td>' + myJSON[i].ShiftName +
                    '</td><td><button type="button" class="btn btn-danger rounded-round"  onclick="RemoveTripRow(this);"> <i class="icon-close2"></i> Remove</button>'
                '</td></tr>';
                $('#tblTrip > tbody').append(tr);
            }
            ChangeIdText();
           
        }
    });
}
function SaveAndUpdateRouteTripInfo() {
    
    var isvalid = 1;
  
    //var tablarr =  [];
    //$('#tblTrip > tbody > tr').each(function (i, el) {
        
    //    var $tds = $(this).find('td');
    //    var tripid = $(this).attr('data-tripid');
    //    var TId = $tds.eq(1).text();
    //    var BufferMin = $tds.eq(2).text();
    //    var vuid = ""; //$(this).attr('data-vuid');
    //    // var SDeptTime = $tds.eq(2).text();
    //    var data = {};
    //    data.TripId = tripid;
    //    data.VehicleUId = vuid;
    //    data.TId = TId;
    //    data.BufferMin = BufferMin;
    //    tablarr.push(data);
    //});

    
    if (tablarr.length == 0)
        isvalid = 0

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Collection/AddRouteTrip',
            data: { RouteId: $("#hfRouteId").val(), JArrayval: JSON.stringify(tablarr) },
            success: function (data) {
                var myJSON = JSON.parse(data);
                if (myJSON.Result == 1 || myJSON.Result == 2) {

                    ShowCustomMessage('1', myJSON.Msg, '/Collection/AllRouteTrip');

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
function AllShiftLst(ControlId, IsRequired, Category) {

    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/SWMMaster/AllShiftInfo",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            $("#ddlShift").select2({

            });
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {

                if (Myjson[i].ShiftId <= 4) {
                    Resource = Resource + '<option value=' + Myjson[i].ShiftId + '>' + Myjson[i].ShiftName + '</option>';
                }

            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}