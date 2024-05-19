
function AllMCityLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllCityInfo",
        data: { IsAll: "NO" },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].CId + '>' + Myjson[i].CityNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}












function AllMZoneLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllZoneInfo",
        data: { IsAll: "NO" },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].ZId + '>' + Myjson[i].ZoneNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function SelectCircleByWard1() {

    var selected_ward = $('#ddlWard').find(":selected").attr('value');

    $.ajax({
        type: "post",
        url: "/Master/SelectCircleByWard",
        data: { selected_ward: selected_ward },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            document.getElementById("txt_circle").value = Myjson[0].CircleName;

        }
    });

}


function SelectZoneByWard() {

    var selected_ward = $('#ddlWard').find(":selected").attr('value');

    $.ajax({
        type: "post",
        url: "/Master/SelectZoneByWard",
        data: { selected_ward: selected_ward },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            document.getElementById("txt_Zone").value = Myjson[0].ZoneNo;

        }
    });

}


function AllWardLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllWardInfo",
        data: { IsAll: "NO" },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].WardId+ '>' + Myjson[i].WardNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}



function SelectCircleByWard() {

    var selected_ward = $('#ddlWard').find(":selected").attr('value');


    $.ajax({
        type: "post",
        url: "/Master/SelectCircleByWard",
        data: { selected_ward: selected_ward },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            $("txt_circle").val(Myjson[0].CircleName);

        }
    });




}

function AllMCircleLst(ControlId, IsRequired, Category, ZoneId) {
    var selected_val = ZoneId;//$('#ddlZone').find(":selected").attr('value');
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllCircleByZone",
        data: { ZoneId: selected_val },
        success: function (data) {
            var myJSON = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource = Resource + '<option value=' + myJSON[i].CircleId + '>' + myJSON[i].CircleName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllMWardLst(ControlId, IsRequired, Category, ZoneId) {
    var selected_val = ZoneId;//$('#ddlZone').find(":selected").attr('value');
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/SWMMaster/GetAllWardByUser",
        data: { CircleId: selected_val },
        success: function (data) {
            var myJSON = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource = Resource + '<option value=' + myJSON[i].WardId + '>' + myJSON[i].WardNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllContainerTypeLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllContainerTypeInfo",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].CTypeId + '>' + Myjson[i].ContainerType + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllAssetStatusLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllAssetStatus",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].AssetStatusId + '>' + Myjson[i].AssetStatus + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllVehicleTypeLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllVehicleType",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {

            var Myjson = JSON.parse(data);
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
function AllOperationTypeLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllOperationType",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {

            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].OPTypeId + '>' + Myjson[i].OperationType + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllMShiftLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/SWMMaster/GetAllShift",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = data;// JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].ShiftId + '>' + Myjson[i].ShiftName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllOwnerTypeLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllOwnerType",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].OwnerTId + '>' + Myjson[i].OwnerType + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllRoleLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/User/GetAllRoles",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].fnRoleId + '>' + Myjson[i].ftRoleName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllComplaintCategoryLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllComplaintCategory",
        data: { IsAll: 'No', ComplaintTypeId: 0 },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].ComplaintTypeId + '>' + Myjson[i].ComplaintType + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllActiveVehicleLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllActiveVehicle",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].UId + '>' + Myjson[i].VehicleNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllActiveGeoPointLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllActiveGeoPoint",
        data: { ZoneId: 0 },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].GeoPointId + '>' + Myjson[i].GeoPointName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllGeoPointByZoneLst(ControlId, IsRequired, Category, ZoneId, PointCatId) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllActiveGeoPoint",
        data: { ZoneId: ZoneId, PointCatId: PointCatId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].GeoPointId + "-" + Myjson[i].Lat + "-" + Myjson[i].Lng + '>' + Myjson[i].GeoPointName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllMTripLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllTripInfo",
        data: { IsAll: "NO" },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].TMId + '>' + Myjson[i].TripName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllMActiveRouteLst(ControlId, IsRequired, Category, ZoneId, CircleId) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllActiveRoute",
        data: { ZoneId: ZoneId, CircleId: CircleId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].RouteId + '>' + Myjson[i].RouteCode + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllMActiveRouteTrip(ControlId, IsRequired, Category, RouteId) {
    var selected_val = RouteId;//$('#ddlZone').find(":selected").attr('value');
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllActiveRouteTrip",
        data: { RouteId: selected_val },
        success: function (data) {
            var myJSON = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource = Resource + '<option value=' + myJSON[i].TripId + '>' + myJSON[i].TId + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllAssignedVehicleLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllAssignedVehicleNumber",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            $("#ddlVehicleNumber").select2({

            });
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].VehicleUId + '>' + Myjson[i].VehicleNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllAssignedVehicleLst1(ControlId, IsRequired, Category, RouteId, TripId) {
    var selected_val = RouteId;//$('#ddlZone').find(":selected").attr('value');
    var selected_val_TripId = TripId;
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllAssignedVehicleNumber1",
        data: { RouteId: selected_val, TripId: selected_val_TripId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            $("#ddlVehicleNumber").select2({

            })
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].VehicleUId + '>' + Myjson[i].VehicleNo + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}

function AllMInchargeLst(ControlId, IsRequired, Category) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllDIncharge",
        data: { IsAll: "NO" },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].DIId + '>' + Myjson[i].Name + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllMDLocationLst(ControlId, IsRequired, Category, WardId) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllDLocation",
        data: { WardId: WardId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].DLId + '>' + Myjson[i].LandMark + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}
function AllMTSByWardLst(ControlId, IsRequired, Category, WardId) {
    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Master/GetAllTransferStationByWardId",
        data: { WardId: WardId },
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            var Resource = "<select id='" + ControlId + "' class='form-control'>";
            if (IsRequired == 1)
                Resource = Resource + '<option value="">Select</option>';
            else
                Resource = Resource + '<option value="' + IsRequired + '">' + Category + '</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].TSId + '>' + Myjson[i].TStationName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#' + ControlId).html(Resource);
        }
    });
}