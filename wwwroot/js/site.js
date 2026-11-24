// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

document.querySelectorAll('.confirm-delete, .confirm-cancel').forEach(function (button) {
    button.addEventListener('click', function (event) {
        if (!confirm('Czy na pewno wykonać tę operację?')) event.preventDefault();
    });
});

var filmForm = document.getElementById('film-form');
if (filmForm) {
    filmForm.addEventListener('submit', function (event) {
        var dlugoscInput = filmForm.querySelector('[name="DlugoscMinuty"]');
        if (dlugoscInput) {
            var dlugosc = Number(dlugoscInput.value);
            if (!Number.isInteger(dlugosc) || dlugosc <= 0) {
                event.preventDefault();
                alert('Długość filmu musi być liczbą większą od 0.');
                dlugoscInput.focus();
            }
        }
    });
}

var reservationForm = document.getElementById('reservation-form');
if (reservationForm) {
    reservationForm.addEventListener('submit', function (event) {
        var selected = reservationForm.querySelector('input[name="ReservationSlotId"]:checked');
        if (!selected) {
            event.preventDefault();
            alert('Wybierz dostępny termin rezerwacji.');
        }
    });
}

var slotDate = document.querySelector('input[type="datetime-local"]');
if (slotDate) {
    var now = new Date();
    now.setMinutes(now.getMinutes() - now.getTimezoneOffset());
    slotDate.min = now.toISOString().slice(0, 16);
}
