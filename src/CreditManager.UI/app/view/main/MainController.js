/**
 * This class is the controller for the main view for the application. It is specified as
 * the "controller" of the Main view class.
 */
Ext.define('CreditManager.UI.view.main.MainController', {
    extend: 'Ext.app.ViewController',

    alias: 'controller.main',

    onItemSelected: function (sender, record) {
        Ext.Msg.confirm('Confirm', 'Are you sure?', 'onConfirm', this);
    },

    onConfirm: function (choice) {
        if (choice === 'yes') {
            //
        }
    },

    init: function() {
        var me = this;
        var userInfo = CreditManager.UI.service.AuthService.getUserInfo();
        
        var userInfoComponent = me.lookup('userInfo');
        var loginLink = me.lookup('loginLink');
        var logoutLink = me.lookup('logoutLink');
        
        if (userInfo) {
            console.log('User info in MainController:', userInfo);
            var displayName = userInfo.name || 
                            (userInfo.firstName && userInfo.lastName ? 
                            `${userInfo.firstName} ${userInfo.lastName}` : 
                            userInfo.email);
            
            userInfoComponent.setHtml(displayName);
            userInfoComponent.show();
            logoutLink.show();
            loginLink.hide();
        } else {
            console.log('No user info available, showing login link');
            userInfoComponent.hide();
            logoutLink.hide();
            loginLink.show();
        }
    },

    onLoginClick: function() {
        CreditManager.UI.service.AuthService.login();
    },

    onLogoutClick: function() {
        CreditManager.UI.service.AuthService.logout();
    }
});
