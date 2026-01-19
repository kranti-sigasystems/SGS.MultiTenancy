//// wwwroot/js/tenant.js

//// ==============================
//// Country → State dynamic dropdown
//// ==============================
//$(document).on('change', '#CountryID', function () {

//    const countryId = $(this).val();
//    const stateDropdown = $('#StateID');

//    stateDropdown.empty()
//        .append('<option value="">Select State</option>');

//    if (!countryId) return;

//    $.ajax({
//        url: '/Tenant/GetStates',
//        type: 'GET',
//        data: { countryId: countryId },
//        success: function (data) {
//            $.each(data, function (_, state) {
//                stateDropdown.append(
//                    $('<option></option>')
//                        .val(state.value)
//                        .text(state.text)
//                );
//            });
//        },
//        error: function () {
//            alert('Failed to load states');
//        }
//    });
//});


//// ==============================
//// Delete modal
//// ==============================
//$(document).on('show.bs.modal', '#deleteTenantModal', function (event) {

//    const button = event.relatedTarget;
//    const tenantId = button.getAttribute('data-tenant-id');
//    const tenantName = button.getAttribute('data-tenant-name');

//    $('#tenantId').val(tenantId);
//    $('#tenantName').text(tenantName);
//});

//// ==============================
//// Edit modal: Load form dynamically
//// ==============================
//$(document).on('show.bs.modal', '#editTenantModal', function (event) {

//    const button = $(event.relatedTarget);
//    const tenantId = button.data('id');

//    $('#editTenantBody').html(
//        '<div class="text-center p-3"><div class="spinner-border"></div></div>'
//    );

//    $('#editTenantBody').load('/Tenant/UpdateTenant?id=' + tenantId);
//});
