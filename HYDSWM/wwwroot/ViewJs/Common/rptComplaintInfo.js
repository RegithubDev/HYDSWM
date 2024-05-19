
$(document).ready(function () {
    $('#txtFromDate').datepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });
    $('#txtToDate').datepicker({
        changeMonth: true,
        changeYear: true,
        maxDate: '0'
    });
    var date = new Date();
    document.getElementById("txtFromDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    document.getElementById("txtToDate").value = (date.getMonth() + 1) + '/' + date.getDate() + '/' + date.getFullYear();
    GetDataTableData('Load');
});
var dt;
function GetDataTableData(Type) {
    var FromDate = document.getElementById('txtFromDate').value;
    var ToDate = document.getElementById('txtToDate').value;
    var IsClick = '0';
    var NotiId = "";
    if (Type != 'Load') {
        IsClick = '1';
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
            url: "/Complaint/GetAllStaffComplaint_Paging/",
            type: 'POST',
            data: function (d) {
                d.ToDate = ToDate;
                d.FromDate = FromDate;
                d.ContratorId = IsClick;
                d.NotiId = NotiId;
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

                    return '<a class="gticket" cticketid="' + row.SComplaintId + '" href="' + row.Img1Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img1Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "Complaintcode", sortable: true },
            { data: "ComplaintType", sortable: true },
            { data: "Location", sortable: true },
            { data: "FAddress", sortable: true },
            { data: "Remarks", sortable: true },
            { data: "CreatedBy", sortable: true },
            { data: "ComplaintOn", sortable: true },
            { data: "ClosedOn", sortable: true },
            { data: "TotalTimeTaken", sortable: true },
            {
                sortable: true,
                "render": function (data, type, row, meta) {
                    return '<span class="' + row.LabelClass + '">' + row.Status + '</span>';
                }
            },

            { data: "CAddress", sortable: true },
            {
                sortable: false,
                "render": function (data, type, row, meta) {

                    return '<a class="gticket" cticketid="' + row.SComplaintId + '" href="' + row.Img2Base64 + '" data-fancybox="gallery"><img id="imageresource" src = "' + row.Img2Base64 + '" alt="" class="img-preview rounded"></a>';
                }
            },
            { data: "CRemarks", sortable: true },
            { data: "ClosedBy", sortable: true },
            { data: "TStationName", sortable: true }

        ]
    });

}
