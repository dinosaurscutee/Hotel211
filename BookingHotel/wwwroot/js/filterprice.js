$(document).ready(function () {
    // Lưu các phần tử cần thiết vào biến
    var rangeMinPrice = document.getElementById('rangeMinPrice');
    var rangeMaxPrice = document.getElementById('rangeMaxPrice');
    var minPriceInput = document.getElementById('minPrice');
    var maxPriceInput = document.getElementById('maxPrice');
    var minPriceValue = 0;
    var maxPriceValue = 120000;

    rangeMinPrice.addEventListener('input', function () {
        minPriceInput.value = this.value;
        minPriceValue = parseInt(this.value);
        if (minPriceValue > maxPriceValue) {
            maxPriceInput.value = minPriceValue;
            rangeMaxPrice.value = minPriceValue;
            maxPriceValue = minPriceValue;
        }
    });

    rangeMaxPrice.addEventListener('input', function () {
        maxPriceInput.value = this.value;
        maxPriceValue = parseInt(this.value);
        if (maxPriceValue < minPriceValue) {
            minPriceInput.value = maxPriceValue;
            rangeMinPrice.value = maxPriceValue;
            minPriceValue = maxPriceValue;
        }
    });

    $('.btn-primary').click(function () {
        var minPrice = parseInt($('#minPrice').val());
        var maxPrice = parseInt($('#maxPrice').val());

        $.ajax({
            url: '/Room/Rooms',
            type: 'GET',
            data: { minPrice: minPrice, maxPrice: maxPrice },
            success: function (data) {
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });
});