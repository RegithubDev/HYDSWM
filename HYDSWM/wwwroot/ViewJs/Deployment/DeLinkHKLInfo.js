$(document).ready(function () {
    GetDataTableData('Load');
    GetAllTransferStation();
    GetAllHookloader();
});

var dt3;
function GetDataTableData(Type) {

    var TSId = "0";

    if (Type == 'Click')
        TSId = $('#ddlSTransferStation').val();

    dt3 = $('#example').DataTable({
        processing: true,
        destroy: true,
        responsive: true,
        serverSide: true,
        searchable: true,
        lengthMenu: [[10, 20, 50, 100, 500, 10000, -1], [10, 20, 50, 100, 500, 10000, "All"]],
        language: {
            //infoEmpty: "No records available",
            searchPlaceholder: "Search Records",
            search: ""
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
                    text: '<i class="icon-file-spreadsheet mr-2"></i> Excel',
                    extension: '.csv'
                },
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i>',
                    className: 'btn bg-blue btn-icon dropdown-toggle'
                }
            ]
        },
        initComplete: function () {
            $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'off');
        },
        ajax: {
            url: "/Deployment/GetAllHLForDLink_Paging/",
            type: 'POST',
            data: function (d) {
                d.Status = TSId;
                return {

                    requestModel: d
                };
            },
            dataType: "json",
            dataSrc: function (json) {

                json.draw = json.draw;
                json.recordsTotal = json.recordsTotal;
                json.recordsFiltered = json.recordsFiltered;
                json.data = json.data;
                var return_data = json;
                return return_data.data;
            }
        },
        columns: [

            {
                sortable: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            { data: "ContainerCode", sortable: true },
            { data: "VehicleNo", sortable: true },
            { data: "TStationName", sortable: true },
            { data: "CreatedOn", sortable: true },
            { data: "CreatedBy", sortable: false },
            { data: "ReplaceVehicleNo", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    return "<a cid='" + row.CTDId + "' href='javascript:void(0);' title='edit' onclick='CallFunc(this);'><i class='ti-pencil'></i></a>";
                }
            }
        ]
    });


}

function Formsubmit() {

    ReplaceHKLInfo();
    return false;
}
function CallFunc(obj) {

    var ddId = $(obj).attr('cid');
    $("#hfDHLTId").val(ddId);
    //$('#user_content').load("/Asset/AddContainer");
    $('#modal_form_AddDetail').modal('toggle');

}

function GetAllTransferStation() {
    $('#ddlSTransferStation').html('');

    $.ajax({
        type: "post",
        url: "/Operation/GetAllTransferStationByUser",
        data: '{}',
        success: function (data) {
            var myJSON = JSON.parse(data);

            var Resource1 = "<select id='ddlSTransferStation'>";
            Resource1 = Resource1 + '<option value="0">All Transfer Station</option>';
            // Resource = Resource + '<option value="0">N/A</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource1 = Resource1 + '<option value=' + myJSON[i].TSId + '>' + myJSON[i].TStationName + '</option>';
            }

            Resource1 = Resource1 + '</select>';
            $('#ddlSTransferStation').html(Resource1);
        }

    });
}

function GetAllHookloader() {
    $('#ddlVehicle').html('');

    $.ajax({
        type: "post",
        url: "/Deployment/GetAllUnAllocatedHKL",
        data: '{}',
        success: function (data) {
            var myJSON = JSON.parse(data);

            var Resource1 = "<select id='ddlVehicle'>";
            Resource1 = Resource1 + '<option value="">Select</option>';
            // Resource = Resource + '<option value="0">N/A</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource1 = Resource1 + '<option value=' + myJSON[i].UID + '>' + myJSON[i].VehicleNo + '</option>';
            }

            Resource1 = Resource1 + '</select>';
            $('#ddlVehicle').html(Resource1);
        }

    });
}

function ReplaceHKLInfo() {

    var isvalid = 1;
    var DHLTId = $("#hfDHLTId").val();
    var Remarks = $("#txtRemarks").val();
    var UId = $("#ddlVehicle").val()

    if (UId == '' || DHLTId == '' || DHLTId == '0' || Remarks == '')
        isvalid = 0;

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Deployment/AddDelinkHL',
            data: { CTDId: DHLTId, UId: UId, Remarks: Remarks },
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Deployment/AllDeLinkHL');

                    $('#modal_form_AddDetail').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJson.Msg, '');

            },
            error: function (result) {
                ShowCustomMessage('0', 'Something Went Wrong!', '');
            }
        });
    else
        ShowCustomMessage('0', 'Please Enter All Required Details', '');

}