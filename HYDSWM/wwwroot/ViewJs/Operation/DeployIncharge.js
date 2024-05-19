
$(document).ready(function () {
    $('#Email').on('keypress', function (e) {
        if (e.which == 32) {
            // console.log('Space Detected');
            return false;
        }
    });
    GetDataTableData();
    GetAllRoles();
    GetAllEmp();
    $('#tblCirclemaster').DataTable().clear().destroy();
});

var dt;
function GetDataTableData() {
    dt = $('#example').DataTable({
        processing: true,
        destroy: true,
        responsive: true,
        serverSide: true,
        searchable: true,
        lengthMenu: [[10, 20, 50, 100, 500, 10000], [10, 20, 50, 100, 500, 10000]],
        language: {
            infoEmpty: "No records available",
            searchPlaceholder: "Search Records"
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
            url: "/Operation/GetAllDeployIncharge/",
            // contentType: "application/json",
            type: 'POST',
            data: function (d) {
                return { requestModel: d };
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
            { data: "Name", sortable: true },
           
           
            { data: "ContactNo", sortable: true },
            //{ data: "EmailId", sortable: true },
            { data: "LastLoginDate", sortable: false },
            
            /*{ data: "WardNo", sortable: false },*/
            {
                sortable: true,
                "render": function (data, type, row, meta) {

                    if (row.IsActive == 1)
                        return '<span class="badge badge-success">ACTIVE</span>';
                    else
                        return '<span class="badge badge-danger">DE-ACTIVE</span>';

                }
            },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    //var singleSentShowURL = '/User/GetUserDataById';
                    //singleSentShowURL = singleSentShowURL + "?param=" + row.UserId;
                    return "<a cid='" + row.DIId + "' href='javascript:void(0);' title='edit' loginid='" + row.ContactNo + "' onclick='CallFunc(this);'><i class='ti-pencil'></i></i></a>";
                }
            }
        ]
    });
}
function GetAllRoles() {
    $('#ddlRole').html('');

    $.ajax({
        type: "post",
        url: "/User/GetAllRoles",
        data: {},
        datatype: "json",
        traditional: true,
        success: function (response) {
            var data = JSON.parse(response);
            var Resource = "<select id='ddlRole'>";
            Resource = Resource + '<option value="">Select</option>';
            for (var i = 0; i < data.length; i++) {
                Resource = Resource + '<option value=' + data[i].fnRoleId + '>' + data[i].ftRoleName + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlRole').html(Resource);
        }
    });
}
function Formsubmit() {
    SaveAndUpdateMenuInfo();
    return false;
}
function SaveAndUpdateMenuInfo() {
    //;
    var isvalid = 1;
    if ($("#FullName").val().trim() == '') { isvalid = 0; }
    //if ($("#Email").val().trim() == '') { isvalid = 0; }
    //if ($("#ddlRole").val() == '') { isvalid = 0; }
    //if ($("#ddlRole").val() == '') { isvalid = 0; }
    if ($("#Password").val().trim() == '') { isvalid = 0; }

    if (isvalid == 1) {
        var FormData = {
            UserId: $("#UserId").val(),
            Name: $("#FullName").val(),
            LoginId: $("#Email").val(),
            Pwd: $('#Password').val(),
            ContactNo: $('#Mobile').val(),
            IsActive: $("#IsActive").is(':checked')
            
        };

        var menuary = [];
        var isActive = $('#cbIsActive').is(':checked');
        var rid = $('#RoleId').val();
        var table = $(".submenutable tbody");
        
        $(".submenutable tbody tr").each(function () {
            $(this).find('td').each(function (key, val) {
                if ($(this).find('input[type=checkbox]').is(':checked') == true) {
                    var circleid = $(this).find('input[type=checkbox]').attr('circleid');
                    var wardid = $(this).find('input[type=checkbox]').attr('wardid');
                    var zoneid = $(this).find('input[type=checkbox]').attr('zoneid');
                    var LandMarkId = $(this).find('input[type=checkbox]').attr('LandMarkId');
                    var isChecked = $(this).find('input[type=checkbox]').is(':checked');
                    var sData = {
                        CircleId: circleid,
                        WardId: wardid,
                        ZoneId: zoneid,
                        LandMarkId: LandMarkId,
                        LoginId: $('#Mobile').val()
                    };
                    menuary.push(sData);
                }
                
            });
        });
        
        if (menuary.length > 0) {
            $.ajax({
                type: "POST",
                url: '/Operation/SaveandupdateDeployIncharge',
                data: { jobj: JSON.stringify(FormData), JArrayval: JSON.stringify(menuary) },
                success: function (reponse) {

                    var data = JSON.parse(reponse);
                    if (data[0].Result == 1 || data[0].Result == 2) {
                        //$('#modal_form_vertical').modal('toggle');
                        //$('#FullName').val('');
                        //$('#EmpCode').val('');
                        //$('#Email').val('');
                        //$('#Mobile').val('');
                        //$('#ddlRole').val('');
                        //$('#Password').val('');
                        //$('#UserId').val('0');
                        //$('#ConfirmPassword').val('');
                        //$('#IsActive').attr('checked', 'checked');
                        ShowMessage('1', data[0].Msg);

                    }
                    else
                        ShowMessage('0', data[0].Msg);

                },
                error: function (result) {
                    ShowMessage('0', 'Something Went Wrong!');
                }
            });
        }
        else {
            ShowMessage('0', 'Select At Least One Landmark!');
        }
    }
}
function ShowMessage(typemsg, msg) {

    if (typemsg == '1') {
        swal({
            title: 'Good job!',
            text: msg,
            type: 'success'
        },
            function () {
                window.location.href = '/Operation/AllDeployIncharge';
            });
    }
    else {
        swal({
            title: 'Oops...',
            text: msg,
            type: 'error'
        });
    }
}
function GetUserInfo(cid) {
    $.ajax({
        type: "post",
        url: "/Operation/GetDeployInchargeDataById",
        data: { UserId: cid },
        datatype: "json",
        traditional: true,
        success: function (data) {

            $('#FullName').val(data.Name);

            $('#Email').val(data.LoginId);
            $('#Mobile').val(data.ContactNo);
            
            $('#Password').val(data.Pwd);
            $('#UserId').val(data.DIId);
            if (data.IsActive == "True")
                $('#IsActive').attr('checked', 'checked');
            else
                $('#IsActive').removeAttr('checked');
            $('#Email').attr('readonly', 'readonly');
        }
    });
}
function GetAllEmp() {
    // var selected_val = $('#ddlCircle').find(":selected").attr('value');
    $('#ddlEmp').html('');

    $.ajax({
        type: "post",
        url: "/Employee/GetEmployeeMaster",
        data: '{}',
        success: function (data) {
            var myJSON = JSON.parse(data);
            var Resource = "<select id='ddlEmp'>";
            Resource = Resource + '<option value="">--select--</option>';
            for (var i = 0; i < myJSON.length; i++) {
                Resource = Resource + '<option value=' + myJSON[i].EmpCode + '>' + myJSON[i].Name + '(' + myJSON[i].Designation + ')</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlEmp').html(Resource);
        }
    });
}
function GetAllCircleAndWard(rid) {
    $('#tblCirclemaster > tbody').empty();
    $.ajax({
        type: "post",
        url: "/Operation/GetAllDeployLocationAndWardMaster",
        data: { loginId: rid },
        success: function (data) {
            var myJSON = JSON.parse(data);
            var myJSON1 = JSON.parse(myJSON.data1);
            var myJSON2 = JSON.parse(myJSON.data2);

            
            for (var i = 0; i < myJSON1.length; i++) {
                var sutd = '';
                var subHtm = '';
                for (var j = 0; j < myJSON2.length; j++) {
                    if (myJSON2[j].ZoneId == myJSON1[i].CircleId) {
                        if (myJSON2[j].IsEnable == 1)
                            sutd += '<td><input checked="checked" type="checkbox"   LandMarkId="' + myJSON2[j].DLId + '"   circleid="' + myJSON2[j].CircleId + '"  wardid="' + myJSON2[j].WardId + '" zoneid="' + myJSON2[j].ZoneId + '" />' + myJSON2[j].LandMark + '</td>';
                        else
                            sutd += '<td><input type="checkbox"   LandMarkId="' + myJSON2[j].DLId + '"   circleid="' + myJSON2[j].CircleId + '"  wardid="' + myJSON2[j].WardId + '" zoneid="' + myJSON2[j].ZoneId + '"/>' + myJSON2[j].LandMark + '</td>';

                    }
                }
                subHtm = '<table class="submenutable"><tr>' + sutd + '</tr></table>';
                // subHtm = '';
                var txtrow = '<tr><td>' + myJSON1[i].CircleName + '</td><td>' + subHtm + '</td></tr>';

                $('#tblCirclemaster > tbody').append(txtrow);
                sutd = '';
                subHtm = '';
            }
        }
    });
}
function CallFunc(obj) {

    var ddId = $(obj).attr('cid');

    if (ddId > 0) {
        GetUserInfo(ddId);
        $('#modalTitle').text('Update Deploy Incharge Information');
    }
    else {
        $('#modalTitle').text('Add Deploy Incharge Information');
        $('#FullName').val('');
        $('#ddlEmp').val('');
        $('#Email').val('');
        $('#Mobile').val('');
        $('#ddlRole').val('');
        $('#Password').val('demo@123');
        $('#UserId').val('0');
        $('#IsActive').attr('checked', 'checked');
        $('#Email').removeAttr('readonly');
    }
    var loginid = $(obj).attr('loginid');
    GetAllCircleAndWard(loginid);

    $('#modal_form_vertical').modal('toggle');
}
function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};

function OnEmpChange(objthis) {
    var empCode = $(objthis).val();
    $.ajax({
        type: "post",
        url: "/Employee/EmloyeeInfoByEmpCode",
        data: { empcode: empCode },
        success: function (data) {
            var myJSON = JSON.parse(data);
            $('#FullName').val(myJSON.Name);
            $('#Email').val(myJSON.EmpCode);
            $('#Mobile').val(myJSON.ContactNo);
        }
    });
}
