
document.addEventListener("DOMContentLoaded", function () {
    const modal = document.getElementById('addressModal');

    modal.addEventListener('show.bs.modal', function (event) {
        const button = event.relatedTarget;

        document.getElementById('modalLine1').textContent = button.dataset.line1 || '-';
        document.getElementById('modalCity').textContent = button.dataset.city || '-';
        document.getElementById('modalState').textContent = button.dataset.state || '-';
        document.getElementById('modalCountry').textContent = button.dataset.country || '-';
        document.getElementById('modalPostal').textContent = button.dataset.postal || '-';
    });
});