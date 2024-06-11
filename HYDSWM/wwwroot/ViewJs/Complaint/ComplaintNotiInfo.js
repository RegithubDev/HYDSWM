

var today;

$(document).ready(function () {

    today = new Date();
    

    let dd = today.getDate();
    let mm = today.getMonth() + 1;

    let yyyy = today.getFullYear();

    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    today = mm + '/' + dd + '/' + yyyy;

    var TName = getUrlParameterInfo('TName');
    var status = "";


    if (TName == "All Complaints") {

        var el = document.getElementById("Status");
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].text == "All") {
                el.selectedIndex = i;
                status = "All";
                break;
            }

        }
    }




    else if (TName == "All Pending Complaints") {

        var el = document.getElementById("Status");
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].text == "Open") {
                el.selectedIndex = i;
                status = "Open";
                break;
            }

        }
    }
    else if (TName == "All Closed Complaints") {
        var el = document.getElementById("Status");
        for (var i = 0; i < el.options.length; i++) {
            if (el.options[i].text == "Closed") {
                el.selectedIndex = i;
                status = "Closed";
                break;
            }

        }


        //$('#Status').prop('disabled', true);
    }



    GetDataTableData('Load', "05/01/2001", today, status);
});
var dt;
var comp_id;
var totalrows;

var f1 = 0;
var f2 = 0;
function GetDataTableData(Type, from_date, to_date, sort_status) {
    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');

    

    $("#spnHeader").html('');
    $("#spnHeader").html(TName);
    var IsClick = '0';

    if (from_date != "" && to_date != "")
        IsClick = '1';
    else
        IsClick = '0';

    var NotiId = TId;


    

    dt = $('#example').DataTable({
        processing: true,
        destroy: true,
        responsive: true,
        serverSide: true,
        searchable: true,
        lengthMenu: [[10, 20, 50, 100, 500, 10000, -1], [10, 20, 50, 100, 500, 10000, "All"]],
        //paging: true,
        //pageLength: 1000,
        /*layout: {
            topStart: {
                buttons: ['copy', 'csv', 'excel', 'pdf', 'print']
            }
        },*/
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
                    extension: '.csv',
                    filename: 'All Complaint - MSW Waste Management',
                    action: function () {
                        var fileSelector = $('<input type="button" id="download_excel" name="download_excel" onclick="Download_excel();">');
                        fileSelector.click();
                        return true;
                    }
                },
               
                
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i>',
                    className: 'btn bg-blue btn-icon dropdown-toggle'
                }/*,
                {
                    extend: 'excel',
                    text: '<i class="icon-file-spreadsheet mr-2"></i> Excel'
                    exportOptions: {
                    columns: ':visible',
                    orthogonal: 'excel',
                    modifier: {
                        order: 'current',
                        page: 'all',
                        selected: false,
                    }
                    
                    }
                }*/
            ]
        },
        initComplete: function () {
            $(this.api().table().container()).find('input').parent().wrap('<form>').parent().attr('autocomplete', 'on');
        },
        ajax: {
            url: "/Complaint/GetAllStaffComplaint_Paging/",
            type: 'POST',
            data: function (d) {
                d.ContratorId = IsClick;
                d.NotiId = NotiId;

                if (from_date != "" && to_date != "") {
                    d.FromDate = from_date;

                    d.ToDate = to_date;
                }
                else if (from_date == "" && to_date == "")
                {
                    d.FromDate = "05/01/2001";
                    d.ToDate = today;
                }
                else if (from_date != "" && to_date == "") {
                    d.FromDate = from_date;
                    d.ToDate = today;
                }
                else if (from_date == "" && to_date != "") {
                    d.FromDate = "05/01/2001";
                    d.ToDate = to_date;
                }

                if (sort_status == "All") {
                    sort_status = "";
                }



                
                    d.sort_status = sort_status;
                
                
                
                return {

                    requestModel: d
                };
            },
            dataType: "json",
            dataSrc: function (json) {

                json.draw = json.draw;
                json.recordsTotal = json.recordsTotal;
                
                totalrows = json.recordsTotal;

                

                json.recordsFiltered = json.recordsFiltered;
                json.data = json.data;
               /* if (flag == 0) { 
                    total_records(json.recordsTotal);
                    flag = 1;
                }
                */
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
            
            /*{
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a cticketid="' + row.VehicleId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource1" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },*/



















            



            { data: "Complaintcode", sortable: true },

            { data: "RegistrationType", sortable: true },

            { data: "RegistrationDate", sortable: true },

            { data: "ComplaintType", sortable: true },

            { data: "ZoneName", sortable: true },

            { data: "CircleName", sortable: true },


            { data: "WardName", sortable: true },

            { data: "Location", sortable: true },

            { data: "Status", sortable: true },

            { data: "ComplaintName", sortable: true },

            { data: "ComplaintContactNumber", sortable: true },

            { data: "citizen_email", sortable: true },


            /*{ data: "Complaint Type", sortable: true },*/


            { data: "Location", sortable: true },

            { data: "ClosedOn", sortable: true },

            { data: "ActionRemarks", sortable: true },
            /*
            { data: "CLOSE TIME LOCATION", sortable: true },
            { data: "CLOSING IMAGE", sortable: true },
            { data: "ACTION REMARKS", sortable: true },

            { data: "REVISED WARD NO", sortable: true },
            { data: "REVISED STREET LOCATION", sortable: true },
            */
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    
                    
                        return "<a cid='" + row.Complaint_num + "' complaintId='"+ row.SComplaintId+"' href='javascript:void(0);' title='edit' onclick='CallFunc(this,2);'><i class='ti-pencil'></i></a>";
                    
                }
            }

        ]
    });

  //  $('#example').on('xhr.dt', function (e, settings, json, xhr) {
    //    dt.page.len(json.data.length).draw()
   // })


    //dt.fnSettings().fnRecordsTotal();fnGetData().length

    //var somestring = dt.column(0).data().length;

    //var count = 0;
    //$('#example tr').each(function () {
    //    count++;
    // });

   // var l = dt.rows().eq(0).length;

    
    
    
}

