(function () {
    var defaultOptions = {
        highlight: function (element) {
            var $formGroup = $(element).closest(".form-group");
            $formGroup.find(".form-control").addClass('is-invalid');
        },
        unhighlight: function (element) {
            var $formGroup = $(element).closest(".form-group");
            $formGroup.find(".form-control").removeClass('is-invalid');
            $formGroup.find(".form-control").addClass('is-valid');
        }
    };

    $.validator.setDefaults(defaultOptions);
})();
