function validatephasename() {
    var inputValue = event.which;

    if (!(inputValue >= 65 && inputValue <= 122) && (inputValue != 32 && inputValue != 0)) {
        //$("#lblPhaseName").html("Not Allowed").show().fadeOut(2000);
        return false;
    }
}