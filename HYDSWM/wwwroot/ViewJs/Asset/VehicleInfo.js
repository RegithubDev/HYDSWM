


$(document).ready(function () {
   
    AllMZoneLst('ddlSZone', 0, 'All Zone');
    //AllDVehicleTypeLst('ddlSVehicleType', 0, 'All Vehicle Type');
    AllAssetStatusLst('ddlSStatus', 0, 'All');
    GetDataTableData('Load');
  
    GetAllDesignation();
   

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


    CallAllFunc1();
});


function CallFunc1() {
    SetDataBind('Click');
    EchartsColumnsStackedLight.init();

}

function CallAllFunc1() {


    EchartsColumnsStackedLight.init();
    $('#example1').DataTable().clear().destroy();
    SetDataBind();


}
function CallSCircleByZone() {
    $('#ddlSCircle').val('0');
    AllMCircleLst('ddlSCircle', 0, 'All Circle', $('#ddlSZone').find(":selected").attr('value'));
}
function CallSWardByCircle() {
    AllMWardLst('ddlSWard', 0, 'All Ward', $('#ddlSCircle').find(":selected").attr('value'));
}
function boxDisable() {
    if ($('#ckbIsManual').is(':checked')) {
        $('#txtUId').removeAttr('disabled');
    } else {
        $('#txtUId').attr('disabled', 'disabled');
        $('#txtUId').val("");
    }
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
        AllMZoneLst('ddlZone', 0, 'Select');
        AllOwnerTypeLst('ddlOwnerType', 0, 'Select');
        AllVehicleTypeLst('ddlVehicleType', 1, 'Select');
        $("#ddlZone").select2({

            dropdownParent: $("#modal_form_AddDetail")

        });
        $("#ddlVehicleType").select2({

            dropdownParent: $("#modal_form_AddDetail")

        });
        $("#ddlOwnerType").select2({

            dropdownParent: $("#modal_form_AddDetail")

        });
        AllMInchargeLst('ddlIncharge', 0, 'Select');
        $("#ddlIncharge").select2({

            dropdownParent: $("#modal_form_AddDetail")

        });
        if (ddId > 0)
            AllAssetStatusLst('ddlStatus', 1, 'Select');
        else
            AllAssetStatusLst('ddlStatus', 0, 'Select');
    }, 1000);

    if (ddId > 0)
        SetDataOncontrols(ddId);
}


function DeleteVehicle(obj) {

    

    var UId = $(obj).attr('UId');

    var Uid_json = { 'UId': UId };

    $.ajax({
        type: 'POST',
        url: '/Asset/Delete_Vehicle?UId=' + JSON.stringify(Uid_json),
        contentType: "application/json",
        success: function (result) {
            console.log(result);
        }
    });

    //$('#user_content').load("/Asset/AddVehicle");



    window.location.href = window.location.href;
    



}














