/**
 * The main application class. An instance of this class is created by app.js when it
 * calls Ext.application(). This is the ideal place to handle application launch and
 * initialization details.
 */
Ext.define('CreditManager.UI.Application', {
    extend: 'Ext.app.Application',

    name: 'CreditManager.UI',

    requires: [
        'CreditManager.UI.service.AuthService',
        'Ext.plugin.Viewport'
    ],

    quickTips: false,
    platformConfig: {
        desktop: {
            quickTips: true
        }
    },

    launch: function() {
        const token = localStorage.getItem('access_token');

        if (!token) {
            Ext.create('CreditManager.UI.view.auth.LoginForm', { renderTo: Ext.getBody() });
            return;
        }

        console.log('now main view should load');
        Ext.create('CreditManager.UI.view.main.Main', {
            renderTo: Ext.getBody()
        });
    },

    onAppUpdate: function () {
        Ext.Msg.confirm('Application Update', 'This application has an update, reload?',
            function (choice) {
                if (choice === 'yes') {
                    window.location.reload();
                }
            }
        );
    }
});
