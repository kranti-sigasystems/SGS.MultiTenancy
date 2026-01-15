let createModal;
let editModal;
let deleteModel;
document.addEventListener('DOMContentLoaded', function () {
    createModal = new bootstrap.Modal(document.getElementById('createPermissionModal'));
    editModal = new bootstrap.Modal(document.getElementById('permissionModal'));
    deleteModal = new bootstrap.Modal(document.getElementById('deletePermissionModal'));
});

function openCreate() {
    document.getElementById('CreatePermissionCode').value = '';
    document.getElementById('CreatePermissionDescription').value = '';
    createModal.show();
}

function openEdit(id, code, description) {
    document.getElementById('UpdatePermissionId').value = id;
    document.getElementById('UpdatePermissionCode').value = code;
    document.getElementById('UpdatePermissionDescription').value = description;
    editModal.show();
}


function openDelete(id) {
    console.log("delete is open")
    document.getElementById("DeletePermissionId").value = id;
    deleteModal.show()
}


// js form validation
$(function () {
    $('form').each(function () {
        var $form = $(this);

       
        if ($form.data('validator')) {

            var validator = $form.validate();

          
            validator.settings.onkeyup = false;

           
            validator.settings.onfocusout = function (element) {
                $(element).valid();
            };
        }
    });
});