var dt;
function GetDataTableData(Type) {
    var TId = getUrlParameterInfo('TId');
    var VehicleTypeId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    var Status = '0';

    if (Type == 'Click') {
        
        var selectedCheckboxes = [];
        var checkboxes = document.querySelectorAll('#checkboxContainer input[type="checkbox"]');

        checkboxes.forEach(function (checkbox) {
            if (checkbox.checked) {
                selectedCheckboxes.push(checkbox.value);
            }
        });
        var concatenatedValues = selectedCheckboxes.join(', ');
        VehicleTypeId = concatenatedValues;
        ZoneId = $('#ddlSZone').val();
        CircleId = $('#ddlSCircle').val();
        WardId = $('#ddlSWard').val();
        Status = $('#ddlSStatus').val();
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
                    text: '<i class="icon-file-spreadsheet mr-2"></i> CSV',
                    extension: '.csv'
                },
                
                {
                    text: '<i class="fa fa-upload"></i> File Upload',
                    action: function () {
                        var fileSelector = $('<input type="file" id="upload_file" name="upload_file" onchange="upload_file(this);">');
                        fileSelector.click();
                        return true;
                    }
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
                d.ZoneId = ZoneId;
                d.CircleId = CircleId;
                d.WardId = WardId;
                d.Status = Status;
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
        'createdRow': function (row, data, dataIndex) {
            $(row).attr('UId', data.UId);
            $(row).attr('OperationTypeId', data.OperationTypeId);
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
            //{
            //    "render": function (data, type, row, meta) {
            //        return "<a  data-QUID='" + row.UId +

            //            "' href=javascript:void(0)  onclick=DownloadQrCode(this);  >" + row.UId + "</a>";

            //    }
            //},
            { data: "UId", sortable: true },
            {
                "render": function (data, type, row, meta) {
                    return "<a  data-QUID='" + row.UId +
                        "'data-VehicleNo='" + row.VehicleNo +
                        "'data-VehicleType='" + row.VehicleType +
                        "'data-OperationTypeId='" + row.OperationTypeId +
                        "'data-OwnerType='" + row.OwnerType +
                        "'data-ZoneNo='" + row.ZoneNo +
                        "'data-CircleName='" + row.CircleName +
                        "'data-WardNo='" + row.WardNo +
                        "'data-LandMark='" + row.LandMark +
                        "' href=javascript:void(0)  onclick=GetQrCodeB64(this);  >Print QrCode</a>";

                }
            },
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
            { data: "NetWt", sortable: true },

            //{
            //    sortable: false,
            //    "render": function (data, type, row, meta) {

            //        return '<a class="gticket" cticketid="' + row.VehicleId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
            //    }
            //},
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.VehicleId + '" href="' + row.ImgUrl + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.ImgUrl + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "DriverName", sortable: false },
            { data: "ContactNo", sortable: false },
            { data: "InchargeName", sortable: false },
            { data: "InchargeContactNo", sortable: false },
            { data: "LandMark", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    var link = 'http://maps.google.co.in/maps?q=' + row.DLCordinates;
                    var linkHTML = '<a href="' + link + '" target="_blank">' + row.DLCordinates + '</a>';
                    return linkHTML;
                }
            },
            { data: "DLRadius", sortable: false },
            { data: "DTStationName", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    var link = 'http://maps.google.co.in/maps?q=' + row.DTCordinates;
                    var linkHTML = '<a href="' + link + '" target="_blank">' + row.DTCordinates + '</a>';
                    return linkHTML;
                }
            },
            { data: "DTRadius", sortable: false },
            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    if (TId == "") {
                        return "<a cid='" + row.VehicleId + "' href='javascript:void(0);' title='edit' onclick='CallFunc(this);'><i class='ti-pencil'></i></a>";
                    }
                    else {

                        return '';
                    }
                }
            },
            {data: "CreatedOn", sortable: true},
            { data: "LastModifiedOn", sortable: true },

            {
                sortable: false,
                "render": function (data, type, row, meta) {
                    if (TId == "") {
                        return "<a UId='" + row.UId + "' href='javascript:void(0);' title='delete' onclick='DeleteVehicle(this);'>Delete</a>";
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


function AllDVehicleTypeLst(ControlId, IsRequired, Category) {

    $("#" + ControlId).html();

    $.ajax({
        type: "post",
        url: "/Asset/GetAllVehicleTypeByLogin",
        data: '{}',
        datatype: "json",
        traditional: true,
        success: function (data) {
            var Myjson = JSON.parse(data);
            $("#ddlVehicleType").select2({

            });
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
function GetAllDesignation() {
    //var selected_val = $('#ddlDepartment').find(":selected").attr('value');

    
    $.ajax({
        type: "post",
        url: "/Asset/GetAllVehicleTypeByLogin",
        data: '{}',
        datatype: "json",
        //traditional: true,
        success: function (data) {
            
            var myjson = JSON.parse(data);
            $("#checkboxContainer").html("");
           
            const checkboxContainer = document.getElementById('checkboxContainer');

            // Dynamically generate checkboxes based on the data
            myjson.forEach(language => {
                const li = document.createElement('li');
                const label = document.createElement('label');
                const checkbox = document.createElement('input');

                checkbox.type = 'checkbox';
                checkbox.value = language.VehicleTypeId;

                label.appendChild(checkbox);
                label.appendChild(document.createTextNode(language.VehicleType));
                li.appendChild(label);

                checkboxContainer.appendChild(li);


            });

            // Event listener to track changes and update selected values
            const checkboxes = document.querySelectorAll('#checkboxContainer input[type="checkbox"]');
            const selectedLanguages = [];

            checkboxes.forEach(checkbox => {
                checkbox.addEventListener('change', function () {
                    if (this.checked) {
                        selectedLanguages.push(this.value);
                    } else {
                        const index = selectedLanguages.indexOf(this.value);
                        if (index !== -1) {
                            selectedLanguages.splice(index, 1);
                        }
                    }
                    console.log("Selected languages: " + selectedLanguages);
                });
            });

        }

    });
}


function upload_file(obj1) {
    var input = $(obj1);
    var fileinput = input[0];
    var formData = new FormData();
    
    //Appending file to FormData object
    formData.append(fileinput.files[0].name, fileinput.files[0]);
    
    //Creating an XMLHttpRequest and sending
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Asset/Upload_file');
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {
            alert("File was Uploaded Successfully");
        }
    }




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
function SaveAndUpdateVehicleInfo() {

    var isvalid = 1;
    var input = {
        VehicleId: $("#hfVehicleId").val(),
        VehicleNo: $("#txtVehicleNo").val(),
        ChassisNo: $("#txtChassisNo").val(),
        VehicleTypeId: $("#ddlVehicleType").val(),
        OwnerTypeId: $("#ddlOwnerType").val(),
        ZoneId: $("#ddlZone").val(),
        CircleId: $("#ddlCircle").val(),
        WardId: $("#ddlWard").val(),
        GrossWt: $("#txtGrossWt").val(),
        TareWt: $("#txtTareWt").val(),
        NetWt: $("#txtNetWt").val(),
        UId: $("#txtUId").val(),
        DriverName: $("#txtDriverName").val(),
        ContactNo: $("#txtDriverNo").val(),
        IsManual: $('#ckbIsManual').is(':checked'),

        // IsActive: $('#ckbIsActive').is(':checked'),
        StatusId: $("#hfVehicleId").val() == 0 ? 2 : $("#ddlStatus").val(),
        ARemarks: $("#hfVehicleId").val() == 0 ? $("#txtRemarks").val() : $("#txtARemarks").val(),
        ExStatusVal: $("#hfStatus").val(),
        UserId: $("#hfUserId").val(),
        DInchargeId: $("#ddlIncharge").val(),
        DLocationId: $("#ddlLandmark").val(),
        DTsId: $("#ddlTs").val(),
    };
    if (input.VehicleNo == '' || input.ChassisNo == '' || input.VehicleTypeId == '')
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
            url: '/Asset/SaveAndUpdateVehicleInfo',
            data: formData,
            contentType: false,
            processData: false,
            success: function (data) {
                var myJson = JSON.parse(data);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Asset/AllVehicle');

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
        url: "/Asset/GetVehicleInfoById",
        data: { VehicleId: ddId },
        success: function (data) {
            //$('#txtVehicleNo').attr('disabled', 'disabled');
            $('#dvStatus').css("display", "block");
            $('#txtARemarks').attr('readonly', 'true');
            var myJSON = JSON.parse(data);
            $("#hfVehicleId").val(myJSON.VehicleId);
            $("#txtVehicleNo").val(myJSON.VehicleNo);
            $("#txtChassisNo").val(myJSON.ChassisNo);
            $("#txtDriverName").val(myJSON.DriverName);
            $("#txtDriverNo").val(myJSON.ContactNo);


            setTimeout(() => {
                $("#ddlVehicleType").val(myJSON.VehicleTypeId).change();
                $("#ddlOwnerType").val(myJSON.OwnerTypeId).change();
                $("#ddlZone").val(myJSON.ZoneId).change();

                setTimeout(() => {
                    $("#ddlCircle").val(myJSON.CircleId).change();

                    setTimeout(() => {
                        $("#ddlWard").val(myJSON.WardId).change();

                        setTimeout(() => {

                            $("#ddlLandmark").val(myJSON.DLocationId).change();
                            $("#ddlTs").val(myJSON.DTsId).change();
                        }, 2000);


                    }, 2000);

                }, 2000);
            }, 2000);



            $("#txtGrossWt").val(myJSON.GrossWt);
            $("#txtTareWt").val(myJSON.TareWt);
            $("#txtNetWt").val(myJSON.NetWt);
            if (myJSON.IsManualUId == 'True')
                $("#ckbIsManual").prop("checked", true);
            else
                $("#ckbIsManual").prop("checked", false);
            $("#txtUId").val(myJSON.UId);
            $('#txtUId').attr('disabled', 'disabled');
            $('#ckbIsManual').css("display", "none");

            setTimeout(() => {
                $("#ddlIncharge").val(myJSON.DInchargeId).change();
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
function CallFCircleByZone() {
    $('#ddlCircle').val('0');
    AllMCircleLst('ddlCircle', 0, 'Select', $('#ddlZone').find(":selected").attr('value'));
    $("#ddlCircle").select2({

        dropdownParent: $("#modal_form_AddDetail")

    });
}
function CallFWardByCircle() {
    AllMWardLst('ddlWard', 0, 'Select', $('#ddlCircle').find(":selected").attr('value'));
    $("#ddlWard").select2({

        dropdownParent: $("#modal_form_AddDetail")

    });
}

function CallOtherLocationByWard() {

    AllMDLocationLst('ddlLandmark', 0, 'Select', $('#ddlWard').find(":selected").attr('value'));
    $("#ddlLandmark").select2({

        dropdownParent: $("#modal_form_AddDetail")

    });
    AllMTSByWardLst('ddlTs', 0, 'Select', $('#ddlWard').find(":selected").attr('value'));
    $("#ddlTs").select2({

        dropdownParent: $("#modal_form_AddDetail")

    });
}

function GetMultipleQrCode() {
    ShowLoading($('#example'));
    var menuary = [];
    var fileName = "QrCodes.zip";
    $(".vehicletable tbody tr").each(function () {

        var isassigned = $(this).find('input[type=checkbox]').is(':checked');
        if (isassigned) {
            var UId = $(this).attr("UId");
            var VehicleNo = $(this).find("td").eq(6).text();
            var VehicleType = $(this).find("td").eq(8).text();
            var OperationTypeId = $(this).attr("OperationTypeId");
            var OwnerType = $(this).find("td").eq(9).text();
            var Ward = $(this).find("td").eq(13).text();
            var Circle = $(this).find("td").eq(12).text();
            var Zone = $(this).find("td").eq(11).text();
            var LandMark = $(this).find("td").eq(22).text();

            var sData = {
                UId: UId,
                VehicleNo: VehicleNo,
                VehicleType: VehicleType,
                OperationTypeId: OperationTypeId,
                OwnerType: OwnerType,
                Ward: Ward,
                Circle: Circle,
                Zone: Zone,
                LandMark: LandMark
            };
            menuary.push(sData);
        }
    });

    if (menuary.length > 0) {

        $.ajax({
            type: "POST",
            url: "/Asset/GetMultipleQrCode",
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
    var fileName = "abc.pdf";
    // var myImage = "data:image/gif;base64,R0lGODlhEAAQAMQAAORHHOVSKudfOulrSOp3WOyDZu6QdvCchPGolfO0o/XBs/fNwfjZ0frl3/zy7////wAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAACH5BAkAABAALAAAAAAQABAAAAVVICSOZGlCQAosJ6mu7fiyZeKqNKToQGDsM8hBADgUXoGAiqhSvp5QAnQKGIgUhwFUYLCVDFCrKUE1lBavAViFIDlTImbKC5Gm2hB0SlBCBMQiB0UjIQA7";
    //var b64 = "JVBERi0xLjQKJeLjz9MKMiAwIG9iago8PC9UeXBlL1hPYmplY3QvU3VidHlwZS9JbWFnZS9XaWR0aCA2NjAvSGVpZ2h0IDY2MC9MZW5ndGggMTAxNDkvQ29sb3JTcGFjZS9EZXZpY2VSR0IvQml0c1BlckNvbXBvbmVudCA4L0ZpbHRlci9GbGF0ZURlY29kZT4+c3RyZWFtCnic7dVBrsNIDgTRuf+lexa9+WhAkGCWGEwx3gVIZqXsf/6RJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJEmSJOn7/qfxqPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5F90v3qPfd1qvEnCmnMtd76I7oXXS/dI963229SsyZcipzvYfuiN5lN3qY83OVrBLRef9i270Uc9YVu9HDnJ+rZJWIzvsX2+6lmLOu2I0e5vxcJatEdN6/2HYvxZx1xW70MOfnKlklovP+xbZ7KeasK3ajhzk/V8kqEZ33L7bdSzFn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nZnuJXaD2lnzUb2qzHXn+TtXVO6l0JnpXmI3qJ2lN5z6ljvRmeke9b7b5lI7U/dSc6VpTn2PnejMdI96321zqZ2pe6m50jSnvsdOdGa6R73vtrnUztS91FxpmlPfYyc6M92j3nfbXGpn6l5qrjTNqe+xE52Z7lHvu20utTN1LzVXmubU99iJzkz3qPfdNpfambqXmitNc+p77ERnpnvU+26bS+1M3UvNlaY59T12ojPTPep9t82ldqbupeZK05z6HjvRmeke9b7b5lI7U/dSc6VpTn2PnejMdI96321zqZ2pe6m50jSnvsdOdGa6R73vtrnUztS91FxpmlPfYyc6M92j3nfbXGpn6l5qrjTNqe+xE52Z7lHvu20utTN1LzVXmubU99iJzkz3qPfdNpfambqXmitNc+p77ERnpnvU+26bS+1M3UvNlaY59T12ojPTPep9t82ldqbupeZK05z6HjvRmeke9b7b5lI7U/dSc6VpTn2PnejMdI96321zqZ2pe6m50jSnvsdOdGa6R73vtrnUztS91FxpmlPfYyc6M92j3nfbXGpn6l5qrnTl1HfR2edtc9XD932OymrbXOlKpZNUn7fNVQ/f9zkqq21zpSuVTlJ93jZXPXzf56ists2VrlQ6SfV521z18H2fo7LaNle6Uukk1edtc9XD932OymrbXOlKpZNUn7fNVQ/f9zkqq21zpSuVTlJ93jZXPXzf56ists2VrlQ6SfV521z18H2fo7LaNle6Uukk1edtc9XD932OymrbXOlKpZNUn7fNVQ/f9zkqq21zpSuVTlJ93jZXPXzf56ists2VrlQ6SfV521z18H2fo7LaNle6Uukk1edtc9XD932OymrbXOlKpZNUn7fNVQ/f9zkqq21zpSuVTlJ93jZXPXzf56ists2VrlQ6SfV521z18H2fo7LaNle6Uukk1edtc9XD932OymrbXOlKpZNUn7fNVQ/f9zkqq21zpSuVTlJ93jZXPXzf56ists2VrlQ6SfV521z18H2fo7LaNlffZp/n72xW3567TSXnbcxZV6huJHbSrObvnDh3m0rO25izrlDdSOykWc3fOXHuNpWctzFnXaG6kdhJs5q/c+LcbSo5b2POukJ1I7GTZjV/58S521Ry3sacdYXqRmInzWr+zolzt6nkvI056wrVjcROmtX8nRPnblPJeRtz1hWqG4mdNKv5OyfO3aaS8zbmrCtUNxI7aVbzd06cu00l523MWVeobiR20qzm75w4d5tKztuYs65Q3UjspFnN3zlx7jaVnLcxZ12hupHYSbOav3Pi3G0qOW9jzrpCdSOxk2Y1f+fEudtUct7GnHWF6kZiJ81q/s6Jc7ep5LyNOesK1Y3ETprV/J0T525TyXkbc9YVqhuJnTSr+Tsnzt2mkvM25qwrVDcSO2lW83dOnLtNJedtzFlXqG4kdtKs5u+cOHebSs7bmLOuUN1I7KRZzd85ce42lZy3MWddobqR2Emzmr9z4txtKjlvY86ahupk4tzEnROzopjVfIlvVNlZukJ1MnFu4s6JWVHMar7EN6rsLF2hOpk4N3HnxKwoZjVf4htVdpauUJ1MnJu4c2JWFLOaL/GNKjtLV6hOJs5N3DkxK4pZzZf4RpWdpStUJxPnJu6cmBXFrOZLfKPKztIVqpOJcxN3TsyKYlbzJb5RZWfpCtXJxLmJOydmRTGr+RLfqLKzdIXqZOLcxJ0Ts6KY1XyJb1TZWbpCdTJxbuLOiVlRzGq+xDeq7CxdoTqZODdx58SsKGY1X+IbVXaWrlCdTJybuHNiVhSzmi/xjSo7S1eoTibOTdw5MSuKWc2X+EaVnaUrVCcT5ybunJgVxazmS3yjys7SFaqTiXMTd07MimJW8yW+UWVn6QrVycS5iTsnZkUxq/kS36iys3SF6mTi3MSdE7OimNV8iW9U2Vm6QnUycW7izolZUcxqvsQ3quwsXaE6mTg3cefErChmNV/iG1V2lq5QnUycm7hzYlYUs5ov8Y0qO0tiUb8bFLN67lTHOpmVpA2o3zqKWT13qmOdzErSBtRvHcWsnjvVsU5mJWkD6reOYlbPnepYJ7OStAH1W0cxq+dOdayTWUnagPqto5jVc6c61smsJG1A/dZRzOq5Ux3rZFaSNqB+6yhm9dypjnUyK0kbUL91FLN67lTHOpmVpA2o3zqKWT13qmOdzErSBtRvHcWsnjvVsU5mJWkD6reOYlbPnepYJ7OStAH1W0cxq+dOdayTWUnagPqto5jVc6c61smsJG1A/dZRzOq5Ux3rZFaSNqB+6yhm9dypjnUyK0kbUL91FLN67lTHOpmVpA2o3zqKWT13qmOdzErSBtRvHcWsnjvVsU5mJWkD6reOYlbPnepYJ7OS/kV9C3qO7sgvqHvN+ds5J86lVO7VfHS/dI/uyC+oe8352zknzqVU7tV8dL90j+7IL6h7zfnbOSfOpVTu1Xx0v3SP7sgvqHvN+ds5J86lVO7VfHS/dI/uyC+oe8352zknzqVU7tV8dL90j+7IL6h7zfnbOSfOpVTu1Xx0v3SP7sgvqHvN+ds5J86lVO7VfHS/dI/uyC+oe8352zknzqVU7tV8dL90j+7IL6h7zfnbOSfOpVTu1Xx0v3SP7sgvqHvN+ds5J86lVO7VfHS/dI/uyC+oe8352zknzqVU7tV8dL90j+7IL6h7zfnbOSfOpVTu1Xx0v3SP7sgvqHvN+ds5J86lVO7VfHS/dI/uyC+oe8352zknzqVU7tV8dL90j+7IL6h7zfnbOSfOpVTu1Xx0v3SP7sgvqHvN+ds5J86lVO7VfHS/dI/uyC+oe8352zknzqVU7tV8dL90j+7IL6h7zfnbOSfOpVTu1Xx0v3SP7sgvqHvN+ds5J86lVO7VfHS/dI/uyC+oe8352zknzqVU7tV8dqNHYs7UzmY1fy6lcm9iVok7q4fd6JGYM7WzWc2fS6ncm5hV4s7qYTd6JOZM7WxW8+dSKvcmZpW4s3rYjR6JOVM7m9X8uZTKvYlZJe6sHnajR2LO1M5mNX8upXJvYlaJO6uH3eiRmDO1s1nNn0up3JuYVeLO6mE3eiTmTO1sVvPnUir3JmaVuLN62I0eiTlTO5vV/LmUyr2JWSXurB52o0diztTOZjV/LqVyb2JWiTurh93okZgztbNZzZ9LqdybmFXizuphN3ok5kztbFbz51Iq9yZmlbizetiNHok5Uzub1fy5lMq9iVkl7qwedqNHYs7UzmY1fy6lcm9iVok7q4fd6JGYM7WzWc2fS6ncm5hV4s7qYTd6JOZM7WxW8+dSKvcmZpW4s3rYjR6JOVM7m9X8uZTKvYlZJe6sHnajR2LO1M5mNX8upXJvYlaJO6uH3eiRmDO1s1nNn0up3JuYVeLO6mE3eiTmTO1sVvPnUir3JmaVuLN62I0eiTlTO5vV/LmUyr2JWSXurB5UNypzKYk5U8y5R+VefRvdTb2L6sapfnZKzJlizj0q9+rb6G7qXVQ3TvWzU2LOFHPuUblX30Z3U++iunGqn50Sc6aYc4/Kvfo2upt6F9WNU/3slJgzxZx7VO7Vt9Hd1LuobpzqZ6fEnCnm3KNyr76N7qbe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IFIvUGFyZW50IDQgMCBSPj4KZW5kb2JqCjEgMCBvYmoKPDwvVHlwZS9Gb250L1N1YnR5cGUvVHlwZTEvQmFzZUZvbnQvSGVsdmV0aWNhL0VuY29kaW5nL1dpbkFuc2lFbmNvZGluZz4+CmVuZG9iago0IDAgb2JqCjw8L1R5cGUvUGFnZXMvQ291bnQgMS9LaWRzWzUgMCBSXT4+CmVuZG9iago2IDAgb2JqCjw8L1R5cGUvQ2F0YWxvZy9QYWdlcyA0IDAgUj4+CmVuZG9iago3IDAgb2JqCjw8L1Byb2R1Y2VyKGlUZXh0U2hhcnCSIDUuNS4xMy4yIKkyMDAwLTIwMjAgaVRleHQgR3JvdXAgTlYgXChBR1BMLXZlcnNpb25cKSkvQ3JlYXRpb25EYXRlKEQ6MjAyMjAyMTcxNzMzNDYrMDUnMzAnKS9Nb2REYXRlKEQ6MjAyMjAyMTcxNzMzNDYrMDUnMzAnKT4+CmVuZG9iagp4cmVmCjAgOAowMDAwMDAwMDAwIDY1NTM1IGYgCjAwMDAwMTA4MjUgMDAwMDAgbiAKMDAwMDAwMDAxNSAwMDAwMCBuIAowMDAwMDEwMzIxIDAwMDAwIG4gCjAwMDAwMTA5MTMgMDAwMDAgbiAKMDAwMDAxMDY5MCAwMDAwMCBuIAowMDAwMDEwOTY0IDAwMDAwIG4gCjAwMDAwMTEwMDkgMDAwMDAgbiAKdHJhaWxlcgo8PC9TaXplIDgvUm9vdCA2IDAgUi9JbmZvIDcgMCBSL0lEIFs8ZWIzOTE4ZmJhOTU3N2QwYWY2YjBkM2NjNjJmYTRlN2M+PGViMzkxOGZiYTk1NzdkMGFmNmIwZDNjYzYyZmE0ZTdjPl0+PgolaVRleHQtNS41LjEzLjIKc3RhcnR4cmVmCjExMTc0CiUlRU9GCg==";
    let pdfWindow = window.open("")
    pdfWindow.document.write(
        "<iframe width='100%' height='100%' src='data:application/pdf;base64, " +
        encodeURI(myImage) + "'></iframe>"
    )
    //let pdfWindow = window.open("");
    //pdfWindow.document.write("<html<head><title>" + fileName + "</title><style>body{margin: 0px;}iframe{border-width: 0px;}</style></head>");
    //pdfWindow.document.write("<body><embed width='100%' height='100%' src='data:application/pdf;base64, " + encodeURI(myImage) + "#toolbar=0&navpanes=0&scrollbar=0'></embed></body></html>");
    // $("#myFrame").attr('src', b64);
    // Pagelink = "about:blank";
    // var pwin = window.open(Pagelink, "_new");
    // //pwin.onload = function () { window.print(); }
    //// pwin.document.open();
    // pwin.document.write(ImagetoPrint(document.getElementById("mainImg").src));
    // pwin.document.close();
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
        VehicleNo: $(objthis).attr('data-VehicleNo'),
        VehicleType: $(objthis).attr('data-VehicleType'),
        OperationTypeId: $(objthis).attr('data-OperationTypeId'),
        OwnerType: $(objthis).attr('data-OwnerType'),
        Ward: $(objthis).attr('data-WardNo'),
        Circle: $(objthis).attr('data-CircleName'),
        Zone: $(objthis).attr('data-ZoneNo'),
        LandMark: $(objthis).attr('data-LandMark')
    };

    $.ajax({
        type: "post",
        url: "/Asset/GetSingleQrCode",
        data: { Jobjval: JSON.stringify(sData) },
        success: function (data) {

            printImg(data);
        }
    });
}



function bindtable(data) {



    $('#example1 tbody').empty();

    if ($('#hfTotalrows1').val() > 0)
        $('#example1').DataTable().clear().destroy();


    var rowcount = data.length;

    $.each(data, function (i, item) {
        var count = i + 1;

        var rows = "<tr>" + "<td>" + count +
            "</td>" + "<td>" + item.VehicleType +
            "</td>" + "<td>" + item.TotalActive +
            "</td>" + "<td>" + item.TotalInActive +
            "</td>" + "<td>" + item.TotalRepair +
            "</td>" + "<td>" + item.TotalCondemed +
            "</td>" + "<td>" + item.Total +


            "</td> </tr>";
        $('#example1 tbody').append(rows);

    });

    var tabid = $('#example1');
    $('#hfTotalrows1').val(rowcount);
    if ($('#hfTotalrows1').val() > 0)
        setdatatable();


}

function SetDataBind(Type) {



    var VehicleTypeId = '0';
    var ZoneId = '0';
    var CircleId = '0';
    var WardId = '0';
    //VehicleTypeIdN = '0';


    if (Type == 'Click') {
        var selectedCheckboxes = [];
        var checkboxes = document.querySelectorAll('#checkboxContainer input[type="checkbox"]');

        checkboxes.forEach(function (checkbox) {
            if (checkbox.checked) {
                selectedCheckboxes.push(checkbox.value);
            }
        });
        var concatenatedValues = selectedCheckboxes.join(', ');
        
        //VehicleTypeId = $('#ddlSVehicleType').val();
        VehicleTypeId = concatenatedValues;
        ZoneId = $('#ddlSZone').val();
        CircleId = $('#ddlSCircle').val();
        WardId = $('#ddlSWard').val();

    }

    var requestModel = {
        ZoneId: ZoneId,
        CircleId: CircleId,
        WardId: WardId,
        VehicleTypeId: VehicleTypeId,
      
    };

    $.ajax({
        type: "POST",
        url: '/Asset/GetVehicleDepoymentReport',
        dataType: "json",
        data: requestModel,
        success: function (result) {
            var myJSON = JSON.parse(result);
            var Result1 = myJSON;

            bindtable(Result1);


        }
    });
}



function setdatatable() {
    $('#example1').DataTable({
        destroy: true,
        "responsive": true,
        "order": [[0, "asc"]],
        lengthMenu: [
            [10, 25, 50, 500, 1000, 5000],
            ['10 rows', '25 rows', '50 rows', '500 rows', '1000 rows', '5000 rows']
        ],
        language: {
            infoEmpty: "No records available",
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
                    text: '<i class="icon-file-spreadsheet mr-2"></i> CSV',
                    fieldSeparator: '\t',
                    extension: '.csv'
                },
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i>',
                    className: 'btn bg-blue btn-icon dropdown-toggle'
                },

            ]
        }

    });


}

var EchartsColumnsStackedLight = function () {


    //
    // Setup module components
    //

    // Stacked column chart
    var _columnsStackedLightExample = function () {
        if (typeof echarts == 'undefined') {
            console.warn('Warning - echarts.min.js is not loaded.');
            return;
        }

        // Define element
        var columns_stacked_element = document.getElementById('columns_clustered');


        //
        // Charts configuration
        //

        if (columns_stacked_element) {

            // Initialize chart
            var columns_stacked = echarts.init(columns_stacked_element);
            ShowLoading($('#columns_clustered'));


            //
            // Chart config
            //
            var selectedCheckboxes = [];
            var checkboxes = document.querySelectorAll('#checkboxContainer input[type="checkbox"]');

            checkboxes.forEach(function (checkbox) {
                if (checkbox.checked) {
                    selectedCheckboxes.push(checkbox.value);
                }
            });
            var concatenatedValues = selectedCheckboxes.join(', ');
           // VehicleTypeId = concatenatedValues;

            $.ajax({


                type: "POST",
                // contentType: "application/json; charset=utf-8",
                url: '/Asset/GetVehicleDepoymentReport',
                data: {

                    //VehicleTypeId: $('#ddlSVehicleType').val(),
                    VehicleTypeId: concatenatedValues,
                    ZoneId: $('#ddlSZone').val(),
                    CircleId: $('#ddlSCircle').val(),
                    WardId: $('#ddlSWard').val(),



                },

                success: function (data) {
                    var myJSON = JSON.parse(data);

                    var Result1 = myJSON;

                    var collectedarr = [];
                    var totalhousedarr = [];
                    var circlearr = [];
                    var circlearr1 = [];
                    var circlearr2 = [];



                    for (i = 0; i < Result1.length; i++) {
                        collectedarr.push(Result1[i].TotalActive);
                        totalhousedarr.push(Result1[i].TotalInActive);
                        circlearr.push(Result1[i].VehicleType);
                        circlearr1.push(Result1[i].TotalRepair);
                        circlearr2.push(Result1[i].TotalCondemed);


                    }



                    // Options
                    columns_stacked.setOption({

                        // Define colors
                        color: ['#2ec7c9', '#b6a2de', '#5ab1ef', '#ffb980', '#d87a80'],

                        // Global text styles
                        textStyle: {
                            fontFamily: 'var(--body-font-family)',
                            color: 'var(--body-color)',
                            fontSize: 14,
                            lineHeight: 22,
                            textBorderColor: 'transparent'
                        },

                        // Chart animation duration
                        animationDuration: 750,

                        // Setup grid
                        grid: {
                            left: 10,
                            right: 10,
                            top: 35,
                            bottom: 0,
                            containLabel: true
                        },

                        // Add legend
                        legend: {
                            data: ['Active', 'InActive', 'Repair', 'Condemned'],   
                            itemHeight: 5,
                            itemGap: 3,
                            textStyle: {
                                padding: [0, 2],
                                color: '#000000'
                            },
                            orient: 'horizontal', // set orientation to horizontal
                            top: 'top', // adjust top position
                            width: '95%', // set a width to fit all items in a single row
                            // adjust left position
                        },

                        // Add tooltip
                        tooltip: {
                            trigger: 'axis',
                            backgroundColor: 'rgba(0,0,0,0.9)',
                            padding: [10, 15],
                            textStyle: {
                                fontSize: 13,
                                fontFamily: 'Roboto, sans-serif',
                                color: '#fff'
                            }
                        },

                        // Horizontal axis
                        xAxis: [{
                            type: 'category',
                            data: circlearr,//['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
                            axisLabel: {
                                color: '#000000',
                                rotate: 70,
                            },
                            axisLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.25)'
                                }
                            },
                            splitLine: {
                                show: true,
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.1)',
                                    type: 'dashed'
                                }
                            }
                        }],

                        // Vertical axis
                        yAxis: [{
                            type: 'value',
                            axisLabel: {
                                color: '#000000'
                            },
                            axisLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.25)'
                                }
                            },
                            splitLine: {
                                lineStyle: {
                                    color: 'rgba(0,0,0,0.1)'
                                }
                            },
                            splitArea: {
                                show: true,
                                areaStyle: {
                                    color: ['rgba(255,255,255,0.01)', 'rgba(0,0,0,0.01)']
                                }
                            }
                        }],

                        // Add series
                        series: [





                            {
                                name: 'Active',
                                type: 'bar',
                                data: collectedarr,
                                stack: 'Advertising',

                            },
                            {
                                name: 'InActive',

                                data: totalhousedarr,
                                type: 'bar',
                                stack: 'Advertising',



                            },
                            {
                                name: 'Repair',
                                type: 'bar',
                                data: circlearr1,
                                stack: 'Advertising',

                            },
                            {
                                name: 'Condemned',
                                type: 'bar',
                                data: circlearr2,
                                stack: 'Advertising',

                            },

                        ]
                    });
                    HideLoading($('#columns_clustered'));
                },





            });






        }


        //
        // Resize charts
        //

        // Resize function
        var triggerChartResize = function () {
            columns_stacked_element && columns_stacked.resize();
        };

        // On sidebar width change
        var sidebarToggle = document.querySelectorAll('.sidebar-control');
        if (sidebarToggle) {
            sidebarToggle.forEach(function (togglers) {
                togglers.addEventListener('click', triggerChartResize);
            });
        }

        // On window resize
        var resizeCharts;
        window.addEventListener('resize', function () {
            clearTimeout(resizeCharts);
            resizeCharts = setTimeout(function () {
                triggerChartResize();
            }, 200);
        });
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function () {
            _columnsStackedLightExample();
        }
    }
}();


function DownloadFile(FType) {



    var ZoneId = $('#ddlSZone').val();
    var CircleId = $('#ddlSCircle').val();
    var WardId = $('#ddlSWard').val();

    var Status = $('#ddlSStatus').val();

   /* var VehicleTypeId = $('#ddlSVehicleType').val();*/

    var selectedCheckboxes = [];
    var checkboxes = document.querySelectorAll('#checkboxContainer input[type="checkbox"]');

    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            selectedCheckboxes.push(checkbox.value);
        }
    });
    var concatenatedValues = selectedCheckboxes.join(', ');
    var TName = "Vehicle Master Details";

    ShowLoading($('#example'));

    $.ajax({
        type: "POST",
        url: "/Asset/ExportAllVehicleDetails",
        data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleTypeId: concatenatedValues, FName: TName, Status: Status },
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
function CSVData(FType) {
    
    // Assuming you have jQuery loaded
    var ZoneId = $('#ddlSZone').val();
    var CircleId = $('#ddlSCircle').val();
    var WardId = $('#ddlSWard').val();

    var Status = $('#ddlSStatus').val();

    //var VehicleTypeId = $('#ddlSVehicleType').val();

    var selectedCheckboxes = [];
    var checkboxes = document.querySelectorAll('#checkboxContainer input[type="checkbox"]');

    checkboxes.forEach(function (checkbox) {
        if (checkbox.checked) {
            selectedCheckboxes.push(checkbox.value);
        }
    });
    var concatenatedValues = selectedCheckboxes.join(', ');

    var TName = "Vehicle Master Details";

    $.ajax({
        url: '/Asset/ExportAllVehicleDetailsCSV', // URL of your controller action
        method: 'POST',
        data: { ZoneId: ZoneId, CircleId: CircleId, WardId: WardId, VehicleTypeId: concatenatedValues, FName: TName, Status: Status },
        success: function (csvData) {

            // For downloading the CSV as a file
            var blob = new Blob([csvData], { type: 'text/csv' });
            var url = window.URL.createObjectURL(blob);
            var a = document.createElement('a');
            a.href = url;
            a.download = TName + '.csv';
            document.body.appendChild(a);
            a.click();
            window.URL.revokeObjectURL(url);
        },
        error: function (error) {
            console.error('Error:', error);
        }
    });


}