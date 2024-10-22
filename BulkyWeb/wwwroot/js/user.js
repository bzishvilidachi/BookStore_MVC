var dataTable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/user/getall' },
        "columns": [
            
            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "5%" },
            {
                data: { id:"id", lockoutEnd:"lockoutEnd"},
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();

                    if (lockout > today) {

                        return `
                        <div class="w-75 btn-group" role = "group">
                         <a onClick=LockUnlock('${data.id}') class="btn btn-success rounded-pill shadow-lg px-3 py-2 d-flex align-items-center me-2" style="cursor:pointer; width:89px; margin-right:5px;">
                            <i class="bi bi-unlock-fill"></i> Unlock
                        </a>
                        
                           <a href="/admin/user/rolemanagement?id=${data.id}" class="btn btn-primary rounded-pill shadow-lg px-4 py-2 d-flex align-items-center me-2" style="cursor:pointer; width:138px; margin-right:5px;">
                                <i class="bi bi-pencil-square me-2"></i> Permission
                           </a>
                   
                        </div >`

                    } else {

                        return `
                        <div class="w-75 btn-group" role = "group">
                         <a onClick=LockUnlock('${data.id}') class="btn btn-danger rounded-pill shadow-lg px-3 py-2 d-flex align-items-center me-2" style="cursor:pointer; width:89px; margin-right:5px;">
                            <i class="bi bi-lock-fill"></i> Lock
                        </a>
                        
                        <a href="/admin/user/rolemanagement?id=${data.id}" class="btn btn-primary rounded-pill shadow-lg px-4 py-2 d-flex align-items-center me-2" style="cursor:pointer; width:138px; margin-right:5px;">
                                <i class="bi bi-pencil-square me-2"></i> Permission
                        </a>
                   
                        </div >`
                    }
                  
                },

                "width": "25%"
            }

        ]
    });
}





function LockUnlock(id) {
    $.ajax({
        type: "POST",
        url: '/Admin/User/LockUnlock',
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
        }

    });
}