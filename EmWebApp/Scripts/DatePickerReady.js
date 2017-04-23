
$(function () {
    
    $(".datefield").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: "1900:+nn",
        altFormat: "yy-mm-dd",
        dateFormat: "yy-mm-dd",
        constrainInput: true
    });

});

/*
    $(".datefield").datepicker();

Modernizr.load({
    test: Modernizr.inputtypes.date,
    yep: alert('Supports it!'),
    nope: alert('Oh, damn! This browser sucks!')
});


    nope: $(function () {

        $(".datefield").datepicker();

    })
$(function () {
    $(".datepicker").datepicker({
        format: "yyyy/mm/dd",
        autoclose: true
    });
    $(".datepicker").datepicker('setValue', new Date());
    $("#abc").datepicker();
    alert('okay');

});

*/