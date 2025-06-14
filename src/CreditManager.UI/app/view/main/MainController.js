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
        
        if (userInfo) {
            console.log('User info in MainController:', userInfo);
            var displayName = userInfo.name || 
                            (userInfo.firstName && userInfo.lastName ? 
                            `${userInfo.firstName} ${userInfo.lastName}` : 
                            userInfo.email);
            
            var userInfoComponent = me.lookup('userInfo');
            if (userInfoComponent) {
                userInfoComponent.setHtml(displayName);
            } else {
                console.error('User info component not found');
            }
        } else {
            console.error('No user info available');
        }
    }
});
