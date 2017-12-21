

/*
$(function () {
    $.validator.methods.date = function (value, element) {
        //Fix chrom Asp.Net MVC jQuery validation: The field "Myfield" must be a date.
        var ischrom = /chrom(e|ium)/.test(navigator.userAgent.toLowerCase());
        if (ischrom) {
            if ($.browser.webkit) {
                //Chrome does not use the locale when new Date objects instantiated:
                var d = new Date();
                return this.optional(element) || !/Invalid|NaN/.test(new Date(d.toLocaleDateString(value)));
            }
            else {
                return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
            }
        }
        else {
            return this.optional(element) || !/Invalid|NaN/.test(new Date(value));
        }
    };

});
*/