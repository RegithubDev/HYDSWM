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
                url: "/Asset/GetTStationInfoById",
                type: "POST",
                dataType: "json",
                data: { CMId: ticketid },
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

function Formsubmit() {

    SaveAndUpdateContainerInfo();
    return false;
}
function CallFunc(obj) {

    var ddId = $(obj).attr('cid');
    $('#user_content').load("/Asset/AddContainer");
    $('#modal_form_AddDetail').modal('toggle');

    setTimeout(function () {
        AllContainerTypeLst('ddlContainerType', 1, 'Select');
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
            url: "/Asset/GetAllContainerInfo/",
            type: 'POST',
            data: function (d) {
                d.ZoneId = "0";
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
        'createdRow': function (row, data, dataIndex) {
            $(row).attr('UId', data.UId);
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
                    return '<p><input type="checkbox" dvid=' + row.UId + ' class="cbIsAssigned" /></p>';
                }
            },

            {
                sortable: true,
                "render": function (data, type, row, meta) {
                    return '<span class="' + row.LabelClass + '">' + row.AssetStatus + '</span>';
                }
            },
            { data: "UId", sortable: true },
            {
                "render": function (data, type, row, meta) {
                    return "<a  data-QUID='" + row.UId +
                        "'data-ContainerCode='" + row.ContainerCode +
                        "'data-ContainerName='" + row.ContainerName +
                        "'data-ContainerType='" + row.ContainerType +
                        "' href=javascript:void(0)  onclick=GetQrCodeB64(this);  >Print QrCode</a>";

                }
            },
            { data: "ContainerCode", sortable: true },
            { data: "ContainerName", sortable: true },
            { data: "Capacity", sortable: true },
            { data: "ContainerType", sortable: true },

            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.CMId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    if (TId == "") {
                        return "<a cid='" + row.CMId + "' href='javascript:void(0);' title='edit' onclick='CallFunc(this);'><i class='ti-pencil'></i></a>";
                    }
                    else {

                        return '';
                    }
                }
            }
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
function SaveAndUpdateContainerInfo() {

    var isvalid = 1;
    var input = {
        CMId: $("#hfCMId").val(),
        Containercode: $("#txtContainercode").val(),
        ContainerName: $("#txtContainerName").val(),
        ContainerCapacity: $("#txtContainerCapacity").val(),
        UId: $("#txtUId").val(),
        IsManual: $('#ckbIsManual').is(':checked'),
        ContainerTypeId: $("#ddlContainerType").val(),
        //IsActive: $('#ckbIsActive').is(':checked'),
        StatusId: $("#hfCMId").val() == 0 ? 2 : $("#ddlStatus").val(),
        ARemarks: $("#hfCMId").val() == 0 ? $("#txtRemarks").val() : $("#txtARemarks").val(),
        ExStatusVal: $("#hfStatus").val(),
        UserId: $("#hfUserId").val(),
    };
    if (input.Containercode == '' || input.ContainerName == '' || input.ContainerCapacity == '' || input.ContainerTypeId == '')
        isvalid = 0;
    if (input.IsManual == true)
        if (input.UId == '')
            isvalid = 0;
    var formData = new FormData();
    formData.append('file', $('#files')[0].files[0]);
    formData.append('input', JSON.stringify(input));
    if (isvalid == 1)
        $.ajax({
            type: "POST",
            url: '/Asset/SaveAndUpdateContainer',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Asset/AllContainer');

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
function SetDataOncontrols(ddId) {
    $.ajax({
        type: "post",
        url: "/Asset/GetContainerInfoById",
        data: { CMId: ddId },
        success: function (data) {
            $('#txtContainercode').css("display", "block");
            $('#dvStatus').css("display", "block");
            $('#txtARemarks').attr('readonly', 'true');
            var myJSON = JSON.parse(data);
            $("#hfCMId").val(myJSON.CMId);
            $("#txtContainercode").val(myJSON.ContainerCode);
            $("#txtContainerName").val(myJSON.ContainerName);
            $("#txtContainerCapacity").val(myJSON.Capacity);
            if (myJSON.IsManualUId == 'True')
                $("#ckbIsManual").prop("checked", true);
            else
                $("#ckbIsManual").prop("checked", false);
            $("#txtUId").val(myJSON.UId);
            $('#txtUId').attr('disabled', 'disabled');
            $('#ckbIsManual').css("display", "none");
            setTimeout(() => {
                $("#ddlContainerType").val(myJSON.ContainerTypeId);
            }, 2000);
            //if (myJSON.IsActive == 'True')
            //    $("#ckbIsActive").prop("checked", true);
            //else
            //    $("#ckbIsActive").prop("checked", false);

            $("#hfStatus").val(myJSON.StatusId);
            setTimeout(() => {
                $("#ddlStatus").val(myJSON.StatusId);
            }, 2000);
        }
    });
}
function DownloadQrCode(objthis) {

    var UHouseId = $(objthis).attr('data-QUID');
    window.location = "/Asset/DownLoadQR?UId=" + UHouseId;
}

function GetMultipleQrCode() {
    ShowLoading($('#example'));
    var menuary = [];
    var fileName = "QrCodes.zip";
    $(".vehicletable tbody tr").each(function () {

        var isassigned = $(this).find('input[type=checkbox]').is(':checked');
        if (isassigned) {
            var UId = $(this).attr("UId");
            var ContainerCode = $(this).find("td").eq(6).text();
            var ContainerName = $(this).find("td").eq(7).text();
            var ContainerType = $(this).find("td").eq(9).text();

            var sData = {
                UId: UId,
                ContainerCode: ContainerCode,
                ContainerName: ContainerName,
                ContainerType: ContainerType,
            };
            menuary.push(sData);
        }
    });

    if (menuary.length > 0) {

        $.ajax({
            type: "POST",
            url: "/Asset/GetMultipleContainerQrCode",
            data: { JArrayval: JSON.stringify(menuary) },
            cache: false,
            xhr: function () {
                var xhr = new XMLHttpRequest();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 2) {
                        if (xhr.status == 200) {
                            xhr.responseType = "blob";
                        } else {
                            xhr.responseType = "text";
                        }
                    }
                };
                return xhr;
            },
            success: function (data) {
                HideLoading($('#example'));
                //Convert the Byte Data to BLOB object.
                var blob = new Blob([data], { type: "application/octetstream" });

                //Check the Browser type and download the File.
                var isIE = false || !!document.documentMode;
                if (isIE) {
                    window.navigator.msSaveBlob(blob, fileName);
                } else {
                    var url = window.URL || window.webkitURL;
                    link = url.createObjectURL(blob);
                    var a = $("<a />");
                    a.attr("download", fileName);
                    a.attr("href", link);
                    $("body").append(a);
                    a[0].click();
                    $("body").remove(a);
                }
            },
            error: function (result) {
                HideLoading($('#example'));
            }
        });

    }
    else {
        HideLoading($('#example'));
        ShowCustomMessage('0', 'Please Select QRCode To Download', '');
    }
}

function printImg(myImage) {
    // var myImage = "data:image/gif;base64,R0lGODlhEAAQAMQAAORHHOVSKudfOulrSOp3WOyDZu6QdvCchPGolfO0o/XBs/fNwfjZ0frl3/zy7////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAkAABAALAAAAAAQABAAAAVVICSOZGlCQAosJ6mu7fiyZeKqNKToQGDsM8hBADgUXoGAiqhSvp5QAnQKGIgUhwFUYLCVDFCrKUE1lBavAViFIDlTImbKC5Gm2hB0SlBCBMQiB0UjIQA7";
    //$("#mainImg").attr('src', myImage);
    //Pagelink = "about:blank";
    //var pwin = window.open(Pagelink, "_new");
    ////pwin.onload = function () { window.print(); }
    //// pwin.document.open();
    //pwin.document.write(ImagetoPrint(document.getElementById("mainImg").src));
    //pwin.document.close();
    let pdfWindow = window.open("")
    pdfWindow.document.write(
        "<iframe width='100%' height='100%' src='data:application/pdf;base64, " +
        encodeURI(myImage) + "'></iframe>"
    )
}
function ImagetoPrint(source) {
    return "<html><head><script>function step1(){\n" +
        "setTimeout('step2()', 10);}\n" +
        "function step2(){window.print();window.close()}\n" +
        "</scri" + "pt></head><body onload='step1()'>\n" +
        "<img src='" + source + "' /></body></html>";
}

function GetQrCodeB64(objthis) {
    var sData = {
        UId: $(objthis).attr('data-QUID'),
        ContainerCode: $(objthis).attr('data-ContainerCode'),
        ContainerName: $(objthis).attr('data-ContainerName'),
        ContainerType: $(objthis).attr('data-ContainerType'),
    };

    $.ajax({
        type: "post",
        url: "/Asset/GetSingleContainerQrCode",
        data: { Jobjval: JSON.stringify(sData) },
        success: function (data) {

            printImg(data);
        }
    });
}