$(document).ready(function () {
    var TId = getUrlParameterInfo('TId');
    callfunc(TId, 'Load');
    // Add event listener for opening and closing details
    $('#example3 tbody').on('click', 'td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#example3').DataTable().row(tr);
        var CTDId = tr.find('.gticket').attr('cticketid');
        var TDate = tr.find('.gticket').attr('TDate');

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var iccData;
            $.ajax({
                url: "/Operation/GetAllMPrimaryVehicleTransactionByContainer",
                type: "POST",
                dataType: "json",
                data: { CTDId: CTDId, TDate: TDate },
                success: function (data) {
                    var myJSON = JSON.parse(data);
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });
});
function format(item) {
    var InnerGrid = '<div class="table-scrollable"><table class="table table-bordered table-striped"><thead><tr><th>Sr. No.</th><th>Image</th> <th> Operation Type </th><th> Vehicle No </th><th> Vehicle Type </th><th> Vehicle UId </th><th> Transfer Station </th><th> Is Deviated </th><th> Remarks </th><th> Scanned On </th><th> Scanned By </th><th> Status </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;
        var Ftransaction = '';
        var isdeviated = ''
        if (info.IsDeviated == false)
            isdeviated = 'INSIDE';
        else
            isdeviated = 'OUTSIDE';

        if (info.ActionStatus == 'FORCE TRANSACTION')
            Ftransaction = '<span class="badge badge-danger">' + info.ActionStatus + '</span>';
        else if (info.ActionStatus == 'REJECTED TRANSACTION')
            Ftransaction = '<span class="badge badge-warning">' + info.ActionStatus + '</span>';
        else
            Ftransaction = '<span class="badge badge-success">' + info.ActionStatus + '</span>';

        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td><a class="gticket" cticketid = "' + info.CTDId + '" href = "' + info.ImgUrl + '" data-fancybox="gallery" > <img id="imageresource" src="' + info.ImgUrl + '" alt="" class="img-preview rounded"></a></td>' +
            '<td>' + info.OperationType + '</td>' +
            '<td>' + info.VehicleNo + '</td>' +
            '<td>' + info.VehicleType + '</td>' +
            '<td>' + info.UId + '</td>' +
            '<td>' + info.TStationName + '</td>' +
            '<td>' + isdeviated + '</td>' +
            '<td>' + info.Remarks + '</td>' +
            '<td>' + info.CreatedOn + '</td>' +
            // '<td><span class="' + info.LabelClass + '">' + info.AssetStatus + '</span></td>' +

            '<td>' + info.CreatedBy + '</td>' +
            '<td>' + Ftransaction + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}
function callfunc(CollectionTypeId, Type) {
    if (CollectionTypeId == 1) {

        document.getElementById("dv1").style.display = "block";
        GetDataTableData(Type);
    }
    else if (CollectionTypeId == 2) {
        document.getElementById("dv2").style.display = "block";
        GetDataTableData1(Type);
    }
    else if (CollectionTypeId == 3) {
        document.getElementById("dv3").style.display = "block";
        GetDataTableData2(Type);
    }
    else if (CollectionTypeId == 4) {
        document.getElementById("dv4").style.display = "block";
        GetDataTableData3(Type);
    }
}
function CallFuncModal(obj) {
    // loadMap();
    var ddId = $(obj).attr('cid');
    $("#hfCTDId").val('0');
    $("#hfCTDId").val(ddId);
    $('#modal_form_vertical').modal('toggle');

}
function Formsubmit() {

    ChangeStatusOfTransaction();
    return false;
}
function ChangeStatusOfTransaction() {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var ZId = getUrlParameterInfo('ZId');
    var CId = getUrlParameterInfo('CId');
    var WId = getUrlParameterInfo('WId');
    var IsVehicle = getUrlParameterInfo('IsVehicle');
    var TransactionType = getUrlParameterInfo('TransactionType');

    var isvalid = 1;
    var FormData = {
        CTDId: $("#hfCTDId").val(),
        StatusId: $("#ddlStatus").val(),
    };
    if (FormData.CTDId == '' || FormData.StatusId == '')
        isvalid = 0;

    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Operation/ChangeStatusOfTransaction',
            data: { jobj: JSON.stringify(FormData) },
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Operation/VehicleCollectionNoti?ZId=' + ZId + '&CId=' + CId + '&WId=' + WId + '&TId=' + TId + '&TName=' + TName + '&IsVehicle=' + IsVehicle + '&TransactionType=' + TransactionType);

                    $('#modal_form_vertical').modal('toggle');
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
var dt;
var dt1;
var dt2;
var dt3;
function GetDataTableData(Type) {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var ZId = getUrlParameterInfo('ZId');
    var CId = getUrlParameterInfo('CId');
    var WId = getUrlParameterInfo('WId');
    var IsVehicle = getUrlParameterInfo('IsVehicle');
    var TransactionType = getUrlParameterInfo('TransactionType');

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
    }

    dt = $('#example').DataTable({
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
            url: "/Operation/GetAllCollectionNoti/",
            type: 'POST',
            data: function (d) {
                d.NotiId = TId;
                d.ZoneId = ZId;
                d.CircleId = CId;
                d.WardId = WId;
                d.VehicleTypeId = IsVehicle;
                d.Status = TransactionType;
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
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.CTDId + '" href="' + row.ImgUrl + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.ImgUrl + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "OperationType", sortable: true },
            { data: "ContainerCode", sortable: true },
            { data: "ContainerName", sortable: true },
            { data: "Step1UId", sortable: true },
            /*   { data: "Step1CreatedOn", sortable: true },*/
            { data: "VehicleType", sortable: true },
            { data: "VehicleNo", sortable: true },
            { data: "Step2UId", sortable: true },
            /*{ data: "Step2CreatedOn", sortable: true },*/
            { data: "TStationName", sortable: true },
            /*{ data: "CreatedBy", sortable: false },*/
            { data: "CreatedOn", sortable: false },
            { data: "CreatedBy", sortable: false },
            { data: "ContactNo", sortable: false },
            { data: "Remarks", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.IsDeviated == 1)
                        return '<span class="badge badge-danger">OUTSIDE</span>';
                    else
                        return '<span class="badge badge-success">INSIDE</span>';

                }
            }
            ,
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.ActionStatus == 'FORCE TRANSACTION' && TransactionType == '2')
                        return "<a cid='" + row.CTDId + "' href='javascript:void(0);' title='edit' onclick='CallFuncModal(this);'>" + row.ActionStatus + "</a>";
                    else
                        return row.ActionStatus;

                }
            }
        ]
    });

    //if (TId != "")
    //    dt.column(8).visible(false);
}
function GetDataTableData1(Type) {

    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var ZId = getUrlParameterInfo('ZId');
    var CId = getUrlParameterInfo('CId');
    var WId = getUrlParameterInfo('WId');
    var IsVehicle = getUrlParameterInfo('IsVehicle');

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
    }

    dt1 = $('#example1').DataTable({
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
            url: "/Operation/GetAllCollectionNoti/",
            type: 'POST',
            data: function (d) {
                d.NotiId = TId;
                d.ZoneId = ZId;
                d.CircleId = CId;
                d.WardId = WId;
                d.VehicleTypeId = IsVehicle;
                d.Status = "0";
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
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.CTDId + '" href="' + row.ImgUrl + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.ImgUrl + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "OperationType", sortable: true },

            { data: "VehicleType", sortable: true },
            { data: "VehicleNo", sortable: true },
            { data: "Step1UId", sortable: true },
            /*   { data: "Step1CreatedOn", sortable: true },*/
            { data: "ContainerCode", sortable: true },
            { data: "ContainerName", sortable: true },
            { data: "Step2UId", sortable: true },
            /*{ data: "Step2CreatedOn", sortable: true },*/
            { data: "CreatedOn", sortable: false },
            { data: "CreatedBy", sortable: false },
            { data: "ContactNo", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.IsDeviated == 1)
                        return '<span class="badge badge-danger">OUTSIDE</span>';
                    else
                        return '<span class="badge badge-success">INSIDE</span>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    //if (row.ActionStatus == 'FORCE TRANSACTION' && TransactionType == '2')
                    //    return "<a cid='" + row.CTDId + "' href='javascript:void(0);' title='edit' onclick='CallFuncModal(this);'>" + row.ActionStatus + "</a>";
                    //else
                    return row.ActionStatus;

                }
            }
        ]
    });


}
function GetDataTableData2(Type) {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var ZId = getUrlParameterInfo('ZId');
    var CId = getUrlParameterInfo('CId');
    var WId = getUrlParameterInfo('WId');
    var IsVehicle = getUrlParameterInfo('IsVehicle');

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
    }

    dt2 = $('#example2').DataTable({
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
            url: "/Operation/GetAllCollectionNoti/",
            type: 'POST',
            data: function (d) {
                d.NotiId = TId;
                d.ZoneId = ZId;
                d.CircleId = CId;
                d.WardId = WId;
                d.VehicleTypeId = IsVehicle;
                d.Status = "0";
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
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.CTDId + '" href="' + row.ImgUrl + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.ImgUrl + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "OperationType", sortable: true },

            { data: "VehicleType", sortable: true },
            { data: "VehicleNo", sortable: true },
            { data: "Step1UId", sortable: true },
            { data: "CreatedOn", sortable: false },
            { data: "CreatedBy", sortable: false },
            { data: "ContactNo", sortable: false },
        ]
    });


}

function GetDataTableData3(Type) {
    var TId = getUrlParameterInfo('TId');
    if (TId == "1")
        TId = "4";
    var TName = getUrlParameterInfo('TName');

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
    }

    dt3 = $('#example3').DataTable({
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
            url: "/Operation/GetAllPrimaryTransaction/",
            type: 'POST',
            data: function (d) {
                d.NotiId = TId;
                d.Status = "0";
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
                "className": 'details-control',
                "orderable": false,
                "data": null,

                "defaultContent": ''
            },
            {
                sortable: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },

            { data: "OperationType", sortable: true },
            { data: "ContainerCode", sortable: true },
            { data: "ContainerName", sortable: true },
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    return '<span class="gticket" cticketid="' + row.CTDId + '" TDate="' + row.CreatedDate + '">' + row.Step1UId + '</span>';

                }
            },
            { data: "CreatedOn", sortable: false },
            { data: "LastActivityOn", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.Status == 'IN PROGRESS')
                        return '<span class="badge badge-danger">' + row.Status + '</span>';
                    else
                        return '<span class="badge badge-success">' + row.Status + '</span>';

                }
            },
            { data: "CreatedBy", sortable: false },
            { data: "ContactNo", sortable: false },
        ]
    });


}


