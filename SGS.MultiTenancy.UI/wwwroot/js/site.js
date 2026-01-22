$(document).on('change', '#CountryID', function () {

    const countryId = $(this).val();
    const stateDropdown = $('#StateID');

    stateDropdown.empty()
        .append('<option value="">Select State</option>');

    if (!countryId) return;

    $.ajax({
        url: '/Tenant/GetStates',
        type: 'GET',
        data: { countryId },
        success: function (data) {
            $.each(data, function (_, state) {
                stateDropdown.append(
                    $('<option></option>')
                        .val(state.value)
                        .text(state.text)
                );
            });
        },
        error: function () {
            alert('Failed to load states');
        }
    });
});


document.addEventListener("DOMContentLoaded", function () {

    const addressModal = document.getElementById('addressModal');

    if (addressModal) {
        addressModal.addEventListener('show.bs.modal', function (event) {

            const button = event.relatedTarget;

            document.getElementById('modalLine1').textContent = button.dataset.line1 || '-';
            document.getElementById('modalCity').textContent = button.dataset.city || '-';
            document.getElementById('modalState').textContent = button.dataset.state || '-';
            document.getElementById('modalCountry').textContent = button.dataset.country || '-';
            document.getElementById('modalPostal').textContent = button.dataset.postal || '-';
        });
    }

    const editModal = document.getElementById('editTenantModal');
    const editBody = document.getElementById('editTenantBody');

    if (editModal && editBody) {
        editModal.addEventListener('show.bs.modal', function (event) {

            const button = event.relatedTarget;
            const tenantId = button.dataset.id;

            editBody.innerHTML = `
                <div class="text-center p-4">
                    <div class="spinner-border text-primary"></div>
                </div>`;

            fetch(`/Tenant/UpdateTenant?id=${tenantId}`)
                .then(response => response.text())
                .then(html => editBody.innerHTML = html)
                .catch(() => {
                    editBody.innerHTML =
                        '<div class="alert alert-danger">Unable to load tenant</div>';
                });
        });
    }

    const deleteModal = document.getElementById('deleteTenantModal');

    if (deleteModal) {
        deleteModal.addEventListener('show.bs.modal', function (event) {

            const button = event.relatedTarget;

            document.getElementById('tenantId').value = button.dataset.tenantId;
            document.getElementById('tenantName').textContent = button.dataset.tenantName;
        });
    }
});

//js for permissions management
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