function total_records(total) {
    dt.page.len(total).draw();
}

//dt.page.len(totalrows).draw();

//var rowCount = dt.settings()[0].json.recordsTotal;


function Download_excel() {
    var From_date = document.getElementById("FromDate").value;

    var To_date = document.getElementById("ToDate").value;

    var status_selected = $("#Status option:selected").text();

    var formData = new FormData();

    


    if (From_date != "" && To_date != "") {
        From_date = From_date;

        To_date = To_date;
    }
    else if (From_date == "" && To_date == "") {
        From_date = "05/01/2001";
        To_date = today;
    }
    else if (From_date != "" && To_date == "") {
        From_date = From_date;
        To_date = today;
    }
    else if (From_date == "" && To_date != "") {
        From_date = "05/01/2001";
        To_date = To_date;
    }

    if (status_selected == "All") {
        status_selected = "";
    }


    formData.append("FromDate", From_date);
    formData.append("ToDate", To_date);
    formData.append("sort_status", status_selected)

    //Creating an XMLHttpRequest and sending
    var xhr = new XMLHttpRequest();
    xhr.open('POST', '/Complaint/GetAllStaffComplaints');
    xhr.send(formData);
    xhr.onreadystatechange = function () {
        if (xhr.readyState == 4 && xhr.status == 200) {

            $('#downloadfile')[0].click();

            //$('a[href="#downloadfile"]').click(function () {
               // alert('Sign new href executed.');
            //}); 
            //alert("File was Uploaded Successfully");
        }
    }


}


function get_inform_btw_from_to_date_and_status() {

    var From_date = document.getElementById("FromDate").value;

    var To_date = document.getElementById("ToDate").value;

    var status_selected = $("#Status option:selected").text();


    if (!(From_date != "" && To_date == "") || !(From_date == "" && To_date != "")) {

        GetDataTableData('Load', From_date, To_date, status_selected);

    }

}




function Accept_image_type_only() {

    var file_name = document.getElementById("file").files[0].name;

    var file_extension = file_name.split('.').pop();


    if (file_extension == "jpg" || file_extension == "jpeg" || file_extension == "png") {
        
    }
    else {
        $("#file").val('');
        alert("Files of type jpg, jpeg, png are only accepted");
    }



    //alert(file_extension);
}







function Formsubmit() {


    

    
    if (add_upd == 1)
        SaveAndUpdateComplaintInfo();
   else
        updateComplaintInfo();
    
    
        
    

    
}

function updateComplaintInfo() {


    var isvalid = 1;
    var formData = new FormData();

    //var file = document.getElementById("file").files[0].name;
    var file = document.getElementById("file").files[0];

    var input = {

        //revised_ward_num: $("#revised_ward_num").val(),
        Status: $("#Status").val(),
        Action_Remark: $("#Action_Remark").val(),
        complaint_address: $("#complaint_add").val(),
        comp_id: comp_id,
        complaint_num: $("#complaint_num").val()


    };

    //formData.append("revised_ward_num", input.revised_ward_num);
    formData.append("Status", input.Status);
    formData.append("Action_Remark", input.Action_Remark);
    formData.append("complaint_address", input.complaint_address);
    formData.append("comp_id", input.comp_id);
    formData.append("complaint_num", input.complaint_num);
    formData.append("file", file);


    if (input.revised_ward_num == '' || input.Status == '' || input.Action_Remark == '' || input.address == '')//|| input.Transfer_station == '')
        isvalid = 0;


    //var formData = new FormData();

    //myData = JSON.parse(input);

    var stringified = JSON.stringify(input);


    //formData.append('input', JSON.stringify(input));
    if (isvalid == 1) {
        $.ajax({
            type: "POST",
            url: '/Complaint/UpdateComplaintInfo?Complaintdata=' + stringified,
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (msg) {
                var myJson = JSON.parse(msg);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Complaint/AllComplaint');

                    $('#modal_form_AddDetail').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJson.Msg, '');

            },
            error: function (result) {
                ShowCustomMessage('0', 'Something Went Wrong!', '');
            }
        });
    }
    else
        ShowCustomMessage('0', 'Please Enter All Required Details', '');


}






