$(document).ready(function () {
    
    $('#tlbRole').DataTable().clear().destroy();
    $('#tblMenumaster').DataTable().clear().destroy();

    //$('#ddlVehicleType').multiselect({
    //    selectAllValue: 'multiselect-all',
    //    maxHeight: '300',
    //    buttonWidth: '235',
    //    numberDisplayed: 2,
    //    // nonSelectedText: 'Select Vehicle',
    //    //includeSelectAllOption: true,
    //    enableFiltering: true,
    //    enableCaseInsensitiveFiltering: true
    //});
   // GetAllVehicleType();
    GetDataTableData();



});
function GetAllVehicleType() {
    //var selected_val = $('#ddlDepartment').find(":selected").attr('value');
    

    $("#ddlVehicleType").html();
  

    $.ajax({
        type: "post",
        url: "/Master/GetAllVehicleType",
        data: {  },
        success: function (data) {
            var Myjson = JSON.parse(data);
          

            var Resource = "<select id='ddlVehicleType' class='form-control'>";
           
               // Resource = Resource + '<option value="0">Select</option>';
            for (var i = 0; i < Myjson.length; i++) {
                Resource = Resource + '<option value=' + Myjson[i].VehicleTypeId + '>' + Myjson[i].VehicleType + '</option>';
            }
            Resource = Resource + '</select>';
            $('#ddlVehicleType').html(Resource);
        }

    });
}
function Formsubmit() {
    SaveAndUpdateMenuInfo();
}
function GetDataTableData() {

    $('#tlbRole tbody').empty();

    if ($('#hfTotalrows').val() > 0)
        $('#tlbRole').DataTable().clear().destroy();
    $.ajax({
        type: "post",
        url: "/User/GetAllRoles",
        data: '{}',
        success: function (data) {

            var myJSON = JSON.parse(data);

            var rowcount = myJSON.length;


            for (var i = 0; i < myJSON.length; i++) {
                var txtrow = ''
                if (myJSON[i].fbIsActive)
                    txtrow = '<tr><td>' + (i + 1) + '</td><td>' + myJSON[i].ftRoleName + '</td><td><span class="badge badge-success">ACTIVE</span></td><td><a isactive="' + myJSON[i].fbIsActive + '" roleName="' + myJSON[i].ftRoleName + '" cid="' + myJSON[i].fnRoleId + '" VehicleTypeId="' + myJSON[i].fnVehicleTypeId + '" href="javascript: void (0);" title="edit" onclick="CallFunc(this);"><i class="ti-pencil"></i></a></td></tr>';
                else
                    txtrow = '<tr><td>' + (i + 1) + '</td><td>' + myJSON[i].ftRoleName + '</td><td><span class="badge badge-danger">DE-ACTIVE</span></td><td><a isactive="' + myJSON[i].fbIsActive + '" roleName="' + myJSON[i].ftRoleName + '" cid="' + myJSON[i].fnRoleId  + '" VehicleTypeId="' + myJSON[i].fnVehicleTypeId + '" href="javascript: void (0);" title="edit" onclick="CallFunc(this);"><i class="ti-pencil"></i></a></td></tr>';

                $('#tlbRole tbody').append(txtrow);
            }
            // $("#tlbRole tbody").html(txtrow);

            var tabid = $('#tlbRole');
            $('#hfTotalrows').val(rowcount);
            if ($('#hfTotalrows').val() > 0)
                setdatatable();
        }
    });
}


