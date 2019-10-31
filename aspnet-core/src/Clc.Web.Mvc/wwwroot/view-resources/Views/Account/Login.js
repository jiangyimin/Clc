(function ($) {
    var _$Form = $('#LoginForm');

    function login() {
        abp.ui.setBusy(
            $('#LoginArea'),
            abp.ajax({
                contentType: 'application/x-www-form-urlencoded',
                url: _$Form.attr('action'),
                data: _$Form.serialize(),
            })              
        );
    }

    $('#VerifyButton').click(function (e) {
        e.preventDefault();
        if (!_$Form.form('validate'))
            return;
            
        $('#VerifyButton').attr("disabled", true);
        setTimeout(function () {
            $('#VerifyButton').attr("disabled", false);
        }, 5000);
        abp.ajax({
            contentType: 'application/x-www-form-urlencoded',
            url: "SendVerifyCode",
            data: _$Form.serialize()
        }).done(function(ret) {
            if (ret.success == true)
                abp.notify.info('已向'+$('#UserName').val()+'发送了验证码');
        });
    })

    $('#LoginButton').click(function (e) {
        e.preventDefault();
        if (!_$Form.form('validate'))
            return;

        var tenancyName = $('#TenancyName').val();    
        if (!tenancyName) {
            abp.multiTenancy.setTenantIdCookie(null);
            login();
        }
        else {
            abp.services.app.account.isTenantAvailable({tenancyName: tenancyName})
            .done(function (result) {
                switch (result.state) {
                    case 1: //Available
                        abp.multiTenancy.setTenantIdCookie(result.tenantId);
                        // location.reload();
                        login();
                        break;
                    case 2: //InActive
                        abp.message.warn(abp.utils.formatString(abp.localization
                            .localize("TenantIsNotActive", "Clc"),
                            tenancyName));
                        break;
                    case 3: //NotFound
                        abp.message.warn(abp.utils.formatString(abp.localization
                            .localize("ThereIsNoTenantDefinedWithName{0}", "Clc"),
                            tenancyName));
                        break;
                }
            })
        }
    })
})(jQuery);