function SaveAndUpdateComplaintInfo() {

    var isvalid = 1;
    var formData = new FormData();

    var file = document.getElementById("file").files[0];


    
    var RegDate = $("#RegDate").val();
    var ddlRegType = $("#ddlRegType option:selected").text();
    var citizen_name = $("#citizen_name").val();
    var citizen_email =  $("#citizen_email").val();
    var txtContactNo = $("#txtContactNo").val();
    var ddlComplaint_Type = $("#ddlComplaint_Type option:selected").text();
    var ddlWard =  $("#ddlWard option:selected").text();
    var txt_circle = $("#txt_circle").val();
    var txt_Zone = $("#txt_Zone").val();
    var complaint_add = $("#complaint_add").val();
    var complaint_descrip = $("#complaint_descrip").val();

    

    formData.append("RegDate", RegDate);
    formData.append("ddlRegType", ddlRegType);
    formData.append("citizen_name", citizen_name);
    formData.append("citizen_email", citizen_email);
    formData.append("txtContactNo", txtContactNo);
    formData.append("ddlComplaint_Type", ddlComplaint_Type);
    formData.append("ddlWard", ddlWard);

    formData.append("txt_circle", txt_circle);
    formData.append("txt_Zone", txt_Zone);

    formData.append("file", file); 
    formData.append("complaint_add", complaint_add);
    formData.append("complaint_descrip", complaint_descrip);
    
    
  //  if (input.Complaint_num == '' || input.Complaint_cat == '' || input.Complaint_add == '' || input.Complaint_descrip == '' )//|| input.Transfer_station == '')
        //isvalid = 0;


    //var formData = new FormData();

    //myData = JSON.parse(input);

    //var stringified = JSON.stringify(input);


    //formData.append('input', JSON.stringify(input));
    if (isvalid == 1) {
        $.ajax({
            type: "POST",
            url: '/Complaint/SaveAndUpdateComplaintInfo',// + stringified,
            data: formData,
            dataType: 'json',
            contentType: false,
            processData: false,
            success: function (msg) {
                var myJson = JSON.parse(msg);
                if (myJson.Result == 1 || myJson.Result == 2) {

                    ShowCustomMessage('1', myJson.Msg, '/Complaint/AllComplaint');

                    $('#modal_form_AddDetail').modal('toggle');
                }
                else
                    ShowCustomMessage('0', myJson.Msg, '');

            },
            error: function (result) {
                ShowCustomMessage('0', 'Something Went Wrong!', '');
            }
        });
    }
    else
        ShowCustomMessage('0', 'Please Enter All Required Details', '');

}




var add_upd;

var comp_id;



function CallFunc(obj,i) {


    add_upd = i;


    var ddId = $(obj).attr('complaintId');

    comp_id = ddId;

    

    if (i == 1) {
        $('#user_content').load("/Complaint/AddComplaint");

    }
    else { 
        $('#user_content').load("/Complaint/UpdateComplaint");
        SetDataOnControls(ddId);
    }
    $('#modal_form_AddDetail').modal('toggle');

    

        //SetDataOnControls(ddId);

    
}


function SetDataOnControls(ddId) {
    $.ajax({
        type: "post",
        url: "/Complaint/GetComplaintInfoById",
        data: { CId: ddId },
        success: function (data4) {

            var myJSON = JSON.parse(data4);


            //$("#hfCId").val(myJSON.CId);

            


            $("#add_upd1").val('2');



            $("#SComplaintId").val(myJSON.SComplaintId);

            $("#Action_Remark").val(myJSON.ActionRemarks);




            
            $("#complaint_num").val(myJSON.Complaintcode);

            $("#revised_ward_num").val(myJSON.WardNo);

            $("#complaint_add").val(myJSON.Location);
            
            if (myJSON.IsClosed == 'True')
                $("#Status").val("2");
            else
                $("#Status").val("3");

            
            
        }
    });
}







function DownloadFile(FType) {

    var TId = getUrlParameterInfo('TId');
    var TName = getUrlParameterInfo('TName');
    $("#spnHeader").html('');
    $("#spnHeader").html(TName);
    var IsClick = '0';
    var NotiId = TId;

    var TName = "All complaints";

    
    ShowLoading($('#dvNotification'));
    $.ajax({
        type: "POST",
        url: "/Complaint/ExportGetAllStaffComplaint_Paging",
        data: { ContratorId: IsClick, NotiId: NotiId, FName: TName },
        xhrFields: {
            responseType: 'blob' // to avoid binary data being mangled on charset conversion
        },
        success: function (response) {
            HideLoading($('#dvNotification'));
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
            HideLoading($('#dvNotification'));
        }
    });
}

