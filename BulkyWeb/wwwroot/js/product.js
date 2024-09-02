var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/product/getall' },
        "columns": [
            { "data": "title", "width": "25%" },
            { "data": "isbn", "width": "15%" },
            { "data": "listPrice", "width": "10%" },
            { "data": "author", "width": "15%" },
            { "data": "category.name", "width": "10%" },
            {
                "data": "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/product/upsert?id=${data}" class="btn btn-primary rounded-pill shadow-lg px-4 py-2 d-flex align-items-center me-2">
                                        <i class="bi bi-pencil-square me-2"></i> Edit </a>
                    <a onClick=Delete('/admin/product/delete/${data}') class="btn btn-danger rounded-pill shadow-lg px-4 py-2 d-flex align-items-center">
                                        <i class="bi bi-trash me-2"></i> Delete </a>
                                    
                    </div >`
                },

                "width": "25%"
            }

        ]
    });
}
    function Delete(url) {
        Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: url,
                    type: 'DELETE',
                    success: function (data) {
                        dataTable.ajax.reload();
                        toatstr.success(data.message);
                    }
                }

              )
            }
        });

    }

