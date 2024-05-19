$(document).ready(function () {
    AllRoleLst('ddlRole', 1, 'Select');
    $('#tlbRole').DataTable().clear().destroy();
    $('#tblMenumaster').DataTable().clear().destroy();
    GetDataTableData();

});

function Formsubmit() {
    SaveAndUpdateMenuInfo();
}
function GetDataTableData() {
    ShowLoading($('#tlbRole'));
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
                    txtrow = '<tr><td>' + (i + 1) + '</td><td>' + myJSON[i].ftRoleName + '</td><td><a isactive="' + myJSON[i].fbIsActive + '" roleName="' + myJSON[i].ftRoleName + '" cid="' + myJSON[i].fnRoleId + '" href="javascript: void (0);" title="edit" onclick="CallFunc(this);"><i class="ti-pencil"></i></a></td></tr>';
                else
                    txtrow = '<tr><td>' + (i + 1) + '</td><td>' + myJSON[i].ftRoleName + '</td><td><a isactive="' + myJSON[i].fbIsActive + '" roleName="' + myJSON[i].ftRoleName + '" cid="' + myJSON[i].fnRoleId + '" href="javascript: void (0);" title="edit" onclick="CallFunc(this);"><i class="ti-pencil"></i></a></td></tr>';

                $('#tlbRole tbody').append(txtrow);
            }
            // $("#tlbRole tbody").html(txtrow);

            var tabid = $('#tlbRole');
            $('#hfTotalrows').val(rowcount);
            if ($('#hfTotalrows').val() > 0)
                setdatatableoncontrol('example');
            HideLoading($('#tlbRole'));
        },
        error: function (result) {
            HideLoading($('#tlbRole'));
        }
    });
}


function GetAllMenu(rid) {
    $('#tblMenumaster > tbody').empty();
    $.ajax({
        type: "post",
        url: "/User/GetAllMobileMenuMaster",
        data: { roleId: rid },
        success: function (data) {
            var myJSON = JSON.parse(data);
            var myJSON1 = JSON.parse(myJSON.data1);
            var myJSON2 = JSON.parse(myJSON.data2);


            for (var i = 0; i < myJSON1.length; i++) {
                var sutd = '';
                var subHtm = '';
                for (var j = 0; j < myJSON2.length; j++) {
                    if (myJSON2[j].PMenuId == myJSON1[i].MMId) {
                        if (myJSON2[j].IsEnable == 1)
                            sutd += '<td><input checked="checked" type="checkbox" submenuid="' + myJSON2[j].MSubMenuId + '" />' + myJSON2[j].SubMenuName + '</td>';
                        else
                            sutd += '<td><input type="checkbox" submenuid="' + myJSON2[j].MSubMenuId + '" />' + myJSON2[j].SubMenuName + '</td>';

                    }
                }
                subHtm = '<table class="submenutable"><tr>' + sutd + '</tr></table>';
                // subHtm = '';
                var txtrow = '<tr><td>' + myJSON1[i].MenuName + '</td><td>' + subHtm + '</td></tr>';

                $('#tblMenumaster > tbody').append(txtrow);
                sutd = '';
                subHtm = '';
            }
        }
    });
}
function SaveAndUpdateMenuInfo() {

    if ($('#txtRoleName').val().trim() == '') {
        alert('Role name is required');
        return;
    }
    var isvalid = 1;
    var menuary = [];
    var rid = $('#hfRoleId').val();
    var table = $(".submenutable tbody");

    $(".submenutable tbody tr").each(function () {
        $(this).find('td').each(function (key, val) {
            var ids = $(this).find('input[type=checkbox]').attr('submenuid');
            var isChecked = $(this).find('input[type=checkbox]').is(':checked');
            if (isChecked == true) {
                var sData = {
                    SubMenuId: ids,
                    RoleId: rid
                };
                menuary.push(sData);
            }
        });
    });
    if (menuary.length > 0) {
    $.ajax({
        type: "POST",
        url: '/User/SaveandupdateMobileMenu',
        data: { JArrayval: JSON.stringify(menuary) },
        success: function (reponse) {
            var data = JSON.parse(reponse);
            if (data.Result == 1 || data.Result == 2) {

                ShowCustomMessage('1', data.Msg,'/User/AllMobileRole');

                $('#modal_form_vertical').modal('toggle');
            }
            else
                ShowCustomMessage('0', data.Msg,'');

        },
        error: function (result) {
            ShowCustomMessage('0', 'Something Went Wrong!','');
        }
    });
    }
    else
        ShowCustomMessage('0', 'Please Select Shift And Date Details', '');
}

function CallFunc(obj) {


    var rid = $(obj).attr('cid');

    if (rid > 0) {
        $('#modalTitle').text('Update App Roles Information');
    }
    else {
        $('#modalTitle').text('Add App Roles Information');
    }
    // $('#user_content').load("/User/AddRoles");
    $('#modal_form_vertical').modal('toggle');
    GetAllMenu(rid);

    var RoleName = $(obj).attr('rolename');
    $('#txtRoleName').val(RoleName);

    $('#hfRoleId').val(rid);
    

}
