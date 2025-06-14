/**
 * The main application class. An instance of this class is created by app.js when it
 * calls Ext.application(). This is the ideal place to handle application launch and
 * initialization details.
 */
Ext.define('CreditManager.UI.Application', {
    extend: 'Ext.app.Application',

    name: 'CreditManager.UI',

    requires: [
        'CreditManager.UI.service.AuthService'
    ],

    quickTips: false,
    platformConfig: {
        desktop: {
            quickTips: true
        }
    },

    launch: function() {
        // Check if we have a stored auth code
        var authCode = localStorage.getItem('auth_code');
        if (authCode) {
            // Exchange the code for tokens
            CreditManager.UI.service.AuthService.exchangeCodeForToken(authCode);
            return;
        }

        // Check if user is authenticated
        if (!CreditManager.UI.service.AuthService.isAuthenticated()) {
            // Start the login process
            CreditManager.UI.service.AuthService.login();
            return;
        }

        // Show the main view
        Ext.create('CreditManager.UI.view.main.Main');
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
