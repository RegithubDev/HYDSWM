$(document).ready(function () {
    GetDataTableData('Load');

});





function GetDataTableData(Type) {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var TSId = getUrlParameterInfo('TSId');
    var VehicleTypeId = getUrlParameterInfo('VehicleTypeId');

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
            url: "/Operation/GetAllVehicleOpt1Noti/",
            type: 'POST',
            data: function (d) {
                d.NotiId = TId;
                d.Status = TSId;
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
                sortable: false,
                render: function (data, type, row, meta) {
                    return meta.row + meta.settings._iDisplayStart + 1;
                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.CTDId + '" href="' + row.ImgUrl + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.ImgUrl + '" alt="" class="img-preview rounded"></a>';
                    //  return '<a class="gticket" cticketid="' + row.CTDId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
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
            /*{ data: "Remarks", sortable: false },*/
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    if (row.IsDeviated == 1)
                        return '<span class="badge badge-danger">OUTSIDE</span>';
                    else
                        return '<span class="badge badge-success">INSIDE</span>';

                }
            }
            //,
            //{
            //    sortable: false,
            //    "render": function (data, type, row, meta) {

            //        if (row.ActionStatus == 'FORCE TRANSACTION' && TransactionType == '2')
            //            return "<a cid='" + row.CTDId + "' href='javascript:void(0);' title='edit' onclick='CallFuncModal(this);'>" + row.ActionStatus + "</a>";
            //        else
            //            return row.ActionStatus;

            //    }
            //}
        ]
    });

    //if (TId != "")
    //    dt.column(8).visible(false);
}



function DownloadFile(FType) {

    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    var TSId = getUrlParameterInfo('TSId');
    var VehicleTypeId = getUrlParameterInfo('VehicleTypeId');

    if (TId != '') {
        $("#spnHeader").html('');
        $("#spnHeader").html(TName);
    }


    var TName = "Vehicle Operation";

    ShowLoading($('#example'));
    $.ajax({
        type: "POST",
        url: "/Operation/ExportAllOptVehicle",
        data: { NotiId: TId, Status: TSId, VehicleTypeId: VehicleTypeId, FName: TName },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#example'));
            var ctype = '';
            if (FType == 'Excel')
                ctype = 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet';
            else
                ctype = 'application/pdf';

            var filename = TName;//
            var blob = new Blob([response], { type: ctype });
            var DownloadUrl = URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = DownloadUrl;
            if (FType == 'Excel')
                a.download = filename + ".xlsx";
            else
                a.download = filename + ".pdf";
            document.body.appendChild(a);
            a.click();
        },
        error: function (result) {
            HideLoading($('#example'));
        }
    });
}