var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#departmentTbl').DataTable({
        "ajax": {
            url : '/Department/GetDepartment',
        },
        "columns": [
            { 'data': 'departmentId' },
            { 'data': 'name' },
            { 'data': 'statusTbl.name' },
            { 'data': 'bankTbl.name' },
            {
                'data': 'departmentId',
                "render": function (data) {
                    return `
                        <a href="/Department/Edit?id=${data}" class="btn btn-sm btn-floating">
                        <i class="mdi mdi-pencil text-primary" style="font-size:1.1rem"></i>
                        </a>
                        <a onclick="Delete('/Department/Delete?id=${data}')" class="btn btn-sm btn-floating">
                        <i class="mdi mdi-trash-can text-danger" style="font-size:1.1rem"></i>
                        </a>
                    `;
                }
            }

            ]

        })
}

function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    if (data.success) {
                        console.log(data)

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