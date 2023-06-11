$(document).ready(function () {
    $('.otp-input').on('input', function () {
        var filledDigits = $('.otp-input').filter(function () {
            return this.value.length > 0;
        });

        $('#verifyButton').prop('disabled', filledDigits.length !== 6);

        var maxLength = parseInt($(this).attr('maxlength'));
        if ($(this).val().length === maxLength) {
            var currentIndex = $(this).index('.otp-input');
            var nextIndex = currentIndex + 1;
            var nextInput = $('.otp-input').eq(nextIndex);

            if (nextInput.length > 0) {
                nextInput.focus();
            } else {
                $(this).blur();
            }
        }
    });

    $('.otp-input').on('keydown', function (e) {
        var currentIndex = $(this).index('.otp-input');
        var previousIndex = currentIndex - 1;
        var previousInput = $('.otp-input').eq(previousIndex);

        if (e.which === 8 && $(this).val().length === 0 && previousInput.length > 0) {
            previousInput.focus();
        }
    });
});