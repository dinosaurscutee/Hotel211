$(document).ready(function () {
    var originalRoomTypes = $('#roomType').html();

    $('#roomType').change(function () {
        var selectedRoomType = $(this).val();

        if (selectedRoomType === '') {
            $('#roomType').html(originalRoomTypes);
        } else {
            $('#roomType option').each(function () {
                if ($(this).val() !== selectedRoomType && $(this).val() !== '') {
                    $(this).hide();
                } else {
                    $(this).show();
                }
            });
        }
    });
});
