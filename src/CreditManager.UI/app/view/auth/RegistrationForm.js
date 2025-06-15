Ext.define('CreditManager.UI.view.auth.RegistrationForm', {
    extend: 'Ext.form.Panel',
    xtype: 'registrationform',
    title: 'Register',
    bodyPadding: 20,

    items: [
        { xtype: 'textfield', name: 'username', fieldLabel: 'Username', allowBlank: false },
        { xtype: 'textfield', name: 'firstName', fieldLabel: 'First Name', allowBlank: false },
        { xtype: 'textfield', name: 'lastName', fieldLabel: 'Last Name', allowBlank: false },
        { xtype: 'textfield', name: 'personalNumber', fieldLabel: 'Personal Number', allowBlank: false },
        { xtype: 'datefield', name: 'birthDate', fieldLabel: 'Birth Date', format: 'Y-m-d', allowBlank: false },
        { xtype: 'textfield', name: 'password', fieldLabel: 'Password', inputType: 'password', allowBlank: false }
    ],

    buttons: [
        {
            text: 'Register',
            formBind: true,
            handler: async function (btn) {
                const form = btn.up('form');
                const values = form.getValues();
    
                try {
                    await CreditManager.UI.service.ApiService.register(values);
                    Ext.Msg.alert('Success', 'Registration successful, you can login now.', function () {
                        // On success, destroy RegistrationForm and load LoginForm
                        Ext.destroy(Ext.ComponentQuery.query('registrationform')[0]);
                        Ext.create('CreditManager.UI.view.auth.LoginForm', { renderTo: Ext.getBody() });
                    });
                } catch (e) {
                    Ext.Msg.alert('Registration Failed', e.message);
                }
            }
        },
        {
            text: 'Back to Login',
            handler: function () {
                Ext.destroy(Ext.ComponentQuery.query('registrationform')[0]);
                Ext.create('CreditManager.UI.view.auth.LoginForm', { renderTo: Ext.getBody() });
            }
        }
    ]
});