function GetAllMenu(rid) {
    $('#tblMenumaster > tbody').empty();
    $.ajax({
        type: "post",
        url: "/User/GetAllMenuMaster",
        data: { roleId: rid },
        success: function (data) {
            var myJSON = JSON.parse(data);
            var myJSON1 = JSON.parse(myJSON.data1);
            var myJSON2 = JSON.parse(myJSON.data2);


            for (var i = 0; i < myJSON1.length; i++) {
                var sutd = '';
                var subHtm = '';
                for (var j = 0; j < myJSON2.length; j++) {
                    if (myJSON2[j].PMenuId == myJSON1[i].fnMenuId) {
                        if (myJSON2[j].IsEnable == 1)
                            sutd += '<td><input checked="checked" type="checkbox" submenuid="' + myJSON2[j].fnSubMenuId + '" />' + myJSON2[j].ftSubMenuName + '</td>';
                        else
                            sutd += '<td><input type="checkbox" submenuid="' + myJSON2[j].fnSubMenuId + '" />' + myJSON2[j].ftSubMenuName + '</td>';

                    }
                }
                subHtm = '<table class="submenutable"><tr>' + sutd + '</tr></table>';
                // subHtm = '';
                var txtrow = '<tr><td>' + myJSON1[i].ftMenuName + '</td><td>' + subHtm + '</td></tr>';

                $('#tblMenumaster > tbody').append(txtrow);
                sutd = '';
                subHtm = '';
            }
        }
    });
}
function SaveAndUpdateMenuInfo() {

    ;
    if ($('#txtRoleName').val().trim() == '') {
        alert('Role name is required');
        return;
    }
    var isvalid = 1;
    var menuary = [];
    var isActive = $('#cbIsActive').is(':checked');
    var rid = $('#RoleId').val();
    var VehicleTypeId = $('#ddlVehicleType').val();
 



    


    var table = $(".submenutable tbody");

    $(".submenutable tbody tr").each(function () {
        $(this).find('td').each(function (key, val) {
            var ids = $(this).find('input[type=checkbox]').attr('submenuid');
            var isChecked = $(this).find('input[type=checkbox]').is(':checked');
            var sData = {
                fnSubMenuId: ids,
                fbIsAllow: isChecked
            };
            menuary.push(sData);
        });
    });

    $.ajax({
        type: "POST",
        url: '/User/SaveandupdateMenu',
        data: { roleName: $('#txtRoleName').val(), JArrayval: JSON.stringify(menuary), roleId: rid, IsActive: isActive, VehicleTypeId: JSON.stringify(VehicleTypeId) },
        success: function (reponse) {
            var data = JSON.parse(reponse);
            if (data.Result == 1 || data.Result == 2) {

                ShowMessage('1', data.Msg);

                $('#modal_form_vertical').modal('toggle');
            }
            else
                ShowMessage('0', data.Msg);

        },
        error: function (result) {
            ShowMessage('0', 'Something Went Wrong!');
        }
    });
}
function setdatatable() {
    $('#tlbRole').DataTable({
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
                    extension: '.csv'
                },
                {
                    extend: 'colvis',
                    text: '<i class="icon-three-bars"></i>',
                    className: 'btn bg-blue btn-icon dropdown-toggle'
                }
            ]
        }

    });
}

function CallFunc(obj) {

   
    var rid = $(obj).attr('cid');

    if (rid > 0) {
        $('#modalTitle').text('Update Roles Information');
    }
    else {
        $('#modalTitle').text('Add Roles Information');
    }
   // $('#user_content').load("/User/AddRoles");
    $('#modal_form_vertical').modal('toggle');


    GetAllMenu(rid);
    var VehicleId = $(obj).attr('VehicleTypeId');
    $("#ddlVehicleType").select2({

        dropdownParent: $("#modal_form_vertical")

    });
    setTimeout(() => {
        GetAllVehicleType();
        setTimeout(() => {
            if (VehicleId != undefined) {
                const array = VehicleId.split(',');
                $('#ddlVehicleType').val(array).trigger('change')
               

               
            }
        }, 1000);
    }, 1000);
   
   
    var RoleName = $(obj).attr('rolename');
    var IsActive = $(obj).attr('isactive');
 

   


    
    $('#txtRoleName').val(RoleName);

    $('#RoleId').val(rid);
    if (IsActive=="true")
        $('#cbIsActive').attr('checked', 'checked');
    else
        $('#cbIsActive').removeAttr('checked');




}
function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
};
function ShowMessage(typemsg, msg) {

    if (typemsg == '1') {
        swal({
            title: 'Good job!',
            text: msg,
            type: 'success'
        },
            function () {
                window.location.href = '/User/AllRoles';
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