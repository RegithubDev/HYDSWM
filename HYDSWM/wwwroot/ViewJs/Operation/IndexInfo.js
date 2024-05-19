$(document).ready(function () {
    AllMZoneLst('ddlZone', 0, 'All Zone');
    CallAllClickFunc();
    CallAllFunc();


});
function CallAllClickFunc() {
    $('#dvPScannedVehicle').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var IsVehicle = '1';
        var TransactionType = '0';
        var TName = "All Scanned Secondary Vehicle";
        window.open('/Operation/VehicleCollectionNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&IsVehicle=' + IsVehicle + '&TransactionType=' + TransactionType, "_blank");
    });
    $('#dvPNotScannedVehicle').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';
        var CollectionTypeId = '1';
        var TName = "All Not Scanned Secondary Vehicle";
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&CollectionTypeId=' + CollectionTypeId, "_blank");
    });
    $('#dvPForceTransaction').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var IsVehicle = '1';
        var TransactionType = '2';
        var TName = "All Forced Transaction";
        window.open('/Operation/VehicleCollectionNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&IsVehicle=' + IsVehicle + '&TransactionType=' + TransactionType, "_blank");
    });
    $('#dvPRejectedTransaction').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '1';
        var TransactionType = '1';
        var IsVehicle = '1';
        var TName = "All Rejected Transaction";
        window.open('/Operation/VehicleCollectionNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&IsVehicle=' + IsVehicle + '&TransactionType=' + TransactionType, "_blank");
    });
    $('#dvPScannedContainer').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '4';
        var IsVehicle = '0';
        var TName = "All Scanned Secondary Container";
        window.open('/Operation/VehicleCollectionNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&IsVehicle=' + IsVehicle, "_blank");
    });
    $('#dvPNotScannedContainer').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';
        var CollectionTypeId = '1';
        var TName = "All Not Scanned Secondary Container";
        window.open('/Asset/AllContainerNoti?TId=' + TId + '&TName=' + TName + '&CollectionTypeId=' + CollectionTypeId, "_blank");
    });

    $('#dvSScannedVehicle').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';
        var IsVehicle = '1';
        var TName = "All Scanned Linkage to Hook Loader Vehicle";
        window.open('/Operation/VehicleCollectionNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&IsVehicle=' + IsVehicle, "_blank");
    });
    $('#dvSNotScannedVehicle').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';
        var CollectionTypeId = '2';
        var TName = "All Not Scanned Linkage to Hook Loader Vehicle";
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&CollectionTypeId=' + CollectionTypeId, "_blank");
    });
    $('#dvSScannedContainer').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';
        var IsVehicle = '0';
        var TName = "All Scanned Linkage to Hook Loader Container";
        window.open('/Operation/VehicleCollectionNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&IsVehicle=' + IsVehicle, "_blank");
    });
    $('#dvSNotScannedContainer').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';
        var CollectionTypeId = '2';
        var TName = "All Not Scanned Linkage to Hook Loader Container";
        window.open('/Asset/AllContainerNoti?TId=' + TId + '&TName=' + TName + '&CollectionTypeId=' + CollectionTypeId, "_blank");
    });

    $('#dvTScannedVehicle').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '3';
        var IsVehicle = '1';
        var TName = "All Scanned Open Tipper Vehicle";
        window.open('/Operation/VehicleCollectionNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&IsVehicle=' + IsVehicle, "_blank");
    });
    $('#dvTNotScannedVehicle').click(function (e) {
        var ZId = $('#ddlZone').find(":selected").attr('value');
        var CId = $('#ddlCircle').find(":selected").attr('value');
        var WId = $('#ddlWard').find(":selected").attr('value');
        var TId = '2';
        var CollectionTypeId = '3';
        var TName = "All Not Scanned Open Tipper Vehicle";
        window.open('/Asset/AllVehicleNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&CollectionTypeId=' + CollectionTypeId, "_blank");
    });

}

function CallAllFunc() {
    ShowLoading($('#dvNotification'));
    ShowLoading($('#dvNotification1'));
    ShowLoading($('#dvNotification2'));
    $.ajax({
        type: "POST",
        url: '/Operation/GetAllCollectionNotification',
        data: { ZoneId: $('#ddlZone').find(":selected").attr('value'), CircleId: $('#ddlCircle').find(":selected").attr('value'), WardId: $('#ddlWard').find(":selected").attr('value') },
        success: function (data) {
            var myJSON = JSON.parse(data);
            $("#h2PScannedVehicle").html(myJSON.TotalScannedPrimaryVehicle);
            $("#h2PNotScannedVehicle").html(myJSON.TotalNotScannedPrimaryVehicle);
            $("#h2PForceTransaction").html(myJSON.TotalScannedPrimaryDeviatedVehicle);
            $("#h2PRejectedTransaction").html(myJSON.TotalScannedPrimaryRejectedVehicle);
            $("#h2PScannedContainer").html(myJSON.TotalScannedPrimaryContainer);
            $("#h2PNotScannedContainer").html(myJSON.TotalNotScannedPrimaryContainer);


            $("#h2SScannedVehicle").html(myJSON.TotalScannedSecondaryVehicle);
            $("#h2SNotScannedVehicle").html(myJSON.TotalNotScannedSecondaryVehicle);

            $("#h2SScannedContainer").html(myJSON.TotalScannedSecondaryContainer);
            $("#h2SNotScannedContainer").html(myJSON.TotalNotScannedSecondaryContainer);
            $("#h2SVForceTransaction").html(myJSON.TotalScannedSecondayDeviatedVehicle);
            $("#h2SVRejectedTransaction").html(myJSON.TotalScannedSecondayRejectedVehicle);
            $("#h2SCForceTransaction").html(myJSON.TotalScannedSecondayDeviatedContainer);
            $("#h2SCRejectedTransaction").html(myJSON.TotalScannedSecondayRejectedContainer);

            $("#h2TScannedVehicle").html(myJSON.TotalScannedTertiaryVehicle);
            $("#h2TNotScannedVehicle").html(myJSON.TotalNotScannedTertiaryVehicle);

            HideLoading($('#dvNotification'));
            HideLoading($('#dvNotification1'));
            HideLoading($('#dvNotification2'));
        },
        error: function (result) {
            HideLoading($('#dvNotification'));
            HideLoading($('#dvNotification1'));
            HideLoading($('#dvNotification2'));
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