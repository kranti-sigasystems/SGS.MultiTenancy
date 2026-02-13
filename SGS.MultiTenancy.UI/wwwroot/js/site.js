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
});
document.addEventListener("change", function (e) {

    if (e.target.classList.contains("countryDropdown")) {

        var countryId = e.target.value;
        var stateDropdown = document.querySelector(".stateDropdown");

        stateDropdown.innerHTML = '<option value="">-- Select State --</option>';

        if (countryId) {
            fetch('/User/GetStatesByCountry?countryId=' + countryId)
                .then(response => response.json())
                .then(data => {
                    data.forEach(function (state) {

                        var option = document.createElement("option");
                        option.value = state.value;
                        option.textContent = state.name;
                        stateDropdown.appendChild(option);
                    });
                });
        }
    }
});