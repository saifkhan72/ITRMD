let dataTable;

$(document).ready(function () {
    showLoader(); // Show the loader initially
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tbl').DataTable({
        "ajax": {
            "url": "/bank/getbanks",
            
        },
        "columns": [
            { "data": "bankId"},
            { "data": "name",},
            { "data": "statusTbl.name", "width": "30%" },
            {
                "data": "bankId",
                "render": function (data) {
                    return `
                        <a href="/Bank/Edit?id=${data}" class="btn btn-sm btn-floating">
                            <i class="mdi mdi-pencil text-primary" style="font-size:1.1rem"></i>
                        </a>
                        <a onclick="Delete('/Bank/delete?id=${data}')" class="btn btn-sm btn-floating">
                            <i class="mdi mdi-trash-can text-danger" style="font-size:1.1rem"></i>
                        </a>
                    `;
                }
            }
        ],
        "drawCallback": function () {
            hideLoader(); // Hide the loader after DataTables renders the table
        }
    });
}
function showLoader() {
    $("#loader").show();
}

function hideLoader() {
    $("#loader").hide();
}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: (data) => {
                    if (data?.success) {
                        dataTable.ajax.reload();
                        toastr.success(data.message);
                    } else {
                        toastr.error(data.message)
                    }
                }

            })
        }
    })
}