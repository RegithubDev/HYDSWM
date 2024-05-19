$(document).ready(function () {

    GetDataTableData('Load');
    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function () {

        var tr = $(this).closest('tr');
        var row = $('#example').DataTable().row(tr);
        //var routecode = tr.find('td:eq(5)').text();
        var ticketid = tr.find('.gticket').attr('cticketid');
        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        }
        else {
            // Open this row
            var iccData;
            $.ajax({
                url: "/Asset/GetVehicleActionStatusById",
                type: "POST",
                dataType: "json",
                data: { VehicleId: ticketid },
                success: function (data) {
                    var myJSON = JSON.parse(data);
                    row.child(format(myJSON)).show();
                    tr.addClass('shown');
                }
            });

        }


    });
});

function boxDisable() {
    if ($('#ckbIsManual').is(':checked')) {
        $('#txtUId').removeAttr('disabled');
    } else {
        $('#txtUId').attr('disabled', 'disabled');
        $('#txtUId').val("");
    }
}
function isNumberKey(evt, element) {
    var charCode = (evt.which) ? evt.which : event.keyCode
    if (charCode > 31 && (charCode < 48 || charCode > 57) && !(charCode == 46 || charCode == 8))
        return false;
    else {
        var len = $(element).val().length;
        var index = $(element).val().indexOf('.');
        if (index > 0 && charCode == 46) {
            return false;
        }
        if (index > 0) {
            var CharAfterdot = (len + 1) - index;
            if (CharAfterdot > 3) {
                return false;
            }
        }

    }
    return true;
}

function Formsubmit() {

    SaveAndUpdateVehicleInfo();
    return false;
}
function CallFunc(obj) {

    var ddId = $(obj).attr('cid');
    $('#user_content').load("/Asset/AddVehicle");
    $('#modal_form_AddDetail').modal('toggle');

    setTimeout(function () {
        AllMZoneLst('ddlZone', 1, 'Select');
        AllVehicleTypeLst('ddlVehicleType', 1, 'Select');
        if (ddId > 0)
            AllAssetStatusLst('ddlStatus', 1, 'Select');
        else
            AllAssetStatusLst('ddlStatus', 0, 'Select');
    }, 1000);

    if (ddId > 0)
        SetDataOncontrols(ddId);
}
var dt;
function GetDataTableData(Type) {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var ZId = getUrlParameterInfo('ZId');
    var CId = getUrlParameterInfo('CId');
    var WId = getUrlParameterInfo('WId');
    var VehicleTypeId = getUrlParameterInfo('VehicleTypeId');
    var CollectionTypeId = "0";
    CollectionTypeId = getUrlParameterInfo('CollectionTypeId');
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
            url: "/Asset/GetAllVehicleInfo/",
            type: 'POST',
            data: function (d) {
                d.Status = TId;
                d.ZoneId = ZId;
                d.CircleId = CId;
                d.WardId = WId;
                d.NotiId = CollectionTypeId;
                d.VehicleTypeId = VehicleTypeId;
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
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.VehicleId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },
            {
                sortable: true,
                "render": function (data, type, row, meta) {
                    return '<span class="' + row.LabelClass + '">' + row.AssetStatus + '</span>';
                }
            },
            { data: "UId", sortable: true },
            { data: "VehicleNo", sortable: true },
            { data: "ChassisNo", sortable: true },
            { data: "VehicleType", sortable: true },
            { data: "OwnerType", sortable: true },
            { data: "OperationType", sortable: true },
            { data: "ZoneNo", sortable: true },
            { data: "CircleName", sortable: true },
            { data: "WardNo", sortable: true },
            { data: "GrossWt", sortable: true },
            { data: "TareWt", sortable: true },
            { data: "NetWt", sortable: true }
        ]
    });

    //if (TId != "")
    //    dt.column(8).visible(false);
}
function format(item) {
    var InnerGrid = '<div class="table-scrollable"><table class="table table-bordered table-striped"><thead><tr><th>Sr. No.</th> <th> Status </th><th> Remarks </th><th> Modified On </th></tr></thead><tbody>';
    $.each(item, function (i, info) {
        var Icount = i + 1;

        InnerGrid += '<tr>' +
            '<td>' + Icount + '</td>' +
            '<td><span class="' + info.LabelClass + '">' + info.AssetStatus + '</span></td>' +
            '<td>' + info.Remarks + '</td>' +
            '<td>' + info.SCreatedOn + '</td>' +

            '</tr>';
    });

    InnerGrid += '</tbody></table></div>';



    return InnerGrid;
}
function StatusChange() {
    var stval = $('#ddlStatus').val();
    var defaultval = $('#hfStatus').val();
    if (stval == defaultval) {
        $("#txtARemarks").val('');
        $('#txtARemarks').attr('readonly', 'true');
    }
    else {
        $("#txtARemarks").val('');
        $("#txtARemarks").removeAttr("readonly")
    }
}
function DownloadQrCode(objthis) {

    var UHouseId = $(objthis).attr('data-QUID');
    window.location = "/Asset/DownLoadQR?UId=" + UHouseId;
}
