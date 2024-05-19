$(document).ready(function () {
    CallAllFunc();
    CallAllClickFunc();

});

function CallAllFunc() {
    ShowLoading($('#dvNotification'));
    $.ajax({
        type: "POST",
        url: '/Complaint/GetAllComplaintNotification',
        data: '{}',
        success: function (data) {
            var myJSON = JSON.parse(data);
            $("#h2TotalComplaint").html(myJSON.TotalComplaint);
            $("#h2PendingComplaint").html(myJSON.TotalPendingComplaint);
            $("#h2ClosedComplaint").html(myJSON.TotalClosedComplaint);

            HideLoading($('#dvNotification'));
        },
        error: function (result) {
            HideLoading($('#dvNotification'));
        }
    });

}
function CallAllClickFunc() {
    $('#dvTotalComplaint').click(function (e) {
        var TId = '';
        var TName = "All Complaints";
        window.open('/Complaint/AllComplaintNoti?TId=' + TId + '&TName=' + TName , "_blank");
    });
    $('#dvPendingComplaint').click(function (e) {
        var TId = '0';
        var TName = "All Pending Complaints";
        window.open('/Complaint/AllComplaintNoti?TId=' + TId + '&TName=' + TName, "_blank");
    });
    $('#dvClosedComplaint').click(function (e) {
        var TId = '1';
        var TName = "All Closed Complaints";
        window.open('/Complaint/AllComplaintNoti?TId=' + TId + '&TName=' + TName, "_blank");
    });
    
}