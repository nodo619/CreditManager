Ext.define('CreditManager.UI.view.auth.LoginForm', {
    extend: 'Ext.form.Panel',
    xtype: 'loginform',
    title: 'Login',
    bodyPadding: 20,

    requires: [
        'CreditManager.UI.service.ApiService'
    ],

    items: [
        { xtype: 'textfield', name: 'username', fieldLabel: 'Username', allowBlank: false },
        { xtype: 'textfield', name: 'password', fieldLabel: 'Password', inputType: 'password', allowBlank: false }
    ],

    buttons: [
        {
            text: 'Login',
            formBind: true,
            handler: async function (btn) {
                const form = btn.up('form');
                const values = form.getValues();

                try {
                    await CreditManager.UI.service.ApiService.login(values.username, values.password);
                    window.location.reload();
                } catch (e) {
                    Ext.Msg.alert('Login Failed', e.message);
                }
            }
        },
        {
            text: 'Register',
            handler: function () {
                Ext.destroy(Ext.ComponentQuery.query('loginform')[0]);
                Ext.create('CreditManager.UI.view.auth.RegistrationForm', { renderTo: Ext.getBody() });
            }
        }
    ]
});