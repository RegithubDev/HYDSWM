$(document).ready(function () {

    GetDataTableData('Load');
    
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


var dt;
function GetDataTableData(Type) {
    var TId = '0';
    var TName = 'All Container';
    var CollectionTypeId = "0";
    
    
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
            url: "/Demo1/GetAllContainerInfo/",
            type: 'POST',
            data: function (d) {
                d.Status = TId;
                d.NotiId = CollectionTypeId;
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
            
            

            { data: "UId", sortable: true },
            { data: "ContainerCode", sortable: true },
            { data: "ContainerName", sortable: true },
            { data: "Capacity", sortable: true },
            { data: "ContainerType", sortable: true }

        ]
    });

    //if (TId != "")
    //    dt.column(8).visible(false);
}


